using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_Net_Tool.Helpers;
using static Win_Net_Tool.Helpers.NetworkActions;

namespace Win_Net_Tool
{
    public partial class Form1 : Form
    {
        #region Constants

        private const string OutputSeparator =
            "\r\n#$# -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*- #$#\r\n";

        #endregion


        #region Windows Proxy API

        private const int HWND_BROADCAST =
            0xffff;

        private const uint WM_SETTINGCHANGE =
            0x001A;

        private const uint SMTO_ABORTIFHUNG =
            0x0002;

        private const int INTERNET_OPTION_REFRESH =
            37;

        private const int INTERNET_OPTION_SETTINGS_CHANGED =
            39;

        [DllImport(
            "user32.dll",
            CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint msg,
            UIntPtr wParam,
            string lParam,
            uint flags,
            uint timeout,
            out UIntPtr result);

        [DllImport(
            "wininet.dll",
            SetLastError = true)]
        private static extern bool InternetSetOption(
            IntPtr hInternet,
            int dwOption,
            IntPtr lpBuffer,
            int dwBufferLength);

        #endregion


        #region Constructor and Form Events

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(
            object sender,
            EventArgs e)
        {
            try
            {

                LoadDnsServers();

                InitializeDnsComboBox();

                CommandExecutionResult result =
                    await CommandExecutor.ExecuteAsync(
                        "whoami",
                        CommandShell.Cmd);

                ShowCommandResult(
                    "دستور اجراشده: whoami",
                    result);
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "اجرای دستور whoami",
                    ex);
            }
        }

        #endregion


        #region Proxy Settings

        /// <summary>
        /// اعلام تغییر تنظیمات Internet و Refresh کردن WinINet.
        /// </summary>
        private static void RefreshInternetSettings()
        {
            bool settingsChanged =
                InternetSetOption(
                    IntPtr.Zero,
                    INTERNET_OPTION_SETTINGS_CHANGED,
                    IntPtr.Zero,
                    0);

            bool settingsRefreshed =
                InternetSetOption(
                    IntPtr.Zero,
                    INTERNET_OPTION_REFRESH,
                    IntPtr.Zero,
                    0);

            SendMessageTimeout(
                new IntPtr(HWND_BROADCAST),
                WM_SETTINGCHANGE,
                UIntPtr.Zero,
                "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings",
                SMTO_ABORTIFHUNG,
                1000,
                out _);
        }

        /// <summary>
        /// حذف تنظیمات Proxy مربوط به کاربر فعلی.
        /// </summary>
        private static void DeleteCurrentUserProxySettings()
        {
            const string internetSettingsPath =
                @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

            using (RegistryKey internetSettings =
                Registry.CurrentUser.OpenSubKey(
                    internetSettingsPath,
                    writable: true))
            {
                if (internetSettings == null)
                {
                    throw new InvalidOperationException(
                        "کلید تنظیمات Proxy در Registry پیدا نشد.");
                }

                /*
                 * Automatically detect settings
                 */
                internetSettings.SetValue(
                    "AutoDetect",
                    0,
                    RegistryValueKind.DWord);

                /*
                 * Use a proxy server
                 */
                internetSettings.SetValue(
                    "ProxyEnable",
                    0,
                    RegistryValueKind.DWord);

                /*
                 * Use setup script
                 *
                 * فعال بودن AutoConfigURL باعث فعال‌شدن
                 * گزینه Use setup script می‌شود.
                 */
                internetSettings.DeleteValue(
                    "AutoConfigURL",
                    false);

                /*
                 * Proxy address and port
                 */
                internetSettings.DeleteValue(
                    "ProxyServer",
                    false);

                /*
                 * Proxy exceptions
                 */
                internetSettings.DeleteValue(
                    "ProxyOverride",
                    false);
            }

            RefreshInternetSettings();
        }

