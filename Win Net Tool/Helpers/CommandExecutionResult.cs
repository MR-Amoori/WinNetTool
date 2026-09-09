namespace Win_Net_Tool.Helpers
{
    public class CommandExecutionResult
    {
        public string Output { get; set; }

        public string Error { get; set; }

        public int ExitCode { get; set; }

        public bool IsSuccess
        {
            get
            {
                return ExitCode == 0;
            }
        }
    }
}