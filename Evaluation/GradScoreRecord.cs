using System;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生畢業成績資訊
    /// </summary>
    public class GradScoreRecord
    {
        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生", IsEntityPrimaryKey = true)]
        public string RefStudentID { get; set; }
        /// <summary>
        /// 畢業領域成績
        /// </summary>
        public Dictionary<string, GradDomainScore> Domains { get; set; }
        /// <summary>
        /// 畢業學習領域成績
        /// </summary>
        [Field(Caption = "學習領域成績", EntityName = "GradScore", EntityCaption = "畢業")]
        public decimal? LearnDomainScore { get; set; }
        /// <summary>
        /// 畢業課程學習成績
        /// </summary>
        [Field(Caption = "課程學習成績", EntityName = "GradScore", EntityCaption = "畢業")]
        public decimal? CourseLearnScore { get; set; }
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
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public GradScoreRecord(XmlElement data)
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

            RefStudentID = helper.GetString("@ID");

            Domains = new Dictionary<string, GradDomainScore>();
            foreach (var domainElement in helper.GetElements("GradScore/GradScore/Domain"))
            {
                GradDomainScore domainScore = new GradDomainScore(domainElement);
                Domains.Add(domainScore.Domain, domainScore);
            }

            decimal score;
            if (decimal.TryParse(helper.GetString("GradScore/GradScore/LearnDomainScore"), out score))
                LearnDomainScore = score;
            if (decimal.TryParse(helper.GetString("GradScore/GradScore/CourseLearnScore"), out score))
                CourseLearnScore = score;
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public GradScoreRecord()
        {
        }
    }

    /// <summary>
    /// 畢業領域成績
    /// </summary>
    public class GradDomainScore : ICloneable
    {
        /// <summary>
        /// 領域名稱
        /// </summary>
        [Field(Caption = "領域", EntityName = "GradDomainScore", EntityCaption = "畢業領域")]
        public string Domain { get;  set; }
        /// <summary>
        /// 成績
        /// </summary>
        [Field(Caption = "成績", EntityName = "GradDomainScore", EntityCaption = "畢業領域")]
        public decimal? Score { get; set; }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="element"></param>
        public GradDomainScore(XmlElement element)
        {
            Domain = element.GetAttribute("Name");
            decimal d;
            if (decimal.TryParse(element.GetAttribute("Score"), out d))
                Score = d;
        }

        /// <summary>
        /// 建構式，傳入預設領域名稱
        /// </summary>
        /// <param name="domain"></param>
        public GradDomainScore(string domain)
        {
            Domain = domain;
        }


        #region ICloneable 成員

        /// <summary>
        /// 複製畢業成績物件
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            GradDomainScore newScore = new GradDomainScore(Domain);
            newScore.Score = this.Score;
            return newScore;
        }

        #endregion
    }
}