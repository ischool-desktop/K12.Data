using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生地址資訊
    /// </summary>
    public class AddressRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public AddressRecord()
        {
            Permanent = new AddressItem(null);
            Mailing = new AddressItem(null);
            Address1 = new AddressItem(null);
            Address2 = new AddressItem(null);
            Address3 = new AddressItem(null);
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public AddressRecord(XmlElement data)
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
            RefStudentID = data.GetAttribute("RefStudentID");

            Permanent = new AddressItem(data.SelectSingleNode("Permanent/Address") as XmlElement);
            Mailing = new AddressItem(data.SelectSingleNode("Mailing/Address") as XmlElement);

            Address1 = new AddressItem(null);
            Address2 = new AddressItem(null);
            Address3 = new AddressItem(null);

            int index = 0;
            foreach (XmlElement each in data.SelectNodes("Addresses/AddressList/Address"))
            {
                if (index == 0)
                    Address1 = new AddressItem(each);

                if (index == 1)
                    Address2 = new AddressItem(each);

                if (index == 2)
                    Address3 = new AddressItem(each);

                index++;
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
        /// 戶籍地址
        /// </summary>
        public AddressItem Permanent { get; set; }

        /// <summary>
        /// 戶籍郵遞區號，此為唯讀屬性，要修改請使用Permanent.ZipCode屬性。
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentZipCode { get { return Permanent.ZipCode; } }

        /// <summary>
        /// 戶籍縣市，此為唯讀屬性，要修改請使用Permanent.County屬性。
        /// </summary>
        [Field(Caption = "縣市", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentCounty { get { return Permanent.County; } }

        /// <summary>
        /// 戶籍鄉鎮市區，此為唯讀屬性，要修改請使用Permanent.Town屬性。
        /// </summary>
        [Field(Caption = "鄉鎮市區", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentTown { get { return Permanent.Town; } }

        /// <summary>
        /// 戶籍村里，此為唯讀屬性，要修改請使用Permanent.District屬性。
        /// </summary>
        [Field(Caption = "村里", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentDistrict { get { return Permanent.District; } }

        /// <summary>
        /// 戶籍鄰，此為唯讀屬性，要修改請使用Permanent.Area屬性。
        /// </summary>
        [Field(Caption = "鄰", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentArea { get { return Permanent.Area; } }

        /// <summary>
        /// 戶籍其他，此為唯讀屬性，要修改請使用Permanent.Detail屬性。
        /// </summary>
        [Field(Caption = "其他", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentDetail { get { return Permanent.Detail; } }

        /// <summary>
        /// 戶籍完整地址，此為唯讀屬性，要修改請使用Permanent物件屬性。
        /// </summary>
        [Field(Caption = "地址", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentAddress { get { return Permanent.ToString();} }

        /// <summary>
        /// 戶籍經度，此為唯讀屬性，要修改請使用Permanent.Longitude屬性。
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentLongitude { get { return Permanent.Longitude; } }

        /// <summary>
        /// 戶籍緯度，此為唯讀屬性，要修改請使用Permanent.Latitude屬性。
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "Address", EntityCaption = "戶籍")]
        public string PermanentLatitude { get { return Permanent.Latitude; } }
        
        /// <summary>
        /// 聯絡地址
        /// </summary>
        public AddressItem Mailing { get;  set; }

        /// <summary>
        /// 聯絡郵遞區號，此為唯讀屬性，要修改請使用Mailing.ZipCode屬性。
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingZipCode { get { return Mailing.ZipCode; } }

        /// <summary>
        /// 聯絡縣市，此為唯讀屬性，要修改請使用Mailing.County屬性。
        /// </summary>
        [Field(Caption = "縣市", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingCounty { get { return Mailing.County; } }

        /// <summary>
        /// 聯絡鄉鎮市區，此為唯讀屬性，要修改請使用Mailing.Town屬性。
        /// </summary>
        [Field(Caption = "鄉鎮市區", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingTown { get { return Mailing.Town; } }

        /// <summary>
        /// 聯絡村里，此為唯讀屬性，要修改請使用Mailing.District屬性。
        /// </summary>
        [Field(Caption = "村里", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingDistrict { get { return Mailing.District; } }

        /// <summary>
        /// 聯絡鄰，此為唯讀屬性，要修改請使用Mailing.Area屬性。
        /// </summary>
        [Field(Caption = "鄰", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingArea { get { return Mailing.Area; } }

        /// <summary>
        /// 聯絡其他，此為唯讀屬性，要修改請使用Mailing.Detail屬性。
        /// </summary>
        [Field(Caption = "其他", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingDetail { get { return Mailing.Detail; } }

        /// <summary>
        /// 聯絡地址，此為唯讀屬性，要修改請使用Mailing物件屬性。
        /// </summary>
        [Field(Caption = "地址", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingAddress { get { return Mailing.ToString(); } }

        /// <summary>
        /// 聯絡經度，此為唯讀屬性，要修改請使用Mailing.Longitude屬性。
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingLongitude { get { return Mailing.Longitude; } }

        /// <summary>
        /// 聯絡緯度，此為唯讀屬性，要修改請使用Mailing.Latitude屬性。
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "Address", EntityCaption = "聯絡")]
        public string MailingLatitude { get { return Mailing.Latitude; } }

        /// <summary>
        /// 其他地址一
        /// </summary>
        public AddressItem Address1 { get;  set; }

        /// <summary>
        /// 其他地址一郵遞區號，此為唯讀屬性，要修改請使用Address1.ZipCode屬性。
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1ZipCode { get { return Address1.ZipCode; } }

        /// <summary>
        /// 其他地址一縣市，此為唯讀屬性，要修改請使用Address1.County屬性。
        /// </summary>
        [Field(Caption = "縣市", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1County { get { return Address1.County; } }

        /// <summary>
        /// 其他地址一鄉鎮市區，此為唯讀屬性，要修改請使用Address1.Town屬性。
        /// </summary>
        [Field(Caption = "鄉鎮市區", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Town { get { return Address1.Town; } }

        /// <summary>
        /// 其他地址一村里，此為唯讀屬性，要修改請使用Address1.District屬性。
        /// </summary>
        [Field(Caption = "村里", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1District { get { return Address1.District; } }

        /// <summary>
        /// 其他地址一鄰，此為唯讀屬性，要修改請使用Address1.Area屬性。
        /// </summary>
        [Field(Caption = "鄰", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Area { get { return Address1.Area; } }

        /// <summary>
        /// 其他地址一其他，此為唯讀屬性，要修改請使用Address1.Detail屬性。
        /// </summary>
        [Field(Caption = "其他", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Detail { get { return Address1.Detail; } }

        /// <summary>
        /// 其他地址一完整地址，此為唯讀屬性，要修改請使用Address1物件屬性。
        /// </summary>
        [Field(Caption = "地址", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Address { get { return Address1.ToString(); } }

        /// <summary>
        /// 其他地址一經度，此為唯讀屬性，要修改請使用Address1.Longitude屬性。
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Longitude { get { return Address1.Longitude; } }

        /// <summary>
        /// 其他地址一緯度，此為唯讀屬性，要修改請使用Address1.Latitude屬性。
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "Address", EntityCaption = "其他地址一")]
        public string Address1Latitude { get { return Address1.Latitude; } }

        /// <summary>
        /// 其他地址二
        /// </summary>
        public AddressItem Address2 { get; set; }

        /// <summary>
        /// 其他地址二郵遞區號，此為唯讀屬性，要修改請使用Address2.ZipCode屬性。
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2ZipCode { get { return Address2.ZipCode; } }

        /// <summary>
        /// 其他地址二縣市，此為唯讀屬性，要修改請使用Address2.County屬性。
        /// </summary>
        [Field(Caption = "縣市", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2County { get { return Address2.County; } }

        /// <summary>
        /// 其他地址二鄉鎮市區，此為唯讀屬性，要修改請使用Address2.Town屬性。
        /// </summary>
        [Field(Caption = "鄉鎮市區", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Town { get { return Address2.Town; } }

        /// <summary>
        /// 其他地址二村里，此為唯讀屬性，要修改請使用Address2.District屬性。
        /// </summary>
        [Field(Caption = "村里", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2District { get { return Address2.District; } }

        /// <summary>
        /// 其他地址二鄰，要修改請使用Address2.Area屬性。
        /// </summary>
        [Field(Caption = "鄰", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Area { get { return Address2.Area; } }

        /// <summary>
        /// 其他地址二其他，要修改請使用Address2.Detail屬性。
        /// </summary>
        [Field(Caption = "其他", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Detail { get { return Address2.Detail; } }

        /// <summary>
        /// 其他地址二完整地址，要修改請使用Address2物件屬性。
        /// </summary>
        [Field(Caption = "地址", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Address { get { return Address2.ToString(); } }

        /// <summary>
        /// 其他地址二經度，要修改請使用Address2.Longitude屬性。
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Longitude { get { return Address2.Longitude; } }

        /// <summary>
        /// 其他地址二緯度，要修改請使用Address2.Latitude屬性。
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "Address", EntityCaption = "其他地址二")]
        public string Address2Latitude { get { return Address2.Latitude; } }

        /// <summary>
        /// 其他地址三
        /// </summary>
        public AddressItem Address3 { get; set; }

        /// <summary>
        /// 其他地址三郵遞區號，要修改請使用Address3.ZipCode屬性。
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3ZipCode { get { return Address3.ZipCode; } }

        /// <summary>
        /// 其他地址三縣市，要修改請使用Address3.County屬性。
        /// </summary>
        [Field(Caption = "縣市", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3County { get { return Address3.County; } }

        /// <summary>
        /// 其他地址三鄉鎮市區，要修改請使用Address3.Town屬性。
        /// </summary>
        [Field(Caption = "鄉鎮市區", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Town { get { return Address3.Town; } }

        /// <summary>
        /// 其他地址三村里，要修改請使用Address3.District屬性。
        /// </summary>
        [Field(Caption = "村里", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3District { get { return Address3.District; } }

        /// <summary>
        /// 其他地址三鄰，要修改請使用Address3.Area屬性。
        /// </summary>
        [Field(Caption = "鄰", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Area { get { return Address3.Area; } }

        /// <summary>
        /// 其他地址三其他，要修改請使用Address3.Detail屬性。
        /// </summary>
        [Field(Caption = "其他", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Detail { get { return Address3.Detail; } }

        /// <summary>
        /// 其他地址三完整地址，要修改請使用Address3物件屬性。
        /// </summary>
        [Field(Caption = "地址", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Address { get { return Address3.ToString(); } }

        /// <summary>
        /// 其他地址三經度，要修改請使用Address3.Longitude屬性。
        /// </summary>
        [Field(Caption = "地址經度", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Longitude { get { return Address3.Longitude; } }

        /// <summary>
        /// 其他地址三緯度，要修改請使用Address3.Latitude屬性。
        /// </summary>
        [Field(Caption = "地址緯度", EntityName = "Address", EntityCaption = "其他地址三")]
        public string Address3Latitude { get { return Address3.Latitude; } }
    }

    /// <summary>
    /// 詳細地址資料
    /// </summary>
    public class AddressItem
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public AddressItem()
        {
            County = Town = District = Area = Detail = Longitude = Latitude = string.Empty;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public AddressItem(XmlElement data)
        {
            County = Town = District = Area = Detail = Longitude = Latitude = string.Empty;

            if (data == null) return;

            XmlHelper xdata = new XmlHelper(data);
            ZipCode = xdata.GetString("ZipCode");
            County = xdata.GetString("County");
            Town = xdata.GetString("Town");
            District = xdata.GetString("District");
            Area = xdata.GetString("Area");
            Detail = xdata.GetString("DetailAddress");
            Longitude = xdata.GetString("Longitude");
            Latitude = xdata.GetString("Latitude");
        }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 縣市
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 鄉鎮市區
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// 村里
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 鄰
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}{2}{3}{4}{5}", ZipCode, County, Town, District, Area, Detail);
        }
    }
}