using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 評分樣板類別，提供方法用來取得、新增、修改及刪除評分樣板資訊
    /// </summary>
    public class AEInclude
    {
        private const string SELECT_SERVICENAME = "SmartSchool.ExamTemplate.GetIncludeExamList";
        private const string UPDATE_SERVICENAME = "SmartSchool.ExamTemplate.UpdateIncludeExam";
        private const string INSERT_SERVICENAME = "SmartSchool.ExamTemplate.InsertIncludeExam";
        private const string DELETE_SERVICENAME = "SmartSchool.ExamTemplate.DeleteIncludeExam";

        /// <summary>
        /// 取得所有評分樣板列表。
        /// </summary>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectAll();
        /// </example>
        [SelectMethod("K12.AEInclude.SelectAll", "成績.評分樣板")]
        public static List<AEIncludeRecord> SelectAll()
        {
            return SelectAll<K12.Data.AEIncludeRecord>();
        }

        
        /// <summary>
        /// 取得所有評分樣板列表。
        /// </summary>
        /// <typeparam name="T">評分樣板記錄物件型別，K12共用為K12.Data.AEIncludeRecord</typeparam>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectAll&lt;K12.Data.AEIncludeRecord&gt;();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:AEIncludeRecord , new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");

            List<T> Types = new List<T>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("IncludeExam"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆評分樣板編號取得評分樣板列表。
        /// </summary>
        /// <param name="AEIncludeIDs">多筆評分樣板編號</param>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectByIDs(AEIncludeIDs);
        /// </example>
        public static List<AEIncludeRecord> SelectByIDs(IEnumerable<string> AEIncludeIDs)
        {
            return SelectByIDs<K12.Data.AEIncludeRecord>(AEIncludeIDs);
        }

        /// <summary>
        /// 根據多筆評分樣板編號取得評分樣板列表。
        /// </summary>
        /// <typeparam name="T">評分樣板記錄物件型別，K12共用為K12.Data.AEIncludeRecord</typeparam>
        /// <param name="AEIncludeIDs">多筆評分樣板編號</param>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectByIDs&lt;K12.Data.AEIncludeRecord&gt;(AEIncludeIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> AEIncludeIDs) where T:AEIncludeRecord,new()
        {
            bool hasKey = false;

            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            foreach (var item in AEIncludeIDs)
            {
                helper.AddElement("Condition", "ID", item);
                hasKey = true;
            }
            if (hasKey)
            {
                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("IncludeExam"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        /// <summary>
        /// 根據單筆評量設定編號取得評分樣板列表。
        /// </summary>
        /// <param name="AssessmentSetupID">單筆評量設定編號</param>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectByIDs(AEIncludeIDs);
        /// </example>
        public static List<AEIncludeRecord> SelectByAssessmentSetupID(string AssessmentSetupID)
        {
            return SelectByAssessmentSetupID<AEIncludeRecord>(AssessmentSetupID); 
        }

        protected static List<T> SelectByAssessmentSetupID<T>(string AssessmentSetupID) where T:AEIncludeRecord ,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(AssessmentSetupID);

            return SelectByAssessmentSetupIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆單筆評量設定編號取得評分樣板列表。
        /// </summary>
        /// <param name="AssessmentSetupIDs">多筆評量設定編號</param>
        /// <returns>List&lt;AEIncludeRecord&gt;，代表多筆評分樣板記錄物件。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AEIncludeRecord&gt; aeincluderecords = AEInclude.SelectByIDs(AEIncludeIDs);
        /// </example>
        public static List<AEIncludeRecord> SelectByAssessmentSetupIDs(IEnumerable<string> AssessmentSetupIDs)
        {
            return SelectByAssessmentSetupIDs<AEIncludeRecord>(AssessmentSetupIDs);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByAssessmentSetupIDs<T>(IEnumerable<string> AssessmentSetupIDs) where T:AEIncludeRecord,new()
        {
            bool hasKey = false;

            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            foreach (var item in AssessmentSetupIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    helper.AddElement("Condition", "ExamTemplateID", item);
                    hasKey = true;
                }
            }

            if (hasKey)
            {
                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("IncludeExam"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        /// <summary>
        /// 新增單筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecord">評分樣板記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(AEIncludeRecord AEIncludeRecord)
        {
            List<AEIncludeRecord> Params = new List<AEIncludeRecord>();

            Params.Add(AEIncludeRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecords">多筆評分樣板記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<AEIncludeRecord> AEIncludeRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<AEIncludeRecord> worker = new MultiThreadWorker<AEIncludeRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AEIncludeRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("InsertRequest");

                foreach (var editor in e.List)
                {
                    helper.AddElement("IncludeExam");
                    helper.AddElement("IncludeExam", "RefExamTemplateID", editor.RefAssessmentSetupID);
                    helper.AddElement("IncludeExam", "RefExamID", editor.RefExamID);
                    helper.AddElement("IncludeExam", "UseScore", editor.UseScore ? "是" : "否");
                    helper.AddElement("IncludeExam", "UseText", editor.UseText ? "是" : "否");

                    helper.AddElement("IncludeExam", "Weight", "" + editor.Weight);
                    helper.AddElement("IncludeExam", "StartTime", editor.StartTime);
                    helper.AddElement("IncludeExam", "EndTime", editor.EndTime);
                    helper.AddElement("IncludeExam", "OpenTeacherAccess", (editor.OpenTeacherAccess==true)?"是":"否");
                    helper.AddElement("IncludeExam", "InputRequired", (editor.InputRequired==true)?"是":"否");

                    helper.AddElement("IncludeExam", "Extension");
                    helper.AddElement("IncludeExam/Extension" , editor.Extension);

                    #region 參考
                    //<InsertIncludeExamRequest>
                    //   <IncludeExam>
                    //      <RefExamTemplateID>integer</RefExamTemplateID>
                    //      <RefExamID>integer</RefExamID>
                    //      <UseScore>是否</UseScore>
                    //      <UseText>是否</UseText>
                    //      <Weight>integer</Weight>
                    //      <OpenTeacherAccess>是否</OpenTeacherAccess>
                    //      <StartTime>timestamp</StartTime>
                    //      <EndTime>timestamp</EndTime>
                    //      <InputRequired/>
                    //   </IncludeExam>
                    //</InsertIncludeExamRequest>
                    #endregion
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<AEIncludeRecord>> packages = worker.Run(AEIncludeRecords);

            foreach (PackageWorkEventArgs<AEIncludeRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecord">評分樣板記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(AEIncludeRecord AEIncludeRecord)
        {
            List<AEIncludeRecord> Params = new List<AEIncludeRecord>();

            Params.Add(AEIncludeRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecords">多筆評分樣板記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<AEIncludeRecord> AEIncludeRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<AEIncludeRecord> worker = new MultiThreadWorker<AEIncludeRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AEIncludeRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateRequest");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("IncludeExam");
                    updateHelper.AddElement("IncludeExam", "RefExamTemplateID", editor.RefAssessmentSetupID);
                    updateHelper.AddElement("IncludeExam", "RefExamID", editor.RefExamID);
                    updateHelper.AddElement("IncludeExam", "UseScore", editor.UseScore ? "是" : "否");
                    updateHelper.AddElement("IncludeExam", "UseText", editor.UseText ? "是" : "否");
                    
                    updateHelper.AddElement("IncludeExam", "Weight", "" + editor.Weight);
                    updateHelper.AddElement("IncludeExam", "StartTime", editor.StartTime);
                    updateHelper.AddElement("IncludeExam", "EndTime", editor.EndTime);

                    updateHelper.AddElement("IncludeExam", "Extension");
                    updateHelper.AddElement("IncludeExam/Extension",editor.Extension);

                    updateHelper.AddElement("IncludeExam", "Condition");
                    updateHelper.AddElement("IncludeExam/Condition", "ID", editor.ID);

                    #region 參考
                    //<UpdateIncludeExamRequest>
                    //   <IncludeExam>
                    //      <RefExamID>integer</RefExamID>
                    //      <RefExamTemplateID></RefExamTemplateID>
                    //      <UseScore>是否</UseScore>
                    //      <UseText>是否</UseText>
                    //      <Weight>integer</Weight>
                    //      <OpenTeacherAccess>是否</OpenTeacherAccess>
                    //      <StartTime>timestamp</StartTime>
                    //      <EndTime>timestamp</EndTime>
                    //      <Condition>
                    //         <ID>integer</ID>
                    //      </Condition>
                    //      <InputRequired/>
                    //   </IncludeExam>
                    //</UpdateIncludeExamRequest>
                    #endregion

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<AEIncludeRecord>> packages = worker.Run(AEIncludeRecords);

            foreach (PackageWorkEventArgs<AEIncludeRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            
            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecord">評分樣板記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(AEIncludeRecord AEIncludeRecord)
        {
            return Delete(AEIncludeRecord.ID);
        }

        /// <summary>
        /// 刪除單筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecordID">評分樣板記錄編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string AEIncludeRecordID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(AEIncludeRecordID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecords">多筆評分樣板記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AEIncludeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<AEIncludeRecord> AEIncludeRecords)
        {
            List<string> Keys = new List<string>();

            foreach (AEIncludeRecord AEIncludeRecord in AEIncludeRecords)
                Keys.Add(AEIncludeRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆評分樣板記錄
        /// </summary>
        /// <param name="AEIncludeRecordIDs">多筆評分樣板記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> AEIncludeRecordIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("DeleteRequest");

                helper.AddElement("IncludeExam");

                foreach (string Key in e.List)
                    helper.AddElement("IncludeExam", "ID", Key);

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(AEIncludeRecordIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(AEIncludeRecordIDs, ChangedSource.Local));

            return result;
        }

        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        static public event EventHandler<DataChangedEventArgs> AfterDelete;
    }

    //public static class AEInclude_ExtendMethods
    //{
    //    public static List<AEIncludeRecord> GetAEIncludes(this AssessmentSetupRecord record)
    //    {
    //        List<AEIncludeRecord> result = new List<AEIncludeRecord>();
    //        foreach (var item in AEInclude.Instance.Items)
    //        {
    //            if (item.RefAssessmentSetupID == record.ID)
    //                result.Add(item);
    //        }
    //        return result;
    //    }
    //}
}