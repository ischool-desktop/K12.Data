using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 教師教授課程類別，提供方法用來取得、新增、修改及刪除教師教授課程資訊
    /// </summary>
    public class TCInstruct
    {

        private const string SELECT_SERVICENAME = "SmartSchool.Course.GetTCInstruct";
        private const string UPDATE_SERVICENAME = "SmartSchool.Course.UpdateTCInstruct";
        private const string INSERT_SERVICENAME = "SmartSchool.Course.InsertTCInstruct";
        private const string DELET_SERVICENAME = "SmartSchool.Course.DeleteTCInstruct";

        /// <summary>
        /// 取得所有教師教授課程列表。
        /// </summary>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TCInstructRecord&gt; tcinstructrecords = TCInstruct.SelectAll();
        ///     </code>
        /// </example>
        [SelectMethod("K12.TCInstruct.SelectAll", "成績.教師授課")]
        public static List<TCInstructRecord> SelectAll()
        {
            return SelectAll<K12.Data.TCInstructRecord>();
        }

        /// <summary>
        /// 取得所有教師教授課程列表。
        /// </summary>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TCInstructRecord&gt; tcinstructrecords = TCInstruct.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:TCInstructRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("All");

            DSRequest dsreq = new DSRequest(helper);

            List<T> Types = new List<T>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("TCInstruct"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }
            return Types;
        }

        /// <summary>
        /// 根據多筆教師教授課程編號取得教師教授課程列表。
        /// </summary>
        /// <param name="TCInstructIDs">多筆教師教授課程編號</param>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TCInstructRecord&gt; tcinstructrecords = TCInstruct.SelectByIDs(TCInstructIDs);
        /// </example>
        public static List<TCInstructRecord> SelectByIDs(IEnumerable<string> TCInstructIDs)
        {
            return SelectByIDs<K12.Data.TCInstructRecord>(TCInstructIDs);
        }
        /// <summary>
        /// 根據多筆教師教授課程編號取得教師教授課程列表。
        /// </summary>
        /// <param name="TCInstructIDs">多筆教師教授課程編號</param>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TCInstructRecord&gt; tcinstructrecords = TCInstruct.SelectByIDs(TCInstructIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> TCInstructIDs) where T:TCInstructRecord ,new()
        {
            // 指示是否需要呼叫 Service。
            bool execute_require = false;

            //建立 Request。
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("All");
            helper.AddElement("Condition");
            foreach (string id in TCInstructIDs)
            {
                helper.AddElement("Condition", "ID", id);
                execute_require = true;
            }

            //儲存最後結果的集合。
            List<T> Types = new List<T>();

            if (execute_require)
            {
                DSRequest dsreq = new DSRequest(helper);

                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("TCInstruct"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆教師教授課程編號及課程編號取得教師教授課程列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TCInstructRecord&gt; records = TCInstruct.SelectByTeacherIDAndCourseID(TeacherIDs,CourseIDs);
        ///     </code>
        /// </example>
        public static List<TCInstructRecord> SelectByTeacherIDAndCourseID(IEnumerable<string> TeacherIDs,IEnumerable<string> CourseIDs)
        {
            return SelectByTeacherIDAndCourseIDs<K12.Data.TCInstructRecord>(TeacherIDs,CourseIDs);
        }

        /// <summary>
        /// 根據多筆教師教授課程編號及課程編號取得教師教授課程列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="TeacherIDs">多筆教師編號</param>
        /// <returns>List&lt;TCInstructRecord&gt;，代表多筆教師教授課程記錄物件。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;TCInstructRecord&gt; tcinstructrecords = TCInstruct.SelectByIDs(TCInstructIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByTeacherIDAndCourseIDs<T>(IEnumerable<string> TeacherIDs,IEnumerable<string> CourseIDs) where T : TCInstructRecord, new()
        {
            // 指示是否需要呼叫 Service。
            bool execute_require = false;

            //建立 Request。
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("All");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(TeacherIDs))
            {
                foreach (string TeacherID in TeacherIDs)
                {
                    helper.AddElement("Condition", "RefTeacherID", TeacherID);
                    execute_require = true;
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
            {
                foreach (string CourseID in CourseIDs)
                {
                    helper.AddElement("Condition", "RefCourseID", CourseID);
                    execute_require = true;
                }
            }

            //儲存最後結果的集合。
            List<T> Types = new List<T>();

            if (execute_require)
            {
                DSRequest dsreq = new DSRequest(helper);

                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("TCInstruct"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 新增單筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecord">教師教授課程記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(TCInstructRecord TCInstructRecord)
        {
            List<TCInstructRecord> Params = new List<TCInstructRecord>();

            Params.Add(TCInstructRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecords">多筆教師教授課程記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<TCInstructRecord> TCInstructRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<TCInstructRecord> worker = new MultiThreadWorker<TCInstructRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TCInstructRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Instruct");
                    helper.AddElement("Instruct", "RefTeacherID", editor.RefTeacherID);
                    helper.AddElement("Instruct", "RefCourseID", editor.RefCourseID);
                    helper.AddElement("Instruct", "Sequence", K12.Data.Int.GetString(editor.Sequence));
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<TCInstructRecord>> packages = worker.Run(TCInstructRecords);

            foreach (PackageWorkEventArgs<TCInstructRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;
            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecord">教師教授課程記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(TCInstructRecord TCInstructRecord)
        {
            List<TCInstructRecord> Params = new List<TCInstructRecord>();

            Params.Add(TCInstructRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecords">多筆教師教授課程記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<TCInstructRecord> TCInstructRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<TCInstructRecord> worker = new MultiThreadWorker<TCInstructRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<TCInstructRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Instruct");
                    updateHelper.AddElement("Instruct", "RefTeacherID", editor.RefTeacherID);
                    updateHelper.AddElement("Instruct", "RefCourseID", editor.RefCourseID);
                    updateHelper.AddElement("Instruct", "Sequence", K12.Data.Int.GetString(editor.Sequence));
                    updateHelper.AddElement("Instruct", "Condition");
                    updateHelper.AddElement("Instruct/Condition", "ID", editor.ID);
                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<TCInstructRecord>> packages = worker.Run(TCInstructRecords);

            foreach (PackageWorkEventArgs<TCInstructRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecord">教師教授課程記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(TCInstructRecord TCInstructRecord)
        {
            return Delete(TCInstructRecord.ID);
        }

        /// <summary>
        /// 刪除單筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructID">教師教授課程編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string TCInstructID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(TCInstructID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructRecords">多筆教師教授課程記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="TCInstructRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<TCInstructRecord> TCInstructRecords)
        {
            List<string> Keys = new List<string>();

            foreach (TCInstructRecord TCInstructRecord in TCInstructRecords)
                Keys.Add(TCInstructRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆教師教授課程記錄
        /// </summary>
        /// <param name="TCInstructIDs">多筆教師教授課程編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> TCInstructIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (string Key in e.List)
                {
                    helper.AddElement("Instruct");
                    helper.AddElement("Instruct", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELET_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(TCInstructIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(TCInstructIDs, ChangedSource.Local));

            return result;
        }

        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        static public event EventHandler<DataChangedEventArgs> AfterDelete;

    //    /// <summary>
    //    /// 從(課程)查詢授課教師。
    //    /// </summary>
    //    private Dictionary<string, List<string>> _course_teachers = new Dictionary<string, List<string>>();

    //    /// <summary>
    //    /// 取得課程的所有授課教師。
    //    /// </summary>
    //    /// <param name="courseID">課程編號。</param>
    //    /// <returns>授課教師清單。</returns>
    //    public List<TCInstructRecord> GetCourseTeachers(string courseID)
    //    {
    //        List<TCInstructRecord> result = new List<TCInstructRecord>();
    //        if (_course_teachers.ContainsKey(courseID))
    //        {
    //            foreach (var eachInstructID in _course_teachers[courseID])
    //            {
    //                var objInstruct = Items[eachInstructID];
    //                if (objInstruct.Course != null && objInstruct.Teacher != null)
    //                    result.Add(Items[eachInstructID]);
    //            }
    //        }
    //        return result;
    //    }

    //    /// <summary>
    //    /// 從(教師)查詢教授課程。
    //    /// </summary>
    //    private Dictionary<string, List<string>> _teacher_courses = new Dictionary<string, List<string>>();

    //    /// <summary>
    //    /// 取得教師所授教的課程。
    //    /// </summary>
    //    /// <param name="studentID">學生編號</param>
    //    /// <returns>修課記錄清單</returns>
    //    public List<TCInstructRecord> GetTeacherCourses(string studentID)
    //    {
    //        List<TCInstructRecord> result = new List<TCInstructRecord>();
    //        if (_teacher_courses.ContainsKey(studentID))
    //        {
    //            foreach (var eachInstructID in _teacher_courses[studentID])
    //            {
    //                var objInstruct = Items[eachInstructID];
    //                if (objInstruct.Course != null && objInstruct.Teacher != null)
    //                    result.Add(Items[eachInstructID]);
    //            }
    //        }
    //        return result;
    //    }
    //}

    //public static class TCInstruct_ExtendMethods
    //{
    //    /// <summary>
    //    /// 取得課程的第一位授課教師。
    //    /// </summary>
    //    public static TeacherRecord GetFirstTeacher(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            TCInstructRecord tc = course.GetFirstInstruct();
    //            if (tc != null) return tc.Teacher;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的第二位授課教師。
    //    /// </summary>
    //    public static TeacherRecord GetSecondTeacher(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            TCInstructRecord tc = course.GetSecondInstruct();
    //            if (tc != null) return tc.Teacher;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的第三位授課教師。
    //    /// </summary>
    //    public static TeacherRecord GetThirdTeacher(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            TCInstructRecord tc = course.GetThirdInstruct();
    //            if (tc != null) return tc.Teacher;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的第一位授課教師。
    //    /// </summary>
    //    internal static TCInstructRecord GetFirstInstruct(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            foreach (TCInstructRecord each in course.GetInstructs())
    //                if (each.Sequence == "1") return each;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的第二位授課教師。
    //    /// </summary>
    //    internal static TCInstructRecord GetSecondInstruct(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            foreach (TCInstructRecord each in course.GetInstructs())
    //                if (each.Sequence == "2") return each;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的第三位授課教師。
    //    /// </summary>
    //    internal static TCInstructRecord GetThirdInstruct(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //            foreach (TCInstructRecord each in course.GetInstructs())
    //                if (each.Sequence == "3") return each;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// 取得課程的所有上課教師關聯資料。
    //    /// </summary>
    //    public static List<TCInstructRecord> GetInstructs(this CourseRecord course)
    //    {
    //        if (course != null)
    //            return TCInstruct.Instance.GetCourseTeachers(course.ID);
    //        else
    //            return null;
    //    }

    //    /// <summary>
    //    /// 取得教師上的所有課程關聯資料。 
    //    /// </summary>
    //    public static List<TCInstructRecord> GetInstructs(this TeacherRecord teacher)
    //    {
            
    //        return TCInstruct.Instance.GetTeacherCourses(teacher.ID);
    //    }

    //    /// <summary>
    //    /// 取得課程的所有教師資料。
    //    /// </summary>
    //    public static List<TeacherRecord> GetInstructTeachers(this CourseRecord course)
    //    {
    //        if (course != null)
    //        {
    //        List<TeacherRecord> teachers = new List<TeacherRecord>();
    //        foreach (TCInstructRecord each in GetInstructs(course))
    //            teachers.Add(each.Teacher);

    //        return teachers;
    //        }
    //        else 
    //            return null ;
    //    }

    //    /// <summary>
    //    /// 取得教師上的所有課程資料。
    //    /// </summary>
    //    public static List<CourseRecord> GetInstructCoruses(this TeacherRecord teacher)
    //    {
    //        List<CourseRecord> courses = new List<CourseRecord>();
    //        foreach (TCInstructRecord each in GetInstructs(teacher))
    //            courses.Add(each.Course);

    //        return courses;
    //    }
    }
}
