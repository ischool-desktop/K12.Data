using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生離校資訊
    /// </summary>
    public class LeaveInfoRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public LeaveInfoRecord()
        {

        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public LeaveInfoRecord(XmlElement data)
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
            RefStudentID = data.SelectSingleNode("@ID").InnerText;

            if (data.SelectSingleNode("LeaveInfo/LeaveInfo/@ClassName") != null)
                ClassName = data.SelectSingleNode("LeaveInfo/LeaveInfo/@ClassName").InnerText;

            if (data.SelectSingleNode("LeaveInfo/LeaveInfo/@Memo") != null)
                Memo = data.SelectSingleNode("LeaveInfo/LeaveInfo/@Memo").InnerText;

            if (data.SelectSingleNode("LeaveInfo/LeaveInfo/@Reason") != null)
                Reason = data.SelectSingleNode("LeaveInfo/LeaveInfo/@Reason").InnerText;

            if (data.SelectSingleNode("LeaveInfo/LeaveInfo/@Department")!=null)
                DepartmentName = data.SelectSingleNode("LeaveInfo/LeaveInfo/@Department").InnerText;

            if (data.SelectSingleNode("DiplomaNumber/DiplomaNumber") != null)
                DiplomaNumber = data.SelectSingleNode("DiplomaNumber/DiplomaNumber").InnerText;

            XmlNode SchoolYearNode = data.SelectSingleNode("LeaveInfo/LeaveInfo/@SchoolYear");

            SchoolYear = (SchoolYearNode != null) ? K12.Data.Int.ParseAllowNull(SchoolYearNode.InnerText) : null;

            //高中XmlElement
            //<LeaveInfo>
            //    <LeaveInfo ClassName="電三忠" Department="電機修護科" Reason="畢業" SchoolYear="99" />
            //</LeaveInfo>
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
            get { return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null; } 
        }

        /// <summary>
        /// 班級名稱
        /// </summary>
        [Field(Caption = "班級名稱", EntityName = "LeaveInfo", EntityCaption = "離校")]
        public string ClassName { get; set; }

        /// <summary>
        /// 備忘資訊
        /// </summary>
        [Field(Caption = "備忘資訊", EntityName = "LeaveInfo", EntityCaption = "離校")]
        public string Memo { get;  set; }

        /// <summary>
        /// 離校原因
        /// </summary>
        [Field(Caption = "原因", EntityName = "LeaveInfo", EntityCaption = "離校")]
        public string Reason { get; set; }

        /// <summary>
        /// 離校學年度
        /// </summary>
        [Field(Caption = "學年度", EntityName = "LeaveInfo", EntityCaption = "離校")]
        public int? SchoolYear { get;  set; }

        /// <summary>
        /// 畢業證書字號
        /// </summary>
        [Field(Caption = "畢業證書字號", EntityName = "LeaveInfo", EntityCaption = "離校")]
        public string DiplomaNumber { get; set; }

        /// <summary>
        /// 離校時的科別
        /// </summary>
        [Field(Caption = "科別名稱",EntityName ="LeaveInfo",EntityCaption ="離校")]
        protected internal string DepartmentName { get; set; }
    }
}