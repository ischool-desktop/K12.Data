using System;
using System.Collections.Generic;
using System.Linq;

namespace K12.Data
{
    /// <summary>
    /// 提供Entity的Cache協助，抽像化Entity與CacheProvider之間的互動
    /// </summary>
    public abstract class EntityCacheHelper
    {
        /// <summary>
        /// 建構式為protected只有後代才能建立
        /// </summary>
        protected EntityCacheHelper()
        {
 
        }

        /// <summary>
        /// 驗證輸入的鍵值是否合法，當要求查尋資料時若鍵值不合法則不進行查尋
        /// 預設驗證方法為是否可轉化為int
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <returns>是否合法</returns>
        public virtual bool ValidateKey(string key)
        {
            int a;
            return int.TryParse(key, out a);
        }

        /// <summary>
        /// 快取的類別名稱
        /// </summary>
        public abstract string Name { get;}

        /// <summary>
        /// 直接根據編號列表取得資料
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        protected abstract List<string> SelectFromServerByIDs(IEnumerable<string> IDs);

        /// <summary>
        /// 快取提供者介面
        /// </summary>
        public abstract ICacheProvider Provider { get; }

        /// <summary>
        /// 根據編號列表取得資料，會先從快取中取得資料，沒有的資料再呼叫SelectFromServerByIDs
        /// </summary>
        /// <param name="IDs">編號列表</param>
        /// <returns></returns>
        public List<string> SelectByIDs(IEnumerable<string> IDs)
        {
            if (Provider != null)
            {
                CacheSet<string> CacheSet = Provider.SelectByIDs(Name, IDs);

                //針對沒有存在Cache當中的資料再向直接要一次
                if (CacheSet.WantIDs.Count > 0)
                {
                    List<string> WantRecords = SelectFromServerByIDs(IDs);

                    CacheSet.Records.AddRange(WantRecords);

                    CacheSet.WantIDs.Clear();

                    Provider.Insert(Name, WantRecords);
                }

                return CacheSet.Records;
            }

            return new List<string>();
        }

        /// <summary>
        /// 將快取資料移除
        /// </summary>
        /// <param name="IDs"></param>
        public void Remove(IEnumerable<string> IDs)
        {
            if (Provider != null)
                Provider.Delete(Name,IDs);
        }

        /// <summary>
        /// 提供事件方法可被告知要移除快取資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyRemove(object sender, DataChangedEventArgs e)
        {
            this.Remove(e.PrimaryKeys);
        }
    }
}