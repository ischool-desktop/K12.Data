
namespace K12.Data
{
    /// <summary>
    /// 課程標籤資訊
    /// </summary>
    public class CourseTagRecord : GeneralTagRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public CourseTagRecord()
        {
 
        }

        /// <summary>
        /// 新增用建構式
        /// </summary>
        /// <param name="RefEntityID">RefEntityID，班級編號</param>
        /// <param name="RefTagID">RefTagID，標籤編號</param>
        public CourseTagRecord(string RefEntityID, string RefTagID)
        {
            this.RefEntityID = RefEntityID;
            this.RefTagID = RefTagID;
        }

        /// <summary>
        /// 取得課程標籤編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("@TagCourseID").InnerText;
        }

        /// <summary>
        /// 取得課程編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetEntityID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("CourseID").InnerText;
        }

        /// <summary>
        /// 所屬課程編號
        /// </summary>
        [Field(Caption = "課程編號", EntityName = "Course", EntityCaption = "課程",IsEntityPrimaryKey=true)]
        public string RefCourseID { get { return RefEntityID; } set { RefEntityID = value; } }
        
        /// <summary>
        /// 取得所屬課程
        /// </summary>
        public CourseRecord Course 
        { 
            get 
            {
                return !string.IsNullOrEmpty(RefEntityID)?K12.Data.Course.SelectByID(RefEntityID):null; 
            }
        }
    }
}