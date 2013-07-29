using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學校資訊記錄物件
    /// </summary>
    public class SchoolRecord
    {
        /// <summary>
        /// 縣市名稱
        /// </summary>
        [Field(Caption = "縣市名稱", EntityName = "School", EntityCaption = "學校")]
        public string County { get; set; }

        /// <summary>
        /// 學校代碼
        /// </summary>
        [Field(Caption = "代碼", EntityName = "School", EntityCaption = "學校")]
        public string Code { get; set; }

        /// <summary>
        /// 學校名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "School", EntityCaption = "學校")]
        public string Name { get; set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SchoolRecord()
        {

        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            if (data != null)
            {
                Name = data.GetAttribute("Name");
                Code = data.GetAttribute("Code");
                County = data.GetAttribute("County");
            }
        }
    }
}