using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學校放假日清單記錄物件
    /// </summary>
    public class SchoolHolidayRecord
    {
        private XmlDocument xmlDoc;

        private List<DateTime> holidayList;

        /// <summary>
        /// 預設建構式
        /// </summary>
        public SchoolHolidayRecord()
            : this("")
        {
        }

        /// <summary>
        /// XML字串建構式
        /// </summary>
        /// <param name="xmlString"></param>
        public SchoolHolidayRecord(string xmlString)
        {
            this.holidayList = new List<DateTime>();

            if (xmlString == "")
                return;

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            this.BeginDate = DateTime.Parse(xmlDoc.DocumentElement.SelectSingleNode("BeginDate").InnerText);
            this.EndDate = DateTime.Parse(xmlDoc.DocumentElement.SelectSingleNode("EndDate").InnerText);
            this.SchoolDayCountG1 = int.Parse(xmlDoc.DocumentElement.SelectSingleNode("SchoolDayCountG1").InnerText);
            this.SchoolDayCountG2 = int.Parse(xmlDoc.DocumentElement.SelectSingleNode("SchoolDayCountG2").InnerText);
            this.SchoolDayCountG3 = int.Parse(xmlDoc.DocumentElement.SelectSingleNode("SchoolDayCountG3").InnerText);


            foreach (XmlNode nd in xmlDoc.DocumentElement.SelectNodes("HolidayList/Holiday"))
            {
                this.holidayList.Add(DateTime.Parse(nd.InnerText));
            }
        }

        /// <summary>
        /// 指定日期是否在日期區間中
        /// </summary>
        /// <param name="dtTarget"></param>
        /// <returns></returns>
        public bool IsContained(DateTime dtTarget)
        {
            return (dtTarget >= this.BeginDate && dtTarget <= this.EndDate);
        }


        /// <summary>
        /// 查詢指定日期是否為假日？
        /// </summary>
        /// <param name="dtTarget">指定要查詢的日期</param>
        /// <returns>是否為假日。若是則回傳 true‧</returns>
        public bool IsHoliday(DateTime dtTarget)
        {
            bool result = false;
            foreach (DateTime dt in this.holidayList)
            {
                if (dt.ToShortDateString() == dtTarget.ToShortDateString())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 取得可存回組態檔的XML字串
        /// </summary>
        /// <returns></returns>
        public String GetXmlString()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement elmRoot = doc.CreateElement("SchoolHolidays");
            doc.AppendChild(elmRoot);

            elmRoot.AppendChild(createElement(doc, "BeginDate", this.BeginDate.ToShortDateString()));

            elmRoot.AppendChild(createElement(doc, "EndDate", this.EndDate.ToShortDateString()));

            elmRoot.AppendChild(createElement(doc, "SchoolDayCountG1", this.SchoolDayCountG1.ToString()));

            elmRoot.AppendChild(createElement(doc, "SchoolDayCountG2", this.SchoolDayCountG2.ToString()));

            elmRoot.AppendChild(createElement(doc, "SchoolDayCountG3", this.SchoolDayCountG3.ToString()));

            XmlElement elmHolidayList = createElement(doc, "HolidayList", "");
            elmRoot.AppendChild(elmHolidayList);

            foreach (DateTime dt in this.holidayList)
            {
                elmHolidayList.AppendChild(createElement(doc, "Holiday", dt.ToShortDateString()));
            }

            return doc.InnerXml;

        }

        private XmlElement createElement(XmlDocument doc, string elementName, string innerText)
        {
            XmlElement result = doc.CreateElement(elementName);
            result.InnerText = innerText;
            return result;
        }

        /// <summary>
        /// 上課日期
        /// </summary>
        [Field(Caption = "上課日期", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Field(Caption = "結束日期", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 一年級上課天數
        /// </summary>
        [Field(Caption = "一年級上課天數", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public int SchoolDayCountG1 { get; set; }

        /// <summary>
        /// 二年級上課天數
        /// </summary>
        [Field(Caption = "二年級上課天數", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public int SchoolDayCountG2 { get; set; }

        /// <summary>
        /// 三年級上課天數
        /// </summary>
        [Field(Caption = "三年級上課天數", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public int SchoolDayCountG3 { get; set; }

        /// <summary>
        /// 放假日清單字串
        /// </summary>
        [Field(Caption = "放假日清單", EntityName = "SchoolHoliday", EntityCaption = "學校")]
        public string HolydayListStr
        {
            get 
            {
                string Result = string.Empty;

                for (int i = 0; i < HolidayList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Result))
                        Result += ",";
                    Result += HolidayList[i].ToShortDateString();
                }

                return string.Empty;
            }   
        }   

        /// <summary>
        /// 放假日清單
        /// </summary>
        public List<DateTime> HolidayList { get { return this.holidayList; } }
    }
}