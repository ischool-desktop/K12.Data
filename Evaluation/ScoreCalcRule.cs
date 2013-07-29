using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 成績計算規則類別，適用於學生及班級，提供方法用來取得、新增、修改及刪除成績計算規則資訊
    /// </summary>
    public class ScoreCalcRule
    {
        private const string SELECT_SERVICENAME = "SmartSchool.ScoreCalcRule.GetScoreCalcRule";
        private const string INSERT_SERVICENAME = "SmartSchool.ScoreCalcRule.InsertScoreCalcRule";
        private const string UPDATE_SERVICENAME = "SmartSchool.ScoreCalcRule.UpdateScoreCalcRule";
        private const string DELETE_SERVICENAME = "SmartSchool.ScoreCalcRule.DeleteScoreCalcRule";

        /// <summary>
        /// 取得所有成績計算規則列表。
        /// </summary>
        /// <returns>List&lt;ScoreCalcRuleRecord&gt;，代表多筆成績計算規則記錄物件。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ScoreCalcRuleRecord&gt; scorecalcrulerecords = ScoreCalcRule.SelectAll();
        ///     </code>
        /// </example>
        [SelectMethod("K12.ScoreCalcRule.SelectAll", "成績.成績計算規則")]
        public static List<ScoreCalcRuleRecord> SelectAll()
        {
            return SelectAll<K12.Data.ScoreCalcRuleRecord>();
        }

        /// <summary>
        /// 取得所有成績計算規則列表。
        /// </summary>
        /// <returns>List&lt;ScoreCalcRuleRecord&gt;，代表多筆成績計算規則記錄物件。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ScoreCalcRuleRecord&gt; scorecalcrulerecords = ScoreCalcRule.SelectAll();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:ScoreCalcRuleRecord ,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            List<T> Types = new List<T>();

            request.AddElement(".", "Field", "<ID/><Name/><Content/>", true);
            request.AddElement(".", "Order", "<Name/>", true);

            foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("ScoreCalcRule"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 依學生系統編號取得成績計算規則
        /// </summary>
        /// <param name="StudentIDs"></param>
        /// <returns></returns>
        public static Dictionary<string, ScoreCalcRuleRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<ScoreCalcRuleRecord>(StudentIDs);
        }

        /// <summary>
        /// 依學生系統編號取得成績計算規則
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StudentIDs"></param>
        /// <returns></returns>
        protected static Dictionary<string,T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:ScoreCalcRuleRecord,new()
        {
            //Key為學生系統編號，Value為成績計算規則物件
            Dictionary<string, T> StudentScoreCalcRules = new Dictionary<string, T>();

            #region 取得學生、班級及成績計算規則資料
            List<StudentRecord> Students = Student
                .SelectByIDs(StudentIDs); //依學生系統編號取得學生
            
            Dictionary<string,ClassRecord> Classes = Class
                .SelectByIDs(Students.Select(x => x.RefClassID).Distinct())
                .ToDictionary(x => x.ID);                                   //依班級編號取得班級並整理成Dictionary
            
            Dictionary<string, T> ScoreCalcRules = SelectAll<T>()
                .ToDictionary(x => x.ID);                                   //取得所有的成績計算規則，並依系統編號整理成Dictionary
            #endregion

            //針對每位學生
            foreach (StudentRecord StudentRec in Students)
            {
                //若學生身上有成績計算規則就用學生身上的
                if (!string.IsNullOrEmpty(StudentRec.OverrideScoreCalcRuleID))
                {
                    //判斷是成績計算規則是否存在
                    if (ScoreCalcRules.ContainsKey(StudentRec.OverrideScoreCalcRuleID))
                        if (!StudentScoreCalcRules.ContainsKey(StudentRec.ID))
                            StudentScoreCalcRules.Add(StudentRec.ID,ScoreCalcRules[StudentRec.OverrideScoreCalcRuleID]);
                }
                else if (!string.IsNullOrEmpty(StudentRec.RefClassID)) //否則就用班級的
                {
                    if (Classes.ContainsKey(StudentRec.RefClassID))
                        if (ScoreCalcRules.ContainsKey(Classes[StudentRec.RefClassID].RefScoreCalcRuleID))
                            if (!StudentScoreCalcRules.ContainsKey(StudentRec.ID))
                                StudentScoreCalcRules.Add(StudentRec.ID,ScoreCalcRules[Classes[StudentRec.RefClassID].RefScoreCalcRuleID]);
                }
            }

            return StudentScoreCalcRules;
        }

        /// <summary>
        /// 根據多筆成績計算規則編號取得成績計算規則列表。
        /// </summary>
        /// <param name="ScoreCalcRuleIDs">多筆成績計算規則編號</param>
        /// <returns>List&lt;ScoreCalcRuleRecord&gt;，代表多筆成績計算規則記錄物件。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ScoreCalcRuleRecord&gt; scorecalcrulerecords = ScoreCalcRule.SelectByIDs(ScoreCalcRuleIDs);
        ///     </code>
        /// </example>
        public static List<ScoreCalcRuleRecord> SelectByIDs(IEnumerable<string> ScoreCalcRuleIDs)
        {
            return SelectByIDs<K12.Data.ScoreCalcRuleRecord>(ScoreCalcRuleIDs);
        }

        /// <summary>
        /// 根據單筆成績計算規則編號取得成績計算規則物件。
        /// </summary>
        /// <param name="ScoreCalcRuleID">成績計算規則編號</param>
        /// <returns>ScoreCalcRuleRecord，成績計算規則物件</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        public static ScoreCalcRuleRecord SelectByID(string ScoreCalcRuleID)
        {
            return SelectByID<ScoreCalcRuleRecord>(ScoreCalcRuleID);
        }

        protected static T SelectByID<T>(string ScoreCalcRuleID) where T : ScoreCalcRuleRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ScoreCalcRuleID);

            List<T> Types = SelectByIDs<T>(IDs);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆成績計算規則編號取得成績計算規則列表。
        /// </summary>
        /// <param name="ScoreCalcRuleIDs">多筆成績計算規則編號</param>
        /// <returns>List&lt;ScoreCalcRuleRecord&gt;，代表多筆成績計算規則記錄物件。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ScoreCalcRuleRecord&gt; scorecalcrulerecords = ScoreCalcRule.SelectByIDs(ScoreCalcRuleIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> ScoreCalcRuleIDs) where T:ScoreCalcRuleRecord ,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            List<T> Types = new List<T>();
            bool execute_required = false;

            request.AddElement(".", "Field", "<ID/><Name/><Content/>", true);
            request.AddElement(".", "Order", "<Name/>", true);

            request.AddElement(".", "Condition", "<IDList/>", true);
            foreach (string each in ScoreCalcRuleIDs)
            {
                request.AddElement("Condition/IDList", "ID", each);
                execute_required = true;
            }

            if (execute_required)
            {
                foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("ScoreCalcRule"))
                {
                    T Type = new T();
                    Type.Load(each);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 新增單筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecord">成績計算規則記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     </code>
        /// </example>
        public static string Insert(ScoreCalcRuleRecord ScoreCalcRuleRecord)
        {
            List<ScoreCalcRuleRecord> Params = new List<ScoreCalcRuleRecord>();

            Params.Add(ScoreCalcRuleRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecords">多筆成績計算規則記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<ScoreCalcRuleRecord> ScoreCalcRuleRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<ScoreCalcRuleRecord> worker = new MultiThreadWorker<ScoreCalcRuleRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ScoreCalcRuleRecord> e)
            {
                DSXmlHelper insertReq = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    insertReq.AddElement("ScoreCalcRule");
                    insertReq.AddElement("ScoreCalcRule", "Name", editor.Name);
                    insertReq.AddElement("ScoreCalcRule", "Content");
                    if (editor.Content != null)
                        insertReq.AddElement("ScoreCalcRule/Content", editor.Content);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(insertReq.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<ScoreCalcRuleRecord>> packages = worker.Run(ScoreCalcRuleRecords);

            foreach (PackageWorkEventArgs<ScoreCalcRuleRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecord">成績計算規則記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        public static int Update(ScoreCalcRuleRecord ScoreCalcRuleRecord)
        {
            List<ScoreCalcRuleRecord> Params = new List<ScoreCalcRuleRecord>();

            Params.Add(ScoreCalcRuleRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecords">多筆成績計算規則記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ScoreCalcRuleRecord> ScoreCalcRuleRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ScoreCalcRuleRecord> worker = new MultiThreadWorker<ScoreCalcRuleRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ScoreCalcRuleRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("ScoreCalcRule");
                    updateHelper.AddElement("ScoreCalcRule", "Field");
                    updateHelper.AddElement("ScoreCalcRule/Field", "Name", editor.Name);
                    updateHelper.AddElement("ScoreCalcRule/Field", "Content");
                    if (editor.Content != null)
                        updateHelper.AddElement("ScoreCalcRule/Field/Content", editor.Content);
                    updateHelper.AddElement("ScoreCalcRule", "Condition");
                    updateHelper.AddElement("ScoreCalcRule/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ScoreCalcRuleRecord>> packages = worker.Run(ScoreCalcRuleRecords);

            foreach (PackageWorkEventArgs<ScoreCalcRuleRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecord">成績計算規則記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        static public int Delete(ScoreCalcRuleRecord ScoreCalcRuleRecord)
        {
            return Delete(ScoreCalcRuleRecord.ID);
        }


        /// <summary>
        /// 刪除單筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleID">成績計算規則編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        static public int Delete(string ScoreCalcRuleID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(ScoreCalcRuleID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleRecords">多筆成績計算規則記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ScoreCalcRuleRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     
        ///     </code>
        /// </example>
        static public int Delete(IEnumerable<ScoreCalcRuleRecord> ScoreCalcRuleRecords)
        {
            List<string> Keys = new List<string>();

            foreach (ScoreCalcRuleRecord ScoreCalcRuleRecord in ScoreCalcRuleRecords)
                Keys.Add(ScoreCalcRuleRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆成績計算規則記錄
        /// </summary>
        /// <param name="ScoreCalcRuleIDs">多筆成績計算規則編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> ScoreCalcRuleIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (string Key in e.List)
                {
                    helper.AddElement("ScoreCalcRule");
                    helper.AddElement("ScoreCalcRule", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(ScoreCalcRuleIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(ScoreCalcRuleIDs, ChangedSource.Local));

            return result;
        }

        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        static public event EventHandler<DataChangedEventArgs> AfterDelete;
    }
}