using System.Xml;

namespace K12.Data
{
    //<SemesterMoralScore ID="19">
    //    <Name>黃靖婷</Name>
    //    <Semester>1</Semester>
    //    <RefStudentID>168708</RefStudentID>
    //    <TextScore>....一大堆東西</TextScore>
    //    <SchoolYear>96</SchoolYear>
    //</SemesterMoralScore>

    /// <summary>
    /// 缺曠獎懲統計類別
    /// 1.Discipline：獎懲統計
    /// 2.Attendance：缺曠統計
    /// 3.DisciplineAndAttendance：獎懲統計及缺曠統計
    /// 4.None：無
    /// </summary>
    public enum SummaryType
    {
        Discipline, Attendance, DisciplineAndAttendance,None
    }

    /// <summary>
    /// 學期德行評量資訊
    /// </summary>
    public class MoralScoreRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public MoralScoreRecord()
        {

        }

        /// <summary>
        /// 新增試別項目記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        public MoralScoreRecord(string RefStudentID,int SchoolYear,int Semester)
        {
            this.RefStudentID = RefStudentID;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]> 
        /// </summary>
        /// <param name="element"></param>
        public MoralScoreRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public void Load(XmlElement element)
        {
            XmlHelper helper = new XmlHelper(element);

            RefStudentID = helper.GetString("RefStudentID");
            ID = helper.GetString("@ID");
            SchoolYear = K12.Data.Int.Parse(helper.GetString("SchoolYear"));
            Semester = K12.Data.Int.Parse(helper.GetString("Semester"));
            TextScore = helper.GetElement("TextScore");
            InitialSummary = helper.GetElement("InitialSummary");
            Summary = helper.GetElement("Summary");
            Diff = K12.Data.Decimal.ParseAllowNull(helper.GetString("SupervisedByDiff"));
            Comment = helper.GetString("SupervisedByComment");
            OtherDiff = helper.GetElement("OtherDiff");
        }

        /// <summary>
        /// 所屬學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get;  set; }            
        
        /// <summary>
        /// 所屬學生物件
        /// </summary>
        public StudentRecord Student
        { 
            get
            {
                return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null;
            }
        }

        /// <summary>
        /// 系統編號 
        /// </summary>
        [Field(Caption = "編號", EntityName = "MoralScore", EntityCaption = "德行評量", IsEntityPrimaryKey = true)]
        public string ID { get;  set; }                    
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "MoralScore", EntityCaption = "德行評量")]
        public int SchoolYear { get;  set; }                  
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "MoralScore", EntityCaption = "德行評量")]
        public int Semester { get;  set; }
        /// <summary>
        /// 導師加減分
        /// </summary>
        protected internal decimal? Diff { get; set; }
        /// <summary>
        /// 導師評語
        /// </summary>
        protected internal string Comment { get; set; }
        /// <summary>
        /// 其他加減分
        /// </summary>
        protected internal XmlElement OtherDiff { get; set; }
        /// <summary>
        /// 學期文字評量
        /// </summary>
        public XmlElement TextScore { get;  set; }
        /// <summary>
        /// 非明細缺曠獎懲統計
        /// </summary>
        public XmlElement InitialSummary { get; set; }
        /// <summary>
        /// 非明細缺曠獎懲統計類別，根據InitialSummary當中的內容來判斷包含的統計類別
        /// </summary>
        public SummaryType InitialSummaryType
        {
            get
            {
                //假設InitialSummary為null則不包含任何的缺曠獎懲類別
                if (InitialSummary == null)
                    return SummaryType.None;

                if (InitialSummary.SelectSingleNode("AttendanceStatistics") != null && InitialSummary.SelectSingleNode("DisciplineStatistics") != null)
                    return SummaryType.DisciplineAndAttendance;

                if (InitialSummary.SelectSingleNode("AttendanceStatistics") != null)
                    return SummaryType.Attendance;

                if (InitialSummary.SelectSingleNode("DisciplineStatistics") !=null)
                    return SummaryType.Discipline;

                return SummaryType.None;
            }
        }

        /// <summary>
        /// 學期缺曠統計，為轉入學期缺曠統計再加上系統缺曠，此計算需經過ischool介面完成
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")] 
        public XmlElement Summary { get; set; }
    }
}