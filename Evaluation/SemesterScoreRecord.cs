using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生學期成績資訊
    /// </summary>
    public class SemesterScoreRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "SemesterScore", EntityCaption = "學期成績", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 所屬學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; set; }
        /// <summary>
        /// 學年度，必填
        /// </summary>
        [Field(Caption = "學年度", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int SchoolYear { get; set; }
        /// <summary>
        /// 學期，必填
        /// </summary>
        [Field(Caption = "學期", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int Semester { get; set; }
        /// <summary>
        /// 年級，必填
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("學期成績上的成績年級已不再使用。")]
        [Field(Caption = "年級", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int GradeYear { get; set; }
        /// <summary>
        /// 學期課程學習成績，由ischool介面所計算
        /// </summary>
        [Field(Caption = "課程學習成績", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public decimal? CourseLearnScore { get; set; }
        /// <summary>
        /// 學期學習領域成績，由ischool介面所計算
        /// </summary>
        protected internal decimal? LearnDomainScore { get; set; }
        /// <summary>
        /// 實中學期平均成績
        /// </summary>
        public decimal? AvgScore { get; set; }
        /// <summary>
        /// 實中學期平均成績GPA
        /// </summary>
        public decimal? AvgGPA { get; set; }
        /// <summary>
        /// 實中學期累計成績GPA
        /// </summary>
        public decimal? CumulateGPA { get; set; }
        /// <summary>
        /// 學期科目成績明細
        /// </summary>
        public Dictionary<string, SubjectScore> Subjects { get; set; }
        /// <summary>
        /// 學期領域成績明細
        /// </summary>
        protected internal Dictionary<string, DomainScore> Domains { get; set; }
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
        /// 預設建構式，初始化Subjects及Domains
        /// </summary>
        public SemesterScoreRecord()
        {
            Subjects = new Dictionary<string, SubjectScore>();
            Domains = new Dictionary<string, DomainScore>();
        }

        /// <summary>
        /// 新增學生學期成績記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        public SemesterScoreRecord(string RefStudentID, int SchoolYear, int Semester)
            : this()
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
        public SemesterScoreRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public virtual void Load(XmlElement element)
        {
            XmlHelper helper = new XmlHelper(element);

            ID = helper.GetString("@ID");
            RefStudentID = helper.GetString("RefStudentId");
            SchoolYear = helper.GetInteger("SchoolYear", 0);
            Semester = helper.GetInteger("Semester", 0);
            GradeYear = helper.GetInteger("GradeYear", 0);
            LearnDomainScore = K12.Data.Decimal.ParseAllowNull(helper.GetString("ScoreInfo/LearnDomainScore"));
            CourseLearnScore = K12.Data.Decimal.ParseAllowNull(helper.GetString("ScoreInfo/CourseLearnScore"));
            AvgScore = K12.Data.Decimal.ParseAllowNull(helper.GetString("ScoreInfo/AvgScore"));
            AvgGPA = K12.Data.Decimal.ParseAllowNull(helper.GetString("ScoreInfo/AvgGPA"));
            CumulateGPA = K12.Data.Decimal.ParseAllowNull(helper.GetString("ScoreInfo/CumulateGPA"));

            Subjects = new Dictionary<string, SubjectScore>();
            foreach (var subjectElement in helper.GetElements("ScoreInfo/SemesterSubjectScoreInfo/Subject"))
            {
                subjectElement.SetAttribute("ID", ID);
                subjectElement.SetAttribute("RefStudentID", RefStudentID);
                subjectElement.SetAttribute("SchoolYear", K12.Data.Int.GetString(SchoolYear));
                subjectElement.SetAttribute("Semester", K12.Data.Int.GetString(Semester));

                SubjectScore subjectScore = new SubjectScore(subjectElement);

                if (!Subjects.ContainsKey(subjectScore.Subject))
                    Subjects.Add(subjectScore.Subject, subjectScore);
            }

            Domains = new Dictionary<string, DomainScore>();
            foreach (var domainElement in helper.GetElements("ScoreInfo/Domains/Domain"))
            {
                domainElement.SetAttribute("ID", ID);
                domainElement.SetAttribute("RefStudentID", RefStudentID);
                domainElement.SetAttribute("SchoolYear", K12.Data.Int.GetString(SchoolYear));
                domainElement.SetAttribute("Semester", K12.Data.Int.GetString(Semester));

                DomainScore domainScore = new DomainScore(domainElement);

                if (!Domains.ContainsKey(domainScore.Domain))
                    Domains.Add(domainScore.Domain, domainScore);
            }
        }
    }

    /// <summary>
    /// 科目成績
    /// </summary>
    public class SubjectScore : ICloneable
    {
        /// <summary>
        /// 所屬學期成績編號，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "編號", EntityName = "SemesterScore", EntityCaption = "學期成績", IsEntityPrimaryKey = true)]
        public string RefSemesterScoreID { get; private set; }

        /// <summary>
        /// 所屬學生編號，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; private set; }

        /// <summary>
        /// 學年度，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學年度", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int SchoolYear { get; private set; }

        /// <summary>
        /// 學期，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學期", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int Semester { get; private set; }

        /// <summary>
        /// 所屬領域
        /// </summary>
        [Field(Caption = "領域", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public string Domain { get; set; } 
        /// <summary>
        /// 科目名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public string Subject { get; set; }
        /// <summary>
        /// 上課時段
        /// </summary>
        [Field(Caption = "上課時段", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public decimal? Period { get; set; }
        /// <summary>
        /// 權數
        /// </summary>
        [Field(Caption = "權數", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public decimal? Credit { get; set; }
        /// <summary>
        /// 百分比成績
        /// </summary>
        [Field(Caption = "成績", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public decimal? Score { get; set; }
        /// <summary>
        /// 努力程度
        /// </summary>
        [Field(Caption = "努力程度", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public int? Effort { get; set; }
        /// <summary>
        /// 文字評量
        /// </summary>
        [Field(Caption = "文字評量", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public string Text { get; set; }
        /// <summary>
        /// 註解
        /// </summary>
        [Field(Caption = "註解", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public string Comment { get; set; }
        /// <summary>
        /// GPA
        /// </summary>
        [Field(Caption = "GPA", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public decimal? GPA { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [Field(Caption = "Type", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public string Type { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        [Field(Caption = "Level", EntityName = "SubjectScore", EntityCaption = "科目成績")]
        public int? Level { get; set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SubjectScore()
        {

        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="subject"></param>
        public SubjectScore(XmlElement subject)
        {
            //SemesterScore屬性
            RefSemesterScoreID = subject.GetAttribute("ID");
            RefStudentID = subject.GetAttribute("RefStudentID");
            SchoolYear = K12.Data.Int.Parse(subject.GetAttribute("SchoolYear"));
            Semester = K12.Data.Int.Parse(subject.GetAttribute("Semester"));

            //SubjectScore屬性
            Domain = subject.GetAttribute("領域");
            Subject = subject.GetAttribute("科目");
            Period = K12.Data.Decimal.ParseAllowNull(subject.GetAttribute("節數"));
            Credit = K12.Data.Decimal.ParseAllowNull(subject.GetAttribute("權數"));
            Score = K12.Data.Decimal.ParseAllowNull(subject.GetAttribute("成績"));
            Effort = K12.Data.Int.ParseAllowNull(subject.GetAttribute("努力程度"));
            Text = subject.GetAttribute("文字描述");
            Comment = subject.GetAttribute("註記");
            GPA = K12.Data.Decimal.ParseAllowNull(subject.GetAttribute("GPA"));
            Level = K12.Data.Int.ParseAllowNull(subject.GetAttribute("Level"));
            Type = subject.GetAttribute("Type");
        }

        #region ICloneable 成員

        /// <summary>
        /// 複製科目成績物件
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SubjectScore cloneSubjectScore = new SubjectScore();
            cloneSubjectScore.Domain = this.Domain;
            cloneSubjectScore.Subject = this.Subject;
            cloneSubjectScore.Period = this.Period;
            cloneSubjectScore.Credit = this.Credit;
            cloneSubjectScore.Score = this.Score;
            cloneSubjectScore.Effort = this.Effort;
            cloneSubjectScore.Text = this.Text;
            cloneSubjectScore.Comment = this.Comment;
            cloneSubjectScore.GPA = this.GPA;
            cloneSubjectScore.Level = this.Level;
            cloneSubjectScore.Type = this.Type;
            return cloneSubjectScore;
        }

        #endregion
    }

    /// <summary>
    /// 領域成績
    /// </summary>
    public class DomainScore : ICloneable
    {
        /// <summary>
        /// 所屬學期成績編號，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "編號", EntityName = "SemesterScore", EntityCaption = "學期成績", IsEntityPrimaryKey = true)]
        public string RefSemesterScoreID { get; private set; }

        /// <summary>
        /// 所屬學生編號，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; private set; }

        /// <summary>
        /// 學年度，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學年度", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int SchoolYear { get; private set; }

        /// <summary>
        /// 學期，此為唯讀屬性。
        /// </summary>
        [Field(Caption = "學期", EntityName = "SemesterScore", EntityCaption = "學期成績")]
        public int Semester { get; private set; }

        /// <summary>
        /// 所屬領域
        /// </summary>
        [Field(Caption = "領域", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public string Domain { get; set; }
        /// <summary>
        /// 上課時段
        /// </summary>
        [Field(Caption = "上課時段", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public decimal? Period { get; set; }
        /// <summary>
        /// 權數
        /// </summary>
        [Field(Caption = "權數", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public decimal? Credit { get; set; }
        /// <summary>
        /// 百分比成績
        /// </summary>
        [Field(Caption = "成績", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public decimal? Score { get; set; }
        /// <summary>
        /// 努力程度
        /// </summary>
        [Field(Caption = "努力程度", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public int? Effort { get; set; }
        /// <summary>
        /// 文字評量
        /// </summary>
        [Field(Caption = "文字評量", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public string Text { get; set; }
        /// <summary>
        /// 註解
        /// </summary>
        [Field(Caption = "註解", EntityName = "DomainScore", EntityCaption = "領域成績")]
        public string Comment { get; set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public DomainScore()
        {

        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="domain"></param>
        public DomainScore(XmlElement domain)
        {
            //SemesterScore屬性
            RefSemesterScoreID = domain.GetAttribute("ID");
            RefStudentID = domain.GetAttribute("RefStudentID");
            SchoolYear = K12.Data.Int.Parse(domain.GetAttribute("SchoolYear"));
            Semester = K12.Data.Int.Parse(domain.GetAttribute("Semester"));

            //DomainScore屬性
            Domain = domain.GetAttribute("領域");
            Period = K12.Data.Decimal.ParseAllowNull(domain.GetAttribute("節數"));
            Credit = K12.Data.Decimal.ParseAllowNull(domain.GetAttribute("權數"));
            Score = K12.Data.Decimal.ParseAllowNull(domain.GetAttribute("成績"));
            Effort = K12.Data.Int.ParseAllowNull(domain.GetAttribute("努力程度"));
            Text = domain.GetAttribute("文字描述");
            Comment = domain.GetAttribute("註記");
        }

        #region ICloneable 成員

        /// <summary>
        /// 複製領域成績物件
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            DomainScore cloneDomainScore = new DomainScore();
            cloneDomainScore.Domain = this.Domain;
            cloneDomainScore.Period = this.Period;
            cloneDomainScore.Credit = this.Credit;
            cloneDomainScore.Score = this.Score;
            cloneDomainScore.Effort = this.Effort;
            cloneDomainScore.Text = this.Text;
            cloneDomainScore.Comment = this.Comment;
            return cloneDomainScore;
        }

        #endregion
    }
}