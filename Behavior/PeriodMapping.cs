using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 節次對照表類別，提供方法用來取得節次對照資訊
    /// </summary>
    public class PeriodMapping
    {
        private static string SELECT_SERVICENAME = "SmartSchool.Others.GetPeriodList";
        private static string UPDATE_SERVICENAME = "SmartSchool.Config.UpdateList";
        private static string LIST_PERIODS_NAME = "節次對照表";

        /// <summary>
        /// 取得所有節次對照表清單
        /// </summary>
        /// <returns>List&lt;PeriodMappingInfo&gt;，代表節次對照資訊物件列表。</returns>
        [SelectMethod("K12.PeriodMapping.SelectAll", "學務.節次對照表")]
        public static List<PeriodMappingInfo> SelectAll()
        {
            return K12.Data.PeriodMapping.SelectAll<PeriodMappingInfo>();
        }

        /// <summary>
        /// 取得節次對照表清單
        /// </summary>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:PeriodMappingInfo ,new()
        {
            StringBuilder req = new StringBuilder("<Request><Field><Content/><All/></Field></Request>");

            List<T> Types = new List<T>();

            foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Period"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            Types.Sort(new PeriodComparer<T>());

            return Types;
        }

        /// <summary>
        /// 更新多筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecords">多筆學生學期成績記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<PeriodMappingInfo> Records)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("AbsenceList");
            doc.AppendChild(root);

            foreach (PeriodMappingInfo Record in Records)
            {
                XmlElement period = doc.CreateElement("Period");
                root.AppendChild(period);
                period.SetAttribute("Name", Record.Name);
                period.SetAttribute("Type", Record.Type);
                period.SetAttribute("Sort", ""+Record.Sort);
                period.SetAttribute("Aggregated",""+Record.Aggregated);
            }

            DSXmlHelper helper = new DSXmlHelper("Lists");
            helper.AddElement("List");
            helper.AddElement("List", "Content", root.OuterXml, true);
            helper.AddElement("List", "Condition");
            helper.AddElement("List/Condition", "Name",LIST_PERIODS_NAME);

            int result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(Records.Select(x => x.Name).ToList(), ChangedSource.Local));

            return result;
        }

                /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;    
    }

    class PeriodComparer<T> : IComparer<T> where T:PeriodMappingInfo,new()
    {
        #region IComparer<PeriodInfo> 成員

        public int Compare(T x, T y)
        {
            if (x.Sort == y.Sort) return 0;
            else if (x.Sort > y.Sort) return 1;
            else return -1;
        }

        #endregion
    }
}