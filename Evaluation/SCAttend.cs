using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生修課類別，提供方法用來取得、新增、修改及刪除學生修課資訊
    /// </summary>
    public class SCAttend
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Course.GetSCAttendAll";
        private const string INSERT_SERVICENAME = "SmartSchool.Course.InsertSCAttend";
        private const string UPDATE_SERVICENAME = "SmartSchool.Course.UpdateSCAttend";
        private const string DELETE_SERVICENAME = "SmartSchool.Course.DeleteSCAttend";

        /// <summary>
        /// 取得所有學生修課列表。
        /// </summary>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SCAttendRecord&gt; scattendrecords = SCAttend.SelectAll();
        /// </example>
        [SelectMethod("K12.SCAttend.SelectAll", "成績.學生修課")]
        public static List<SCAttendRecord> SelectAll()
        {
            return SelectAll<K12.Data.SCAttendRecord>();
        }

        /// <summary>
        /// 取得所有學生修課列表。
        /// </summary>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:SCAttendRecord,new()
        {
            DSXmlHelper helper = new DSXmlHelper("SelectRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentID");
            helper.AddElement("Field", "RefCourseID");
            helper.AddElement("Field", "IsRequired");
            helper.AddElement("Field", "RequiredBy");
            helper.AddElement("Field", "Score");
            helper.AddElement("Field", "Extension");
            helper.AddElement("Field", "Extensions");
            helper.AddElement("Condition");
            helper.AddElement("Order");
            DSRequest dsreq = new DSRequest(helper);

            List<T> Types = new List<T>();
            
            foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Student"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據條件取得學生修課列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="SCAttendIDs">多筆學生修課列表</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        public static List<SCAttendRecord> Select(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs, IEnumerable<string> SCAttendIDs, string SchoolYear, string Semester)
        {
            return Select<K12.Data.SCAttendRecord>(StudentIDs, CourseIDs, SCAttendIDs, SchoolYear, Semester);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")]
        public static List<string> SelectIDs(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs, IEnumerable<string> SCAttendIDs, string SchoolYear, string Semester)
        {
            bool hasKey = false;

            DSXmlHelper helper = new DSXmlHelper("SelectRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
                foreach (var item in StudentIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "StudentID", item);
                        hasKey = true;
                    }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                foreach (var item in CourseIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "CourseID", item);
                        hasKey = true;
                    }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SCAttendIDs))
                foreach (var item in SCAttendIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "SCAttendID", item);
                        hasKey = true;
                    }

            if (!string.IsNullOrEmpty(SchoolYear))
            {
                helper.AddElement("Condition", "SchoolYear", SchoolYear);
                hasKey = true;
            }

            if (!string.IsNullOrEmpty(Semester))
            {
                helper.AddElement("Condition", "Semester", Semester);
                hasKey = true;
            }

            helper.AddElement("Order");

            List<string> IDs = new List<string>();

            if (hasKey)
            {
                DSRequest dsreq = new DSRequest(helper);
                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Student"))
                    IDs.Add(item.GetAttribute("ID"));
            }

            return IDs;
        }

        /// <summary>
        /// 根據條件取得學生修課列表。
        /// </summary>
        /// <typeparam name="T">學生修課類別</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <param name="SCAttendIDs">多筆學生修課列表</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>List&lt;T&gt;，代表多筆學生修課記錄物件。</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs,IEnumerable<string> SCAttendIDs,string SchoolYear,string Semester) where T : SCAttendRecord, new()
        {
            bool hasKey = false;

            DSXmlHelper helper = new DSXmlHelper("SelectRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "RefStudentID");
            helper.AddElement("Field", "RefCourseID");
            helper.AddElement("Field", "IsRequired");
            helper.AddElement("Field", "RequiredBy");
            helper.AddElement("Field", "Score");
            helper.AddElement("Field", "Extension");
            helper.AddElement("Field", "Extensions");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
                foreach (var item in StudentIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "StudentID", item);
                        hasKey = true;
                    }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(CourseIDs))
                foreach (var item in CourseIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "CourseID", item);
                        hasKey = true;
                    }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SCAttendIDs))
                foreach (var item in SCAttendIDs)
                    if (!string.IsNullOrEmpty(item))
                    {
                        helper.AddElement("Condition", "SCAttendID", item);
                        hasKey = true;
                    }

            if (!string.IsNullOrEmpty(SchoolYear))
            {
                helper.AddElement("Condition", "SchoolYear", SchoolYear);
                hasKey = true;
            }

            if (!string.IsNullOrEmpty(Semester))
            {
                helper.AddElement("Condition", "Semester", Semester);
                hasKey = true; 
            }

            helper.AddElement("Order");

            List<T> Types = new List<T>();

            if (hasKey)
            {
                DSRequest dsreq = new DSRequest(helper);
                foreach (var item in DSAServices.CallService(SELECT_SERVICENAME, dsreq).GetContent().GetElements("Student"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生修課編號取得學生修課列表。
        /// </summary>
        /// <param name="SCAttendID">單筆學生修課編號</param>
        /// <returns>SCAttendRecord，代表單筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     SCAttendRecord record = SCAttend.SelectByID(SCAttendID);
        ///     </code>
        /// </example>
        public static SCAttendRecord SelectByID(string SCAttendID)
        {
            return SelectByID<K12.Data.SCAttendRecord>(SCAttendID);
        }

        
        /// <summary>
        /// 根據單筆學生修課編號取得學生修課列表。
        /// </summary>
        /// <typeparam name="T">學生修課記錄物件型別，K12共用為K12.Data.SCAttendRecord</typeparam>
        /// <param name="SCAttendID">單筆學生修課編號</param>
        /// <returns>SCAttendRecord，代表單筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     SCAttendRecord record = SCAttend.SelectByID&lt;K12.Data.SCAttendRecord&gt;(SCAttendID);
        ///     </code>
        /// </example>
        protected static T SelectByID<T>(string SCAttendID) where T:SCAttendRecord,new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(SCAttendID);

            List<T> Types = SelectByIDs<T>(IDs);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生修課編號取得學生修課列表。
        /// </summary>
        /// <param name="SCAttendIDs">多筆學生修課編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByIDs(SCAttendIDs);
        ///     </code>
        /// </example>
        public static List<SCAttendRecord> SelectByIDs(IEnumerable<string> SCAttendIDs)
        {
            return SelectByIDs<K12.Data.SCAttendRecord>(SCAttendIDs);
        }

        /// <summary>
        /// 根據多筆學生修課編號取得學生修課列表。
        /// </summary>
        /// <typeparam name="T">學生修課記錄物件型別，K12共用為K12.Data.SCAttendRecord</typeparam>
        /// <param name="SCAttendIDs">多筆學生修課編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByIDs&lt;K12.Data.SCAttendRecord&gt;(SCAttendIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> SCAttendIDs) where T:SCAttendRecord,new()
        {
            #region 分批取得學生修課，以每1000筆為單位
            //FunctionSpliter<string, T> Spliter = new FunctionSpliter<string, T>(1000, 3);

            //Spliter.Function = x => Select<T>(null, null, x, string.Empty, string.Empty);
            //List<T> records = Spliter.Execute(SCAttendIDs.ToList());
            #endregion

            return Select<T>(null, null, SCAttendIDs , string.Empty, string.Empty);
        }

        /// <summary>
        /// 根據多筆學生編號及多筆課程編號取得學生修課列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; scattendrecords = SCAttend.GetByStudentIDsAndCourseIDs(StudentIDs,CourseIDs);
        ///     </code>
        /// </example>
        public static List<SCAttendRecord> SelectByStudentIDAndCourseID(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs)
        {
            return SelectByStudentIDAndCourseID<SCAttendRecord>(StudentIDs, CourseIDs);
        }

        /// <summary>
        /// 根據多筆學生編號及多筆課程編號取得學生修課列表。
        /// </summary>
        /// <typeparam name="T">學生修課記錄物件型別，K12共用為K12.Data.SCAttendRecord</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="CourseIDs">多筆課程編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; scattendrecords = SCAttend.GetByStudentIDsAndCourseIDs&lt;K12.Data.SCAttendRecord&gt;(StudentIDs,CourseIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDAndCourseID<T>(IEnumerable<string> StudentIDs, IEnumerable<string> CourseIDs) where T:SCAttendRecord,new()
        {
            return Select<T>(StudentIDs, CourseIDs, null, string.Empty, string.Empty);
        }

        /// <summary>
        /// 根據單筆學生編號取得學生修課列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByStudentIDsAndCourseID(StudentID);
        ///     </code>
        /// </example>
        public static List<SCAttendRecord> SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<SCAttendRecord>(StudentID);
        }


        /// <summary>
        /// 根據單筆學生編號取得學生修課列表。
        /// </summary>
        /// <param name="StudentID">單筆學生編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByStudentIDsAndCourseID(StudentID);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentID<T>(string StudentID) where T:SCAttendRecord ,new()
        {
            List<string> IDs = new List<string>();

            if (!string.IsNullOrEmpty(StudentID))
                IDs.Add(StudentID);

            return SelectByStudentIDs<T>(IDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生修課列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByStudentIDs(StudentIDs);
        ///     </code>
        /// </example>
        public static List<SCAttendRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<SCAttendRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生修課列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SCAttendRecord&gt;，代表多筆學生修課記錄物件。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;SCAttendRecord&gt; records = SCAttend.SelectByStudentIDs(StudentIDs);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:SCAttendRecord ,new()
        {
            return Select<T>(StudentIDs, null, null, string.Empty, string.Empty);
        }

        /// <summary>
        /// 根據課程系統編號列表取得學生修課
        /// </summary>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        public static List<SCAttendRecord> SelectByCourseIDs(IEnumerable<string> CourseIDs)
        {
            return SelectByCourseIDs<SCAttendRecord>(CourseIDs);
        }

        /// <summary>
        /// 根據多筆課程系統編號取得課程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CourseIDs"></param>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByCourseIDs<T>(IEnumerable<string> CourseIDs) where T : SCAttendRecord, new()
        {
            #region 分批取得學生修課，以每250課程為單位
            //FunctionSpliter<string, T> Spliter = new FunctionSpliter<string, T>(250, 3);

            //Spliter.Function = x => Select<T>(null, null, x, string.Empty, string.Empty);
            //List<T> records = Spliter.Execute(CourseIDs.ToList());
            #endregion

            return Select<T>(null, CourseIDs, null, string.Empty, string.Empty);
        }

        /// <summary>
        /// 新增單筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecord">學生修課記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(SCAttendRecord SCAttendRecord)
        {
            List<SCAttendRecord> Params = new List<SCAttendRecord>();

            Params.Add(SCAttendRecord);

            return Insert(Params)[0];
        }

        private static string GetString(bool? Value)
        {
            if (Value.HasValue)
                if (Value.Value == true)
                    return "必";
                else
                    return "選";
            else
                return "";
        }

        /// <summary>
        /// 新增多筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecords">多筆學生修課記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<SCAttendRecord> SCAttendRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<SCAttendRecord> worker = new MultiThreadWorker<SCAttendRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SCAttendRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("InsertSCAttend");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Attend");
                    helper.AddElement("Attend", "RefStudentID", editor.RefStudentID);
                    helper.AddElement("Attend", "RefCourseID", editor.RefCourseID);

                    helper.AddElement("Attend", "Score", "" + editor.Score);
                    helper.AddElement("Attend", "IsRequired", GetString(editor.OverrideRequired));
                    helper.AddElement("Attend", "RequiredBy", editor.OverrideRequiredBy);
                 
                    helper.AddElement("Attend", "Extension");
                    helper.AddElement("Attend/Extension", "Extension");
                    helper.AddElement("Attend/Extension/Extension", "Effort", "" + editor.Effort);
                    helper.AddElement("Attend/Extension/Extension", "Text", editor.Text);
                    helper.AddElement("Attend/Extension/Extension", "OrdinarilyEffort", "" + editor.OrdinarilyEffort);
                    helper.AddElement("Attend/Extension/Extension", "OrdinarilyScore", "" + editor.OrdinarilyScore);

                    helper.AddElement("Attend", "Extensions");
                    if (editor.Extensions!=null)
                        helper.AddElement("Attend/Extensions",editor.Extensions);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<SCAttendRecord>> packages = worker.Run(SCAttendRecords);

            foreach (PackageWorkEventArgs<SCAttendRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecord">學生修課記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(SCAttendRecord SCAttendRecord)
        {
            List<SCAttendRecord> Params = new List<SCAttendRecord>();

            Params.Add(SCAttendRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecords">多筆學生修課記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<SCAttendRecord> SCAttendRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<SCAttendRecord> worker = new MultiThreadWorker<SCAttendRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SCAttendRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateSCAttend");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Attend");
                    updateHelper.AddElement("Attend", "RefCourseID", "" + editor.RefCourseID);
                    updateHelper.AddElement("Attend", "IsRequired", GetString(editor.OverrideRequired));
                    updateHelper.AddElement("Attend", "RequiredBy", editor.OverrideRequiredBy);
                    updateHelper.AddElement("Attend", "ID", editor.ID);
                    updateHelper.AddElement("Attend", "Score", "" + editor.Score);

                    updateHelper.AddElement("Attend", "Extension");
                    updateHelper.AddElement("Attend/Extension", "Extension");
                    updateHelper.AddElement("Attend/Extension/Extension", "Effort", "" + editor.Effort);
                    updateHelper.AddElement("Attend/Extension/Extension", "Text", editor.Text);
                    updateHelper.AddElement("Attend/Extension/Extension", "OrdinarilyEffort", "" + editor.OrdinarilyEffort);
                    updateHelper.AddElement("Attend/Extension/Extension", "OrdinarilyScore", "" + editor.OrdinarilyScore);

                    updateHelper.AddElement("Attend", "Extensions");

                    if (editor.Extensions!=null)
                        updateHelper.AddElement("Attend/Extensions", editor.Extensions);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<SCAttendRecord>> packages = worker.Run(SCAttendRecords);

            foreach (PackageWorkEventArgs<SCAttendRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecord">學生修課記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(SCAttendRecord SCAttendRecord)
        {
            return Delete(SCAttendRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendID">學生修課編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string SCAttendID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(SCAttendID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendRecords">多筆學生修課記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="SCAttendRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<SCAttendRecord> SCAttendRecords)
        {
            List<string> Keys = new List<string>();

            foreach (SCAttendRecord SCAttendRecord in SCAttendRecords)
                Keys.Add(SCAttendRecord.ID);

            return Delete(Keys);
        }


        /// <summary>
        /// 刪除多筆學生修課記錄
        /// </summary>
        /// <param name="SCAttendIDs">多筆學生修課編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> SCAttendIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("DeleteSCAttendRequest");

                foreach (string Key in e.List)
                {
                    helper.AddElement("Attend");
                    helper.AddElement("Attend", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(SCAttendIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(SCAttendIDs, ChangedSource.Local));

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