using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生修課資訊
    /// </summary>
    public class SCAttendRecord
    {
        /// <summary>
        /// 所屬學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get;  set; }
        /// <summary>
        /// 所屬課程編號，必填
        /// </summary>
        [Field(Caption = "課程編號", EntityName = "Course", EntityCaption = "課程", IsEntityPrimaryKey = true)]
        public string RefCourseID { get;  set; }
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
        /// 所屬課程
        /// </summary>
        public CourseRecord Course 
        { 
            get
            { 
                return !string.IsNullOrEmpty(RefCourseID)?K12.Data.Course.SelectByID(RefCourseID):null; 
            } 
        }
        /// <summary>
        /// 修課總成績
        /// </summary>
        [Field(Caption = "總成績", EntityName = "SCAttend", EntityCaption = "學生修課")]
        public decimal? Score { get;  set; }
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "SCAttend", EntityCaption = "學生修課",IsEntityPrimaryKey=true)]
        public string ID { get;  set; }
        /// <summary>
        /// 修課努力程度
        /// </summary>
        protected internal int? Effort { get; set; }
        /// <summary>
        /// 修課文字描述
        /// </summary>
        protected internal string Text { get; set; }
        /// <summary>
        /// 平時評量努力程度
        /// </summary>
        protected internal int? OrdinarilyEffort { get; set;}
        /// <summary>
        /// 平時評量分數
        /// </summary>
        protected internal decimal? OrdinarilyScore { get; set; }
        /// <summary>
        /// 取得校部定
        /// </summary>
        protected internal string RequiredBy { get { return this.OverrideRequiredBy == null ? Course.RequiredBy : OverrideRequiredBy; } }
        /// <summary>
        /// 取得必選修
        /// </summary>
        protected internal bool Required { get { return this.OverrideRequired == null ? Course.Required:this.OverrideRequired.GetValueOrDefault(); } }
        /// <summary>
        /// 取得，指出是否覆蓋課程的必選修資訊
        /// </summary>
        protected internal bool? OverrideRequired { get; set; }
        /// <summary>
        /// 取得，指出是否覆蓋課程的校部訂資訊
        /// </summary>
        protected internal string OverrideRequiredBy { get; set; }
        /// <summary>
        /// 延伸欄位資訊
        /// </summary>
        protected internal XmlElement Extensions { get; set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SCAttendRecord()
        {
 
        }

        /// <summary>
        /// 新增學生修課記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefStudentID">所屬學生編號</param>
        /// <param name="RefCourseID">所屬課程編號</param>
        public SCAttendRecord(string RefStudentID,string RefCourseID)
        {
            this.RefStudentID = RefStudentID;
            this.RefCourseID = RefCourseID;
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
            RefCourseID = helper.GetString("RefCourseID");
            Score = K12.Data.Decimal.ParseAllowNull(helper.GetString("Score"));
            ID = element.GetAttribute("ID");
            Effort = K12.Data.Int.ParseAllowNull(helper.GetString("Extension/Extension/Effort"));
            Text = helper.GetString("Extension/Extension/Text");
            OrdinarilyEffort = K12.Data.Int.ParseAllowNull(helper.GetString("Extension/Extension/OrdinarilyEffort"));
            OrdinarilyScore = K12.Data.Decimal.ParseAllowNull(helper.GetString("Extension/Extension/OrdinarilyScore"));

            Extensions = helper.GetElement("Extensions/Extensions");

            //下面邏輯待確認
            switch (helper.GetString("IsRequired"))
            {
                case "必":
                    OverrideRequired = true;
                    break;
                case "選":
                    OverrideRequired = false;
                    break;
                default:
                    OverrideRequired = null;
                    break;
            }
            switch (helper.GetString("RequiredBy"))
            {
                case "部訂":
                case "校訂":
                    OverrideRequiredBy = helper.GetString("RequiredBy");
                    break;
                default:
                    OverrideRequiredBy = null;
                    break;
            }
        }
    }
}