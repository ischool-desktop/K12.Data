using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 神奇的自動字典，在取值或寫值時，不會產生錯誤，會自動增加 Key 到字典中，但 ContainsKey 不會自動增加 Key。
    /// </summary>
    public class AutoDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> BaseDictionary { get; set; }
        private bool ReadOnly { get; set; }

        public AutoDictionary()
        {
            BaseDictionary = new Dictionary<string, string>();
            ReadOnly = false;
        }

        /// <summary>
        /// 用指定的 Dictionary 當作基底資料。
        /// </summary>
        /// <param name="copySource"></param>
        /// <param name="readOnly">資料是否可寫入。</param>
        public AutoDictionary(Dictionary<string, string> copySource, bool readOnly)
            : this()
        {
            BaseDictionary = new Dictionary<string, string>(copySource);
            ReadOnly = readOnly;
        }

        /// <summary>
        /// 解析 Xml 內容到 Dictionary 中，如果有重覆的 Element 資料，會產生 Exception。
        /// </summary>
        /// <param name="data">Xml 資料，用 Element.LocalName 當作 Key， InnerXml 當作 Value。</param>
        /// <param name="readOnly">資料是否可寫入。</param>
        public AutoDictionary(XmlElement data, bool readOnly)
            : this()
        {
            ReadOnly = readOnly;

            if (data == null) return;

            foreach (XmlNode each in data.ChildNodes)
            {
                if (each.NodeType == XmlNodeType.Element)
                    BaseDictionary.Add(each.LocalName, each.InnerXml.Trim());
            }
        }

        /// <param name="data">Xml 資料，用指定的屬性值當作 Key，InnerText 當作 Value。</param>
        /// <param name="keyAttName">屬性名稱，該屬性的值會被當作是 Dictionary 的 Key，如果該屬性不存在會產生 Exception。</param>
        /// <param name="readOnly">資料是否可寫入。</param>
        public AutoDictionary(XmlNodeList data, string keyAttName, bool readOnly)
            : this()
        {
            ReadOnly = readOnly;

            if (data == null) return;

            foreach (XmlNode each in data)
                BaseDictionary.Add(each.Attributes[keyAttName].Value, each.InnerText);
        }

        /// <param name="data">Xml 資料，使用指定的屬性值當作 Key 與 Value。</param>
        /// <param name="keyAttName">屬性名稱，該屬性的值會被當作 Dictionary 的 Key，如果該屬性不存在會產生 Exception。</param>
        /// <param name="valueAttName">屬性名稱，該屬性的值會被當作 Dictionary 的 Value，如果該屬性不存在會產生 Exception。</param>
        /// <param name="readOnly">資料是否可寫入。</param>
        public AutoDictionary(XmlNodeList data, string keyAttName, string valueAttName, bool readOnly)
            : this()
        {
            ReadOnly = readOnly;

            if (data == null) return;

            foreach (XmlNode each in data)
                BaseDictionary.Add(each.Attributes[keyAttName].Value, each.Attributes[valueAttName].Value);
        }

        private AutoDictionary(AutoDictionary autoDic)
        {
            BaseDictionary = new Dictionary<string, string>(autoDic.BaseDictionary);
        }

        /// <summary>
        /// 新增 Key 到 Dictionary 中，值為 string.Empty，如果已經存在該 Key 會產生 Exception。
        /// </summary>
        /// <param name="key"></param>
        public void Add(string key)
        {
            if (ReadOnly) throw new ArgumentException("此集合是唯讀的。");

            BaseDictionary.Add(key, string.Empty);
        }

        public bool Remove(string key)
        {
            if (ReadOnly) throw new ArgumentException("此集合是唯讀的。");

            return BaseDictionary.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return BaseDictionary.ContainsKey(key);
        }

        public string this[string key]
        {
            get
            {
                if (BaseDictionary.ContainsKey(key))
                    return BaseDictionary[key];
                else
                {
                    BaseDictionary.Add(key, string.Empty);
                    return BaseDictionary[key];
                }
            }
            set
            {
                if (BaseDictionary.ContainsKey(key))
                    BaseDictionary[key] = value;
                else
                {
                    if (ReadOnly) throw new ArgumentException("此集合是唯讀的。");

                    BaseDictionary.Add(key, value);
                }
            }
        }

        public int Count { get { return BaseDictionary.Count; } }

        public IEnumerable<string> Keys { get { return BaseDictionary.Keys; } }

        public IEnumerable<string> Values { get { return BaseDictionary.Values; } }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, string> each in this)
                builder.Append(string.Format("{0}=\"{1}\"\n,", each.Key, each.Value));

            return builder.ToString();
        }

        #region IEnumerable<KeyValuePair<string,string>> 成員

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return BaseDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成員

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return BaseDictionary.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// 比較兩個 AutoDictionary 是否相同。
        /// </summary>
        /// <param name="obj">要比較的 AutoDictionary。</param>
        /// <returns></returns>
        public bool Equals(AutoDictionary obj)
        {
            if (BaseDictionary.Count != obj.BaseDictionary.Count)
                return false;

            foreach (string each in BaseDictionary.Keys)
            {
                if (obj.BaseDictionary.ContainsKey(each))
                {
                    if (obj.BaseDictionary[each] != BaseDictionary[each])
                        return false;
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 複製一份相同資料並且可讀寫的 AutoDictioanry。
        /// </summary>
        /// <returns></returns>
        public AutoDictionary Clone()
        {
            return new AutoDictionary(this);
        }

        private bool ValidateKey(string key)
        {
            return true;
        }
    }
}