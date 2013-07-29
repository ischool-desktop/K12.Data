using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Data.Configuration
{
    public class ConfigurationManager
    {
        #region Instance Members
        /// <summary>
        /// Configuration 的快取。
        /// </summary>
        internal ConfigurationCache Cache { get; set; }
        /// <summary>
        /// 負責提供 Configuration 的資料。
        /// </summary>
        internal IConfigurationProvider Provider { get; set; }
        /// <summary>
        /// 提供一個時機，處理當使用者要求指定的 Namespace 組態資料時，可以變更實際傳送到底層的 Namespace 名稱。
        /// </summary>
        /// <param name="configNamespace"></param>
        /// <returns></returns>
        protected virtual string NamespacePreprocess(string configNamespace)
        {
            return configNamespace;
        }
        /// <summary>
        /// 指示此組態是否為可讀寫的。
        /// </summary>
        public virtual bool Readonly { get { return false; } }
        /// <summary>
        /// 取得組態資料，如果指定的名稱存在則回傳資料，不存在則自動建立新的組態。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public ConfigData this[string configNamespace]
        {
            get
            {
                string ns = NamespacePreprocess(configNamespace);

                ConfigurationRecord record = Cache[ns];

                if (record == null)
                {
                    if (Readonly)
                        throw new ArgumentException("指定的組態不存在。", "configNamespace");
                    else
                        return new ConfigData(this, new ConfigurationRecord(ns));
                }
                else
                    return new ConfigData(this, record);
            }
        }
        /// <summary>
        /// 向主機同步組態資料。
        /// </summary>
        public void Sync(string configNamespace)
        {
            Cache.SyncData(configNamespace);
        }
        /// <summary>
        /// 刪除指定的組態。
        /// </summary>
        /// <param name="conf"></param>
        public void Remove(ConfigData conf)
        {
            if (Readonly)
                throw new ArgumentException("此類型的組態是唯讀的。");

            conf.Record.EditAction = 3; //刪除
            Provider.SaveConfiguration(new ConfigurationRecord[] { conf.Record });
            Cache.SyncData(conf.Namespace);
        }
        /// <summary>
        /// 當組態被更新時引發，參數中的「PrimaryKeys」是被更新的組態名稱清單。
        /// </summary>
        internal event EventHandler<ItemUpdatedEventArgs> ConfigurationUpdated;
        /// <summary>
        /// 建立實體。
        /// </summary>
        /// <param name="provider"></param>
        internal ConfigurationManager(IConfigurationProvider provider)
        {
            Provider = provider;
            Cache = new ConfigurationCache(Provider);
            Cache.ItemUpdated += delegate(object sender, ItemUpdatedEventArgs e)
            {
                if (ConfigurationUpdated != null)
                {
                    BigEvent be = new BigEvent("ConfigurationUpdated", ConfigurationUpdated, this, e);
                    be.UIRaise();
                }
            };
        }

        #endregion
    }
}
