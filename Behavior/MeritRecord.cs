using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生獎勵記錄，新增物件時會直接將MeritFlag屬性設為1
    /// </summary>
    public class MeritRecord
    {
        /// <summary>
        /// 預設建構式，將MeritFlag設為1，代表為獎勵
        /// </summary>
        public MeritRecord()
        {
            //所有建構式都必需呼叫空建構式，以設定MeritFlag為1代表為獎勵記錄
            MeritFlag = "1";
        }

        /// <summary>
        /// 新增獎懲記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <param name="OccurDate">獎勵日期</param>
        public MeritRecord(string RefStudentID, int SchoolYear, int Semester, DateTime OccurDate):this()
        {
            this.RefStudentID = RefStudentID;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;
            this.OccurDate = OccurDate;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public MeritRecord(XmlElement element):this()
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            XmlHelper helper = new XmlHelper(data);

            RefStudentID = helper.GetString("RefStudentID");
            ID = helper.GetString("@ID");                                               //獎懲編號
            SchoolYear = K12.Data.Int.Parse(helper.GetString("SchoolYear"));                                //學年度
            Semester = K12.Data.Int.Parse(helper.GetString("Semester"));                                    //學期
            OccurDate = K12.Data.DateTimeHelper.ParseDirect(helper.GetDateString("OccurDate"));                              //獎勵日期
            RegisterDate = K12.Data.DateTimeHelper.Parse(helper.GetDateString("RegisterDate"));
            Reason = helper.GetString("Reason");                                        //事由
            MeritA = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@A"));
            MeritB = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@B"));
            MeritC = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@C"));
            MeritFlag = helper.GetString("MeritFlag");                                  //0是懲戒,1是獎勵,2是留察
        }

        #region ========= Properties ========
        /// <summary>
        /// 所屬學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; set; }
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
        /// 學生獎勵系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Merit", EntityCaption = "獎勵", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Merit", EntityCaption = "獎勵")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "Merit", EntityCaption = "獎勵")]
        public int Semester { get; set; }
        /// <summary>
        /// 獎勵日期，必填
        /// </summary>
        [Field(Caption = "日期", EntityName = "Merit", EntityCaption = "獎勵")]
        public DateTime OccurDate { get; set; }
        /// <summary>
        /// 登錄獎勵記錄日期
        /// </summary>
        [Field(Caption = "登錄日期", EntityName = "Merit", EntityCaption = "獎勵")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// 獎勵懲戒類別，0是懲戒,1是獎勵,2是留校察看
        /// </summary>
        [Field(Caption = "類別", EntityName = "Merit", EntityCaption = "獎勵", Remark = "0是懲戒,1是獎勵,2是留校察看")]
        public string MeritFlag { get; set; }
        /// <summary>
        /// 獎勵理由
        /// </summary>
        [Field(Caption = "類別", EntityName = "Merit", EntityCaption = "獎勵")]
        public string Reason { get; set; }
        /// <summary>
        /// 大功數
        /// </summary>
        [Field(Caption = "大功數", EntityName = "Merit", EntityCaption = "獎勵")]
        public int? MeritA { get; set; }
        /// <summary>
        /// 小功數
        /// </summary>
        [Field(Caption = "小功數", EntityName = "Merit", EntityCaption = "獎勵")]
        public int? MeritB { get; set; }
        /// <summary>
        /// 獎勵數
        /// </summary>
        [Field(Caption = "獎勵數", EntityName = "Merit", EntityCaption = "獎勵")]
        public int? MeritC { get; set; }

        #endregion

        // element 的 tag 結構為：

        //<Discipline ID="1097220">
        //    <Semester>1</Semester>
        //    <OccurDate>2007/12/13</OccurDate>
        //    <Type>1</Type>
        //    <StudentNumber>514163</StudentNumber>
        //    <SchoolYear>96</SchoolYear>
        //    <GradeYear>2</GradeYear>
        //    <MeritFlag>2</MeritFlag>
        //    <Name>陳文淇1</Name>
        //    <Detail>
        //        <Discipline>
        //            <Demerit A="0" B="0" C="0" ClearDate="" ClearReason="" Cleared="" />
        //        </Discipline>
        //    </Detail>
        //    <SeatNo />
        //    <Gender>女</Gender>
        //    <RefStudentID>169968</RefStudentID>
        //    <ClassName>綜二義</ClassName>
        //    <Reason>長太醜, 該死</Reason>
        //</Discipline>
    }
}