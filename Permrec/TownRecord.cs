using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 縣市鄉鎮記錄
    /// </summary>
    public class TownRecord
    {
        /// <summary>
        /// 縣市名稱
        /// </summary>
        [Field(Caption = "縣市名稱", EntityName = "Town", EntityCaption = "縣市鄉鎮")]
        public string County { get; private set; }
        /// <summary>
        /// 鄉鎮名稱
        /// </summary>
        [Field(Caption = "鄉鎮名稱", EntityName = "Town", EntityCaption = "縣市鄉鎮")]
        public string Area { get; private set; }
        /// <summary>
        /// 郵遞區號
        /// </summary>
        [Field(Caption = "郵遞區號", EntityName = "Town", EntityCaption = "縣市鄉鎮")]
        public string ZipCode { get; private set; }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public TownRecord(XmlElement data)
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
            County = Area = ZipCode = string.Empty;

            if (data == null) return;

            XmlHelper xdata = new XmlHelper(data);

            County = xdata.GetString("@County");
            Area = xdata.GetString("@Name");
            ZipCode = xdata.GetString("@Code");
        }
    }
}