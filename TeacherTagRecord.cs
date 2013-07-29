
namespace K12.Data
{
    /// <summary>
    /// 教師標籤資訊
    /// </summary>
    public class TeacherTagRecord : GeneralTagRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public TeacherTagRecord()
        {
 
        }

        /// <summary>
        /// 新增用建構式
        /// </summary>
        /// <param name="RefEntityID">RefEntityID，教師編號</param>
        /// <param name="RefTagID">RefTagID，標籤編號</param>
        public TeacherTagRecord(string RefEntityID, string RefTagID)
        {
            this.RefEntityID = RefEntityID;
            this.RefTagID = RefTagID;
        }

        /// <summary>
        /// 取得教師標籤編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("@TagTeacherID").InnerText;
        }

        /// <summary>
        /// 取得教師編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetEntityID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("TeacherID").InnerText;
        }

        /// <summary>
        /// 所屬教師編號
        /// </summary>
        [Field(Caption = "教師編號", EntityName = "Teacher", EntityCaption = "教師",IsEntityPrimaryKey=true)]
        public string RefTeacherID { get { return RefEntityID; } set { RefEntityID = value; } }

        /// <summary>
        /// 取得所屬教師
        /// </summary>
        public TeacherRecord Teacher
        {
            get 
            { 
                return !string.IsNullOrEmpty(RefEntityID)?K12.Data.Teacher.SelectByID(RefEntityID):null;
            } 
        }
    }
}