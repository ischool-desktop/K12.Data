using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace K12.Data.Configuration
{
    /// <summary>
    /// 負責包存 Configuration 的原始資料。
    /// </summary>
    public class ConfigurationRecord
    {
        public const string RootName = "Configurations";
        internal const string RecordName = "Configuration";

        internal AutoDictionary BaseData { get; set; }

        /// <summary>
        /// 儲存動作(1:新增,2:修改,3:刪除)。
        /// </summary>
        public int EditAction { get; internal set; }
        /// <summary>
        /// 取得組態名稱。
        /// </summary>
        public string Namespace { get; private set; }
        /// <summary>
        /// 建立新增的「組態記錄」。
        /// </summary>
        public ConfigurationRecord(string name)
        {
            BaseData = new AutoDictionary();
            PreviousData = null; //新格式的話，保持此資料是 Null。
            Namespace = name;
            EditAction = 1;//新增
        }
        /// <summary>
        /// 建立可更新的「組態記錄」。
        /// </summary>
        /// <param name="data">組態的  Xml 資料。</param>
        public ConfigurationRecord(string name, XmlElement data)
        {
            if (data == null)
                throw new ArgumentException("組態的 Xml 資料不可以是 Null。");

            Namespace = name;
            EditAction = 2;//修改
            BaseData = null; //舊的格式的話，保持此資料是 Null。

            if (data.LocalName != RootName)
                PreviousData = data.CloneNode(true) as XmlElement;
            else
                BaseData = new AutoDictionary(data.SelectNodes(RecordName), "Name", false);
        }
        /// <summary>
        /// 複製一份資料。
        /// </summary>
        /// <returns></returns>
        internal ConfigurationRecord Clone()
        {
            ConfigurationRecord record = new ConfigurationRecord(Namespace);
            record.EditAction = EditAction;

            if (BaseData != null)
                record.BaseData = BaseData.Clone();

            if (PreviousData != null)
                record.PreviousData = PreviousData.CloneNode(true) as XmlElement;

            return record;
        }
        /// <summary>
        /// 前版的組態資料，這是為了相容舊的資料而提供的成員，在一般情況是 Null。
        /// </summary>
        internal XmlElement PreviousData { get; set; }
        /// <summary>
        /// 取得用於儲存的 Xml 資料。
        /// </summary>
        /// <returns>合法的 Xml 資料。</returns>
        public string GetXml()
        {
            if (BaseData != null) //新格式。
            {
                MemoryStream ms = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

                writer.WriteStartElement(RootName);
                foreach (KeyValuePair<string, string> each in BaseData)
                {
                    writer.WriteStartElement(RecordName);
                    writer.WriteAttributeString("Name", each.Key);
                    writer.WriteString(each.Value);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms, System.Text.Encoding.UTF8);

                string result = sr.ReadToEnd();
                ms.Close();
                writer.Close();

                return result;
            }
            else
                return PreviousData.OuterXml;  //舊格式。
        }
    }
}
