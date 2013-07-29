using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;
using FISCA.Synchronization;

namespace K12.Data
{
    /// <summary>
    /// 學生類別，提供方法用來取得、新增、修改及刪除學生資訊
    /// </summary>
    public class Student
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetAbstractListWithTag";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";
        private const string INSERT_SERVICENAME = "SmartSchool.Student.QuickInsert";
        private const string DELET_SERVICENAME = "SmartSchool.Student.Delete";
        private static EntityCache<XmlElement> EntityCache;
        private static DBChangeMonitor DBMonitor;

        /// <summary>
        /// Static建構式
        /// </summary>
        static Student()
        {
            EntityCache = new EntityCache<XmlElement>();
            //DBMonitor = K12DBChangMonitor.GetMonitor();
           
            //DBMonitor["student"].RecordInserted += new FISCA.Synchronization.TableChangedEventHandler(Student_RecordInserted);
            //DBMonitor["student"].RecordUpdated += new FISCA.Synchronization.TableChangedEventHandler(Student_RecordUpdated);
            //DBMonitor["student"].RecordDeleted += new FISCA.Synchronization.TableChangedEventHandler(Student_RecordDeleted);

            AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
            AfterDelete += new EventHandler<DataChangedEventArgs>(EntityCache.NotifyRemove);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")] 
        public static EntityCache<XmlElement> GetInternalCache()
        {
            return EntityCache;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")] 
        public static string GetBirthPlace(string StudentID)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "BirthPlace");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "ID", StudentID);
            dsreq.SetContent(helper);

            DSResponse dsrsp = DSAServices.CallService("SmartSchool.Student.GetDetailList", dsreq);

