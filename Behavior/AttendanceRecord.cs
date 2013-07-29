using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    //<Attendance ID="1718753">
    //    <Detail>
    //        <Attendance>
    //            <Period AbsenceType="病假1" AttendanceType="一般">二</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">三</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">四</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">五</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">六</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">七</Period>
    //            <Period AbsenceType="病假1" AttendanceType="一般">一</Period>
    //        </Attendance>
    //    </Detail>
    //    <Semester>1</Semester>
    //    <OccurDate>2005-09-02 00:00:00</OccurDate>
    //    <RefStudentID>168920</RefStudentID>
    //    <SchoolYear>94</SchoolYear>
    //</Attendance>

    /// <summary>
    /// 學生缺曠資訊
    /// </summary>
    public class AttendanceRecord
    {
        /// <summary>
        /// 預設建構式，將學生編號及缺曠編號設為空字串
        /// </summary>
        public AttendanceRecord()
        {
            RefStudentID = string.Empty;
            ID = string.Empty;
            PeriodDetail = new List<AttendancePeriod>();
        }

        /// <summary>
        /// 新增缺曠記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <param name="OccurDate">缺曠日期</param>
        public AttendanceRecord(string RefStudentID,int SchoolYear,int Semester,DateTime OccurDate)
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
        public AttendanceRecord(XmlElement element)
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
        public virtual void Load(XmlElement data)
        {
            XmlHelper helper = new XmlHelper(data);

            RefStudentID = helper.GetString("RefStudentID");
            ID = helper.GetString("@ID");
            SchoolYear = K12.Data.Int.Parse(helper.GetString("SchoolYear"));
            Semester = K12.Data.Int.Parse(helper.GetString("Semester"));
            OccurDate = DateTime.Parse(helper.GetDateString("OccurDate"));

            PeriodDetail = new List<AttendancePeriod>();

            foreach (XmlElement each in helper.GetElements("Detail/Attendance/Period"))
            {
                each.SetAttribute("RefStudentID", helper.GetString("RefStudentID"));
                each.SetAttribute("ID", helper.GetString("@ID"));
                each.SetAttribute("SchoolYear", helper.GetString("SchoolYear"));
                each.SetAttribute("Semester", helper.GetString("Semester"));
                each.SetAttribute("OccurDate", helper.GetDateString("OccurDate"));
                each.SetAttribute("DayOfWeek",DayOfWeek);

                PeriodDetail.Add(new AttendancePeriod(each));
            }
        }

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
        /// 學生缺曠記錄系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Attendance", EntityCaption = "缺曠",IsEntityPrimaryKey=true)]
        public string ID { get; set; }
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Attendance", EntityCaption = "缺曠")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "Attendance", EntityCaption = "缺曠")]
        public int Semester { get; set; }
        /// <summary>
        /// 缺曠日期，必填
        /// </summary>
        [Field(Caption = "日期", EntityName = "Attendance", EntityCaption = "缺曠")]
        public DateTime OccurDate { get; set; }
        /// <summary>
        /// 缺曠日期屬於週幾
        /// </summary>
        [Field(Caption = "週幾", EntityName = "Attendance", EntityCaption = "缺曠")]
        public string DayOfWeek
        {
            get
            {
                return DayOfWeekInChinese(OccurDate.DayOfWeek);
            }
        }

        /// <summary>
        /// 學生缺曠記錄詳細內容，以節為單位記錄缺曠資訊
        /// </summary>
        public List<AttendancePeriod> PeriodDetail { get; set; }        //記錄內容

        private string DayOfWeekInChinese(DayOfWeek day)
        {
            switch (day)
            {
                case System.DayOfWeek.Monday:
                    return "一";
                case System.DayOfWeek.Tuesday:
                    return "二";
                case System.DayOfWeek.Wednesday:
                    return "三";
                case System.DayOfWeek.Thursday:
                    return "四";
                case System.DayOfWeek.Friday:
                    return "五";
                case System.DayOfWeek.Saturday:
                    return "六";
                default:
                    return "日";
            }
        }
    }

    //<Period AbsenceType="病假1" AttendanceType="一般">二</Period>
    //<Period AbsenceType="病假1" AttendanceType="一般">三</Period>

    /// <summary>
    /// 學生缺曠記錄詳細內容，以節為單位記錄缺曠資訊
    /// </summary>
    public class AttendancePeriod
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public AttendancePeriod() { }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public AttendancePeriod(XmlElement element)
        {
            //AttendanceRecord屬性
            RefStudentID = element.GetAttribute("RefStudentID");
            ID = element.GetAttribute("ID");
            SchoolYear = K12.Data.Int.Parse(element.GetAttribute("SchoolYear"));
            Semester = K12.Data.Int.Parse(element.GetAttribute("Semester"));
            OccurDate = DateTime.Parse(element.GetAttribute("OccurDate"));
            DayOfWeek = element.GetAttribute("DayOfWeek");

            //AttendancePeriod屬性
            Period = element.InnerText;
            AbsenceType = element.GetAttribute("AbsenceType");
            PeriodType = element.GetAttribute("AttendanceType");
        }

        /// <summary>
        /// 缺曠記錄系統編號，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "編號", EntityName = "Attendance", EntityCaption = "缺曠",IsEntityPrimaryKey=true)]
        public string ID { get; private set; }
        /// <summary>
        /// 所屬學生編號，此為唯讀屬性，要修改請使用AttendanceRecord.RefStudentID屬性。
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; private set; }
        /// <summary>
        /// 學年度，此為唯讀屬性，要修改請使用AttendanceRecord.SchoolYear屬性。
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Attendance", EntityCaption = "缺曠")]
        public int SchoolYear { get; private set; }
        /// <summary>
        /// 學期，此為唯讀屬性，要修改請使用AttendanceRecord.Semester屬性。
        /// </summary>
        [Field(Caption = "學期", EntityName = "Attendance", EntityCaption = "缺曠")]
        public int Semester { get; private set; }
        /// <summary>
        /// 缺曠日期，此為唯讀屬性，要修改請使用AttendanceRecord.OccurDate屬性。
        /// </summary>
        [Field(Caption = "日期", EntityName = "Attendance", EntityCaption = "缺曠")]
        public DateTime OccurDate { get; private set; }
        /// <summary>
        /// 缺曠日期屬於週幾，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "週幾", EntityName = "Attendance", EntityCaption = "缺曠")]
        public string DayOfWeek { get; private set;}
        /// <summary>
        /// 節次 
        /// </summary>
        [Field(Caption = "節次", EntityName = "AttendancePeriod", EntityCaption = "缺曠")]
        public string Period { get; set; }
        /// <summary>
        /// 節次類型，此屬性已不再使用，為唯讀屬性，目前節次類別是存在於『節次對照表』當中，請使用PeriodMapping類別來取得。
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")]
        [Field(Caption = "節次類型", EntityName = "AttendancePeriod", EntityCaption = "缺曠", Remark = "此屬性已不再使用，為唯讀屬性，目前節次類別是存在於『節次對照表』當中，開發人員請使用PeriodMapping類別來取得『節次對照表』。")]
        public string PeriodType { get; private set; }
        /// <summary>
        /// 假別，例曠課、事假、喪假…等
        /// </summary>
        [Field(Caption = "假別", EntityName = "AttendancePeriod", EntityCaption = "缺曠")]
        public string AbsenceType { get; set; }
    }
}