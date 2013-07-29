using System.Xml;
using System.IO;

namespace K12.Data
{
    /// <summary>
    /// 學生期中成績資訊
    /// </summary>
    public class SCETakeRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Caption = "編號", EntityName = "SCETake", EntityCaption = "評量成績", IsEntityPrimaryKey = true)]
        public string ID { get;  set; }
        /// <summary>
        /// 成績，此成績為高中專屬，高中的成績會在Extension欄位中定義
        /// </summary>
        protected internal decimal Score { get;  set; }

        /// <summary>
        /// 文字評量，國高中皆存在相同位置，為Extension欄位
        /// </summary>
        [Field(Caption = "文字描述", EntityName = "SCETake", EntityCaption = "評量成績")]
        public string Text
        {
            get
            {
                return Extension.SelectSingleNode("Text").InnerText;
            }
            set
            {
                Extension.SelectSingleNode("Text").InnerText = value;
            }
        }

        /// <summary>
        /// 延伸欄位資訊
        /// </summary>
        protected internal XmlElement Extension {get;set;}
        /// <summary>
        /// 所屬學生修課編號，必填
        /// </summary>
        [Field(Caption = "學生修課編號", EntityName = "SCAttend", EntityCaption = "學生修課", IsEntityPrimaryKey = true)]
        public string RefSCAttendID { get;  set; }
        /// <summary>
        /// 所屬試別設定編號，必填
        /// </summary>
        [Field(Caption = "試別編號", EntityName = "Exam", EntityCaption = "試別", IsEntityPrimaryKey = true)]
        public string RefExamID { get;  set; }
        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get;  set; }
        /// <summary>
        /// 所屬課程編號
        /// </summary>
        [Field(Caption = "課程編號", EntityName = "Course", EntityCaption = "課程", IsEntityPrimaryKey = true)]
        public string RefCourseID { get;  set; }
        /// <summary>
        /// 所屬學生
        /// </summary>
        public StudentRecord Student
        {
            get 
            {
                return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null; 
            }
        }

        /// <summary>
        /// 所屬學生修課
        /// </summary>
        public SCAttendRecord SCAttend 
        {
            get 
            {
                return !string.IsNullOrEmpty(RefSCAttendID)?K12.Data.SCAttend.SelectByID(RefSCAttendID):null; 
            } 
        }
        /// <summary>
        /// 所屬試別設定
        /// </summary>
        public ExamRecord Exam 
        {
            get
            {
                return !string.IsNullOrEmpty(RefExamID)?K12.Data.Exam.SelectByID(RefExamID):null; 
            } 
        }
        /// <summary>
        /// 所屬課程
        /// </summary>
        public CourseRecord Course
        {
            get 
            { 
                return !string.IsNullOrEmpty(RefCourseID)?K12.Data.Course.SelectByID(RefCourseID):null; 
            }
        }

        #region Service欄位參考
        //<Field OutputType="Attribute" Source="ID" Target="sce.id" />
        //<Field OutputConverter="AbsenceOutput" Source="Score" Target="sce.score" />
        //<Field Source="ExamID" Target="sce.ref_exam_id" />
        //<Field Source="AttendID" Target="sce.ref_sc_attend_id" />

        //<Field Source="RefStudentID" Target="sc.ref_student_id" />
        //<Field Source="RefCourseID" Target="sc.ref_course_id" />
        //<Field OutputConverter="RequiredOutput" Source="IsRequired" Target="sc.is_required" />
        //<Field OutputConverter="RequiredByOutput" Source="RequiredBy" Target="sc.required_by" />
        //<Field Source="AttendScore" Target="sc.score" />

        //<Field Source="ExamName" Target="exam.exam_name" />
        //<Field OutputType="Xml" Source="Extensions" Target="sce.extensions" />
        #endregion

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public SCETakeRecord()
        {
            Score = 0;

            System.Xml.XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml("<Extension><Score/><Text/><Effort/></Extension>");

            Extension = xmldoc.DocumentElement;
        }

        /// <summary>
        /// 新增學生期中成績記錄建構式，參數為新增記錄的必填欄位
        /// </summary>
        /// <param name="RefSCAttendID">所屬學生修課編號</param>
        /// <param name="RefExamID">所屬試別設定編號</param>
        public SCETakeRecord(string RefSCAttendID,string RefExamID):this()
        {
            this.RefSCAttendID = RefSCAttendID;
            this.RefExamID = RefExamID;
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public SCETakeRecord(XmlElement element)
        {
            Load(element);
        }

        /// <summary>
        /// 將資料匯出成XML格式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <returns></returns>
        public XmlElement ToXML()
        {
                System.Xml.XmlDocument xmldoc = new XmlDocument();

                xmldoc.LoadXml("<Score ID=\'\'><Extension/><AttendID/><RefStudentID/><RefCourseID/><Score/><ExamID/></Score>");

                xmldoc.DocumentElement.SetAttribute("ID", ID);
                xmldoc.DocumentElement.SelectSingleNode("Score").InnerText = K12.Data.Decimal.GetString(Score);
                xmldoc.DocumentElement.SelectSingleNode("AttendID").InnerText = RefSCAttendID;
                xmldoc.DocumentElement.SelectSingleNode("ExamID").InnerText = RefExamID;
                xmldoc.DocumentElement.SelectSingleNode("RefStudentID").InnerText = RefStudentID;
                xmldoc.DocumentElement.SelectSingleNode("RefCourseID").InnerText = RefCourseID;

                XmlDocumentFragment xmldocfrag = xmldoc.CreateDocumentFragment();

                xmldocfrag.InnerXml = Extension.OuterXml;

                xmldoc.DocumentElement.SelectSingleNode("Extension").AppendChild(xmldocfrag);             

                return xmldoc.DocumentElement;
            
            //    <Score ID="273">
            //    <Extension>
            //        <Extension>
            //            <Effort>0</Effort>
            //            <Text/>
            //        </Extension>
            //    </Extension>
            //    <AttendID>104</AttendID>
            //    <RefStudentID>281</RefStudentID>
            //    <RefCourseID>122</RefCourseID>
            //    <Score>0</Score>
            //    <ExamID>2</ExamID>
            //</Score>
        }

        /// <summary>
        /// 覆寫Object的ToString()方法，傳回XML字串
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            StringWriter stringWriter = new StringWriter();

            XmlTextWriter xmltextWriter = new XmlTextWriter(stringWriter);
            xmltextWriter.Formatting = Formatting.Indented;

            xmltextWriter.WriteStartDocument();
            xmltextWriter.WriteStartElement("Score");
            xmltextWriter.WriteAttributeString("ID", ID);
            xmltextWriter.WriteStartElement("Extension");
            xmltextWriter.WriteRaw(Extension.OuterXml);
            xmltextWriter.WriteEndElement();
            xmltextWriter.WriteElementString("AttendID", RefSCAttendID);
            xmltextWriter.WriteElementString("RefCourseID", RefCourseID);
            xmltextWriter.WriteElementString("Score", K12.Data.Decimal.GetString(Score));
            xmltextWriter.WriteElementString("ExamID", RefExamID);
            xmltextWriter.WriteElementString("RefStudentID", RefStudentID);
            xmltextWriter.WriteEndElement();
            xmltextWriter.WriteEndDocument();
            xmltextWriter.Flush();
            xmltextWriter.Close();
            stringWriter.Flush();

            return stringWriter.ToString();
        }

        /// <summary>
        /// 從XML載入資料
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="element"></param>
        public virtual void Load(XmlElement element)
        {
            XmlHelper helper = new XmlHelper(element);

            ID = helper.GetString("@ID");
            Score = K12.Data.Decimal.Parse(helper.GetString("Score"));
            RefSCAttendID = helper.GetString("AttendID");
            RefExamID = helper.GetString("ExamID");
            RefStudentID = helper.GetString("RefStudentID");
            RefCourseID = helper.GetString("RefCourseID");

            if (element.SelectSingleNode("Extension/Extension") == null)
                element.SelectSingleNode("Extension").AppendChild(element.OwnerDocument.CreateElement("Extension"));

            Extension = helper.GetElement("Extension/Extension");

            if (Extension.SelectSingleNode("Text") == null)
                Extension.AppendChild(Extension.OwnerDocument.CreateElement("Text"));

            //Effort = K12.Data.Int.ParseAllowNull(helper.GetString("Extension/Extension/Effort"));
            //Text = helper.GetString("Extension/Extension/Text");

            #region ResponseXml參考
            //<Score ID="2285">
            //    <Extensions />
            //    <RequiredBy>校訂</RequiredBy>
            //    <AttendScore />
            //    <IsRequired>選</IsRequired>
            //    <ExamName>第二次月考</ExamName>
            //    <AttendID>2967</AttendID>
            //    <RefStudentID>170626</RefStudentID>
            //    <RefCourseID>111</RefCourseID>
            //    <Score>80</Score>
            //    <ExamID>494</ExamID>
            //</Score>
            #endregion
        }

        /// <summary>
        /// 從字串XML載入資料
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(string data)
        {
            #region 使用XMLDocument的方式來解析
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.LoadXml(data);
            #endregion

            Load(xmldoc.DocumentElement);

            #region 使用XPathReader來讀資料，但是有些值會讀不出來
            //XmlTextReader xtr = new XmlTextReader(new StringReader(data));
            //xtr.WhitespaceHandling = WhitespaceHandling.None;
            //MyXPathReader.XPathCollection xc = new MyXPathReader.XPathCollection();
            //int qScore = xc.Add("/Score");
            //int qRefStudentID = xc.Add("/Score/RefStudentID");
            //int qRefSCAttendID = xc.Add("/Score/AttendID");
            //int qRefExamID = xc.Add("/Score/ExamID");
            //int qRefCourseID = xc.Add("/Score/RefCourseID");

            //MyXPathReader.XPathReader xpr = new MyXPathReader.XPathReader(xtr, xc);

            //while (xpr.ReadUntilMatch())
            //{
            //    if (xpr.Match(qRefStudentID))
            //        RefStudentID = xpr.ReadInnerXml();
            //    else if (xpr.Match(qRefSCAttendID))
            //        RefSCAttendID = xpr.ReadInnerXml();
            //    else if (xpr.Match(qRefExamID))
            //        RefExamID = xpr.ReadInnerXml();
            //    else if (xpr.Match(qRefCourseID))
            //        RefCourseID = xpr.ReadInnerXml();
            //}
            #endregion


            #region 使用XmlTextReader來讀取資料，但是RefStudentID會讀不出來
            //StringReader StrReader = new StringReader(data);

            //XmlTextReader TextReader = new XmlTextReader(StrReader);

            //TextReader.WhitespaceHandling = WhitespaceHandling.None;               
            
            //TextReader.Read(); TextReader.Read();

            //ID = TextReader.GetAttribute("ID");

            //TextReader.Read();

            //while (!TextReader.EOF)
            //{
            //    if (TextReader.Name.Equals("RefStudentID"))
            //        System.Console.WriteLine(TextReader.NodeType.ToString());

            //    if (TextReader.NodeType == XmlNodeType.Element)
            //    {
            //        switch (TextReader.Name)
            //        {
            //            case "Score": Score = K12.Data.Decimal.Parse(TextReader.ReadInnerXml()); break;
            //            case "AttendID": RefSCAttendID = TextReader.ReadInnerXml(); break;
            //            case "ExamID": RefExamID = TextReader.ReadInnerXml(); break;
            //            case "RefStudentID": RefStudentID = TextReader.ReadInnerXml(); break;
            //            case "RefCourseID": RefCourseID = TextReader.ReadInnerXml(); break;
            //        }

            //        TextReader.Read();
            //    }
            //    else
            //        TextReader.Read();
            //}
            #endregion
        }
    }
}