using System;
using FISCA.DSAUtil;
using K12.Data.Utility;
using System.Collections.Generic;
using System.Xml;

namespace K12.Data
{
    /// <summary>
    /// 學生照片類別，提供方法用來取得及修改學生照片資訊
    /// </summary>
    public class Photo
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.GetDetailList";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        /// <summary>
        /// 更新學生入學照片
        /// </summary>
        /// <param name="image">照片物件，為System.Drawing.Bitmap格式</param>
        /// <param name="StudentID">學生記錄編號</param>
        public static void UpdateFreshmanPhoto(System.Drawing.Bitmap image, string StudentID)
        {
            UpdateFreshmanPhoto(PhotoUtil.GetBase64Encoding(image), StudentID);
        }

        /// <summary>
        /// 更新學生畢業照片
        /// </summary>
        /// <param name="image">照片物件，為System.Drawing.Bitmap格式</param>
        /// <param name="StudentID">學生記錄編號</param>
        public static void UpdateGraduatePhoto(System.Drawing.Bitmap image, string StudentID)
        {
            UpdateGraduatePhoto(PhotoUtil.GetBase64Encoding(image), StudentID);
        }

        /// <summary>
        /// 更新學生入學照片
        /// </summary>
        /// <param name="picBase64String">照片內容，為字串格式</param>
        /// <param name="StudentID">學生記錄編號</param>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static void UpdateFreshmanPhoto(string picBase64String, string StudentID)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("UpdateStudentList");
            helper.AddElement("Student");
            helper.AddElement("Student", "Field");
            helper.AddElement("Student/Field", "FreshmanPhoto");
            helper.AddCDataSection("Student/Field/FreshmanPhoto", picBase64String);
            helper.AddElement("Student", "Condition");
            helper.AddElement("Student/Condition", "ID", StudentID);
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(UPDATE_SERVICENAME, dsreq);
        }

        /// <summary>
        /// 更新學生畢業照片
        /// </summary>
        /// <param name="picBase64String">照片內容，為字串格式</param>
        /// <param name="StudentID">學生記錄編號</param>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static void UpdateGraduatePhoto(string picBase64String, string StudentID)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("UpdateStudentList");
            helper.AddElement("Student");
            helper.AddElement("Student", "Field");
            helper.AddElement("Student/Field", "GraduatePhoto");
            helper.AddCDataSection("Student/Field/GraduatePhoto", picBase64String);
            helper.AddElement("Student", "Condition");
            helper.AddElement("Student/Condition", "ID", StudentID);
            dsreq.SetContent(helper);
            DSResponse dsrsp = DSAServices.CallService(UPDATE_SERVICENAME, dsreq);
        }

        /// <summary>
        /// 取得多筆學生入學照片
        /// </summary>
        /// <param name="StudentIDs"></param>
        /// <returns>傳回為Dictionary，Key為學生記錄紀號，Value為照片內容</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static Dictionary<string,string> SelectFreshmanPhoto(IEnumerable<string> StudentIDs)
        {
            Dictionary<string, string> PhotoList = new Dictionary<string, string>();

            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field","ID");
            helper.AddElement("Field", "FreshmanPhoto");
            helper.AddElement("Condition");

            foreach(string StudentID in StudentIDs)
                if (!string.IsNullOrEmpty(StudentID))
                   helper.AddElement("Condition", "ID", StudentID);

            dsreq.SetContent(helper);

            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            foreach (XmlElement element in dsrsp.GetContent().GetElements("Student"))
            {
                string strStudentID = element.GetAttribute("ID");
                string strFrshmanPhoto = element.SelectSingleNode("FreshmanPhoto").InnerText;
                PhotoList.Add(strStudentID,strFrshmanPhoto);
            }

            return PhotoList;
        }

        /// <summary>
        /// 取得學生入學照片
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>照片內容</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static string SelectFreshmanPhoto(string StudentID)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "FreshmanPhoto");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "ID", StudentID);
            dsreq.SetContent(helper);


            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            string strFreshmanPhoto = dsrsp.GetContent().GetText("Student/FreshmanPhoto");

            return strFreshmanPhoto;
        }

        /// <summary>
        /// 取得多筆學生畢業照片
        /// </summary>
        /// <param name="StudentIDs"></param>
        /// <returns>傳回為Dictionary，Key為學生記錄紀號，Value為照片內容</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static Dictionary<string,string> SelectGraduatePhoto(IEnumerable<string> StudentIDs)
        {
            Dictionary<string, string> PhotoList = new Dictionary<string, string>();

            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "ID");
            helper.AddElement("Field", "GraduatePhoto");
            helper.AddElement("Condition");

            foreach (string StudentID in StudentIDs)
                if (!string.IsNullOrEmpty(StudentID))
                    helper.AddElement("Condition", "ID", StudentID);

            dsreq.SetContent(helper);

            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            foreach (XmlElement element in dsrsp.GetContent().GetElements("Student"))
            {
                string strStudentID = element.GetAttribute("ID");
                string strGraduatePhoto = element.SelectSingleNode("GraduatePhoto").InnerText;
                PhotoList.Add(strStudentID, strGraduatePhoto);
            }

            return PhotoList;
        }

        /// <summary>
        /// 取得學生畢業照片
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>照片內容</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static string SelectGraduatePhoto(string StudentID)
        {
            DSRequest dsreq = new DSRequest();
            DSXmlHelper helper = new DSXmlHelper("GetStudentListRequest");
            helper.AddElement("Field");
            helper.AddElement("Field", "GraduatePhoto");
            helper.AddElement("Condition");
            helper.AddElement("Condition", "ID", StudentID);
            dsreq.SetContent(helper);

            DSResponse dsrsp = DSAServices.CallService(SELECT_SERVICENAME, dsreq);

            string strFreshmanPhoto = dsrsp.GetContent().GetText("Student/GraduatePhoto");

            return strFreshmanPhoto;
        }
    }
}