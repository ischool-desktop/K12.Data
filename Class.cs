using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 班級類別，提供方法用來取得、新增、修改及刪除班級資訊
    /// </summary>
    public class Class
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Class.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Class.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.Class.Insert";
        private const string DELET_SERVICENAME = "SmartSchool.Class.Delete";
        private static EntityCache<XmlElement> EntityCache;

        /// <summary>
        /// Static建構式
        /// </summary>
        static Class()
        {
            EntityCache = new EntityCache<XmlElement>();

            //App.DBMonitor["class"].RecordInserted += new FISCA.Synchronization.TableChangedEventHandler(Class_RecordInserted);
            //App.DBMonitor["class"].RecordUpdated += new FISCA.Synchronization.TableChangedEventHandler(Class_RecordUpdated);
            //App.DBMonitor["class"].RecordDeleted += new FISCA.Synchronization.TableChangedEventHandler(Class_RecordDeleted);

            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        static void Class_RecordDeleted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static void Class_RecordUpdated(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static void Class_RecordInserted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        /// <summary>
        /// 取得所有班級列表。
        /// </summary>
        /// <returns>List&lt;ClassRecord&gt;，代表班級物件列表。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ClassRecord&gt; records = Class.SelectAll();
        ///     
        ///     foreach(ClassRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        [SelectMethod("K12.Class.SelectAll", "學籍.班級")]
        public static List<ClassRecord> SelectAll()
        {
            return SelectAll<K12.Data.ClassRecord>();
        }
        
        /// <summary>
        /// 取得所有班級列表。
        /// </summary>
        /// <typeparam name="T">班級記錄物件型別，K12共用為K12.Data.ClassRecord</typeparam>
        /// <returns>ClassRecord，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassRecord&gt; classes = Class.SelectAll&lt;K12.Data.ClassRecord&gt;();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:ClassRecord,new()
        {
            string req = "<GetClassListRequest><Field><ID/></Field><Condition/><Order/></GetClassListRequest>";

            List<string> IDs = new List<string>();

            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req)).GetContent().GetElements("Class"))
                IDs.Add(item.GetAttribute("ID"));

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
        /// 根據單筆班級編號取得班級物件。
        /// </summary>
        /// <param name="ClassID">班級編號</param>
        /// <returns>ClassRecord，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ClassRecord&gt; records = Class.SelectAll();
        /// 
        ///    ClassRecord record = Class.SelectByID(records[(new System.Random()).Next(records.Count - 1)].ID);
        ///
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是ClassID不則在則會傳回null</remarks>
        public static ClassRecord SelectByID(string ClassID)
        {
            return SelectByID<ClassRecord>(ClassID);
        }

        /// <summary>
        /// 根據單筆班級編號取得班級物件。
        /// </summary>
        /// <typeparam name="T">班級記錄物件型別，K12共用為K12.Data.ClassRecord</typeparam>
        /// <param name="ClassID">班級編號</param>
        /// <returns>ClassRecord，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     ClassRecord ClassRec = Class.SelectByID&lt;K12.Data.ClassRecord&gt;(ClassID);
        /// </example>
        protected static T SelectByID<T>(string ClassID) where T:ClassRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ClassID);

            List<T> classes = SelectByIDs<T>(IDs);

            if (classes.Count > 0)
                return classes[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆班級編號取得班級物件列表。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassRecord&gt;，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ClassRecord&gt; allrecords = Class.SelectAll();
        ///     
        ///     List&lt;string&gt; IDs = new List&lt;string&gt;();
        ///     
        ///     foreach(ClassRecord record in allrecords)
        ///         IDs.Add(record.ID);
        /// 
        ///     List&lt;ClassRecord&gt; records = Class.SelectByIDs(IDs);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<ClassRecord> SelectByIDs(IEnumerable<string> ClassIDs)
        {
            return SelectByIDs<K12.Data.ClassRecord>(ClassIDs);
        }

        /// <summary>
        /// 根據多筆班級編號取得班級物件列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.ClassRecord</typeparam>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassRecord&gt;，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassRecord&gt; ClassRec = Class.SelectByIDs(classIDs);
        /// </example>
        protected static List<T> SelectByIDs<T>(IEnumerable<string> ClassIDs) where T : ClassRecord, new()
        {
            List<T> Types = new List<T>();

            foreach (System.Xml.XmlElement element in GetCacheData(ClassIDs))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆班級編號取得班級物件列表，供Cache使用。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassRecord&gt;，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassRecord&gt; ClassRec = Class.SelectByIDs(classIDs);
        /// </example>
        private static List<XmlElement> GetCacheData(IEnumerable<string> ClassIDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(ClassIDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheRecords = GetDirectData(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheRecords);

                foreach (XmlElement Class in NoCacheRecords)
                    if (!EntityCache.ContainsKey(Class.GetAttribute("ID")))
                        EntityCache.Add(Class.GetAttribute("ID"),Class);
            }

            return CacheSet.Records;
        }

        /// <summary>
        /// 根據多筆班級編號取得班級物件列表。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;ClassRecord&gt;，代表班級物件。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ClassRecord&gt; ClassRec = Class.SelectByIDs(classIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> ClassIDs)
        {
            List<XmlElement> result = new List<XmlElement>();

            bool haskey = false;
            string req = "<GetClassListRequest><Field><All/></Field><Condition>";
            foreach (string key in ClassIDs)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    req += "<ID>" + key + "</ID>";
                    haskey = true;
                }
            }
            req += "</Condition><Order></Order></GetClassListRequest>";

            if (haskey)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req)).GetContent().GetElements("Class"))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 新增單筆班級記錄
        /// </summary>
        /// <param name="ClassRecord">班級記錄物件</param> 
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         ClassRecord newrecord = new ClassRecord();
        ///         newrecord.Name = (new System.Random()).NextDouble().ToString();
        ///         strng NewID = Class.Insert(newrecord)
        ///         ClassRecord record = Class.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為課程名稱（Name）。
        /// </remarks>
        public static string Insert(ClassRecord ClassRecord)
        {
            List<ClassRecord> Params = new List<ClassRecord>();

            Params.Add(ClassRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆班級記錄
        /// </summary>
        /// <param name="ClassRecords">多筆班級記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         JHClassRecord record = new JHClassRecord();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         List&lt;JHClassRecord&gt; records = new List&lt;JHClassRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = JHClass.Insert(records);
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<ClassRecord> ClassRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<ClassRecord> worker = new MultiThreadWorker<ClassRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ClassRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Class");
                    helper.AddElement("Class", "Field");
                    helper.AddElement("Class/Field", "ClassName", editor.Name);
                    helper.AddElement("Class/Field", "NamingRule", editor.NamingRule);
                    helper.AddElement("Class/Field", "GradeYear", K12.Data.Int.GetString(editor.GradeYear));
                    helper.AddElement("Class/Field", "RefGraduationPlanID", editor.RefProgramPlanID);
                    helper.AddElement("Class/Field", "RefScoreCalcRuleID", editor.RefScoreCalcRuleID);
                    helper.AddElement("Class/Field", "RefTeacherID", editor.RefTeacherID);
                    helper.AddElement("Class/Field", "DisplayOrder", editor.DisplayOrder);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<ClassRecord>> packages = worker.Run(ClassRecords);

            foreach (PackageWorkEventArgs<ClassRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));
            return result;
        }

        /// <summary>
        /// 更新單筆班級記錄
        /// </summary>
        /// <param name="ClassRecord">班級記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ClassRecord record = Class.SelectByID(ClassID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Class.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(ClassRecord ClassRecord)
        {
            List<ClassRecord> Params = new List<ClassRecord>();

            Params.Add(ClassRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆班級記錄
        /// </summary>
        /// <param name="ClassRecords">班級記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ClassRecord record = Class.SelectByID(ClassID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;ClassRecord&gt; records = new List&lt;ClassRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Class.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ClassRecord> ClassRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ClassRecord> worker = new MultiThreadWorker<ClassRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ClassRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Class");
                    updateHelper.AddElement("Class", "Field");
                    updateHelper.AddElement("Class/Field", "ClassName", editor.Name);
                    updateHelper.AddElement("Class/Field", "NamingRule", editor.NamingRule);
                    updateHelper.AddElement("Class/Field", "GradeYear", K12.Data.Int.GetString(editor.GradeYear));
                    updateHelper.AddElement("Class/Field", "RefGraduationPlanID", editor.RefProgramPlanID);
                    updateHelper.AddElement("Class/Field", "RefScoreCalcRuleID", editor.RefScoreCalcRuleID);
                    updateHelper.AddElement("Class/Field", "RefTeacherID", editor.RefTeacherID);
                    updateHelper.AddElement("Class/Field", "DisplayOrder", editor.DisplayOrder);
                    updateHelper.AddElement("Class", "Condition");
                    updateHelper.AddElement("Class/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ClassRecord>> packages = worker.Run(ClassRecords);

            foreach (PackageWorkEventArgs<ClassRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆班級記錄
        /// </summary>
        /// <param name="ClassRecords">多筆班級記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;ClassRecord&gt; records = Class.SelectByIDs(ClassIDs);
        ///       int DeleteCount = Class.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<ClassRecord> ClassRecords)
        {
            List<string> Keys = new List<string>();

            foreach (ClassRecord ClassRecord in ClassRecords)
                Keys.Add(ClassRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除單筆班級記錄
        /// </summary>
        /// <param name="ClassRecord">班級記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;ClassRecord&gt; record = Class.SelectByID(ClassID);
        ///       int DeleteCount = Class.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(ClassRecord ClassRecord)
        {
            return Delete(ClassRecord.ID);
        }

        /// <summary>
        /// 刪除單筆班級記錄
        /// </summary>
        /// <param name="ClassID">班級記錄系統編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Class.Delete(ClassID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string ClassID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(ClassID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆班級記錄
        /// </summary>
        /// <param name="ClassIDs">多筆班級記錄系統編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Class.Delete(ClassIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> ClassIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("Request");

                foreach (string Key in e.List)
                {
                    deleteHelper.AddElement("Class");
                    deleteHelper.AddElement("Class", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(ClassIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            //K12.Data.Configuration.BigEvent be = new K12.Data.Configuration.BigEvent(AfterDelete, null, new DataChangedEventArgs(ClassIDs, ChangedSource.Local));
            
            //be.Raise();

            //AfterDelete.GetInvocationList()[0]

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(ClassIDs, ChangedSource.Local));
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(ClassIDs, ChangedSource.Local));


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
        /// 根據多筆班級編號移除快取資料。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <exception cref="Exception">
        /// </exception>
        public static void RemoveByIDs(IEnumerable<string> ClassIDs)
        {
            EntityCache.Remove(ClassIDs);
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