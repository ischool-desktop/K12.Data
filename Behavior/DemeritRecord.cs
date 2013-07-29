using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生懲戒資訊，新增物件時會直接將MeritFlag屬性設為0
    /// </summary>
    public class DemeritRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public DemeritRecord()
        {
            //所有建構式都必需呼叫空建構式，以設定MeritFlag為1代表為獎勵記錄
            MeritFlag = "0";
        }

        /// <summary>
        /// 新增學生懲戒記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <param name="OccurDate">懲戒日期</param>
        public DemeritRecord(string RefStudentID, int SchoolYear, int Semester, DateTime OccurDate):this()
        {
            this.RefStudentID = RefStudentID;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;
            this.OccurDate = OccurDate;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        ///<Discipline ID="1097220">
        ///    <Semester>1</Semester>
        ///    <OccurDate>2007/12/13</OccurDate>
        ///    <Type>1</Type>
        ///    <StudentNumber>514163</StudentNumber>
        ///    <SchoolYear>96</SchoolYear>
        ///    <GradeYear>2</GradeYear>
        ///    <MeritFlag>2</MeritFlag>
        ///    <Name>陳文淇1</Name>
        ///    <Detail>
        ///        <Discipline>
        ///            <Demerit A="0" B="0" C="0" ClearDate="" ClearReason="" Cleared="" />
        ///        </Discipline>
        ///    </Detail>
        ///    <SeatNo />
        ///    <Gender>女</Gender>
        ///    <RefStudentID>169968</RefStudentID>
        ///    <ClassName>綜二義</ClassName>
        ///    <Reason>長太醜, 該死</Reason>
        ///</Discipline>
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public DemeritRecord(XmlElement element):this()
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        ///<Discipline ID="1097220">
        ///    <Semester>1</Semester>
        ///    <OccurDate>2007/12/13</OccurDate>
        ///    <Type>1</Type>
        ///    <StudentNumber>514163</StudentNumber>
        ///    <SchoolYear>96</SchoolYear>
        ///    <GradeYear>2</GradeYear>
        ///    <MeritFlag>2</MeritFlag>
        ///    <Name>陳文淇1</Name>
        ///    <Detail>
        ///        <Discipline>
        ///            <Demerit A="0" B="0" C="0" ClearDate="" ClearReason="" Cleared="" />
        ///        </Discipline>
        ///    </Detail>
        ///    <SeatNo />
        ///    <Gender>女</Gender>
        ///    <RefStudentID>169968</RefStudentID>
        ///    <ClassName>綜二義</ClassName>
        ///    <Reason>長太醜, 該死</Reason>
        ///</Discipline>
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            XmlHelper helper = new XmlHelper(data);

            RefStudentID = helper.GetString("RefStudentID");
            ID = helper.GetString("@ID");                                               //ID
            SchoolYear = K12.Data.Int.Parse(helper.GetString("SchoolYear"));                                //學年度
            Semester = K12.Data.Int.Parse(helper.GetString("Semester"));                                    //學期

            OccurDate = K12.Data.DateTimeHelper.ParseDirect(helper.GetDateString("OccurDate"));                              //懲戒日期
            RegisterDate = K12.Data.DateTimeHelper.Parse(helper.GetDateString("RegisterDate"));

            Reason = helper.GetString("Reason");                                        //事由
            DemeritA = K12.Data.Int.ParseAllowNull(helper.GetElement("Detail/Discipline/Demerit").Attributes["A"].Value);                //大過
            DemeritB = K12.Data.Int.ParseAllowNull(helper.GetElement("Detail/Discipline/Demerit").Attributes["B"].Value);                //小過
            DemeritC = K12.Data.Int.ParseAllowNull(helper.GetElement("Detail/Discipline/Demerit").Attributes["C"].Value);                //警告

            if (helper.GetElement("Detail/Discipline/Demerit").Attributes["ClearDate"] != null)
                ClearDate = K12.Data.DateTimeHelper.Parse(helper.GetElement("Detail/Discipline/Demerit").Attributes["ClearDate"].Value);       //銷過日期

            if (helper.GetElement("Detail/Discipline/Demerit").Attributes["ClearReason"] != null)
                ClearReason = helper.GetElement("Detail/Discipline/Demerit").Attributes["ClearReason"].Value;   //銷過事由

            Cleared = helper.GetElement("Detail/Discipline/Demerit").Attributes["Cleared"].Value;           //銷過
            MeritFlag = helper.GetString("MeritFlag");                                  //0是懲戒,1是獎勵,2是留察
        }

        #region ========= Properties ========

        /// <summary>
        /// 所屬學生記錄編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get;  set; }
        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student
        {
            get
            {
                return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null;
            }
        }
        /// <summary>
        /// 學生懲戒記錄系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Demerit", EntityCaption = "懲戒",IsEntityPrimaryKey=true)]
        public string ID { get;  set; }
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Demerit", EntityCaption = "懲戒")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "Demerit", EntityCaption = "懲戒")]
        public int Semester { get;  set; }
        /// <summary>
        /// 日期，必填
        /// </summary>
        [Field(Caption = "日期", EntityName = "Demerit", EntityCaption = "懲戒")]
        public DateTime OccurDate { get;  set; }
        /// <summary>
        /// 獎勵懲戒類別，0是懲戒,1是獎勵,2是留校察看
        /// </summary>
        [Field(Caption = "類別", EntityName = "Demerit", EntityCaption = "懲戒", Remark = "0是懲戒,1是獎勵,2是留校察看")]
        public string MeritFlag { get;  set; }
        /// <summary>
        /// 事由
        /// </summary>
        [Field(Caption = "事由", EntityName = "Demerit", EntityCaption = "懲戒")]
        public string Reason { get;  set; }
        /// <summary>
        /// 大過數
        /// </summary>
        [Field(Caption = "大過數", EntityName = "Demerit", EntityCaption = "懲戒")]
        public int? DemeritA { get; set; }
        /// <summary>
        /// 小過數
        /// </summary>
        [Field(Caption = "小過數", EntityName = "Demerit", EntityCaption = "懲戒")]
        public int? DemeritB { get;  set; }
        /// <summary>
        /// 警告數
        /// </summary>
        [Field(Caption = "警告數", EntityName = "Demerit", EntityCaption = "懲戒")]
        public int? DemeritC { get;  set; }
        /// <summary>
        /// 登錄獎懲記錄日期
        /// </summary>
        [Field(Caption = "登錄日期", EntityName = "Demerit", EntityCaption = "懲戒")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// 銷過日期
        /// </summary>
        [Field(Caption = "銷過日期", EntityName = "Demerit", EntityCaption = "懲戒")]
        public DateTime? ClearDate { get;  set; }
        /// <summary>
        /// 銷過事由
        /// </summary>
        [Field(Caption = "銷過事由", EntityName = "Demerit", EntityCaption = "懲戒")]
        public string ClearReason { get;  set; }
        /// <summary>
        /// 是否銷過
        /// </summary>
        [Field(Caption = "是否銷過", EntityName = "Demerit", EntityCaption = "懲戒")]
        public string Cleared { get;  set; }

        #endregion

    }
}