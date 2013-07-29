using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 成績計算規則資訊
    /// </summary>
    public class ScoreCalcRuleRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "ScoreCalcRule", EntityCaption = "成績計算", IsEntityPrimaryKey = true)]
        public string ID { get;  set; }
        /// <summary>
        /// 名稱，必填
        /// </summary>
        [Field(Caption = "名稱", EntityName = "ScoreCalcRule", EntityCaption = "成績計算")]
        public string Name { get;  set; }

        public int SchoolYearEntryScoreDecimal { get; set; }

        /// <summary>
        /// 內容，必填
        /// </summary>
        public XmlElement Content { get;  set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public ScoreCalcRuleRecord()
        {
 
        }

        /// <summary>
        /// 新增成績計算規則記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="Name">名稱</param>
        /// <param name="Content">內容</param>
        public ScoreCalcRuleRecord(string Name,XmlElement Content)
        {
            this.Name = Name;
            this.Content = Content;    
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public ScoreCalcRuleRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(XmlElement data)
        {
            XmlHelper helper = new XmlHelper(data);

            ID = helper.GetString("@ID");
            Name = helper.GetString("Name");
            Content = helper.GetElement("Content/ScoreCalcRule");

            #region 精準位數
            //if (scoreCalcRule.SelectSingleNode("各項成績計算位數/學年分項成績計算位數") != null)
            //{
            //    if (int.TryParse(helper.GetText("各項成績計算位數/學年分項成績計算位數/@位數"), out tryParseint))
            //        decimals = tryParseint;
            //    if (bool.TryParse(helper.GetText("各項成績計算位數/學年分項成績計算位數/@四捨五入"), out tryParsebool) && tryParsebool)
            //        mode = RoundMode.四捨五入;
            //    if (bool.TryParse(helper.GetText("各項成績計算位數/學年分項成績計算位數/@無條件捨去"), out tryParsebool) && tryParsebool)
            //        mode = RoundMode.無條件捨去;
            //    if (bool.TryParse(helper.GetText("各項成績計算位數/學年分項成績計算位數/@無條件進位"), out tryParsebool) && tryParsebool)
            //        mode = RoundMode.無條件進位;
            //}
            #endregion


        }
    }
}