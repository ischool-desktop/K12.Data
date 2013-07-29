using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Xml;
using System.ComponentModel;

namespace K12.Data.Configuration
{
    /// <summary>
    /// 代表一組的組態資料，使用「Key、Value」的方式取存。
    /// </summary>
    public class ConfigData : IEnumerable<string>
    {
        #region Instance Members
        /// <summary>
        /// 取得或設定組態記錄資料。
        /// </summary>
        internal ConfigurationRecord Record { get; set; }
        /// <summary>
        /// 取得組態的名稱空間。
        /// </summary>
        public string Namespace { get { return Record.Namespace; } }
        /// <summary>
        /// 取得或設定指定的組態資料。
        /// </summary>
        /// <param name="name">組態 key 名稱，名稱中不可以包含特殊符號。(將值指定為「空字串」或是「Null」代表移除該設定，不過此行為在資料同步模式下無作用。)</param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                if (!Record.BaseData.ContainsKey(name))
                {
                    if (Manager.Readonly)
                        throw new ArgumentException("指定的組態資料不存在。");
                }

                if (Record.BaseData == null)
                    throw new ArgumentException("舊的組態格式不支援此屬性存取，請使用 PreviousData 屬性讀取資料。");

                return Record.BaseData[name];
            }
            set
            {
                if (Manager.Readonly) throw new ArgumentException("此組態是唯讀的。");

                if (Encoding.UTF8.GetByteCount(value) > 1024 * 1024 * 2) //大約是 2MB。
                    throw new ArgumentException("儲存的資料量過大，將會造成系統不穩定，請儲存較小的資料，或是分成數個組態儲存。");

                if (Record.BaseData == null)
                    throw new ArgumentException("舊的組態格式不支援此屬性存取，請使用 PreviousData 屬性讀取資料。");

                if (name.IndexOfAny(new char[] { '$', '&' }) >= 0)
                    throw new ArgumentException("名稱中不能含有「&」與「$」符號。");

                //if (string.IsNullOrEmpty(value))
                //{
                //    if (Record.BaseData.ContainsKey(name))
                //        Record.BaseData.Remove(name);
                //}
                //else
                    Record.BaseData[name] = value;
            }
        }
        /// <summary>
        /// 前版的組態資料，這是為了相容舊的資料而提供的成員，在一般情況是 Null。
        /// </summary>
        public XmlElement PreviousData
        {
            get
            {
                return Record.PreviousData;
            }
            set
            {
                if (Manager.Readonly) throw new ArgumentException("此組態是唯讀的。");

                if (Record.BaseData != null)
                    throw new ArgumentException("此組態是新格式，無法指定舊格式資料。");

                Record.PreviousData = value;
            }
        }
        /// <summary>
        /// 取得組態數量。
        /// </summary>
        public int Count
        {
            get
            {
                if (Record.BaseData == null)
                    throw new ArgumentException("舊的組態格式不支援此屬性存取，請使用 PreviousData 屬性讀取資料。");

                return Record.BaseData.Count;
            }
        }
        /// <summary>
        /// 判斷指定的名稱是否已經存在於組態中。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            if (Record.BaseData == null)
                throw new ArgumentException("舊的組態格式不支援此方法，請使用 PreviousData 屬性讀取資料。");

            return Record.BaseData.ContainsKey(name);
        }
        /// <summary>
        /// 儲存組態資料，會一併更新相同 Namespace 的組態實體資料。
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Save()
        {
            if (Manager.Readonly) throw new ArgumentException("此組態是唯讀的。");

            //當有組態被更新的時後。
            Manager.ConfigurationUpdated += new EventHandler<ItemUpdatedEventArgs>(Manager_ConfigurationUpdated);
            Manager.Cache.SyncData(Namespace);//先同步資料。

            Manager.Provider.SaveConfiguration(new ConfigurationRecord[] { Record });

            //當有組態被更新的時後。
            Manager.ConfigurationUpdated += new EventHandler<ItemUpdatedEventArgs>(Manager_ConfigurationUpdated);
            Manager.Cache.SyncData(Namespace); //此動作會通知相關的 Configuration 實體，更新自身的資料。
        }

        /// <summary>
        /// 取得一個非同步版本的組態資料。
        /// </summary>
        /// <returns></returns>
        public ConfigData Async()
        {
            return new ConfigData(Manager, Record.Clone(), false);
        }
        /// <summary>
        /// 指示是否只要有人更新相同 Namespace 的資料，一併同步資料。
        /// </summary>
        private bool Sync { get; set; }
        /// <summary>
        /// 管理此 Configuration 的類別。
        /// </summary>
        internal ConfigurationManager Manager { get; set; }

        internal ConfigData(ConfigurationManager manager, ConfigurationRecord record)
        {
            Sync = true;
            Manager = manager;

            if (record == null)
                throw new ArgumentException("組態記錄不可以是 Null。");
            else
                Record = record;
        }

        internal ConfigData(ConfigurationManager manager, ConfigurationRecord record, bool sync)
            : this(manager, record)
        {
            Sync = sync;
        }

        private void Manager_ConfigurationUpdated(object sender, ItemUpdatedEventArgs e)
        {
            string name = Namespace;

            if (e.PrimaryKeys.Contains(name))
            {
                if (Sync) //同步時的處理方式。
                {
                    if (Manager.Cache[name] != null)
                    {
                        ConfigurationRecord newRecord = Manager.Cache[name];

                        if (newRecord.BaseData == null)
                            Record = newRecord;
                        else
                        {
                            //神秘的大絕招... = =''
                            foreach (string each in new List<string>(Record.BaseData.Keys)) //將自身的資料同步到別人的資料中…
                                newRecord.BaseData[each] = Record.BaseData[each];

                            Record = newRecord; //一定是更新模式。
                        }
                    }
                    else //代表別人把這個組態從資料庫刪掉了。
                        Record.EditAction = 1;//新增模式。
                }
                else //非同步時的處理方式。
                {
                    //處理當原本是新增的組態，因為另一個相同 Namespace 的組態先儲存了，這裡就改成更新。
                    if (Manager.Cache[name] != null)
                    {
                        if (Record.EditAction == 1)
                            Record.EditAction = 2;
                    }
                }
            }

            Manager.ConfigurationUpdated -= new EventHandler<ItemUpdatedEventArgs>(Manager_ConfigurationUpdated);
        }

        #region IEnumerable<string> 成員

        public IEnumerator<string> GetEnumerator()
        {
            return Record.BaseData.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Record.BaseData.Keys.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
