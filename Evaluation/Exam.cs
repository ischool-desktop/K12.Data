using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 試別項目類別，提供方法用來取得、新增、修改及刪除試別項目資訊
    /// </summary>
    public class Exam
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Exam.GetAbstractList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Exam.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.Exam.Insert";
        private const string DELETE_SERVICENAME = "SmartSchool.Exam.Delete";

        /// <summary>
        /// 取得所有試別項目列表。
        /// </summary>
        /// <returns>List&lt;ExamRecord&gt;，代表多筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ExamRecord&gt; records = Exam.SelectAll();
        /// </example>
        [SelectMethod("K12.Exam.SelectAll", "成績.試別")]
        public static List<ExamRecord> SelectAll()
        {
            return SelectAll<K12.Data.ExamRecord>();
        }

        /// <summary>
        /// 取得所有試別項目列表。
        /// </summary>
        /// <returns>List&lt;ExamRecord&gt;，代表多筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ExamRecord&gt; records = Exam.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:ExamRecord,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");

            List<T> Types = new List<T>();

            request.AddElement(".", "Field", "<All/>", true);

            foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("Exam"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆試別項目編號取得試別項目。
        /// </summary>
        /// <param name="ExamID">單筆試別項目編號</param>
        /// <returns>ExamRecord，代表單筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     ExamRecord record = Exam.SelectByID(ExamID);
        /// </example>
        public static ExamRecord SelectByID(string ExamID)
        {
            return SelectByID<K12.Data.ExamRecord>(ExamID);
        }

        /// <summary>
        /// 根據單筆試別項目編號取得試別項目。
        /// </summary>
        /// <param name="ExamID">單筆試別項目編號</param>
        /// <returns>ExamRecord，代表單筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     ExamRecord record = Exam.SelectByID(ExamID);
        /// </example>
        protected static T SelectByID<T>(string ExamID) where T:ExamRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ExamID);

            List<T> examRecords = SelectByIDs<T>(IDs);

            if (examRecords.Count > 0)
                return examRecords[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆試別項目編號取得試別項目列表。
        /// </summary>
        /// <param name="ExamIDs">多筆試別項目編號</param>
        /// <returns>List&lt;ExamRecord&gt;，代表多筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ExamRecord&gt; records = Exam.SelectByIDs(ExamIDs);
        /// </example>
        public static List<ExamRecord> SelectByIDs(IEnumerable<string> ExamIDs)
        {
            return SelectByIDs<K12.Data.ExamRecord>(ExamIDs);
        }
        
        /// <summary>
        /// 根據多筆試別項目編號取得試別項目列表。
        /// </summary>
        /// <param name="ExamIDs">多筆試別項目編號</param>
        /// <returns>List&lt;ExamRecord&gt;，代表多筆試別項目記錄物件。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ExamRecord&gt; records = Exam.SelectByIDs(ExamIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> ExamIDs) where T:ExamRecord,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");

            List<T> Types = new List<T>();

            bool execute_required = false;

            request.AddElement(".", "Field", "<All/>", true);
            request.AddElement("Condition");

            foreach (string each in ExamIDs)
            {
                request.AddElement("Condition", "ID", each);
                execute_required = true;
            }

            if (execute_required)
            {
                foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("Exam"))
                {
                    T Type = new T();
                    Type.Load(each);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 新增單筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecord">試別項目記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(ExamRecord ExamRecord)
        {
            List<ExamRecord> Params = new List<ExamRecord>();

            Params.Add(ExamRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecords">多筆試別項目記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<ExamRecord> ExamRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<ExamRecord> worker = new MultiThreadWorker<ExamRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ExamRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Exam");
                    helper.AddElement("Exam", "ExamName", editor.Name);
                    helper.AddElement("Exam", "Description", editor.Description);
                    helper.AddElement("Exam", "DisplayOrder", "" + editor.DisplayOrder);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<ExamRecord>> packages = worker.Run(ExamRecords);

            foreach (PackageWorkEventArgs<ExamRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecord">試別項目記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(ExamRecord ExamRecord)
        {
            List<ExamRecord> Params = new List<ExamRecord>();

            Params.Add(ExamRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecords">多筆試別項目記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ExamRecord> ExamRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ExamRecord> worker = new MultiThreadWorker<ExamRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ExamRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Exam");
                    updateHelper.AddElement("Exam", "ExamName", editor.Name);
                    updateHelper.AddElement("Exam", "Description", editor.Description);
                    updateHelper.AddElement("Exam", "DisplayOrder", "" + editor.DisplayOrder);

                    updateHelper.AddElement("Exam", "Condition");
                    updateHelper.AddElement("Exam/Condition", "ID", editor.ID);


                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ExamRecord>> packages = worker.Run(ExamRecords);

            foreach (PackageWorkEventArgs<ExamRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecord">試別項目記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(ExamRecord ExamRecord)
        {
            return Delete(ExamRecord.ID);
        }

        /// <summary>
        /// 刪除單筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecordID">試別項目記錄編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string ExamID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(ExamID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecords">多筆試別項目記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ExamRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<ExamRecord> ExamRecords)
        {
            List<string> Keys = new List<string>();

            foreach (ExamRecord ExamRecord in ExamRecords)
                Keys.Add(ExamRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆試別項目記錄
        /// </summary>
        /// <param name="ExamRecordIDs">多筆試別項目記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> ExamIDs)
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
                    helper.AddElement("Exam");
                    helper.AddElement("Exam", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);
            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(ExamIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(ExamIDs, ChangedSource.Local));

            return result;
        }

        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        static public event EventHandler<DataChangedEventArgs> AfterDelete;

    }
}