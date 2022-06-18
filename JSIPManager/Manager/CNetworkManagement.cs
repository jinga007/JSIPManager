using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace JSIPManager

{

    internal class CNetworkAdptManager
    {
        static List<CNIC_DETIL_INFOR> __NIC_DETIL_INFORS = new List<CNIC_DETIL_INFOR>();

        internal List<CNIC_DETIL_INFOR> NIC_DETIL_INFORS
        {
            get { return __NIC_DETIL_INFORS; }
        }

        internal Dictionary<int, string> GetNICDescriptions()
        {
            Dictionary<int, string> dicNICDesc = new Dictionary<int, string>();

            try
            {
                int _idx = 0;
                __NIC_DETIL_INFORS.Clear();
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                    {
                        continue;
                    }
                    dicNICDesc.Add(_idx++, adapter.Description);

                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    CNIC_DETIL_INFOR NIC_DETIL_INFOR = new CNIC_DETIL_INFOR();
                    NIC_DETIL_INFOR.NIC_NAME = adapter.Description; ;
                    NIC_DETIL_INFOR.DNSADDRESSES = GetDNSAddresses(adapterProperties);
                    NIC_DETIL_INFOR.ANYCASTADDRESSES = GetAnycastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.MULTICASTADDRESSES = GetMulticastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.UNICASTADDRESSES = GetUnicastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.GATEWAYADDRESSES = GetGatewayAddresses(adapterProperties);
                    NIC_DETIL_INFOR.SUBNETMASKADDRESSES = GetSubnetMaskAddresses(adapterProperties);
                    __NIC_DETIL_INFORS.Add(NIC_DETIL_INFOR);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return dicNICDesc;
        }

        internal ObservableCollection<NIC_ITEM> GetNICDescs()
        {
            ObservableCollection<NIC_ITEM> dicNICDesc = new ObservableCollection<NIC_ITEM>();

            try
            {
                int _idx = 0;
                __NIC_DETIL_INFORS.Clear();
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                    {
                        continue;
                    }
                    NIC_ITEM _NIC_ITEM = new NIC_ITEM();
                    _NIC_ITEM.IDX = _idx++;
                    _NIC_ITEM.NIC_NAME = adapter.Description;
                    dicNICDesc.Add(_NIC_ITEM);
                    //dicNICDesc.Add(_idx++, adapter.Description);

                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    CNIC_DETIL_INFOR NIC_DETIL_INFOR = new CNIC_DETIL_INFOR();
                    NIC_DETIL_INFOR.NIC_NAME = adapter.Description; ;
                    NIC_DETIL_INFOR.DNSADDRESSES = GetDNSAddresses(adapterProperties);
                    NIC_DETIL_INFOR.ANYCASTADDRESSES = GetAnycastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.MULTICASTADDRESSES = GetMulticastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.UNICASTADDRESSES = GetUnicastAddresses(adapterProperties);
                    NIC_DETIL_INFOR.GATEWAYADDRESSES = GetGatewayAddresses(adapterProperties);
                    NIC_DETIL_INFOR.SUBNETMASKADDRESSES = GetSubnetMaskAddresses(adapterProperties);
                    __NIC_DETIL_INFORS.Add(NIC_DETIL_INFOR);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return dicNICDesc;
        }

        internal bool setDynamicIP(string scDesc)
        {
            bool bRest = false;

            foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
            {
                if (!(bool)mo["ipEnabled"])
                    continue;

                try
                {
                    string desc = (string)(mo["Description"]);

                    if (string.Compare(desc, scDesc, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = null;
                        mo.InvokeMethod("EnableDHCP", null, null);
                        mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    }

                    bRest = true;
                }
                catch (Exception ex)
                {
                    bRest = false;
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally
                {

                }
            }

            return bRest;

        }

        internal bool SetStaticIP(string scDesc, string scIPAddr, string scSubnetMask, string scGateWay, string scDNSServer, string scSubDNSServer)
        {
            bool bRest = false;
            try
            {
                ManagementObjectCollection mgObjCls = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();

                foreach (ManagementObject mgObj in mgObjCls)
                {
                    string desc = mgObj["Description"] as string;

                    if (string.Compare(desc, scDesc, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        try
                        {
                            ManagementBaseObject setGatewaysMangmBaseObj = mgObj.GetMethodParameters("SetGateways");
                            setGatewaysMangmBaseObj["DefaultIpGateway"] = new string[] { scGateWay };
                            setGatewaysMangmBaseObj["GatewayCostMetric"] = new int[] { 1 };
                            ManagementBaseObject enableStaticMangmBaseObj = mgObj.GetMethodParameters("EnableStatic");
                            enableStaticMangmBaseObj["IPAddress"] = new string[] { scIPAddr };
                            enableStaticMangmBaseObj["SubnetMask"] = new string[] { scSubnetMask };

                            ManagementBaseObject SetDNSServerMangmBaseObj = mgObj.GetMethodParameters("SetDNSServerSearchOrder");
                            SetDNSServerMangmBaseObj["DNSServerSearchOrder"] = new string[] { scDNSServer, scSubDNSServer };

                            mgObj.InvokeMethod("SetDNSServerSearchOrder", SetDNSServerMangmBaseObj, null);
                            mgObj.InvokeMethod("EnableStatic", enableStaticMangmBaseObj, null);
                            mgObj.InvokeMethod("SetGateways", setGatewaysMangmBaseObj, null);

                            bRest = true;
                        }
                        catch (Exception ex)
                        {
                            bRest = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bRest = false;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {


            }
            return bRest;
        }

        private List<string> GetDNSAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> DNSAddresses = new List<string>();
            IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
            if (dnsServers != null)
            {
                foreach (IPAddress dns in dnsServers)
                {
                    DNSAddresses.Add( dns.ToString());
                }
            }

            return DNSAddresses;

        }

        private List<string> GetAnycastAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> AnycastAddresses = new List<string>();
            IPAddressInformationCollection anyCast = adapterProperties.AnycastAddresses;
            if (anyCast != null)
            {
                foreach (IPAddressInformation any in anyCast)
                {
                    AnycastAddresses.Add(any.Address.ToString());
                }
            }

            return AnycastAddresses;

        }

        private List<string> GetMulticastAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> MulticastAddresses = new List<string>();
            MulticastIPAddressInformationCollection multiCast = adapterProperties.MulticastAddresses;
            if (multiCast != null)
            {
                foreach (IPAddressInformation multi in multiCast)
                {
                    MulticastAddresses.Add(multi.Address.ToString());
                }
            }

            return MulticastAddresses;

        }

        private List<string> GetUnicastAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> UnicastAddresses = new List<string>();
            UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
            if (uniCast != null)
            {
                foreach (UnicastIPAddressInformation uni in uniCast)
                {
                    UnicastAddresses.Add(uni.Address.ToString());
                }
            }

            return UnicastAddresses;

        }

        private List<string> GetGatewayAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> GatewayAddresses = new List<string>();
            GatewayIPAddressInformationCollection gateWay = adapterProperties.GatewayAddresses;
            if (gateWay != null)
            {
                foreach (GatewayIPAddressInformation gate in gateWay)
                {
                    GatewayAddresses.Add(gate.Address.ToString());
                }
            }

            return GatewayAddresses;

        }

        private List<string> GetSubnetMaskAddresses(IPInterfaceProperties adapterProperties)
        {
            List<string> SubnetMaskAddresses = new List<string>();
            foreach (var uipi in adapterProperties.UnicastAddresses)
            {
                if(uipi.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                { 
                    //if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback) 
                    {
                        SubnetMaskAddresses.Add(uipi.IPv4Mask.ToString()); 
                    } 
                }

            }
            return SubnetMaskAddresses;

        }

        internal Boolean GetDhcp(string scDesc)
        {
            bool bResult = false;
            NetworkInterface[] niAdpaters = NetworkInterface.GetAllNetworkInterfaces();

            foreach(var item in niAdpaters)
            {
                if (string.Compare(item.Description, scDesc, StringComparison.InvariantCultureIgnoreCase) == 0)
                { 
                    if (item.GetIPProperties().GetIPv4Properties() != null)
                    {
                        bResult = item.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
                    }
                }
            }
            return bResult;
        }

        internal string Ping(string strAddress, int iRetrycnt = 5, int iTimeOut = 120)
        {
            string strResult = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                var ipAddress = strAddress;
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

                int i = 0;
                do
                {
                    PingReply reply = pingSender.Send(ipAddress, iTimeOut, buffer, options);
                    if (reply != null)
                    {
                        if (reply.Status == IPStatus.Success)
                        {
                            sb.Append($"{reply.Address} 의 응답 : BYTE={reply.Buffer.Length} 시간<{reply.RoundtripTime}ms TTL={reply.Options.Ttl}");
                        }
                        else
                        {
                            sb.Append($"[{reply.Status}]  [{reply.RoundtripTime}]");
                        }
                    }

                    if (iRetrycnt == 5)
                    {
                        sb.AppendLine("");
                    }

                } while (++i < iRetrycnt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                strResult = sb.ToString();
            }

            return strResult;
        }

        #region 참조 자료
        private void ShowIPAddresses(IPInterfaceProperties adapterProperties)
        {
            IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
            if (dnsServers != null)
            {
                foreach (IPAddress dns in dnsServers)
                {
                    Console.WriteLine("  DNS Servers ............................. : {0}",
                        dns.ToString()
                   );
                }
            }


            IPAddressInformationCollection anyCast = adapterProperties.AnycastAddresses;
            if (anyCast != null)
            {
                foreach (IPAddressInformation any in anyCast)
                {
                    Console.WriteLine("  Anycast Address .......................... : {0} {1} {2}",
                        any.Address,
                        any.IsTransient ? "Transient" : "",
                        any.IsDnsEligible ? "DNS Eligible" : ""
                    );
                }
                Console.WriteLine();
            }

            MulticastIPAddressInformationCollection multiCast = adapterProperties.MulticastAddresses;
            if (multiCast != null)
            {
                foreach (IPAddressInformation multi in multiCast)
                {
                    Console.WriteLine("  Multicast Address ....................... : {0} {1} {2}",
                        multi.Address,
                        multi.IsTransient ? "Transient" : "",
                        multi.IsDnsEligible ? "DNS Eligible" : ""
                    );
                }
                Console.WriteLine();
            }
            UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
            if (uniCast != null)
            {
                string lifeTimeFormat = "dddd, MMMM dd, yyyy  hh:mm:ss tt";
                foreach (UnicastIPAddressInformation uni in uniCast)
                {
                    DateTime when;

                    Console.WriteLine("  Unicast Address ......................... : {0}", uni.Address);
                    Console.WriteLine("     Prefix Origin ........................ : {0}", uni.PrefixOrigin);
                    Console.WriteLine("     Suffix Origin ........................ : {0}", uni.SuffixOrigin);
                    Console.WriteLine("     Duplicate Address Detection .......... : {0}",
                        uni.DuplicateAddressDetectionState);

                    // Format the lifetimes as Sunday, February 16, 2003 11:33:44 PM
                    // if en-us is the current culture.

                    // Calculate the date and time at the end of the lifetimes.
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressValidLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Valid Life Time ...................... : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressPreferredLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Preferred life time .................. : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );

                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.DhcpLeaseLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     DHCP Leased Life Time ................ : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );
                }
                Console.WriteLine();
            }
        }
        #endregion
    }

}
