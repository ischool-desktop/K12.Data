using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 節次對照表資訊，一個物件代表一個節次的假別對照資訊
    /// </summary>
    public class PeriodMappingInfo
    {
        //<Period Aggregated="0.5" Name="早自習" Sort="1" Type="集會" />
        //<Period Aggregated="0.8" Name="升旗" Sort="2" Type="集會" />
        
        /// <summary>
        /// 預設建構式
        /// </summary>
        public PeriodMappingInfo() { }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public PeriodMappingInfo(XmlElement data)
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
            Name = data.Attributes["Name"].InnerText;
            Type = data.Attributes["Type"].InnerText;

            int sort;
            if (!int.TryParse(data.Attributes["Sort"].InnerText, out sort))
                Sort = int.MaxValue;
            else
                Sort = sort;

            float aggregated;
            if (!float.TryParse(data.GetAttribute("Aggregated"), out aggregated))
                Aggregated = 0.0f;
            else
                Aggregated = aggregated; 
        }

        /// <summary>
        /// 節次名稱，如早自習、升旗
        /// </summary>
        [Field(Caption = "名稱", EntityName = "PeriodMapping", EntityCaption = "節次")]
        public string Name { get; set; }
        /// <summary>
        /// 節次類別，如一般、集會
        /// </summary>
        [Field(Caption = "類別", EntityName = "PeriodMapping", EntityCaption = "節次")]
        public string Type { get; set; }
        /// <summary>
        /// 節次順序
        /// </summary>
        [Field(Caption = "順序", EntityName = "PeriodMapping", EntityCaption = "節次")]
        public int Sort { get; set; }
        /// <summary>
        /// 統計權重，一般而言一節是1，但像早修、升旗的時間比較短，其值可能是0.5；目前在計算特殊學生表現時會用到。
        /// </summary>
        [Field(Caption = "統計權重", EntityName = "PeriodMapping", EntityCaption = "節次", Remark = "一般而言一節是1，但像早修、升旗的時間比較短，其值可能是0.5；目前在計算特殊學生表現時會用到。")]
        public float Aggregated { get; set; }
    }
}