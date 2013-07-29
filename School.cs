using System;
using System.Xml;
using K12.Data.Configuration;
using System.Collections.Generic;

namespace K12.Data
{
    /// <summary>
    /// 提供學校相關資訊。
    /// </summary>
    public class School
    {
        private static ConfigurationManager _configuration = null;
        private static ConfigurationManager _globalconfiguration = null;

        /// <summary>
        /// 取得學校的組態資料。
        /// </summary>
        public static ConfigurationManager Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationManager(new ConfigProvider_App());
                    _configuration.ConfigurationUpdated += new EventHandler<ItemUpdatedEventArgs>(Configuration_ConfigurationUpdated);
                }

                return _configuration;
            }
        }

        public static ConfigurationManager GlobalConfiguration
        {
            get
            {
                if (_globalconfiguration == null)
                {
                    _globalconfiguration = new ConfigurationManager(new ConfigProvider_Global());
                    _globalconfiguration.ConfigurationUpdated += new EventHandler<ItemUpdatedEventArgs>(Configuration_ConfigurationUpdated);
                }

                return _globalconfiguration;
            }
        }

        private static void Configuration_ConfigurationUpdated(object sender, ItemUpdatedEventArgs e)
        {
            if (e.PrimaryKeys.Contains("系統設定"))
                SysConfig = null;

            if (e.PrimaryKeys.Contains("學校資訊"))
                SchoolConfig = null;
        }

        //系統設定
        protected static XmlElement SysConfig = null;
        protected static XmlElement SchoolConfig = null;

        /// <summary>
        /// 取得所有國小學校列表
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.School.SelectElementarySchools", "學籍.國小列表")]
        public static List<SchoolRecord> SelectElementarySchools()
        {
            List<SchoolRecord> SchoolRecs = new List<SchoolRecord>();

            // 讀取國小資料
            ConfigData cd = GlobalConfiguration["SchoolListElementary"];

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(cd["XmlData"]);

            foreach (XmlElement xe in doc.SelectSingleNode("SchoolList"))
            {
                SchoolRecord SchoolRec = new SchoolRecord();
                SchoolRec.Load(xe);
                SchoolRecs.Add(SchoolRec);
            }

            return SchoolRecs; 
        }

        /// <summary>
        /// 取得學所有國中學校列表
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.School.SelectJuniorSchools", "學籍.國中列表")]
        public static List<SchoolRecord> SelectJuniorSchools()
        {
            List<SchoolRecord> SchoolRecs = new List<SchoolRecord>();

            // 讀取國中資料
            ConfigData cd = GlobalConfiguration["SchoolListJunior"];

            XmlDocument doc = new XmlDocument();
            
            doc.LoadXml(cd["XmlData"]);
            
            foreach (XmlElement xe in doc.SelectSingleNode("SchoolList"))
            {
                SchoolRecord SchoolRec = new SchoolRecord();
                SchoolRec.Load(xe);
                SchoolRecs.Add(SchoolRec);
            }

            return SchoolRecs;
        }

        /// <summary>
        /// 取得學校代碼。
        /// </summary>
        public static string Code
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "Code"); }
        }

        /// <summary>
        /// 取得預設學年度。
        /// </summary>
        public static string DefaultSchoolYear
        {
            get { return GetConfigurationString(ref SysConfig, "系統設定", "DefaultSchoolYear"); }
        }

        /// <summary>
        /// 取得預設學期。
        /// </summary>
        public static string DefaultSemester
        {
            get { return GetConfigurationString(ref SysConfig, "系統設定", "DefaultSemester"); }
        }

        /// <summary>
        /// 取得學校中文名稱。
        /// </summary>
        public static string ChineseName
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "ChineseName"); }
        }

        /// <summary>
        /// 取得學校英文名稱。
        /// </summary>
        public static string EnglishName
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "EnglishName"); }
        }

        /// <summary>
        /// 取得學校地址。
        /// </summary>
        public static string Address
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "Address"); }
        }

        /// <summary>
        /// 取得學生電話資料。
        /// </summary>
        public static string Telephone
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "Telephone"); }
        }

        /// <summary>
        /// 取得學校傳真。
        /// </summary>
        public static string Fax
        {
            get { return GetConfigurationString(ref SchoolConfig, "學校資訊", "Fax"); }
        }

        protected static string GetConfigurationString(ref XmlElement xmlObject, string configName, string xpath)
        {
            if (xmlObject == null)
            {
                Configuration.Cache.SyncData(configName);
                xmlObject = Configuration[configName].PreviousData;
            }

            if (xmlObject == null)
                throw new InvalidOperationException("系統規格可能已改變。");

            XmlElement n = xmlObject.SelectSingleNode(xpath) as XmlElement;

            if (n == null)
                return string.Empty;
            else
                return n.InnerText;
        }
    }
}
