using System.Xml;

namespace K12.Data
{
    public interface IXmlTransform
    {
        void Load(XmlElement data);

        XmlElement ToXml();
    }
}