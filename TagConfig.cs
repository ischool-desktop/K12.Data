using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;
using FISCA.Permission;

namespace K12.Data
{
    /// <summary>
    /// 標籤類別種類
    /// </summary>
    public enum TagCategory
    {
        /// <summary>
        /// 學生（Student）、班級（Class）、教師（Teacher）、課程（Course）
        /// </summary>
        Student, Class, Teacher, Course
    }

    /// <summary>
    /// 標籤設定類別，提供方法用來取得、新增、修改及刪除標籤設定
    /// </summary>
    public class TagConfig
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Tag.GetDetailList";
        private const string INSERT_SERVICENAME = "SmartSchool.Tag.Insert";
        private const string UPDATE_SERVICENAME = "SmartSchool.Tag.Update";
        private const string DELETE_SERVICENAME = "SmartSchool.Tag.Delete";
        private static EntityCache<XmlElement> EntityCache;

        static TagConfig()
        {
            EntityCache = new EntityCache<XmlElement>();
            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有標籤設定列表。
        /// </summary>
        /// <returns>List&lt;TagConfigRecord&gt;，代表多筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectAll();
        ///     
        ///     foreach(TagConfigRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks></remarks>        
        [SelectMethod("K12.TagConfig.SelectAll", "學籍.類別設定")]
        public static List<TagConfigRecord> SelectAll()
        {
            return SelectAll<TagConfigRecord>();
        }

        /// <summary>
        /// 取得所有標籤設定列表。
        /// </summary>
        /// <returns>List&lt;TagConfigRecord&gt;，代表多筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TagConfigRecord&gt; tagconfigs = TagConfig.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : TagConfigRecord, new()
        {
            List<T> Records = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            List<string> IDs = new List<string>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("ID"));

            return EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }

        /// <summary>
        /// 根據單筆標籤設定編號取得標籤設定物件。
        /// </summary>
        /// <param name="TagConfigID">標籤設定編號</param>
        /// <returns>TagConfigRecord，代表單筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TagConfigRecord record = TagConfig.SelectByID(TagConfigID);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是TagConfigID不則在則會傳回null</remarks>
        public static TagConfigRecord SelectByID(string TagConfigID)
        {
            return SelectByID<TagConfigRecord>(TagConfigID);
        }


        /// <summary>
        /// 根據單筆標籤設定編號取得標籤設定物件。
        /// </summary>
        /// <param name="TagConfigRecordID">標籤設定編號</param>
        /// <returns>TagConfigRecord，代表單筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     TagConfigRecord tagconfig = ClassTag.SelectByID(TagConfigRecordID);
        /// </example>
        protected static T SelectByID<T>(string TagConfigRecordID) where T : TagConfigRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(TagConfigRecordID);

            List<T> Records = SelectByIDs<T>(IDs);

