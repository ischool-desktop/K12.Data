using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生家長及監護人類別，提供方法用來取得及修改學生家長及監護人資訊
    /// </summary>
    public class Parent
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetParentsDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 取得所有學生家長及監護人記錄物件列表。
        /// </summary>
        /// <returns>List&lt;ParentRecord&gt;，代表多筆學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ParentRecord&gt; records = Parent.SelectAll();
        ///     
        ///     foreach(ParentRecord record in records)
        ///         Console.WrlteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        [SelectMethod("K12.Parent.SelectAll", "學籍.學生家長及監護人")]
        public static List<ParentRecord> SelectAll()
        {
            return SelectAll<K12.Data.ParentRecord>();
        }

        /// <summary>
        /// 取得所有學生家長及監護人記錄物件列表。
        /// </summary>
        /// <typeparam name="T">家長及監護人記錄物件及其後代。</typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : ParentRecord, new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
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
        /// 根據單筆學生記錄物件取得學生家長及監護人記錄物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>ParentRecord，代表學生家長及監人記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     PaerntRecord record = Parent.SelectByStudent(Student);
        ///     
        ///    if (record != null)
        ///        System.Console.WriteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是Student不則在則會傳回null</remarks>
        public static ParentRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.ParentRecord>(Student);
        }

        /// <summary>
        /// 根據單筆學生記錄物件取得學生家長及監護人記錄物件。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>ParentRecord，代表學生家長及監人記錄物件。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ParentRecord record = Parent.SelectByStudent(StudentID);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        protected static T SelectByStudent<T>(StudentRecord Student) where T : ParentRecord, new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生家長及監護人記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>ParentRecord，代表學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ParentRecord record = Parent.SelectByStudent(StudentID);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        public static ParentRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.ParentRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生記錄編號取得學生家長及監護人記錄物件。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>ParentRecord，代表學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     ParentRecord parentrec = Parent.SelectByStudent(StudentID);
        /// </example>
        protected static T SelectByStudentID<T>(string StudentID) where T:ParentRecord ,new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            return SelectByStudentIDs<T>(Keys)[0];
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生家長及監護人記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;ParentRecord&gt;，代表多筆學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ParentRecord&gt; records = Parent.SelectByStudents(Students);
        ///     
        ///     foreach(ParentRecord record in records)
        ///         Console.WrlteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<ParentRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.ParentRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生記錄物件取得學生家長及監護人記錄物件列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;ParentRecord&gt;，代表多筆學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <seealso cref="StudentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ParentRecord&gt; parentrecs = Parent.SelectByStudents(Students);
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T:ParentRecord ,new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生家長及監護人記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;ParentRecord&gt;，代表多筆學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;ParentRecord&gt; records = Parent.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(ParentRecord record in records)
        ///         Console.WrlteLine(record.Mother.Name);
        ///     </code>
        /// </example>
        /// <remarks>可能情況若是傳5筆學生，但是其中1筆沒有資料，就只會回傳4筆資料</remarks>
        public static List<ParentRecord> SelectByStudentIDs(List<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.ParentRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生記錄編號取得學生家長及監護人記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;ParentRecord&gt;，代表多筆學生家長及監護人記錄物件。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ParentRecord&gt; parentrecs = Parent.SelectByStudents(StudentIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:ParentRecord ,new()
        {
            List<T> Types = new List<T>();

            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Field");
            helper.AddElement("Field", "All");
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
        /// 取得關係列表，如父子、母子…等。
        /// </summary>
        /// <returns>List&lt;KeyValuePair&lt;string, string&gt;&gt;，多筆關係列表物件。</returns>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static List<KeyValuePair<string, string>> GetRelationship()
        {
            List<KeyValuePair<string, string>> List = new List<KeyValuePair<string, string>>();


            DSRequest request = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetRelationshipListRequest");
            helper.AddElement("Fields");
            helper.AddElement("Fields", "All");
            request.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService("SmartSchool.Config.GetRelationshipList", request);

            DSXmlHelper relhelper = dsrsp.GetContent();

            KeyValuePair<string, string> kvpRel = new KeyValuePair<string, string>("", "請選擇");

            List.Add(kvpRel);

            foreach (XmlNode node in relhelper.GetElements("Relationship"))
                List.Add(new KeyValuePair<string, string>(node.InnerText, node.InnerText));

            return List;
        }

        /// <summary>
        /// 取得國籍列表。
        /// </summary>
        /// <returns>List&lt;string&gt;，代表國籍列表。</returns>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static List<string> GetNationalityList()
        {
            List<string> List = new List<string>();

            DSRequest request = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetNationalityListRequest");
            helper.AddElement("Fields");
            helper.AddElement("Fields", "All");
            request.SetContent(helper);
            DSXmlHelper response = DSAServices.CallService("SmartSchool.Config.GetNationalityList", request).GetContent();

            foreach (XmlElement Element in response.GetElements("Nationality"))
                List.Add(Element.InnerText);

            return List;
        }

        /// <summary>
        /// 更新單筆家長及監護人記錄
        /// </summary>
        /// <param name="ParentRecord">家長及監護人記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ParentRecord record = Parent.SelectByStudentID(StudentID);
        ///     record.Mother.Name = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = Parent.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(ParentRecord ParentRecord)
        {
            List<ParentRecord> Params = new List<ParentRecord>();

            Params.Add(ParentRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆家長及監護人記錄
        /// </summary>
        /// <param name="ParentRecords">多筆家長及監護人記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ParentRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     ParentRecord record = Parent.SelectByStudentID(StudentID);
        ///     record.Mother.Name = (new System.Random()).NextDouble().ToString();
        ///     List&lt;ParentRecord&gt; records = new List&lt;ParentRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Parent.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ParentRecord> ParentRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ParentRecord> worker = new MultiThreadWorker<ParentRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ParentRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");

                    //父親資訊
                    updateHelper.AddElement("Student/Field", "FatherName", editor.Father.Name);
                    updateHelper.AddElement("Student/Field", "FatherIDNumber", editor.Father.IDNumber);
                    updateHelper.AddElement("Student/Field", "FatherNationality", editor.Father.Nationality);
                    updateHelper.AddElement("Student/Field", "FatherLiving", editor.Father.Living);
                    updateHelper.AddElement("Student/Field","FatherOtherInfo");
                    updateHelper.AddElement("Student/Field/FatherOtherInfo", "FatherOtherInfo");
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "FatherEducationDegree", editor.Father.EducationDegree);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "FatherJob", editor.Father.Job);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "Phone", editor.Father.Phone);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "EMail", editor.Father.EMail);

                    //父親公司
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "Company");
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/Company", "Name", editor.Father.CompanyName);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/Company", "Title", editor.Father.CompanyTitle);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/Company", "Category", editor.Father.CompanyCategory);

                    //父親電話
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "PhoneList");
                    XmlElement FatherCellElement = updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/PhoneList", "Phone");
                    FatherCellElement.SetAttribute("Title","手機");
                    FatherCellElement.InnerText = editor.Father.CellPhone;

                    XmlElement FatherCompanyElement = updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/PhoneList", "Phone");
                    FatherCompanyElement.SetAttribute("Title", "辦公室電話");
                    FatherCompanyElement.InnerText = editor.Father.CompanyPhone;

                    XmlElement FatherHomeElement = updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/PhoneList", "Phone");
                    FatherHomeElement.SetAttribute("Title", "住家電話");
                    FatherHomeElement.InnerText = editor.Father.HomePhone;

                    //父親地址
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo", "AddressList");
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "County", editor.Father.AddressCounty);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "Town", editor.Father.AddressTown);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "Detail", editor.Father.AddressDetail);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "District", editor.Father.AddressDistrict);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "Area", editor.Father.AddressArea);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "ZipCode", editor.Father.AddressZipCode);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "Longitude", editor.Father.AddressLongitude);
                    updateHelper.AddElement("Student/Field/FatherOtherInfo/FatherOtherInfo/AddressList/Address", "Latitude", editor.Father.AddressLatitude);                

                    //母親資訊
                    updateHelper.AddElement("Student/Field", "MotherName", editor.Mother.Name);
                    updateHelper.AddElement("Student/Field", "MotherIDNumber", editor.Mother.IDNumber);
                    updateHelper.AddElement("Student/Field", "MotherNationality", editor.Mother.Nationality);
                    updateHelper.AddElement("Student/Field", "MotherLiving", editor.Mother.Living);
                    updateHelper.AddElement("Student/Field", "MotherOtherInfo");
                    updateHelper.AddElement("Student/Field/MotherOtherInfo", "MotherOtherInfo");
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "MotherEducationDegree", editor.Mother.EducationDegree);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "MotherJob", editor.Mother.Job);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "Phone", editor.Mother.Phone);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "EMail", editor.Mother.EMail);

                    //母親公司
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "Company");
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/Company", "Name", editor.Mother.CompanyName);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/Company", "Title", editor.Mother.CompanyTitle);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/Company", "Category", editor.Mother.CompanyCategory);

                    //母親電話
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "PhoneList");
                    XmlElement MotherCellElement = updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/PhoneList", "Phone");
                    MotherCellElement.SetAttribute("Title", "手機");
                    MotherCellElement.InnerText = editor.Mother.CellPhone;

                    XmlElement MotherCompanyElement = updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/PhoneList", "Phone");
                    MotherCompanyElement.SetAttribute("Title", "辦公室電話");
                    MotherCompanyElement.InnerText = editor.Mother.CompanyPhone;

                    XmlElement MotherHomeElement = updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/PhoneList", "Phone");
                    MotherHomeElement.SetAttribute("Title", "住家電話");
                    MotherHomeElement.InnerText = editor.Mother.HomePhone;

                    //母親地址
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo", "AddressList");
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "County", editor.Mother.AddressCounty);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "Town", editor.Mother.AddressTown);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "Detail", editor.Mother.AddressDetail);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "District", editor.Mother.AddressDistrict);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "Area", editor.Mother.AddressArea);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "ZipCode", editor.Mother.AddressZipCode);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "Longitude", editor.Mother.AddressLongitude);
                    updateHelper.AddElement("Student/Field/MotherOtherInfo/MotherOtherInfo/AddressList/Address", "Latitude", editor.Mother.AddressLatitude);

                    //監護人資訊
                    updateHelper.AddElement("Student/Field", "CustodianName", editor.Custodian.Name);
                    updateHelper.AddElement("Student/Field", "CustodianIDNumber", editor.Custodian.IDNumber);
                    updateHelper.AddElement("Student/Field", "CustodianNationality", editor.Custodian.Nationality);
                    updateHelper.AddElement("Student/Field", "CustodianRelationship", editor.Custodian.Relationship);
                    updateHelper.AddElement("Student/Field", "CustodianOtherInfo");
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo", "CustodianOtherInfo");
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "EducationDegree", editor.Custodian.EducationDegree);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "Job", editor.Custodian.Job);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "Phone", editor.Custodian.Phone);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "EMail", editor.Custodian.EMail);

                    //監護人公司
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "Company");
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/Company", "Name", editor.Custodian.CompanyName);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/Company", "Title", editor.Custodian.CompanyTitle);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/Company", "Category", editor.Custodian.CompanyCategory);

                    //監護人電話
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "PhoneList");
                    XmlElement CustodianCellElement = updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/PhoneList", "Phone");
                    CustodianCellElement.SetAttribute("Title", "手機");
                    CustodianCellElement.InnerText = editor.Custodian.CellPhone;

                    XmlElement CustodianCompanyElement = updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/PhoneList", "Phone");
                    CustodianCompanyElement.SetAttribute("Title", "辦公室電話");
                    CustodianCompanyElement.InnerText = editor.Custodian.CompanyPhone;

                    XmlElement CustodianHomeElement = updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/PhoneList", "Phone");

                    CustodianHomeElement.SetAttribute("Title", "住家電話");
                    CustodianHomeElement.InnerText = editor.Custodian.HomePhone;

                    //監護人地址
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo", "AddressList");
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "County", editor.Custodian.AddressCounty);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "Town", editor.Custodian.AddressTown);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "Detail", editor.Custodian.AddressDetail);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "District", editor.Custodian.AddressDistrict);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "Area", editor.Custodian.AddressArea);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "ZipCode", editor.Custodian.AddressZipCode);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "Longitude", editor.Custodian.AddressLongitude);
                    updateHelper.AddElement("Student/Field/CustodianOtherInfo/CustodianOtherInfo/AddressList/Address", "Latitude", editor.Custodian.AddressLatitude);

                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);

                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ParentRecord>> packages = worker.Run(ParentRecords);

            foreach (PackageWorkEventArgs<ParentRecord> each in packages)
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