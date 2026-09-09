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
    }
}