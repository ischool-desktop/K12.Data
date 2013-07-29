using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生地址類別，提供方法用來取得及修改學生地址資訊
    /// </summary>
    public class Address
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetAddressDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生地址記錄物件列表。
        /// </summary>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;AddressRecord&gt; records = Address.SelectAll();
        ///     
        ///     foreach(AddressRecord record in records)
        ///         System.Console.WriteLine(record.Name); 
        ///     </code>
        /// </example>
        [SelectMethod("K12.Address.SelectAll", "學籍.學生地址")]
        public static List<AddressRecord> SelectAll()
        {
            return SelectAll<K12.Data.AddressRecord>();
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得地址記錄物件列表。
        /// </summary>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;AddressRecord&gt; records = Address.SelectAll();
        ///     
        ///     foreach(AddressRecord record in records)
        ///         System.Console.WriteLine(record.Name); 
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : AddressRecord, new()
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
        /// 根據單筆學生記錄物件取得學生地址記錄物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>AddressRecord，代表學生地址物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         AddressRecord record = Address.SelectByStudent(Student);
        ///         
        ///         if (record!=null)
        ///             System.Console.WriteLine(record.Permanent.ZipCode); 
        ///     </code>
        /// </example>
        /// <remarks>若是Student不存在則會傳回null</remarks>
        public static AddressRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.AddressRecord>(Student);
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生地址記錄物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>AddressRecord，代表學生地址物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     AddressRecord addressrec = Address.SelectByStudent(Student);
        /// </example>
        protected static T SelectByStudent<T>(StudentRecord Student) where T:AddressRecord,new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生地址記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>AddressRecord，代表學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         AddressRecord record = Address.SelectByStudentID(StudentID);
        ///         
        ///         if (record!=null)
        ///           System.Console.WriteLine(record.Permanent.ZipCode); 
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不存在則會傳回null</remarks>
        public static AddressRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.AddressRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生地址記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>AddressRecord，代表學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     AddressRecord addressrec = Address.SelectByStudent(StudentID);
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T:AddressRecord,new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            T addrTarget= new T();
            List<T> addrs =  SelectByStudentIDs<T>(Keys);

            if (addrs.Count > 0)
                return addrTarget = addrs[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得地址記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;AddressRecord&gt; records = Address.SelectByStudents(Students);
        ///     
        ///     foreach(AddressRecord record in records)
        ///         System.Console.WriteLine(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<AddressRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.AddressRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得地址記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AddressRecord&gt; addressrecs = Address.SelectByStudents(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T : AddressRecord, new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得地址記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;AddressRecord&gt; records = Address.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(AddressRecord record in records)
        ///         System.Console.WriteLine(record.Name); 
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆ID，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<AddressRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.AddressRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得地址記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;AddressRecord&gt;，代表多筆學生地址記錄物件。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;AddressRecord&gt; addressrecs = Address.SelectByStudents(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : AddressRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("All");
            helper.AddElement("Condition");

            if (StudentIDs!=null)
                foreach (string each in StudentIDs)
                    if (!string.IsNullOrEmpty(each))
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
        /// 更新單筆學生地址記錄
        /// </summary>
        /// <param name="AddressRecord">學生地址記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     AddressRecord record = Address.SelectByStudentID(StudentID);
        ///     record.PerPermanent.ZipCode = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Address.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(AddressRecord AddressRecord)
        {
            List<AddressRecord> Params = new List<AddressRecord>();

            Params.Add(AddressRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生地址記錄
        /// </summary>
        /// <param name="AddressRecords">多筆學生地址記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="AddressRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     AddressRecord record = Address.SelectByStudentID(StudentID);
        ///     record.PerPermanent.ZipCode = (new System.Random()).NextDouble().ToString();
        ///     List&lt;AddressRecord&gt; records = new List&lt;AddressRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Address.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<AddressRecord> AddressRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<AddressRecord> worker = new MultiThreadWorker<AddressRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<AddressRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");

                    //新增永久地址
                    updateHelper.AddElement("Student/Field", "PermanentAddress");
                    updateHelper.AddElement("Student/Field/PermanentAddress", "AddressList");
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList", "Address");

                    //if (editor.Permanent.Area!=string.Empty)
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "Area", editor.Permanent.Area);
         
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "County", editor.Permanent.County);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "DetailAddress", editor.Permanent.Detail);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "District", editor.Permanent.District);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "Latitude", editor.Permanent.Latitude);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "Longitude", editor.Permanent.Longitude);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "Town", editor.Permanent.Town);
                    updateHelper.AddElement("Student/Field/PermanentAddress/AddressList/Address", "ZipCode", editor.Permanent.ZipCode);

                    //新增郵寄地址
                    updateHelper.AddElement("Student/Field", "MailingAddress");
                    updateHelper.AddElement("Student/Field/MailingAddress", "AddressList");
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "Area", editor.Mailing.Area);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "County", editor.Mailing.County);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "DetailAddress", editor.Mailing.Detail);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "District", editor.Mailing.District);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "Latitude", editor.Mailing.Latitude);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "Longitude", editor.Mailing.Longitude);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "Town", editor.Mailing.Town);
                    updateHelper.AddElement("Student/Field/MailingAddress/AddressList/Address", "ZipCode", editor.Mailing.ZipCode);

                    updateHelper.AddElement("Student/Field", "OtherAddresses");
                    updateHelper.AddElement("Student/Field/OtherAddresses", "AddressList");

                    //新增其它地址1
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Area", editor.Address1.Area);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "County", editor.Address1.County);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "DetailAddress", editor.Address1.Detail);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "District", editor.Address1.District);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Latitude", editor.Address1.Latitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Longitude", editor.Address1.Longitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Town", editor.Address1.Town);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "ZipCode", editor.Address1.ZipCode);

                    //新增其它地址2
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Area", editor.Address2.Area);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "County", editor.Address2.County);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "DetailAddress", editor.Address2.Detail);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "District", editor.Address2.District);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Latitude", editor.Address2.Latitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Longitude", editor.Address2.Longitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Town", editor.Address2.Town);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "ZipCode", editor.Address2.ZipCode);

                    //新增其它地址3
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Area", editor.Address3.Area);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "County", editor.Address3.County);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "DetailAddress", editor.Address3.Detail);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "District", editor.Address3.District);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Latitude", editor.Address3.Latitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Longitude", editor.Address3.Longitude);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "Town", editor.Address3.Town);
                    updateHelper.AddElement("Student/Field/OtherAddresses/AddressList/Address", "ZipCode", editor.Address3.ZipCode);


                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);

                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<AddressRecord>> packages = worker.Run(AddressRecords);

            foreach (PackageWorkEventArgs<AddressRecord> each in packages)
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