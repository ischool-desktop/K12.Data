using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生獎勵類別，提供方法用來取得、新增、修改及刪除學生獎勵資訊
    /// </summary>
    public class Merit
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.Discipline.GetDiscipline";
        private const string INSERT_SERVICENAME = "SmartSchool.Student.Discipline.Insert";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.Discipline.Update";
        private const string DELETE_SERVICENAME = "SmartSchool.Student.Discipline.Delete";

        /// <summary>
        /// 取得所有學生獎勵記錄物件列表。
        /// </summary>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectAll();
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        [SelectMethod("K12.Merit.SelectAll", "學務.獎勵")]
        public static List<MeritRecord> SelectAll()
        {
            return SelectAll<K12.Data.MeritRecord>(); 
        }

        /// <summary>
        /// 取得所有學生獎勵記錄物件列表。
        /// </summary>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; MeritRecords = Merit.SelectAll();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:MeritRecord ,new()
        {
            List<T> Types = new List<T>();

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");
            req.Append("<MeritFlag>1</MeritFlag>");    //MeritFlag=0 銷過,  MeritFlag=2 記過 , MeritFlag=1 記功
            req.Append("</Condition><Order><RefStudentID/><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //Invoke DSA Services and parse the response doc into MeritRecord objects.
            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據條件取得獎勵紀錄列表 
        /// </summary>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">開始時間</param>
        /// <param name="EndDate">結束時間</param>
        /// <param name="StartRegisterDate">開始登錄時間</param>
        /// <param name="EndRegisterDate">結束登錄時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <returns>List&lt;MeritRecord&gt;，多筆獎勵紀錄物件列表</returns>
        public static List<MeritRecord> Select(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? StartRegisterDate, DateTime? EndRegisterDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters)
        {
            return Select<K12.Data.MeritRecord>(StudentIDs, StartDate, EndDate, StartRegisterDate, EndRegisterDate, SchoolYears, Semesters);
        }

        /// <summary>
        /// 根據條件取得獎勵紀錄列表
        /// </summary>
        /// <typeparam name="T">獎勵紀錄型別，繼承至K12.Data.MeritRecord</typeparam>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">開始時間</param>
        /// <param name="EndDate">結束時間</param>
        /// <param name="StartRegisterDate">開始登錄時間</param>
        /// <param name="EndRegisterDate">結束登錄時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <returns>List&lt;MeritRecord&gt;，多筆獎勵紀錄物件列表</returns>
        protected static List<T> Select<T>(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? StartRegisterDate, DateTime? EndRegisterDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters) where T : MeritRecord, new()
        {
            return Select<T>(StudentIDs, StartDate, EndDate, StartRegisterDate, EndRegisterDate, SchoolYears, Semesters, null);
        }

        /// <summary>
        /// 根據條件取得獎勵紀錄列表
        /// </summary>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">開始時間</param>
        /// <param name="EndDate">結束時間</param>
        /// <param name="StartRegisterDate">開始登錄時間</param>
        /// <param name="EndRegisterDate">結束登錄時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <param name="SchoolYearSemesters">學年度學期列表</param>
        /// <returns>List&lt;MeritRecord&gt;，多筆獎勵紀錄物件列表</returns>
        public static List<MeritRecord> Select(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? StartRegisterDate, DateTime? EndRegisterDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters, IEnumerable<SchoolYearSemester> SchoolYearSemesters)
        {
            return Select<MeritRecord>(StudentIDs, StartDate, EndDate, StartRegisterDate, EndRegisterDate, SchoolYears, Semesters, SchoolYearSemesters); 
        }

        /// <summary>
        /// 根據條件取得獎勵紀錄列表
        /// </summary>
        /// <typeparam name="T">獎勵紀錄型別，繼承至K12.Data.MeritRecord</typeparam>
        /// <param name="StudentIDs">學生編號列表</param>
        /// <param name="StartDate">開始時間</param>
        /// <param name="EndDate">結束時間</param>
        /// <param name="StartRegisterDate">開始登錄時間</param>
        /// <param name="EndRegisterDate">結束登錄時間</param>
        /// <param name="SchoolYears">學年度列表</param>
        /// <param name="Semesters">學期列表</param>
        /// <param name="SchoolYearSemesters">學年度學期列表</param>
        /// <returns>List&lt;MeritRecord&gt;，多筆獎勵紀錄物件列表</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> StudentIDs, DateTime? StartDate, DateTime? EndDate, DateTime? StartRegisterDate, DateTime? EndRegisterDate, IEnumerable<int> SchoolYears, IEnumerable<int> Semesters,IEnumerable<SchoolYearSemester> SchoolYearSemesters) where T : MeritRecord, new()
        {
            List<T> Types = new List<T>();

            bool IsSendRequest = false;

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                foreach (string StudentID in StudentIDs)
                {
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        req.Append("<RefStudentID>" + StudentID + "</RefStudentID>");
                        IsSendRequest = true;
                    }
                }
            }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(StartDate))
            {
                req.Append("<StartDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartDate) + "</StartDate>");
                IsSendRequest = true;
            }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(EndDate))
            {
                req.Append("<EndDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndDate) + "</EndDate>");
                IsSendRequest = true;
            }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(StartRegisterDate))
            {
                req.Append("<StartRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartRegisterDate) + "</StartRegisterDate>");
                IsSendRequest = true;
            }

            if (!K12.Data.DateTimeHelper.IsNullOrEmpty(EndRegisterDate))
            {
                req.Append("<EndRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndRegisterDate) + "</EndRegisterDate>");
                IsSendRequest = true;
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
                    req.Append("<Semester>" + Semester + "</Semester>");
                    IsSendRequest = true;
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SchoolYearSemesters))
            {
                req.Append("<Or>");

                foreach (SchoolYearSemester schoolYearSemester in SchoolYearSemesters)
                {
                    req.Append("<And>");
                    req.Append("<SchoolYear>" + schoolYearSemester.SchoolYear + "</SchoolYear>");
                    req.Append("<Semester>" + schoolYearSemester.Semester + "</Semester>");
                    req.Append("</And>");
                }

                req.Append("</Or>");

                IsSendRequest = true;
            }

            req.Append("<MeritFlag>1</MeritFlag>"); 
            req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //如果有傳學生ID進來
            if (IsSendRequest)
            {
                //Invoke DSA Services and parse the response doc into DemeritRecord objects.
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生編號、學年度及學期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null取得所有學年度資料</param>
        /// <param name="Semester">學期，傳入null取得所有學期資料</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectBySchoolYearAndSemester(StudentID,School,Semester);
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<MeritRecord> SelectBySchoolYearAndSemester(string StudentID, int? SchoolYear, int? Semester)
        {
            return SelectBySchoolYearAndSemester<MeritRecord>(StudentID, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據單筆學生編號、學年度及學期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null取得所有學年度資料</param>
        /// <param name="Semester">學期，傳入null取得所有學期資料</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectBySchoolYearAndSemester<T>(string StudentID, int? SchoolYear, int? Semester) where T:MeritRecord ,new()
        {
            List<string> IDs = new List<string>();
            IDs.Add(StudentID);

            return SelectBySchoolYearAndSemester<T>(IDs, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據多筆學生編號、學年度及學期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度的資料</param>
        /// <param name="Semester">學期，傳入null代表取得所有學年度的資料</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectBySchoolYearAndSemester(StudentIDs,School,Semester);
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<MeritRecord> SelectBySchoolYearAndSemester(IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.MeritRecord>(StudentIDs, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據多筆學生編號、學年度及學期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度的資料</param>
        /// <param name="Semester">學期，傳入null代表取得所有學年度的資料</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectBySchoolYearAndSemester<T>(IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester) where T:MeritRecord ,new()
        {
            List<int> SchoolYears = new List<int>();
            List<int> Semesters = new List<int>();

            if (!K12.Data.Int.IsNullOrEmpty(SchoolYear))
                SchoolYears.Add(SchoolYear.Value);

            if (!K12.Data.Int.IsNullOrEmpty(Semester))
                Semesters.Add(Semester.Value);

            return Select<T>(StudentIDs, null, null, null, null, SchoolYears, Semesters);
        }

        public static List<MeritRecord> SelectBySchoolYearAndSemesterLessEqual(IEnumerable<string> StudentIDs, SchoolYearSemester SchoolYearSemester)
        {
            return SelectBySchoolYearAndSemesterLessEqual<MeritRecord>(StudentIDs, SchoolYearSemester);
        }

        protected static List<T> SelectBySchoolYearAndSemesterLessEqual<T>(IEnumerable<string> StudentIDs,SchoolYearSemester SchoolYearSemester) where T:MeritRecord ,new()
        {
            List<T> Types = new List<T>();

            bool IsSendRequest = false;

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                foreach (string StudentID in StudentIDs)
                {
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        req.Append("<RefStudentID>" + StudentID + "</RefStudentID>");
                        IsSendRequest = true;
                    }
                }
            }

            string strSchoolYearSemesterCondition = SchoolYearSemester.ToLessThanRequest();

            if (!string.IsNullOrEmpty(strSchoolYearSemesterCondition))
            {
                req.Append(strSchoolYearSemesterCondition);
                IsSendRequest = true;
            }

            req.Append("<MeritFlag>1</MeritFlag>");
            req.Append("</Condition><Order><OccurDate>desc</OccurDate><RefStudentID/></Order></SelectRequest>");

            //如果有傳學生ID進來
            if (IsSendRequest)
            {
                //Invoke DSA Services and parse the response doc into DemeritRecord objects.
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆學生編號取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<MeritRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.MeritRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:MeritRecord ,new()
        {
            return Select<T>(StudentIDs, null, null, null, null, null, null);
        }

        /// <summary>
        /// 根據多筆學生編號及登錄日期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartRegisterDate">登錄開始日期</param>
        /// <param name="EndRegisterDate">登錄結束日期</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectByRegisterDate(StudentIDs,StartRegisterDate,EndRegisterDate);
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.若是StartRegisterDate傳入null，則會傳回EndRegisterDate之前的資料。
        /// 2.若是EndRegisterDate傳入null，則會傳回StartRegisterDate之後的資料。
        /// </remarks>
        public static List<MeritRecord> SelectByRegisterDate(IEnumerable<string> StudentIDs, DateTime? StartRegisterDate, DateTime? EndRegisterDate)
        {
            return SelectByRegisterDate<MeritRecord>(StudentIDs, StartRegisterDate, EndRegisterDate);
        }

        /// <summary>
        /// 根據多筆學生編號及登錄日期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartRegisterDate">登錄開始日期</param>
        /// <param name="EndRegisterDate">登錄結束日期</param>
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectByRegisterDate<T>(IEnumerable<string> StudentIDs,DateTime? StartRegisterDate,DateTime? EndRegisterDate) where T : MeritRecord, new()
        {
            return Select<T>(StudentIDs, null, null, StartRegisterDate, EndRegisterDate, null, null);

            //List<T> Types = new List<T>();

            //bool haskey = false;

            ////建立 Request Document.
            //StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");
            //foreach (string key in StudentIDs)
            //{
            //    req.Append("<RefStudentID>" + key + "</RefStudentID>");
            //    haskey = true;
            //}

            //if (StartRegisterDate != null)
            //    req.Append("<StartRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartRegisterDate) + "</StartRegisterDate>");

            //if (EndRegisterDate != null)
            //    req.Append("<EndRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndRegisterDate) + "</EndRegisterDate>");

            //req.Append("<MeritFlag>1</MeritFlag>");    //MeritFlag=0 銷過,  MeritFlag=2 記過 , MeritFlag=1 記功
            //req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            ////如果有傳學生ID進來
            //if (haskey)
            //{
            //    //Invoke DSA Services and parse the response doc into MeritRecord objects.
            //    foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
            //    {
            //        T Type = new T();
            //        Type.Load(item);
            //        Types.Add(Type);
            //    }
            //}
            //return Types;
        }

        /// <summary>
        /// 根據多筆學生編號及發生日期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartOccurDate">發生開始日期</param>
        /// <param name="EndOccurDate">發生結束日期</param> 
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MeritRecord&gt; records = Merit.SelectByOccurDate(StudentIDs,StartOccurDate,EndOccurDate);
        ///     
        ///     foreach(MeritRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.若是StartOccurDate傳入null，則會傳回EndOccurDate之前的資料。
        /// 2.若是EndOccurDate傳入null，則會傳回StartOccurDate之後的資料。
        /// </remarks>
        public static List<MeritRecord> SelectByOccurDate(IEnumerable<string> StudentIDs, DateTime? StartOccurDate, DateTime? EndOccurDate)
        {
            return SelectByOccurDate<MeritRecord>(StudentIDs, StartOccurDate, EndOccurDate);
        }

        /// <summary>
        /// 根據多筆學生編號及發生日期取得學生獎勵記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartOccurDate">發生開始日期</param>
        /// <param name="EndOccurDate">發生結束日期</param> 
        /// <returns>List&lt;MeritRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        protected static List<T> SelectByOccurDate<T>(IEnumerable<string> StudentIDs, DateTime? StartOccurDate, DateTime? EndOccurDate) where T : MeritRecord, new()
        {
            return Select<T>(StudentIDs, StartOccurDate, EndOccurDate, null, null, null, null);

            //List<T> Types = new List<T>();

            //bool haskey = false;

            ////建立 Request Document.
            //StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");
            //foreach (string key in StudentIDs)
            //{
            //    req.Append("<RefStudentID>" + key + "</RefStudentID>");
            //    haskey = true;
            //}

            //if (StartOccurDate != null)
            //    req.Append("<StartDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartOccurDate) + "</StartDate>");

            //if (EndOccurDate != null)
            //    req.Append("<EndDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndOccurDate) + "</EndDate>");

            //req.Append("<MeritFlag>1</MeritFlag>");    //MeritFlag=0 銷過,  MeritFlag=2 記過 , MeritFlag=1 記功
            //req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            ////如果有傳學生ID進來
            //if (haskey)
            //{
            //    //Invoke DSA Services and parse the response doc into MeritRecord objects.
            //    foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
            //    {
            //        T Type = new T();
            //        Type.Load(item);
            //        Types.Add(Type);
            //    }
            //}
            //return Types;
        }

        /// <summary>
        /// 新增單筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecord">學生獎勵記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         MeritRecord newrecord = new MeritRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         strng NewID = Merit.Insert(newrecord)
        ///         MeritRecord record = Merit.SelectByID(NewID);
        ///         System.Console.Writeln(record.RefStudentID);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為學生記錄編號（RefStudentID）、學年度（SchoolYear）、學期（Semester）、缺曠日期（OccurDate）。
        /// </remarks>
        public static string Insert(MeritRecord MeritRecord)
        {
            List<MeritRecord> Params = new List<MeritRecord>();

            Params.Add(MeritRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecords">多筆學生獎勵記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         MeritRecord record = new MeritRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         
        ///         List&lt;MeritRecord&gt; records = new List&lt;MeritRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Merit.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<MeritRecord> MeritRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<MeritRecord> worker = new MultiThreadWorker<MeritRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<MeritRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("InsertRequest");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Discipline");
                    helper.AddElement("Discipline", "RefStudentID", editor.RefStudentID);
                    helper.AddElement("Discipline", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("Discipline", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("Discipline", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));
                    helper.AddElement("Discipline", "RegisterDate", K12.Data.DateTimeHelper.ToDisplayString(editor.RegisterDate));
                    helper.AddElement("Discipline", "Reason", editor.Reason);
                    helper.AddElement("Discipline", "MeritFlag", editor.MeritFlag);
                    helper.AddElement("Discipline", "Type", "1");
                    helper.AddElement("Discipline", "Detail", GetDetailContent(editor), true);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<MeritRecord>> packages = worker.Run(MeritRecords);

            foreach (PackageWorkEventArgs<MeritRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecord">學生獎勵記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     MeritRecord record = Merit.SelectByStudentIDs(StudentIDs)[0];
        ///     record.OccurDate = DateTime.Today;
        ///     int UpdateCount = Merit.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(MeritRecord MeritRecord)
        {
            List<MeritRecord> Params = new List<MeritRecord>();

            Params.Add(MeritRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecords">多筆學生獎勵記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     MeritRecord record = Merit.SelectByStudentIDs(StudentIDs)[0];
        ///     record.OccurDate = DateTime.Today;
        ///     List&lt;MeritRecord&gt; records = new List&lt;MeritRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Merit.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<MeritRecord> MeritRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<MeritRecord> worker = new MultiThreadWorker<MeritRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<MeritRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateRequest");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Discipline");
                    updateHelper.AddElement("Discipline", "Field");
                    updateHelper.AddElement("Discipline/Field", "RefStudentID", editor.RefStudentID);
                    updateHelper.AddElement("Discipline/Field", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("Discipline/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    updateHelper.AddElement("Discipline/Field", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));
                    updateHelper.AddElement("Discipline/Field", "RegisterDate", K12.Data.DateTimeHelper.ToDisplayString(editor.RegisterDate));
                    updateHelper.AddElement("Discipline/Field", "Reason", editor.Reason);
                    updateHelper.AddElement("Discipline/Field", "MeritFlag", editor.MeritFlag);
                    updateHelper.AddElement("Discipline/Field", "Type", "1");
                    updateHelper.AddElement("Discipline/Field", "Detail", GetDetailContent(editor), true);
                    updateHelper.AddElement("Discipline", "Condition");
                    updateHelper.AddElement("Discipline/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<MeritRecord>> packages = worker.Run(MeritRecords);

            foreach (PackageWorkEventArgs<MeritRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange  != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecord">學生獎勵記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;MeritRecord&gt; records = Merit.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Merit.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(MeritRecord MeritRecord)
        {
            return Delete(MeritRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritID">學生獎勵記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Merit.Delete(MeritID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string MeritID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(MeritID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritRecords">多筆學生獎勵記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="MeritRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;MeritRecord&gt; records = Merit.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Merit.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<MeritRecord> MeritRecords)
        {
            List<string> Keys = new List<string>();

            foreach (MeritRecord MeritRecord in MeritRecords)
                Keys.Add(MeritRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生獎勵記錄
        /// </summary>
        /// <param name="MeritIDs">多筆學生獎勵記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Merit.Delete(MeritIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> MeritIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("DeleteRequest");

                foreach (string Key in e.List)
                {
                    deleteHelper.AddElement("Discipline");
                    deleteHelper.AddElement("Discipline", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(MeritIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(MeritIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(MeritIDs, ChangedSource.Local));

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


        /// <summary>
        /// 組合出 Detail 節點的內容。新增和修改的 Request Doc 都會用到。
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        private static string GetDetailContent(MeritRecord editor)
        {
            DSXmlHelper helper = new DSXmlHelper("Discipline");
            XmlElement element = helper.AddElement("Merit");

            element.SetAttribute("A", K12.Data.Int.GetString(editor.MeritA));
            element.SetAttribute("B", K12.Data.Int.GetString(editor.MeritB));
            element.SetAttribute("C", K12.Data.Int.GetString(editor.MeritC));

            return helper.GetRawXml();
        }

    }
}