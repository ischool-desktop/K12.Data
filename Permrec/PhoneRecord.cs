using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生電話資訊
    /// </summary>
    public class PhoneRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public PhoneRecord()
        {

        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public PhoneRecord(XmlElement data)
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
            XmlHelper xdata = new XmlHelper(data);

            RefStudentID = data.GetAttribute("RefStudentID");
            Permanent = xdata.GetString("Permanent");
            Contact = xdata.GetString("Contact");
            Cell = xdata.GetString("Cell");

            int index = 0;
            Phone1 = Phone2 = Phone3 = string.Empty;
            foreach (XmlElement each in xdata.GetElements("Phones/PhoneNumber"))
            {
                switch (index)
                {
                    case 0:
                        Phone1 = each.InnerText;
                        break;
                    case 1:
                        Phone2 = each.InnerText;
                        break;
                    case 2:
                        Phone3 = each.InnerText;
                        break;
                }
                index++;
            }
        }

        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get; set; }

        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student
        {
            get { return !string.IsNullOrEmpty(RefStudentID) ? K12.Data.Student.SelectByID(RefStudentID) : null; }
        }

        /// <summary>
        /// 戶籍電話
        /// </summary>
        [Field(Caption = "戶籍電話", EntityName = "Phone", EntityCaption = "學生")]
        public string Permanent { get;  set; }

        /// <summary>
        /// 聯絡電話
        /// </summary>
        [Field(Caption = "聯絡電話", EntityName = "Phone", EntityCaption = "學生")]
        public string Contact { get;  set; }

        /// <summary>
        /// 手機
        /// </summary>
        [Field(Caption = "手機", EntityName = "Phone", EntityCaption = "學生")]
        public string Cell { get;  set; }

        /// <summary>
        /// 其他電話一
        /// </summary>
        [Field(Caption = "其他電話一", EntityName = "Phone", EntityCaption = "學生")]
        public string Phone1 { get;  set; }

        /// <summary>
        /// 其他電話二
        /// </summary>
        [Field(Caption = "其他電話二", EntityName = "Phone", EntityCaption = "學生")]
        public string Phone2 { get;  set; }

        /// <summary>
        /// 其他電話三
        /// </summary>
        [Field(Caption = "其他電話三", EntityName = "Phone", EntityCaption = "學生")]
        public string Phone3 { get;  set; }
    }
}