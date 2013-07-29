using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using K12.Data;

namespace K12.Data.Utility
{
    public class EntityHelper
    {
        public static List<T> GetListFromXml<T>(IEnumerable<XmlElement> Elements) where T:K12.Data.IXmlTransform,new()
        {
            List<T> Records = new List<T>();

            foreach (XmlElement element in Elements)
            {
                T Record = new T();
                Record.Load(element);
                Records.Add(Record);
            }

            return Records;
        }
    }
}
