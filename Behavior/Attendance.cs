using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生缺曠類別，提供方法用來取得、新增、修改及刪除學生缺曠資訊
    /// </summary>
    public class Attendance
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.Attendance.GetAttendance";
        private const string INSERT_SERVICENAME = "SmartSchool.Student.Attendance.Insert";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.Attendance.Update";
        private const string DELETE_SERVICENAME = "SmartSchool.Student.Attendance.Delete";

        [SelectMethod("K12.Attendance.SelectAllAttendancePeriod", "學務.缺曠")]
        public static List<AttendancePeriod> SelectAllAttendancePeriod()
        {
            List<AttendanceRecord> Attendances = SelectAll();

            List<AttendancePeriod> Periods = new List<AttendancePeriod>();

            for(int i=0;i<Attendances.Count;i++)
                Periods.AddRange(Attendances[i].PeriodDetail);

            return Periods;
        }

        /// <summary>
        /// 取得所有缺曠紀錄
        /// </summary>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectAll();
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>    
        /// <exception cref="Exception">
        /// </exception>
        public static List<AttendanceRecord> SelectAll()
        {
            return SelectAll<K12.Data.AttendanceRecord>();
        }

        /// <summary>
        /// 取得所有缺曠紀錄
        /// </summary>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectAll();
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>    
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : AttendanceRecord, new()
        {
            List<T> Types = new List<T>();

            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition/></Request>");

            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Attendance"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據條件取得缺曠紀錄列表
        /// </summary>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">缺曠發生開始時間</param>
        /// <param name="EndDate">缺曠發生結束時間</param>
        /// <param name="OccurDate">缺曠發生時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        public static List<AttendanceRecord> Select(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? OccurDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters)
        {
            return Select<K12.Data.AttendanceRecord>(StudentIDs,StartDate,EndDate,OccurDate,SchoolYears,Semesters);
        }

        /// <summary>
        /// 根據條件取得缺曠紀錄列表
        /// </summary>
        /// <typeparam name="T">缺曠紀錄型別，繼承至K12.Data.AttendanceRecord</typeparam>
        /// <param name="StartDate">缺曠發生開始時間</param>
        /// <param name="EndDate">缺曠發生結束時間</param>
        /// <param name="OccurDate">缺曠發生時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        protected static List<T> Select<T>(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? OccurDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters) where T : AttendanceRecord, new()
        {
            List<DateTime> OccurDates = new List<DateTime>();

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(OccurDate))
                OccurDates.Add(OccurDate.Value);

            return Select<T>(StudentIDs, StartDate, EndDate, OccurDates, SchoolYears, Semesters,null);
        }

        /// <summary>
        /// 根據條件取得缺曠紀錄列表
        /// </summary>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">缺曠發生開始時間</param>
        /// <param name="EndDate">缺曠發生結束時間</param>
        /// <param name="OccurDates">缺曠發生時間列表</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <param name="SchoolYearSemesters">學年度學期列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        public static List<AttendanceRecord> Select(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, IEnumerable<DateTime> OccurDates, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters, IEnumerable<SchoolYearSemester> SchoolYearSemesters)
        {
            return Select<AttendanceRecord>(StudentIDs, StartDate, EndDate, OccurDates, SchoolYears, Semesters, SchoolYearSemesters);
        }

        /// <summary>
        /// 根據條件取得缺曠紀錄列表
        /// </summary>
        /// <typeparam name="T">缺曠紀錄型別，繼承至K12.Data.AttendanceRecord</typeparam>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">缺曠發生開始時間</param>
        /// <param name="EndDate">缺曠發生結束時間</param>
        /// <param name="OccurDates">缺曠發生時間列表</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <param name="SchoolYearSemesters">學年度學期列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, IEnumerable<DateTime> OccurDates, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters, IEnumerable<SchoolYearSemester> SchoolYearSemesters) where T : AttendanceRecord, new()
        {
            bool IsSendRequest = false;

            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
                foreach (string StudentID in StudentIDs)
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        req.Append("<RefStudentID>" + StudentID + "</RefStudentID>");
                        IsSendRequest = true;
                    }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(StartDate))
            {
                req.Append("<StartDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartDate) + "</StartDate>");
                IsSendRequest = true;
            }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(EndDate))
            {
                req.Append("<EndDate>"+ K12.Data.DateTimeHelper.ToDisplayString(EndDate)+"</EndDate>");
                IsSendRequest = true;
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(OccurDates))
            {
                foreach (DateTime OccurDate in OccurDates)
                {
                    req.Append("<OccurDate>" + K12.Data.DateTimeHelper.ToDisplayString(OccurDate) + "</OccurDate>");
                    IsSendRequest = true;
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SchoolYears))
            {
                foreach (int SchoolYear in SchoolYears)
                {
                    req.Append("<SchoolYear>" + SchoolYear + "</SchoolYear>");
                    IsSendRequest = true; 
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Semesters))
            {
                foreach (int Semester in Semesters)
                {
                    req.Append("<Semester>"+Semester+"</Semester>");
                    IsSendRequest = true;                        
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SchoolYearSemesters))
            {
                req.Append("<Or>");

                foreach (SchoolYearSemester schoolYearSemester in SchoolYearSemesters)
                {
                    req.Append("<And>");
                    req.Append("<SchoolYear>"+schoolYearSemester.SchoolYear+"</SchoolYear>");
                    req.Append("<Semester>" + schoolYearSemester.Semester + "</Semester>");
                    req.Append("</And>");
                }

                req.Append("</Or>");

                IsSendRequest = true;
            }

            req.Append("</Condition><Order><SchoolYear/><Semester/><OccurDate/></Order></Request>");

            List<T> Types = new List<T>();

            if (IsSendRequest == true)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Attendance"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 取得指定學生在某學年度學期的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectBySchoolYearAndSemester(Students, 98,1);
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>    
        /// <exception cref="Exception">
        /// </exception>
        public static List<AttendanceRecord> SelectBySchoolYearAndSemester(IEnumerable<StudentRecord> Students,
                                                                        int? SchoolYear,
                                                                        int? Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.AttendanceRecord>(Students,SchoolYear,Semester);
        }

        /// <summary>
        /// 取得指定學生在某學年度學期的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     List&lt;StudentRecord&gt; students = Student.Instance.SelectedList;
        ///     List&lt;AttendanceRecord&gt; records = Attendance.GetByGetBySchoolYearAndSemester(students, 98,1);
        /// </example>    
        /// <exception cref="Exception">
        /// </exception>
        protected static List<T> SelectBySchoolYearAndSemester<T>(IEnumerable<StudentRecord> Students,
                                                                        int? SchoolYear,
                                                                        int? Semester) where T:AttendanceRecord ,new()
        {
            
            List<string> StudentIDs = new List<string>();
            List<int> SchoolYears = new List<int>();
            List<int> Semesters = new List<int>();

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Students))
                foreach (StudentRecord StudentRec in Students)                
                    if (!string.IsNullOrEmpty(StudentRec.ID))
                        StudentIDs.Add(StudentRec.ID);

            if (!K12.Data.Int.IsNullOrEmpty(SchoolYear))
                SchoolYears.Add(SchoolYear.Value);

            if (!K12.Data.Int.IsNullOrEmpty(Semester))
                Semesters.Add(Semester.Value);

            return Select<T>(StudentIDs, null, null, null, SchoolYears, Semesters);
        }

        /// <summary>
        /// 取得小於指定學年度及學期的資料
        /// </summary>
        /// <param name="StudentIDs">學生系統編號列表</param>
        /// <param name="SchoolYearSemester">學年度學期</param>
        /// <returns></returns>
        public static List<AttendanceRecord> SelectBySchoolYearAndSemesterLessEqual(IEnumerable<string> StudentIDs,SchoolYearSemester SchoolYearSemester)
        {
            return SelectBySchoolYearAndSemesterLessEqual<K12.Data.AttendanceRecord>(StudentIDs,SchoolYearSemester);
        }

        /// <summary>
        /// 取得小於指定學年度及學期的資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StudentIDs">學生系統編號列表</param>
        /// <param name="SchoolYearSemester">學年度學期</param>
        /// <returns></returns>
        protected static List<T> SelectBySchoolYearAndSemesterLessEqual<T>(IEnumerable<string> StudentIDs, SchoolYearSemester SchoolYearSemester) where T : AttendanceRecord, new()
        {
            bool IsSendRequest = false;

            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
                foreach (string StudentID in StudentIDs)
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        req.Append("<RefStudentID>" + StudentID + "</RefStudentID>");
                        IsSendRequest = true;
                    }

            string strSchoolYearSemesterCondition = SchoolYearSemester.ToLessThanRequest();

            if (!string.IsNullOrEmpty(strSchoolYearSemesterCondition))
            {
                req.Append(strSchoolYearSemesterCondition);
                IsSendRequest = true;
            }

            req.Append("</Condition><Order><OccurDate>desc</OccurDate><RefStudentID/></Order></Request>");

            List<T> Types = new List<T>();

            if (IsSendRequest == true)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Attendance"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 取得指定學生在日期區間的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <param name="BeginDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <remarks></remarks>
        /// <example>
        ///     <code>
        ///     DateTime beginDate = new DateTime(2009, 4, 1);
        ///     DateTime endDate = DateTime.Now ;
        ///
        ///     ListList&lt;AttendanceRecord&gt; records = Attendance.SelectByDate(Students, beginDate, endDate );
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>           
        public static List<AttendanceRecord> SelectByDate(IEnumerable<StudentRecord> Students,
                                                        DateTime BeginDate,
                                                        DateTime EndDate) 
        {
            return SelectByDate<K12.Data.AttendanceRecord>(Students ,BeginDate ,EndDate);
        }

        /// <summary>
        /// 取得指定學生在日期區間的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <param name="BeginDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     DateTime beginDate = new DateTime(2009, 4, 1);
        ///     DateTime endDate = DateTime.Now ;
        ///     string[] studentIDs = Student.Instance.SelectedList.AsKeyList().ToArray();
        ///     ListList&lt;AttendanceRecord&gt; records = Attendance.SelectByDate(studentIDs, beginDate, endDate );
        /// </example>           
        /// <remarks></remarks>
        protected static List<T> SelectByDate<T>(IEnumerable<StudentRecord> Students,
                                                        DateTime BeginDate,
                                                        DateTime EndDate) where T:AttendanceRecord ,new()
        {

            List<string> StudentIDs = new List<string>();

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Students))
                foreach (StudentRecord StudentRec in Students)
                    if (!string.IsNullOrEmpty(StudentRec.ID))
                        StudentIDs.Add(StudentRec.ID);

            return Select<T>(StudentIDs, BeginDate, EndDate, null, null, null);
        }

        /// <summary>
        /// 取得指定日期區間的學生缺曠紀錄
        /// </summary>
        /// <param name="BeginDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     DateTime beginDate = new DateTime(2009, 4, 1);
        ///     DateTime endDate = DateTime.Now ;
        ///
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByDate(StudentIDs,beginDate, endDate );
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>           
        public static List<AttendanceRecord> SelectByDate(DateTime BeginDate, DateTime EndDate)
        {
            return SelectByDate<K12.Data.AttendanceRecord>(BeginDate, EndDate);
        }

        /// <summary>
        /// 取得指定日期區間的學生缺曠紀錄
        /// </summary>
        /// <param name="BeginDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     DateTime beginDate = new DateTime(2009, 4, 1);
        ///     DateTime endDate = DateTime.Now ;
        ///     string[] studentIDs = Student.Instance.SelectedList.AsKeyList().ToArray();
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByDate(studentIDs, beginDate, endDate );
        /// </example>           
        protected static List<T> SelectByDate<T>(DateTime BeginDate,DateTime EndDate) where T:AttendanceRecord ,new()
        {
            return Select<T>(null, BeginDate, EndDate, null, null, null);
        }

        /// <summary>
        /// 取得指定學生歷年所有的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudents(Students);
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<AttendanceRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.AttendanceRecord>(Students);
        }

        /// <summary>
        /// 取得指定學生歷年所有的學生缺曠紀錄
        /// </summary>
        /// <param name="Students">學生記錄物件列表</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     string[] studentIDs = Student.Instance.SelectedList.AsKeyList().ToArray();
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudents(studentIDs, beginDate, endDate );
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T:AttendanceRecord,new()
        {
            List<string> StudentIDs = new List<string>();

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(Students))
                foreach (StudentRecord StudentRec in Students)
                    if (!string.IsNullOrEmpty(StudentRec.ID))
                        StudentIDs.Add(StudentRec.ID);

            return Select<T>(StudentIDs, null, null, null, null, null);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生缺曠紀錄
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     <code>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(AttendanceRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<AttendanceRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<AttendanceRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生缺曠紀錄
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;AttendanceRecord&gt;，一個 AttendanceRecord物件代表一個學生在某一天的缺曠紀錄。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <example>
        ///     List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudents(studentIDs, beginDate, endDate );
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : AttendanceRecord, new()
        {
            return Select<T>(StudentIDs, null, null, null, null, null);
        }
        
        /// <summary>
        /// 新增單筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecord">學生缺曠記錄</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         AttendanceRecord newrecord = new AttendanceRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         strng NewID = Attendance.Insert(newrecord)
        ///         AttendanceRecord record = Attendance.SelectByID(NewID);
        ///         System.Console.Writeln(record.RefStudentID);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為學生記錄編號（RefStudentID）、學年度（SchoolYear）、學期（Semester）、缺曠日期（OccurDate）。
        /// </remarks>
        public static string Insert(AttendanceRecord AttendanceRecord)
        {
            List<AttendanceRecord> Params = new List<AttendanceRecord>();

            Params.Add(AttendanceRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecords">多筆班級記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         AttendanceRecord record = new AttendanceRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         
        ///         List&lt;AttendanceRecord&gt; records = new List&lt;AttendanceRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Attendance.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<AttendanceRecord> AttendanceRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<AttendanceRecord> worker = new MultiThreadWorker<AttendanceRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AttendanceRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Attendance");
                    helper.AddElement("Attendance", "Field");
                    helper.AddElement("Attendance/Field", "RefStudentID", editor.RefStudentID);
                    helper.AddElement("Attendance/Field", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("Attendance/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("Attendance/Field", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));

                    helper.AddElement("Attendance/Field", "Detail");
                    helper.AddElement("Attendance/Field/Detail", "Attendance");
                    foreach (var period in editor.PeriodDetail)
                    {
                        XmlElement node = helper.AddElement("Attendance/Field/Detail/Attendance", "Period", period.Period);
                        node.SetAttribute("AbsenceType", period.AbsenceType);
                    }
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<AttendanceRecord>> packages = worker.Run(AttendanceRecords);

            foreach (PackageWorkEventArgs<AttendanceRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange!=null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));
    
            return result;
        }

        /// <summary>
        /// 更新單筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecord">學生缺曠記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     AttendanceRecord record = Attendance.SelectByStudentID(Student)[0];
        ///     record.OccurDate = DateTime.Today;
        ///     int UpdateCount = Attendance.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(AttendanceRecord AttendanceRecord)
        {
            List<AttendanceRecord> Params = new List<AttendanceRecord>();

            Params.Add(AttendanceRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecords">多筆學生缺曠記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     AttendanceRecord record = Attendance.SelectByStudentID(Student)[0];
        ///     record.Date = DateTime.Today;
        ///     List&lt;AttendanceRecord&gt; records = new List&lt;AttendanceRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Attendance.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<AttendanceRecord> AttendanceRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<AttendanceRecord> worker = new MultiThreadWorker<AttendanceRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AttendanceRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Attendance");
                    updateHelper.AddElement("Attendance", "Field");
                    updateHelper.AddElement("Attendance/Field", "RefStudentID", editor.RefStudentID);
                    updateHelper.AddElement("Attendance/Field", "SchoolYear",K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("Attendance/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    updateHelper.AddElement("Attendance/Field", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));

                    updateHelper.AddElement("Attendance/Field", "Detail");
                    updateHelper.AddElement("Attendance/Field/Detail", "Attendance");
                    foreach (var period in editor.PeriodDetail)
                    {
                        XmlElement node = updateHelper.AddElement("Attendance/Field/Detail/Attendance", "Period", period.Period);
                        node.SetAttribute("AbsenceType", period.AbsenceType);
                    }

                    updateHelper.AddElement("Attendance", "Condition");
                    updateHelper.AddElement("Attendance/Condition", "ID", editor.ID);
                    updateHelper.AddElement("Attendance/Condition", "RefStudentID", editor.RefStudentID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<AttendanceRecord>> packages = worker.Run(AttendanceRecords);

            foreach (PackageWorkEventArgs<AttendanceRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));
            
            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecord">學生缺曠記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Attendance.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(AttendanceRecord AttendanceRecord)
        {
            return Delete(AttendanceRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceID">學生缺曠記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Attendance.Delete(AttendanceID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string AttendanceID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(AttendanceID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceRecords">多筆學生缺曠記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="AttendanceRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;AttendanceRecord&gt; records = Attendance.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Attendance.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<AttendanceRecord> AttendanceRecords)
        {
            List<string> Keys = new List<string>();

            foreach (AttendanceRecord AttendanceRecord in AttendanceRecords)
                Keys.Add(AttendanceRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生缺曠記錄
        /// </summary>
        /// <param name="AttendanceIDs">多筆學生缺曠記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Attendance.Delete(AttendanceIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> AttendanceIDs)
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
                    deleteHelper.AddElement("Attendance");
                    deleteHelper.AddElement("Attendance", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(AttendanceIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(AttendanceIDs, ChangedSource.Local));
    
            if (AfterChange !=null)
                AfterChange(null, new DataChangedEventArgs(AttendanceIDs, ChangedSource.Local));

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

        /// <summary>
        /// 資料改變之後所觸發的事件，新增、更新、刪除都會觸發
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterChange;

        #region =================    private functions    ====================

        /// <summary>
        /// 建立取得缺曠記錄的Request Document.
        /// </summary>
        /// <param name="students"></param>
        /// <param name="schoolYear"></param>
        /// <param name="semester"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        //private static string makeSelectReq(IEnumerable<string> students,
        //                                            string schoolYear,
        //                                            string semester,
        //                                            string beginDate,
        //                                            string endDate)
        //{
        //    bool HasCondition = false;

        //    StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition>");

        //    if (students != null)
        //        foreach (string sr in students)
        //            if (!string.IsNullOrEmpty(sr))
        //            {
        //                req.Append("<RefStudentID>" + sr + "</RefStudentID>");
        //                HasCondition = true;
        //            }

        //    if (!string.IsNullOrEmpty(schoolYear))
        //    {
        //        req.Append(string.Format("<SchoolYear>{0}</SchoolYear>", schoolYear));
        //        HasCondition = true;
        //    }

        //    if (!string.IsNullOrEmpty(semester))
        //    {
        //        req.Append(string.Format("<Semester>{0}</Semester>", semester));
        //        HasCondition = true;
        //    }
        //    if (!string.IsNullOrEmpty(beginDate))
        //    {
        //        req.Append(string.Format("<StartDate>{0}</StartDate>", beginDate));
        //        HasCondition = true;
        //    }
        //    if (!string.IsNullOrEmpty(endDate))
        //    {
        //        req.Append(string.Format("<EndDate>{0}</EndDate>", endDate));
        //        HasCondition = true;
        //    }

        //    req.Append("</Condition><Order><SchoolYear/><Semester/><OccurDate/></Order></Request>");

        //    return HasCondition?req.ToString():string.Empty;
        //}
        /// <summary>
        /// 呼叫 DSA 取得缺曠記錄
        /// </summary>
        /// <param name="reqDoc"></param>
        /// <returns></returns>
        //private static List<T> GetRecords<T>(string reqDoc) where T:AttendanceRecord ,new()
        //{
        //    List<T> Types = new List<T>();

        //    if (!string.IsNullOrEmpty(reqDoc))
        //    {
        //        foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(reqDoc)).GetContent().GetElements("Attendance"))
        //        {
        //            T Type = new T();
        //            Type.Load(item);
        //            Types.Add(Type);
        //        }
        //    }

        //    return Types;
        //}

        #endregion
    }
}