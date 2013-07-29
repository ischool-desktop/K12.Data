using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data.Configuration
{
    public static class Configuration_ExtendMethods
    {
        /// <summary>
        /// 儲存所有組態資料。
        /// </summary>
        public static void SaveAll(this IEnumerable<ConfigData> configs)
        {
            Dictionary<int, ConfigurationManager> managers = new Dictionary<int, ConfigurationManager>();
            Dictionary<int, List<ConfigurationRecord>> batchs = new Dictionary<int, List<ConfigurationRecord>>();
            Dictionary<int, List<string>> batchkeys = new Dictionary<int, List<string>>();

            ConfigurationManager manager;
            foreach (ConfigData each in configs)
            {
                manager = each.Manager;

                if (!managers.ContainsKey(manager.GetHashCode()))
                    managers.Add(manager.GetHashCode(), manager);

                if (!batchs.ContainsKey(manager.GetHashCode()))
                    batchs.Add(manager.GetHashCode(), new List<ConfigurationRecord>());

                if (!batchkeys.ContainsKey(manager.GetHashCode()))
                    batchkeys.Add(manager.GetHashCode(), new List<string>());

                batchs[manager.GetHashCode()].Add(each.Record);
                batchkeys[manager.GetHashCode()].Add(each.Namespace);
            }

            foreach (ConfigurationManager each in managers.Values)
            {
                if (each.Readonly) throw new ArgumentException("您試圖儲存唯讀的組態。");

                each.Cache.SyncData(batchkeys[each.GetHashCode()]);
                each.Provider.SaveConfiguration(batchs[each.GetHashCode()]);
                each.Cache.SyncData(batchkeys[each.GetHashCode()]);
            }
        }
    }
}