            return dsrsp.GetContent().GetText("Student/BirthPlace");

        }

        /// <summary>
        /// 根據班級記錄物件取得學生記錄編號列表。
        /// </summary>
        /// <param name="ClassRec">班級記錄物件</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectByClass(ClassRec);
        ///     
        ///     foreach(StudentRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        public static List<StudentRecord> SelectByClass(ClassRecord ClassRec)
        {
            return SelectByClassID<K12.Data.StudentRecord>(ClassRec.ID);
        }
        
        /// <summary>
        /// 根據班級編號取得學生記錄編號列表。
        /// </summary>
        /// <param name="ClassID">班級編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectByClassID(ClassID);
        ///     
        ///     foreach(StudentRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        public static List<StudentRecord> SelectByClassID(string ClassID)
        {
            return SelectByClassID<K12.Data.StudentRecord>(ClassID);
        }

        
        /// <summary>
        /// 根據班級編號取得學生記錄編號列表。
        /// </summary>
        /// <typeparam name="T">學生記錄物件型別，K12共用為K12.Data.StudentRecord</typeparam>
        /// <param name="ClassID">班級編號</param>
        /// <returns>List&lt;string&gt;，代表多筆學生記錄編號。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;string; IDs = Student.GetByClassID&lt;K12.Data.StudentRecord&gt;(ClassID);
        /// </example>
        protected static List<T> SelectByClassID<T>(string ClassID) where T : StudentRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ClassID);

            return SelectByClassIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆班級記錄物件取得學生記錄編號列表。
        /// </summary>
        /// <param name="ClassRecs">多筆班級記錄物件</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectByClasses(ClassRecs);
        ///     
        ///     foreach(StudentRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        public static List<StudentRecord> SelectByClasses(IEnumerable<ClassRecord> ClassRecs)
        {
            List<string> IDs = new List<string>();

            foreach (ClassRecord ClassRec in ClassRecs)
                IDs.Add(ClassRec.ID);

            return SelectByClassIDs<K12.Data.StudentRecord>(IDs);
        }
        
        /// <summary>
        /// 根據多筆班級編號取得學生記錄編號列表。
        /// </summary>
        /// <param name="ClassIDs">多筆班級編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectByClassIDs(ClassIDs);
        ///     
        ///     foreach(StudentRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        public static List<StudentRecord> SelectByClassIDs(IEnumerable<string> ClassIDs)
        {
            return SelectByClassIDs<K12.Data.StudentRecord>(ClassIDs);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByClassIDs<T>(IEnumerable<string> ClassIDs) where T : StudentRecord, new()
        {
            bool hasdata = false;

            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            foreach(string ClassID in ClassIDs)
                if (!string.IsNullOrEmpty(ClassID))
                {
                    helper.AddElement("Condition", "RefClassID", ClassID);
                    hasdata = true;
                }

            List<string> IDs = new List<string>();

            if (hasdata)
            {
                DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

                foreach (XmlElement element in rsp.GetContent().GetElements("Student"))
                {
                    DSXmlHelper studenthelper = new DSXmlHelper(element);
                    IDs.Add(studenthelper.GetText("@ID"));
                }
            }

            List<T> TypeStudents = new List<T>();

            foreach (System.Xml.XmlElement element in SelectCacheByIDs(IDs))
            {
                T TypeStudent = new T();
                TypeStudent.Load(element);
                TypeStudents.Add(TypeStudent);
            }

            return TypeStudents;
        }
        /// <summary>
        /// 取得所有學生記錄列表。
        /// </summary>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectAll();
        ///     
        ///     foreach(StudentRecord record in records)
        ///         System.Console.Writeln(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>
        /// 請先using K12.Data;
        /// </remarks>
        [SelectMethod("K12.Student.SelectAll","學籍.學生")]
        public static List<StudentRecord> SelectAll()
        {
            return SelectAll<K12.Data.StudentRecord>();
        }
        
        /// <summary>
        /// 取得所有學生記錄列表。
        /// </summary>
        /// <typeparam name="T">學生記錄物件型別，K12共用為K12.Data.StudentRecord</typeparam>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentRecord&gt; students = Student.SelectAll&lt;K12.Data.StudentRecord&gt;();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : StudentRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            List<string> IDs = new List<string>();

            foreach (XmlElement element in rsp.GetContent().GetElements("Student"))
            {
                DSXmlHelper studenthelper = new DSXmlHelper(element);
                IDs.Add(studenthelper.GetText("@ID"));
            }

            List<T> TypeStudents = new List<T>();

            foreach (System.Xml.XmlElement element in SelectCacheByIDs(IDs))
            {
                T TypeStudent = new T();
                TypeStudent.Load(element);
                TypeStudents.Add(TypeStudent);
            }

            return TypeStudents;
        }

        /// <summary>
        /// 根據單筆學生編號取得學生記錄。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>StudentRecord，代表學生記錄物件</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     StudentRecord record = Student.SelectByID(StudentID);
        ///     
        ///     if (record != null)
        ///         System.Console.WriteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        public static StudentRecord SelectByID(string StudentID)
        {
            return SelectByID<K12.Data.StudentRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生編號取得學生記錄。
        /// </summary>
        /// <typeparam name="T">學生記錄物件型別，K12共用為K12.Data.StudentRecord</typeparam>
        /// <param name="StudentID">學生編號</param>
        /// <returns>StudentRecord，代表學生記錄物件</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     StudentRecord StudentRec = Student.SelectByID&lt;K12.Data.StudentRecord&gt;(StudentID);
        /// </example>
        protected static T SelectByID<T>(string StudentID) where T : StudentRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(StudentID);

            List<T> Types = SelectByIDs<T>(IDs);

            if (Types.Count>0)
                return Types[0];
            else 
                return null;
        }

        /// <summary>
        /// 根據多筆學生編號取得學生記錄列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;StudentRecord&gt; records = Student.SelectByIDs(CourseIDs);
        ///     
        ///     foreach(StudentRecord record in records)
        ///         Console.WrlteLine(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<StudentRecord> SelectByIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByIDs<K12.Data.StudentRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生記錄列表。
        /// </summary>
        /// <typeparam name="T">學生記錄物件型別，K12共用為K12.Data.StudentRecord</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentRecord&gt; studentrecs = Student.SelectByIDs&lt;K12.Data.StudentRecord&gt;(StudentIDs);
        /// </example>
        protected static List<T> SelectByIDs<T>(IEnumerable<string> StudentIDs) where T : StudentRecord, new()
        {
            List<T> TypeStudents = new List<T>();

            foreach (System.Xml.XmlElement element in SelectCacheByIDs(StudentIDs))
            {
                T TypeStudent = new T();
                TypeStudent.Load(element);
                TypeStudents.Add(TypeStudent);
            }

            return TypeStudents;
        }

        /// <summary>
        /// 根據多筆學生編號取得學生記錄列表，供Cache使用。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentRecord&gt; studentrecs = Student.SelectByIDs(StudentIDs);
        /// </example>
        private static List<XmlElement> SelectCacheByIDs(IEnumerable<string> StudentIDs)
        {
            CacheSet<XmlElement> CacheSet = EntityCache.SelectByIDs(StudentIDs);

            //針對沒有存在Cache當中的資料再向直接要一次
            if (CacheSet.WantIDs.Count > 0)
            {
                List<XmlElement> NoCacheStudents = SelectDirectByIDs(CacheSet.WantIDs);

                CacheSet.Records.AddRange(NoCacheStudents);

                foreach (XmlElement Student in NoCacheStudents)
                    if (!EntityCache.ContainsKey(Student.GetAttribute("ID")))
                        EntityCache.Add(Student.GetAttribute("ID"), Student);
            }

            return CacheSet.Records;
        }

        /// <summary>
        /// 根據多筆學生編號取得學生記錄列表，供Cache，直接向DSA取得資料。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;StudentRecord&gt;，代表多筆學生記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;StudentRecord&gt; studentrecs = Student.SelectByIDs(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<XmlElement> SelectDirectByIDs(IEnumerable<string> StudentIDs)
        {
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "Status");
            helper.AddElement("Field", "SeatNo");
            helper.AddElement("Field", "Name");
            helper.AddElement("Field", "EnglishName");
            helper.AddElement("Field", "StudentNumber");
            helper.AddElement("Field", "Gender");
            helper.AddElement("Field", "IDNumber");
            helper.AddElement("Field", "Birthdate");
            helper.AddElement("Field", "BirthPlace");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefClassID");
            helper.AddElement("Field", "OverrideDeptID");
            helper.AddElement("Field", "RefGraduationPlanID");
            helper.AddElement("Field", "RefScoreCalcRuleID");
            helper.AddElement("Field", "Nationality");
            helper.AddElement("Field", "AccountType");
            helper.AddElement("Field", "SALoginName");
            helper.AddElement("Field", "SAPassword");

            helper.AddElement("Field", "EnrollmentCategory");
            helper.AddElement("Field", "HomeSchooling");
            helper.AddElement("Field", "Comment");

            helper.AddElement("Condition");
            foreach (var each in StudentIDs)
                helper.AddElement("Condition", "ID", each);

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            List<XmlElement> result = new List<XmlElement>();
            foreach (XmlElement element in rsp.GetContent().GetElements("Student"))
                result.Add(element);

            return result;
        }

        /// <summary>
        /// 新增單筆學生記錄
        /// </summary>
        /// <param name="StudentRecord">學生記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         StudentRecord newrecord = new StudentRecord();
        ///         newrecord.Name = (new System.Random()).NextDouble().ToString();
        ///         newrecord.Gender = "男";
        ///         strng NewID = Student.Insert(newrecord)
        ///         StudentRecord record = Student.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為課程名稱（Name）。
        /// </remarks>
        public static string Insert(StudentRecord StudentRecord)
        {
            List<StudentRecord> Params = new List<StudentRecord>();

            Params.Add(StudentRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生記錄
        /// </summary>
        /// <param name="StudentRecords">多筆班級記錄物件</param>
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         StudentRecord record = new StudentRecord();
        ///         record.Name = (new System.Random()).NextDouble().ToString();
        ///         List&lt;StudentRecord&gt; records = new List&lt;StudentRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Student.Insert(records)
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static List<string> Insert(IEnumerable<StudentRecord> StudentRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<StudentRecord> worker = new MultiThreadWorker<StudentRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<StudentRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Student");
                    helper.AddElement("Student", "Name", editor.Name);
                    helper.AddElement("Student", "EnglishName", editor.EnglishName);
                    helper.AddElement("Student", "Gender", editor.Gender);
                    helper.AddElement("Student", "BirthPlace", editor.BirthPlace);
                    helper.AddElement("Student","Birthdate",K12.Data.DateTimeHelper.ToDisplayString(editor.Birthday));
                    helper.AddElement("Student","IDNumber",editor.IDNumber);
                    helper.AddElement("Student","Nationality",editor.Nationality);
                    helper.AddElement("Student","OverrideDeptID",editor.OverrideDepartmentID);
                    helper.AddElement("Student", "RefGraduationPlanID", editor.OverrideProgramPlanID);
                    helper.AddElement("Student", "RefScoreCalcRuleID", editor.OverrideScoreCalcRuleID);
                    helper.AddElement("Student", "RefClassID", editor.RefClassID);
                    helper.AddElement("Student", "SeatNo", K12.Data.Int.GetString(editor.SeatNo));
                    helper.AddElement("Student", "Status", editor.Status.ToString());
                    helper.AddElement("Student", "StudentNumber", editor.StudentNumber);
                    helper.AddElement("Student", "AccountType", editor.AccountType);
                    helper.AddElement("Student", "SALoginName", editor.SALoginName);
                    helper.AddElement("Student", "SAPassword", editor.SAPassword);

                    helper.AddElement("Student", "EnrollmentCategory", editor.EnrollmentCategory);
                    helper.AddElement("Student", "HomeSchooling", ""+editor.HomeSchooling);
                    helper.AddElement("Student", "Comment", editor.Comment);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<StudentRecord>> packages = worker.Run(StudentRecords);

            foreach (PackageWorkEventArgs<StudentRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            //K12.Data.Utility.K12DBChangMonitor.NotifyClientChangeEntries(K12.Data.Utility.Utility.GetChangeEntries("student", ChangeAction.Insert,result));

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生記錄
        /// </summary>
        /// <param name="StudentRecord">學生記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     StudentRecord record = Student.SelectByID(StudentID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Student.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(StudentRecord StudentRecord)
        {
            List<StudentRecord> Params = new List<StudentRecord>();

            Params.Add(StudentRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生記錄
        /// </summary>
        /// <param name="StudentRecords">多筆學生記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     StudentRecord record = Student.SelectByID(StudentID);
        ///     record.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;StudentRecord&gt; records = new List&lt;StudentRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Student.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<StudentRecord> StudentRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<StudentRecord> worker = new MultiThreadWorker<StudentRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<StudentRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "Birthdate", K12.Data.DateTimeHelper.ToDisplayString(editor.Birthday));
                    updateHelper.AddElement("Student/Field", "BirthPlace", editor.BirthPlace);
                    updateHelper.AddElement("Student/Field", "Gender", editor.Gender);
                    updateHelper.AddElement("Student/Field", "IDNumber", editor.IDNumber);
                    updateHelper.AddElement("Student/Field", "Name", editor.Name);
                    updateHelper.AddElement("Student/Field", "EnglishName", editor.EnglishName);
                    updateHelper.AddElement("Student/Field", "SeatNo", K12.Data.Int.GetString(editor.SeatNo));
                    updateHelper.AddElement("Student/Field", "Status", editor.Status.ToString());
                    updateHelper.AddElement("Student/Field", "StudentNumber", editor.StudentNumber);
                    updateHelper.AddElement("Student/Field", "RefClassID", editor.RefClassID);
                    updateHelper.AddElement("Student/Field", "OverrideDeptID", editor.OverrideDepartmentID);
                    updateHelper.AddElement("Student/Field", "Nationality", editor.Nationality);
                    updateHelper.AddElement("Student/Field", "RefGraduationPlanID", editor.OverrideProgramPlanID);
                    updateHelper.AddElement("Student/Field", "RefScoreCalcRuleID",editor.OverrideScoreCalcRuleID);
                    updateHelper.AddElement("Student/Field", "AccountType", editor.AccountType);
                    updateHelper.AddElement("Student/Field", "SALoginName", editor.SALoginName);
                    updateHelper.AddElement("Student/Field", "SAPassword", editor.SAPassword);

                    updateHelper.AddElement("Student/Field", "EnrollmentCategory", editor.EnrollmentCategory);
                    updateHelper.AddElement("Student/Field", "HomeSchooling", ""+editor.HomeSchooling);
                    updateHelper.AddElement("Student/Field", "Comment", editor.Comment);

                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<StudentRecord>> packages = worker.Run(StudentRecords);

            foreach (PackageWorkEventArgs<StudentRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            //K12.Data.Utility.K12DBChangMonitor.NotifyClientChangeEntries(K12.Data.Utility.Utility.GetChangeEntries("student", ChangeAction.Update, IDs));

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除多筆學生記錄
        /// </summary>
        /// <param name="StudentRecords">多筆學生記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;StudentRecord&gt; records = Student.SelectByIDs(StudentIDs);
        ///       int DeleteCount = Student.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<StudentRecord> StudentRecords)
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord studentRecord in StudentRecords)
                Keys.Add(studentRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除單筆學生記錄
        /// </summary>
        /// <param name="StudentRecord">學生記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       StudentRecord record = Student.SelectByID(StudentID);
        ///       int DeleteCount = Student.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(StudentRecord StudentRecord)
        {
            return Delete(StudentRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生記錄
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Student.Delete(StudentID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string StudentID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生標籤記錄
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Student.Delete(StudentIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> StudentIDs)
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
                    helper.AddElement("Student");
                    helper.AddElement("Student", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(StudentIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            //K12.Data.Utility.K12DBChangMonitor.NotifyClientChangeEntries(K12.Data.Utility.Utility.GetChangeEntries("student", ChangeAction.Delete, StudentIDs));

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(StudentIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(StudentIDs, ChangedSource.Local));

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
        /// 根據多筆學生編號移除快取資料。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <exception cref="Exception">
        /// </exception>
        public static void RemoveByIDs(IEnumerable<string> StudentIDs)
        {
            EntityCache.Remove(StudentIDs);
        }

        static protected void Student_RecordDeleted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static protected void Student_RecordUpdated(object sender, FISCA.Synchronization.TableChangedEventArgs e)
        {
            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(e.PrimaryKeys, ChangedSource.Remote));
        }

        static protected void Student_RecordInserted(object sender, FISCA.Synchronization.TableChangedEventArgs e)
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