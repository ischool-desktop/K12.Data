using System.Xml;
using System.Data;

namespace K12.Data
{
    /// <summary>
    /// 緊急連絡人記錄物件
    /// </summary>
    public class EmergencyContactRecord
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public EmergencyContactRecord()
        {
 
        }

        /// <summary>
        /// DataRow參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public EmergencyContactRecord(DataRow data)
        {
            Load(data);
        }

        /// <summary>
        /// 從DataRow載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(DataRow Row)
        {
            RefStudentID = "" + Row["id"];
            Name = "" + Row["contact_name"];
            Nationality = "" + Row["contact_nationality"];
            Relationship = "" + Row["contact_relationship"];

            string OtherInfo = "" + Row["contact_other_info"];

            if (!string.IsNullOrEmpty(OtherInfo))
            {
                XmlDocument Document = new XmlDocument();
                Document.LoadXml(OtherInfo);

                XmlHelper xdata = new XmlHelper(Document.DocumentElement);

                //最高學歷
                EducationDegree = xdata.GetString("EducationDegree");

                //公司資訊
                CompanyName = xdata.GetString("Company/Name");
                CompanyTitle = xdata.GetString("Company/Title");
                CompanyCategory = xdata.GetString("Company/Category");

                //電話資訊
                CellPhone = xdata.GetString("PhoneList/Phone[@Title='手機']");
                HomePhone = xdata.GetString("PhoneList/Phone[@Title='住家電話']");
                CompanyPhone = xdata.GetString("PhoneList/Phone[@Title='辦公室電話']");
                Hospital = xdata.GetString("Hospital");

                //地址資訊
                AddressZipCode = xdata.GetString("AddressList/Address/ZipCode");
                AddressCounty = xdata.GetString("AddressList/Address/County");
                AddressDistrict = xdata.GetString("AddressList/Address/District");
                AddressArea = xdata.GetString("AddressList/Address/Area");
                AddressTown = xdata.GetString("AddressList/Address/Town");
                AddressDetail = xdata.GetString("AddressList/Address/Detail");
                AddressLongitude = xdata.GetString("AddressList/Address/Longitude");
                AddressLatitude = xdata.GetString("AddressList/Address/Latitude");
            }
        }

        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get;  set; }

        /// <summary>
        /// 所屬學生記錄物件
        /// </summary>
        public StudentRecord Student 
        { 
            get 
            {
                return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null;
            } 
        }

        /// <summary>
        /// 緊急連絡人姓名
        /// </summary>
        [Field(Caption = "姓名", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string Name { get; set; }

        /// <summary>
        /// 緊急連絡人關係
        /// </summary>
        [Field(Caption = "關係", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string Relationship { get; set; }

        /// <summary>
        /// 緊急連絡人國籍
        /// </summary>
        [Field(Caption = "國籍", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string Nationality { get; set; }

        /// <summary>
        /// 緊急連絡人最高學歷
        /// </summary>
        [Field(Caption = "最高學歷", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string EducationDegree { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        [Field(Caption = "公司名稱", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string CompanyName { get; set;}

        /// <summary>
        /// 公司職稱
        /// </summary>
        [Field(Caption = "公司職稱", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string CompanyTitle { get; set;}

        /// <summary>
        /// 公司分類
        /// </summary>
        [Field(Caption = "公司分類", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string CompanyCategory { get; set;}

        /// <summary>
        /// 緊急指定就醫醫院
        /// </summary>
        [Field(Caption = "指定就醫醫院", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string Hospital { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        [Field(Caption = "手機", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 家中電話
        /// </summary>
        [Field(Caption = "家中電話", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string HomePhone { get; set; }

        /// <summary>
        /// 辦公室膧話
        /// </summary>
        [Field(Caption = "辦公室電話", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string CompanyPhone { get; set; }

        /// <summary>
        /// 地址郵遞區號
        /// </summary>
        [Field(Caption = "地址郵遞區號", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressZipCode { get; set; }

        /// <summary>
        /// 地址縣市
        /// </summary>
        [Field(Caption = "地址縣市", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressCounty { get; set; }

        /// <summary>
        /// 地址鄉鎮市區
        /// </summary>
        [Field(Caption = "地址鄉鎮市區", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressTown { get; set; }

        /// <summary>
        /// 地址村里
        /// </summary>
        [Field(Caption = "地址村里", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressDistrict { get; set; }

        /// <summary>
        /// 地址鄰
        /// </summary>
        [Field(Caption = "地址鄰", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressArea { get; set; }

        /// <summary>
        /// 地址其他
        /// </summary>
        [Field(Caption = "地址其他", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressDetail { get; set; }

        /// <summary>
        /// 地址經度
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressLongitude { get; set; }

        /// <summary>
        /// 地址緯度
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "EmergencyContact", EntityCaption = "緊急連絡人")]
        public string AddressLatitude { get; set; }
    }
}