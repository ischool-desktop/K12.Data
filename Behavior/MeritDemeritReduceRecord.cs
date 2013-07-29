using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 功過換算表
    /// </summary>
    public class MeritDemeritReduceRecord
    {
        /// <summary>
        /// 大功換算小功
        /// </summary>
        public int? MeritAToMeritB { get; set; }

        /// <summary>
        /// 小功換算獎勵
        /// </summary>
        public int? MeritBToMeritC { get; set; }

        /// <summary>
        /// 大過換算小過
        /// </summary>
        public int? DemeritAToDemeritB { get; set; }

        /// <summary>
        /// 小過換算警告
        /// </summary>
        public int? DemeritBToDemeritC { get; set; }

        /// <summary>
        /// XML參數建構式
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            MeritAToMeritB = K12.Data.Int.Parse(data.SelectSingleNode("Merit/AB").InnerText);
            MeritBToMeritC = K12.Data.Int.Parse(data.SelectSingleNode("Merit/BC").InnerText);
            DemeritAToDemeritB = K12.Data.Int.Parse(data.SelectSingleNode("Demerit/AB").InnerText);
            DemeritBToDemeritC = K12.Data.Int.Parse(data.SelectSingleNode("Demerit/BC").InnerText);
            //<GetMDReduce>
            //    <Merit>
            //        <AB>3</AB>
            //        <BC>3</BC>
            //    </Merit>
            //    <Demerit>
            //        <AB>3</AB>
            //        <BC>3</BC>
            //    </Demerit>
            //</GetMDReduce>
        }
    }
}
