using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生獎懲資訊
    /// </summary>
    public class DisciplineRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public DisciplineRecord()
        {

        }

        /// <summary>
        /// 新增獎懲記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <param name="OccurDate">獎懲日期</param>
        public DisciplineRecord(string RefStudentID, int SchoolYear, int Semester, DateTime OccurDate,string MeritFlag)
            : this()
        {
            this.RefStudentID = RefStudentID;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;
            this.OccurDate = OccurDate;
            this.MeritFlag = MeritFlag;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public DisciplineRecord(XmlElement element)
            : this()
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

            if (helper.GetElement("Detail/Discipline/Merit") != null)
            {
                MeritA = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@A"));
                MeritB = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@B"));
                MeritC = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Merit/@C"));
            }

            if (helper.GetElement("Detail/Discipline/Demerit") != null)
            {
                DemeritA = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Demerit/@A"));                //大過
                DemeritB = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Demerit/@B"));                //小過
                DemeritC = K12.Data.Int.ParseAllowNull(helper.GetString("Detail/Discipline/Demerit/@C"));                //警告
                ClearDate = K12.Data.DateTimeHelper.Parse(helper.GetString("Detail/Discipline/Demerit/@ClearDate"));       //銷過日期
                ClearReason = helper.GetString("Detail/Discipline/Demerit/@ClearReason");   //銷過事由
                Cleared = helper.GetString("Detail/Discipline/Demerit/@Cleared");           //銷過 
            }

            MeritFlag = helper.GetString("MeritFlag");                                  //0是懲戒,1是獎勵,2是留察
        }

        #region ========= Properties ========
        /// <summary>
        /// 所屬學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
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
        /// 學生獎懲系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Discipline", EntityCaption = "獎懲", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int Semester { get; set; }
        /// <summary>
        /// 獎懲日期，必填
        /// </summary>
        [Field(Caption = "日期", EntityName = "Discipline", EntityCaption = "獎懲")]
        public DateTime OccurDate { get; set; }
        /// <summary>
        /// 登錄記錄日期
        /// </summary>
        [Field(Caption = "登錄日期", EntityName = "Discipline", EntityCaption = "獎懲")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// 獎勵懲戒類別，0是懲戒,1是獎勵,2是留校察看
        /// </summary>
        [Field(Caption = "類別", EntityName = "Discipline", EntityCaption = "獎懲", Remark = "0是懲戒,1是獎勵,2是留校察看")]
        public string MeritFlag { get; set; }
        /// <summary>
        /// 獎懲理由
        /// </summary>
        [Field(Caption = "理由", EntityName = "Discipline", EntityCaption = "獎懲")]
        public string Reason { get; set; }
        /// <summary>
        /// 大功數
        /// </summary>
        [Field(Caption = "大功數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? MeritA { get; set; }
        /// <summary>
        /// 小功數
        /// </summary>
        [Field(Caption = "小功數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? MeritB { get; set; }
        /// <summary>
        /// 獎勵數
        /// </summary>
        [Field(Caption = "獎勵數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? MeritC { get; set; }
        /// <summary>
        /// 大過數
        /// </summary>
        [Field(Caption = "大過數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? DemeritA { get; set; }
        /// <summary>
        /// 小過數
        /// </summary>
        [Field(Caption = "小過數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? DemeritB { get; set; }
        /// <summary>
        /// 警告數
        /// </summary>
        [Field(Caption = "警告數", EntityName = "Discipline", EntityCaption = "獎懲")]
        public int? DemeritC { get; set; }
        /// <summary>
        /// 銷過日期
        /// </summary>
        [Field(Caption = "銷過日期", EntityName = "Discipline", EntityCaption = "獎懲")]
        public DateTime? ClearDate { get; set; }
        /// <summary>
        /// 銷過事由
        /// </summary>
        [Field(Caption = "銷過事由", EntityName = "Discipline", EntityCaption = "獎懲")]
        public string ClearReason { get; set; }
        /// <summary>
        /// 是否銷過
        /// </summary>
        [Field(Caption = "是否銷過", EntityName = "Discipline", EntityCaption = "獎懲")]
        public string Cleared { get; set; }

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