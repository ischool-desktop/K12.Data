using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 課程類別，提供方法用來取得、新增、修改及刪除課程資訊
    /// </summary>
    public class Course
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Course.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Course.Update";
        private const string INSERT_SERVICENAME = "SmartSchool.Course.Insert";
        private const string DELET_SERVICENAME = "SmartSchool.Course.Delete";
        private static EntityCache<XmlElement> EntityCache;

        /// <summary>
        /// Static建構式
        /// </summary>
        static Course()
        {
            EntityCache = new EntityCache<XmlElement>();

            //App.DBMonitor["course"].RecordInserted += new FISCA.Synchronization.TableChangedEventHandler(Course_RecordInserted);
            //App.DBMonitor["course"].RecordUpdated += new FISCA.Synchronization.TableChangedEventHandler(Course_RecordUpdated);
            //App.DBMonitor["course"].RecordDeleted += new FISCA.Synchronization.TableChangedEventHandler(Course_RecordDeleted);

            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        static void Course_RecordDeleted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static void Course_RecordUpdated(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static void Course_RecordInserted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        /// <summary>
        /// 取得所有課程列表。
        /// </summary>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectAll();
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        [SelectMethod("K12.Course.SelectAll", "學籍.課程")]
        public static List<CourseRecord> SelectAll()
        {
            return SelectAll<K12.Data.CourseRecord>();
        }

        /// <summary>
        /// 取得所有課程列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;CourseRecord&gt; courserecs = Course.SelectAll&lt;K12.Data.CourseRecord&gt;();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>()  where T:CourseRecord,new()
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetDetailListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");
            helper.AddElement("Order");
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            List<string> CourseIDs = new List<string>();

            foreach (XmlElement var in dsrsp.GetContent().GetElements("Course"))
                CourseIDs.Add(var.GetAttribute("ID"));

            return SelectByIDs<T>(CourseIDs);

            //List<T> Types = new List<T>();

            //foreach (System.Xml.XmlElement element in GetCacheData(IDs))
            //{
            //    T Type = new T();
            //    Type.Load(element);
            //    Types.Add(Type);
            //}

            //return Types;
        }

        /// <summary>
        /// 根據學年度、學期及班級記錄物件列表取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="ClassRecs">多筆班級記錄物件</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <seealso cref="ClassRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///   <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectByClass(SchoolYear,Semester,ClassRecs);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///    </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectByClass(int? SchoolYear, int? Semester, IEnumerable<ClassRecord> ClassRecs)
        {
            List<string> IDs = new List<string>();

            foreach (ClassRecord ClassRec in ClassRecs)
                IDs.Add(ClassRec.ID);

            return SelectByClass<K12.Data.CourseRecord>(SchoolYear, Semester, IDs);
        }

        /// <summary>
        /// 根據學年度、學期及班級記錄編號列表取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="ClassIDs">多筆班級記錄編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///   <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectByClass(SchoolYear,Semester,ClassIDs);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///    </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectByClass(int? SchoolYear, int? Semester, IEnumerable<string> ClassIDs)
        {
            return SelectByClass<K12.Data.CourseRecord>(SchoolYear, Semester,ClassIDs);
        }

        
        /// <summary>
        /// 根據學年度、學期及班級記錄物件取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="ClassRec">班級記錄物件</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///   <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectByClass(SchoolYear,Semester,ClassRec);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///    </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectByClass(int? SchoolYear, int? Semester, ClassRecord ClassRec)
        {
            List<string> IDs = new List<string>();

            IDs.Add(ClassRec.ID);

            return SelectByClass<K12.Data.CourseRecord>(SchoolYear, Semester, IDs);
        }
        
        /// <summary>
        /// 根據學年度、學期及班級記錄編號取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="ClassID">班級記錄編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///   <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectByClass(SchoolYear,Semester,ClassID);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///    </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectByClass(int? SchoolYear, int? Semester,string ClassID)
        {
            List<string> IDs=new List<string>();

            IDs.Add(ClassID);

            return SelectByClass<K12.Data.CourseRecord>(SchoolYear, Semester, IDs);
        }

        /// <summary>
        /// 根據學年度、學期及取得課程列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseRecord&gt; records = Course.SelectBySchoolYearAndSemester&lt;K12.Data.CourseRecord&gt;(SchoolYear,Semester);
        ///
        ///       foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByClass<T>(int? SchoolYear, int? Semester,IEnumerable<string> ClassIDs) where T : CourseRecord, new()
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetDetailListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            if (SchoolYear != null)
                helper.AddElement("Condition", "SchoolYear", K12.Data.Int.GetString(SchoolYear));

            if (Semester != null)
                helper.AddElement("Condition", "Semester", K12.Data.Int.GetString(Semester));

            helper.AddElement("Condition", "RefClassIDList");

            foreach (string ClassID in ClassIDs)
                if (!string.IsNullOrEmpty(ClassID))
                    helper.AddElement("Condition/RefClassIDList", "RefClassID", ClassID);

            helper.AddElement("Order");
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            List<string> CourseIDs = new List<string>();

            foreach (XmlElement var in dsrsp.GetContent().GetElements("Course"))
                CourseIDs.Add(var.GetAttribute("ID"));

            return SelectByIDs<T>(CourseIDs);

            //List<T> Types = new List<T>();

            //foreach (System.Xml.XmlElement element in GetCacheData(IDs))
            //{
            //    T Type = new T();
            //    Type.Load(element);
            //    Types.Add(Type);
            //}

            //return Types;
        }

        /// <summary>
        /// 根據學年度及學期取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///   <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectBySchoolYearAndSemester(SchoolYear,Semester);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///    </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectBySchoolYearAndSemester(int? SchoolYear, int? Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.CourseRecord>(SchoolYear, Semester);
        }

        /// <summary>
        /// 根據學年度及學期取得課程列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseRecord&gt; records = Course.SelectBySchoolYearAndSemester&lt;K12.Data.CourseRecord&gt;(SchoolYear,Semester);
        ///
        ///       foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectBySchoolYearAndSemester<T>(int? SchoolYear,int? Semester) where T:CourseRecord,new()
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetDetailListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            if (SchoolYear != null)
                helper.AddElement("Condition", "SchoolYear",K12.Data.Int.GetString(SchoolYear));

            if (Semester != null)
                helper.AddElement("Condition","Semester",K12.Data.Int.GetString(Semester));

            helper.AddElement("Order");
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            List<string> CourseIDs = new List<string>();

            foreach (XmlElement var in dsrsp.GetContent().GetElements("Course"))
                CourseIDs.Add(var.GetAttribute("ID"));

            return SelectByIDs<T>(CourseIDs);

            //List<T> Types = new List<T>();

            //foreach (System.Xml.XmlElement element in GetCacheData(IDs))
            //{
            //    T Type = new T();
            //    Type.Load(element);
            //    Types.Add(Type);
            //}

            //return Types;
        }

        /// <summary>
        /// 根據學年度、學期及課程名稱取得課程列表。
        /// </summary>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="CourseName">課程名稱</param> 
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseRecord&gt; records = Course.SelectBySchoolYearAndSemester(SchoolYear,Semester,CourseName);
        ///     
        ///       foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks></remarks>
        public static List<CourseRecord> SelectBySchoolYearAndSemester(int? SchoolYear, int? Semester, string CourseName)
        {
            return SelectBySchoolYearAndSemester<K12.Data.CourseRecord>(SchoolYear, Semester, CourseName);
        }

        /// <summary>
        /// 根據學年度、學期及課程名稱取得課程列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <param name="CourseName">課程名稱</param> 
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseRecord&gt; records = Course.SelectBySchoolYearAndSemester&lt;K12.Data.CourseRecord&gt;(SchoolYear,Semester,CourseName);

        ///       foreach(CourseRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        protected static List<T> SelectBySchoolYearAndSemester<T>(int? SchoolYear, int? Semester,string CourseName) where T:CourseRecord,new()
        {
            List<T> Courses = SelectBySchoolYearAndSemester<T>(SchoolYear,Semester);

            List<T> NameConditionCourses = new List<T>();

            foreach (T Course in Courses)
                if (Course.Equals(CourseName))
                    NameConditionCourses.Add(Course);

            return NameConditionCourses;
        }

        /// <summary>
        /// 根據單筆課程編號取得課程物件。
        /// </summary>
        /// <param name="CourseID">課程編號</param>
        /// <returns>CourseRecord，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     CourseRecord record = Course.SelectByID(CourseID);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是CourseID不則在則會傳回null</remarks>
        public static CourseRecord SelectByID(string CourseID)
        {
            return SelectByID<K12.Data.CourseRecord>(CourseID);
        }


        /// <summary>
        /// 根據單筆課程編號取得課程物件。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <param name="CourseID">課程編號</param>
        /// <returns>CourseRecord，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     CourseRecord CourseRec = Course.SelectByID&lt;K12.Data.CourseRecord&gt;(CourseID);
        /// </example>
        protected static T SelectByID<T>(string CourseID) where T:CourseRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(CourseID);

            List<T> courseRecords = SelectByIDs<T>(IDs);

            if (courseRecords.Count > 0)
                return courseRecords[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆課程編號取得課程物件列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;CourseRecord&gt; records = Course.SelectByIDs(CourseIDs);
        ///     
        ///     foreach(CourseRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<CourseRecord> SelectByIDs(IEnumerable<string> CourseIDs)
        {
            return SelectByIDs<K12.Data.CourseRecord>(CourseIDs);
        }


        /// <summary>
        /// 根據多筆課程編號取得課程物件列表。
        /// </summary>
        /// <typeparam name="T">課程記錄物件型別，K12共用為K12.Data.CourseRecord</typeparam>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;CourseRecord&gt; CourseRec = Course.SelectByIDs&lt;K12.Data.CourseRecord&gt;(courseIDs);
        ///     </code>
        /// </example>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> CourseIDs) where T:CourseRecord,new()
        {
            List<T> Types = new List<T>();

            if (K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                return Types;

            //避免有重覆的ID會向Server重覆要資料
            CourseIDs = CourseIDs.Distinct();

            FunctionSpliter<string, XmlElement> CacheSpliter = new FunctionSpliter<string, XmlElement>(1000, 3);

            CacheSpliter.Function = GetCacheData;
            List<XmlElement> Elements = CacheSpliter.Execute(CourseIDs.ToList());

            foreach (System.Xml.XmlElement element in Elements)
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆課程編號取得課程物件列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;CourseRecord&gt; CourseRec = Course.SelectByIDs(courseIDs);
        ///     </code>
        /// </example>
        private static List<XmlElement> GetCacheData(IEnumerable<string> CourseIDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(CourseIDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheRecords = GetDirectData(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheRecords);

                foreach (XmlElement Course in NoCacheRecords)
                    if (!EntityCache.ContainsKey(Course.GetAttribute("ID")))
                        EntityCache.Add(Course.GetAttribute("ID"), Course);
            }

            return CacheSet.Records;
        }

        /// <summary>
        /// 根據多筆課程編號取得課程物件列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;CourseRecord&gt;，代表課程物件。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;CourseRecord&gt; CourseRec = Course.SelectByIDs(courseIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> GetDirectData(IEnumerable<string> CourseIDs)
        {
            bool hasKey = false;
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetDetailListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");
            
            foreach (var key in CourseIDs)
            {
                helper.AddElement("Condition", "ID", key);
                hasKey = true;
            }
            
            helper.AddElement("Order");
            List<XmlElement> result = new List<XmlElement>();
            if (hasKey)
            {
                dsreq.SetContent(helper);
                DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);
                foreach (XmlElement var in dsrsp.GetContent().GetElements("Course"))
                {
                    result.Add(var);
                }
            }
            return result;
        }

        /// <summary>
        /// 新增單筆課程記錄
        /// </summary>
        /// <param name="CourseRecord">課程記錄物件</param> 
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         CourseRecord record = new CourseRecord();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         strng NewID = Course.Insert(record);
        ///         CourseRecord actual = Course.SelectByID(NewID);
        ///         System.Console.Writeln(actual.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為課程名稱（Name）。
        /// </remarks>
        public static string Insert(CourseRecord CourseRecord)
        {
            List<CourseRecord> Params = new List<CourseRecord>();

            Params.Add(CourseRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆課程記錄
        /// </summary>
        /// <param name="CourseRecords">課程記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         CourseRecord record = new CourseRecord();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         List&lt;CourseRecord&gt; records = new List&lt;CourseRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Course.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<CourseRecord> CourseRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<CourseRecord> worker = new MultiThreadWorker<CourseRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<CourseRecord> e)
            {
                DSXmlHelper insertHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    insertHelper.AddElement("Course");
                    insertHelper.AddElement("Course", "CourseName", editor.Name);
                    insertHelper.AddElement("Course", "SchoolYear", editor.SchoolYear.ToString());
                    insertHelper.AddElement("Course", "Semester", editor.Semester.ToString());
                    insertHelper.AddElement("Course", "Subject", editor.Subject);
                    insertHelper.AddElement("Course", "SubjectLevel", K12.Data.Decimal.GetString(editor.Level));
                    insertHelper.AddElement("Course", "Period", K12.Data.Decimal.GetString(editor.Period));
                    insertHelper.AddElement("Course", "Credit", K12.Data.Decimal.GetString(editor.Credit));
                    insertHelper.AddElement("Course", "Domain", editor.Domain);
                    insertHelper.AddElement("Course", "ScoreCalcFlag", editor.CalculationFlag);
                    insertHelper.AddElement("Course", "RefClassID", editor.RefClassID);
                    insertHelper.AddElement("Course", "RefExamTemplateID", editor.RefAssessmentSetupID);
                    insertHelper.AddElement("Course", "IsRequired", editor.Required.Equals(true) ? "必" : "選");
                    insertHelper.AddElement("Course", "RequiredBy", editor.RequiredBy);
                    insertHelper.AddElement("Course", "NotIncludedInCredit", editor.NotIncludedInCredit.Equals(true) ? "是" : "否");
                    insertHelper.AddElement("Course", "NotIncludedInCalc", editor.NotIncludedInCalc.Equals(true) ? "是" : "否");
                    insertHelper.AddElement("Course", "ScoreType", editor.Entry);                    
                    insertHelper.AddElement("Course", "Extensions");
                    if (editor.Extensions!=null)
                        insertHelper.AddElement("Course/Extensions", editor.Extensions);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(insertHelper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);
            };

            List<PackageWorkEventArgs<CourseRecord>> packages = worker.Run(CourseRecords);

            foreach (PackageWorkEventArgs<CourseRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆課程記錄
        /// </summary>
        /// <param name="CourseRecord">課程記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     CourseRecord record = Course.SelectByID(CourseID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Course.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(CourseRecord CourseRecord)
        {
            List<CourseRecord> Params = new List<CourseRecord>();

            Params.Add(CourseRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆課程記錄
        /// </summary>
        /// <param name="CourseRecords">課程記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     CourseRecord record = Course.SelectByID(CourseID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;CourseRecord&gt; records = new List&lt;CourseRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Course.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<CourseRecord> CourseRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<CourseRecord> worker = new MultiThreadWorker<CourseRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<CourseRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Course");
                    updateHelper.AddElement("Course", "Field");
                    updateHelper.AddElement("Course/Field", "CourseName", editor.Name);
                    updateHelper.AddElement("Course/Field", "SchoolYear", editor.SchoolYear.ToString());
                    updateHelper.AddElement("Course/Field", "Semester", editor.Semester.ToString());
                    updateHelper.AddElement("Course/Field", "Subject", editor.Subject);
                    updateHelper.AddElement("Course/Field", "SubjectLevel", K12.Data.Decimal.GetString(editor.Level));
                    updateHelper.AddElement("Course/Field", "Domain", editor.Domain);
                    updateHelper.AddElement("Course/Field", "ScoreCalcFlag", editor.CalculationFlag);
                    updateHelper.AddElement("Course/Field", "Period", K12.Data.Decimal.GetString(editor.Period));
                    updateHelper.AddElement("Course/Field", "Credit", K12.Data.Decimal.GetString(editor.Credit));
                    updateHelper.AddElement("Course/Field", "RefClassID", editor.RefClassID);
                    updateHelper.AddElement("Course/Field", "RefExamTemplateID", editor.RefAssessmentSetupID);

                    updateHelper.AddElement("Course/Field", "IsRequired", editor.Required.Equals(true) ? "必" : "選");
                    updateHelper.AddElement("Course/Field", "RequiredBy", editor.RequiredBy);
                    updateHelper.AddElement("Course/Field", "NotIncludedInCredit", editor.NotIncludedInCredit.Equals(true) ? "是" : "否");
                    updateHelper.AddElement("Course/Field", "NotIncludedInCalc", editor.NotIncludedInCalc.Equals(true) ? "是" : "否");
                    updateHelper.AddElement("Course/Field", "ScoreType", editor.Entry);                    

                    updateHelper.AddElement("Course/Field", "Extensions");
                    if (editor.Extensions!=null)
                        updateHelper.AddElement("Course/Field/Extensions",editor.Extensions);
                    updateHelper.AddElement("Course", "Condition");
                    updateHelper.AddElement("Course/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<CourseRecord>> packages = worker.Run(CourseRecords);

            foreach (PackageWorkEventArgs<CourseRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆課程記錄
        /// </summary>
        /// <param name="CourseRecords">多筆課程記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;CourseRecord&gt; records = Course.SelectByIDs(CourseIDs);
        ///       int DeleteCount = Course.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<CourseRecord> CourseRecords)
        {
            List<string> Keys = new List<string>();

            foreach (CourseRecord CourseRecord in CourseRecords)
                Keys.Add(CourseRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除單筆課程記錄
        /// </summary>
        /// <param name="CourseRecord">課程記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="CourseRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       CourseRecord record = Course.SelectByID(CourseID);
        ///       int DeleteCount = Course.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(CourseRecord CourseRecord)
        {
            return Delete(CourseRecord.ID);
        }

        /// <summary>
        /// 刪除單筆課程記錄
        /// </summary>
        /// <param name="CourseID">課程記錄系統編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Course.Delete(CourseID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string CourseID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(CourseID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆課程記錄
        /// </summary>
        /// <param name="CourseIDs">多筆課程記錄系統編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Course.Delete(CourseIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> CourseIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (string courseID in CourseIDs)
                {
                    helper.AddElement("Course");
                    helper.AddElement("Course", "ID", courseID);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(CourseIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(CourseIDs , ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(CourseIDs, ChangedSource.Local));

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
        /// 根據多筆課程編號移除快取資料。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <exception cref="Exception">
        /// </exception>
        public static void RemoveByIDs(IEnumerable<string> CourseIDs)
        {
            EntityCache.Remove(CourseIDs);
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