            if (Records.Count > 0)
                return Records[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆標籤設定編號取得標籤設定物件。
        /// </summary>
        /// <param name="TagConfigIDs">多筆標籤設定編號</param>
        /// <returns>List&lt;TagConfigRecord&gt;，代表多筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/> 
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectByIDs(TagConfigIDs);
        ///     
        ///     foreach(TagConfigRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<TagConfigRecord> SelectByIDs(IEnumerable<string> TagConfigIDs)
        {
            return SelectByIDs<TagConfigRecord>(TagConfigIDs);
        }

        /// <summary>
        /// 根據多筆標籤設定編號取得標籤設定物件。
        /// </summary>
        /// <param name="TagConfigRecordIDs">多筆標籤設定編號</param>
        /// <returns>List&lt;TagConfigRecord&gt;，代表多筆標籤設定物件。</returns>
        /// <seealso cref="TagConfigRecord"/> 
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectByIDs(TagConfigRecordIDs);
        /// </example>
        protected static List<T> SelectByIDs<T>(IEnumerable<string> TagConfigRecordIDs) where T : TagConfigRecord, new()
        {
            return EntityHelper.GetListFromXml<T>(GetCacheData(TagConfigRecordIDs));
        }

        /// <summary>
        /// 根據標籤設定所屬分類( TagCategory )，取得該分類的標籤設定
        /// </summary>
        /// <param name="TagCategory">標籤設定所屬分類，參考 TagCategory 列舉型別。</param>
        /// <returns>List&lt;TagConfigRecord&gt;，指定標籤分類中的所有標籤設定</returns>
        /// <seealso cref="TagCategory"/>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectByCategory(TagCategory.Student);
        ///     
        ///     foreach(TagConfigRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        ///</example>
        ///<remarks>傳入的參數可為TagCategory.Student、TagCategory.Class、TagCategory、Teacher、TagCategory.Course</remarks>
        public static List<TagConfigRecord> SelectByCategory(TagCategory TagCategory)
        {
            return SelectByCategory<TagConfigRecord>(TagCategory);
        }


        /// <summary>
        /// 根據標籤設定所屬分類( TagCategory )，取得該分類的標籤設定
        /// </summary>
        /// <param name="TagCategory">標籤設定所屬分類，參考 TagCategory 列舉型別。</param>
        /// <returns>List&lt;TagConfigRecord&gt;，指定標籤分類中的所有標籤設定</returns>
        /// <seealso cref="TagCategory"/>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectByCategory(TagCategory.Student);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByCategory<T>(TagCategory TagCategory) where T : TagConfigRecord, new()
        {
            List<TagConfigRecord> result = new List<TagConfigRecord>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "Category").InnerText = GetTagCategoryString(TagCategory);

            List<string> IDs = new List<string>();

            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Tag"))
                IDs.Add(item.GetAttribute("ID"));

            return EntityHelper.GetListFromXml<T>(GetCacheData(IDs));
        }


        /// <summary>
        /// 根據標籤所屬分類( TagCategory )，取得該種類中所有的標籤類別 (TagPrefix)
        /// </summary>
        /// <param name="TagCategory"> 標籤所屬分類，參考 TagCategory 列舉型別。</param>
        /// <returns>指定標籤分類中的所有標籤類別</returns>
        /// <seealso cref="TagCategory"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;string&gt; records = TagConfig.SelectPrefixByCategory(TagCategory.Student);
        ///     
        ///         foreach(string record in records)
        ///             Console.WrlteLine(record);
        ///     </code>
        /// </example>
        public static List<string> SelectPrefixByCategory(TagCategory TagCategory)
        {
            List<TagConfigRecord> records = SelectByCategory<TagConfigRecord>(TagCategory);

            List<string> result = new List<string>();

            result.Clear();

            foreach (TagConfigRecord record in records)
                if (!string.IsNullOrEmpty(record.Prefix) && !result.Contains(record.Prefix))
                    result.Add(record.Prefix);

            return result;
        }

        /// <summary>
        /// 根據標籤設定所屬分類( TagCategory )以及標籤設定類別（Prefix），取得該分類的標籤設定
        /// </summary>
        /// <param name="TagCategory">標籤設定所屬分類，參考 TagCategory 列舉型別。</param>
        /// <param name="Prefix">標籤設定類別（Prefix）</param>
        /// <returns>List&lt;TagConfigRecord&gt;，多筆標籤設定</returns>
        /// <seealso cref="TagCategory"/>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TagConfigRecord&gt; records = TagConfig.SelectByCategoryAndPrefix(TagCategory.Student,Prefix);
        ///     
        ///     foreach(TagConfigRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        public static List<TagConfigRecord> SelectByCategoryAndPrefix(TagCategory TagCategory, string Prefix)
        {
            return SelectByCategoryAndPrefix<TagConfigRecord>(TagCategory, Prefix);
        }


        /// <summary>
        /// 根據標籤設定所屬分類( TagCategory )以及標籤設定類別（Prefix），取得該分類的標籤設定
        /// </summary>
        /// <param name="TagCategory">標籤設定所屬分類，參考 TagCategory 列舉型別。</param>
        /// <param name="Prefix">標籤設定類別（Prefix）</param>
        /// <returns>List&lt;TagConfigRecord&gt;，多筆標籤設定</returns>
        /// <seealso cref="TagCategory"/>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectByCategoryAndPrefix<T>(TagCategory TagCategory, string Prefix) where T : TagConfigRecord, new()
        {
            List<T> tagrecords = SelectByCategory<T>(TagCategory);
            List<T> result = new List<T>();

            foreach (T tagrecord in tagrecords)
                if (tagrecord.Prefix.Equals(Prefix))
                    result.Add(tagrecord);
            return result;
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
                    if (!EntityCache.ContainsKey(Record.GetAttribute("ID")))
                        EntityCache.Add(Record.GetAttribute("ID"), Record);
            }

