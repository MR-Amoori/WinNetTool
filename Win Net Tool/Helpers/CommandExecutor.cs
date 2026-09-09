using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Win_Net_Tool.Helpers
{
    public static class CommandExecutor
    {
        public static async Task<CommandExecutionResult> ExecuteAsync(
            string command,
            CommandShell shell)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException(
                    "دستور نمی‌تواند خالی باشد.",
                    nameof(command));
            }

            ProcessStartInfo startInfo = CreateStartInfo(command, shell);

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    return new CommandExecutionResult
                    {
                        Output = string.Empty,
                        Error = ex.Message,
                        ExitCode = -1
                    };
                }

                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);

                process.WaitForExit();

                return new CommandExecutionResult
                {
                    Output = outputTask.Result,
                    Error = errorTask.Result,
                    ExitCode = process.ExitCode
                };
            }
        }

        private static ProcessStartInfo CreateStartInfo(
            string command,
            CommandShell shell)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                WorkingDirectory = Environment.CurrentDirectory
            };

            if (shell == CommandShell.Cmd)
            {
                startInfo.FileName =
                    Environment.GetEnvironmentVariable("ComSpec")
                    ?? "cmd.exe";

                startInfo.Arguments = "/c " + command;
            }
            else
            {
                startInfo.FileName = "powershell.exe";

                string encodedCommand = Convert.ToBase64String(
                    Encoding.Unicode.GetBytes(command));

                startInfo.Arguments =
                    "-NoProfile " +
                    "-NonInteractive " +
                    "-ExecutionPolicy Bypass " +
                    "-EncodedCommand " +
                    encodedCommand;
            }

            return startInfo;
        }
    }
}