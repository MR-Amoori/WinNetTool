using System;
using System.Collections.Generic;
using System.Linq;
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

        #region DNS Models

        public sealed class DnsServerInfo
        {
            #region Basic Information

            public string Name { get; set; }

            public string PrimaryAddress { get; set; }

            public string SecondaryAddress { get; set; }

            public string Purpose { get; set; }

            public string Stability { get; set; }

            public string Source { get; set; }

            #endregion


            #region Ping Information

            /// <summary>
            /// زمان Ping DNS اولیه برحسب میلی‌ثانیه.
            /// مقدار null یعنی پاسخی دریافت نشده است.
            /// </summary>
            public long? PrimaryPingMilliseconds { get; set; }

            /// <summary>
            /// زمان Ping DNS ثانویه برحسب میلی‌ثانیه.
            /// مقدار null یعنی پاسخی دریافت نشده است.
            /// </summary>
            public long? SecondaryPingMilliseconds { get; set; }

            /// <summary>
            /// امتیاز نهایی DNS برای مرتب‌سازی.
            /// میانگین Pingهای موفق است.
            /// </summary>
            public double SortPingMilliseconds
            {
                get
                {
                    List<long> successfulPings =
                        new List<long>();

                    if (PrimaryPingMilliseconds.HasValue)
                    {
                        successfulPings.Add(
                            PrimaryPingMilliseconds.Value);
                    }

                    if (SecondaryPingMilliseconds.HasValue)
                    {
                        successfulPings.Add(
                            SecondaryPingMilliseconds.Value);
                    }

                    if (successfulPings.Count == 0)
                    {
                        return double.MaxValue;
                    }

                    return successfulPings.Average();
                }
            }

            #endregion


            #region Display

            public override string ToString()
            {
                string primaryPing =
                    PrimaryPingMilliseconds.HasValue
                        ? PrimaryPingMilliseconds.Value.ToString()
                        : "-";

                string secondaryPing =
                    SecondaryPingMilliseconds.HasValue
                        ? SecondaryPingMilliseconds.Value.ToString()
                        : "-";

                return Name +
                       " | " +
                       primaryPing +
                       " " +
                       PrimaryAddress +
                       " | " +
                       secondaryPing +
                       " " +
                       SecondaryAddress;
            }

            #endregion
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


        #endregion


        #region DNS Ping Operations

        /// <summary>
        /// نتیجه Ping یک IP مربوط به DNS.
        /// </summary>
        public sealed class DnsPingResult
        {
            public string Address { get; set; }

            public long? PingMilliseconds { get; set; }

            public IPStatus Status { get; set; }

            public string ErrorMessage { get; set; }

            public bool IsSuccess
            {
                get
                {
                    return PingMilliseconds.HasValue &&
                           Status == IPStatus.Success;
                }
            }
        }

        /// <summary>
        /// ارسال Ping به یک IP با Timeout مشخص.
        /// </summary>
        private static async Task<DnsPingResult>
            PingDnsAddressAsync(
                string address,
                int timeoutMilliseconds)
        {
            DnsPingResult result =
                new DnsPingResult
                {
                    Address = address,
                    Status = IPStatus.Unknown
                };

            if (string.IsNullOrWhiteSpace(address))
            {
                result.ErrorMessage =
                    "آدرس DNS خالی است.";

                return result;
            }

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply =
                        await ping.SendPingAsync(
                            address,
                            timeoutMilliseconds);

                    result.Status =
                        reply.Status;

                    if (reply.Status == IPStatus.Success)
                    {
                        result.PingMilliseconds =
                            reply.RoundtripTime;
                    }
                    else
                    {
                        result.ErrorMessage =
                            "وضعیت Ping: " +
                            reply.Status;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status =
                    IPStatus.Unknown;

                result.ErrorMessage =
                    ex.Message;
            }

            return result;
        }

        /// <summary>
        /// تست هم‌زمان DNS اولیه و ثانویه یک سرویس.
        /// </summary>
        private static async Task<DnsServerInfo>
            PingDnsServerAsync(
                DnsServerInfo dnsServer,
                int timeoutMilliseconds)
        {
            Task<DnsPingResult> primaryTask =
                PingDnsAddressAsync(
                    dnsServer.PrimaryAddress,
                    timeoutMilliseconds);

            Task<DnsPingResult> secondaryTask =
                string.IsNullOrWhiteSpace(
                    dnsServer.SecondaryAddress)
                    ? Task.FromResult<DnsPingResult>(null)
                    : PingDnsAddressAsync(
                        dnsServer.SecondaryAddress,
                        timeoutMilliseconds);

            DnsPingResult[] results =
                await Task.WhenAll(
                    primaryTask,
                    secondaryTask);

            DnsPingResult primaryResult =
                results[0];

            DnsPingResult secondaryResult =
                results[1];

            dnsServer.PrimaryPingMilliseconds =
                primaryResult != null &&
                primaryResult.IsSuccess
                    ? primaryResult.PingMilliseconds
                    : null;

            dnsServer.SecondaryPingMilliseconds =
                secondaryResult != null &&
                secondaryResult.IsSuccess
                    ? secondaryResult.PingMilliseconds
                    : null;

            return dnsServer;
        }

        /// <summary>
        /// تست Ping تمام DNSهای موجود در لیست.
        /// </summary>
        public static async Task<List<DnsServerInfo>>
            PingAllDnsServersAsync(
                List<DnsServerInfo> dnsServers,
                int timeoutMilliseconds = 3000)
        {
            if (dnsServers == null)
            {
                throw new ArgumentNullException(
                    nameof(dnsServers));
            }

            List<Task<DnsServerInfo>> pingTasks =
                new List<Task<DnsServerInfo>>();

            foreach (DnsServerInfo dnsServer in dnsServers)
            {
                pingTasks.Add(
                    PingDnsServerAsync(
                        dnsServer,
                        timeoutMilliseconds));
            }

            DnsServerInfo[] pingResults =
                await Task.WhenAll(
                    pingTasks);

            List<DnsServerInfo> sortedResults =
                new List<DnsServerInfo>(
                    pingResults);

            /*
             * DNSهایی که پاسخ داده‌اند، براساس میانگین Ping
             * از کمترین به بیشترین مرتب می‌شوند.
             *
             * DNSهایی که هیچ پاسخی نداده‌اند،
             * به دلیل double.MaxValue در انتهای لیست قرار می‌گیرند.
             */
            sortedResults.Sort(
                delegate (
                    DnsServerInfo first,
                    DnsServerInfo second)
                {
                    return first.SortPingMilliseconds.CompareTo(
                        second.SortPingMilliseconds);
                });

            return sortedResults;
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