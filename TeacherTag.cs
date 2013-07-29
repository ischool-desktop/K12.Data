using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 教師標籤類別，提供方法用來取得、新增、修改及刪除班級標籤資訊
    /// </summary>
    public class TeacherTag
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Tag.GetDetailListByTeacher";
        private const string INSERT_SERVICENAME = "SmartSchool.Tag.AddTeacherTag";
        private const string UPDATE_SERVICENAME = "SmartSchool.Tag.UpdateTeacherTag";
        private const string DELETE_SERVICENAME = "SmartSchool.Tag.RemoveTeacherTag";
        private static EntityCache<XmlElement> EntityCache;

        static TeacherTag()
        {
            EntityCache = new EntityCache<XmlElement>();
            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有教師標籤列表。
        /// </summary>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectAll();
        ///        
        ///         foreach(TeacherTagRecord record in records)
        ///             System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        [SelectMethod("K12.TeacherTag.SelectAll", "學籍.教師類別")]
        public static List<TeacherTagRecord> SelectAll()
        {
            return SelectAll<TeacherTagRecord>();
        }

        /// <summary>
        /// 取得所有教師標籤列表。
        /// </summary>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:TeacherTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("Field");
            helper.AddElement("Field", "TagTeacherID");

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagTeacherID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        /// <summary>
        /// 根據單筆教師編號取得教師標籤列表。
        /// </summary>
        /// <param name="TeacherID">教師編號</param>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <sample>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);
        ///        
        ///         foreach(TeacherTagRecord record in records)
        ///           System.Console.WriteLine(record.Name);
        ///     </code>
        /// </sample>
        public static List<TeacherTagRecord> SelectByTeacherID(string TeacherID)
        {
            return SelectByTeacherID<TeacherTagRecord>(TeacherID);
        }

        /// <summary>
        /// 根據單筆教師編號取得教師標籤列表。
        /// </summary>
        /// <param name="TeacherID">教師編號</param>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);
        /// </example>
        protected static List<T> SelectByTeacherID<T>(string TeacherID) where T:TeacherTagRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(TeacherID);

            return SelectByTeacherIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆教師編號取得教師標籤列表。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherIDs(TeacherIDs);
        ///    
        ///     forech(TeacherTagRecord record in records)
        ///         System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<TeacherTagRecord> SelectByTeacherIDs(IEnumerable<string> TeacherIDs)
        {
            return SelectByTeacherIDs<TeacherTagRecord>(TeacherIDs);
        }

        
        /// <summary>
        /// 根據多筆教師編號取得教師標籤列表。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherTagRecord&gt;，代表多筆教師標籤物件。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherIDs(TeacherIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByTeacherIDs<T>(IEnumerable<string> TeacherIDs) where T:TeacherTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagTeacherID");
            helper.AddElement("Condition");

            foreach (string each in TeacherIDs)
                helper.AddElement("Condition", "TeacherID", each);

            List<string> IDs=new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagTeacherID"));

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
                    if (!EntityCache.ContainsKey(Record.GetAttribute("TagTeacherID")))
                        EntityCache.Add(Record.GetAttribute("TagTeacherID"), Record);
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
                helper.AddElement("Condition", "TagTeacherID", Key);


            DSRequest dsreq = new DSRequest(helper);

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Tag"))
                result.Add(item);

            return result;
        }

        /// <summary>
        /// 新增單筆教師標籤記錄
        /// </summary>
        /// <param name="TeacherTagRecord">教師標籤記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TeacherTagRecord  record = new TeacherTagRecord(TeacherID, TagConfigID);
        ///     string NewID = TeacherTag.Insert(record); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為教師編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks>
        public static string Insert(TeacherTagRecord TeacherTagRecord)
        {
            List<TeacherTagRecord> Params = new List<TeacherTagRecord>();

            Params.Add(TeacherTagRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆教師標籤記錄
        /// </summary>
        /// <param name="TeacherTagRecords">多筆教師編號記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TeacherTagRecord record = new TeacherTagRecord(TeacherID, TagConfigID);
        ///     List&lt;TeacherTagRecord&gt; records = new List&lt;TeacherTagRecord&gt;();
        ///     records.Add(record);
        ///     List&lt;string&gt; NewIDs = TeacherTag.Insert(records); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為班級編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static List<string> Insert(IEnumerable<TeacherTagRecord> TeacherTagRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<TeacherTagRecord> worker = new MultiThreadWorker<TeacherTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TeacherTagRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Tag");
                    helper.AddElement("Tag", "RefTeacherID", editor.RefEntityID);
                    helper.AddElement("Tag", "RefTagID", editor.RefTagID);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<TeacherTagRecord>> packages = worker.Run(TeacherTagRecords);

            foreach (PackageWorkEventArgs<TeacherTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆教師標籤記錄
        /// </summary>
        /// <param name="TeacherTagRecord"> 教師標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);       
        ///         records[0].RefEntityID = TeacherID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = TeacherTag.Update(record[0]);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有TeacherID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>       
        public static int Update(TeacherTagRecord TeacherTagRecord)
        {
            List<TeacherTagRecord> Params = new List<TeacherTagRecord>();

            Params.Add(TeacherTagRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆教師標籤記錄
        /// </summary>
        /// <param name="TeacherTagRecords">多筆教師標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);       
        ///         records[0].RefEntityID = TeacherID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = TeacherTag.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有TeacherID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<TeacherTagRecord> TeacherTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<TeacherTagRecord> worker = new MultiThreadWorker<TeacherTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TeacherTagRecord> e)
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

            List<PackageWorkEventArgs<TeacherTagRecord>> packages = worker.Run(TeacherTagRecords);

            foreach (PackageWorkEventArgs<TeacherTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆教師標籤記錄
        /// </summary>
        /// <param name="TeacherTagRecords">多筆教師標籤記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);
        ///         int DeleteCount = TeacherTag.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 傳回值為成功刪除的筆數。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<TeacherTagRecord> TeacherTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<TeacherTagRecord> worker = new MultiThreadWorker<TeacherTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TeacherTagRecord> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (TeacherTagRecord editor in TeacherTagRecords)
                {
                    deleteHelper.AddElement("Tag");
                    deleteHelper.AddElement("Tag", "RefTeacherID", editor.RefEntityID);
                    deleteHelper.AddElement("Tag", "RefTagID", editor.RefTagID);
                    deleteHelper.AddElement("Tag", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<TeacherTagRecord>> packages = worker.Run(TeacherTagRecords);

            foreach (PackageWorkEventArgs<TeacherTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;


            AfterDelete(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆教師記錄
        /// </summary>
        /// <param name="TeacherTagRecord">教師標籤記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TeacherTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TeacherTagRecord&gt; records = TeacherTag.SelectByTeacherID(TeacherID);
        ///         int DeleteCount = TeacherTag.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(TeacherTagRecord TeacherTagRecord)
        {
            List<TeacherTagRecord> TeacherTagRecords = new List<TeacherTagRecord>();

            TeacherTagRecords.Add(TeacherTagRecord);

            return Delete(TeacherTagRecords);
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