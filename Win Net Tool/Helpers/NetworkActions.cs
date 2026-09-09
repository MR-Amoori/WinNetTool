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
        #region DNS Ping Result

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
                    return Status == IPStatus.Success &&
                           PingMilliseconds.HasValue;
                }
            }
        }

        #endregion

        /// <summary>
        /// ارسال Ping به یک IP با Timeout مشخص.
        /// </summary>
        #region DNS Ping

        private static async Task<DnsPingResult>
            PingDnsAddressAsync(
                string address,
                int timeoutMilliseconds)
        {
            DnsPingResult result =
                new DnsPingResult
                {
                    Address = address,
                    Status = IPStatus.Unknown,
                    PingMilliseconds = null
                };

            if (string.IsNullOrWhiteSpace(address))
            {
                result.ErrorMessage =
                    "آدرس خالی است.";

                return result;
            }

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply =
                        await ping.SendPingAsync(
                            address.Trim(),
                            timeoutMilliseconds);

                    result.Status =
                        reply.Status;

                    if (reply.Status == IPStatus.Success)
                    {
                        /*
                         * زمان واقعی پاسخ برحسب میلی‌ثانیه
                         */
                        result.PingMilliseconds =
                            reply.RoundtripTime;
                    }
                    else
                    {
                        result.PingMilliseconds =
                            null;

                        result.ErrorMessage =
                            reply.Status.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result.PingMilliseconds =
                    null;

                result.Status =
                    IPStatus.Unknown;

                result.ErrorMessage =
                    ex.Message;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// تست هم‌زمان DNS اولیه و ثانویه یک سرویس.
        /// </summary>
        #region Ping One DNS Server

        private static async Task<DnsServerInfo>
            PingDnsServerAsync(
                DnsServerInfo dnsServer,
                int timeoutMilliseconds)
        {
            /*
             * مقادیر قبلی پاک می‌شوند تا نتیجه تست جدید
             * با اطلاعات قبلی مخلوط نشود.
             */
            dnsServer.PrimaryPingMilliseconds =
                null;

            dnsServer.SecondaryPingMilliseconds =
                null;

            /*
             * ابتدا DNS اولیه تست می‌شود.
             */
            DnsPingResult primaryResult =
                await PingDnsAddressAsync(
                    dnsServer.PrimaryAddress,
                    timeoutMilliseconds);

            if (primaryResult.IsSuccess)
            {
                dnsServer.PrimaryPingMilliseconds =
                    primaryResult.PingMilliseconds;
            }

            /*
             * بعد از پایان DNS اولیه، DNS ثانویه تست می‌شود.
             */
            if (!string.IsNullOrWhiteSpace(
                dnsServer.SecondaryAddress))
            {
                DnsPingResult secondaryResult =
                    await PingDnsAddressAsync(
                        dnsServer.SecondaryAddress,
                        timeoutMilliseconds);

                if (secondaryResult.IsSuccess)
                {
                    dnsServer.SecondaryPingMilliseconds =
                        secondaryResult.PingMilliseconds;
                }
            }

            return dnsServer;
        }

        #endregion

        /// <summary>
        /// تست Ping تمام DNSهای موجود در لیست.
        /// </summary>
        #region Ping All DNS Servers Sequentially

        public static async Task<List<DnsServerInfo>>
            PingAllDnsServersAsync(
                List<DnsServerInfo> dnsServers,
                int timeoutMilliseconds = 1500)
        {
            if (dnsServers == null)
            {
                throw new ArgumentNullException(
                    nameof(dnsServers));
            }

            List<DnsServerInfo> pingResults =
                new List<DnsServerInfo>();

            /*
             * DNSها عمداً یکی‌یکی تست می‌شوند.
             */
            foreach (DnsServerInfo dnsServer in dnsServers)
            {
                DnsServerInfo result =
                    await PingDnsServerAsync(
                        dnsServer,
                        timeoutMilliseconds);

                pingResults.Add(
                    result);
            }

            /*
             * DNSهایی که حداقل یک Ping موفق دارند
             * براساس کمترین امتیاز مرتب می‌شوند.
             *
             * DNSهایی که هیچ پاسخی نداده‌اند
             * در انتهای لیست قرار می‌گیرند.
             */
            pingResults = pingResults
                .OrderBy(
                    dns => dns.SortPingMilliseconds ==
                           double.MaxValue)
                .ThenBy(
                    dns => dns.SortPingMilliseconds)
                .ToList();

            return pingResults;
        }

        #endregion

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