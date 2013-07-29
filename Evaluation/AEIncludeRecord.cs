using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 評分樣板資訊
    /// </summary>
    public class AEIncludeRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "AEInclude", EntityCaption = "評分樣板", IsEntityPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 所屬評量設定編號，必填
        /// </summary>
        [Field(Caption = "評量設定編號", EntityName = "AssessmentSetup", EntityCaption = "評量設定", IsEntityPrimaryKey = true)]
        public string RefAssessmentSetupID { get; set; } 
        /// <summary>
        /// 所屬試別項目編號，必填
        /// </summary>
        [Field(Caption = "試別編號", EntityName = "Exam", EntityCaption = "試別", IsEntityPrimaryKey = true)]
        public string RefExamID { get; set; }   
        /// <summary>
        /// 試別名稱
        /// </summary>
        [Field(Caption = "試別名稱", EntityName = "Exam", EntityCaption = "試別")]
        public string ExamName { get;  set; }
        /// <summary>
        /// 試別項目記錄物件
        /// </summary>
        public ExamRecord Exam 
        {
            get
            { 
                return !string.IsNullOrEmpty(RefExamID)?K12.Data.Exam.SelectByID(RefExamID):null;
            } 
        }
        /// <summary>
        /// 是否有文字評量（高中使用） 
        /// </summary>
        protected internal bool UseText { get; set; }
        /// <summary>
        /// 是否有百分比成績（高中使用）
        /// </summary>
        protected internal bool UseScore { get;  set; }
        /// <summary>
        /// 是各評量配分的比例，例如第一次月考 30%、第二次月考 30%、期末考 40%，其值為0到100。
        /// </summary>
        [Field(Caption = "評量配分比例", EntityName = "AEInclude", EntityCaption = "評分樣板")]
        public int Weight { get;  set; }
        /// <summary>
        /// 輸入開始時間
        /// </summary>
        [Field(Caption = "輸入開始時間", EntityName = "AEInclude", EntityCaption = "評分樣板")]
        public string StartTime { get;  set; }
        /// <summary>
        /// 輸入結束時間
        /// </summary>
        [Field(Caption = "輸入結束時間", EntityName = "AEInclude", EntityCaption = "評分樣板")]
        public string EndTime { get;  set; }
        /// <summary>
        /// 試別樣版記錄物件
        /// </summary>
        public AssessmentSetupRecord AssessmentSetup 
        { 
            get
            {
                return !string.IsNullOrEmpty(RefAssessmentSetupID)?K12.Data.AssessmentSetup.SelectByID(RefAssessmentSetupID):null;
            }
        }
        /// <summary>
        /// 是否開放TeacherAccess輸入成績
        /// </summary>
        protected internal bool OpenTeacherAccess { get; set; }
        /// <summary>
        /// 是否強制輸入成績
        /// </summary>
        protected internal bool InputRequired { get; set; }
        /// <summary>
        /// 延伸欄位資訊
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法僅供ischool內部開發人員使用。")]
        public XmlElement Extension { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public AEIncludeRecord()
        {
            System.Xml.XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml("<Extension><UseScore>否</UseScore><UseEffort>否</UseEffort><UseText>否</UseText></Extension>");

            Extension = xmldoc.DocumentElement;
        }

        /// <summary>
        /// 新增評分樣板記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefExamID">所屬試別項目編號</param>
        /// <param name="RefAssessmentSetupID">所屬評分樣板編號</param>
        public AEIncludeRecord(string RefExamID,string RefAssessmentSetupID) : this()
        {
            this.RefExamID = RefExamID;
            this.RefAssessmentSetupID = RefAssessmentSetupID;
        }

        /// <summary>
        /// 建構式，傳入XML Element
        /// </summary>
        /// <param name="element"></param>
        public AEIncludeRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="element"></param>
        public virtual void Load(XmlElement element)
        {
            XmlHelper helper = new XmlHelper(element);

            RefAssessmentSetupID = helper.GetString("ExamTemplateID");
            RefExamID = helper.GetString("RefExamID");

            ID = helper.GetString("@ID");
            ExamName = helper.GetString("ExamName");
            UseScore = (helper.GetString("UseScore") == "是") ? true : false;
            UseText = (helper.GetString("UseText") == "是") ? true : false;
            Weight = K12.Data.Int.Parse(helper.GetString("Weight"));
            StartTime = helper.GetString("StartTime");
            EndTime = helper.GetString("EndTime");

            OpenTeacherAccess = helper.GetString("OpenTeacherAccess").Equals("是")?true:false;
            InputRequired = helper.GetString("InputRequired").Equals("否")?true:false;

            if (element.SelectSingleNode("Extension/Extension") == null)
                element.SelectSingleNode("Extension").AppendChild(element.OwnerDocument.CreateElement("Extension"));

            Extension = helper.GetElement("Extension/Extension");
        }

        /// <summary>
        /// XML規格如下
        /// <![CDATA[
        ///&gt;IncludeExam ID="169">
        ///    <ExamTemplateID>1</ExamTemplateID>
        ///    <RefExamID>1</RefExamID>
        ///    <UseText>否</UseText>
        ///    <UseScore>是</UseScore>
        ///    <Weight>15</Weight>
        ///    <EndTime>2009/06/02 23:59</EndTime>
        ///    <StartTime>2009/05/20 11:00</StartTime>
        ///    <Extension>
        ///        <Extension>
        ///            <UseEffort>否</UseEffort>
        ///            <UseText>否</UseText>
        ///        </Extension>
        ///    </Extension>
        ///    <OpenTeacherAccess>是</OpenTeacherAccess>
        ///    <InputRequired>是</InputRequired>
        ///</IncludeExam>
        /// ]]>
        /// </summary>
        /// <returns></returns>
        public XmlElement ToXML()
        {
            System.Xml.XmlDocument xmldoc = new XmlDocument();
            
            xmldoc.LoadXml("<IncludeExam ID=\'\'><ExamTemplateID/><RefExamID/><UseText/><UseScore/><Weight/><EndTime/><StartTime/><Extension/><OpenTeacherAccess/><InputRequired/></IncludeExam>");

            xmldoc.DocumentElement.SetAttribute("ID", ID);
            xmldoc.DocumentElement.SelectSingleNode("ExamTemplateID").InnerText = RefAssessmentSetupID;
            xmldoc.DocumentElement.SelectSingleNode("RefExamID").InnerText = RefExamID;
            xmldoc.DocumentElement.SelectSingleNode("UseText").InnerText = UseText==true?"是":"否";
            xmldoc.DocumentElement.SelectSingleNode("UseScore").InnerText = UseScore==true?"是":"否";
            xmldoc.DocumentElement.SelectSingleNode("Weight").InnerText = Weight.ToString();
            xmldoc.DocumentElement.SelectSingleNode("StartTime").InnerText = StartTime;
            xmldoc.DocumentElement.SelectSingleNode("EndTime").InnerText = EndTime;
            xmldoc.DocumentElement.SelectSingleNode("OpenTeacherAccess").InnerText = OpenTeacherAccess==true?"是":"否";
            xmldoc.DocumentElement.SelectSingleNode("InputRequired").InnerText = InputRequired==true?"是":"否";

            XmlDocumentFragment xmldocfrag = xmldoc.CreateDocumentFragment();

            xmldocfrag.InnerXml = Extension.OuterXml;

            xmldoc.DocumentElement.SelectSingleNode("Extension").AppendChild(xmldocfrag);

            return xmldoc.DocumentElement;

       }
    }
}