using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Win_Net_Tool.Helpers
{
    public static class NetworkActions
    {
        public static Task<CommandExecutionResult> GetIpConfigAsync()
        {
            return CommandExecutor.ExecuteAsync(
                "ipconfig",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult> FlushDnsCacheAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /flushdns",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult> ClearCommands()
        {
            return await CommandExecutor.ExecuteAsync(
                "clear",
                CommandShell.Cmd);
        }

        #region Reset Network
        public static async Task<CommandExecutionResult> ResetWinsockAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "netsh winsock reset",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult> ResetIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "netsh int ip reset",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult> ReleaseIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /release",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult> RenewIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /renew",
                CommandShell.Cmd);
        }
        #endregion




    }
}