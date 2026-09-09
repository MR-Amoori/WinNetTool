using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_Net_Tool.Helpers;
using static Win_Net_Tool.Helpers.NetworkActions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Win_Net_Tool
{
    public partial class Form1 : Form
    {
        #region Internet Options Off

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(
            IntPtr hInternet,
            int dwOption,
            IntPtr lpBuffer,
            int dwBufferLength);

        private const int INTERNET_OPTION_REFRESH = 37;
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;

        private static void RefreshInternetSettings()
        {
            InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION_SETTINGS_CHANGED,
                IntPtr.Zero,
                0);

            InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION_REFRESH,
                IntPtr.Zero,
                0);
        }

        private static void DisableLanProxySettings()
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
                        "مسیر تنظیمات Internet Options در Registry پیدا نشد.");
                }

                // خاموش‌کردن Automatically detect settings
                internetSettings.SetValue(
                    "AutoDetect",
                    0,
                    RegistryValueKind.DWord);

                // خاموش‌کردن Use a proxy server for your LAN
                internetSettings.SetValue(
                    "ProxyEnable",
                    0,
                    RegistryValueKind.DWord);

                // خاموش‌کردن Use automatic configuration script
                internetSettings.DeleteValue(
                    "AutoConfigURL",
                    false);
            }

            RefreshInternetSettings();
        }

        private static void OpenInternetOptionsConnectionsTab()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "control.exe",
                Arguments = "inetcpl.cpl,,4",
                UseShellExecute = true
            });
        }

        private void btnDisableLanSettings_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                DisableLanProxySettings();

                OpenInternetOptionsConnectionsTab();

                rtbOutput.AppendText(
                    "تنظیمات LAN با موفقیت غیرفعال شدند.\r\n" +
                    "- Automatically detect settings: خاموش\r\n" +
                    "- Use automatic configuration script: خاموش\r\n" +
                    "- Use a proxy server for your LAN: خاموش\r\n" +
                    "پنجره Internet Options روی تب Connections باز شد.\r\n" +
                    OutputSeparator);

                rtbOutput.SelectionStart = rtbOutput.TextLength;
                rtbOutput.ScrollToCaret();

                lblStatus.Text =
                    "تنظیمات LAN غیرفعال شدند.";

                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "غیرفعال‌کردن تنظیمات LAN",
                    ex);
            }
        }

        #endregion

        private const string OutputSeparator =
            "\r\n#$# -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*- #$#\r\n";

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
                    "دستور اجراشده: whoami",
                    ex);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnIpConfig.Enabled = enabled;
            btnFlushDns.Enabled = enabled;
            btnResetNetwork.Enabled = enabled;
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

                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                rtbOutput.AppendText(
                    "برخی دستورات با خطا یا هشدار اجرا شدند.\r\n");

                lblStatus.Text =
                    "ریست شبکه کامل انجام نشد؛ جزئیات خطا در خروجی نمایش داده شده است.";

                lblStatus.ForeColor = Color.DarkOrange;
            }

            rtbOutput.AppendText(OutputSeparator);

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();
        }

        private void AppendExceptionResult(
            string commandTitle,
            Exception ex)
        {
            rtbOutput.AppendText(
                commandTitle +
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

            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                message.AppendLine(
                    result.Output.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                if (!string.IsNullOrWhiteSpace(result.Output))
                {
                    message.AppendLine();
                }

                message.AppendLine("خطای دستور:");

                message.AppendLine(
                    result.Error.TrimEnd());
            }

            if (string.IsNullOrWhiteSpace(result.Output) &&
                string.IsNullOrWhiteSpace(result.Error))
            {
                message.AppendLine(
                    "این دستور خروجی‌ای تولید نکرد.");
            }

            message.Append(OutputSeparator);

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
                    Color.Red;
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

        #region ShowCommandResult

        /*
        private void ShowCommandResult(
            CommandExecutionResult result)
        {
            if (result.IsSuccess)
            {
                rtbOutput.Text =
                    "خروجی دستور:\n\n" +
                    result.Output;

                lblStatus.Text =
                    "دستور با موفقیت اجرا شد.";

                lblStatus.ForeColor =
                    Color.Green;
            }
            else
            {
                rtbOutput.Text =
                    "خروجی استاندارد:\n" +
                    (string.IsNullOrWhiteSpace(result.Output)
                        ? "(بدون خروجی)"
                        : result.Output) +
                    "\n\nخطای دستور:\n" +
                    (string.IsNullOrWhiteSpace(result.Error)
                        ? "(بدون پیام خطا)"
                        : result.Error);

                lblStatus.Text =
                    "اجرای دستور با خطا مواجه شد.";

                lblStatus.ForeColor =
                    Color.Red;
            }
        }
        */

        #endregion

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
                    "دستور اجراشده: ipconfig",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

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
                    "دستور اجراشده: ipconfig /flushdns",
                    ex);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

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

        private async void btnResetNetwork_Click(
            object sender,
            EventArgs e)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    "با اجرای این عملیات ممکن است اتصال شبکه موقتاً قطع شود.\n" +
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

        private void btnInternetOptions_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "control.exe",
                    Arguments = "inetcpl.cpl",
                    UseShellExecute = true
                });

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

        private void AppendAdapterDhcpResult(
    AdapterDhcpResult result)
        {
            StringBuilder message =
                new StringBuilder();

            message.AppendLine(
                "آداپتور: " + result.AdapterName);

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

            message.Append(OutputSeparator);

            rtbOutput.AppendText(
                message.ToString());

            rtbOutput.SelectionStart =
                rtbOutput.TextLength;

            rtbOutput.ScrollToCaret();
        }

        private async void btnSetAllAdaptersDhcp_Click(object sender, EventArgs e)
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
                List<AdapterDhcpResult> results =
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

                foreach (AdapterDhcpResult result in results)
                {
                    AppendAdapterDhcpResult(result);

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
                        "برخی آداپتورها با خطا یا هشدار مواجه شدند. جزئیات در خروجی نمایش داده شده است.\r\n");

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

        private bool IsValidIPv4Strict(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string[] parts =
                value.Split('.');

            // IPv4 باید دقیقاً چهار بخش داشته باشد
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

                // فقط عدد قبول می‌شود
                if (!int.TryParse(
                    part,
                    out int number))
                {
                    return false;
                }

                // هر بخش باید بین 0 تا 255 باشد
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

            // دامنه باید طول منطقی داشته باشد
            if (value.Length > 253)
            {
                return false;
            }

            // دامنه باید حداقل یک نقطه داشته باشد.
            // بنابراین abc پذیرفته نمی‌شود.
            string[] labels =
                value.Split('.');

            if (labels.Length < 2)
            {
                return false;
            }

            foreach (string label in labels)
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    return false;
                }

                if (label.Length > 63)
                {
                    return false;
                }

                // هر بخش باید با حرف یا عدد شروع و تمام شود
                if (!char.IsLetterOrDigit(label[0]) ||
                    !char.IsLetterOrDigit(
                        label[label.Length - 1]))
                {
                    return false;
                }

                // فقط حروف، اعداد و خط تیره مجاز هستند
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

            // پسوند دامنه حداقل دو حرف داشته باشد
            // مانند com، ir، org
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

            value = value.Trim();

            // جلوگیری از پذیرش IPv6 مانند ::
            if (value.Contains(":"))
            {
                return false;
            }

            // اگر IPv4 معتبر باشد، قبول شود
            if (IsValidIPv4Strict(value))
            {
                return true;
            }

            // در غیر این صورت، به‌عنوان دامنه بررسی شود
            return IsValidDomain(value);
        }


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

            btnPing.Enabled = false;
            txtPingTarget.Enabled = false;

            lblPingStatus.Text =
                "در حال ارسال Ping به " + target + "...";

            lblPingStatus.ForeColor =
                Color.Black;

            StringBuilder output =
                new StringBuilder();

            output.AppendLine(
                "دستور اجراشده: ping " + target);

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
                output.Append(OutputSeparator);
                output.AppendLine();

                rtbOutput.AppendText(
                    output.ToString());

                rtbOutput.SelectionStart =
                    rtbOutput.TextLength;

                rtbOutput.ScrollToCaret();

                btnPing.Enabled = true;
                txtPingTarget.Enabled = true;
            }
        }



    }
}