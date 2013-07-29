using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 功過換算表
    /// </summary>
    public class MeritDemeritReduce
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Config.GetMDReduce";

        /// <summary>
        /// 取得功過換算表
        /// </summary>
        /// <returns></returns>
        public static MeritDemeritReduceRecord Select()
        {
            return Select<MeritDemeritReduceRecord>();
        }

        /// <summary>
        /// 取得功過換算表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static T Select<T>() where T : MeritDemeritReduceRecord, new()
        {
            XmlElement Element = DSAServices
                .CallService(SELECT_SERVICENAME, new DSRequest())
                .GetContent()
                .GetElement(".");

            T record = new T();

            record.Load(Element);

            return record;                
        }
    }
}