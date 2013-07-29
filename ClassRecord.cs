using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 班級資訊
    /// </summary>
    public class ClassRecord : IComparable<ClassRecord>
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Class", EntityCaption = "班級", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "Class", EntityCaption = "班級")]
        public string Name { get; set; }
        /// <summary>
        /// 年級
        /// </summary>
        [Field(Caption = "年級", EntityName = "Class", EntityCaption = "班級")]
        public int? GradeYear { get; set; }
        /// <summary>
        /// 命名規則，班級升級用，如果設定成「資{一、二、三}甲」，資 X 甲的 X 會依據班級的年級自動改變班級名稱
        /// </summary>
        [Field(Caption = "命名規則", EntityName = "Class", EntityCaption = "班級")]
        public string NamingRule { get; set; }
        /// <summary>
        /// 班導師編號
        /// </summary>
        [Field(Caption = "教師編號（班導師）", EntityName = "Teacher", EntityCaption = "教師", IsEntityPrimaryKey = true)]
        public string RefTeacherID { get; set; }
        /// <summary>
        /// 課程規劃編號
        /// </summary>
        [Field(Caption = "課程規劃編號", EntityName = "ProgramPlan", EntityCaption = "課程規劃", IsEntityPrimaryKey = true)]
        public string RefProgramPlanID { get; set; }
        /// <summary>
        /// 成績計算規則編號
        /// </summary>
        [Field(Caption = "成績計算規則編號", EntityName = "ScoreCalcRule", EntityCaption = "成績計算", IsEntityPrimaryKey = true)]
        public string RefScoreCalcRuleID { get; set; }
        /// <summary>
        /// 顯示順序
        /// </summary>
        [Field(Caption = "顯示順序", EntityName = "Class", EntityCaption = "班級")]
        public string DisplayOrder { get; set; }
        /// <summary>
        /// 科別編號
        /// </summary>
        protected string RefDepartmentID { get; set; }

        /// <summary>
        /// 班導師
        /// </summary>
        public TeacherRecord Teacher
        {
            get
            {
                return !string.IsNullOrEmpty(RefTeacherID)?K12.Data.Teacher.SelectByID(RefTeacherID):null;
            }
        }

        /// <summary>
        /// 所屬課程規劃
        /// </summary>
        public ProgramPlanRecord ProgramPlan
        {
            get { return !string.IsNullOrEmpty(RefProgramPlanID)?K12.Data.ProgramPlan.SelectByID(RefProgramPlanID):null; }
        }

        /// <summary>
        /// 所屬成績計算規則
        /// </summary>
        public ScoreCalcRuleRecord ScoreCalcRule
        {
            get { return !string.IsNullOrEmpty(RefScoreCalcRuleID)?K12.Data.ScoreCalcRule.SelectByID(RefScoreCalcRuleID):null; }
        }

        /// <summary>
        /// 取得班級學生
        /// </summary>
        public List<StudentRecord> Students
        {
            get
            {
                return !string.IsNullOrEmpty(this.ID)?Student.SelectByClassID(this.ID):new List<StudentRecord>();
            }
        }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassRecord()
        {
            Name = "";
        }

        /// <summary>
        /// 新增班級記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="Name">班級名稱</param>
        public ClassRecord(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Xml參數建構式
        /// </summary>
        /// <param name="element"></param>
        public ClassRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 載入XML方法
        /// </summary>
        /// <param name="element"></param>
        public void Load(XmlElement element)
        {
            ID = element.GetAttribute("ID");
            XmlHelper helper = new XmlHelper(element);
            Name = helper.GetString("ClassName");
            GradeYear = K12.Data.Int.ParseAllowNull(helper.GetString("GradeYear"));
            NamingRule = helper.GetString("NamingRule");
            RefTeacherID = helper.GetString("RefTeacherID");
            RefProgramPlanID = helper.GetString("RefGraduationPlanID");
            DisplayOrder = helper.GetString("DisplayOrder");
            RefDepartmentID = helper.GetString("RefDepartmentID");
            RefScoreCalcRuleID = helper.GetString("RefScoreCalcRuleID");
        }

        #region IComparable<ClassRecord> 成員

        public static event EventHandler<CompareClassRecordEventArgs> CompareClassRecord;

        public int CompareTo(ClassRecord other)
        {
            if (CompareClassRecord != null)
            {
                CompareClassRecordEventArgs args = new CompareClassRecordEventArgs(this, other);
                CompareClassRecord(null, args);
                return args.Result;
            }
            else
            {
                int g1 = int.MinValue, g2 = int.MinValue;
                //int.TryParse(this.GradeYear.Trim(), out g1);
                //int.TryParse(other.GradeYear.Trim(), out g2);
                if (g1 == g2)
                {
                    int order1 = int.MinValue, order2 = int.MinValue;
                    int.TryParse(this.DisplayOrder, out order1);
                    int.TryParse(other.DisplayOrder, out order2);
                    if (order1 == order2)
                    {
                        return StringComparer.Comparer(this.Name, other.Name);
                    }
                    else
                        return order1.CompareTo(order2);
                }
                else
                    return g1.CompareTo(g2);
            }
        }

        #endregion
    }
    public class CompareClassRecordEventArgs : EventArgs
    {
        internal CompareClassRecordEventArgs(ClassRecord v1, ClassRecord v2)
        {
            Value1 = v1;
            Value2 = v2;
        }
        public ClassRecord Value1 { get; private set; }
        public ClassRecord Value2 { get; private set; }
        public int Result { get; set; }
    }
}