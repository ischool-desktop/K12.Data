
namespace K12.Data
{
    /// <summary>
    /// 班級標籤資訊
    /// </summary>
    public class ClassTagRecord : GeneralTagRecord
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public ClassTagRecord()
        {
 
        }

        /// <summary>
        /// 新增用建構式
        /// </summary>
        /// <param name="RefEntityID">RefEntityID，班級編號</param>
        /// <param name="RefTagID">RefTagID，標籤編號</param>
        public ClassTagRecord(string RefEntityID, string RefTagID)
        {
            this.RefEntityID = RefEntityID;
            this.RefTagID = RefTagID;
        }

        /// <summary>
        /// 取得班級標籤編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("@TagClassID").InnerText;
        }

        /// <summary>
        /// 取得班級編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string GetEntityID(System.Xml.XmlElement data)
        {
            return data.SelectSingleNode("ClassID").InnerText;
        }

        /// <summary>
        /// 所屬班級編號
        /// </summary>
        [Field(Caption = "班級編號", EntityName = "Class", EntityCaption = "班級",IsEntityPrimaryKey=true)]
        public string RefClassID { get { return RefEntityID; } set { RefEntityID = value; } }

        /// <summary>
        /// 所屬班級記錄物件
        /// </summary>
        public ClassRecord Class
        {
            get
            {
                return !string.IsNullOrEmpty(RefEntityID)?K12.Data.Class.SelectByID(RefEntityID):null;
            }
        }
    }
}