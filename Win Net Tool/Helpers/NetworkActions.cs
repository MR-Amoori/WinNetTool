using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Win_Net_Tool.Helpers
{
    public static class NetworkActions
    {
        #region General Network Commands

        public static Task<CommandExecutionResult>
            GetIpConfigAsync()
        {
            return CommandExecutor.ExecuteAsync(
                "ipconfig",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult>
            FlushDnsCacheAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /flushdns",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult>
            ClearCommands()
        {
            return await CommandExecutor.ExecuteAsync(
                "clear",
                CommandShell.Cmd);
        }

        #endregion


        #region Reset Network

        public static async Task<CommandExecutionResult>
            ResetWinsockAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "netsh winsock reset",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult>
            ResetIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "netsh int ip reset",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult>
            ReleaseIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /release",
                CommandShell.Cmd);
        }

        public static async Task<CommandExecutionResult>
            RenewIpAsync()
        {
            return await CommandExecutor.ExecuteAsync(
                "ipconfig /renew",
                CommandShell.Cmd);
        }

        #endregion


        #region DHCP

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
                if (adapter.NetworkInterfaceType ==
                    NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                string adapterName =
                    EscapeAdapterName(
                        adapter.Name);

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

        #endregion


        #region DNS Models

        public sealed class DnsServerInfo
        {
            public string Name { get; set; }

            public string PrimaryAddress { get; set; }

            public string SecondaryAddress { get; set; }

            public string Purpose { get; set; }

            public string Stability { get; set; }

            public string Source { get; set; }

            public override string ToString()
            {
                return Name +
                       " | " +
                       PrimaryAddress +
                       " | " +
                       SecondaryAddress;
            }
        }

        public sealed class AdapterDnsResult
        {
            public string AdapterName { get; set; }

            public CommandExecutionResult PrimaryResult { get; set; }

            public CommandExecutionResult SecondaryResult { get; set; }

            public bool IsSuccess
            {
                get
                {
                    bool primarySucceeded =
                        PrimaryResult != null &&
                        PrimaryResult.IsSuccess;

                    bool secondarySucceeded =
                        SecondaryResult == null ||
                        SecondaryResult.IsSuccess;

                    return primarySucceeded &&
                           secondarySucceeded;
                }
            }
        }

        #endregion


        #region DNS Operations

        /// <summary>
        /// تنظیم DNS اولیه و ثانویه روی تمام آداپتورهای شبکه.
        /// IPv6 عمداً در این عملیات استفاده نمی‌شود.
        /// </summary>
        public static async Task<List<AdapterDnsResult>>
            SetDnsForAllAdaptersAsync(
                string primaryDns,
                string secondaryDns)
        {
            List<AdapterDnsResult> results =
                new List<AdapterDnsResult>();

            NetworkInterface[] adapters =
                NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in adapters)
            {
                /*
                 * Loopback مانند 127.0.0.1
                 * آداپتور شبکه قابل تنظیم نیست.
                 */
                if (adapter.NetworkInterfaceType ==
                    NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                string adapterName =
                    EscapeAdapterName(
                        adapter.Name);

                /*
                 * تنظیم DNS اولیه روی IPv4.
                 * اجرای set باعث جایگزینی DNSهای قبلی می‌شود.
                 */
                string setPrimaryDnsCommand =
                    "netsh interface ipv4 set dnsservers " +
                    "name=\"" +
                    adapterName +
                    "\" " +
                    "source=static " +
                    "address=" +
                    primaryDns +
                    " " +
                    "validate=no";

                CommandExecutionResult primaryResult =
                    await CommandExecutor.ExecuteAsync(
                        setPrimaryDnsCommand,
                        CommandShell.Cmd);

                CommandExecutionResult secondaryResult =
                    null;

                /*
                 * DNS ثانویه در صورت معتبر بودن اضافه می‌شود.
                 * برای سرویس‌هایی که DNS ثانویه ندارند،
                 * این مرحله عمداً انجام نمی‌شود.
                 */
                if (!string.IsNullOrWhiteSpace(
                    secondaryDns))
                {
                    string addSecondaryDnsCommand =
                        "netsh interface ipv4 add dnsservers " +
                        "name=\"" +
                        adapterName +
                        "\" " +
                        "address=" +
                        secondaryDns +
                        " " +
                        "index=2 " +
                        "validate=no";

                    secondaryResult =
                        await CommandExecutor.ExecuteAsync(
                            addSecondaryDnsCommand,
                            CommandShell.Cmd);
                }

                results.Add(
                    new AdapterDnsResult
                    {
                        AdapterName = adapter.Name,
                        PrimaryResult = primaryResult,
                        SecondaryResult = secondaryResult
                    });
            }

            return results;
        }

        #endregion


        #region Utility Methods

        private static string EscapeAdapterName(
            string adapterName)
        {
            if (string.IsNullOrWhiteSpace(adapterName))
            {
                return string.Empty;
            }

            return adapterName.Replace(
                "\"",
                "\\\"");
        }

        #endregion
    }
}