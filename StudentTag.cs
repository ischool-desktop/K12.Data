using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生標籤類別，提供方法用來取得、新增、修改及刪除學生標籤資訊
    /// </summary>
    public class StudentTag
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Tag.GetDetailListByStudent";
        private const string INSERT_SERVICENAME = "SmartSchool.Tag.AddStudentTag";
        private const string UPDATE_SERVICENAME = "SmartSchool.Tag.UpdateStudentTag";
        private const string DELETE_SERVICENAME = "SmartSchool.Tag.RemoveStudentTag";
        private static EntityCache<XmlElement> EntityCache;

        static StudentTag()
        {
            EntityCache = new EntityCache<XmlElement>();
            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有學生標籤列表。
        /// </summary>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectAll();
        ///         
        ///         foreach(StudentTagRecord record in records)
        ///             System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        [SelectMethod("K12.StudentTag.SelectAll", "學籍.學生類別")]
        public static List<StudentTagRecord> SelectAll()
        {
            return SelectAll<StudentTagRecord>();
        }

        /// <summary>
        /// 取得所有學生標籤列表。
        /// </summary>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentTagRecord&gt; records = StudentTag.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : StudentTagRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagStudentID");

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagStudentID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        /// <summary>
        /// 根據單筆學生編號取得學生標籤列表。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);
        ///         
        ///         foreach(StudentTagRecord record in records)
        ///           System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<StudentTagRecord> SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<StudentTagRecord>(StudentID);
        }


        /// <summary>
        /// 根據單筆學生編號取得學生標籤列表。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);
        /// </example>
        protected static List<T> SelectByStudentID<T>(string StudentID) where T : StudentTagRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(StudentID);

            return SelectByStudentIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生標籤列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentIDs(StudentIDs);
        ///     
        ///     forech(StudentTagRecord record in records)
        ///         System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<StudentTagRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<StudentTagRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生標籤列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentTagRecord&gt;，代表多筆學生標籤物件。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassIDs(ClassIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : StudentTagRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            foreach (string each in StudentIDs)
                helper.AddElement("Condition", "StudentID", each);

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagStudentID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }
        
        private static List<XmlElement> GetCacheData(IEnumerable<string> IDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(IDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheRecords = GetDirectData(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheRecords);

                foreach (XmlElement Record in NoCacheRecords)
                    if (!EntityCache.ContainsKey(Record.GetAttribute("TagStudentID")))
                        EntityCache.Add(Record.GetAttribute("TagStudentID"), Record);
            }

            return CacheSet.Records;
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> IDs)
        {
            List<XmlElement> result = new List<XmlElement>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            foreach (string Key in IDs)
                helper.AddElement("Condition", "TagStudentID", Key);


            DSRequest dsreq = new DSRequest(helper);

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Tag"))
                result.Add(item);

            return result;
        }

        /// <summary>
        /// 新增單學生標籤記錄
        /// </summary>
        /// <param name="StudentTagRecord">學生標籤記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     StudentTagRecord  record = new StudentTagRecord(StudentID, TagConfigID); 
        ///     string NewID = StudentTag.Insert(record);  
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為學生編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks> 
        public static string Insert(StudentTagRecord StudentTagRecord)
        {
            List<StudentTagRecord> Params = new List<StudentTagRecord>();

            Params.Add(StudentTagRecord);

            List<string> Result = Insert(Params);

            return Result[0];
        }

        /// <summary>
        /// 新增多筆學生標籤記錄
        /// </summary>
        /// <param name="StudentTagRecords">多筆學生標籤記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     StudentTagRecord record = new StudentTagRecord(StudentID, TagConfigID); 
        ///     List&lt;StudentTagRecord&gt; records = new List&lt;StudentTagRecord&gt;();
        ///     records.Add(record);
        ///     List&lt;string&gt; NewIDs = StudentTag.Insert(records);  
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為學生編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks>
        public static List<string> Insert(IEnumerable<StudentTagRecord> StudentTagRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<StudentTagRecord> worker = new MultiThreadWorker<StudentTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<StudentTagRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Tag");
                    helper.AddElement("Tag", "RefStudentID", editor.RefEntityID);
                    helper.AddElement("Tag", "RefTagID", editor.RefTagID);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<StudentTagRecord>> packages = worker.Run(StudentTagRecords);

            foreach (PackageWorkEventArgs<StudentTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生標籤記錄
        /// </summary>
        /// <param name="StudentTagRecord">學生標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);        
        ///         records[0].RefEntityID = StudentID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = StudentTag.Update(record[0]);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有StudentID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>
        public static int Update(StudentTagRecord StudentTagRecord)
        {
            List<StudentTagRecord> Params = new List<StudentTagRecord>();

            Params.Add(StudentTagRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生標籤記錄
        /// </summary>
        /// <param name="StudentTagRecords">多筆學生標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);        
        ///         records[0].RefEntityID = StudentID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = StudentTag.Update(records);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<StudentTagRecord> StudentTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<StudentTagRecord> worker = new MultiThreadWorker<StudentTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<StudentTagRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Tag");
                    updateHelper.AddElement("Tag", "Field");
                    updateHelper.AddElement("Tag/Field", "RefStudentID", editor.RefEntityID);
                    updateHelper.AddElement("Tag/Field", "RefTagID", editor.RefTagID);
                    updateHelper.AddElement("Tag", "Condition");
                    updateHelper.AddElement("Tag/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);
            };

            List<PackageWorkEventArgs<StudentTagRecord>> packages = worker.Run(StudentTagRecords);

            foreach (PackageWorkEventArgs<StudentTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆學生標籤記錄
        /// </summary>
        /// <param name="StudentTagRecords">多筆標籤記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);
        ///         int DeleteCount = StudentTag.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 傳回值為成功刪除的筆數。
        /// </remarks>        
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<StudentTagRecord> StudentTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<StudentTagRecord> worker = new MultiThreadWorker<StudentTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 300;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<StudentTagRecord> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (StudentTagRecord editor in e.List)
                {
                    deleteHelper.AddElement("Tag");
                    deleteHelper.AddElement("Tag", "RefStudentID", editor.RefEntityID);
                    deleteHelper.AddElement("Tag", "RefTagID", editor.RefTagID);
                    deleteHelper.AddElement("Tag", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<StudentTagRecord>> packages = worker.Run(StudentTagRecords);

            foreach (PackageWorkEventArgs<StudentTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生記錄
        /// </summary>
        /// <param name="StudentTagRecord">學生記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;StudentTagRecord&gt; records = StudentTag.SelectByStudentID(StudentID);
        ///         int DeleteCount = StudentTag.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(StudentTagRecord StudentTagRecord)
        {
            List<StudentTagRecord> studentTagRecords = new List<StudentTagRecord>();

            studentTagRecords.Add(StudentTagRecord);

            return Delete(studentTagRecords);
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
    }
}