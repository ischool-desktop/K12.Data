using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 入學前資訊
    /// 詳細規格：https://docs.google.com/a/ischool.com.tw/View?id=dcw7gq95_29ff6fpfhd
    /// </summary>
    public class BeforeEnrollmentRecord
    {
        /// <summary>
        /// 所屬學生編號
        /// </summary>
        [Field(Caption = "學生編號", EntityName = "Student", EntityCaption = "學生",IsEntityPrimaryKey=true)]
        public string RefStudentID { get; set; }
        /// <summary>
        /// 所屬學生記錄物件
        /// </summary>
        public StudentRecord Student
        {
            get 
            {
                return !string.IsNullOrEmpty(RefStudentID)?K12.Data.Student.SelectByID(RefStudentID):null;
            }
        }
        /// <summary>
        /// 入學前學校名稱
        /// </summary>
        [Field(Caption = "學校名稱", EntityName = "BeforeEnrollment", EntityCaption = "入學前")]
        public string School {get; set;}
        /// <summary>
        /// 入學前學校位置
        /// </summary>
        [Field(Caption = "學校位置", EntityName = "BeforeEnrollment", EntityCaption = "入學前")]
        public string SchoolLocation {get; set;}
        /// <summary>
        /// 入學前班級名稱
        /// </summary>
        [Field(Caption = "班級名稱", EntityName = "BeforeEnrollment", EntityCaption = "入學前")]
        public string ClassName {get; set;}
        /// <summary>
        /// 入學前班級座號
        /// </summary>
        [Field(Caption = "座號", EntityName = "BeforeEnrollment", EntityCaption = "入學前")]
        public int? SeatNo {get; set;}
        /// <summary>
        /// 備忘資訊
        /// </summary>
        [Field(Caption = "備忘資訊", EntityName = "BeforeEnrollment", EntityCaption = "入學前")]
        public string Memo {get; set;}

        /// <summary>
        /// 國中畢業學年度
        /// </summary>
        [Field(Caption = "國中畢業學年度",EntityName ="BeforeEnrollment",EntityCaption ="入學前")]
        protected internal string GraduateSchoolYear { get; set; }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public BeforeEnrollmentRecord()
        {
 
        }

        /// <summary>
        /// XML參數建構式
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public BeforeEnrollmentRecord(XmlElement data)
        {
            Load(data);
        }

        /// <summary>
        /// 從XML載入設定值
        /// <![CDATA[
        /// ]]>
        /// </summary>
        /// <param name="data"></param>
        public void Load(XmlElement data)
        {
            //<BeforeEnrollment>
            //    <School/>
            //    <SchoolLocation/>
            //    <ClassName/>
            //    <SeatNo/>
            //    <Memo/>
            //</BeforeEnrollment>

            RefStudentID = data.SelectSingleNode("@ID") == null ? string.Empty : data.SelectSingleNode("@ID").InnerText;

            School = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/School") == null ? string.Empty : data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/School").InnerText;

            SchoolLocation = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/SchoolLocation") == null ? string.Empty : data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/SchoolLocation").InnerText;

            ClassName = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/ClassName") == null ? string.Empty : data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/ClassName").InnerText;

            SeatNo = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/SeatNo") == null ? null : K12.Data.Int.ParseAllowNull(data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/SeatNo").InnerText);

            Memo = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/Memo") == null ? string.Empty : data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/Memo").InnerText;

            GraduateSchoolYear = data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/GraduateSchoolYear") == null ? string.Empty : data.SelectSingleNode("BeforeEnrollment/BeforeEnrollment/GraduateSchoolYear").InnerText; 
        }
    }
}