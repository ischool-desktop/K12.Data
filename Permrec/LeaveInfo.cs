using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生離校資訊類別，提供方法用來取得及修改學生地址資訊
    /// </summary>
    public class LeaveInfo
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生離校資訊物件列表。
        /// </summary>
        /// <returns>List&lt;LeaveInfoRecord&gt;，代表多筆學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;LeaveInfoRecord&gt; records = LeaveInfo.SelectAll();
        ///     
        ///     foreach(LeaveInfoRecord record in records)
        ///         Console.WrlteLine(record.Reason);
        ///     </code>
        /// </example>
        [SelectMethod("K12.LeaveInfo.SelectAll", "學籍.離校資訊")]
        public static List<LeaveInfoRecord> SelectAll()
        {
            return SelectAll<K12.Data.LeaveInfoRecord>();
        }

        /// <summary>
        /// 取得所有學生離校資訊物件列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static List<T> SelectAll<T>() where T : LeaveInfoRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "LeaveInfo", "DiplomaNumber" }).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生離校資訊物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>LeaveInfoRecord，代表學生離校資訊物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudent(Student);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>若是Student不則在則會傳回null</remarks>
        public static LeaveInfoRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.LeaveInfoRecord>(Student);
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生離校資訊物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>LeaveInfoRecord，代表學生離校資訊物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudent(Student);
        /// </example>
        protected static T SelectByStudent<T>(StudentRecord Student) where T:LeaveInfoRecord ,new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生離校資訊物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>LeaveInfoRecord，代表學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudentID(StudentID);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        public static LeaveInfoRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.LeaveInfoRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生離校資訊物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>LeaveInfoRecord，代表學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudent(StudentID);
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T:LeaveInfoRecord ,new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            List<T> LeaveInfoRecords = SelectByStudentIDs<T>(Keys);

            if (LeaveInfoRecords.Count > 0)
                return LeaveInfoRecords[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生離校資訊物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;LeaveInfoRecord&gt;，代表多筆學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;LeaveInfoRecord&gt; records = LeaveInfo.SelectByStudents(Students);
        ///     
        ///     foreach(LeaveInfoRecord record in records)
        ///         Console.WrlteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<LeaveInfoRecord> SelectByStudents(List<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.LeaveInfoRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生離校資訊物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;LeaveInfoRecord&gt;，代表多筆學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;LeaveInfoRecord&gt; records = LeaveInfo.SelectByStudents(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(List<StudentRecord> Students) where T:LeaveInfoRecord ,new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生離校資訊物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;LeaveInfoRecord&gt;，代表多筆學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;LeaveInfoRecord&gt; records = LeaveInfo.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(LeaveInfoRecord record in records)
        ///         Console.WrlteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<LeaveInfoRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.LeaveInfoRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生離校資訊物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;LeaveInfoRecord&gt;，代表多筆學生離校資訊物件。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;LeaveInfoRecord&gt; records = LeaveInfo.SelectByStudents(StudentIDs);
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:LeaveInfoRecord ,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "LeaveInfo", "DiplomaNumber" }, StudentIDs.ToArray()).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 更新單筆學生離校資訊
        /// </summary>
        /// <param name="LeaveInfoRecord">學生離校資訊物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudentID(StudentID);
        ///     record.Reason = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = LeaveInfo.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(LeaveInfoRecord LeaveInfoRecord)
        {
            List<LeaveInfoRecord> Params = new List<LeaveInfoRecord>();

            Params.Add(LeaveInfoRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生離校資訊
        /// </summary>
        /// <param name="LeaveInfoRecords">多筆學生離校資訊物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="LeaveInfoRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     LeaveInfoRecord record = LeaveInfo.SelectByStudentID(StudentID);
        ///     record.Reason = (new System.Random()).NextDouble().ToString();
        ///     List&lt;LeaveInfoRecord&gt; records = new List&lt;LeaveInfoRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = LeaveInfo.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<LeaveInfoRecord> LeaveInfoRecords)
        {
            int result = 0;
            List<string> IDs = new List<string>();

            MultiThreadWorker<LeaveInfoRecord> worker = new MultiThreadWorker<LeaveInfoRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<LeaveInfoRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "LeaveInfo");
                    updateHelper.AddElement("Student/Field","DiplomaNumber");
                    updateHelper.AddElement("Student/Field/DiplomaNumber", "DiplomaNumber",editor.DiplomaNumber);

                    XmlElement Elm = updateHelper.AddElement("Student/Field/LeaveInfo", "LeaveInfo");

                    Elm.SetAttribute("ClassName", editor.ClassName);
                    Elm.SetAttribute("Memo", editor.Memo);
                    Elm.SetAttribute("Reason", editor.Reason);
                    Elm.SetAttribute("Department",editor.DepartmentName);

                    if (editor.SchoolYear == null)
                      Elm.SetAttribute("SchoolYear", "");
                    else
                      Elm.SetAttribute("SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));

                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);
                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<LeaveInfoRecord>> packages = worker.Run(LeaveInfoRecords);

            foreach (PackageWorkEventArgs<LeaveInfoRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        /// <summary>
        /// 取得詳細資料列表
        /// </summary>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static DSResponse GetDetailList(IEnumerable<string> fields, params string[] list)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            bool hasfield = false;

            if (fields!=null)
                foreach (string field in fields)
                    if (!string.IsNullOrEmpty(field))
                    {
                        helper.AddElement("Field", field);
                        hasfield = true;
                    }

            if (!hasfield)
                throw new Exception("必須傳入Field");
            
            helper.AddElement("Condition");

            if (list!=null)
                foreach (string id in list)
                    if (!string.IsNullOrEmpty(id))
                        helper.AddElement("Condition", "ID", id);

            dsreq.SetContent(helper);
            return DSAServices.CallService(SELECT_SERVICENAME, dsreq);
        }
    }
}