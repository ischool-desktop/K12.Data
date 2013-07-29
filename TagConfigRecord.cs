using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 標籤設定資訊
    /// </summary>
    public class TagConfigRecord : IComparable<TagConfigRecord>, IXmlTransform
    {
        /// <summary>
        /// 預設建構式
        /// </summary>
        public TagConfigRecord()
        {
            Category = "";
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public TagConfigRecord(XmlElement data)
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
            Prefix = data.SelectSingleNode("Prefix").InnerText;
            Name = data.SelectSingleNode("Name").InnerText;
            Category = data.SelectSingleNode("Category").InnerText;

            XmlElement acc = data.SelectSingleNode("AccessControlCode") as XmlElement;
            AccessControlCode = (acc == null ? "" : acc.InnerText);

            int ci;
            if (int.TryParse(data.SelectSingleNode("Color").InnerText, out ci))
                ColorCode = ci;
            else
                ColorCode = System.Drawing.Color.White.ToArgb(); //預設是白色。 
        }

        /// <summary>
        /// 輸出成XML
        /// </summary>
        /// <returns></returns>
        public XmlElement ToXml()
        {
            return null;
        }

        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "TagConfig", EntityCaption = "類別設定", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 前置詞
        /// </summary>
        [Field(Caption = "前置詞", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string Prefix { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string Name { get; set; }
        /// <summary>
        /// 分類，目前適用於Student、Class、Teacher、Course
        /// </summary>
        [Field(Caption = "分類", EntityName = "TagConfig", EntityCaption = "類別設定", Remark = "目前分類有『學生』、『班級』、『教師』、『課程』")]
        public string Category { get; set; }

        /// <summary>
        /// 顏色的原始32位元數字
        /// </summary>
        [Field(Caption = "顏色代碼", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public int ColorCode { get; set; }

        /// <summary>
        /// 顏色
        /// </summary>
        public System.Drawing.Color Color
        {
            get { return System.Drawing.Color.FromArgb(ColorCode); }
            set { ColorCode = value.ToArgb(); }
        }

        /// <summary>
        /// 權限存取代碼。
        /// </summary>
        [Field(Caption = "權限代碼", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string AccessControlCode { get; set; }

        /// <summary>
        /// 完整名稱
        /// </summary>
        [Field(Caption = "完整名稱", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Prefix))
                    return Name;
                else
                    return string.Format("{0}:{1}", Prefix, Name);
            }
        }

        #region IComparable<TagRecord> 成員

        public int CompareTo(TagConfigRecord other)
        {
            return Prefix.CompareTo(other.Prefix);
        }

        #endregion
    }
}