using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using FISCA.Data;

namespace K12.Data
{
    /// <summary>
    /// 科目中英文對照表
    /// </summary>
    public class ChineseEnglishSubjectMapping
    {
        /// <summary>
        /// 更新科目中英文對照表
        /// </summary>
        /// <returns></returns>
        public static void Update(Dictionary<string, string> Subjects)
        {
            UpdateHelper helper = new UpdateHelper();

            XElement Element = new XElement("Content");

            foreach (string Key in Subjects.Keys)
            {
                XElement elmSubject = new XElement("Subject");
                elmSubject.SetAttributeValue("Chinese", Key);
                elmSubject.SetAttributeValue("English", Subjects[Key]);

                Element.Add(elmSubject);
            }

            string strElement = Element.ToString();

            int result = helper.Execute("update list set content='"+ strElement +"' where name='科目中英文對照表'"); 
        }

        /// <summary>
        /// 取得科目中英文對照表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> SelectAll()
        {
            //"科目中英文對照表"
            //<Content>
            //   <Subject Chinese="國文" English="Chinese"/>
            //   <Subject Chinese="英文" English="English"/>
            //</Content>

            QueryHelper helper = new QueryHelper();

            Dictionary<string, string> result = new Dictionary<string, string>();

            DataTable table = helper.Select("select * from list where name='科目中英文對照表'");

            if (table.Rows.Count == 1)
            {
                string Content = table.Rows[0].Field<string>("content");

                StringReader reader = new StringReader(Content);

                XElement Element = XElement.Load(reader);

                foreach (XElement elmSubject in Element.Elements("Subject"))
                {
                    string Chinese = elmSubject.AttributeText("Chinese");
                    string English = elmSubject.AttributeText("English");

                    if (!result.ContainsKey(Chinese))
                        result.Add(Chinese, English);
                }
            }

            return result;
       }
    }
}