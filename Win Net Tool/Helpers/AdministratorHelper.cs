using System.Security.Principal;

namespace Win_Net_Tool.Helpers
{
    public static class AdministratorHelper
    {
        public static bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal =
                    new WindowsPrincipal(identity);

                return principal.IsInRole(
                    WindowsBuiltInRole.Administrator);
            }
        }
    }
}