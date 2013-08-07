using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學期歷程類別，提供方法用來取得及修改學期歷程資訊
    /// </summary>
    public class SemesterHistory
    {
        private const string SELECT_SERVICE = "SmartSchool.Student.GetDetailList";
        private const string UPDATE_SERVICE = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生學期歷程細項列表。
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.SemesterHistory.SelectAllDetail", "學籍.學期歷程")]
        public static List<SemesterHistoryItem> SelectAllDetail()
        {
            List<SemesterHistoryItem> Items = new List<SemesterHistoryItem>();

            foreach (SemesterHistoryRecord Record in SelectAll())
                foreach (SemesterHistoryItem Item in Record.SemesterHistoryItems)
                    Items.Add(Item);

            return Items;
        }

        /// <summary>
        /// 取得所有學生學期歷程列表。
        /// </summary>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectAll();
        ///         
        ///         foreach(SemesterHistoryRecord record in records)
        ///             System.Console.Writeln(record.SchoolYear); 
        ///     </code>
        /// </example>
        public static List<SemesterHistoryRecord> SelectAll()
        {
            return SelectAll<SemesterHistoryRecord>();
        }

        /// <summary>
        /// 取得所有學生學期歷程列表。
        /// </summary>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectAll();
        ///         
        ///         foreach(SemesterHistoryRecord record in records)
        ///             System.Console.Writeln(record.SchoolYear); 
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : SemesterHistoryRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "SemesterHistory" }).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據學生編號取得學期歷程。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>SemesterHistoryRecord，代表學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     SemesterHistoryRecord record = SemesterHistory.SelectByStudentID(StudentID);
        ///     
        ///     if (record != null)
        ///     {
        ///        System.Console.WriteLine(record.SchoolYear);
        ///        System.Console.WriteLine(record.Semester);
        ///     }
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不存在則會傳回null</remarks>
        public static SemesterHistoryRecord  SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<SemesterHistoryRecord>(StudentID);
        }

        /// <summary>
        /// 根據學生編號取得學期歷程。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <returns>SemesterHistoryRecord，代表學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     SemesterHistoryRecord records = SemesterHistory.SelectByStudentID(StudentID);
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T:SemesterHistoryRecord,new()
        {
            List<string> Params = new List<string>();

            Params.Add(StudentID);

            List<T> Types = SelectByStudentIDs<T>(Params);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據學生物件取得學期歷程。
        /// </summary>
        /// <param name="Student">學生物件</param>
        /// <returns>SemesterHistoryRecord，代表學期歷程物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <sample>
        ///     <code>
        ///     SemesterHistoryRecord record = SemesterHistory.SelectByStudent(Student);
        ///     
        ///     if (record != null)
        ///     {
        ///        System.Console.WriteLine(record.SchoolYear);
        ///        System.Console.WriteLine(record.Semester);
        ///     }
        ///     </code>
        ///</sample>
        /// <remarks>若是StudentID不存在則會傳回null</remarks>
        public static SemesterHistoryRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<SemesterHistoryRecord>(Student);
        }

        /// <summary>
        /// 根據學生物件取得學期歷程。
        /// </summary>
        /// <param name="Student">學生物件</param>
        /// <returns>SemesterHistoryRecord，代表學期歷程物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     SemesterHistoryRecord records = SemesterHistory.SelectByStudentID(Student);
        /// </example>
        protected static T SelectByStudent<T>(StudentRecord Student) where T:SemesterHistoryRecord,new()
        {
            List<StudentRecord> Params = new List<StudentRecord>();

            Params.Add(Student);

            List<T> Types = SelectByStudents<T>(Params);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生物件取得學期歷程列表。
        /// </summary>
        /// <param name="Students">多筆學生物件</param>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectByStudents(Students);
        ///         
        ///         foreach(SemesterHistoryRecord record in records)
        ///             System.Console.Writeln(record.SchoolYear); 
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<SemesterHistoryRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<SemesterHistoryRecord>(Students);
        }


        /// <summary>
        /// 根據多筆學生物件取得學期歷程列表。
        /// </summary>
        /// <param name="Students">多筆學生物件</param>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectByStudentIDs(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T:SemesterHistoryRecord,new()
        {
            List<string> Params = new List<string>();

            foreach(StudentRecord student in Students)
                Params.Add(student.ID);

            return SelectByStudentIDs<T>(Params);       
        }

        /// <summary>
        /// 根據多筆學生編號取得學期歷程列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectByStudentIDs(StudentIDs);
        ///         
        ///         foreach(SemesterHistoryRecord record in records)
        ///             System.Console.Writeln(record.SchoolYear); 
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<SemesterHistoryRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<SemesterHistoryRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學期歷程列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;SemesterHistoryRecord&gt;，代表多筆學期歷程物件。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;SemesterHistoryRecord&gt; records = SemesterHistory.SelectByStudentIDs(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:SemesterHistoryRecord,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = GetDetailList(new string[] { "ID", "SemesterHistory" }, StudentIDs.ToArray()).GetContent();

            foreach (XmlElement element in helper.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(element);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 更新單筆學期歷程記錄
        /// </summary>
        /// <param name="SemesterHistoryRecord">學期歷程記錄</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     SemesterHistoryRecord record = SmesterHistory.SelectByStudentID(StudentID);
        ///     record.SchoolYear = 100;
        ///     int UpdateCount = SmesterHistory.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(SemesterHistoryRecord SemesterHistoryRecord)
        {
            List<SemesterHistoryRecord> Params = new List<SemesterHistoryRecord>();

            Params.Add(SemesterHistoryRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學期歷程記錄
        /// </summary>
        /// <param name="SemesterHistoryRecords">多筆學期歷程記錄物件</param>
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="SemesterHistoryRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     SemesterHistoryRecord record = SemesterHistory.SelectByStudentID(StudentID);
        ///     record.SchoolYear = 100;
        ///     List&lt;SemesterHistoryRecord&gt; records = new List&lt;SemesterHistoryRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = SemesterHistory.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<SemesterHistoryRecord> SemesterHistoryRecords)
        {
            int result = 0;
            List<string> IDs = new List<string>();

            MultiThreadWorker<SemesterHistoryRecord> worker = new MultiThreadWorker<SemesterHistoryRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SemesterHistoryRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student","Field");
                    updateHelper.AddElement("Student/Field", "SemesterHistory");
                    
                    foreach(SemesterHistoryItem item in editor.SemesterHistoryItems)
                    {
                        XmlElement Elm=updateHelper.AddElement("Student/Field/SemesterHistory", "History");
                        Elm.SetAttribute("ClassName",item.ClassName);
                        Elm.SetAttribute("GradeYear",K12.Data.Int.GetString(item.GradeYear));
                        Elm.SetAttribute("SchoolYear", K12.Data.Int.GetString(item.SchoolYear));
                        Elm.SetAttribute("SeatNo", K12.Data.Int.GetString(item.SeatNo));
                        Elm.SetAttribute("SchoolDayCount", K12.Data.Int.GetString(item.SchoolDayCount));
                        Elm.SetAttribute("Semester", K12.Data.Int.GetString(item.Semester));
                        Elm.SetAttribute("Teacher",item.Teacher);
                        Elm.SetAttribute("DeptName", item.DeptName);
                    }
                        
                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition","ID", editor.RefStudentID);
                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICE, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<SemesterHistoryRecord>> packages = worker.Run(SemesterHistoryRecords);

            foreach (PackageWorkEventArgs<SemesterHistoryRecord> each in packages)
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

            return DSAServices.CallService(SELECT_SERVICE, dsreq);
        }
    }
}