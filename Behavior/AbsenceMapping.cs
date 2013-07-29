using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;
using System;
using System.Linq;

namespace K12.Data
{
    /// <summary>
    /// 假別對照類別，提供方法用來取得假別對照資訊
    /// </summary>
    public class AbsenceMapping
    {
        private static string GET_ABSENCE_LIST = "SmartSchool.Others.GetAbsenceList";
        private static string UPDATE_SERVICENAME = "SmartSchool.Config.UpdateList";
        private static string LIST_ABSENCES_NAME = "假別對照表";

        /// <summary>
        /// 取得所有假別對照資訊
        /// </summary>
        /// <returns>List&lt;AbsenceMappingInfo&gt;，代表假別對照資訊物件列表。</returns>
        [SelectMethod("K12.AbsenceMapping.SelectAll", "學務.假別對照表")]
        public static List<AbsenceMappingInfo> SelectAll()
        {
            return SelectAll<AbsenceMappingInfo>();
        }

        /// <summary>
        /// 取得所有假別對照資訊
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:AbsenceMappingInfo,new()
        {
            StringBuilder req = new StringBuilder("<Request><Field><Content/><All/></Field></Request>");
            
            List<T> Types = new List<T>();

            foreach (XmlElement each in DSAServices.CallService(GET_ABSENCE_LIST, new DSRequest(req.ToString())).GetContent().GetElements("Absence"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 假別對照表
        /// </summary>
        /// <param name="Records"></param>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<AbsenceMappingInfo> Records)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("AbsenceList");
            doc.AppendChild(root);

            foreach (AbsenceMappingInfo Record in Records)
            {
                XmlElement period = doc.CreateElement("Absence");
                root.AppendChild(period);
                period.SetAttribute("Name", "" + Record.Name);
                period.SetAttribute("Abbreviation", Record.Abbreviation);
                period.SetAttribute("HotKey", Record.HotKey);
                period.SetAttribute("Noabsence", "" + Record.Noabsence);
            }

            DSXmlHelper helper = new DSXmlHelper("Lists");
            helper.AddElement("List");
            helper.AddElement("List", "Content", root.OuterXml, true);
            helper.AddElement("List", "Condition");
            helper.AddElement("List/Condition", "Name", LIST_ABSENCES_NAME);

            int result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(Records.Select(x=>x.Name).ToList(), ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;
    }
}