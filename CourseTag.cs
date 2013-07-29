using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 課程標籤類別，提供方法用來取得、新增、修改及刪除課程標籤資訊
    /// </summary>
    public class CourseTag 
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Tag.GetDetailListByCourse";
        private const string INSERT_SERVICENAME = "SmartSchool.Tag.AddCourseTag";
        private const string UPDATE_SERVICENAME = "SmartSchool.Tag.UpdateCourseTag";
        private const string DELETE_SERVICENAME = "SmartSchool.Tag.RemoveCourseTag";
        private static EntityCache<XmlElement> EntityCache;

        static CourseTag()
        {
            EntityCache = new EntityCache<XmlElement>();
            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有課程標籤列表。
        /// </summary>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;CourseTagRecord&gt; records = CourseTag.SelectAll();
        ///         foreach(CourseTagRecord record in records)
        ///             System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        [SelectMethod("K12.CourseTag.SelectAll", "學籍.課程類別")]
        public static List<CourseTagRecord> SelectAll()
        {
            return SelectAll<CourseTagRecord>();
        }

        /// <summary>
        /// 取得所有課程標籤列表。
        /// </summary>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseTagRecord&gt; records = CourseTag.SelectAll();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:CourseTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagCourseID");

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagCourseID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        public static List<CourseTagRecord> Select(IEnumerable<string> CourseIDs, IEnumerable<string> TagCourseIDs)
        {
            return Select<CourseTagRecord>(CourseIDs, TagCourseIDs);
        }


        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> CourseIDs,IEnumerable<string> TagCourseIDs) where T : CourseTagRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagCourseID");
            helper.AddElement("Condition");

            if (CourseIDs!=null)
                foreach (string each in CourseIDs)
                    helper.AddElement("Condition", "CourseID", each);

            if (TagCourseIDs != null)
                foreach (string each in TagCourseIDs)
                    helper.AddElement("Condition", "TagCourseID", each);

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagCourseID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        /// <summary>
        /// 根據單筆課程編號取得課程標籤列表。
        /// </summary>
        /// <param name="CourseID">課程編號</param>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(CourseID);
        ///       
        ///       foreach(CourseTagRecord record in records)
        ///           System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseTagRecord> SelectByCourseID(string CourseID)
        {
            return SelectByCourseID<CourseTagRecord>(CourseID);
        }

        /// <summary>
        /// 根據單筆課程編號取得課程標籤列表。
        /// </summary>
        /// <param name="CourseID">課程編號</param>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(ClassID);
        /// </example>
        protected static List<T> SelectByCourseID<T>(string CourseID) where T:CourseTagRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(CourseID);

            return SelectByCourseIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆課程編號取得課程標籤列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseIDs(CourseIDs);
        ///         
        ///         forech(CourseTagRecord record in records)
        ///             System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseTagRecord> SelectByCourseIDs(IEnumerable<string> CourseIDs)
        {
            return SelectByCourseIDs<CourseTagRecord>(CourseIDs);
        }            

        /// <summary>
        /// 根據多筆課程編號取得課程標籤列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseTagRecord&gt;，代表多筆課程標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseIDs(CourseIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByCourseIDs<T>(IEnumerable<string> CourseIDs) where T:CourseTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagCourseID");
            helper.AddElement("Condition");

            foreach (string each in CourseIDs)
                helper.AddElement("Condition", "CourseID", each);

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagCourseID"));

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
                    if (!EntityCache.ContainsKey(Record.GetAttribute("TagCourseID")))
                        EntityCache.Add(Record.GetAttribute("TagCourseID"), Record);
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
                helper.AddElement("Condition", "TagCourseID", Key);

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                result.Add(item);

            return result;
        }

        /// <summary>
        /// 新增單筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecord">課程標籤記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///    <code>
        ///     CourseTagRecord  record = new CourseTagRecord(CourseID, TagConfigID); 
        ///     string NewID = CourseTag.Insert(record);  
        ///    </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為課程編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks> 
        public static string Insert(CourseTagRecord CourseTagRecord)
        {
            List<CourseTagRecord> Params = new List<CourseTagRecord>();

            Params.Add(CourseTagRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecords">多筆課程記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     CourseTagRecord record = new CourseTagRecord(CourseID, TagConfigID); 
        ///     List&lt;CourseTagRecord&gt; records = new List&lt;CourseTagRecord&gt;();
        ///     records.Add(record);
        ///     List&lt;string&gt; NewIDs = CourseTag.Insert(records);  
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為課程編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks>
        public static List<string> Insert(IEnumerable<CourseTagRecord> CourseTagRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<CourseTagRecord> worker = new MultiThreadWorker<CourseTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<CourseTagRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Tag");
                    helper.AddElement("Tag", "RefCourseID", editor.RefEntityID);
                    helper.AddElement("Tag", "RefTagID", editor.RefTagID);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<CourseTagRecord>> packages = worker.Run(CourseTagRecords);

            foreach (PackageWorkEventArgs<CourseTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecord">課程標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(CourseID);        
        ///         records[0].RefEntityID = CourseID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = CourseTag.Update(record[0]);
        ///     </code>
        /// </example>
        public static int Update(CourseTagRecord CourseTagRecord)
        {
            List<CourseTagRecord> Params = new List<CourseTagRecord>();

            Params.Add(CourseTagRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecords">多筆課程標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(CourseID);        
        ///         records[0].RefEntityID = CourseID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = CourseTag.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有CourseID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<CourseTagRecord> CourseTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<CourseTagRecord> worker = new MultiThreadWorker<CourseTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<CourseTagRecord> e)
            {
                DSXmlHelper updateHelper= new DSXmlHelper("Request");

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

            List<PackageWorkEventArgs<CourseTagRecord>> packages = worker.Run(CourseTagRecords);

            foreach (PackageWorkEventArgs<CourseTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecords">多筆班級課程記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(CourseID);
        ///         int DeleteCount = CourseTag.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<CourseTagRecord> CourseTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<CourseTagRecord> worker = new MultiThreadWorker<CourseTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<CourseTagRecord> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (CourseTagRecord editor in CourseTagRecords)
                {
                    deleteHelper.AddElement("Tag");
                    deleteHelper.AddElement("Tag", "RefCourseID", editor.RefEntityID);
                    deleteHelper.AddElement("Tag", "RefTagID", editor.RefTagID);
                    deleteHelper.AddElement("Tag","ID",editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<CourseTagRecord>> packages = worker.Run(CourseTagRecords);

            foreach (PackageWorkEventArgs<CourseTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆課程標籤記錄
        /// </summary>
        /// <param name="CourseTagRecord">課程標籤記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="CourseTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;CourseTagRecord&gt; records = CourseTag.SelectByCourseID(CourseID);
        ///         int DeleteCount = CourseTag.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(CourseTagRecord CourseTagRecord)
        {
            List<CourseTagRecord> CourseTagRecords = new List<CourseTagRecord>();

            CourseTagRecords.Add(CourseTagRecord);

            return Delete(CourseTagRecords);
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