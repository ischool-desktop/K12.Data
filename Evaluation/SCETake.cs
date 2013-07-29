using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生期中成績類別，提供方法用來取得、新增、修改及刪除學生期中成績資訊
    /// </summary>
    public class SCETake
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Course.GetSCETake";
        private const string INSERT_SERVICENAME = "SmartSchool.Course.InsertSECScore";
        private const string UPDATE_SERVICENAME = "SmartSchool.Course.UpdateSCEScore";
        private const string DELETE_SERVICENAME = "SmartSchool.Course.DeleteSCEScore";
        private static ICacheProvider mCacheProvider = null;

        private class EntityHelper : EntityCacheHelper
        {
            private static EntityCacheHelper mCacheHelper = null;

            public static EntityCacheHelper Cache
            {
                get
                {
                    if (mCacheHelper == null)
                        mCacheHelper = new EntityHelper();
                    return mCacheHelper;
                }
            }

            protected override List<string> SelectFromServerByIDs(IEnumerable<string> IDs)
            {
                bool hasKey = false;

                List<string> Result = new List<string>();

                DSXmlHelper helper = new DSXmlHelper("Request");
                helper.AddElement("Field");
                helper.AddElement("Field", "All");
                helper.AddElement("Condition");
                foreach (var ID in IDs)
                {
                    if (ValidateKey(ID))
                    {
                        helper.AddElement("Condition", "ID", ID);
                        hasKey = true;
                    }
                }

                if (hasKey)
                    foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
                        Result.Add(item.OuterXml);

                return Result;
            }

            public override string Name
            {
                get { return "SCETake"; }
            }

            public override ICacheProvider Provider
            {
                get { return CacheProvider; }
            }
        }

        static SCETake()
        {
            //若沒有指定Cache此行會爆掉要重寫
            //AfterUpdate += new EventHandler<DataChangedEventArgs>(EntityHelper.Cache.NotifyRemove);
            //AfterDelete += new EventHandler<DataChangedEventArgs>(EntityHelper.Cache.NotifyRemove);
        }

        /// <summary>
        /// 指定快取介面，在設定快取介面時會將快取內對應的資料清空。
        /// </summary>
        public static ICacheProvider CacheProvider
        {
            get { return mCacheProvider; }
            set 
            {
                mCacheProvider = value;
                mCacheProvider.Delete("SCETake");
            }
        }

        /// <summary>
        /// 取得所有學生期中成績列表。
        /// </summary>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectAll();
        ///     </code>
        /// </example>
        [SelectMethod("K12.SCETake.SelectAll", "成績.評量成績")]
        public static List<SCETakeRecord> SelectAll()
        {
            return SelectAll<K12.Data.SCETakeRecord>();
        }

        /// <summary>
        /// 取得所有學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectAll&lt;K12.Data.SCETakeRecord&gt;();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : SCETakeRecord, new()
        {
            List<SCETakeRecord> result = new List<SCETakeRecord>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");

            List<T> Types = new List<T>();

            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆學生期中成績編號取得學生期中成績列表。
        /// </summary>
        /// <param name="SCETakeIDs">多筆學生期中成績編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByIDs(SCETakeIDs);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> Select(IEnumerable<string> CourseIDs, IEnumerable<string> StudentIDs, IEnumerable<string> ExamIDs, IEnumerable<string> SCETakeIDs, IEnumerable<string> SCAttendIDs)
        {
            return Select<K12.Data.SCETakeRecord>(CourseIDs,StudentIDs,ExamIDs,SCETakeIDs,SCAttendIDs);
        }

        /// <summary>
        /// 根據多筆學生期中成績編號取得學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <param name="SCETakeIDs">多筆學生期中成績編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByIDs&lt;K12.Data.SCETakeRecord&gt;(SCETakeIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> CourseIDs, IEnumerable<string> StudentIDs, IEnumerable<string> ExamIDs, IEnumerable<string> SCETakeIDs, IEnumerable<string> SCAttendIDs) where T : SCETakeRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                foreach (var item in CourseIDs)
                    if (!string.IsNullOrEmpty(item))
                        helper.AddElement("Condition", "CourseID", item);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
                foreach (var item in StudentIDs)
                    if (!string.IsNullOrEmpty(item))
                        helper.AddElement("Condition", "StudentID", item);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(ExamIDs))
                foreach (var item in ExamIDs)
                    if (!string.IsNullOrEmpty(item))
                        helper.AddElement("Condition", "ExamID", item);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SCETakeIDs))
                foreach (var item in SCETakeIDs)
                    if (!string.IsNullOrEmpty(item))
                        helper.AddElement("Condition", "ID", item);

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SCAttendIDs))
                foreach (var item in SCAttendIDs)
                    if (!string.IsNullOrEmpty(item))
                        helper.AddElement("Condition", "RefSCAttendID", item);

            //判斷若是沒有任何Condition則傳回空集合
            if (!string.IsNullOrEmpty(helper.GetElement("Condition").InnerXml))
            {
                if (CacheProvider == null)
                {
                    helper.AddElement("Field", "All");

                    foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
                    {
                        T Type = new T();
                        Type.Load(item);
                        Types.Add(Type);
                    }
                }
                else
                {
                    helper.AddElement("Field", "ID");

                    List<string> IDs = new List<string>();

                    foreach (XmlElement element in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
                    {
                        DSXmlHelper studenthelper = new DSXmlHelper(element);
                        IDs.Add(studenthelper.GetText("@ID"));
                    }

                    foreach (string element in EntityHelper.Cache.SelectByIDs(IDs))
                    {
                        T Type = new T();
                        Type.Load(element);
                        Types.Add(Type);
                    }
                }
            }
            
            return Types;
        }

        /// <summary>
        /// 根據多筆學生期中成績編號取得學生期中成績列表。
        /// </summary>
        /// <param name="SCETakeIDs">多筆學生期中成績編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByIDs(SCETakeIDs);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByIDs(IEnumerable<string> SCETakeIDs)
        {
            return SelectByIDs<K12.Data.SCETakeRecord>(SCETakeIDs);
        }

        /// <summary>
        /// 根據多筆學生期中成績編號取得學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <param name="SCETakeIDs">多筆學生期中成績編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByIDs&lt;K12.Data.SCETakeRecord&gt;(SCETakeIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> SCETakeIDs) where T:SCETakeRecord,new()
        {
            List<T> Types = new List<T>();

            //若是SelectByIDs則直接用Cache來取得資料即可，因為不用再向Server要ID是哪些
            //foreach (string Content in EntityHelper.Cache.SelectByIDs(SCETakeIDs))
            //{
            //    T Type = new T();
            //    Type.Load(Content);
            //    Types.Add(Type);
            //}

            //return Types;

            return Select<T>(null, null, null, SCETakeIDs, null);

            //bool hasKey = false;

            //List<T> Types = new List<T>();

            //DSXmlHelper helper = new DSXmlHelper("Request");
            //helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            //helper.AddElement("Condition");
            //foreach (var item in SCETakeIDs)
            //{
            //    helper.AddElement("Condition", "ID", item);
            //    hasKey = true;
            //}

            //if (hasKey)
            //{
            //    foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            //    {
            //        T Type = new T();
            //        Type.Load(item);
            //        Types.Add(Type);
            //    }
            //}

            //return Types;
        }

        /// <summary>
        /// 根據課程編號及考試項目編號取得學生期中成績列表。
        /// </summary>
        /// <param name="CourseID">課程編號</param> 
        /// <param name="ExamID">考試項目編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByCourseAndExam(CourseID,ExamID);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByCourseAndExam(string CourseID, string ExamID)
        {
            List<string> courseIDs = new List<string>();
            courseIDs.Add(CourseID);

            return SelectByCourseAndExam<K12.Data.SCETakeRecord>(courseIDs, ExamID);
        }

        
        /// <summary>
        /// 根據課程編號及考試項目編號取得學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <param name="CourseID">課程編號</param> 
        /// <param name="ExamID">考試項目編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByCourseAndExam&lt;K12.Data.SCETakeRecord&gt;(CourseID,ExamID);
        ///     </code>
        /// </example>
        protected static List<T> SelectByCourseAndExam<T>(string CourseID, string ExamID) where T:SCETakeRecord,new()
        {
            List<string> courseIDs = new List<string>();
            courseIDs.Add(CourseID);

            return SelectByCourseAndExam<T>(courseIDs, ExamID);
        }

        /// <summary>
        /// 根據多筆課程編號及單筆考試項目編號取得學生期中成績列表。
        /// </summary>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="ExamID">單筆考試項目編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByCourseAndExam(CourseIDs,ExamID);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByCourseAndExam(List<string> CourseIDs, string ExamID)
        {
            return SelectByCourseAndExam<K12.Data.SCETakeRecord>(CourseIDs, ExamID);
        }

        /// <summary>
        /// 根據多筆課程編號及單筆考試項目編號取得學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="ExamID">單筆考試項目編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByCourseAndExam&lt;K12.Data.SCETakeRecord&gt;(CourseIDs,ExamID);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByCourseAndExam<T>(List<string> CourseIDs, string ExamID) where T:SCETakeRecord,new()
        {
            List<string> ExamIDs = new List<string>(){ExamID};

            return Select<T>(CourseIDs, null, ExamIDs, null, null);
            
            //List<T> Types = new List<T>();

            //DSXmlHelper helper = new DSXmlHelper("Request");
            //helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            //helper.AddElement("Condition");
            //if (CourseIDs != null)
            //{
            //    foreach (string courseID in CourseIDs)
            //    {
            //        if (!string.IsNullOrEmpty(courseID))
            //            helper.AddElement("Condition", "CourseID", courseID);
            //    }
            //}
            //if (!string.IsNullOrEmpty(ExamID))
            //    helper.AddElement("Condition", "ExamID", ExamID);

            //foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            //{
            //    T Type = new T();
            //    Type.Load(item);
            //    Types.Add(Type);
            //}

            //return Types;
        }

        /// <summary>
        /// 根據學生編號及課程編號取得學生期中成績列表。
        /// </summary>
        /// <param name="CourseID">課程編號</param> 
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentAndCourse(StudentID,CourseID);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByStudentAndCourse(string StudentID, string CourseID)
        {
            return SelectByStudentAndCourse<K12.Data.SCETakeRecord>(StudentID, CourseID);
        }

        
        /// <summary>
        /// 根據學生編號及課程編號取得學生期中成績列表。
        /// </summary>
        /// <typeparam name="T">學生期中成績記錄物件型別，K12共用為K12.Data.SCETakeRecord</typeparam>
        /// <param name="CourseID">課程編號</param> 
        /// <param name="StudentID">學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentAndCourse&lt;K12.Data.SCETakeRecord&gt;(StudentID,CourseID);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentAndCourse<T>(string StudentID,string CourseID) where T:SCETakeRecord,new()
        {
            List<string> StudentIDs = new List<string>() { StudentID };
            List<string> CourseIDs = new List<string>() { CourseID };

            return Select<T>(CourseIDs, StudentIDs, null, null, null);

            //List<T> Types = new List<T>();

            //DSXmlHelper helper = new DSXmlHelper("Request");
            //helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            //helper.AddElement("Condition");
            //if (!string.IsNullOrEmpty(CourseID))
            //    helper.AddElement("Condition", "CourseID", CourseID);
            //if (!string.IsNullOrEmpty(StudentID))
            //    helper.AddElement("Condition", "StudentID", StudentID);

            //foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            //{
            //    T Type = new T();
            //    Type.Load(item);
            //    Types.Add(Type);
            //}

            //return Types;
        }

        /// <summary>
        /// 根據多筆學生編號及多筆課程編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param> 
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentAndCourse(StudentIDs,CourseIDs);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByStudentAndCourse(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs)
        {
            return SelectByStudentAndCourse<SCETakeRecord>(StudentIDs, CourseIDs);
        }

        /// <summary>
        /// 根據多筆學生編號及多筆課程編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param> 
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentAndCourse(StudentIDs,CourseIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentAndCourse<T>(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs) where T:SCETakeRecord,new()
        {
            return Select<T>(CourseIDs, StudentIDs, null, null, null);

            //bool HasKey = false;

            //List<T> Types = new List<T>();

            //DSXmlHelper helper = new DSXmlHelper("Request");
            //helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            //helper.AddElement("Condition");

            //if (StudentIDs != null)
            //{
            //    foreach (string StudentID in StudentIDs)
            //    {
            //        if (!string.IsNullOrEmpty(StudentID))
            //        {
            //            helper.AddElement("Condition", "StudentID", StudentID);
            //            HasKey = true;
            //        }
            //    }
            //}

            //if (CourseIDs != null)
            //{
            //    foreach (string CourseID in CourseIDs)
            //    {
            //        if (!string.IsNullOrEmpty(CourseID))
            //        {
            //            helper.AddElement("Condition", "CourseID", CourseID);
            //            HasKey = true;
            //        }
            //    }
            //}

            //if (HasKey == true)
            //{
            //    foreach (var item in K12.Data.Utility.DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            //    {
            //        T Type = new T();
            //        Type.Load(item);
            //        Types.Add(Type);
            //    }
            //}

            //return Types;
        }

        /// <summary>
        /// 根據單筆學生編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentID(StudentID);
        ///     </code>
        /// </example>
        public static List<SCETakeRecord> SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<SCETakeRecord>(StudentID);
        }


        /// <summary>
        /// 根據單筆學生編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentID(StudentID);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentID<T>(string StudentID) where T:SCETakeRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(StudentID);

            return SelectByStudentIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentID(StudentIDs);
        /// </example>
        public static List<SCETakeRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<SCETakeRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生期中成績列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SCETakeRecord&gt;，代表多筆學生期中成績記錄物件。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SCETakeRecord&gt; records = SCETake.SelectByStudentID(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:SCETakeRecord ,new()
        {
            return Select<T>(null,StudentIDs,null,null,null);
            //bool hasKey = false;

            //List<T> Types = new List<T>();

            //DSXmlHelper helper = new DSXmlHelper("Request");
            //helper.AddElement("Field");
            //helper.AddElement("Field", "All");
            //helper.AddElement("Condition");

            //if (StudentIDs != null)
            //{
            //    foreach (string StudentID in StudentIDs)
            //    {
            //        if (!string.IsNullOrEmpty(StudentID))
            //        {
            //            helper.AddElement("Condition", "StudentID", StudentID);
            //            hasKey = true;
            //        }
            //    }
            //}

            //if (hasKey == true)
            //{
            //    foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("Score"))
            //    {
            //        T Type = new T();
            //        Type.Load(item);
            //        Types.Add(Type);
            //    }
            //}

            //return Types;
        }

        /// <summary>
        /// 新增單筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecord">學生期中成績記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static string Insert(SCETakeRecord SCETakeRecord)
        {
            List<SCETakeRecord> Params = new List<SCETakeRecord>();

            Params.Add(SCETakeRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecords">多筆學生期中成績記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static List<string> Insert(IEnumerable<SCETakeRecord> SCETakeRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<SCETakeRecord> worker = new MultiThreadWorker<SCETakeRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SCETakeRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("ScoreSheetList");
                    helper.AddElement("ScoreSheetList", "ScoreSheet");
                    helper.AddElement("ScoreSheetList/ScoreSheet", "Score", "" + K12.Data.Decimal.GetString(editor.Score));
                    helper.AddElement("ScoreSheetList/ScoreSheet", "ExamID", editor.RefExamID);
                    helper.AddElement("ScoreSheetList/ScoreSheet", "AttendID", editor.RefSCAttendID);

                    helper.AddElement("ScoreSheetList/ScoreSheet", "Extension");

                    if (editor.Extension!=null)
                        helper.AddElement("ScoreSheetList/ScoreSheet/Extension", editor.Extension);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<SCETakeRecord>> packages = worker.Run(SCETakeRecords);

            foreach (PackageWorkEventArgs<SCETakeRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecord">學生期中成績記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static int Update(SCETakeRecord SCETakeRecord)
        {
            List<SCETakeRecord> Params = new List<SCETakeRecord>();

            Params.Add(SCETakeRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecords">多筆學生期中成績記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<SCETakeRecord> SCETakeRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<SCETakeRecord> worker = new MultiThreadWorker<SCETakeRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SCETakeRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("ScoreSheetList");
                    updateHelper.AddElement("ScoreSheetList", "ScoreSheet");
                    updateHelper.AddElement("ScoreSheetList/ScoreSheet", "Score", "" + editor.Score);
                    updateHelper.AddElement("ScoreSheetList/ScoreSheet", "ExamID", editor.RefExamID);
                    updateHelper.AddElement("ScoreSheetList/ScoreSheet", "AttendID", editor.RefSCAttendID);

                    updateHelper.AddElement("ScoreSheetList/ScoreSheet", "Extension");

                    if (editor.Extension!=null)
                        updateHelper.AddElement("ScoreSheetList/ScoreSheet/Extension", editor.Extension);

                    //這個 Element 是 Condition。這支 Service 寫法比較怪一點。
                    updateHelper.AddElement("ScoreSheetList/ScoreSheet", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<SCETakeRecord>> packages = worker.Run(SCETakeRecords);

            foreach (PackageWorkEventArgs<SCETakeRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecord">學生期中成績記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        static public int Delete(SCETakeRecord SCETakeRecord)
        {
            return Delete(SCETakeRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeID">學生期中成績編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        static public int Delete(string SCETakeID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(SCETakeID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeRecords">多筆學生期中成績記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SCETakeRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        static public int Delete(IEnumerable<SCETakeRecord> SCETakeRecords)
        {
            List<string> Keys = new List<string>();

            foreach (SCETakeRecord SCETakeRecord in SCETakeRecords)
                Keys.Add(SCETakeRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生期中成績記錄
        /// </summary>
        /// <param name="SCETakeIDs">多筆學生期中成績編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> SCETakeIDs)
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
                    helper.AddElement("ScoreSheet");
                    helper.AddElement("ScoreSheet", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(SCETakeIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(SCETakeIDs, ChangedSource.Local));

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
    }
}