using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace K12.Data.Configuration
{
    internal class ExceptionReport
    {
        private Dictionary<Type, IExtraProcesser> _collect_types;
        private List<Type> _subclasses = new List<Type>();

        public ExceptionReport()
        {
            _collect_types = new Dictionary<Type, IExtraProcesser>();
            AddType(typeof(Exception), true);
        }

        public void AddType(Type type)
        {
            AddType(type, false);
        }

        public void AddType(Type type, bool includeSubclass)
        {
            if (_collect_types.ContainsKey(type)) return;

            _collect_types.Add(type, null);

            if (includeSubclass)
                _subclasses.Add(type);
        }

        public void AddType(Type type, IExtraProcesser processer)
        {
            if (_collect_types.ContainsKey(type)) return;

            _collect_types.Add(type, processer);
        }

        public void AddType(Type type, IExtraProcesser processer, bool includeSubclass)
        {
            if (_collect_types.ContainsKey(type)) return;

            _collect_types.Add(type, processer);

            if (includeSubclass)
                _subclasses.Add(type);
        }

        public string Transform(Exception ex)
        {
            XmlTextWriter writer = new XmlTextWriter(new MemoryStream(), Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            //writer.WriteStartElement(ex.GetType().Name);
            writer.WriteStartElement("Exception");
            {
                Transform(writer, ex);
            }
            writer.WriteEndElement();

            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(writer.BaseStream, Encoding.UTF8);

            string result = reader.ReadToEnd();
            reader.Close();

            return result;
        }

        private void Transform(XmlWriter writer, object ex)
        {
            Type ext = ex.GetType();
            foreach (PropertyInfo each in ext.GetProperties())
            {
                if (!each.CanRead)
                    continue;

                writer.WriteStartElement(each.Name);
                writer.WriteAttributeString("PropertyType", each.PropertyType.Name);
                {
                    try
                    {
                        object obj = each.GetValue(ex, null);

                        if (obj != null)
                        {
                            writer.WriteAttributeString("InstanceType", obj.GetType().Name);

                            if (_collect_types.ContainsKey(obj.GetType()) || Subclasses(obj.GetType()))
                                Transform(writer, obj);
                            else
                                writer.WriteString(obj.ToString());
                        }
                        else
                            writer.WriteComment("此屬性值為「Null」。");
                    }
                    catch (Exception e)
                    {
                        writer.WriteComment(string.Format("讀取屬性值錯誤，訊息：\n{0}", e.Message));
                    }
                }
                writer.WriteEndElement();
            }

            writer.WriteComment("自定錯誤訊息處理流程所產生的資料。");
            writer.WriteStartElement("ExtraProcessSection");
            {
                if (_collect_types.ContainsKey(ex.GetType()))
                {
                    IExtraProcesser processer = _collect_types[ex.GetType()];

                    if (processer != null)
                    {
                        ExtraInformation[] infos = processer.Process(ex);

                        if (infos != null)
                        {
                            foreach (ExtraInformation each in infos)
                            {
                                writer.WriteStartElement(each.Name);
                                writer.WriteString(each.Data);
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }

        private bool Subclasses(Type type)
        {
            foreach (Type each in _subclasses)
            {
                if (type.IsSubclassOf(each))
                    return true;
            }

            return false;
        }
    }
}
