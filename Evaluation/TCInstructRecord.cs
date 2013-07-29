using System.Xml;
using FISCA.DSAUtil;

namespace K12.Data
{
    /// <summary>
    /// 教師教授課程資訊
    /// </summary>
    public class TCInstructRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "TCInstruct", EntityCaption = "教師授課", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 教師順序，必填
        /// </summary>
        [Field(Caption = "順序", EntityName = "TCInstruct", EntityCaption = "教師授課", Remark = "1代表為課程的主要授課教師，可以對學生進行評分。")]
        public int Sequence { get; set; }
        /// <summary>
        /// 所屬授課教師編號，必填
        /// </summary>
        [Field(Caption = "教師編號", EntityName = "Teacher", EntityCaption = "教師", IsEntityPrimaryKey = true)]
        public string RefTeacherID { get;  set; }
        /// <summary>
        /// 所屬課程編號，必填
        /// </summary>
        [Field(Caption = "課程編號", EntityName = "Course", EntityCaption = "課程", IsEntityPrimaryKey = true)]
        public string RefCourseID { get;  set; }
        /// <summary>
        /// 所屬教師
        /// </summary>
        public TeacherRecord Teacher 
        { 
            get
            {
                return !string.IsNullOrEmpty(RefTeacherID)?K12.Data.Teacher.SelectByID(RefTeacherID):null;
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
        /// 新增教師授課記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        ///<param name="RefTeacherID">所屬授課教師編號</param>
        ///<param name="RefCourseID">所屬課程編號</param>
        ///<param name="Sequence">教師順序</param>
        public TCInstructRecord(string RefTeacherID,string RefCourseID,int Sequence)
        {
            this.RefTeacherID = RefTeacherID;
            this.RefCourseID = RefCourseID;
            this.Sequence = Sequence;
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public TCInstructRecord()
        {
 
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public TCInstructRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            ID = data.GetAttribute("ID");
            DSXmlHelper helper =new DSXmlHelper(data);
            RefTeacherID = helper.GetText("RefTeacherID");
            RefCourseID = helper.GetText("RefCourseID");
            Sequence = K12.Data.Int.Parse(helper.GetText("Sequence"));
        }
    }
}