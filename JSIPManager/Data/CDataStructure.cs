using System.Collections.Generic;

namespace JSIPManager
{
    public class CNICIP_INFOR
    {
        public string NAME { get; set; }
        public string IPADDRESS { get; set; }
        public string SUBNET_MASK { get; set; }
        public string GATEWAY { get; set; }
        public string DEFAULT_DNS { get; set; }
        public string SUB_DNS { get; set; }
    }

    public class NIC_ITEM
    {
        public int IDX { get; set; }
        public string NIC_NAME { get; set; }
    }

    public class CNIC_DETIL_INFOR
    {
        public string NIC_NAME { get; set; }
        public List<string> DNSADDRESSES { get; set; }
        public List<string> ANYCASTADDRESSES { get; set; }
        public List<string> MULTICASTADDRESSES { get; set; }
        public List<string> UNICASTADDRESSES { get; set; }
        public List<string> GATEWAYADDRESSES { get; set; }
        public List<string> SUBNETMASKADDRESSES { get; set; }
    }


    public class OPTION
    {
        public string NAME { get; set; }
        public string LOAD_NIC { get; set; }
        public string LOAD_IPLIST { get; set; }
        public string SAVE_IPLIST { get; set; }
    }

    public class PING_OPTION
    {
        public PING_OPTION()
        {
            _T = "T";
        }
        public string _T { get; set; }
        //public string _A { get; set; }
    }
}
