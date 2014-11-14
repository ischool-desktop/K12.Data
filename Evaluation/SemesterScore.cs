using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生學期成績類別，提供方法用來取得、新增、修改及刪除學生學期成績資訊
    /// </summary>
    public class SemesterScore
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Score.GetSemesterSubjectScore";
        private const string UPDATE_SERVICENAME = "SmartSchool.Score.UpdateSemesterSubjectScore";
        private const string INSERT_SERVICENAME = "SmartSchool.Score.InsertSemesterSubjectScore";
        private const string DELET_SERVICENAME = "SmartSchool.Score.DeleteSemesterSubjectScore"; 

        /// <summary>
        /// 取得所有科目成績
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.SemesterScore.SelectAllSubjectScore", "成績.科目成績")]
        public static List<SubjectScore> SelectAllSubjectScore()
        {
            List<SubjectScore> Subjects = new List<SubjectScore>();

            List<SemesterScoreRecord> Records = SelectAll();

            foreach (SemesterScoreRecord Record in Records)
                foreach (SubjectScore Subject in Record.Subjects.Values)
                    Subjects.Add(Subject);
            return Subjects;
        }

        /// <summary>
        /// 取得所有學生學期成績記錄物件。
        /// </summary>
        /// <returns></returns>
        public static List<SemesterScoreRecord> SelectAll()
        {
            return SelectAll<K12.Data.SemesterScoreRecord>();
        }

        /// <summary>
        /// 取得所有學生學期成績記錄物件。
        /// </summary>
        /// <typeparam name="T">學期成績記錄物件型別，繼承至SemesterScoreRecord。</typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : SemesterScoreRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("GetSemesterSubjectScore");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentId");
            helper.AddElement("Field", "SchoolYear");
            helper.AddElement("Field", "Semester");
            helper.AddElement("Field", "GradeYear");
            helper.AddElement("Field", "ScoreInfo");
            helper.AddElement("Condition");

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            foreach (XmlElement element in rsp.GetContent().GetElements("SemesterSubjectScore"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據學生編號、學年度及學期取得學生學期成績。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>SemesterScoreRecord，代表學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     SemesterScoreRecord record = SelectBySchoolYearAndSemester(StudentID,SchoolYear,Semester);
        /// </example>
        public static SemesterScoreRecord SelectBySchoolYearAndSemester(string StudentID, int SchoolYear, int Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.SemesterScoreRecord>(StudentID, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據學生編號、學年度及學期取得學生學期成績。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="StudentID">學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>SemesterScoreRecord，代表學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     SemesterScoreRecord rerecord = SelectBySchoolYearAndSemester&lt;K12.Data.SemesterScoreRecord&gt;(StudentID,SchoolYear,Semester);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static T SelectBySchoolYearAndSemester<T>(string StudentID, int SchoolYear, int Semester) where T : SemesterScoreRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("GetSemesterSubjectScore");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentId");
            helper.AddElement("Field", "SchoolYear");
            helper.AddElement("Field", "Semester");
            helper.AddElement("Field", "GradeYear");
            helper.AddElement("Field", "ScoreInfo");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "StudentIDList");
            helper.AddElement("Condition/StudentIDList", "ID", StudentID);
            helper.AddElement("Condition", "SchoolYear", K12.Data.Int.GetString(SchoolYear));
            helper.AddElement("Condition", "Semester", K12.Data.Int.GetString(Semester));

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            System.Xml.XmlElement element = rsp.GetContent().GetElement("SemesterSubjectScore");


            if (element != null)
            {
                T Type = new T();
                Type.Load(element);
                return Type;
            }
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生編號、學年度、學期取得學生學期成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectBySchoolYearAndSemester(StudentIDs,SchoolYear,Semester);
        /// </example>
        public static List<SemesterScoreRecord> SelectBySchoolYearAndSemester(IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.SemesterScoreRecord>(StudentIDs, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據多筆學生編號、學年度、學期取得學生學期成績列表。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="SchoolYear">學年度，傳入null代表取得所有學年度資料。</param>
        /// <param name="Semester">學期，傳入null代表取得所有學期資料。</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectBySchoolYearAndSemester&lt;K12.Data.SemesterScoreRecord&gt;(StudentIDs,SchoolYear,Semester);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectBySchoolYearAndSemester<T>(IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester) where T : SemesterScoreRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("GetSemesterSubjectScore");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentId");
            helper.AddElement("Field", "SchoolYear");
            helper.AddElement("Field", "Semester");
            helper.AddElement("Field", "GradeYear");
            helper.AddElement("Field", "ScoreInfo");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "StudentIDList");
            foreach (var each in StudentIDs)
                helper.AddElement("Condition/StudentIDList", "ID", each);

            if (SchoolYear != null)
                helper.AddElement("Condition", "SchoolYear", K12.Data.Int.GetString(SchoolYear));
            if (Semester != null)
                helper.AddElement("Condition", "Semester", K12.Data.Int.GetString(Semester));

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            List<T> Types = new List<T>();

            foreach (XmlElement element in rsp.GetContent().GetElements("SemesterSubjectScore"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆學生編號取得學生學期成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudentIDs(StudentIDs);
        /// </example>
        public static List<SemesterScoreRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.SemesterScoreRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生學期成績列表。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudentIDs&lt;K12.Data.SemesterScoreRecord&gt;(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : SemesterScoreRecord, new()
        {
            DSXmlHelper helper = new DSXmlHelper("GetSemesterSubjectScore");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentId");
            helper.AddElement("Field", "SchoolYear");
            helper.AddElement("Field", "Semester");
            helper.AddElement("Field", "GradeYear");
            helper.AddElement("Field", "ScoreInfo");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "StudentIDList");
            foreach (var each in StudentIDs)
                helper.AddElement("Condition/StudentIDList", "ID", each);

            DSResponse rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper));

            List<T> Types = new List<T>();


            foreach (XmlElement element in rsp.GetContent().GetElements("SemesterSubjectScore"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生學期成績列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudents(Students);
        /// </example>
        public static List<SemesterScoreRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.SemesterScoreRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生學期成績列表。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudents&lt;K12.Data.SemesterScoreRecord&gt;(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T : SemesterScoreRecord, new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據學生編號取得學生學期成績列表。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudentID(StudentID);
        /// </example>
        public static List<SemesterScoreRecord> SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.SemesterScoreRecord>(StudentID);
        }


        /// <summary>
        /// 根據學生編號取得學生學期成績列表。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudentID&lt;K12.Data.SemesterScoreRecord&gt;(StudentID);
        /// </example>
        protected static List<T> SelectByStudentID<T>(string StudentID) where T : SemesterScoreRecord, new()
        {
            List<string> Params = new List<string>();

            Params.Add(StudentID);

            return SelectByStudentIDs<T>(Params.ToArray());
        }

        /// <summary>
        /// 根據學生記錄物件取得學生學期成績列表。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudent(Student);
        /// </example>
        public static List<SemesterScoreRecord> SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.SemesterScoreRecord>(Student);
        }


        /// <summary>
        /// 根據學生記錄物件取得學生學期成績列表。
        /// </summary>
        /// <typeparam name="T">學生學期成績記錄物件型別，K12共用為K12.Data.SemesterScoreRecord</typeparam>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>List&lt;SemesterScoreRecord&gt;，代表多筆學生學期成績記錄物件。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterScoreRecord&gt; records = SemesterScore.SelectByStudent&lt;K12.Data.SemesterScoreRecord&gt;(Student);
        /// </example>
        protected static List<T> SelectByStudent<T>(StudentRecord Student) where T : SemesterScoreRecord, new()
        {
            List<StudentRecord> Params = new List<StudentRecord>();

            Params.Add(Student);

            return SelectByStudents<T>(Params);
        }

        /// <summary>
        /// 新增單筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecord">學生學期成績記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(SemesterScoreRecord SemesterScoreRecord)
        {
            List<SemesterScoreRecord> Params = new List<SemesterScoreRecord>();

            Params.Add(SemesterScoreRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecords">多筆學生學期成績記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<SemesterScoreRecord> SemesterScoreRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<SemesterScoreRecord> worker = new MultiThreadWorker<SemesterScoreRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SemesterScoreRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("InsertRequest");

                foreach (var editor in e.List)
                {
                    DSXmlHelper partialInsertHelper = new DSXmlHelper("SemesterSubjectScore");

                    partialInsertHelper.AddElement(".", "RefStudentId", editor.RefStudentID);
                    partialInsertHelper.AddElement(".", "SchoolYear", "" + editor.SchoolYear);
                    partialInsertHelper.AddElement(".", "Semester", "" + editor.Semester);
                    partialInsertHelper.AddElement(".", "GradeYear", "" + editor.GradeYear);
                    partialInsertHelper.AddElement(".", "ScoreInfo");

                    partialInsertHelper.AddElement("ScoreInfo", "SemesterSubjectScoreInfo");
                    foreach (var subjectKey in editor.Subjects.Keys)
                    {
                        SubjectScore subject = editor.Subjects[subjectKey];
                        XmlElement element = partialInsertHelper.AddElement("ScoreInfo/SemesterSubjectScoreInfo", "Subject");
                        element.SetAttribute("領域", subject.Domain.ToString());
                        element.SetAttribute("科目", subjectKey);
                        element.SetAttribute("節數", "" + subject.Period);
                        element.SetAttribute("權數", "" + subject.Credit);
                        element.SetAttribute("成績", "" + subject.Score);
                        element.SetAttribute("努力程度", "" + subject.Effort);
                        element.SetAttribute("文字描述", "" + subject.Text);
                        element.SetAttribute("註記", "" + subject.Comment);
                    }

                    partialInsertHelper.AddElement("ScoreInfo", "Domains");
                    foreach (var domainKey in editor.Domains.Keys)
                    {
                        DomainScore domain = editor.Domains[domainKey];
                        XmlElement element = partialInsertHelper.AddElement("ScoreInfo/Domains", "Domain");
                        element.SetAttribute("領域", domainKey);
                        element.SetAttribute("節數", "" + domain.Period);
                        element.SetAttribute("權數", "" + domain.Credit);
                        element.SetAttribute("成績", "" + domain.Score);
                        element.SetAttribute("原始成績", "" + domain.ScoreOrigin);
                        element.SetAttribute("補考成績", "" + domain.ScoreMakeup);
                        element.SetAttribute("努力程度", "" + domain.Effort);
                        element.SetAttribute("文字描述", "" + domain.Text);
                        element.SetAttribute("註記", "" + domain.Comment);
                    }

                    partialInsertHelper.AddElement("ScoreInfo", "LearnDomainScore", "" + editor.LearnDomainScore);
                    partialInsertHelper.AddElement("ScoreInfo", "CourseLearnScore", "" + editor.CourseLearnScore);

                    helper.AddElement(".", partialInsertHelper.BaseElement);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<SemesterScoreRecord>> packages = worker.Run(SemesterScoreRecords);

            foreach (PackageWorkEventArgs<SemesterScoreRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecord">學生學期成績記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(SemesterScoreRecord semesterScoreRecord)
        {
            List<SemesterScoreRecord> Params = new List<SemesterScoreRecord>();

            Params.Add(semesterScoreRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生學期成績記錄
        /// </summary>
        /// <param name="semesterScoreRecords">多筆學生學期成績記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<SemesterScoreRecord> semesterScoreRecords)
        {
            int result = 0;
            List<string> IDs = new List<string>();

            MultiThreadWorker<SemesterScoreRecord> worker = new MultiThreadWorker<SemesterScoreRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SemesterScoreRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("UpdateRequest");

                foreach (var editor in e.List)
                {
                    DSXmlHelper partialUpdateHelper = new DSXmlHelper("SemesterSubjectScore");

                    IDs.Add(editor.ID);

                    partialUpdateHelper.AddElement("Field");
                    partialUpdateHelper.AddElement("Field", "RefStudentId", editor.RefStudentID);
                    partialUpdateHelper.AddElement("Field", "SchoolYear", "" + editor.SchoolYear);
                    partialUpdateHelper.AddElement("Field", "Semester", "" + editor.Semester);
                    partialUpdateHelper.AddElement("Field", "GradeYear", "" + editor.GradeYear);
                    partialUpdateHelper.AddElement("Field", "ScoreInfo");
                    partialUpdateHelper.AddElement("Field/ScoreInfo", "SemesterSubjectScoreInfo");
                    foreach (var subjectKey in editor.Subjects.Keys)
                    {
                        SubjectScore subject = editor.Subjects[subjectKey];
                        XmlElement element = partialUpdateHelper.AddElement("Field/ScoreInfo/SemesterSubjectScoreInfo", "Subject");
                        element.SetAttribute("領域", subject.Domain.ToString());
                        element.SetAttribute("科目", subjectKey);
                        element.SetAttribute("節數", "" + subject.Period);
                        element.SetAttribute("權數", "" + subject.Credit);
                        element.SetAttribute("成績", "" + subject.Score);
                        element.SetAttribute("努力程度", "" + subject.Effort);
                        element.SetAttribute("文字描述", "" + subject.Text);
                        element.SetAttribute("註記", "" + subject.Comment);

                    }

                    partialUpdateHelper.AddElement("Field/ScoreInfo", "Domains");
                    foreach (var domainKey in editor.Domains.Keys)
                    {
                        DomainScore domain = editor.Domains[domainKey];
                        XmlElement element = partialUpdateHelper.AddElement("Field/ScoreInfo/Domains", "Domain");
                        element.SetAttribute("領域", domainKey);
                        element.SetAttribute("節數", "" + domain.Period);
                        element.SetAttribute("權數", "" + domain.Credit);
                        element.SetAttribute("成績", "" + domain.Score);
                        element.SetAttribute("原始成績", "" + domain.ScoreOrigin);
                        element.SetAttribute("補考成績", "" + domain.ScoreMakeup);
                        element.SetAttribute("努力程度", "" + domain.Effort);
                        element.SetAttribute("文字描述", "" + domain.Text);
                        element.SetAttribute("註記", "" + domain.Comment);
                    }

                    partialUpdateHelper.AddElement("Field/ScoreInfo", "LearnDomainScore", "" + editor.LearnDomainScore);
                    partialUpdateHelper.AddElement("Field/ScoreInfo", "CourseLearnScore", "" + editor.CourseLearnScore);

                    partialUpdateHelper.AddElement("Condition");
                    partialUpdateHelper.AddElement("Condition", "ID", editor.ID);

                    helper.AddElement(".", partialUpdateHelper.BaseElement);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<SemesterScoreRecord>> packages = worker.Run(semesterScoreRecords);

            foreach (PackageWorkEventArgs<SemesterScoreRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecord">學生學期成績記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(SemesterScoreRecord SemesterScoreRecord)
        {
            return Delete(SemesterScoreRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecordID">學生學期成績編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string SemesterScoreRecordID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(SemesterScoreRecordID);

            return Delete(Keys);
        }


        /// <summary>
        /// 刪除多筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecords">多筆學生學期成績記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SemesterScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<SemesterScoreRecord> SemesterScoreRecords)
        {
            List<string> Keys = new List<string>();

            foreach (SemesterScoreRecord semesterScoreRecord in SemesterScoreRecords)
                Keys.Add(semesterScoreRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生學期成績記錄
        /// </summary>
        /// <param name="SemesterScoreRecordIDs">多筆學生學期成績編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> SemesterScoreRecordIDs)
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
                    helper.AddElement("SemesterSubjectScore");
                    helper.AddElement("SemesterSubjectScore", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(SemesterScoreRecordIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(SemesterScoreRecordIDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        /// <summary>
        /// 
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        /// <summary>
        /// 
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterDelete;

    }
}