using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data.Configuration
{
    #region Provider Interface
    /// <summary>
    /// 負責提供組態(Configuration)的讀取與儲存功能。
    /// </summary>
    internal interface IConfigurationProvider
    {
        /// <summary>
        /// 取得所有組態資料物件。
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ConfigurationRecord> GetAllConfiguration();
        /// <summary>
        /// 取得指定的組態資料物件。
        /// </summary>
        /// <param name="configNames"></param>
        /// <returns></returns>
        Dictionary<string, ConfigurationRecord> GetConfiguration(IEnumerable<string> namespaces);
        /// <summary>
        /// 儲存組態資料物件，實作時請使同步執行緒(完成儲存動作時才回傳)。
        /// </summary>
        /// <param name="configurations"></param>
        void SaveConfiguration(IEnumerable<ConfigurationRecord> configurations);
    }
    #endregion
}
