using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生手足資訊
    /// </summary>
    public class SiblingRecord
    {
        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; set; }
        /// <summary>
        /// 手足名單
        /// </summary>
        public List<SiblingItem>  SiblingItems { get; set; }
        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student
        {
            get { return !string.IsNullOrEmpty(RefStudentID) ? K12.Data.Student.SelectByID(RefStudentID) : null; }
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SiblingRecord()
        {
            SiblingItems = new List<SiblingItem>();
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="Row"></param>
        public SiblingRecord(DataRow Row)
        {
            Load(Row);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="Row"></param>
        public void Load(DataRow Row)
        {
            RefStudentID = "" + Row["id"];

            string OtherInfo = "" + Row["sibling_info"];

            if (!string.IsNullOrEmpty(OtherInfo))
            {
                SiblingItems = new List<SiblingItem>();

                XmlDocument Document = new XmlDocument();
                Document.LoadXml(OtherInfo);

                XmlHelper Elements = new XmlHelper(Document.DocumentElement);

                foreach (XmlElement Element in Elements.GetElements("SiblingInfo"))
                {
                    SiblingItem Item = new SiblingItem(Element);

                    SiblingItems.Add(Item);
                }
            }
        }
    }

    /// <summary>
    /// 手足項目
    /// </summary>
    public class SiblingItem
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public SiblingItem()        
        {

        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="Element"></param>
        public SiblingItem(XmlElement Element)
        {
            Load(Element);
        }

        /// <summary>
        /// 從XML載入值
        /// <![CDATA[
        ///<SiblingInfo>
        ///    <Relationship>兄</Relationship>
        ///    <Name>王大明</Name>
        ///    <Birthdate/>
        ///    <School/>
        ///    <SchoolLocation/>
        ///    <ClassName/>
        ///    <SeatNo/>
        ///    <Memo/>
        ///<SiblingInfo>
        /// ]]>
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XmlElement Element)
        {
            XmlHelper Elements = new XmlHelper(Element);            

            Name = Elements.GetString("Name");
            Relationship = Elements.GetString("Relationship");
            BirthDate = K12.Data.DateTimeHelper.Parse(Elements.GetString("Birthdate"));
            SchoolName = Elements.GetString("School");
            SchoolLocation = Elements.GetString("SchoolLocation");
            ClassName = Elements.GetString("ClassName");
            Memo = Elements.GetString("Memo");
        }

        /// <summary>
        /// 所屬學生編號，此為唯讀屬性，若需設定請使用SemesterHistoryRecord的RefStudentID屬性。
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; private set; }

        /// <summary>
        /// 所屬學生記錄物件，此為唯讀屬性
        /// </summary>
        public StudentRecord Student
        {
            get { return !string.IsNullOrEmpty(RefStudentID) ? K12.Data.Student.SelectByID(RefStudentID) : null; }
        }

        /// <summary>
        /// 兄弟姊妹姓名
        /// </summary>
        [Field(Caption = "姓名", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string  Name { get; set; }

        /// <summary>
        /// 關係，兄弟或姊妹
        /// </summary>
        [Field(Caption = "關係", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string Relationship { get; set;}


        /// <summary>
        /// 生日，兄弟或姊妹
        /// </summary>
        [Field(Caption = "生日", EntityName = "SiblingItem", EntityCaption = "手足")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 兄弟姊妹學校名稱
        /// </summary>
        [Field(Caption = "學校名稱", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string SchoolName { get; set;}

        /// <summary>
        /// 兄弟姊妹學校地點
        /// </summary>
        [Field(Caption = "學校地點", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string SchoolLocation { get; set; }

        /// <summary>
        /// 兄弟姊妹班級名稱
        /// </summary>
        [Field(Caption = "班級", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string ClassName { get; set;}

        /// <summary>
        /// 兄弟姊妹班級備註
        /// </summary>
        [Field(Caption = "備註", EntityName = "SiblingItem", EntityCaption = "手足")]
        public string Memo { get; set; }
    }
}