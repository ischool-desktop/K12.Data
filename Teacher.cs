using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 教師類別，提供方法用來取得、新增、修改及刪除教師資訊
    /// </summary>
    public class Teacher
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Teacher.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Teacher.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.Teacher.Insert";
        private const string DELET_SERVICENAME = "SmartSchool.Teacher.DeleteForce";
        private static EntityCache<XmlElement> EntityCache;

        /// <summary>
        /// Static建構式
        /// </summary>
        static Teacher()
        {
            EntityCache = new EntityCache<XmlElement>();

            //App.DBMonitor["teacher"].RecordInserted += new FISCA.Synchronization.TableChangedEventHandler(Teacher_RecordInserted);
            //App.DBMonitor["teacher"].RecordUpdated += new FISCA.Synchronization.TableChangedEventHandler(Teacher_RecordUpdated);
            //App.DBMonitor["teacher"].RecordDeleted += new FISCA.Synchronization.TableChangedEventHandler(Teacher_RecordDeleted);

            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        /// <summary>
        /// 取得所有教師記錄列表。
        /// </summary>
        /// <returns>List&lt;TeacherRecord&gt;，代表多筆教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TeacherRecord&gt; records = Teacher.SelectAll();
        ///     
        ///     foreach(TeacherRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        [SelectMethod("K12.Teacher.SelectAll", "學籍.教師")]
        public static List<TeacherRecord> SelectAll()
        {
            return SelectAll<K12.Data.TeacherRecord>();
        }

        /// <summary>
        /// 取得所有教師記錄列表。
        /// </summary>
        /// <typeparam name="T">教師記錄物件型別，K12共用為K12.Data.TeacherRecord</typeparam>
        /// <returns>List&lt;TeacherRecord&gt;，代表多筆教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherRecord&gt; teacherrecs = Teacher.SelectAll&lt;K12.Data.TeacherRecord&gt;();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:TeacherRecord ,new()
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetTeacherListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            List<string> IDs = new List<string>();
            
            foreach (XmlElement var in dsrsp.GetContent().GetElements("Teacher"))
                IDs.Add(var.GetAttribute("ID"));

            List<T> Types = new List<T>();

            foreach (System.Xml.XmlElement element in GetCacheData(IDs))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆教師編號取得教師記錄物件。
        /// </summary>
        /// <param name="TeacherID">教師編號</param>
        /// <returns>TeacherRecord，代表教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TeacherRecord record = Teacher.SelectByID(TeacherID);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是TeacherID不則在則會傳回null</remarks>
        public static TeacherRecord SelectByID(string TeacherID)
        {
            return SelectByID<K12.Data.TeacherRecord>(TeacherID);
        }

        /// <summary>
        /// 根據單筆教師編號取得教師記錄物件。
        /// </summary>
        /// <typeparam name="T">教師記錄物件型別，K12共用為K12.Data.TeacherRecord</typeparam>
        /// <param name="TeacherID">教師編號</param>
        /// <returns>TeacherRecord，代表教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     TeacherRecord teacherrec = Teacher.SelectByID&lt;K12.Data.TeacherRecord&gt;(TeacherID);
        /// </example>
        protected static T SelectByID<T>(string TeacherID) where T:TeacherRecord,new ()
        {
            List<string> IDs = new List<string>();

            IDs.Add(TeacherID);

            List<T> Types = new List<T>();

            Types = SelectByIDs<T>(IDs);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆教師編號取得教師記錄列表。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherRecord&gt;，代表多教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TeacherRecord&gt; records = Teacher.SelectByIDs(TeacherIDs);
        ///     
        ///     foreach(TeacherRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<TeacherRecord> SelectByIDs(IEnumerable<string> TeacherIDs)
        {
            return SelectByIDs<K12.Data.TeacherRecord>(TeacherIDs);
        }

        /// <summary>
        /// 根據多筆教師編號取得教師記錄列表。
        /// </summary>
        /// <typeparam name="T">教師記錄物件型別，K12共用為K12.Data.TeacherRecord</typeparam>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherRecord&gt;，代表多教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherRecord&gt; teacherrecs = Teacher.SelectByIDs&lt;K12.Data.TeacherRecord&gt;(TeacherIDs);
        /// </example>
        protected static List<T> SelectByIDs<T>(IEnumerable<string> TeacherIDs) where T:TeacherRecord,new()
        {
            List<T> Types = new List<T>();

            foreach (System.Xml.XmlElement element in GetCacheData(TeacherIDs))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆教師編號取得教師記錄列表。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherRecord&gt;，代表多教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherRecord&gt; teacherrecs = Teacher.SelectByIDs(TeacherIDs);
        /// </example>
        private static List<XmlElement> GetCacheData(IEnumerable<string> TeacherIDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(TeacherIDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheRecords = GetDirectData(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheRecords);

                foreach (XmlElement Teacher in NoCacheRecords)
                    if (!EntityCache.ContainsKey(Teacher.GetAttribute("ID")))
                        EntityCache.Add(Teacher.GetAttribute("ID"), Teacher);
            }

            return CacheSet.Records;
        }

        private static DSXmlHelper CreateBriefFieldHelper()
        {
            DSXmlHelper helper = new DSXmlHelper("GetTeacherListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "TeacherName");
            helper.AddElement("Field", "Nickname");
            helper.AddElement("Field", "Status");
            helper.AddElement("Field", "Gender");
            helper.AddElement("Field", "IDNumber");
            helper.AddElement("Field", "Category");
            helper.AddElement("Field", "ContactPhone");
            helper.AddElement("Field", "TALoginName");
            helper.AddElement("Field", "TAPassword");
            helper.AddElement("Field", "AccountType");
            helper.AddElement("Field", "Photo");
            helper.AddElement("Field", "Email");
            return helper;
        }


        /// <summary>
        /// 根據多筆教師編號取得教師記錄列表。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TeacherRecord&gt;，代表多教師記錄物件。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TeacherRecord&gt; teacherrecs = Teacher.SelectByIDs(TeacherIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> TeacherIDs)
        {
            bool hasKey = false;
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = CreateBriefFieldHelper();
            helper.AddElement("Condition");
            foreach (var key in TeacherIDs)
            {
                hasKey = true;
                helper.AddElement("Condition", "ID", key);
            }
            helper.AddElement("Order");
            List<XmlElement> result = new List<XmlElement>();
            if (hasKey)
            {
                dsreq.SetContent(helper);
                
                DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

                DSXmlHelper resulthelper = dsrsp.GetContent();

                foreach (XmlElement var in resulthelper.GetElements("Teacher"))
                {
                    result.Add(var);
                }
            }
            return result;
        }

        /// <summary>
        /// 新增單筆教師記錄
        /// </summary>
        /// <param name="TeacherRecord">教師記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         TeacherRecord newrecord = new TeacherRecord();
        ///         newrecord.Name = (new System.Random()).NextDouble().ToString();
        ///         strng NewID = Teacher.Insert(newrecord)
        ///         TeacherRecord record = Teacher.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為課程名稱（Name）。
        /// </remarks>
        public static string Insert(TeacherRecord TeacherRecord)
        {
            List<TeacherRecord> Params = new List<TeacherRecord>();

            Params.Add(TeacherRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆教師記錄
        /// </summary>
        /// <param name="TeacherRecords">多筆教師記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         TeacherRecord record = new TeacherRecord();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         List&lt;TeacherRecord&gt; records = new List&lt;TeacherRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Teacher.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<TeacherRecord> TeacherRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<TeacherRecord> worker = new MultiThreadWorker<TeacherRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TeacherRecord> e)
            {
                DSXmlHelper insertHelper = new DSXmlHelper("InsertRequest");

                foreach (var editor in e.List)
                {
                    insertHelper.AddElement("Teacher");
                    insertHelper.AddElement("Teacher", "Field");
                    insertHelper.AddElement("Teacher/Field", "TeacherName", editor.Name);
                    insertHelper.AddElement("Teacher/Field", "Nickname", editor.Nickname);
                    insertHelper.AddElement("Teacher/Field", "Gender", editor.Gender);
                    insertHelper.AddElement("Teacher/Field", "IDNumber", editor.IDNumber);
                    insertHelper.AddElement("Teacher/Field", "Category", editor.Category);
                    insertHelper.AddElement("Teacher/Field", "Status", editor.Status.ToString());
                    insertHelper.AddElement("Teacher/Field", "ContactPhone", editor.ContactPhone);
                    insertHelper.AddElement("Teacher/Field", "TALoginName", editor.TALoginName);
                    insertHelper.AddElement("Teacher/Field", "TAPassword", editor.TAPassword);
                    insertHelper.AddElement("Teacher/Field", "AccountType", editor.AccountType);
                    insertHelper.AddElement("Teacher/Field", "Photo", editor.Photo);
                    insertHelper.AddElement("Teacher/Field", "Email", editor.Email);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(insertHelper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<TeacherRecord>> packages = worker.Run(TeacherRecords);

            foreach (PackageWorkEventArgs<TeacherRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆教師記錄
        /// </summary>
        /// <param name="TeacherRecord">教師記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TeacherRecord record = Teacher.SelectByID(TeacherID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Teacher.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(TeacherRecord TeacherRecord)
        {
            List<TeacherRecord> Params = new List<TeacherRecord>();

            Params.Add(TeacherRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆教師記錄
        /// </summary>
        /// <param name="TeacherRecords">多筆教師記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     TeacherRecord record = Teacher.SelectByID(CourseID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;TeacherRecord&gt; records = new List&lt;TeacherRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Teacher.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<TeacherRecord> TeacherRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<TeacherRecord> worker = new MultiThreadWorker<TeacherRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TeacherRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateRequest");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Teacher");
                    updateHelper.AddElement("Teacher", "Field");
                    updateHelper.AddElement("Teacher/Field", "TeacherName", editor.Name);
                    updateHelper.AddElement("Teacher/Field", "Nickname", editor.Nickname);
                    updateHelper.AddElement("Teacher/Field", "Gender", editor.Gender);
                    updateHelper.AddElement("Teacher/Field", "IDNumber", editor.IDNumber);
                    updateHelper.AddElement("Teacher/Field", "Category", editor.Category);
                    updateHelper.AddElement("Teacher/Field", "Status", editor.Status.ToString());
                    updateHelper.AddElement("Teacher/Field", "ContactPhone", editor.ContactPhone);
                    updateHelper.AddElement("Teacher/Field", "TALoginName", editor.TALoginName);
                    updateHelper.AddElement("Teacher/Field", "TAPassword", editor.TAPassword);
                    updateHelper.AddElement("Teacher/Field", "AccountType", editor.AccountType);
                    updateHelper.AddElement("Teacher/Field", "Photo", editor.Photo);
                    updateHelper.AddElement("Teacher/Field", "Email", editor.Email);
                    updateHelper.AddElement("Teacher", "Condition");
                    updateHelper.AddElement("Teacher/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<TeacherRecord>> packages = worker.Run(TeacherRecords);

            foreach (PackageWorkEventArgs<TeacherRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆教師記錄
        /// </summary>
        /// <param name="TeacherRecords">多筆教師記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;TeacherRecord&gt; records = Teacher.SelectByIDs(CourseIDs);
        ///       int DeleteCount = Teacher.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<TeacherRecord> TeacherRecords)
        {
            List<string> Keys = new List<string>();

            foreach (TeacherRecord TeacherRecord in TeacherRecords)
                Keys.Add(TeacherRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除單筆教師記錄
        /// </summary>
        /// <param name="TeacherRecord">教師記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TeacherRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       TeacherRecord record = Teacher.SelectByID(TeacherID);
        ///       int DeleteCount = Teacher.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(TeacherRecord TeacherRecord)
        {
            return Delete(TeacherRecord.ID);
        }

        /// <summary>
        /// 刪除單筆教師記錄
        /// </summary>
        /// <param name="TeacherID">教師記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Teacher.Delete(TeacherID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string TeacherID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(TeacherID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆教師記錄
        /// </summary>
        /// <param name="TeacherIDs">多筆教師記錄編琥</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Teacher.Delete(TeacherIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> TeacherIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("DeleteRequest");

                foreach (string TeacherID in TeacherIDs)
                {
                    deleteHelper.AddElement("Teacher");
                    deleteHelper.AddElement("Teacher", "ID", TeacherID);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(TeacherIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(TeacherIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(TeacherIDs, ChangedSource.Local));


            return result;
        }

        /// <summary>
        /// 移除快取當中所有的資料
        /// </summary>
        public static void RemoveAll()
        {
            EntityCache.Clear();
        }

        /// <summary>
        /// 根據多筆教師編號移除快取資料。
        /// </summary>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <exception cref="Exception">
        /// </exception>
        public static void RemoveByIDs(IEnumerable<string> TeacherIDs)
        {
            EntityCache.Remove(TeacherIDs);
        }

        static void Teacher_RecordDeleted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static void Teacher_RecordUpdated(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));

        }

        static void Teacher_RecordInserted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
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