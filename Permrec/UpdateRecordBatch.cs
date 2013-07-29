using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 異動名冊記錄類別，提供方法用來取得、新增、修改及刪除異動名冊記錄物件
    /// </summary>
    public class UpdateRecordBatch
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.UpdateRecordBatch.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.UpdateRecordBatch.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.Student.UpdateRecordBatch.Insert";
        private const string DELETE_SERVICENAME = "SmartSchool.Student.UpdateRecordBatch.Delete";

        /// <summary>
        /// 取得所有異動名冊記錄列表。
        /// </summary>
        /// <returns>List&lt;UpdateRecordBatchRecord&gt;，代表多筆異動名冊記錄物件。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordBatchRecord&gt; records = UpdateRecordBatch.SelectAll();
        ///     
        ///     foreach(UpdateRecordBatchRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<UpdateRecordBatchRecord> SelectAll()
        {
            return SelectAll<UpdateRecordBatchRecord>();
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:UpdateRecordBatchRecord,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("All");

            DSRequest request = new DSRequest(helper.ToString());

            DSResponse response = DSAServices.CallService(SELECT_SERVICENAME,request);

            foreach (XmlElement item in response.GetContent().GetElements("UpdateRecordBatch"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據異動名冊記錄編號取得異動名冊記錄物件。
        /// </summary>
        /// <param name="UpdateRecordBatchID">異動名冊記錄編號</param>
        /// <returns>UpdateRecordBatchRecord，代表單筆異動名冊記錄編號。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     UpdateRecordBatchRecord record = UpdateRecordBatch.SelectByID("15");
        ///     
        ///     Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是UpdateRecordBatchID不存在則會傳回null。</remarks>
        public static UpdateRecordBatchRecord SelectByID(string UpdateRecordBatchID)
        {
            return SelectByID<UpdateRecordBatchRecord>(UpdateRecordBatchID);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static T SelectByID<T>(string UpdateRecordBatchID) where T:UpdateRecordBatchRecord,new()
        {
            if (string.IsNullOrEmpty(UpdateRecordBatchID))
                return null;

            List<string> IDs = new List<string>();

            IDs.Add(UpdateRecordBatchID);

            List<T> Types = SelectByIDs<T>(IDs);

            return Types.Count > 0 ? Types[0] : null;
        }

        /// <summary>
        /// 根據異動名冊記錄編號取得異動名冊記錄物件列表。
        /// </summary>
        /// <param name="UpdateRecordBatchIDs">多筆異動名冊記錄編號。</param>
        /// <returns>List&lt;UpdateRecordBatchRecord&gt;，代表多筆異動名冊記錄物件。</returns>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordBatchRecord&gt; records = UpdateRecordBatch.SelectByIDs(UpdateRecordBatchIDs);
        ///     
        ///     foreach(UpdateRecordBatchRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<UpdateRecordBatchRecord> SelectByIDs(IEnumerable<string> UpdateRecordBatchIDs)
        {
            return SelectByIDs<UpdateRecordBatchRecord>(UpdateRecordBatchIDs);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> UpdateRecordBatchIDs) where T:UpdateRecordBatchRecord,new()
        {
            bool haskey = false; 

            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("All");
            helper.AddElement("Condition");

            foreach(string UpdateRecordBatchID in UpdateRecordBatchIDs)
                if (!string.IsNullOrEmpty(UpdateRecordBatchID))
                {
                    haskey = true;
                    helper.AddElement("Condition", "ID", UpdateRecordBatchID);
                }

            if (haskey == true)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper.ToString())).GetContent().GetElements("UpdateRecordBatch"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據學年度及學期取得異動名冊記錄物件列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，若是傳入null則會傳回所有學年度資料。</param>
        /// <param name="Semester">學期，若是傳入null則會傳回所有學期資料。</param>
        /// <returns>List&lt;UpdateRecordBatchRecord&gt;，代表多筆異動名冊記錄物件。</returns>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordBatchRecord&gt; records = UpdateRecordBatch.SelectBySchoolYearAndSemester(UpdateRecordBatchIDs);
        ///     
        ///     foreach(UpdateRecordBatchRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是學年度及學期參數皆傳入null，則會傳回所有資料。</remarks>
        public static List<UpdateRecordBatchRecord> SelectBySchoolYearAndSemester(int? SchoolYear, int? Semester)
        {
            return SelectBySchoolYearAndSemester<UpdateRecordBatchRecord>(SchoolYear, Semester);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectBySchoolYearAndSemester<T>(int? SchoolYear,int? Semester) where T:UpdateRecordBatchRecord,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("All");
            helper.AddElement("Condition");

            if (SchoolYear != null)
                helper.AddElement("Condition","SchoolYear",K12.Data.Int.GetString(SchoolYear));

            if (Semester != null)
                helper.AddElement("Condition", "Semester", K12.Data.Int.GetString(Semester));

            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper.ToString())).GetContent().GetElements("UpdateRecordBatch"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types; 
        }

        /// <summary>
        /// 新增單筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecord">異動名冊記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         UpdateRecordBatchRecord newrecord = new UpdateRecordBatchRecord();
        ///         newrecord.Name ="test";
        ///         newrecord.SchoolYear = 95;
        ///         newrecord.Semester = 1;
        ///         strng NewID = UpdateRecordBatch.Insert(newrecord)
        ///         UpdateRecordBatchRecord record = UpdateRecordBatch.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為異動名冊名稱、學年度及學期。
        /// </remarks>
        public static string Insert(UpdateRecordBatchRecord UpdateRecordBatchRecord)
        {
            List<UpdateRecordBatchRecord> Params = new List<UpdateRecordBatchRecord>();

            Params.Add(UpdateRecordBatchRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecord">多筆異動名冊記錄</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         UpdateRecordBatchRecord record = new UpdateRecordBatchRecord();
        ///         
        ///         record.Name ="新生異動名冊";
        ///         record.SchoolYear = 96;
        ///         record.Semester = 1;
        ///         
        ///         List&lt;UpdateRecordBatchRecord&gt; records = new List&lt;UpdateRecordBatchRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = UpdateRecordBatch.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<UpdateRecordBatchRecord> UpdateRecordBatchRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<UpdateRecordBatchRecord> worker = new MultiThreadWorker<UpdateRecordBatchRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<UpdateRecordBatchRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("UpdateRecordBatch");
                    helper.AddElement("UpdateRecordBatch", "Name",editor.Name);
                    helper.AddElement("UpdateRecordBatch", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("UpdateRecordBatch", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("UpdateRecordBatch", "ADDate",K12.Data.DateTimeHelper.ToDisplayString(editor.ADDate));
                    helper.AddElement("UpdateRecordBatch", "ADNumber",editor.ADNumber);

                    if (editor.Content != null)
                    {
                        helper.AddElement("UpdateRecordBatch", "Content");
                        helper.AddElement("UpdateRecordBatch/Content", editor.Content);
                    }
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<UpdateRecordBatchRecord>> packages = worker.Run(UpdateRecordBatchRecords);

            foreach (PackageWorkEventArgs<UpdateRecordBatchRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));


            return result;
        }

        /// <summary>
        /// 更新單筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecord">異動名冊記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     UpdateRecordBatchRecord record = UpdateRecordBatch.SelectByID(UpdateRecordBatchID);
        ///     record.Name = "轉入異動名冊";
        ///     int UpdateCount = UpdateRecordBatch.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(UpdateRecordBatchRecord UpdateRecordBatchRecord)
        {
            List<UpdateRecordBatchRecord> Params = new List<UpdateRecordBatchRecord>();

            Params.Add(UpdateRecordBatchRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecords">多筆異動名冊記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     UpdateRecordBatchRecord record = UpdateRecordBatch.SelectByID(UpdateRecordBatchID);
        ///     record.Name = "畢業異動名冊";
        ///     List&lt;UpdateRecordBatchRecord&gt; records = new List&lt;UpdateRecordBatchRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = UpdateRecordBatch.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<UpdateRecordBatchRecord> UpdateRecordBatchRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<UpdateRecordBatchRecord> worker = new MultiThreadWorker<UpdateRecordBatchRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<UpdateRecordBatchRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("UpdateRecordBatch");
                    updateHelper.AddElement("UpdateRecordBatch", "Name", editor.Name);
                    updateHelper.AddElement("UpdateRecordBatch", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("UpdateRecordBatch", "Semester", K12.Data.Int.GetString(editor.Semester));
                    updateHelper.AddElement("UpdateRecordBatch", "ADDate", K12.Data.DateTimeHelper.ToDisplayString(editor.ADDate));
                    updateHelper.AddElement("UpdateRecordBatch", "ADNumber", editor.ADNumber);
                    updateHelper.AddElement("UpdateRecordBatch", "Content");
                    if (editor.Content!=null)
                        updateHelper.AddElement("UpdateRecordBatch/Content",editor.Content);

                    updateHelper.AddElement("UpdateRecordBatch", "Condition");
                    updateHelper.AddElement("UpdateRecordBatch/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<UpdateRecordBatchRecord>> packages = worker.Run(UpdateRecordBatchRecords);

            foreach (PackageWorkEventArgs<UpdateRecordBatchRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }


        /// <summary>
        /// 刪除多筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecords">多筆異動名冊記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="UpdateRecordBatchRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;UpdateRecordBatchRecord&gt; records = UpdateRecordBatch.SelectByIDs(UpdateRecordIDs);
        ///       int DeleteCount = UpdateRecordBatch.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<UpdateRecordBatchRecord> UpdateRecordBatchRecords)
        {
            List<string> Keys = new List<string>();

            foreach (UpdateRecordBatchRecord UpdateRecordBatchRecord in UpdateRecordBatchRecords)
                Keys.Add(UpdateRecordBatchRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchIDs">多筆異動名冊記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///    <code>
        ///    int DeleteCount = UpdateRecordBatch.Delete(UpdateRecordBatchIDs);
        ///    </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> UpdateRecordBatchIDs)
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
                    helper.AddElement("UpdateRecordBatch");
                    helper.AddElement("UpdateRecordBatch", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(UpdateRecordBatchIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(UpdateRecordBatchIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(UpdateRecordBatchIDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchRecord">單筆異動名冊記錄物</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       UpdateRecordBatchRecord&gt; record = UpdateRecordBatch.SelectByID(UpdateRecordBatchID);
        ///       int DeleteCount = UpdateRecordBatch.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(UpdateRecordBatchRecord UpdateRecordBatchRecord)
        {
            return Delete(UpdateRecordBatchRecord.ID);
        }

        /// <summary>
        /// 刪除單筆異動名冊記錄
        /// </summary>
        /// <param name="UpdateRecordBatchID">單筆異動名冊記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = UpdateRecordBatch.Delete(UpdateRecordID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string UpdateRecordBatchID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(UpdateRecordBatchID);

            return Delete(Keys);
        }

        /// <summary>
        /// 新增之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        /// <summary>
        /// 刪除之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterDelete;

        /// <summary>
        /// 資料改變之後所觸發的事件，新增、更新、刪除都會觸發
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterChange;
    }
}