using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生學期歷程資訊
    /// </summary>
    public class SemesterHistoryRecord
    {
        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get;  set; }
        /// <summary>
        /// 學期歷程項目，每位學生每學年度學期會有一筆學期歷程項目
        /// </summary>
        public List<SemesterHistoryItem> SemesterHistoryItems { get; set; }
        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student
        {
            get { return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null; }
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SemesterHistoryRecord()
        {
            SemesterHistoryItems = new List<SemesterHistoryItem>();
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public SemesterHistoryRecord(XmlElement element)
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
            if (data != null)
            {
                RefStudentID = data.GetAttribute("ID");
                
                SemesterHistoryItems = new List<SemesterHistoryItem>();

                foreach (XmlElement elm in data.SelectNodes("SemesterHistory/History"))
                {
                    elm.SetAttribute("ID", RefStudentID);
                    SemesterHistoryItems.Add(new SemesterHistoryItem(elm));
                }
            }
        }
    }

    /// <summary>
    /// 學期歷程項目
    /// </summary>
    public class SemesterHistoryItem
    {
        /// <summary>
        /// 建構式初始化學年度及學期為系統設定之預設學年度及學期
        /// </summary>
        public SemesterHistoryItem()
        {
            SchoolYear = K12.Data.Int.Parse(School.DefaultSchoolYear);
            Semester = K12.Data.Int.Parse(School.DefaultSemester);
        }

        /// <summary>
        /// XM參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public SemesterHistoryItem(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入設定值
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            RefStudentID = data.GetAttribute("ID");
            SchoolYear = K12.Data.Int.Parse(data.GetAttribute("SchoolYear"));
            Semester = K12.Data.Int.Parse(data.GetAttribute("Semester"));
            GradeYear = K12.Data.Int.Parse(data.GetAttribute("GradeYear"));
            ClassName = data.GetAttribute("ClassName");
            SeatNo = K12.Data.Int.ParseAllowNull(data.GetAttribute("SeatNo"));
            SchoolDayCount = K12.Data.Int.ParseAllowNull(data.GetAttribute("SchoolDayCount"));
            Teacher = data.GetAttribute("Teacher");
            // 高中 科別名稱
            DeptName = data.GetAttribute("DeptName"); 
        }

        /// <summary>
        /// 所屬學生編號，此為唯讀屬性，若需設定請使用SemesterHistoryRecord的RefStudentID屬性。
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get; private set; }

        /// <summary>
        /// 所屬學生記錄物件，此為唯讀屬性
        /// </summary>
        public StudentRecord Student 
        { 
            get { return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null; } 
        }

        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Caption = "學年度", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期
        /// </summary>
        [Field(Caption = "學期", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public int Semester { get; set; }
        /// <summary>
        /// 年級
        /// </summary>
        [Field(Caption = "年級", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public int GradeYear { get; set; }
        /// <summary>
        /// 班級名稱
        /// </summary>
        [Field(Caption = "班級名稱", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public string ClassName { get; set; }
        /// <summary>
        /// 座號
        /// </summary>
        [Field(Caption = "座號", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public int? SeatNo { get; set; }
        /// <summary>
        /// 學期上課天數
        /// </summary>
        [Field(Caption = "上課天數", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程")]
        public int? SchoolDayCount { get; set; }
        /// <summary>
        /// 該學年度學期班導師
        /// </summary>
        [Field(Caption = "班導師", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程", Remark = "該學年度學期班導師")]
        public string Teacher { get; set; }

        /// <summary>
        /// 該學年度學期科別名稱
        /// </summary>
        [Field(Caption = "科別名稱", EntityName = "SemesterHistoryItem", EntityCaption = "學期歷程", Remark = "該學年度學期科別名稱")]
        public string DeptName { get; set; }
    }
}