        /// <summary>
        /// حذف Proxy مربوط به WinHTTP.
        /// </summary>
        private static async Task<CommandExecutionResult>
            ResetWinHttpProxyAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "netsh winhttp reset proxy",
                CommandShell.Cmd);
        }

        #endregion


        #region Open Windows Settings

        /// <summary>
        /// بازکردن Internet Options معمولی.
        /// این قابلیت مستقل از حذف Proxy است.
        /// </summary>
        private static void OpenInternetOptions()
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "control.exe",
                    Arguments = "inetcpl.cpl",
                    UseShellExecute = true
                });
        }

        /// <summary>
        /// بازکردن Internet Options روی تب Connections.
        /// </summary>
        private static void OpenInternetOptionsConnectionsTab()
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "control.exe",
                    Arguments = "inetcpl.cpl,,4",
                    UseShellExecute = true
                });
        }

        /// <summary>
        /// بازکردن تنظیمات Proxy در Windows Settings.
        /// </summary>
        private static void OpenWindowsProxySettings()
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "ms-settings:network-proxy",
                    UseShellExecute = true
                });
        }

        #endregion


        #region Controls State

        private void SetControlsEnabled(
            bool enabled)
        {
            btnIpConfig.Enabled =
                enabled;

            btnFlushDns.Enabled =
                enabled;

            btnResetNetwork.Enabled =
                enabled;

            btnSetAllAdaptersDhcp.Enabled =
                enabled;

            btnInternetOptions.Enabled =
                enabled;

            btnRemoveSystemProxy.Enabled =
                enabled;

            btnApplyCustomDns.Enabled =
                enabled;

            btnApplySelectedDns.Enabled =
                enabled;

            btnPingAllDns.Enabled =
                enabled;
        }

        #endregion


        #region Logging and Output

        private void AppendLog(
            string message)
        {
            rtbOutput.AppendText(
                "[" +
                DateTime.Now.ToString("HH:mm:ss") +
                "] " +
                message +
                "\r\n");
        }

        private void AppendExceptionResult(
            string title,
            Exception ex)
        {
            rtbOutput.AppendText(
                title +
                "\r\n\r\n" +
                "خطای غیرمنتظره:\r\n" +
                ex +
                OutputSeparator);

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();

            lblStatus.Text =
                "خطای غیرمنتظره";

            lblStatus.ForeColor =
                Color.Red;
        }

        private void AppendCommandResult(
            string commandTitle,
            CommandExecutionResult result)
        {
            StringBuilder message =
                new StringBuilder();

            message.AppendLine(commandTitle);
            message.AppendLine();

            if (!string.IsNullOrWhiteSpace(
                result.Output))
            {
                message.AppendLine(
                    result.Output.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(
                result.Error))
            {
                if (!string.IsNullOrWhiteSpace(
                    result.Output))
                {
                    message.AppendLine();
                }

                message.AppendLine(
                    "خطای دستور:");

                message.AppendLine(
                    result.Error.TrimEnd());
            }

            if (string.IsNullOrWhiteSpace(
                result.Output) &&
                string.IsNullOrWhiteSpace(
                result.Error))
            {
                message.AppendLine(
                    "این دستور خروجی‌ای تولید نکرد.");
            }

            message.Append(
                OutputSeparator);

            rtbOutput.AppendText(
                message.ToString());

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();

            if (result.IsSuccess)
            {
                lblStatus.Text =
                    "دستور با موفقیت اجرا شد.";

                lblStatus.ForeColor =
                    Color.Green;
            }
            else
            {
                lblStatus.Text =
                    "اجرای دستور با خطا یا هشدار مواجه شد.";

                lblStatus.ForeColor =
                    Color.DarkOrange;
            }
        }

        private void ShowCommandResult(
            string commandTitle,
            CommandExecutionResult result)
        {
            AppendCommandResult(
                commandTitle,
                result);
        }

        private void AppendFinalResetStatus(
            bool allCommandsSucceeded)
        {
            rtbOutput.AppendText(
                "نتیجه نهایی عملیات Reset Network:\r\n");

            if (allCommandsSucceeded)
            {
                rtbOutput.AppendText(
                    "تمام دستورات با موفقیت اجرا شدند.\r\n");

                lblStatus.Text =
                    "ریست شبکه با موفقیت انجام شد. سیستم را Restart کنید.";

                lblStatus.ForeColor =
                    Color.Green;
            }
            else
            {
                rtbOutput.AppendText(
                    "برخی دستورات با خطا یا هشدار اجرا شدند.\r\n");

                lblStatus.Text =
                    "ریست شبکه کامل انجام نشد.";

                lblStatus.ForeColor =
                    Color.DarkOrange;
            }

            rtbOutput.AppendText(
                OutputSeparator);

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();
        }

        #endregion


        #region Internet Options Button

        /// <summary>
        /// این دکمه فقط Internet Options را باز می‌کند.
        /// هیچ تنظیم Proxy را تغییر نمی‌دهد.
        /// </summary>
        private void btnInternetOptions_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                OpenInternetOptions();

                AppendLog(
                    "Internet Options باز شد.");

                lblStatus.Text =
                    "Internet Options باز شد.";

                lblStatus.ForeColor =
                    Color.Green;
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "باز کردن Internet Options",
                    ex);
            }
        }

        #endregion


        #region Remove System Proxy Button

        /// <summary>
        /// حذف کامل Proxy کاربر فعلی و WinHTTP.
        /// </summary>
        private async void btnRemoveSystemProxy_Click(
            object sender,
            EventArgs e)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    "تمام تنظیمات Proxy کاربر فعلی و WinHTTP حذف خواهند شد.\r\n\r\n" +
                    "موارد زیر تغییر می‌کنند:\r\n" +
                    "- Automatically detect settings\r\n" +
                    "- Use setup script\r\n" +
                    "- Script address\r\n" +
                    "- Use a proxy server\r\n" +
                    "- Proxy address and port\r\n" +
                    "- Proxy exceptions\r\n\r\n" +
                    "آیا ادامه می‌دهید؟",
                    "حذف پروکسی سیستم",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            btnRemoveSystemProxy.Enabled =
                false;

            lblStatus.Text =
                "در حال حذف تنظیمات Proxy...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                AppendLog(
                    "شروع حذف تنظیمات Proxy کاربر فعلی.");

                DeleteCurrentUserProxySettings();

                AppendLog(
                    "تنظیمات Proxy مربوط به کاربر فعلی حذف شد.");

                CommandExecutionResult winHttpResult =
                    await ResetWinHttpProxyAsync();

                ShowCommandResult(
                    "دستور اجراشده: netsh winhttp reset proxy",
                    winHttpResult);

                rtbOutput.AppendText(
                    "نتیجه حذف پروکسی سیستم:\r\n" +
                    "- Automatically detect settings: خاموش شد.\r\n" +
                    "- Use setup script: خاموش و حذف شد.\r\n" +
                    "- Script address: حذف شد.\r\n" +
                    "- Use a proxy server: خاموش شد.\r\n" +
                    "- Proxy address and port: حذف شد.\r\n" +
                    "- Proxy exceptions: حذف شد.\r\n");

                if (winHttpResult.IsSuccess)
                {
                    rtbOutput.AppendText(
                        "- WinHTTP Proxy: حذف شد.\r\n");

                    AppendLog(
                        "WinHTTP Proxy با موفقیت Reset شد.");

                    lblStatus.Text =
                        "تمام تنظیمات Proxy با موفقیت حذف شدند.";

                    lblStatus.ForeColor =
                        Color.Green;
                }
                else
                {
                    rtbOutput.AppendText(
                        "- WinHTTP Proxy: با خطا مواجه شد.\r\n");

                    AppendLog(
                        "Reset کردن WinHTTP Proxy با خطا مواجه شد.");

                    lblStatus.Text =
                        "Proxy کاربر حذف شد؛ WinHTTP با هشدار مواجه شد.";

                    lblStatus.ForeColor =
                        Color.DarkOrange;
                }

                rtbOutput.AppendText(
                    OutputSeparator);

                rtbOutput.SelectionStart =
                    rtbOutput.TextLength;

                rtbOutput.ScrollToCaret();

                /*
                 * پس از اتمام عملیات، صفحه Proxy باز می‌شود
                 * تا نتیجه را مشاهده کنی.
                 */
                OpenWindowsProxySettings();
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "حذف پروکسی سیستم",
                    ex);
            }
            finally
            {
                btnRemoveSystemProxy.Enabled =
                    true;
            }
        }

        /*
         * اگر این Event در Designer قدیمی هنوز متصل باشد،
         * برای جلوگیری از خطای Designer این متد باقی می‌ماند.
         *
         * این دکمه فقط تب Connections را باز می‌کند
         * و دیگر عملیات حذف Proxy انجام نمی‌دهد.
         */
        private void btnDisableLanSettings_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                OpenInternetOptionsConnectionsTab();

                AppendLog(
                    "Internet Options روی تب Connections باز شد.");

                lblStatus.Text =
                    "Internet Options روی تب Connections باز شد.";

                lblStatus.ForeColor =
                    Color.Green;
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "باز کردن تب Connections",
                    ex);
            }
        }

        #endregion


        #region IP Configuration

        private async void btnIpConfig_Click(
            object sender,
            EventArgs e)
        {
            SetControlsEnabled(false);

            lblStatus.Text =
                "در حال دریافت اطلاعات IP...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                CommandExecutionResult result =
                    await NetworkActions.GetIpConfigAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig",
                    result);
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "اجرای دستور ipconfig",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        #endregion


        #region Flush DNS

        private async void btnFlushDns_Click(
            object sender,
            EventArgs e)
        {
            SetControlsEnabled(false);

            lblStatus.Text =
                "در حال پاک‌سازی DNS Cache...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                CommandExecutionResult result =
                    await NetworkActions.FlushDnsCacheAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig /flushdns",
                    result);
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "اجرای دستور ipconfig /flushdns",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        #endregion


        #region Clear Output

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            rtbOutput.Clear();

            lblStatus.Text =
                "خروجی پاک شد.";

            lblStatus.ForeColor =
                Color.Black;
        }

        #endregion


        #region Reset Network

        private async void btnResetNetwork_Click(
            object sender,
            EventArgs e)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    "با اجرای این عملیات ممکن است اتصال شبکه موقتاً قطع شود.\r\n" +
                    "آیا از ریست تنظیمات شبکه اطمینان دارید؟",
                    "تأیید ریست شبکه",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            SetControlsEnabled(false);

            lblStatus.Text =
                "در حال ریست تنظیمات شبکه...";

            lblStatus.ForeColor =
                Color.Black;

            bool allCommandsSucceeded =
                true;

            try
            {
                CommandExecutionResult winsockResult =
                    await NetworkActions.ResetWinsockAsync();

                ShowCommandResult(
                    "دستور اجراشده: netsh winsock reset",
                    winsockResult);

                if (!winsockResult.IsSuccess)
                {
                    allCommandsSucceeded =
                        false;
                }

                CommandExecutionResult ipResetResult =
                    await NetworkActions.ResetIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: netsh int ip reset",
                    ipResetResult);

                if (!ipResetResult.IsSuccess)
                {
                    allCommandsSucceeded =
                        false;
                }

                CommandExecutionResult releaseResult =
                    await NetworkActions.ReleaseIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig /release",
                    releaseResult);

                if (!releaseResult.IsSuccess)
                {
                    allCommandsSucceeded =
                        false;
                }

                CommandExecutionResult renewResult =
                    await NetworkActions.RenewIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig /renew",
                    renewResult);

                if (!renewResult.IsSuccess)
                {
                    allCommandsSucceeded =
                        false;
                }

                AppendFinalResetStatus(
                    allCommandsSucceeded);
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "عملیات Reset Network",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        #endregion


        #region DHCP

        private void AppendAdapterDhcpResult(
            NetworkActions.AdapterDhcpResult result)
        {
            StringBuilder message =
                new StringBuilder();

            message.AppendLine(
                "آداپتور: " +
                result.AdapterName);

            message.AppendLine();

            message.AppendLine(
                "تنظیم دریافت خودکار IP:");

            if (!string.IsNullOrWhiteSpace(
                result.AddressResult.Output))
            {
                message.AppendLine(
                    result.AddressResult.Output.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(
                result.AddressResult.Error))
            {
                message.AppendLine(
                    "خطا:");

                message.AppendLine(
                    result.AddressResult.Error.TrimEnd());
            }

            message.AppendLine();

            message.AppendLine(
                "تنظیم دریافت خودکار DNS:");

            if (!string.IsNullOrWhiteSpace(
                result.DnsResult.Output))
            {
                message.AppendLine(
                    result.DnsResult.Output.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(
                result.DnsResult.Error))
            {
                message.AppendLine(
                    "خطا:");

                message.AppendLine(
                    result.DnsResult.Error.TrimEnd());
            }

            message.AppendLine();

            if (result.IsSuccess)
            {
                message.AppendLine(
                    "نتیجه: تنظیمات IP و DNS این آداپتور روی DHCP قرار گرفت.");
            }
            else
            {
                message.AppendLine(
                    "نتیجه: تنظیم کامل این آداپتور با خطا یا هشدار مواجه شد.");
            }

            message.Append(
                OutputSeparator);

            rtbOutput.AppendText(
                message.ToString());

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();
        }

        private async void btnSetAllAdaptersDhcp_Click(
            object sender,
            EventArgs e)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    "تنظیمات IPv4 تمام آداپتورهای شبکه روی دریافت خودکار IP و DNS قرار می‌گیرد.\r\n\r\n" +
                    "ممکن است اتصال شبکه موقتاً قطع شود.\r\n" +
                    "آیا ادامه می‌دهید؟",
                    "تنظیم DHCP برای همه آداپتورها",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            SetControlsEnabled(false);

            lblStatus.Text =
                "در حال تنظیم خودکار IP و DNS تمام آداپتورها...";

            lblStatus.ForeColor =
                Color.Black;

            bool allAdaptersSucceeded =
                true;

            try
            {
                List<NetworkActions.AdapterDhcpResult> results =
                    await NetworkActions.SetAllAdaptersToDhcpAsync();

                if (results.Count == 0)
                {
                    rtbOutput.AppendText(
                        "هیچ آداپتور قابل تنظیمی پیدا نشد.\r\n" +
                        OutputSeparator);

                    lblStatus.Text =
                        "آداپتور قابل تنظیمی پیدا نشد.";

                    lblStatus.ForeColor =
                        Color.DarkOrange;

                    return;
                }

                foreach (
                    NetworkActions.AdapterDhcpResult result
                    in results)
                {
                    AppendAdapterDhcpResult(
                        result);

                    if (!result.IsSuccess)
                    {
                        allAdaptersSucceeded =
                            false;
                    }
                }

                rtbOutput.AppendText(
                    "نتیجه نهایی تنظیم DHCP:\r\n");

                if (allAdaptersSucceeded)
                {
                    rtbOutput.AppendText(
                        "تنظیم IP و DNS تمام آداپتورها با موفقیت روی حالت خودکار قرار گرفت.\r\n");

                    lblStatus.Text =
                        "IP و DNS تمام آداپتورها خودکار شدند.";

                    lblStatus.ForeColor =
                        Color.Green;
                }
                else
                {
                    rtbOutput.AppendText(
                        "برخی آداپتورها با خطا یا هشدار مواجه شدند.\r\n");

                    lblStatus.Text =
                        "تنظیم همه آداپتورها کامل انجام نشد.";

                    lblStatus.ForeColor =
                        Color.DarkOrange;
                }

                rtbOutput.AppendText(
                    OutputSeparator);

                rtbOutput.SelectionStart =
                    rtbOutput.TextLength;

                rtbOutput.ScrollToCaret();
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "تنظیم DHCP برای تمام آداپتورها",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        #endregion


        #region Ping Validation

        private bool IsValidIPv4Strict(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string[] parts =
                value.Split('.');

            if (parts.Length != 4)
            {
                return false;
            }

            foreach (string part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    return false;
                }

                if (!int.TryParse(
                    part,
                    out int number))
                {
                    return false;
                }

                if (number < 0 || number > 255)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidDomain(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length > 253)
            {
                return false;
            }

            string[] labels =
                value.Split('.');

            if (labels.Length < 2)
            {
                return false;
            }

            foreach (string label in labels)
            {
                if (string.IsNullOrWhiteSpace(label) ||
                    label.Length > 63)
                {
                    return false;
                }

                if (!char.IsLetterOrDigit(label[0]) ||
                    !char.IsLetterOrDigit(
                        label[label.Length - 1]))
                {
                    return false;
                }

                foreach (char character in label)
                {
                    if (!char.IsLetterOrDigit(character) &&
                        character != '-')
                    {
                        return false;
                    }
                }
            }

            string topLevelDomain =
                labels[labels.Length - 1];

            if (topLevelDomain.Length < 2)
            {
                return false;
            }

            foreach (char character in topLevelDomain)
            {
                if (!char.IsLetter(character) &&
                    character != '-')
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidPingTarget(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value =
                value.Trim();

            if (value.Contains(":"))
            {
                return false;
            }

            if (IsValidIPv4Strict(value))
            {
                return true;
            }

            return IsValidDomain(value);
        }

        #endregion


        #region Ping

        private async void btnPing_Click(
            object sender,
            EventArgs e)
        {
            string target =
                txtPingTarget.Text.Trim();

            if (!IsValidPingTarget(target))
            {
                MessageBox.Show(
                    "لطفاً یک IPv4 یا دامنه معتبر وارد کنید.\r\n\r\n" +
                    "نمونه‌های معتبر:\r\n" +
                    "8.8.8.8\r\n" +
                    "192.168.1.1\r\n" +
                    "127.0.0.1\r\n" +
                    "google.com\r\n" +
                    "mramoori.ir",
                    "مقصد نامعتبر",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPingTarget.Focus();
                txtPingTarget.SelectAll();

                return;
            }

            btnPing.Enabled =
                false;

            txtPingTarget.Enabled =
                false;

            lblPingStatus.Text =
                "در حال ارسال Ping به " +
                target +
                "...";

            lblPingStatus.ForeColor =
                Color.Black;

            StringBuilder output =
                new StringBuilder();

            output.AppendLine(
                "دستور اجراشده: ping " +
                target);

            output.AppendLine();

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply =
                        await ping.SendPingAsync(
                            target,
                            4000);

                    if (reply.Status ==
                        IPStatus.Success)
                    {
                        output.AppendLine(
                            "پاسخ دریافت شد.");

                        output.AppendLine(
                            "مقصد واردشده: " +
                            target);

                        if (reply.Address != null)
                        {
                            output.AppendLine(
                                "آدرس Resolve شده: " +
                                reply.Address);
                        }

                        output.AppendLine(
                            "زمان پاسخ: " +
                            reply.RoundtripTime +
                            " ms");

                        output.AppendLine(
                            "وضعیت: اتصال مقصد برقرار است.");

                        lblPingStatus.Text =
                            "اتصال برقرار است - " +
                            reply.RoundtripTime +
                            " ms";

                        lblPingStatus.ForeColor =
                            Color.Green;
                    }
                    else
                    {
                        output.AppendLine(
                            "پاسخی از مقصد دریافت نشد.");

                        output.AppendLine(
                            "مقصد: " +
                            target);

                        output.AppendLine(
                            "وضعیت Ping: " +
                            reply.Status);

                        output.AppendLine(
                            "ممکن است مقصد یا فایروال پاسخ ICMP را مسدود کرده باشد.");

                        lblPingStatus.Text =
                            "پاسخی دریافت نشد: " +
                            reply.Status;

                        lblPingStatus.ForeColor =
                            Color.DarkOrange;
                    }
                }
            }
            catch (PingException ex)
            {
                output.AppendLine(
                    "خطا در اجرای Ping:");

                output.AppendLine(
                    ex.Message);

                lblPingStatus.Text =
                    "خطا در اجرای Ping";

                lblPingStatus.ForeColor =
                    Color.Red;
            }
            catch (SocketException ex)
            {
                output.AppendLine(
                    "خطا در Resolve کردن دامنه:");

                output.AppendLine(
                    ex.Message);

                lblPingStatus.Text =
                    "دامنه پیدا نشد یا DNS مشکل دارد.";

                lblPingStatus.ForeColor =
                    Color.Red;
            }
            catch (Exception ex)
            {
                output.AppendLine(
                    "خطای غیرمنتظره:");

                output.AppendLine(
                    ex.Message);

                lblPingStatus.Text =
                    "خطای غیرمنتظره";

                lblPingStatus.ForeColor =
                    Color.Red;
            }
            finally
            {
                output.AppendLine();
                output.Append(
                    OutputSeparator);

                rtbOutput.AppendText(
                    output.ToString());

                rtbOutput.SelectionStart =
                    rtbOutput.TextLength;

                rtbOutput.ScrollToCaret();

                btnPing.Enabled =
                    true;

                txtPingTarget.Enabled =
                    true;
            }
        }

        #endregion


        #region DNS Server List

        private List<NetworkActions.DnsServerInfo>
            dnsServers =
                new List<NetworkActions.DnsServerInfo>();

        private void LoadDnsServers()
        {
            dnsServers =
                new List<NetworkActions.DnsServerInfo>
                {
            new NetworkActions.DnsServerInfo
            {
                Name = "شکن",
                PrimaryAddress = "178.22.122.100",
                SecondaryAddress = "185.51.200.2",
                Purpose = "رفع تحریم، دانلود",
                Stability = "بالا",
                Source = "shecan.ir"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "شلتر",
                PrimaryAddress = "94.103.125.157",
                SecondaryAddress = "94.103.125.158",
                Purpose = "گیمینگ، کاهش پینگ",
                Stability = "متوسط تا بالا",
                Source = "sheltertm.com"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "بشکن",
                PrimaryAddress = "181.41.194.177",
                SecondaryAddress = "181.41.194.186",
                Purpose = "رفع تحریم عمومی",
                Stability = "متوسط",
                Source = "beshkanapp.ir"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "الکترو",
                PrimaryAddress = "78.157.42.100",
                SecondaryAddress = "78.157.42.101",
                Purpose = "بازی آنلاین، سرعت",
                Stability = "بالا",
                Source = "electrotm.org"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "رادار",
                PrimaryAddress = "10.202.10.10",
                SecondaryAddress = "10.202.10.11",
                Purpose = "گیمینگ، دانلود",
                Stability = "بالا",
                Source = "radar.game"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "403",
                PrimaryAddress = "10.202.10.202",
                SecondaryAddress = "10.202.10.102",
                Purpose = "رسمی دولتی، رفع تحریم",
                Stability = "متوسط",
                Source = "403.online"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "بگذر",
                PrimaryAddress = "185.55.226.26",
                SecondaryAddress = "185.55.225.25",
                Purpose = "دسترسی بدون محدودیت",
                Stability = "بالا",
                Source = "begzar.ir"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "پیشگامان",
                PrimaryAddress = "5.202.100.100",
                SecondaryAddress = "5.202.100.101",
                Purpose = "دسترسی سریع داخلی",
                Stability = "متوسط",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "شاتل",
                PrimaryAddress = "85.15.1.14",
                SecondaryAddress = "85.15.1.15",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "آسیاتک",
                PrimaryAddress = "194.36.174.161",
                SecondaryAddress = "178.22.122.100",
                Purpose = "دسترسی سریع داخلی",
                Stability = "بالا",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "مخابرات",
                PrimaryAddress = "217.218.155.155",
                SecondaryAddress = "217.218.127.127",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "پارس آنلاین",
                PrimaryAddress = "91.99.101.12",
                SecondaryAddress = "",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "ایرانسل",
                PrimaryAddress = "74.82.42.42",
                SecondaryAddress = "",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "Irancell"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "داده گستر عصر نوین",
                PrimaryAddress = "46.224.1.42",
                SecondaryAddress = "",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "ISPs in Iran"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "دانشگاه تهران علوم پزشکی",
                PrimaryAddress = "194.225.62.80",
                SecondaryAddress = "",
                Purpose = "استفاده داخلی ISP",
                Stability = "متوسط",
                Source = "دانشگاه تهران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "لگ اسلیر",
                PrimaryAddress = "5.160.132.221",
                SecondaryAddress = "5.160.132.222",
                Purpose = "گیمینگ، کاهش لگ",
                Stability = "متوسط",
                Source = "گزارش‌های کاربران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "لگ زیرو",
                PrimaryAddress = "95.38.132.153",
                SecondaryAddress = "95.38.132.152",
                Purpose = "گیمینگ، کاهش لگ",
                Stability = "متوسط",
                Source = "گزارش‌های کاربران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "dns.malw.link",
                PrimaryAddress = "84.21.189.133",
                SecondaryAddress = "64.188.98.242",
                Purpose = "جلوگیری از بدافزار",
                Stability = "بالا",
                Source = "گزارش‌های کاربران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "هاست ایران",
                PrimaryAddress = "172.29.0.100",
                SecondaryAddress = "172.29.2.100",
                Purpose = "دسترسی سریع داخلی",
                Stability = "متوسط",
                Source = "گزارش‌های کاربران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "رسانه پرداز سپاهان",
                PrimaryAddress = "185.186.242.161",
                SecondaryAddress = "",
                Purpose = "دسترسی داخلی سریع",
                Stability = "متوسط",
                Source = "گزارش‌های کاربران"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "US - Google Public DNS",
                PrimaryAddress = "8.8.8.8",
                SecondaryAddress = "8.8.4.4",
                Purpose = "دسترسی جهانی سریع",
                Stability = "بالا",
                Source = "Google DNS"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "RU - Yandex",
                PrimaryAddress = "77.88.8.1",
                SecondaryAddress = "77.88.8.8",
                Purpose = "دسترسی امن جهانی",
                Stability = "متوسط",
                Source = "Yandex DNS"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "AU - Cloudflare",
                PrimaryAddress = "1.1.1.1",
                SecondaryAddress = "1.0.0.1",
                Purpose = "امنیت و سرعت بالا",
                Stability = "بالا",
                Source = "Cloudflare"
            },

            new NetworkActions.DnsServerInfo
            {
                Name = "US - Dyn",
                PrimaryAddress = "216.146.35.35",
                SecondaryAddress = "216.146.36.36",
                Purpose = "دسترسی سریع جهانی",
                Stability = "متوسط",
                Source = "Dyn DNS"
            }
                };
        }

        #region DNS ComboBox Initialization

        private void InitializeDnsComboBox()
        {
            ConfigureDnsComboBoxDrawing();

            cmbDnsServers.Items.Clear();

            foreach (
                NetworkActions.DnsServerInfo dns
                in dnsServers)
            {
                cmbDnsServers.Items.Add(
                    dns);
            }

            if (cmbDnsServers.Items.Count > 0)
            {
                cmbDnsServers.SelectedIndex =
                    0;
            }
        }

        #endregion

        #endregion


        #region Ping All DNS Servers

        private void AppendDnsPingLog(
            List<NetworkActions.DnsServerInfo> results)
        {
            StringBuilder log =
                new StringBuilder();

            log.AppendLine(
                "نتیجه Ping و مرتب‌سازی DNS Serverها:");

            log.AppendLine();

            int index =
                1;

            foreach (
                NetworkActions.DnsServerInfo dns
                in results)
            {
                log.AppendLine(
                    index +
                    ". " +
                    dns.ToString());

                log.AppendLine(
                    "   امتیاز مرتب‌سازی: " +
                    GetDnsSortPingText(dns));

                index++;
            }

            log.Append(
                OutputSeparator);

            rtbOutput.AppendText(
                log.ToString());

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();
        }

        private string GetDnsSortPingText(
            NetworkActions.DnsServerInfo dns)
        {
            if (double.IsPositiveInfinity(
                dns.SortPingMilliseconds) ||
                dns.SortPingMilliseconds ==
                double.MaxValue)
            {
                return "بدون پاسخ";
            }

            return dns.SortPingMilliseconds
                .ToString("0.00");
        }

        #region Ping All DNS Servers

        private async void btnPingAllDns_Click(
            object sender,
            EventArgs e)
        {
            if (dnsServers == null ||
                dnsServers.Count == 0)
            {
                MessageBox.Show(
                    "هیچ DNS Serverای برای تست وجود ندارد.",
                    "لیست DNS خالی است",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            btnPingAllDns.Enabled =
                false;

            btnApplyCustomDns.Enabled =
                false;

            btnApplySelectedDns.Enabled =
                false;

            cmbDnsServers.Enabled =
                false;

            lblStatus.Text =
                "در حال تست Ping تمام DNSها...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                AppendLog(
                    "شروع تست Ping تمام DNS Serverها.");

                /*
                 * برای هر DNS، Ping اولیه و ثانویه ارسال می‌شود.
                 * سپس نتیجه بر اساس کمترین میانگین Ping مرتب می‌شود.
                 */
                List<NetworkActions.DnsServerInfo>
                    sortedDnsServers =
                    await NetworkActions.PingAllDnsServersAsync(
                        dnsServers,
                        3000);

                /*
                 * لیست اصلی نیز با لیست مرتب‌شده جایگزین می‌شود
                 * تا اعمال DNS انتخاب‌شده از ترتیب جدید استفاده کند.
                 */
                dnsServers =
                    sortedDnsServers;

                /*
                 * بازسازی کامل ComboBox باعث می‌شود:
                 *
                 * - Pingهای جدید نمایش داده شوند.
                 * - ترتیب آیتم‌ها تغییر کند.
                 * - رنگ‌ها براساس رتبه جدید محاسبه شوند.
                 */
                cmbDnsServers.BeginUpdate();

                try
                {
                    cmbDnsServers.Items.Clear();

                    foreach (
                        NetworkActions.DnsServerInfo dns
                        in dnsServers)
                    {
                        cmbDnsServers.Items.Add(
                            dns);
                    }

                    if (cmbDnsServers.Items.Count > 0)
                    {
                        cmbDnsServers.SelectedIndex =
                            0;
                    }
                }
                finally
                {
                    cmbDnsServers.EndUpdate();
                }

                /*
                 * درخواست بازطراحی ComboBox.
                 * این خط باعث اعمال رنگ‌بندی براساس ترتیب جدید می‌شود.
                 */
                cmbDnsServers.Invalidate();

                AppendDnsPingLog(
                    sortedDnsServers);

                lblStatus.Text =
                    "DNSها براساس کمترین Ping مرتب شدند.";

                lblStatus.ForeColor =
                    Color.Green;

                AppendLog(
                    "Pingها، رنگ‌ها و ترتیب ComboBox با موفقیت بروزرسانی شدند.");
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "تست Ping تمام DNS Serverها",
                    ex);
            }
            finally
            {
                btnPingAllDns.Enabled =
                    true;

                btnApplyCustomDns.Enabled =
                    true;

                btnApplySelectedDns.Enabled =
                    true;

                cmbDnsServers.Enabled =
                    true;
            }
        }

        #endregion

        #endregion


        #region DNS Validation

        private bool IsValidIPv4Address(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value =
                value.Trim();

            if (!IPAddress.TryParse(
                value,
                out IPAddress address))
            {
                return false;
            }

            return address.AddressFamily ==
                   System.Net.Sockets.AddressFamily.InterNetwork;
        }

        #endregion


        #region DNS Logging

        private void AppendDnsApplyResult(
            List<NetworkActions.AdapterDnsResult> results,
            string primaryDns,
            string secondaryDns)
        {
            StringBuilder message =
                new StringBuilder();

            message.AppendLine(
                "نتیجه اعمال DNS روی تمام آداپتورها:");

            message.AppendLine(
                "DNS اولیه: " +
                primaryDns);

            message.AppendLine(
                "DNS ثانویه: " +
                (string.IsNullOrWhiteSpace(secondaryDns)
                    ? "تنظیم نشده"
                    : secondaryDns));

            message.AppendLine();

            bool allSucceeded =
                true;

            foreach (
                NetworkActions.AdapterDnsResult result
                in results)
            {
                message.AppendLine(
                    "آداپتور: " +
                    result.AdapterName);

                message.AppendLine(
                    "وضعیت DNS اولیه: " +
                    GetCommandStatus(
                        result.PrimaryResult));

                if (result.SecondaryResult != null)
                {
                    message.AppendLine(
                        "وضعیت DNS ثانویه: " +
                        GetCommandStatus(
                            result.SecondaryResult));
                }
                else
                {
                    message.AppendLine(
                        "وضعیت DNS ثانویه: تنظیم نشد.");
                }

                if (!result.IsSuccess)
                {
                    allSucceeded =
                        false;
                }

                message.AppendLine();
            }

            message.AppendLine(
                allSucceeded
                    ? "نتیجه نهایی: DNS روی تمام آداپتورها با موفقیت اعمال شد."
                    : "نتیجه نهایی: برخی آداپتورها با خطا یا هشدار مواجه شدند.");

            message.Append(
                OutputSeparator);

            rtbOutput.AppendText(
                message.ToString());

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();

            lblStatus.Text =
                allSucceeded
                    ? "DNS روی تمام آداپتورها اعمال شد."
                    : "اعمال DNS کامل انجام نشد.";

            lblStatus.ForeColor =
                allSucceeded
                    ? Color.Green
                    : Color.DarkOrange;
        }

        private string GetCommandStatus(
            CommandExecutionResult result)
        {
            if (result == null)
            {
                return "اجرا نشد";
            }

            if (result.IsSuccess)
            {
                return "موفق";
            }

            if (!string.IsNullOrWhiteSpace(
                result.Error))
            {
                return "خطا: " +
                       result.Error.Trim();
            }

            return "ناموفق";
        }


        #endregion


        #region Apply Custom DNS

        private async void btnApplyCustomDns_Click(
            object sender,
            EventArgs e)
        {
            string primaryDns =
                txtPrimaryDns.Text.Trim();

            string secondaryDns =
                txtSecondaryDns.Text.Trim();

            if (!IsValidIPv4Address(primaryDns))
            {
                MessageBox.Show(
                    "DNS اولیه معتبر نیست.\r\n\r\n" +
                    "نمونه صحیح:\r\n" +
                    "8.8.8.8",
                    "DNS اولیه نامعتبر",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPrimaryDns.Focus();
                txtPrimaryDns.SelectAll();

                return;
            }

            if (!string.IsNullOrWhiteSpace(
                secondaryDns) &&
                !IsValidIPv4Address(secondaryDns))
            {
                MessageBox.Show(
                    "DNS ثانویه معتبر نیست.\r\n\r\n" +
                    "نمونه صحیح:\r\n" +
                    "8.8.4.4",
                    "DNS ثانویه نامعتبر",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSecondaryDns.Focus();
                txtSecondaryDns.SelectAll();

                return;
            }

            DialogResult confirmation =
                MessageBox.Show(
                    "DNS زیر روی تمام آداپتورهای IPv4 اعمال می‌شود:\r\n\r\n" +
                    "DNS اولیه: " +
                    primaryDns +
                    "\r\n" +
                    "DNS ثانویه: " +
                    (string.IsNullOrWhiteSpace(secondaryDns)
                        ? "تنظیم نشده"
                        : secondaryDns) +
                    "\r\n\r\n" +
                    "آیا ادامه می‌دهید؟",
                    "تأیید اعمال DNS",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            btnApplyCustomDns.Enabled =
                false;

            btnApplySelectedDns.Enabled =
                false;

            lblStatus.Text =
                "در حال اعمال DNS روی تمام آداپتورها...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                AppendLog(
                    "شروع اعمال DNS دستی روی تمام آداپتورها.");

                List<NetworkActions.AdapterDnsResult> results =
                    await NetworkActions.SetDnsForAllAdaptersAsync(
                        primaryDns,
                        secondaryDns);

                AppendDnsApplyResult(
                    results,
                    primaryDns,
                    secondaryDns);

                AppendLog(
                    "عملیات اعمال DNS دستی پایان یافت.");
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "اعمال DNS دستی",
                    ex);
            }
            finally
            {
                btnApplyCustomDns.Enabled =
                    true;

                btnApplySelectedDns.Enabled =
                    true;
            }
        }


        #endregion


        #region Apply Selected DNS

        private async void btnApplySelectedDns_Click(
            object sender,
            EventArgs e)
        {
            if (cmbDnsServers.SelectedItem == null)
            {
                MessageBox.Show(
                    "لطفاً ابتدا یک DNS Server را انتخاب کنید.",
                    "انتخاب DNS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            NetworkActions.DnsServerInfo selectedDns =
                cmbDnsServers.SelectedItem
                as NetworkActions.DnsServerInfo;

            if (selectedDns == null)
            {
                MessageBox.Show(
                    "اطلاعات DNS انتخاب‌شده معتبر نیست.",
                    "خطای DNS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult confirmation =
                MessageBox.Show(
                    "DNS انتخاب‌شده روی تمام آداپتورهای IPv4 اعمال می‌شود:\r\n\r\n" +
                    selectedDns.Name +
                    "\r\n" +
                    "DNS اولیه: " +
                    selectedDns.PrimaryAddress +
                    "\r\n" +
                    "DNS ثانویه: " +
                    (string.IsNullOrWhiteSpace(
                        selectedDns.SecondaryAddress)
                        ? "تنظیم نشده"
                        : selectedDns.SecondaryAddress) +
                    "\r\n\r\n" +
                    "آیا ادامه می‌دهید؟",
                    "تأیید اعمال DNS",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            btnApplyCustomDns.Enabled =
                false;

            btnApplySelectedDns.Enabled =
                false;

            lblStatus.Text =
                "در حال اعمال DNS انتخاب‌شده...";

            lblStatus.ForeColor =
                Color.Black;

            try
            {
                AppendLog(
                    "شروع اعمال DNS انتخاب‌شده: " +
                    selectedDns.Name);

                List<NetworkActions.AdapterDnsResult> results =
                    await NetworkActions.SetDnsForAllAdaptersAsync(
                        selectedDns.PrimaryAddress,
                        selectedDns.SecondaryAddress);

                AppendDnsApplyResult(
                    results,
                    selectedDns.PrimaryAddress,
                    selectedDns.SecondaryAddress);

                AppendLog(
                    "DNS انتخاب‌شده روی آداپتورها اعمال شد.");
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "اعمال DNS انتخاب‌شده",
                    ex);
            }
            finally
            {
                btnApplyCustomDns.Enabled =
                    true;

                btnApplySelectedDns.Enabled =
                    true;
            }
        }

        #endregion


        #region DNS ComboBox Color Management

        private enum DnsPingColorGroup
        {
            Green,
            Yellow,
            Red
        }

        private DnsPingColorGroup
            GetDnsPingColorGroup(
                int index)
        {
            int itemCount =
                cmbDnsServers.Items.Count;

            if (itemCount <= 0)
            {
                return DnsPingColorGroup.Red;
            }

            int greenCount =
                (int)Math.Ceiling(
                    itemCount / 3.0);

            int yellowEndIndex =
                (int)Math.Ceiling(
                    itemCount * 2.0 / 3.0);

            if (index < greenCount)
            {
                return DnsPingColorGroup.Green;
            }

            if (index < yellowEndIndex)
            {
                return DnsPingColorGroup.Yellow;
            }

            return DnsPingColorGroup.Red;
        }

        #region DNS ComboBox Drawing

        private void cmbDnsServers_DrawItem(
            object sender,
            DrawItemEventArgs e)
        {
            if (e.Index < 0 ||
                e.Index >= cmbDnsServers.Items.Count)
            {
                return;
            }

            NetworkActions.DnsServerInfo dnsServer =
                cmbDnsServers.Items[e.Index]
                as NetworkActions.DnsServerInfo;

            if (dnsServer == null)
            {
                return;
            }

            DnsPingColorGroup colorGroup =
                GetDnsPingColorGroup(
                    e.Index);

            Color backgroundColor;

            switch (colorGroup)
            {
                case DnsPingColorGroup.Green:

                    backgroundColor =
                        Color.FromArgb(
                            198,
                            239,
                            206);

                    break;

                case DnsPingColorGroup.Yellow:

                    backgroundColor =
                        Color.FromArgb(
                            255,
                            235,
                            156);

                    break;

                default:

                    backgroundColor =
                        Color.FromArgb(
                            255,
                            199,
                            206);

                    break;
            }

            /*
             * هنگام انتخاب آیتم، فقط پس‌زمینه کمی تیره‌تر می‌شود.
             * رنگ متن در همه حالت‌ها مشکی باقی می‌ماند.
             */
            if ((e.State & DrawItemState.Selected) ==
                DrawItemState.Selected)
            {
                backgroundColor =
                    ControlPaint.Dark(
                        backgroundColor,
                        0.08f);
            }

            using (SolidBrush backgroundBrush =
                new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(
                    backgroundBrush,
                    e.Bounds);
            }

            using (SolidBrush textBrush =
                new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(
                    dnsServer.ToString(),
                    e.Font,
                    textBrush,
                    e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        #endregion

        private void ConfigureDnsComboBoxDrawing()
        {
            cmbDnsServers.DrawMode =
                DrawMode.OwnerDrawFixed;

            cmbDnsServers.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbDnsServers.DrawItem -=
                cmbDnsServers_DrawItem;

            cmbDnsServers.DrawItem +=
                cmbDnsServers_DrawItem;
        }

        #endregion


    }
}