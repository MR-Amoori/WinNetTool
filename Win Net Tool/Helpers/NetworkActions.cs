using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;

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

        public sealed class AdapterDhcpResult
        {
            public string AdapterName { get; set; }

            public CommandExecutionResult AddressResult { get; set; }

            public CommandExecutionResult DnsResult { get; set; }

            public bool IsSuccess
            {
                get
                {
                    return AddressResult != null &&
                           DnsResult != null &&
                           AddressResult.IsSuccess &&
                           DnsResult.IsSuccess;
                }
            }
        }

        public static async Task<List<AdapterDhcpResult>>
    SetAllAdaptersToDhcpAsync()
        {
            List<AdapterDhcpResult> results =
                new List<AdapterDhcpResult>();

            NetworkInterface[] adapters =
                NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in adapters)
            {
                // Loopback مثل 127.0.0.1 قابل تنظیم با DHCP نیست
                if (adapter.NetworkInterfaceType ==
                    NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                string adapterName =
                    adapter.Name.Replace("\"", "\\\"");

                string setIpToDhcpCommand =
                    "netsh interface ipv4 set address " +
                    "name=\"" +
                    adapterName +
                    "\" source=dhcp";

                string setDnsToDhcpCommand =
                    "netsh interface ipv4 set dnsservers " +
                    "name=\"" +
                    adapterName +
                    "\" source=dhcp";

                CommandExecutionResult addressResult =
                    await CommandExecutor.ExecuteAsync(
                        setIpToDhcpCommand,
                        CommandShell.Cmd);

                CommandExecutionResult dnsResult =
                    await CommandExecutor.ExecuteAsync(
                        setDnsToDhcpCommand,
                        CommandShell.Cmd);

                results.Add(
                    new AdapterDhcpResult
                    {
                        AdapterName = adapter.Name,
                        AddressResult = addressResult,
                        DnsResult = dnsResult
                    });
            }

            return results;
        }



    }
}