using System.Collections.Generic;

namespace K12.Data
{
    /// <summary>
    /// 快取集合類別，提供實際提供快取的類別或是介面，一般使用在向快取類別要資料時，快取類別傳回快取資料，並告知沒有資料的編號列表。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheSet<T> : IEnumerable<T>
    {
        /// <summary>
        /// 缺乏的資料編號列表
        /// </summary>
        public List<string> WantIDs;
        /// <summary>
        /// 實際的快取資料
        /// </summary>
        public List<T> Records;

        /// <summary>
        /// 無參數建構式，會初始化NoExistIDs及Records
        /// </summary>
        public CacheSet()
        {
            WantIDs = new List<string>();
            Records = new List<T>();
        }

        #region IEnumerable<T> 成員

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Records).GetEnumerator();
        }

        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        #endregion
    }
}