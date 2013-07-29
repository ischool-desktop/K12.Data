using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 鄉鎮縣市類別，提供方法用來取得鄉鎮縣市資訊
    /// </summary>
    public class Town
    {
        private const string srvName = "SmartSchool.Config.GetCountyTownList";

        private static List<TownRecord> _townRecord;
        private static Dictionary<string, TownRecord> TownRecord_County_Area;

        /// <summary>
        /// 根據郵遞區號取得縣市鄉鎮列表。
        /// </summary>
        /// <param name="ZipCode">郵遞區號</param>
        /// <returns>List&lt;TownRecord&gt;，一個TownRecord物件代表一個縣市鄉鎮。</returns>
        /// <seealso cref="TownRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         List&lt;TownRecord&gt; records = Town.SelectByZipCode("302");
        ///         
        ///         foreach(TownRecord record in records)
        ///             System.Console.WriteLine(record.ZipCode);
        ///     </code>
        /// </example>
        public static List<TownRecord> SelectByZipCode(string ZipCode)
        {

            if (_townRecord == null)
                InitializeData();

            List<TownRecord> counties = new List<TownRecord>();

            foreach (TownRecord each in _townRecord)
            {
                if (each.ZipCode.Equals(ZipCode))
                    counties.Add(each);
            }

            return counties;
        }

        /// <summary>
        /// 根據縣市鄉鎮名稱取得郵遞區號。
        /// </summary>
        /// <param name="County">縣市名稱</param>
        /// <param name="Area">鄉鎮名稱</param>
        /// <returns>string，代表郵遞區號。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     string zipcode = Town.SelectZipCodeByTown("新竹縣","新豐鄉");
        ///     </code>
        /// </example>
        public static string SelectZipCodeByTown(string County, string Area)
        {

            if (_townRecord == null)
                InitializeData();

            if (TownRecord_County_Area.ContainsKey(County + "_" + Area))
                return TownRecord_County_Area[County + "_" + Area].ZipCode;
            else
                return string.Empty;
        }

        /// <summary>
        /// 根據縣市名稱取得縣市鄉鎮列表。
        /// </summary>
        /// <param name="County">縣市名稱</param>
        /// <returns>List&lt;TownRecord&gt;，TownRecord代表縣市鄉鎮物件。</returns>
        /// <seealso cref="TownRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///    <code>
        ///    List&lt;TownRecord&gt; records = Town.SelectByCounty(County);
        ///      
        ///    foreach(TownRecord record in records)
        ///         System.Console.WriteLine(record.ZipCode);
        ///    </code>
        /// </example>    
        public static List<TownRecord> SelectByCounty(string County)
        {
            if (_townRecord == null)
                InitializeData();

            List<TownRecord> counties = new List<TownRecord>();

            foreach (TownRecord each in _townRecord)
            {
                if (each.County.Equals(County))
                    counties.Add(each);
            }

            return counties;
        }

        /// <summary>
        /// 取得所有縣市列表
        /// </summary>
        /// <returns>List&lt;string&gt;，縣市列表。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;string&gt; countylist = Town.SelectCountyList();
        ///     
        ///     foreach(string county in countylist)
        ///         System.Console.WriteLine(county);
        ///     </code>
        /// </example>    
        public static List<string> SelectCountyList()
        {
            if (_townRecord == null)
                InitializeData();

            Dictionary<string, TownRecord> counties = new Dictionary<string, TownRecord>();

            foreach (TownRecord each in _townRecord)
            {
                if (!counties.ContainsKey(each.County))
                    counties.Add(each.County, each);
            }

            return counties.Keys.ToList();
        }

        /// <summary>
        /// 取得所有縣市鄉鎮區碼資料
        /// </summary>
        /// <returns>List&lt;TownRecord&gt;,TownRecord的清單。一個TownRecord 物件代表一個科別。</returns>
        /// <seealso cref="TownRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;TownRecord&gt; records = Town.SelectAll();
        ///     
        ///    foreach(TownRecord record in records)
        ///         System.Console.WriteLine(record.ZipCode);
        ///     </code>
        /// </example>    
        [SelectMethod("K12.Town.SelectAll", "學籍.縣市鄉鎮區")]
        public static List<TownRecord> SelectAll()
        {
            if (_townRecord == null)
                InitializeData();

            return _townRecord;
        }

        #region ============  private functions  =============================
        [FISCA.Authentication.AutoRetryOnWebException()]
        private static void InitializeData()
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Fields");
            helper.AddElement("Fields", "All");

            DSXmlHelper rsp = DSAServices.CallService(srvName, new DSRequest(helper)).GetContent();

            _townRecord = new List<TownRecord>();
            TownRecord_County_Area = new Dictionary<string, TownRecord>();

            foreach (XmlElement each in rsp.GetElements("Town"))
            {
                TownRecord townRecord = new TownRecord(each);
                _townRecord.Add(townRecord);
                TownRecord_County_Area.Add(townRecord.County + "_" + townRecord.Area, townRecord);
            }
        }
        #endregion

    }
}
