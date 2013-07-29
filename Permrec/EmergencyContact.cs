using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using FISCA.Data;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 緊急連絡人類別，提供方法用來取得及修改緊急連絡人資訊
    /// </summary>
    public class EmergencyContact
    {
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        [SelectMethod("K12.Parent.SelectAll", "學籍.學生家長及監護人")]
        public static List<EmergencyContactRecord> SelectAll()
        {
            return SelectAll<K12.Data.EmergencyContactRecord>();
        }

        /// <summary>
        /// 取得所有學生家長及監護人記錄物件列表。
        /// </summary>
        /// <typeparam name="T">家長及監護人記錄物件及其後代。</typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : EmergencyContactRecord, new()
        {
            List<T> Types = new List<T>();

            QueryHelper helper = new QueryHelper();

            DataTable Table = helper.Select("select id,contact_name,contact_nationality,contact_relationship,contact_other_info from student");

            foreach (DataRow Row in Table.Rows)
            {
                T Type = new T();
                Type.Load(Row);
                Types.Add(Type);
            }

            return Types;
        }

        public static EmergencyContactRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.EmergencyContactRecord>(Student);
        }

        protected static T SelectByStudent<T>(StudentRecord Student) where T : EmergencyContactRecord, new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        public static EmergencyContactRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.EmergencyContactRecord>(StudentID);
        }

        protected static T SelectByStudentID<T>(string StudentID) where T : EmergencyContactRecord, new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            return SelectByStudentIDs<T>(Keys)[0];
        }

        public static List<EmergencyContactRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.EmergencyContactRecord>(Students);
        }

        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T : EmergencyContactRecord, new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        public static List<EmergencyContactRecord> SelectByStudentIDs(List<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.EmergencyContactRecord>(StudentIDs);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : EmergencyContactRecord, new()
        {
            List<T> Types = new List<T>();

            QueryHelper helper = new QueryHelper();

            DataTable Table = helper.Select("select id,contact_name,contact_nationality,contact_relationship,contact_other_info from student where id in (" + string.Join(",",StudentIDs.ToArray()) +")");

            foreach (DataRow Row in Table.Rows)
            {
                T Type = new T();
                Type.Load(Row);
                Types.Add(Type);
            }

            return Types;
        }

        public static int Update(EmergencyContactRecord ParentRecord)
        {
            List<EmergencyContactRecord> Params = new List<EmergencyContactRecord>();

            Params.Add(ParentRecord);

            return Update(Params);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<EmergencyContactRecord> Records)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<EmergencyContactRecord> worker = new MultiThreadWorker<EmergencyContactRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<EmergencyContactRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "ContactName", editor.Name);
                    updateHelper.AddElement("Student/Field", "ContactNationality", editor.Nationality);
                    updateHelper.AddElement("Student/Field", "ContactRelationship", editor.Relationship);
                    updateHelper.AddElement("Student/Field", "ContactOtherInfo");
                    updateHelper.AddElement("Student/Field/ContactOtherInfo", "ContactOtherInfo");
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo","Hospital",editor.Hospital);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo", "EducationDegree", editor.EducationDegree);

                    //父親公司
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo", "Company");
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/Company", "Name", editor.CompanyName);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/Company", "Title", editor.CompanyTitle);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/Company", "Category", editor.CompanyCategory);

                    //父親電話
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo", "PhoneList");
                    XmlElement FatherCellElement = updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/PhoneList", "Phone");
                    FatherCellElement.SetAttribute("Title", "手機");
                    FatherCellElement.InnerText = editor.CellPhone;

                    XmlElement FatherCompanyElement = updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/PhoneList", "Phone");
                    FatherCompanyElement.SetAttribute("Title", "辦公室電話");
                    FatherCompanyElement.InnerText = editor.CompanyPhone;

                    XmlElement FatherHomeElement = updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/PhoneList", "Phone");
                    FatherHomeElement.SetAttribute("Title", "住家電話");
                    FatherHomeElement.InnerText = editor.HomePhone;

                    //父親地址
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo", "AddressList");
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList", "Address");
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "County", editor.AddressCounty);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "Town", editor.AddressTown);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "Detail", editor.AddressDetail);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "District", editor.AddressDistrict);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "Area", editor.AddressArea);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "ZipCode", editor.AddressZipCode);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "Longitude", editor.AddressLongitude);
                    updateHelper.AddElement("Student/Field/ContactOtherInfo/ContactOtherInfo/AddressList/Address", "Latitude", editor.AddressLatitude);

                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);

                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<EmergencyContactRecord>> packages = worker.Run(Records);

            foreach (PackageWorkEventArgs<EmergencyContactRecord> each in packages)
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