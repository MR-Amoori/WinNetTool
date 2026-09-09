using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_Net_Tool.Helpers;

namespace Win_Net_Tool
{
    public partial class Form1 : Form
    {
        private const string OutputSeparator =
    "\r\n#$# -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*- #$#\r\n";
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
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

        private void AppendFinalResetStatus(bool allCommandsSucceeded)
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
            rtbOutput.SelectionStart = rtbOutput.TextLength;
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

            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();

            lblStatus.Text = "خطای غیرمنتظره";
            lblStatus.ForeColor = Color.Red;
        }
        private void AppendCommandResult(
    string commandTitle,
    CommandExecutionResult result)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine(commandTitle);
            message.AppendLine();

            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                message.AppendLine(result.Output.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                if (!string.IsNullOrWhiteSpace(result.Output))
                {
                    message.AppendLine();
                }

                message.AppendLine("خطای دستور:");
                message.AppendLine(result.Error.TrimEnd());
            }

            if (string.IsNullOrWhiteSpace(result.Output) &&
                string.IsNullOrWhiteSpace(result.Error))
            {
                message.AppendLine("این دستور خروجی‌ای تولید نکرد.");
            }

            message.Append(OutputSeparator);

            rtbOutput.AppendText(message.ToString());
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();

            if (result.IsSuccess)
            {
                lblStatus.Text = "دستور با موفقیت اجرا شد.";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "اجرای دستور با خطا یا هشدار مواجه شد.";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void ShowCommandResult(
            string commandTitle,
            CommandExecutionResult result)
        {
            AppendCommandResult(commandTitle, result);
        }

        #region ShowCommandResult
        //private void ShowCommandResult(CommandExecutionResult result)
        //{
        //    if (result.IsSuccess)
        //    {
        //        rtbOutput.Text =
        //            "خروجی دستور:\n\n" +
        //            result.Output;

        //        lblStatus.Text = "دستور با موفقیت اجرا شد.";
        //        lblStatus.ForeColor = Color.Green;
        //    }
        //    else
        //    {
        //        rtbOutput.Text =
        //            "خروجی استاندارد:\n" +
        //            (string.IsNullOrWhiteSpace(result.Output)
        //                ? "(بدون خروجی)"
        //                : result.Output) +
        //            "\n\nخطای دستور:\n" +
        //            (string.IsNullOrWhiteSpace(result.Error)
        //                ? "(بدون پیام خطا)"
        //                : result.Error);

        //        lblStatus.Text = "اجرای دستور با خطا مواجه شد.";
        //        lblStatus.ForeColor = Color.Red;
        //    }
        //}
        #endregion

        private async void btnIpConfig_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(false);

            lblStatus.Text = "در حال دریافت اطلاعات IP...";
            lblStatus.ForeColor = Color.Black;

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

        private async void btnFlushDns_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(false);

            lblStatus.Text = "در حال پاک‌سازی DNS Cache...";
            lblStatus.ForeColor = Color.Black;

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();

            lblStatus.Text = "خروجی پاک شد.";
            lblStatus.ForeColor = Color.Black;
        }

        private async void btnResetNetwork_Click(object sender, EventArgs e)
        {
            DialogResult confirmation = MessageBox.Show(
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

            lblStatus.Text = "در حال ریست تنظیمات شبکه...";
            lblStatus.ForeColor = Color.Black;

            bool allCommandsSucceeded = true;

            try
            {
                CommandExecutionResult winsockResult =
                    await NetworkActions.ResetWinsockAsync();

                ShowCommandResult(
                    "دستور اجراشده: netsh winsock reset",
                    winsockResult);

                if (!winsockResult.IsSuccess)
                {
                    allCommandsSucceeded = false;
                }

                CommandExecutionResult ipResetResult =
                    await NetworkActions.ResetIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: netsh int ip reset",
                    ipResetResult);

                if (!ipResetResult.IsSuccess)
                {
                    allCommandsSucceeded = false;
                }

                CommandExecutionResult releaseResult =
                    await NetworkActions.ReleaseIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig /release",
                    releaseResult);

                if (!releaseResult.IsSuccess)
                {
                    allCommandsSucceeded = false;
                }

                CommandExecutionResult renewResult =
                    await NetworkActions.RenewIpAsync();

                ShowCommandResult(
                    "دستور اجراشده: ipconfig /renew",
                    renewResult);

                if (!renewResult.IsSuccess)
                {
                    allCommandsSucceeded = false;
                }

                AppendFinalResetStatus(allCommandsSucceeded);
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

        private void btnInternetOptions_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "control.exe",
                    Arguments = "inetcpl.cpl",
                    UseShellExecute = true
                });

                lblStatus.Text = "Internet Options باز شد.";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                AppendExceptionResult(
                    "باز کردن Internet Options",
                    ex);
            }
        }


    }
}