            return CacheSet.Records;
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> IDs)
        {
            List<XmlElement> result = new List<XmlElement>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "Category");
            helper.AddElement("Field", "Prefix");
            helper.AddElement("Field", "Name");
            helper.AddElement("Field", "Color");

            if (IsSupportAccessControlCode())
                helper.AddElement("Field", "AccessControlCode");

            helper.AddElement("Condition");

            foreach (string Key in IDs)
                helper.AddElement("Condition", "ID", Key);

            DSRequest dsreq = new DSRequest(helper);

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Tag"))
                result.Add(item);

            return result;
        }

        /// <summary>
        /// 新增單筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecord">標籤設定記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         TagConfigRecord newrecord = new TagConfigRecord();
        ///         newrecord.Prefix = (new System.Random()).NextDouble().ToString();
        ///         newrecord.Name = (new System.Random()).NextDouble().ToString();
        ///         newrecord.Category = "student";
        ///         strng NewID = TagConfig.Insert(newrecord)
        ///         TagConfigRecord record = TagConfig.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 新增一律傳回新增物件的編號。
        /// </remarks>
        public static string Insert(TagConfigRecord TagConfigRecord)
        {
            List<TagConfigRecord> Params = new List<TagConfigRecord>();

            Params.Add(TagConfigRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecords">多筆標籤設定記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         TagConfigRecord record = new TagConfigRecord();
        ///         record.Prefix = (new System.Random()).NextDouble().ToString();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         record.Category = "student";
        ///         List&lt;TagConfigRecord&gt; records = new List&lt;TagConfigRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = TagConfig.Insert(records);
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<TagConfigRecord> TagConfigRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<TagConfigRecord> worker = new MultiThreadWorker<TagConfigRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TagConfigRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Tag");
                    helper.AddElement("Tag", "Field");
                    helper.AddElement("Tag", "Prefix", editor.Prefix);
                    helper.AddElement("Tag", "Name", editor.Name.ToString());
                    helper.AddElement("Tag", "Color", editor.ColorCode.ToString());
                    helper.AddElement("Tag", "Category", editor.Category);

                    if (IsSupportAccessControlCode())
                        helper.AddElement("Tag", "AccessControlCode", editor.AccessControlCode);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<TagConfigRecord>> packages = worker.Run(TagConfigRecords);

            foreach (PackageWorkEventArgs<TagConfigRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecord">標籤設定記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TagConfigRecord record = TagConfig.SelectByID(TagConfigID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = TagConfig.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(TagConfigRecord TagConfigRecord)
        {
            List<TagConfigRecord> Params = new List<TagConfigRecord>();

            Params.Add(TagConfigRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecords">多筆標籤設定記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TagConfigRecords"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TagConfigRecord record = TagConfig.SelectByID(TagConfigID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;TagConfigRecord&gt; records = new List&lt;TagConfigRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = TagConfig.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<TagConfigRecord> TagConfigRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<TagConfigRecord> worker = new MultiThreadWorker<TagConfigRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TagConfigRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Tag");
                    updateHelper.AddElement("Tag", "Field");
                    updateHelper.AddElement("Tag/Field", "Prefix", editor.Prefix);
                    updateHelper.AddElement("Tag/Field", "Name", editor.Name);
                    updateHelper.AddElement("Tag/Field", "Color", editor.ColorCode.ToString());

                    if (IsSupportAccessControlCode())
                        updateHelper.AddElement("Tag/Field", "AccessControlCode", editor.AccessControlCode);

                    updateHelper.AddElement("Tag", "Condition");
                    updateHelper.AddElement("Tag/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);
            };

            List<PackageWorkEventArgs<TagConfigRecord>> packages = worker.Run(TagConfigRecords);

            foreach (PackageWorkEventArgs<TagConfigRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecords">多筆標籤設定記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;TagConfigRecord&gt; records = TagConfig.SelectByIDs(TagConfigIDs);
        ///       int DeleteCount = TagConfig.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<TagConfigRecord> TagConfigRecords)
        {
            List<string> Keys = new List<string>();

            foreach (TagConfigRecord TagConfigRecord in TagConfigRecords)
                Keys.Add(TagConfigRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除單筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigRecord">標籤設定記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TagConfigRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       TagConfigRecord record = TagConfig.SelectByID(TagConfigID);
        ///       int DeleteCount = TagConfig.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(TagConfigRecord TagConfigRecord)
        {
            return Delete(TagConfigRecord.ID);
        }

        /// <summary>
        /// 刪除單筆標籤設定記錄
        /// </summary>
        /// <param name="TagConfigID">標籤設定記錄編號</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = TagConfig.Delete(TagConfigID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string TagConfigID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(TagConfigID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆標籤設定記錄
        /// </summary>
        /// <param name="List&lt;string&gt">多筆標籤設定記錄編號</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = TagConfig.Delete(TagConfigIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> TagConfigIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (string editor in TagConfigIDs)
                {
                    deleteHelper.AddElement("Tag");
                    deleteHelper.AddElement("Tag", "ID", editor);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(TagConfigIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(TagConfigIDs, ChangedSource.Local));

            return result;
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


        #region ============  private functions  =============================

        /// <summary>
        /// 判斷資料庫 Schema 的版本是否大於「20120501」。
        /// 大於該版本，代表支援「AccessControlCode」。
        /// </summary>
        /// <returns></returns>
        private static bool IsSupportAccessControlCode()
        {
            Type target = typeof(FISCA.Authentication.DSAServices);

            System.Reflection.MemberInfo[] member = target.GetMember("PhysicalSchemaVersion");

            if (member.Length > 0)
            {
                System.Reflection.PropertyInfo ptyTarget = (member[0] as System.Reflection.PropertyInfo);

                string strVer = ptyTarget.GetValue(null, null) + "";

                int version;
                if (int.TryParse(strVer, out version))
                    return version >= 20120501;
                else
                    return false;
            }

            return false;
        }

        //其實可以直接 tagCategory.ToString() 就可以了。此function 考慮廢除。
        private static string GetTagCategoryString(TagCategory tagCategory)
        {
            switch (tagCategory)
            {
                case TagCategory.Student:
                    return "Student";
                case TagCategory.Class:
                    return "Class";
                case TagCategory.Course:
                    return "Course";
                case TagCategory.Teacher:
                    return "Teacher";
                default:
                    return "";
            }
        }

        #endregion
    }

    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //public static class TagConfig_Extensions
    //{
    //    /// <summary>
    //    /// 取得具有檢視權限的 TagID 清單。
    //    /// </summary>
    //    /// <param name="acl"></param>
    //    /// <param name="tagIDs"></param>
    //    /// <returns></returns>
    //    public static ISet<string> Viewable(this FeatureAcl acl, IEnumerable<string> tagIDs)
    //    {
    //        HashSet<string> accepts = new HashSet<string>();
    //        List<TagConfigRecord> alltag = TagConfig.SelectAll();

    //        foreach (TagConfigRecord record in alltag)
    //        {
    //            if (acl[record.AccessControlCode].Viewable)
    //                accepts.Add(record.ID);
    //        }

    //        return accepts;
    //    }

    //    /// <summary>
    //    /// 取得具有編輯權限的 TagID 清單。
    //    /// </summary>
    //    /// <param name="acl"></param>
    //    /// <param name="tagIDs"></param>
    //    /// <returns></returns>
    //    public static ISet<string> Editable(this FeatureAcl acl, IEnumerable<string> tagIDs)
    //    {
    //        HashSet<string> accepts = new HashSet<string>();
    //        List<TagConfigRecord> alltag = TagConfig.SelectAll();

    //        foreach (TagConfigRecord record in alltag)
    //        {
    //            if (acl[record.AccessControlCode].Editable)
    //                accepts.Add(record.ID);
    //        }

    //        return accepts;
    //    }
    //}
}