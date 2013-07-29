using System;
using System.Collections.Generic;

namespace K12.Data
{
    public class EntityCache<T> : Dictionary<string, T>
    {
        /// <summary>
        /// 驗證輸入的鍵值是否合法，當要求查尋資料時若鍵值不合法則不進行查尋
        /// 預設驗證方法為是否可轉化為int
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <returns>是否合法</returns>
        public bool ValidateKey(string key)
        {
            int a;
            return int.TryParse(key, out a);
        }

        public CacheSet<T> SelectByIDs(IEnumerable<string> IDs)
        {
            CacheSet<T> CacheSet = new CacheSet<T>();

            //針對傳入的ID檢查Cache當中是否有相對應的物件，若有的話放到物件列表當中，沒有的話將ID存起來
            foreach (string strID in IDs)
            {
                if (ValidateKey(strID))
                {
                    if (this.ContainsKey(strID))
                        CacheSet.Records.Add(this[strID]);
                    else
                        CacheSet.WantIDs.Add(strID);
                }
            }

            return CacheSet;
        }

        public int Remove(IEnumerable<string> IDs)
        {
            int Result = 0;

            foreach (string ID in IDs)
            {
                if (this.ContainsKey(ID))
                {
                    this.Remove(ID);
                    Result++;
                }
            }

            return Result;
        }

        public void NotifyRemove(object sender, DataChangedEventArgs e)
        {
            this.Remove(e.PrimaryKeys);
        }

    }
}