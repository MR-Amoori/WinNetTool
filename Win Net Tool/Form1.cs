using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_Net_Tool.Helpers;

namespace Win_Net_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            CommandExecutionResult result =
                await CommandExecutor.ExecuteAsync(
                    "whoami",
                    CommandShell.Cmd);

            string message;

            if (result.IsSuccess)
            {
                message =
                    "دستور با موفقیت اجرا شد:\n\n" +
                    result.Output;
            }
            else
            {
                message =
                    "دستور با خطا اجرا شد.\n\n" +
                    result.Error;
            }

            MessageBox.Show(
                message,
                "نتیجه اجرای دستور",
                MessageBoxButtons.OK,
                result.IsSuccess
                    ? MessageBoxIcon.Information
                    : MessageBoxIcon.Error);
        }

        private async void btnIpConfig_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(false);

            rtbOutput.Clear();
            lblStatus.Text = "در حال دریافت اطلاعات IP...";
            lblStatus.ForeColor = Color.Black;

            try
            {
                CommandExecutionResult result =
                    await NetworkActions.GetIpConfigAsync();

                ShowCommandResult(result);
            }
            catch (Exception ex)
            {
                rtbOutput.Text = ex.ToString();
                lblStatus.Text = "خطای غیرمنتظره";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnIpConfig.Enabled = enabled;
        }

        private void ShowCommandResult(CommandExecutionResult result)
        {
            if (result.IsSuccess)
            {
                rtbOutput.Text = result.Output;
                lblStatus.Text = "دستور با موفقیت اجرا شد.";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                rtbOutput.Text = result.Error;
                lblStatus.Text = "اجرای دستور با خطا مواجه شد.";
                lblStatus.ForeColor = Color.Red;
            }
        }

    }
}