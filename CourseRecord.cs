using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 課程資訊
    /// </summary>
    public class CourseRecord : IComparable<CourseRecord>
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName="Course" , EntityCaption = "課程",IsEntityPrimaryKey=true)]
        public string ID { get;  set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "Course", EntityCaption = "課程")]
        public string Name { get;  set; }
        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Caption = "學年度", EntityName = "Course", EntityCaption = "課程")]
        public int? SchoolYear { get;  set; }
        /// <summary>
        /// 學期
        /// </summary>
        [Field(Caption = "學期", EntityName = "Course", EntityCaption = "課程")]
        public int? Semester { get;  set; }
        /// <summary>
        /// 科目
        /// </summary>
        [Field(Caption = "科目", EntityName = "Course", EntityCaption = "課程")]
        public string Subject { get;  set; }
        /// <summary>
        /// 科目級別
        /// </summary>
        protected internal decimal? Level { get; set; }
        /// <summary>
        /// 節數，實際的上課時數
        /// </summary>
        [Field(Caption = "節數", EntityName = "Course", EntityCaption = "課程")]
        public decimal? Period { get;  set; }
        /// <summary>
        /// 權數，相當於高中的學分數
        /// </summary>
        [Field(Caption = "權數(學分數)", EntityName = "Course", EntityCaption = "課程")]
        public decimal? Credit { get;  set; }
        /// <summary>
        /// 所屬班級編號
        /// </summary>
        [Field(Caption = "班級編號", EntityName = "Class", EntityCaption = "班級",IsEntityPrimaryKey=true)]
        public string RefClassID { get;  set; }
        /// <summary>
        /// 所屬試別設定編號
        /// </summary>
        [Field(Caption = "評量設定編號", EntityName = "AssessmentSetup", EntityCaption = "評量設定",IsEntityPrimaryKey=true)]
        public string RefAssessmentSetupID { get;  set; }
        /// <summary>
        /// 分項，主要高中使用
        /// </summary>
        protected internal string Entry { get; set; }
        /// <summary>
        /// 領域
        /// </summary>
        protected internal string Domain { get; set; }
        /// <summary>
        /// 是否列入學期成績計算，1:列入學期成績，2:不列入學期成績。
        /// </summary>
        protected internal string CalculationFlag { get; set; }
        /// <summary>
        /// 部/校訂
        /// </summary>
        protected internal string RequiredBy { get; set; }
        /// <summary>
        /// 必俢
        /// </summary>
        protected internal bool Required { get; set; }
        /// <summary>
        /// 不計學分
        /// </summary>
        protected internal bool NotIncludedInCredit { get; set; }
        /// <summary>
        /// 不評分
        /// </summary>
        protected internal bool NotIncludedInCalc { get; set; }
        /// <summary>
        /// 所屬班級
        /// </summary>
        public ClassRecord Class 
        { 
            get 
            { 
                return !string.IsNullOrEmpty(RefClassID) ? K12.Data.Class.SelectByID(RefClassID) : null; 
            }
        }
        /// <summary>
        /// 所屬試別設定
        /// </summary>
        public AssessmentSetupRecord AssessmentSetup 
        { 
            get
            {  
                return !string.IsNullOrEmpty(RefAssessmentSetupID)? K12.Data.AssessmentSetup.SelectByID(RefAssessmentSetupID):null;
            }
        }
        /// <summary>
        /// 主要授課教師編號，唯讀；若需對課程授課教師做完整操作，請參考TCInstruct及TCInstructRecord。
        /// </summary>
        [Field(Caption = "教師編號（主要授課）", EntityName ="Teacher" , EntityCaption = "教師" ,IsEntityPrimaryKey=true )]
        public string MajorTeacherID { get; private set; }
        /// <summary>
        /// 主要授課教師名稱，唯讀；若需對課程授課教師做完整操作，請參考TCInstruct及TCInstructRecord。
        /// </summary>
        [Field(Caption = "教師名稱（主要授課）", EntityName = "Teacher", EntityCaption = "教師")]
        public string MajorTeacherName { get; private set; }
        /// <summary>
        /// 主要授課教師暱稱，唯讀；若需對課程授課教師做完整操作，請參考TCInstruct及TCInstructRecord。
        /// </summary>
        [Field(Caption = "教師暱稱（主要授課）", EntityName = "Teacher", EntityCaption = "教師")]
        public string MajorTeacherNickname { get; private set; }
        /// <summary>
        /// 課程授課教師列表，唯讀；若需對課程授課教師做完整操作，請參考TCInstruct及TCInstructRecord。
        /// </summary>
        public List<CourseTeacherRecord> Teachers { get; private set; }
        /// <summary>
        /// 延伸欄位資訊
        /// </summary>
        protected internal XmlElement Extensions { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public CourseRecord()
        {
            Name = "";
            Teachers = new List<CourseTeacherRecord>();
        }

        /// <summary>
        /// 建構式，傳入課程名稱
        /// </summary>
        /// <param name="Name"></param>
        public CourseRecord(string Name)
        {
            this.Name = Name;
            Teachers = new List<CourseTeacherRecord>();
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="element"></param>
        public CourseRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入課程資料
        /// </summary>
        /// <param name="element"></param>
        public void Load(XmlElement element)
        {
            Teachers = new List<CourseTeacherRecord>();

            ID = element.GetAttribute("ID");
            XmlHelper helper = new XmlHelper(element);
            Name = helper.GetString("CourseName");
            SchoolYear = K12.Data.Int.ParseAllowNull(helper.GetString("SchoolYear"));
            Semester = K12.Data.Int.ParseAllowNull(helper.GetString("Semester"));
            Subject = helper.GetString("Subject");
            Period = K12.Data.Decimal.ParseAllowNull(helper.GetString("Period"));
            Credit = K12.Data.Decimal.ParseAllowNull(helper.GetString("Credit"));
            Level = K12.Data.Decimal.ParseAllowNull(helper.GetString("SubjectLevel"));
            RefClassID = helper.GetString("RefClassID");
            RefAssessmentSetupID = helper.GetString("RefExamTemplateID");
            Domain = helper.GetString("Domain");
            Entry = helper.GetString("ScoreType");
            CalculationFlag = helper.GetString("ScoreCalcFlag");
            RequiredBy = helper.GetString("RequiredBy");
            Required = helper.GetString("IsRequired") == "必"; 
            NotIncludedInCredit = helper.GetString("NotIncludedInCredit") == "是";
            NotIncludedInCalc = helper.GetString("NotIncludedInCalc") == "是";
            Extensions = helper.GetElement("Extensions/Extensions");
            
            MajorTeacherID = helper.GetString("MajorTeacherID");
            MajorTeacherName = helper.GetString("MajorTeacherName");
            MajorTeacherNickname = helper.GetString("MajorTeacherNickname");

            foreach (XmlElement each in helper.GetElements("Teachers/Teacher"))
                Teachers.Add(new CourseTeacherRecord(each));
        }

        #region IComparable<CourseRecord> 成員

        public static event EventHandler<CompareCourseRecordEventArgs> CompareCourseRecord;

        public int CompareTo(CourseRecord other)
        {
            if (CompareCourseRecord != null)
            {
                CompareCourseRecordEventArgs args = new CompareCourseRecordEventArgs(this, other);
                CompareCourseRecord(null, args);
                return args.Result;
            }
            else
            {
                if (this.SchoolYear == other.SchoolYear)
                {
                    if (this.Semester == other.Semester)
                    {
                        return StringComparer.Comparer(this.Name, other.Name);
                    }
                    else
                        return K12.Data.Int.GetString(this.Semester).CompareTo(K12.Data.Int.GetString(other.Semester));
                }
                else return K12.Data.Int.GetString(this.SchoolYear).CompareTo(K12.Data.Int.GetString(other.SchoolYear));
            }
        }

        #endregion
    }

    /// <summary>
    /// 課程授課教師，此類別僅供課程記錄物件用來方便取得授課教師資訊，若需要完整存取教師記錄物件，請使用TeacherRecord物件
    /// </summary>
    public class CourseTeacherRecord
    {
        /// <summary>
        /// 授課教師順序，其中Sequence為1是評分教師。
        /// </summary>
        public int Sequence { get; private set; }
        /// <summary>
        /// 授課教師編號
        /// </summary>
        public string TeacherID { get; private set; }
        /// <summary>
        /// 授課教師名稱
        /// </summary>
        public string TeacherName { get; private set; }
        /// <summary>
        /// 授課教師暱稱
        /// </summary>
        public string TeacherNickname { get; private set; }

        /// <summary>
        /// XML建構式，其規格為<Teacher Sequence="1" TeacherCategory="英文" TeacherID="19" TeacherName="張佳煜" TeacherNickname="" />
        /// </summary>
        /// <param name="data"></param>
        public CourseTeacherRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 運用XML載入
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(XmlElement data)
        {
            Sequence = K12.Data.Int.Parse(data.GetAttribute("Sequence"));
            TeacherID = data.GetAttribute("TeacherID");
            TeacherName = data.GetAttribute("TeacherName");
            TeacherNickname = data.GetAttribute("TeacherNickname");
        }
    }

    public class CompareCourseRecordEventArgs : EventArgs
    {
        internal CompareCourseRecordEventArgs(CourseRecord v1, CourseRecord v2)
        {
            Value1 = v1;
            Value2 = v2;
        }
        public CourseRecord Value1 { get; private set; }
        public CourseRecord Value2 { get; private set; }
        public int Result { get; set; }
    }
}