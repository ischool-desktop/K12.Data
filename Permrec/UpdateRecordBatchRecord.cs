using System;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 異動名冊記錄物件
    /// </summary>
    public class UpdateRecordBatchRecord
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 名稱，必填
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 學年度，必填
        /// </summary>
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期，必填
        /// </summary>
        public int Semester { get; set; }

        /// <summary>
        /// 核准日期
        /// </summary>
        public DateTime? ADDate { get; set; }

        /// <summary>
        /// 核准文號
        /// </summary>
        public string ADNumber { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public XmlElement Content { get; set; }

        /// <summary>
        /// 必填欄位建構式
        /// </summary>
        /// <param name="Name">名稱</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        public UpdateRecordBatchRecord(string Name, int SchoolYear, int Semester)
        {
            this.Name = Name;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;
        }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public UpdateRecordBatchRecord()
        {
 
        }

        /// <summary>
        /// XML建構式
        /// </summary>
        /// <param name="data"></param>
        public UpdateRecordBatchRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 用XML載入
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(XmlElement data)
        {
            //<UpdateRecordBatch ID="18">
            //    <ADNumber />
            //    <Name>97_1_新生名冊1</Name>
            //    <Semester>1</Semester>
            //    <ADDate />
            //    <Content>
            //        <異動名冊 學年度="97" 學期="1" 學校代號="111" 學校名稱="光華國中(開發使用)" 類別="新生名冊">
            //            <清單 年級="" 科別="">
            //                <異動紀錄 備註="" 入學年月=" 9801" 入學資格代號="1" 出生年月日="83/4/26" 地址=" 806高雄市前鎮區興東里10鄰天山路７０巷１６號" 姓名="卓旻錞" 學號="5550282" 性別="男" 性別代號="1" 班別=" 309" 畢業國中所在縣市代號="" 畢業國小="" 異動代號="1" 異動日期="98/5/12" 編號="265" 身分證號="E124660334" />
            //                <異動紀錄 備註="" 入學年月=" 9801" 入學資格代號="1" 出生年月日="83/4/17" 地址=" 806高雄市前鎮區竹西里14鄰汕頭街81號9樓-2" 姓名="蘇郁超" 學號="5550283" 性別="男" 性別代號="1" 班別=" 309" 畢業國中所在縣市代號="" 畢業國小="" 異動代號="1" 異動日期="98/5/12" 編號="266" 身分證號="I100306402" />
            //                <異動紀錄 備註="" 入學年月=" 9801" 入學資格代號="1" 出生年月日="83/4/16" 地址=" 806高雄市前鎮區竹中里02鄰二聖一路198號" 姓名="王詩妤" 學號="5550284" 性別="女" 性別代號="2" 班別=" 309" 畢業國中所在縣市代號="" 畢業國小="" 異動代號="1" 異動日期="98/5/12" 編號="267" 身分證號="E224506462" />
            //            </清單>
            //        </異動名冊>
            //    </Content>
            //    <SchoolYear>97</SchoolYear>
            //</UpdateRecordBatch>

            ID = data.GetAttribute("ID");

            if (data.SelectSingleNode("Name")!=null)
                Name = data.SelectSingleNode("Name").InnerText;  
          
            if (data.SelectSingleNode("ADDate")!=null)
                ADDate = K12.Data.DateTimeHelper.Parse(data.SelectSingleNode("ADDate").InnerText);

            if (data.SelectSingleNode("ADNumber") != null)
                ADNumber = data.SelectSingleNode("ADNumber").InnerText;

            if (data.SelectSingleNode("SchoolYear") != null)
                SchoolYear = K12.Data.Int.Parse(data.SelectSingleNode("SchoolYear").InnerText);

            if (data.SelectSingleNode("Semester") != null)
                Semester = K12.Data.Int.Parse(data.SelectSingleNode("Semester").InnerText);

            Content = data.SelectSingleNode("Content") as System.Xml.XmlElement;
        }
    }
}