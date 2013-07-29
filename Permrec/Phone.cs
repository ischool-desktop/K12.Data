using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生電話類別，提供方法用來取得及修改學生電話資訊
    /// </summary>
    public class Phone
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetPhoneDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生電話記錄物件。
        /// </summary>
        /// <returns>List&lt;PhoneRecord&gt;，代表多筆學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;PhoneRecord&gt; records = Phone.SelectAll();
        ///     
        ///     foreach(PhoneRecord record in records)
        ///         Console.WrlteLine(record.Permanent);
        ///     </code>
        /// </example>
        [SelectMethod("K12.Phone.SelectAll", "學籍.學生電話")]
        public static List<PhoneRecord> SelectAll()
        {
            return SelectAll<K12.Data.PhoneRecord>();
        }

        /// <summary>
        /// 取得所有學生電話記錄物件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : PhoneRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("All");
            helper.AddElement("Condition");

            DSXmlHelper rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent();
            foreach (XmlElement each in rsp.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生電話記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>PhoneRecord，代表學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     PhoneRecord record = Phone.SelectByStudentID(StudentID);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Permanent);
        ///     </code>
        /// </example>
        /// <remarks>若是Student不則在則會傳回null</remarks>
        public static PhoneRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.PhoneRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生電話記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>PhoneRecord，代表學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     PhoneRecord phonerec = Phone.SelectByStudent(StudentID);
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T:PhoneRecord,new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            List<T> phones = SelectByStudentIDs<T>(Keys);

            if (phones.Count > 0)
                return phones[0];
            else
                return null;
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生電話記錄物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>PhoneRecord，代表學生電話記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     PhoneRecord record = Phone.SelectByStudent(Student);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Permanent);
        ///     </code>
        /// </example>
        /// <remarks>若是Student不則在則會傳回null</remarks>
        public static PhoneRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.PhoneRecord>(Student);
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生電話記錄物件。
        /// </summary>
        /// <param name="StudentRecord">學生記錄物件</param>
        /// <returns>PhoneRecord，代表學生電話記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     PhoneRecord phonerec = Phone.SelectByStudent(Student);
        /// </example>
        protected static T SelectByStudent<T>(StudentRecord Student) where T:PhoneRecord,new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生電話記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;PhoneRecord&gt;，代表多筆學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;PhoneRecord&gt; records = Phone.SelectByStudents(Students);
        ///     
        ///     foreach(PhoneRecord record in records)
        ///         Console.WrlteLine(record.Permanent);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<PhoneRecord> SelectByStudents(List<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.PhoneRecord>(Students);
        }
        
        /// <summary>
        /// 根據多筆學生記錄物件取得學生電話記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;PhoneRecord&gt;，代表多筆學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;PhoneRecord&gt; phonerecs = Phone.SelectByStudents(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(List<StudentRecord> Students) where T:PhoneRecord,new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生電話記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;PhoneRecord&gt;，代表多筆學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;PhoneRecord&gt; records = Phone.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(PhoneRecord record in records)
        ///         Console.WrlteLine(record.Permanent);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<PhoneRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.PhoneRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生電話記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;PhoneRecord&gt;，代表多筆學生電話記錄物件。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;PhoneRecord&gt; phonerecs = Phone.SelectByStudents(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:PhoneRecord,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("All");
            helper.AddElement("Condition");
            foreach (string each in StudentIDs)
                helper.AddElement("Condition", "ID", each);

            DSXmlHelper rsp = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent();
            foreach (XmlElement each in rsp.GetElements("Student"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 更新單筆學生電話記錄
        /// </summary>
        /// <param name="PhoneRecord">學生電話記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     PhoneRecord record = Phone.SelectByStudentID(StudentID);
        ///     record.Permanent = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Phone.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(PhoneRecord phoneRecord)
        {
            List<PhoneRecord> Params = new List<PhoneRecord>();

            Params.Add(phoneRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生電話記錄
        /// </summary>
        /// <param name="PhoneRecords">多筆學生電話記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="PhoneRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     PhoneRecord record = Phone.SelectByStudentID(StudentID);
        ///     record.Permanent = (new System.Random()).NextDouble().ToString();
        ///     List&lt;PhoneRecord&gt; records = new List&lt;PhoneRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Phone.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<PhoneRecord> PhoneRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<PhoneRecord> worker = new MultiThreadWorker<PhoneRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<PhoneRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "PermanentPhone", editor.Permanent);
                    updateHelper.AddElement("Student/Field", "SMSPhone", editor.Cell);
                    updateHelper.AddElement("Student/Field", "ContactPhone", editor.Contact);
                    updateHelper.AddElement("Student/Field","OtherPhones");
                    updateHelper.AddElement("Student/Field/OtherPhones", "PhoneList");
                    updateHelper.AddElement("Student/Field/OtherPhones/PhoneList","PhoneNumber",editor.Phone1);
                    updateHelper.AddElement("Student/Field/OtherPhones/PhoneList","PhoneNumber",editor.Phone2);
                    updateHelper.AddElement("Student/Field/OtherPhones/PhoneList","PhoneNumber",editor.Phone3);
                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);
                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<PhoneRecord>> packages = worker.Run(PhoneRecords);

            foreach (PackageWorkEventArgs<PhoneRecord> each in packages)
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
    }
}