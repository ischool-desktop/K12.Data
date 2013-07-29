using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 評量設定資訊
    /// </summary>
    public class AssessmentSetupRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "AssessmentSetup", EntityCaption = "評量設定", IsEntityPrimaryKey = true)]
        public string ID { get;  set; }
        /// <summary>
        /// 名稱，必填
        /// </summary>
        [Field(Caption = "名稱", EntityName = "AssessmentSetup", EntityCaption = "評量設定")]
        public string Name { get;  set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Field(Caption = "描述", EntityName = "AssessmentSetup", EntityCaption = "評量設定")]
        public string Description { get;  set; }
        /// <summary>
        /// 總成績上傳開始時間
        /// </summary>
        [Field(Caption = "總成績上傳開始時間", EntityName = "AssessmentSetup", EntityCaption = "評量設定")]
        public string StartTime { get; set; }
        /// <summary>
        /// 總成績上傳結束時間
        /// </summary>
        [Field(Caption = "總成績上傳結束時間", EntityName = "AssessmentSetup", EntityCaption = "評量設定")]
        public string EndTime { get; set; }
        /// <summary>
        /// 是否允許上傳課程成績
        /// </summary>
        protected internal bool AllowUpload { get; set; }
        /// <summary>
        /// 延伸欄位資訊
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法僅供ischool內部開發人員使用。")]
        public XmlElement Extension { get; set; }

        /// <summary>
        /// 預設建構式，初始化ID、Name、Descripton為空字串。
        /// </summary>
        public AssessmentSetupRecord()
        {
            //所有建構式都必須先呼叫此建構式來初始化
            ID = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            AllowUpload = false;

            System.Xml.XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml("<Extension/>");

            Extension = xmldoc.DocumentElement;
        }

        /// <summary>
        /// 新增評量設定記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="Name">評量設定記錄名稱</param>
        public AssessmentSetupRecord(string Name)
            : this()
        {
            this.Name = Name;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public AssessmentSetupRecord(XmlElement data) : this()
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
            XmlHelper xmldata = new XmlHelper(data);

            ID = xmldata.GetString("@ID");
            Name = xmldata.GetString("TemplateName");
            Description = xmldata.GetString("Description");
            StartTime = xmldata.GetString("StartTime");
            EndTime = xmldata.GetString("EndTime");
            AllowUpload = xmldata.GetString("AllowUpload").Equals("是")?true:false;

            if (data.SelectSingleNode("Extension/Extension") == null)
                data.SelectSingleNode("Extension").AppendChild(data.OwnerDocument.CreateElement("Extension"));

            Extension = xmldata.GetElement("Extension/Extension");
        }
    }
}