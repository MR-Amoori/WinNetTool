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
    }
}