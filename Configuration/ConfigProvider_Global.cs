using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using FISCA.DSAUtil;

namespace K12.Data.Configuration
{
    /// <summary>
    /// 實作組態存取介面，核心會透過此類別存取組態資料。
    /// </summary>
    internal class ConfigProvider_Global : IConfigurationProvider
    {
        private static DSConnection Connection { get; set; }

        private static DSXmlHelper CallService(string serviceName, DSXmlHelper request)
        {
            string Server;

            //if (Diagnostic.DebugMode && Diagnostic.Options.UseInsideGlobalConfigurationServer)
            //    Server = Diagnostic.Options.InsideGlobalConfigurationServer;
            //else
                Server = "quality";

            if (Connection == null)
            {
                try
                {
                    Connection = new DSConnection(Server, "anonymous", "");
                    Connection.Connect();
                }
                catch (Exception ex)
                {
                    ExceptionReport report = new ExceptionReport();
                    report.AddType(typeof(HttpWebResponse), true);
                    report.AddType(typeof(HttpWebRequest), true);

                    Console.WriteLine("嘗試連線到組態儲存主機失敗({0})。", Server);
                    Console.WriteLine(report.Transform(ex));

                    throw;
                }
            }

            return Connection.SendRequest(serviceName, request);
        }

        public ConfigProvider_Global()
        {
            Connection = null;
        }

        #region IConfigurationProvider 成員

        public Dictionary<string, ConfigurationRecord> GetAllConfiguration()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            request.AddElement("Content");

            return SendRequest(request);
        }

        public Dictionary<string, ConfigurationRecord> GetConfiguration(IEnumerable<string> namespaces)
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            bool execute_required = false;
            request.AddElement("Content");
            request.AddElement("Condition");
            foreach (string each in namespaces)
            {
                request.AddElement("Condition", "Name", each);
                execute_required = true;
            }

            if (execute_required)
                return SendRequest(request);
            else
                return new Dictionary<string, ConfigurationRecord>();

        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        private static Dictionary<string, ConfigurationRecord> SendRequest(DSXmlHelper request)
        {
            string srvname = "Configuration.GetDetailList";
            Dictionary<string, ConfigurationRecord> records = new Dictionary<string, ConfigurationRecord>();

            DSXmlHelper response = CallService(srvname, request);

            foreach (XmlElement each in response.GetElements("Configuration"))
            {
                XmlHelper helper = new XmlHelper(each);
                string name = helper.GetString("Name");

                XmlElement configdata = null;
                foreach (XmlNode content in helper.GetElement("Content").ChildNodes)
                {
                    if (content.NodeType == XmlNodeType.Element) //內容可能是以「Configurations」為 Root，也可能是舊的格式。
                        configdata = content as XmlElement;
                }

                if (configdata == null)
                    configdata = XmlHelper.LoadXml("<" + ConfigurationRecord.RootName + "/>");

                records.Add(name, new ConfigurationRecord(name, configdata as XmlElement));
            }

            return records;
        }

        public void SaveConfiguration(IEnumerable<ConfigurationRecord> configurations)
        {
            MultiThreadWorker<ConfigurationRecord> worker = new MultiThreadWorker<ConfigurationRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 20;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ConfigurationRecord> e)
            {
                DSXmlHelper insert = new DSXmlHelper("Request");
                DSXmlHelper update = new DSXmlHelper("Request");
                DSXmlHelper delete = new DSXmlHelper("Request");
                bool insert_exec = false, update_exec = false, delete_exec = false;

                foreach (ConfigurationRecord eachConf in e.List)
                {
                    if (eachConf.EditAction == 1) //新增
                    {
                        insert.AddElement("Configuration");
                        insert.AddElement("Configuration", "Name", eachConf.Namespace);
                        insert.AddElement("Configuration", "Content", eachConf.GetXml(), true);
                        insert_exec = true;
                    }
                    else if (eachConf.EditAction == 2) //修改
                    {
                        update.AddElement("Configuration");
                        update.AddElement("Configuration", "Content", eachConf.GetXml(), true);
                        update.AddElement("Configuration", "Condition");
                        update.AddElement("Configuration/Condition", "Name", eachConf.Namespace);
                        update_exec = true;
                    }
                    else if (eachConf.EditAction == 3) //刪除
                    {
                        delete.AddElement("Configuration");
                        delete.AddElement("Configuration", "Name", eachConf.Namespace);
                        delete_exec = true;
                    }
                    else
                        throw new ArgumentException("沒有這一種的啦。");
                }

                if (insert_exec)
                    CallService("Configuration.Insert", (insert));

                if (update_exec)
                    CallService("Configuration.Update", (update));

                if (delete_exec)
                    CallService("Configuration.Delete", (delete));
            };

            List<PackageWorkEventArgs<ConfigurationRecord>> results = worker.Run(configurations);

            foreach (PackageWorkEventArgs<ConfigurationRecord> each in results)
                if (each.HasException) throw each.Exception;
        }

        #endregion
    }
}
