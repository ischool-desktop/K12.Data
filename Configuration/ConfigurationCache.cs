using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data.Configuration
{
    #region Configuration Cache
    internal class ConfigurationCache : CacheManager<ConfigurationRecord>
    {
        private IConfigurationProvider Provider { get; set; }

        public ConfigurationCache(IConfigurationProvider provider)
        {
            Provider = provider;
        }

        protected override Dictionary<string, ConfigurationRecord> GetAllData()
        {
            return Provider.GetAllConfiguration();
        }

        protected override Dictionary<string, ConfigurationRecord> GetData(IEnumerable<string> primaryKeys)
        {
            return Provider.GetConfiguration(primaryKeys);
        }

        protected override bool ValidateKey(string key)
        {
            return key.IndexOfAny(new char[] { '$', '&' }) < 0;
        }
    }
    #endregion
}
