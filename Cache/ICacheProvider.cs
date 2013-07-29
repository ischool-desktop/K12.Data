using System.Collections;
using System.Collections.Generic;

namespace K12.Data
{
    /// <summary>
    /// 快取提供者介面
    /// 1.XmlElement的根節點必需要有ID屬性用來唯一識別。
    /// 2.通常物件必需提供LoadXml及ToXml方法以能提供Xml及物件間的轉換機制。
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 取得快取內的所有資料
        /// </summary>
        /// <param name="EntityName"></param>
        /// <returns></returns>
        Dictionary<string, string> SelectAll(string EntityName);

        /// <summary>
        /// 根據多筆編號取得某個實體的資料，實際儲存時為字串，取出時會轉成XML文件
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="EntityIDs">多筆實體編號</param>
        CacheSet<string> SelectByIDs(string EntityName, IEnumerable<string> EntityIDs);

        /// <summary>
        /// 新增或更新多筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Records">多筆XML文件</param>
        void Set(string EntityName, IEnumerable Records);

        /// <summary>
        /// 新增或更新單筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Record">單筆XML文件</param>
        void Set(string EntityName, object Record);

        /// <summary>
        /// 新增多筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Records">多筆XML文件</param>
        void Insert(string EntityName,IEnumerable Records);

        /// <summary>
        /// 新增單筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Records">多筆XML文件</param>
        void Insert(string EntityName, object Record);
        
        /// <summary>
        /// 更新多筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Records">多筆XML文件</param>
        void Update(string EntityName, IEnumerable Records);

        /// <summary>
        /// 更新單筆XML文件至快取當中，其中XML文件的根節點必需有ID屬性，以便用來唯一識別；實際儲存時會將Xml文件轉為字串儲存。
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        /// <param name="Records">多筆XML文件</param>
        void Update(string EntityName, object Record);

        /// <summary>
        /// 根據識別編號移除實體中的快取
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="EntityIDs"></param>
        void Delete(string EntityName,IEnumerable<string> EntityIDs);

        /// <summary>
        /// 根據識別編號移除實體中的快取
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="EntityID"></param>
        void Delete(string EntityName,string EntityID);      

        /// <summary>
        /// 移除某個實體的所有資料
        /// </summary>
        /// <param name="EntityName">實體名稱</param>
        void Delete(string EntityName);

        /// <summary>
        /// 給一個字串解析出其中ID
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        string ParseID(string Content);
    }
}