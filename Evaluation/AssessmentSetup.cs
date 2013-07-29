using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 評量設定類別，提供方法用來取得、新增、修改及刪除評量設定資訊
    /// </summary>
    public class AssessmentSetup
    {
        private const string SELECT_SERVICENAME = "SmartSchool.ExamTemplate.GetAbstractList";
        private const string UPDATE_SERVICENAME = "SmartSchool.ExamTemplate.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.ExamTemplate.Insert";
        private const string DELETE_SERVICENAME = "SmartSchool.ExamTemplate.Delete";

        /// <summary>
        /// 取得所有評量設定列表。
        /// </summary>
        /// <returns>List&lt;AssessmentSetupRecord&gt;，代表多筆評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectAll();
        /// </example>
        [SelectMethod("K12.AssessmentSetup.SelectAll", "成績.評量設定")]
        public static List<AssessmentSetupRecord> SelectAll()
        {
            return SelectAll<K12.Data.AssessmentSetupRecord>();
        }

        /// <summary>
        /// 取得所有評量設定列表。
        /// </summary>
        /// <returns>List&lt;AssessmentSetupRecord&gt;，代表多筆評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:AssessmentSetupRecord,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            List<T> Types = new List<T>();

            request.AddElement(".", "Field", "<All/>", true);

            foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("ExamTemplate"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據評量設定編號取得評量設定記錄物件。
        /// </summary>
        /// <param name="AssessmentSetupID">評量設定編號</param>
        /// <returns>AssessmentSetupRecord，代表評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectByID(AssessmentSetupID);
        /// </example>
        public static AssessmentSetupRecord SelectByID(string AssessmentSetupID)
        {
            return SelectByID<K12.Data.AssessmentSetupRecord>(AssessmentSetupID);
        }

        /// <summary>
        /// 根據評量設定編號取得評量設定記錄物件。
        /// </summary>
        /// <param name="AssessmentSetupID">評量設定編號</param>
        /// <returns>AssessmentSetupRecord，代表評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectByIDs(AssessmentSetupIDs);
        /// </example>
        protected static T SelectByID<T>(string AssessmentSetupID) where T:AssessmentSetupRecord ,new()       
        {
            List<string> IDs = new List<string>();

            IDs.Add(AssessmentSetupID);

            List<T> Records = SelectByIDs<T>(IDs);

            if (Records.Count > 0)
                return Records[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆評量設定編號取得評量設定列表。
        /// </summary>
        /// <param name="AssessmentSetupIDs">多筆評量設定編號</param>
        /// <returns>List&lt;AssessmentSetupRecord&gt;，代表多筆評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectByIDs(AssessmentSetupIDs);
        /// </example>
        public static List<AssessmentSetupRecord> SelectByIDs(IEnumerable<string> AssessmentSetupIDs)
        {
            return SelectByIDs<K12.Data.AssessmentSetupRecord>(AssessmentSetupIDs);
        }

        /// <summary>
        /// 根據多筆評量設定編號取得評量設定列表。
        /// </summary>
        /// <param name="AssessmentSetupIDs">多筆評量設定編號</param>
        /// <returns>List&lt;AssessmentSetupRecord&gt;，代表多筆評量設定記錄物件。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AssessmentSetupRecord&gt; records = AssessmentSetup.SelectByIDs(AssessmentSetupIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> AssessmentSetupIDs) where T : AssessmentSetupRecord , new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");

            List<T> Types = new List<T>();
            bool execute_required = false;

            request.AddElement(".", "Field", "<All/>", true);
            request.AddElement("Condition");

            foreach (string each in AssessmentSetupIDs)
            {
                request.AddElement("Condition", "ID", each);
                execute_required = true;
            }

            if (execute_required)
            {
                foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("ExamTemplate"))
                {
                    T Type = new T();
                    Type.Load(each);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 新增單筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecord">評量設定記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(AssessmentSetupRecord AssessmentSetupRecord)
        {
            List<AssessmentSetupRecord> Params = new List<AssessmentSetupRecord>();

            Params.Add(AssessmentSetupRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecords">多筆評量設定記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<AssessmentSetupRecord> AssessmentSetupRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<AssessmentSetupRecord> worker = new MultiThreadWorker<AssessmentSetupRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AssessmentSetupRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("ExamTemplate");
                    helper.AddElement("ExamTemplate", "TemplateName", editor.Name);
                    helper.AddElement("ExamTemplate", "Description", editor.Description);
                    helper.AddElement("ExamTemplate", "StartTime", editor.StartTime);
                    helper.AddElement("ExamTemplate", "EndTime", editor.EndTime);
                    helper.AddElement("ExamTemplate", "AllowUpload", (editor.AllowUpload==true)?"是":"否");                    
                    helper.AddElement("ExamTemplate", "Extension");
                    helper.AddElement("ExamTemplate/Extension", editor.Extension);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<AssessmentSetupRecord>> packages = worker.Run(AssessmentSetupRecords);

            foreach (PackageWorkEventArgs<AssessmentSetupRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecord">評量設定記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(AssessmentSetupRecord AssessmentSetupRecord)
        {
            List<AssessmentSetupRecord> Params = new List<AssessmentSetupRecord>();

            Params.Add(AssessmentSetupRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecords">多筆評量設定記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<AssessmentSetupRecord> AssessmentSetupRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<AssessmentSetupRecord> worker = new MultiThreadWorker<AssessmentSetupRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AssessmentSetupRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("ExamTemplate");
                    updateHelper.AddElement("ExamTemplate", "TemplateName", editor.Name);
                    updateHelper.AddElement("ExamTemplate", "Description", editor.Description);
                    updateHelper.AddElement("ExamTemplate", "StartTime", editor.Name);
                    updateHelper.AddElement("ExamTemplate", "EndTime", editor.Description);
                    updateHelper.AddElement("ExamTemplate", "AllowUpload", (editor.AllowUpload == true) ? "是" : "否");
                    updateHelper.AddElement("ExamTemplate", "Extension");
                    updateHelper.AddElement("ExamTemplate/Extension", editor.Extension);
                    updateHelper.AddElement("ExamTemplate", "Condition");
                    updateHelper.AddElement("ExamTemplate/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<AssessmentSetupRecord>> packages = worker.Run(AssessmentSetupRecords);

            foreach (PackageWorkEventArgs<AssessmentSetupRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecord">評量設定記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(AssessmentSetupRecord AssessmentSetupRecord)
        {
            return Delete(AssessmentSetupRecord.ID);
        }

        /// <summary>
        /// 刪除單筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecordID">評量設定記錄編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string AssessmentSetupID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(AssessmentSetupID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecords">多筆評量設定記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AssessmentSetupRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<AssessmentSetupRecord> AssessmentSetupRecords)
        {
            List<string> Keys = new List<string>();

            foreach (AssessmentSetupRecord AssessmentSetupRecord in AssessmentSetupRecords)
                Keys.Add(AssessmentSetupRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆評量設定記錄
        /// </summary>
        /// <param name="AssessmentSetupRecordIDs">多筆評量設定記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> AssessmentSetupIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("DeleteRequest");

                foreach (string Key in e.List)
                {
                    helper.AddElement("ExamTemplate");
                    helper.AddElement("ExamTemplate", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(AssessmentSetupIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(AssessmentSetupIDs, ChangedSource.Local));

            return result;
        }

        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        static public event EventHandler<DataChangedEventArgs> AfterDelete;
    }

    //public static class AssessmentSetup_ExtendMethods
    //{
    //    /// <summary>
    //    /// 取得課程的評分設定。
    //    /// </summary>
    //    /// <param name="course"></param>
    //    /// <returns></returns>
    //    public static AssessmentSetupRecord GetAssessmentSetup(this CourseRecord course)
    //    {
    //        if (course != null)
    //            return AssessmentSetup.Instance[course.RefAssessmentSetupID];
    //        else
    //            return null;
    //    }
    //}
}
