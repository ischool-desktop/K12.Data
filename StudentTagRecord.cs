using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生標籤資訊
    /// </summary>
    public class StudentTagRecord : GeneralTagRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public StudentTagRecord()
        {
 
        }

        /// <summary>
        /// 新增用建構式
        /// </summary>
        /// <param name="RefEntityID">RefEntityID，可能為學生編號、班級編號、課程編號及教師編號</param>
        /// <param name="RefTagID">RefTagID，標籤編號</param>
        public StudentTagRecord(string RefEntityID, string RefTagID)
        {
            this.RefEntityID = RefEntityID;
            this.RefTagID = RefTagID;
        }

        /// <summary>
        /// 取得學生標籤編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetID(XmlElement data)
        {
            return data.SelectSingleNode("@TagStudentID").InnerText;
        }

        /// <summary>
        /// 取得學生編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetEntityID(XmlElement data)
        {
            return data.SelectSingleNode("StudentID").InnerText;
        }

        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get { return RefEntityID; } set { RefEntityID = value; } }

        /// <summary>
        /// 取得所屬學生
        /// </summary>
        public StudentRecord Student 
        { 
            get 
            { 
                return !string.IsNullOrEmpty(RefEntityID)?K12.Data.Student.SelectByID(RefEntityID):null;
            } 
        }
    }
}