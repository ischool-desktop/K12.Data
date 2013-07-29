using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 標籤系統，用來分類Student、Class、Teacher及Course，先運用TagConfig及TagConfigRecord來
    /// 設定標籤值，再套用至四個Entity上。
    /// </summary>
    public abstract class GeneralTagRecord:IXmlTransform
    {
        /// <summary>
        /// 載入XML參數設定值
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(XmlElement data)
        {
            RefTagID = data.SelectSingleNode("RefTagID").InnerText;
            RefEntityID = GetEntityID(data); //每個 Entity  的  Element Name 不同。
            ID = GetID(data);
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
        /// ID，系統編號
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GetID(XmlElement data);

        /// <summary>
        /// EntityID 屬性的名稱，每個 Entity 都不同，所以使用 Template Method Pattern。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GetEntityID(XmlElement data);

        /// <summary>
        /// ID，系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "GeneralTag", EntityCaption = "類別",IsEntityPrimaryKey=true)]
        public string ID { get; set; }
        
        /// <summary>
        /// 目前EntityID可能為Student、Class、Teacher及Course的編號
        /// </summary>
        [Field(Caption = "實體編號", EntityName = "GeneralTag", EntityCaption = "類別",Remark="實體編號其值可能為『學生編號』、『班級編號』、『教師編號』、『課程編號』")]
        public string RefEntityID { get; set; }

        /// <summary>
        /// 標籤設定編號
        /// </summary>
        [Field(Caption = "類別設定編號", EntityName = "TagConfig", EntityCaption = "類別設定",IsEntityPrimaryKey=true)]
        public string RefTagID { get; set; }

        /// <summary>
        /// 標籤前置詞
        /// </summary>
        [Field(Caption = "類別設定前置詞", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string Prefix
        {
            get
            {
                TagConfigRecord tag = !string.IsNullOrEmpty(RefTagID)?K12.Data.TagConfig.SelectByID(RefTagID):null;

                if (tag == null)
                    throw new ArgumentException("類別資訊已經不存在於系統中，可能已經刪除。");

                return tag.Prefix;
            }
        }

        /// <summary>
        /// 標籤名稱
        /// </summary>
        [Field(Caption = "類別設定名稱", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string Name
        {
            get
            {
                TagConfigRecord tag = !string.IsNullOrEmpty(RefTagID)?TagConfig.SelectByID(RefTagID):null;

                if (tag == null)
                    throw new ArgumentException("類別資訊已經不存在於系統中，可能已經刪除。");

                return tag.Name;
            }
        }

        /// <summary>
        /// 標籤顏色
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                TagConfigRecord tag = !string.IsNullOrEmpty(RefTagID)?TagConfig.SelectByID(RefTagID):null;

                if (tag == null)
                    throw new ArgumentException("類別資訊已經不存在於系統中，可能已經刪除。");

                return tag.Color;
            }
        }

        /// <summary>
        /// 標籤完整名稱，包含標籤前置詞及標籤名稱
        /// </summary>
        [Field(Caption = "類別設定完整名稱", EntityName = "TagConfig", EntityCaption = "類別設定")]
        public string FullName
        {
            get
            {
                TagConfigRecord tag = !string.IsNullOrEmpty(RefTagID)?TagConfig.SelectByID(RefTagID):null;

                if (tag == null)
                    throw new ArgumentException("類別資訊已經不存在於系統中，可能已經刪除。");

                return tag.FullName;
            }
        }
    }
}