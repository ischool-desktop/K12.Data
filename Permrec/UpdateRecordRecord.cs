using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生異動記錄資訊
    /// </summary>
    public class UpdateRecordRecord
    {
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public UpdateRecordRecord()
        {
            Attributes = new AutoDictionary();
        }

        /// <summary>
        /// 新增學生異動記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="StudentID">所屬學生編號</param>
        /// <param name="UpdateDate">異動日期</param>
        public UpdateRecordRecord(string StudentID,string UpdateDate)
        {
            this.StudentID = StudentID;
            this.UpdateDate = UpdateDate;
            Attributes = new AutoDictionary();
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="data"></param>
        public UpdateRecordRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 從XML載入設定
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            XmlHelper helper = new XmlHelper(data);

            StudentID = data.GetAttribute("RefStudentID");
            ADDate = helper.GetDateString("ADDate");
            ADNumber = helper.GetString("ADNumber");
            Birthdate = helper.GetDateString("Birthdate");
            Comment = helper.GetString("Comment");
            Gender = helper.GetString("Gender");
            GradeYear = helper.GetString("GradeYear");
            ID = helper.GetString("@ID");
            IDNumber = helper.GetString("IDNumber");
            LastADDate = helper.GetDateString("LastADDate");
            LastADNumber = helper.GetString("LastADNumber");
            StudentName = helper.GetString("Name");
            StudentNumber = helper.GetString("StudentNumber");
            UpdateCode = helper.GetString("UpdateCode");
            UpdateDate = helper.GetDateString("UpdateDate");
            UpdateDescription = helper.GetString("UpdateDescription");
            Department = helper.GetString("Department");
            SchoolYear = K12.Data.Int.ParseAllowNull(helper.GetString("SchoolYear"));
            Semester = K12.Data.Int.ParseAllowNull(helper.GetString("Semester"));
            
            Attributes = new AutoDictionary(data.SelectSingleNode("ContextInfo/ContextInfo") as XmlElement, false);
        }


        /// <summary>
        /// 異動編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "UpdateRecord", EntityCaption = "異動", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 學生編號，必填
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string StudentID { get; set; }
        /// <summary>
        /// 異動日期，必填
        /// </summary>
        [Field(Caption = "日期", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string UpdateDate { get; set; }
        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student 
        {
            get
            {
                return !string.IsNullOrEmpty(StudentID)?K12.Data.Student.SelectByID(StudentID):null; 
            } 
        }
        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Caption = "學年度", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public int? SchoolYear { get; set; }
        /// <summary>
        /// 學期
        /// </summary>
        [Field(Caption = "學期", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public int? Semester { get; set; }
        /// <summary>
        /// 核準日期(回填)
        /// </summary>
        [Field(Caption = "核準日期（回填）", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string ADDate { get; set; }
        /// <summary>
        /// 核準文號(回填)
        /// </summary>
        [Field(Caption = "核準文號（回填）", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string ADNumber { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Field(Caption = "生日", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string Birthdate { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Field(Caption = "備註", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string Comment { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        [Field(Caption = "性別", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string Gender { get; set; }
        /// <summary>
        /// 年級
        /// </summary>
        [Field(Caption = "年級", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string GradeYear { get; set; }
        /// <summary>
        /// 身份證號
        /// </summary>
        [Field(Caption = "身份證字號", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string IDNumber { get; set; }
        /// <summary>
        /// 最後核準日期
        /// </summary>
        [Field(Caption = "最後核準日期", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string LastADDate { get; set; }
        /// <summary>
        /// 最後核準文號
        /// </summary>
        [Field(Caption = "最後核準文號", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string LastADNumber { get; set; }
        /// <summary>
        /// 學生姓名
        /// </summary>
        [Field(Caption = "學生姓名", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string StudentName { get; set; }
        /// <summary>
        /// 學生學號
        /// </summary>
        [Field(Caption = "學生學號", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string StudentNumber { get; set; }
        /// <summary>
        /// 異動代碼，高中異動代碼對照表請參考SHSchool.Data.UpdateCodeMapping、SHSchool.Data.UpdateCodeMappingInfo。
        /// </summary>
        [Field(Caption = "代碼", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string UpdateCode { get; set; }
        /// <summary>
        /// 異動原因，高中一般參考對照表內的異動原因，高中異動代碼對照表請參考SHSchool.Data.UpdateCodeMapping、SHSchool.Data.UpdateCodeMappingInfo。
        /// </summary>
        [Field(Caption = "原因", EntityName = "UpdateRecord", EntityCaption = "異動")]
        public string UpdateDescription { get; set; }        
        /// <summary>
        /// 異動科別
        /// </summary>
        protected internal string Department {get; set;}

        /// <summary>
        /// 擴充欄位
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法僅供ischool內部開發人員使用。")]
        public AutoDictionary Attributes { get; set; }
   }
}