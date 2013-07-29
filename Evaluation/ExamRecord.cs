using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 試別項目資訊
    /// </summary>
    public class ExamRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "Exam", EntityCaption = "試別", IsEntityPrimaryKey = true)]
        public string ID { get;  set; }
        /// <summary>
        /// 名稱，必填
        /// </summary>
        [Field(Caption = "名稱", EntityName = "Exam", EntityCaption = "試別")]
        public string Name { get;  set; }
        /// <summary>
        /// 描述，必填
        /// </summary>
        [Field(Caption = "描述", EntityName = "Exam", EntityCaption = "試別")]
        public string Description { get;  set; }
        /// <summary>
        /// 試別順序，例如第一次評量填入1、第二次評量順序填入2，必填
        /// </summary>
        [Field(Caption = "順序", EntityName = "Exam", EntityCaption = "試別")]
        public int? DisplayOrder { get;  set; }

        /// <summary>
        /// 無參數建構式，會初始化ID、Name及Description為空字串，並釋DisplayOrder設為0
        /// </summary>
        public ExamRecord()
        {
            ID = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            DisplayOrder = 0;
        }

        /// <summary>
        /// 新增試別項目記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="Name">名稱</param>
        /// <param name="Description">描述</param>
        /// <param name="DisplayOrder">試別順序</param>
        public ExamRecord(string Name,string Description,int DisplayOrder):this()
        {
            this.Name = Name;
            this.Description = Description;
            this.DisplayOrder = DisplayOrder;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public ExamRecord(XmlElement data)
            : this()
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
            XmlHelper helper = new XmlHelper(data);
            ID = helper.GetString("@ID");
            Name = helper.GetString("ExamName");
            Description = helper.GetString("Description");
            DisplayOrder = K12.Data.Int.ParseAllowNull(helper.GetString("DisplayOrder"));
        }
    }
}