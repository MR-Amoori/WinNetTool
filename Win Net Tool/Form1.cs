using System;
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
    }
}