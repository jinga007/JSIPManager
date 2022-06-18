using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;



namespace JSIPManager
{
    internal class CXMLManagement
    {
        private readonly string _URL = Properties.Resources.URL;
        private readonly string _CONFIG_URL = Properties.Resources.CONFIG_URL;

        public CXMLManagement()
        {
#if DEBUG

#else
            _URL = Properties.Resources.DIR + _URL;
            _CONFIG_URL = Properties.Resources.DIR + _CONFIG_URL;
#endif
        }

        internal ObservableCollection<CNICIP_INFOR> GetIPHistXML(string url = "")
        {
            ObservableCollection<CNICIP_INFOR> _rcvDatas = new ObservableCollection<CNICIP_INFOR>();
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    url = _URL;

                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(url);
                    XmlNodeList xnList = xml.SelectNodes("HIST_DATAS/HIST_DATA");
                    foreach (XmlNode xn in xnList)
                    {
                        CNICIP_INFOR _rcvData = new CNICIP_INFOR();
                        _rcvData.NAME = xn["NAME"].InnerText;
                        _rcvData.IPADDRESS = xn["IPADDRESS"].InnerText;
                        _rcvData.SUBNET_MASK = xn["SUBNET_MASK"].InnerText;
                        _rcvData.GATEWAY = xn["GATEWAY"].InnerText;
                        _rcvData.DEFAULT_DNS = xn["DEFAULT_DNS"].InnerText;
                        _rcvData.SUB_DNS = xn["SUB_DNS"].InnerText;
                        _rcvDatas.Add(_rcvData);
                    }
                }
                catch (ArgumentException ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }

            return _rcvDatas;
        }

        internal ObservableCollection<OPTION> GetConfigXML(string url = "")
        {
            ObservableCollection<OPTION> _rcvDatas = new ObservableCollection<OPTION>();
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    url = _CONFIG_URL;

                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(url);
                    XmlNodeList xnList = xml.SelectNodes("OPTIONS/OPTION");
                    foreach (XmlNode xn in xnList)
                    {
                        OPTION _rcvData = new OPTION();
                        _rcvData.NAME = xn["NAME"].InnerText;
                        _rcvData.LOAD_NIC = xn["LOAD_NIC"].InnerText;
                        _rcvData.LOAD_IPLIST = xn["LOAD_IPLIST"].InnerText;
                        _rcvData.SAVE_IPLIST = xn["SAVE_IPLIST"].InnerText;
                        _rcvDatas.Add(_rcvData);
                    }
                }
                catch (ArgumentException ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }

            return _rcvDatas;
        }

        internal bool SaveIPHistXML(ObservableCollection<CNICIP_INFOR> _Datas, string url = "")
        {
            bool _rets = false;

            try
            {
                if (_Datas == null)
                    throw new Exception();

                if (string.IsNullOrWhiteSpace(url))
                    url = _URL;

                XDocument xdoc = new XDocument(new XDeclaration("1.0", "UTF-8", null));
                XElement xroot = new XElement("HIST_DATAS");
                xdoc.Add(xroot);
                int idx = 100;
                foreach (var item in _Datas)
                {
                    XElement xe = new XElement("HIST_DATA",
                        new XAttribute("Id", (idx++).ToString("000")),
                        new XElement("NAME", item.NAME),
                        new XElement("IPADDRESS", item.IPADDRESS),
                        new XElement("SUBNET_MASK", item.SUBNET_MASK),
                        new XElement("GATEWAY", item.GATEWAY),
                        new XElement("DEFAULT_DNS", item.DEFAULT_DNS),
                        new XElement("SUB_DNS", item.SUB_DNS)
                        );
                    xroot.Add(xe);
                }
                xdoc.Save(url);
                _rets = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return _rets;
        }


        internal bool SaveOptionXML(ObservableCollection<OPTION> _Datas, string url = "")
        {
            bool _rets = false;

            try
            {
                if (_Datas == null)
                    throw new Exception();

                if (string.IsNullOrWhiteSpace(url))
                    url = _CONFIG_URL;

                XDocument xdoc = new XDocument(new XDeclaration("1.0", "UTF-8", null));
                XElement xroot = new XElement("OPTIONS");
                xdoc.Add(xroot);
                int idx = 100;
                foreach (var item in _Datas)
                {
                    XElement xe = new XElement("OPTION",
                        new XAttribute("Id", (idx++).ToString("000")),
                        new XElement("NAME", item.NAME),
                        new XElement("LOAD_NIC", item.LOAD_NIC),
                        new XElement("LOAD_IPLIST", item.LOAD_IPLIST),
                        new XElement("SAVE_IPLIST", item.SAVE_IPLIST)
                        );
                    xroot.Add(xe);
                }
                xdoc.Save(url);
                _rets = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return _rets;
        }
    }
}
