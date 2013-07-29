using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 假別對照表資訊
    /// </summary>
    public class AbsenceMappingInfo
    {
        //<Absence Abbreviation="曠" HotKey="c" Name="曠課" Noabsence="False" />
        //<Absence Abbreviation="事" HotKey="a" Name="事假" Noabsence="False" />

        /// <summary>
        /// 預設建構式
        /// </summary>
        public AbsenceMappingInfo() { }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="node"></param>
        public AbsenceMappingInfo(XmlElement node)
        {
            Load(node);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            Name = data.Attributes["Name"].InnerText;
            Abbreviation = data.Attributes["Abbreviation"].InnerText;
            HotKey = data.Attributes["HotKey"].InnerText;

            bool noabsence;
            if (bool.TryParse(data.GetAttribute("Noabsence"), out noabsence))
                Noabsence = noabsence;
            else
                Noabsence = false; 
        }

        /// <summary>
        /// 假別名稱
        /// </summary>
        [Field(Caption = "名稱", EntityName = "AbsenceMapping", EntityCaption = "假別")]        
        public string Name { get;  set; }
        /// <summary>
        /// 假別簡稱
        /// </summary>
        [Field(Caption = "簡稱", EntityName = "AbsenceMapping", EntityCaption = "假別")]
        public string Abbreviation { get;  set; }
        /// <summary>
        /// 假別快速鍵
        /// </summary>
        [Field(Caption = "快速鍵", EntityName = "AbsenceMapping", EntityCaption = "假別")]
        public string HotKey { get;  set; }
        /// <summary>
        /// 是否不列入缺曠計算
        /// </summary>
        [Field(Caption = "列入計算", EntityName = "AbsenceMapping", EntityCaption = "假別",Remark="假別是否列入缺曠計算")]
        public bool Noabsence { get;  set; }
    }
}