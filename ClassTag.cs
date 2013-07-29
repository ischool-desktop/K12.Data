using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 班級標籤類別，提供方法用來取得、新增、修改及刪除班級標籤資訊
    /// </summary>
    public class ClassTag
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Tag.GetDetailListByClass";
        private const string INSERT_SERVICENAME = "SmartSchool.Tag.AddClassTag";
        private const string UPDATE_SERVICENAME = "SmartSchool.Tag.UpdateClassTag";
        private const string DELETE_SERVICENAME = "SmartSchool.Tag.RemoveClassTag";
        private static EntityCache<XmlElement> EntityCache;

        static ClassTag()
        {
            EntityCache = new EntityCache<XmlElement>();
            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有班級標籤列表。
        /// </summary>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectAll();
        ///         
        ///         foreach(ClassTagRecord record in records)
        ///             System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        [SelectMethod("K12.ClassTag.SelectAll", "學籍.班級類別")]
        public static List<ClassTagRecord> SelectAll()
        {           
            return SelectAll<ClassTagRecord>();
        }


        /// <summary>
        /// 取得所有班級標籤列表。
        /// </summary>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassTagRecord&gt; records = ClassTag.SelectAll();
        ///     
        ///     foreach(ClassTagRecord record in records)
        ///         System.Console.WriteLine(record.Name);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:ClassTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagClassID");

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagClassID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        /// <summary>
        /// 根據單筆班級編號取得班級標籤列表。
        /// </summary>
        /// <param name="ClassID">班級編號</param>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);
        ///         
        ///         foreach(ClassTagRecord record in records)
        ///           System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        public static List<ClassTagRecord> SelectByClassID(string ClassID)
        {
            return SelectByClassID<ClassTagRecord>(ClassID);
        }
        
        /// <summary>
        /// 根據單筆班級編號取得班級標籤列表。
        /// </summary>
        /// <param name="ClassID">班級編號</param>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);
        /// </example>
        protected static List<T> SelectByClassID<T>(string ClassID) where T:ClassTagRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ClassID);

            return SelectByClassIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆班級編號取得班級標籤列表。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassIDs(ClassIDs);
        ///     
        ///     forech(ClassTagRecord record in records)
        ///         System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<ClassTagRecord> SelectByClassIDs(IEnumerable<string> ClassIDs)
        {
            return SelectByClassIDs<ClassTagRecord>(ClassIDs);
        }



        /// <summary>
        /// 根據多筆班級編號取得班級標籤列表。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassTagRecord&gt;，代表多筆班級標籤物件。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassIDs(ClassIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByClassIDs<T>(IEnumerable<string> ClassIDs) where T:ClassTagRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "TagClassID");
            helper.AddElement("Condition");

            foreach (string each in ClassIDs)
                helper.AddElement("Condition", "ClassID", each);

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("TagClassID"));

            return K12.Data.Utility.EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        private static List<XmlElement> GetCacheData(IEnumerable<string> ClassTagIDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(ClassTagIDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheRecords = GetDirectData(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheRecords);

                foreach (XmlElement ClassTag in NoCacheRecords)
                    if (!EntityCache.ContainsKey(ClassTag.GetAttribute("TagClassID")))
                        EntityCache.Add(ClassTag.GetAttribute("TagClassID"), ClassTag);
            }

            return CacheSet.Records;
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> ClassTagIDs)
        {
            List<XmlElement> result = new List<XmlElement>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            foreach (string Key in ClassTagIDs)
                helper.AddElement("Condition", "TagClassID", Key);


            DSRequest dsreq = new DSRequest(helper);

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Tag"))
                result.Add(item);

            return result;
        }

        /// <summary>
        /// 新增單筆班級標籤記錄
        /// </summary>
        /// <param name="ClassTagRecord">班級標籤記錄物件</param> 
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ClassTagRecord record = new ClassTagRecord(ClassID, TagConfigID); 
        ///     string NewID = ClassTag.Insert(record);  
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為班級編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks> 
        public static string Insert(ClassTagRecord ClassTagRecord)
        {
            List<ClassTagRecord> Params = new List<ClassTagRecord>();

            Params.Add(ClassTagRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆班級標籤記錄
        /// </summary>
        /// <param name="ClassTagRecords">多筆班級記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ClassTagRecord record = new ClassTagRecord(ClassID, TagConfigID); 
        ///     List&lt;ClassTagRecord&gt; records = new List&lt;ClassTagRecord&gt;();
        ///     records.Add(record);
        ///     List&lt;string&gt; NewIDs = ClassTag.Insert(records);  
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增傳入的參數為班級編號以及標籤編號。
        /// 2.回傳值為新增物件的系統編號。
        /// </remarks>
        public static List<string> Insert(IEnumerable<ClassTagRecord> ClassTagRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<ClassTagRecord> worker = new MultiThreadWorker<ClassTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ClassTagRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Tag");
                    helper.AddElement("Tag", "RefClassID", editor.RefEntityID);
                    helper.AddElement("Tag", "RefTagID", editor.RefTagID);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<ClassTagRecord>> packages = worker.Run(ClassTagRecords);

            foreach (PackageWorkEventArgs<ClassTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆班級標籤記錄
        /// </summary>
        /// <param name="ClassTagRecord">班級標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);        
        ///         records[0].RefEntityID = ClassID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = ClassTag.Update(record[0]);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有ClassID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>
        public static int Update(ClassTagRecord ClassTagRecord)
        {
            List<ClassTagRecord> Params = new List<ClassTagRecord>();

            Params.Add(ClassTagRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆班級標籤記錄
        /// </summary>
        /// <param name="ClassTagRecords">多筆班級標籤記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);        
        ///         records[0].RefEntityID = ClassID;
        ///         records[0].RefTagID = TagConfigID;
        ///         int UpdateCount = ClassTag.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.更新的欄位值只有ClassID及TagConfigID，其它為唯讀欄位。
        /// 2.傳回值為成功更新的筆數。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ClassTagRecord> ClassTagRecords)
        {
            //<Request>
            //    <Tag>
            //        <RefTagID/>
            //        <RefStudentID/>
            //        <Condition>
            //          <ID/>
            //        </Condition>
            //    </Tag>
            //</Request>

            int result = 0;
            List<string> IDs = new List<string>();

            MultiThreadWorker<ClassTagRecord> worker = new MultiThreadWorker<ClassTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ClassTagRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Tag");
                    updateHelper.AddElement("Tag", "RefClassID", editor.RefEntityID);
                    updateHelper.AddElement("Tag", "RefTagID", editor.RefTagID);
                    updateHelper.AddElement("Tag", "Condition");
                    updateHelper.AddElement("Tag/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);
            };

            List<PackageWorkEventArgs<ClassTagRecord>> packages = worker.Run(ClassTagRecords);

            foreach (PackageWorkEventArgs<ClassTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆班級標籤記錄
        /// </summary>
        /// <param name="ClassTagRecords">多筆班級標籤記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);
        ///         int DeleteCount = ClassTag.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 傳回值為成功刪除的筆數。
        /// </remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<ClassTagRecord> ClassTagRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ClassTagRecord> worker = new MultiThreadWorker<ClassTagRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ClassTagRecord> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (ClassTagRecord editor in ClassTagRecords)
                {
                    deleteHelper.AddElement("Tag");
                    deleteHelper.AddElement("Tag", "RefClassID", editor.RefEntityID);
                    deleteHelper.AddElement("Tag", "RefTagID", editor.RefTagID);
                    deleteHelper.AddElement("Tag", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ClassTagRecord>> packages = worker.Run(ClassTagRecords);

            foreach (PackageWorkEventArgs<ClassTagRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆班級記錄
        /// </summary>
        /// <param name="ClassTagRecord">班級記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ClassTagRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;ClassTagRecord&gt; records = ClassTag.SelectByClassID(ClassID);
        ///         int DeleteCount = ClassTag.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(ClassTagRecord ClassTagRecord)
        {
            List<ClassTagRecord> ClassTagRecords = new List<ClassTagRecord>();

            ClassTagRecords.Add(ClassTagRecord);

            return Delete(ClassTagRecords);
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