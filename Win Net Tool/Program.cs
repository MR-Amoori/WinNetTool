using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_Net_Tool.Helpers;

namespace Win_Net_Tool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!AdministratorHelper.IsRunningAsAdministrator())
            {
                MessageBox.Show(
                    "برنامه باید با دسترسی Administrator اجرا شود.",
                    "سطح دسترسی ناکافی",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Application.Run(new Form1());
        }
    }
}
