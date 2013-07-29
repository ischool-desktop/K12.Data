using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 家長及監護人資訊
    /// </summary>
    public class ParentRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public ParentRecord()
        {
            Father = new Father();
            Mother = new Mother();
            Custodian = new Custodian();
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public ParentRecord(XmlElement data)
        {
            Load(data);
        }
        
        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            Father = new Father(data);
            Mother = new Mother(data);
            Custodian = new Custodian(data);
            RefStudentID = data.GetAttribute("RefStudentID");
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
        /// 父親資訊
        /// </summary>
        public Father Father { get;  set; }

        /// <summary>
        /// 父親最高學歷，此為唯讀屬性，要修改請使用Father.EducationDegree屬性。
        /// </summary>
        [Field(Caption = "父親最高學歷", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherEducationDegree { get { return Father.EducationDegree; } }

        /// <summary>
        /// 父親身份證字號，此為唯讀屬性，要修改請使用Father.IDNumber屬性。
        /// </summary>
        [Field(Caption = "父親身份證字號", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherIDNumber { get { return Father.IDNumber; } }

        /// <summary>
        /// 父親工作，此為唯讀屬性，要修改請使用Father.Job屬性。
        /// </summary>
        [Field(Caption = "父親工作", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherJob { get { return Father.Job; } }

        /// <summary>
        /// 父親存歿，此為唯讀屬性，要修改請使用Father.Living屬性。
        /// </summary>
        [Field(Caption = "父親存歿", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherLiving { get { return Father.Living; } }

        /// <summary>
        /// 父親姓名，此為唯讀屬性，要修改請使用Father.Name屬性。
        /// </summary>
        [Field(Caption = "父親姓名", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherName { get { return Father.Name; } }

        /// <summary>
        /// 父親國籍，此為唯讀屬性，要修改請使用Father.Nationality屬性。
        /// </summary>
        [Field(Caption = "父親國籍", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherNationality { get { return Father.Nationality; } }

        /// <summary>
        /// 父親電話，此為唯讀屬性，要修改請使用Father.Phone屬性。
        /// </summary>
        [Field(Caption = "父親電話", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherPhone { get { return Father.Phone; } }

        /// <summary>
        /// 父親電子郵件，此為唯讀屬性，要修改請使用Father.EMail屬性。
        /// </summary>
        [Field(Caption = "父親電子郵件", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string FatherEMail { get { return Father.EMail; } }

        /// <summary>
        /// 母親資訊
        /// </summary>
        public Mother Mother { get;  set; }

        /// <summary>
        /// 母親最高學歷，此為唯讀屬性，要修改請使用Mother.EducationDegree屬性。
        /// </summary>
        [Field(Caption = "母親電話", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherEducationDegree { get { return Mother.EducationDegree; } }

        /// <summary>
        /// 母親身份證字號，此為唯讀屬性，要修改請使用Mother.IDNumber屬性。
        /// </summary>
        [Field(Caption = "母親身份證字號", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherIDNumber { get { return Mother.IDNumber; } }

        /// <summary>
        /// 母親工作，此為唯讀屬性，要修改請使用Mother.Job屬性。
        /// </summary>
        [Field(Caption = "母親工作", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherJob { get { return Mother.Job; } }

        /// <summary>
        /// 母親存歿，此為唯讀屬性，要修改請使用Mother.Living屬性。
        /// </summary>
        [Field(Caption = "母親存歿", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherLiving { get { return Mother.Living; } }

        /// <summary>
        /// 母親姓名，此為唯讀屬性，要修改請使用Mother.Name屬性。
        /// </summary>
        [Field(Caption = "母親姓名", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherName { get { return Mother.Name; } }

        /// <summary>
        /// 母親國籍，此為唯讀屬性，要修改請使用Mother.Nationality屬性。
        /// </summary>
        [Field(Caption = "母親國籍", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherNationality { get { return Mother.Nationality; } }

        /// <summary>
        /// 母親電話，此為唯讀屬性，要修改請使用Mother.Phone屬性。
        /// </summary>
        [Field(Caption = "母親電話", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherPhone { get { return Mother.Phone; } }

        /// <summary>
        /// 母親電子郵件，此為唯讀屬性，要修改請使用Mother.EMail屬性。
        /// </summary>
        [Field(Caption = "母親電子郵件", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string MotherEMail { get { return Mother.EMail; } }

        /// <summary> 
        /// 監護人資訊
        /// </summary>
        public Custodian Custodian { get;  set; }

        /// <summary>
        /// 監護人最高學歷，此為唯讀屬性，要修改請使用Custodian.EducationDegree屬性。
        /// </summary>
        [Field(Caption = "監護人最高學歷", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianEducationDegree { get { return Custodian.EducationDegree; } }

        /// <summary>
        /// 監護人身份證字號，此為唯讀屬性，要修改請使用Custodian.IDNumber屬性。
        /// </summary>
        [Field(Caption = "監護人身份證字號", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianIDNumber { get { return Custodian.IDNumber; } }

        /// <summary>
        /// 監護人工作，此為唯讀屬性，要修改請使用Custodian.Job屬性。
        /// </summary>
        [Field(Caption = "監護人工作", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianJob { get { return Custodian.Job; } }

        /// <summary>
        /// 監護人稱謂，此為唯讀屬性，要修改請使用Custodian.Relationship屬性。
        /// </summary>
        [Field(Caption = "監護人稱謂", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianRelationship { get { return Custodian.Relationship; } }

        /// <summary>
        /// 監護人姓名，此為唯讀屬性，要修改請使用Custodian.Name屬性。
        /// </summary>
        [Field(Caption = "監護人姓名", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianName { get { return Custodian.Name; } }

        /// <summary>
        /// 監護人國籍，此為唯讀屬性，要修改請使用Custodian.Nationality屬性。
        /// </summary>
        [Field(Caption = "監護人國籍", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianNationality { get { return Custodian.Nationality; } }

        /// <summary>
        /// 監護人電話，此為唯讀屬性，要修改請使用Custodian.Phone屬性。
        /// </summary>
        [Field(Caption = "監護人電話", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianPhone { get { return Custodian.Phone; } }

        /// <summary>
        /// 監護人電子郵件，此為唯讀屬性，要修改請使用Custodian.EMail屬性。
        /// </summary>
        [Field(Caption = "監護人電子郵件", EntityName = "Parent", EntityCaption = "家長及監護人")]
        public string CustodianEMail { get { return Custodian.EMail; } }
    }

    /// <summary>
    /// 父親、母親及監護人所共同繼承的類別
    /// </summary>
    public abstract class ParentBase
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 工作
        /// </summary>
        public string Job { get;  set; }

        /// <summary>
        /// 最高學歷
        /// </summary>
        public string EducationDegree { get;  set; }

        /// <summary>
        /// 國籍
        /// </summary>
        public string Nationality { get;  set; }

        /// <summary>
        /// 身分證號
        /// </summary>
        public string IDNumber { get;  set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get;  set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司職稱
        /// </summary>
        public string CompanyTitle { get; set; }

        /// <summary>
        /// 公司分類，與Job欄位意義相同，但若要仔細使用Company相關欄位
        /// </summary>
        public string CompanyCategory { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 住家電話
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// 辦公室電話
        /// </summary>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// 地址郵遞區號
        /// </summary>
        public string AddressZipCode { get; set; }

        /// <summary>
        /// 地址縣市
        /// </summary>
        public string AddressCounty { get; set; }

        /// <summary>
        /// 地址鄉鎮市區
        /// </summary>
        public string AddressTown { get; set; }

        /// <summary>
        /// 地址村里
        /// </summary>
        public string AddressDistrict { get; set; }

        /// <summary>
        /// 地址鄰
        /// </summary>
        public string AddressArea { get; set; }

        /// <summary>
        /// 地址其他
        /// </summary>
        public string AddressDetail { get; set; }

        /// <summary>
        /// 地址經度
        /// </summary>
        public string AddressLongitude { get; set; }

        /// <summary>
        /// 地址緯度
        /// </summary>
        public string AddressLatitude { get; set; }
    }

    /// <summary>
    /// 父親資訊
    /// </summary>
    public class Father : ParentBase
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        internal Father()
        {
 
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="data"></param>
        internal Father(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 依XML載入設定值
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            XmlHelper xdata = new XmlHelper(data);
            Name = xdata.GetString("FatherName");
            Nationality = xdata.GetString("FatherNationality");
            IDNumber = xdata.GetString("FatherIDNumber");
            Living = xdata.GetString("FatherLiving");
            EducationDegree = xdata.GetString("FatherOtherInfo/FatherEducationDegree");
            Job = xdata.GetString("FatherOtherInfo/FatherJob");
            Phone = xdata.GetString("FatherOtherInfo/Phone");
            EMail = xdata.GetString("FatherOtherInfo/EMail");

            CompanyName = xdata.GetString("FatherOtherInfo/Company/Name");
            CompanyTitle = xdata.GetString("FatherOtherInfo/Company/Title");
            CompanyCategory = xdata.GetString("FatherOtherInfo/Company/Category");

            CellPhone = xdata.GetString("FatherOtherInfo/PhoneList/Phone[@Title='手機']");
            HomePhone = xdata.GetString("FatherOtherInfo/PhoneList/Phone[@Title='住家電話']");
            CompanyPhone = xdata.GetString("FatherOtherInfo/PhoneList/Phone[@Title='辦公室電話']");

            AddressZipCode = xdata.GetString("FatherOtherInfo/AddressList/Address/ZipCode");
            AddressCounty = xdata.GetString("FatherOtherInfo/AddressList/Address/County");
            AddressTown = xdata.GetString("FatherOtherInfo/AddressList/Address/Town");
            AddressDistrict = xdata.GetString("FatherOtherInfo/AddressList/Address/District");
            AddressArea = xdata.GetString("FatherOtherInfo/AddressList/Address/Area");
            AddressDetail = xdata.GetString("FatherOtherInfo/AddressList/Address/Detail");
            AddressLongitude = xdata.GetString("FatherOtherInfo/AddressList/Address/Longitude");
            AddressLatitude = xdata.GetString("FatherOtherInfo/AddressList/Address/Latitude");
        }

        /// <summary>
        /// 輸出成XML
        /// </summary>
        /// <returns></returns>
        public XmlElement ToXml()
        {
            XmlDataDocument xmldoc = new XmlDataDocument();

            xmldoc.Load("");

            return null;
        }

        /// <summary>
        /// 存歿
        /// </summary>
        public string Living { get;  set; }
    }

    /// <summary>
    /// 母親資訊
    /// </summary>
    public class Mother : ParentBase
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        internal Mother()
        {
 
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="data"></param>
        internal Mother(XmlElement data)
        {
            XmlHelper xdata = new XmlHelper(data);
            Name = xdata.GetString("MotherName");
            Nationality = xdata.GetString("MotherNationality");
            IDNumber = xdata.GetString("MotherIDNumber");
            Living = xdata.GetString("MotherLiving");
            EducationDegree = xdata.GetString("MotherOtherInfo/MotherEducationDegree");
            Job = xdata.GetString("MotherOtherInfo/MotherJob");
            Phone = xdata.GetString("MotherOtherInfo/Phone");
            EMail = xdata.GetString("MotherOtherInfo/EMail");

            CompanyName = xdata.GetString("MotherOtherInfo/Company/Name");
            CompanyTitle = xdata.GetString("MotherOtherInfo/Company/Title");
            CompanyCategory = xdata.GetString("MotherOtherInfo/Company/Category");

            CellPhone = xdata.GetString("MotherOtherInfo/PhoneList/Phone[@Title='手機']");
            HomePhone = xdata.GetString("MotherOtherInfo/PhoneList/Phone[@Title='住家電話']");
            CompanyPhone = xdata.GetString("MotherOtherInfo/PhoneList/Phone[@Title='辦公室電話']");

            AddressZipCode = xdata.GetString("MotherOtherInfo/AddressList/Address/ZipCode");
            AddressCounty = xdata.GetString("MotherOtherInfo/AddressList/Address/County");
            AddressTown = xdata.GetString("MotherOtherInfo/AddressList/Address/Town");
            AddressArea = xdata.GetString("MotherOtherInfo/AddressList/Address/Area");
            AddressDistrict = xdata.GetString("MotherOtherInfo/AddressList/Address/District");
            AddressDetail = xdata.GetString("MotherOtherInfo/AddressList/Address/Detail");
            AddressLongitude = xdata.GetString("MotherOtherInfo/AddressList/Address/Longitude");
            AddressLatitude = xdata.GetString("MotherOtherInfo/AddressList/Address/Latitude");
        }

        /// <summary>
        /// 存歿，型態需要改為boolean
        /// </summary>
        public string Living { get;  set; }
    }

    /// <summary>
    /// 監護人資訊
    /// </summary>
    public class Custodian : ParentBase
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        internal Custodian()
        {
 
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="data"></param>
        internal Custodian(XmlElement data)
        {
            XmlHelper xdata = new XmlHelper(data);
            Name = xdata.GetString("CustodianName");
            Nationality = xdata.GetString("CustodianNationality");
            IDNumber = xdata.GetString("CustodianIDNumber");
            Relationship = xdata.GetString("CustodianRelationship");
            EducationDegree = xdata.GetString("CustodianOtherInfo/EducationDegree");            
            Job = xdata.GetString("CustodianOtherInfo/Job");
            Phone = xdata.GetString("CustodianOtherInfo/Phone");
            EMail = xdata.GetString("CustodianOtherInfo/EMail");

            CompanyName = xdata.GetString("CustodianOtherInfo/Company/Name");
            CompanyTitle = xdata.GetString("CustodianOtherInfo/Company/Title");
            CompanyCategory = xdata.GetString("CustodianOtherInfo/Company/Category");

            CellPhone = xdata.GetString("CustodianOtherInfo/PhoneList/Phone[@Title='手機']");
            HomePhone = xdata.GetString("CustodianOtherInfo/PhoneList/Phone[@Title='住家電話']");
            CompanyPhone = xdata.GetString("CustodianOtherInfo/PhoneList/Phone[@Title='辦公室電話']");

            AddressZipCode = xdata.GetString("CustodianOtherInfo/AddressList/Address/ZipCode");
            AddressCounty = xdata.GetString("CustodianOtherInfo/AddressList/Address/County");
            AddressTown = xdata.GetString("CustodianOtherInfo/AddressList/Address/Town");
            AddressArea = xdata.GetString("CustodianOtherInfo/AddressList/Address/Area");
            AddressDistrict = xdata.GetString("CustodianOtherInfo/AddressList/Address/District");
            AddressDetail = xdata.GetString("CustodianOtherInfo/AddressList/Address/Detail");
            AddressLongitude = xdata.GetString("CustodianOtherInfo/AddressList/Address/Longitude");
            AddressLatitude = xdata.GetString("CustodianOtherInfo/AddressList/Address/Latitude");
        }

        /// <summary>
        /// 稱謂
        /// </summary>
        public string Relationship { get;  set; }
    }
}