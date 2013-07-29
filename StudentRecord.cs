using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生資訊
    /// </summary>
    public class StudentRecord
    {
        private string mSAPassword;

        public XmlElement InitialElement { get; private set;}

        public XmlElement DifferenceElement
        {
            get
            {
                FISCA.DSAUtil.DSXmlHelper vDiff = new FISCA.DSAUtil.DSXmlHelper("Difference");

                XmlHelper helper = new XmlHelper(InitialElement);
                
                if (!helper.GetString("Status").Equals(this.Status.ToString()))
                {
                    XmlElement Elm=vDiff.AddElement("Status");
                    Elm.SetAttribute("Before", helper.GetString("Status"));
                    Elm.SetAttribute("After",this.Status.ToString());
                }

                if (!helper.GetString("SeatNo").Equals(this.SeatNo.ToString()))
                {
                    XmlElement Elm = vDiff.AddElement("SeatNo");
                    Elm.SetAttribute("Before", helper.GetString("SeatNo"));
                    Elm.SetAttribute("After", this.SeatNo.ToString());
                }

                if (!helper.GetString("Name").Equals(this.Name))
                {
                    XmlElement Elm = vDiff.AddElement("Name");
                    Elm.SetAttribute("Before", helper.GetString("Name"));
                    Elm.SetAttribute("After", this.Name);
                }

                if (!helper.GetString("StudentNumber").Equals(this.StudentNumber))
                {
                    XmlElement Elm = vDiff.AddElement("StudentNumber");
                    Elm.SetAttribute("Before", helper.GetString("StudentNumber"));
                    Elm.SetAttribute("After", this.StudentNumber);
                }

                if (!helper.GetString("Gender").Equals(this.Gender))
                {
                    XmlElement Elm = vDiff.AddElement("Gender");
                    Elm.SetAttribute("Before", helper.GetString("Gender"));
                    Elm.SetAttribute("After", this.Gender);
                }

                if (!helper.GetString("IDNumber").Equals(this.IDNumber))
                {
                    XmlElement Elm = vDiff.AddElement("IDNumber");
                    Elm.SetAttribute("Before", helper.GetString("IDNumber"));
                    Elm.SetAttribute("After", this.IDNumber);
                }

                if (!helper.GetDateString("Birthdate").Equals(K12.Data.DateTimeHelper.ToDisplayString(this.Birthday.Value)))
                {
                    XmlElement Elm = vDiff.AddElement("Birthdate");
                    Elm.SetAttribute("Before", helper.GetString("Birthdate"));
                    Elm.SetAttribute("After", K12.Data.DateTimeHelper.ToDisplayString(this.Birthday));
                }

 
                if (!helper.GetString("OverrideDeptID").Equals(this.OverrideDepartmentID==null?"":this.OverrideDepartmentID))
                {
                    XmlElement Elm = vDiff.AddElement("OverrideDeptID");
                    Elm.SetAttribute("Before", helper.GetString("OverrideDeptID"));
                    Elm.SetAttribute("After", this.OverrideDepartmentID);
                }

                if (!helper.GetString("RefGraduationPlanID").Equals(this.OverrideProgramPlanID==null?"":this.OverrideProgramPlanID))
                {
                    XmlElement Elm = vDiff.AddElement("RefGraduationPlanID");
                    Elm.SetAttribute("Before", helper.GetString("RefGraduationPlanID"));
                    Elm.SetAttribute("After", this.OverrideProgramPlanID);
                }

                if (!helper.GetString("RefClassID").Equals(this.RefClassID))
                {
                    XmlElement Elm = vDiff.AddElement("RefClassID");
                    Elm.SetAttribute("Before", helper.GetString("RefClassID"));
                    Elm.SetAttribute("After", this.RefClassID);
                }

                if (!helper.GetString("Nationality").Equals(this.Nationality))
                {
                    XmlElement Elm = vDiff.AddElement("Nationality");
                    Elm.SetAttribute("Before", helper.GetString("Nationality"));
                    Elm.SetAttribute("After", this.Nationality);
                }

                return vDiff.BaseElement;
            }
        }

        /// <summary>
        /// 學生狀態
        /// </summary>
        public enum StudentStatus
        {
            一般,延修,畢業或離校,休學,輟學,刪除,轉出,退學
        }

        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string ID { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public StudentStatus Status { get; set; }

        /// <summary>
        /// 狀態，用字串表示，若要指定請使用Status屬性
        /// </summary>
        [Field(Caption = "狀態", EntityName = "Student",EntityCaption ="學生", Remark = "只能填『一般、延修、畢業或離校、休學、輟學、刪除』")]
        public string StatusStr { get { return Status.ToString(); }}
        /// <summary>
        /// 座號
        /// </summary>
        [Field(Caption="座號",EntityName="Student",EntityCaption="學生")]
        public int? SeatNo { get; set; }
        /// <summary>
        /// 姓名，必填欄位
        /// </summary>
        [Field(Caption="姓名",EntityName="Student",EntityCaption="學生")]
        public string Name { get; set; }
        /// <summary>
        /// 英文姓名
        /// </summary>
        [Field(Caption = "英文姓名", EntityName = "Student", EntityCaption = "學生")]
        public string EnglishName { get; set; }
        /// <summary>
        /// 學號
        /// </summary>
        [Field(Caption = "學號", EntityName = "Student", EntityCaption = "學生")]
        public string StudentNumber { get; set; }
        /// <summary>
        /// 性別，必填欄位
        /// </summary>
        [Field(Caption = "性別", EntityName = "Student", EntityCaption = "學生", Remark = "只能填『男、女』")]
        public string Gender { get; set; }
        /// <summary>
        /// 身分證號
        /// </summary>
        [Field(Caption = "身份證號", EntityName = "Student", EntityCaption = "學生")]
        public string IDNumber { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Field(Caption = "生日", EntityName = "Student", EntityCaption = "學生")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        [Field(Caption = "出生地", EntityName = "Student", EntityCaption = "學生")]
        public string BirthPlace { get; set; }
        /// <summary>
        /// 國籍
        /// </summary>
        [Field(Caption = "國籍", EntityName = "Student", EntityCaption = "學生")]
        public string Nationality { get; set; }
        /// <summary>
        /// 覆蓋後的科別編號
        /// </summary>
        protected internal string OverrideDepartmentID { get; set; }
        /// <summary>
        /// 覆蓋後的課程規劃表編號
        /// </summary>
        [Field(Caption = "課程規劃編號（覆蓋後）", EntityName = "Student", EntityCaption = "學生")]
        public string OverrideProgramPlanID { get; set; }
        /// <summary>
        /// 覆蓋後的課程規劃
        /// </summary>
        public ProgramPlanRecord OverrideProgramPlan
        {
            get 
            { 
                return !string.IsNullOrEmpty(OverrideProgramPlanID)?K12.Data.ProgramPlan.SelectByID(OverrideProgramPlanID):null; 
            }
        }

        /// <summary>
        /// 自動判斷所屬課程規劃，若是學生身上有課程規劃，則用學生的，否則取得班級的課程規劃
        /// </summary>
        public ProgramPlanRecord ProgramPlan
        {
            get
            {
                if (!string.IsNullOrEmpty(OverrideProgramPlanID))
                    return OverrideProgramPlan;
                else
                    return Class!=null?Class.ProgramPlan:null;
            }
        }
        /// <summary>
        /// 覆蓋後的成績計算規則編號
        /// </summary>
        [Field(Caption = "成績計算規則編號（覆蓋後）", EntityName = "Student", EntityCaption = "學生")]
        public string OverrideScoreCalcRuleID { get; set; }
        /// <summary>
        /// 覆蓋後的成績計算規則
        /// </summary>
        public ScoreCalcRuleRecord OverrideScoreCalcRule 
        {
            get 
            { 
                return !string.IsNullOrEmpty(OverrideScoreCalcRuleID)?K12.Data.ScoreCalcRule.SelectByID(OverrideScoreCalcRuleID):null;
            } 
        }
        /// <summary>
        /// 自動判斷所屬成績計算規則，若是學生身上有成績計算規則，則用學生的，否則取得班級的課程規劃
        /// </summary>
        public ScoreCalcRuleRecord ScoreCalcRule 
        {
            get
            {
                if (!string.IsNullOrEmpty(OverrideScoreCalcRuleID))
                    return OverrideScoreCalcRule;
                else
                    return Class!=null?Class.ScoreCalcRule:null;
            }
        }
        /// <summary>
        /// 所屬班級編號
        /// </summary>
        [Field(Caption="班級編號",EntityName="Class",EntityCaption="班級",IsEntityPrimaryKey=true)]
        public string RefClassID { get; set; }
        /// <summary>
        /// 所屬班級
        /// </summary>
        public ClassRecord Class
        {
            get
            {
                return !string.IsNullOrEmpty(RefClassID)?K12.Data.Class.SelectByID(RefClassID):null;
            }
        }

        /// <summary>
        /// 帳號類型
        /// </summary>
        [Field(Caption = "帳號類型", EntityName = "Student", EntityCaption = "學生")]
        public string AccountType { get; set; }

        /// <summary>
        /// School Access登入帳號
        /// </summary>
        [Field(Caption = "登入帳號", EntityName = "Student", EntityCaption = "學生")]
        public string SALoginName { get; set; }

        /// <summary>
        /// School Access登入密碼
        /// </summary>
        [Field(Caption = "登入密碼", EntityName = "Student", EntityCaption = "學生")]
        public string SAPassword 
        {
            get { return mSAPassword;}

            set { mSAPassword = string.IsNullOrEmpty(value)?value:K12.Data.Utility.PasswordHash.Compute(value);}
        }

        /// <summary>
        /// 在家自學
        /// </summary>
        [Field(Caption = "在家自學",EntityName ="Student" ,EntityCaption = "學生")]
        public bool HomeSchooling { get; set;}

        /// <summary>
        /// 入學身分
        /// </summary>
        [Field(Caption = "入學身分",EntityName ="Student" ,EntityCaption = "學生")]
        public string EnrollmentCategory { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Field(Caption = "備註",EntityName ="Student" ,EntityCaption = "學生")]
        public string Comment { get; set; }

        /// <summary>
        /// 空建構式，將狀態設為一般、學生名稱為空白。
        /// </summary>
        public StudentRecord()
        {
            Status = StudentStatus.一般;
            Name = "";

        }

        /// <summary>
        /// 參數建構式，用來新增記錄，必填欄位為學生姓名及性別。
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <param name="Gender">性別</param>
        public StudentRecord(string Name,string Gender):this()
        {
            this.Name = Name;
            this.Gender = Gender;
        }

        public StudentRecord(XmlElement element)
        {
            Load(element);
        }

        public void Load(XmlElement element)
        {
            InitialElement = element;

            XmlHelper helper = new XmlHelper(element);
            ID = helper.GetString("@ID");

            string strStatus = helper.GetString("Status");

            if (strStatus.Equals("一般"))
                Status = StudentStatus.一般;
            else if (strStatus.Equals("畢業或離校"))
                Status = StudentStatus.畢業或離校;
            else if (strStatus.Equals("休學"))
                Status = StudentStatus.休學;
            else if (strStatus.Equals("輟學"))
                Status = StudentStatus.輟學;
            else if (strStatus.Equals("延修"))
                Status = StudentStatus.延修;
            else if (strStatus.Equals("刪除"))
                Status = StudentStatus.刪除;
            else if (strStatus.Equals("轉出"))
                Status = StudentStatus.轉出;
            else if (strStatus.Equals("退學"))
                Status = StudentStatus.退學;

            SeatNo = K12.Data.Int.ParseAllowNull(helper.GetString("SeatNo"));
            Name = helper.GetString("Name");
            EnglishName = helper.GetString("EnglishName");
            StudentNumber = helper.GetString("StudentNumber");
            Gender = helper.GetString("Gender");
            IDNumber = helper.GetString("IDNumber");
            Birthday = K12.Data.DateTimeHelper.Parse(helper.GetString("Birthdate"));
            BirthPlace = helper.GetString("BirthPlace");
            OverrideDepartmentID = helper.GetString("OverrideDeptID");
            if (OverrideDepartmentID == "") OverrideDepartmentID = null;
            OverrideProgramPlanID = helper.GetString("RefGraduationPlanID");
            if (OverrideProgramPlanID == "") OverrideProgramPlanID = null;
            OverrideScoreCalcRuleID = helper.GetString("RefScoreCalcRuleID");
            if (OverrideScoreCalcRuleID == "") OverrideScoreCalcRuleID = null;
            RefClassID = helper.GetString("RefClassID");
            Nationality = helper.GetString("Nationality");
            AccountType = helper.GetString("AccountType");
            SALoginName = helper.GetString("SALoginName");
            mSAPassword = helper.GetString("SAPassword");
            EnrollmentCategory = helper.GetString("EnrollmentCategory");
            HomeSchooling = helper.GetString("HomeSchooling").ToLower().StartsWith("t") ? true : false;
            Comment = helper.GetString("Comment");
        }

        #region IComparable<StudentRecord> 成員

        public static event EventHandler<CompareStudentRecordEventArgs> CompareStudentRecord;

        public int CompareTo(StudentRecord other)
        {
            if (CompareStudentRecord != null)
            {
                CompareStudentRecordEventArgs args = new CompareStudentRecordEventArgs(this, other);
                CompareStudentRecord(null, args);
                return args.Result;
            }
            else
            {
                ClassRecord c1 = this.Class;
                ClassRecord c2 = other.Class;
                if (c1 == c2)
                {
                    int seatNo1 = int.MinValue, seatNo2 = int.MinValue;

                    seatNo1 = this.SeatNo.Value;
                    seatNo2 = other.SeatNo.Value;

                    if (seatNo1 == seatNo2)
                        return this.StudentNumber.CompareTo(other.StudentNumber);
                    else
                        return seatNo1.CompareTo(seatNo2);
                }
                else
                {
                    if (c1 == null)
                        return int.MinValue;
                    else if (c2 == null)
                        return int.MaxValue;
                    return c1.CompareTo(c2);
                }
            }


        #endregion
        }
    }


    public class CompareStudentRecordEventArgs : EventArgs
    {
        internal CompareStudentRecordEventArgs(StudentRecord v1, StudentRecord v2)
        {
            Value1 = v1;
            Value2 = v2;
        }
        public StudentRecord Value1 { get; private set; }
        public StudentRecord Value2 { get; private set; }
        public int Result { get; set; }
    }
}