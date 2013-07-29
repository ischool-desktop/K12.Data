using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生前級畢業資訊類別，提供方法用來取得及修改學生前級畢業資訊。
    /// </summary>
    public class BeforeEnrollment
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生前級畢業資訊物件列表。
        /// </summary>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectAll();
        ///     
        ///     foreach(BeforeEnrollmentRecord record in records)
        ///         Console.WrlteLine(record.Reason);
        ///     </code>
        /// </example>
        [SelectMethod("K12.BeforeEnrollment.SelectAll", "學籍.前級畢業資訊")]
        public static List<BeforeEnrollmentRecord> SelectAll()
        {
            return SelectAll<K12.Data.BeforeEnrollmentRecord>();
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生前級畢業資訊物件列表。
        /// </summary>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectAll();
        ///     </code>
        /// </example>
        protected static List<T> SelectAll<T>() where T : BeforeEnrollmentRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "BeforeEnrollment" }).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生前級畢業資訊物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>BeforeEnrollmentRecord，代表學生前級畢業資訊物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudent(Student);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Memo);
        ///     </code>
        /// </example>
        /// <remarks>若是Student不則在則會傳回null</remarks>
        public static BeforeEnrollmentRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.BeforeEnrollmentRecord>(Student);
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生前級畢業資訊物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>BeforeEnrollmentRecord，代表學生前級畢業資訊物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudent(Student);
        ///     </code>
        /// </example>
        protected static T SelectByStudent<T>(StudentRecord Student) where T : BeforeEnrollmentRecord, new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生前級畢業資訊物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>BeforeEnrollmentRecord，代表學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudentID(StudentID);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        public static BeforeEnrollmentRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.BeforeEnrollmentRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生前級畢業資訊物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>BeforeEnrollmentRecord，代表學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudent(StudentID);
        ///     </code>
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T : BeforeEnrollmentRecord, new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            List<T> records = SelectByStudentIDs<T>(Keys);

            if (records.Count > 0)
                return records[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生前級畢業資訊物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectByStudents(Students);
        ///     
        ///     foreach(BeforeEnrollmentRecord record in records)
        ///         Console.WrlteLine(record.Memo);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<BeforeEnrollmentRecord> SelectByStudents(List<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.BeforeEnrollmentRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生前級畢業資訊物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectByStudents(Students);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudents<T>(List<StudentRecord> Students) where T : BeforeEnrollmentRecord, new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生前級畢業資訊物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(BeforeEnrollmentRecord record in records)
        ///         Console.WrlteLine(record.Reason);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<BeforeEnrollmentRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.BeforeEnrollmentRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生前級畢業資訊物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;BeforeEnrollmentRecord&gt;，代表多筆學生前級畢業資訊物件。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;BeforeEnrollmentRecord&gt; records = BeforeEnrollment.SelectByStudents(StudentIDs);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : BeforeEnrollmentRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "BeforeEnrollment" }, StudentIDs.ToArray()).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 更新單筆學生前級畢業資訊
        /// </summary>
        /// <param name="BeforeEnrollmentRecord">學生前級畢業資訊物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudentID(StudentID);
        ///     record.Memo = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = BeforeEnrollment.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(BeforeEnrollmentRecord BeforeEnrollmentRecord)
        {
            List<BeforeEnrollmentRecord> Params = new List<BeforeEnrollmentRecord>();

            Params.Add(BeforeEnrollmentRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生前級畢業資訊
        /// </summary>
        /// <param name="BeforeEnrollmentRecords">多筆學生前級畢業資訊物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="BeforeEnrollmentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     BeforeEnrollmentRecord record = BeforeEnrollment.SelectByStudentID(StudentID);
        ///     record.Reason = (new System.Random()).NextDouble().ToString();
        ///     List&lt;BeforeEnrollmentRecord&gt; records = new List&lt;BeforeEnrollmentRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = BeforeEnrollment.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<BeforeEnrollmentRecord> BeforeEnrollmentRecords)
        {
            int result = 0;
            List<string> IDs = new List<string>();

            MultiThreadWorker<BeforeEnrollmentRecord> worker = new MultiThreadWorker<BeforeEnrollmentRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<BeforeEnrollmentRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "BeforeEnrollment");
                    updateHelper.AddElement("Student/Field/BeforeEnrollment", "BeforeEnrollment");
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "ClassName", editor.ClassName);
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "School", editor.School);
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "SchoolLocation", editor.SchoolLocation);
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "SeatNo", K12.Data.Int.GetString(editor.SeatNo));
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "Memo", editor.Memo);
                    updateHelper.AddElement("Student/Field/BeforeEnrollment/BeforeEnrollment", "GraduateSchoolYear", editor.GraduateSchoolYear);
                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);
                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<BeforeEnrollmentRecord>> packages = worker.Run(BeforeEnrollmentRecords);

            foreach (PackageWorkEventArgs<BeforeEnrollmentRecord> each in packages)
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