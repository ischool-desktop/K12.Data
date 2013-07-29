using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學期德行評量類別，提供方法用來取得、新增、修改及刪除學期德行評量資訊
    /// </summary>
    public class MoralScore 
    { 
        private const string SELECT_SERVICENAME = "SmartSchool.Score.GetSemesterMoralScore";
        private const string INSERT_SERVICENAME = "SmartSchool.Score.InsertSemesterMoralScore";
        private const string UPDATE_SERVICENAME = "SmartSchool.Score.UpdateSemesterMoralScore";
        private const string DELETE_SERVICENAME = "SmartSchool.Score.DeleteSemesterMoralScore";

        /// <summary>
        /// 取得所有學期德行評量列表。
        /// </summary>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectAll();
        ///     </code>
        /// </example>
        public static List<MoralScoreRecord> SelectAll()
        {
            return SelectAll<K12.Data.MoralScoreRecord>();
        }

        /// <summary>
        /// 取得所有學期德行評量列表。
        /// </summary>
        /// <typeparam name="T">學期德行評量記錄物件型別，K12共用為K12.Data.MoralScoreRecord</typeparam>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectAll&lt;K12.Data.MoralScoreRecord&gt;();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:MoralScoreRecord,new()
        {
            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition/></Request>");

            List<T> Types = new List<T>();

            DSXmlHelper helper = DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent();

            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("SemesterMoralScore"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 取得德行評量列表
        /// </summary>
        /// <param name="IDs">德行評量記錄物件編號列表。</param>
        /// <param name="StudentIDs">學生編號列表。</param>
        /// <param name="SchoolYear">學年度。</param>
        /// <param name="Semester">學期。</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        public static List<MoralScoreRecord> Select(IEnumerable<string> IDs, IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester)
        {
            return Select<MoralScoreRecord>(IDs, StudentIDs, SchoolYear, Semester);
        }

        /// <summary>
        /// 取得德行評量列表 
        /// </summary>
        /// <typeparam name="T">德行評量記錄型別，繼承至K12.Data.MoralScoreRecord</typeparam>
        /// <param name="IDs">德行評量記錄物件編號列表。</param>
        /// <param name="StudentIDs">學生編號列表。</param>
        /// <param name="SchoolYear">學年度。</param>
        /// <param name="Semester">學期。</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        protected static List<T> Select<T>(IEnumerable<string> IDs, IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester) where T : MoralScoreRecord, new()
        {
            return Select<T>(IDs, StudentIDs, SchoolYear, Semester,null);
        }

        /// <summary>
        /// 取得德行評量列表 
        /// </summary>
        /// <typeparam name="T">德行評量記錄型別，繼承至K12.Data.MoralScoreRecord</typeparam>
        /// <param name="IDs">德行評量記錄物件編號列表。</param>
        /// <param name="StudentIDs">學生編號列表。</param>
        /// <param name="SchoolYear">學年度。</param>
        /// <param name="Semester">學期。</param>
        /// <param name="SchoolYearSemesters">學年度學期列表。</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        public static List<MoralScoreRecord> Select(IEnumerable<string> IDs, IEnumerable<string> StudentIDs, int? SchoolYear, int? Semester, IEnumerable<SchoolYearSemester> SchoolYearSemesters)
        {
            return Select<MoralScoreRecord>(IDs, StudentIDs, SchoolYear, Semester, SchoolYearSemesters);
        }

        /// <summary>
        /// 取得德行評量列表 
        /// </summary>
        /// <typeparam name="T">德行評量記錄型別，繼承至K12.Data.MoralScoreRecord</typeparam>
        /// <param name="IDs">德行評量記錄物件編號列表。</param>
        /// <param name="StudentIDs">學生編號列表。</param>
        /// <param name="SchoolYear">學年度。</param>
        /// <param name="Semester">學期。</param>
        /// <param name="SchoolYearSemesters">學年度學期列表。</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> Select<T>(IEnumerable<string> IDs,IEnumerable<string> StudentIDs,int? SchoolYear,int? Semester,IEnumerable<SchoolYearSemester> SchoolYearSemesters) where T : MoralScoreRecord, new()
        {
            bool IsSendRequest = false;

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(IDs))
            {
                helper.AddElement("Condition","IDList");
                foreach (string ID in IDs)
                    if (!string.IsNullOrEmpty(ID))
                    {
                        helper.AddElement("Condition/IDList", "ID", ID);
                        IsSendRequest = true;
                    }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                helper.AddElement("Condition", "StudentIDList");

                foreach (string StudentID in StudentIDs)
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        helper.AddElement("Condition/StudentIDList", "ID", StudentID);
                        IsSendRequest = true;
                    }
            }

            if (!K12.Data.Int.IsNullOrEmpty(SchoolYear))
            {
                helper.AddElement("Condition", "SchoolYear", K12.Data.Int.GetString(SchoolYear));
                IsSendRequest = true;
            }

            if (!K12.Data.Int.IsNullOrEmpty(Semester))
            {
                helper.AddElement("Condition", "Semester", K12.Data.Int.GetString(Semester));
                IsSendRequest = true;
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(SchoolYearSemesters))
            {
                helper.AddElement("Condition", "Or");

                foreach (SchoolYearSemester schoolYearSemester in SchoolYearSemesters)
                {
                    XmlElement Element = helper.AddElement("Condition/Or", "And");

                    XmlElement ElmSchoolYear = Element.OwnerDocument.CreateElement("SchoolYear");

                    ElmSchoolYear.InnerText = K12.Data.Int.GetString(schoolYearSemester.SchoolYear);

                    XmlElement ElmSemester = Element.OwnerDocument.CreateElement("Semester");

                    ElmSemester.InnerText = K12.Data.Int.GetString(schoolYearSemester.Semester);

                    Element.AppendChild(ElmSchoolYear);
                    Element.AppendChild(ElmSemester);
                }

                IsSendRequest = true;
            }

            List<T> Types = new List<T>();

            if (IsSendRequest)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("SemesterMoralScore"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 根據學生編號、學年度及學期取得學期德行評量列表。
        /// </summary>
        /// <param name="StudentID">學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectBySchoolYearAndSemester(StudentID,SchoolYear,Semester);
        ///     </code>
        /// </example>
        public static MoralScoreRecord SelectBySchoolYearAndSemester(string StudentID, int SchoolYear, int Semester)
        {
            return SelectBySchoolYearAndSemester<K12.Data.MoralScoreRecord>(StudentID, SchoolYear, Semester);
        }

        /// <summary>
        /// 根據學生編號、學年度及學期取得學期德行評量列表。
        /// </summary>
        /// <typeparam name="T">學期德行評量記錄物件型別，K12共用為K12.Data.MoralScoreRecord</typeparam>
        /// <param name="StudentID">學生編號</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectBySchoolYearAndSemester&lt;K12.Data.MoralScoreRecord&gt;(StudentID,SchoolYear,Semester);
        ///     </code>
        /// </example>
        protected static T SelectBySchoolYearAndSemester<T>(string StudentID,int SchoolYear,int Semester) where T:MoralScoreRecord,new()
        {
            List<string> StudentIDs = new List<string>();

            if (!string.IsNullOrEmpty(StudentID))
                StudentIDs.Add(StudentID);

            List<T> Types = Select<T>(null, StudentIDs , SchoolYear, Semester);

            return Types.Count > 0 ? Types[0] : null;
        }

        public static List<MoralScoreRecord> SelectBySchoolYearAndSemesterLessEqual(IEnumerable<string> StudentIDs, SchoolYearSemester SchoolYearSemester)
        {
            return SelectBySchoolYearAndSemesterLessEqual<MoralScoreRecord>(StudentIDs,SchoolYearSemester);
        }

        protected static List<T> SelectBySchoolYearAndSemesterLessEqual<T>(IEnumerable<string> StudentIDs, SchoolYearSemester SchoolYearSemester) where T : MoralScoreRecord , new()
        {
            bool IsSendRequest = false;

            DSXmlHelper helper = new DSXmlHelper("Request");

            helper.AddElement("Field");
            helper.AddElement("Field", "All");
            helper.AddElement("Condition");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                helper.AddElement("Condition", "StudentIDList");

                foreach (string StudentID in StudentIDs)
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        helper.AddElement("Condition/StudentIDList", "ID", StudentID);
                        IsSendRequest = true;
                    }
            }

            string strSchoolYearSemesterCondition = SchoolYearSemester.ToLessThanRequest();

            if (!string.IsNullOrEmpty(strSchoolYearSemesterCondition))
            {
                helper.AddXmlString("Condition",strSchoolYearSemesterCondition);
                IsSendRequest = true;
            }

            helper.AddElement(".", "Order");
            helper.AddElement("Order", "SchoolYear","desc");
            helper.AddElement("Order","Semester","desc");

            List<T> Types = new List<T>();

            if (IsSendRequest)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(helper)).GetContent().GetElements("SemesterMoralScore"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }

            return Types; 
        }

        /// <summary>
        /// 根據多筆學生編號取得學期德行評量列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectByStudentIDs(StudentIDs);
        ///     </code>
        /// </example>
        public static List<MoralScoreRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.MoralScoreRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學期德行評量列表。
        /// </summary>
        /// <typeparam name="T">學期德行評量記錄物件型別，K12共用為K12.Data.MoralScoreRecord</typeparam>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;MoralScoreRecord&gt;，代表多筆學期德行評量記錄物件。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;MoralScoreRecord&gt; records = MoralScore.SelectByStudentIDs&lt;K12.Data.MoralScoreRecord&gt;(StudentIDs);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:MoralScoreRecord,new()
        {
            return Select<T>(null, StudentIDs, null, null);
        }

        /// <summary>
        /// 新增單筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecord">學期德行評量記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static string Insert(MoralScoreRecord MoralScoreRecord)
        {
            List<MoralScoreRecord> Params = new List<MoralScoreRecord>();

            Params.Add(MoralScoreRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecords">多筆學期德行評量記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static List<string> Insert(IEnumerable<MoralScoreRecord> MoralScoreRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<MoralScoreRecord> worker = new MultiThreadWorker<MoralScoreRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<MoralScoreRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    helper.AddElement("SemesterMoralScore");
                    helper.AddElement("SemesterMoralScore", "RefStudentID", editor.RefStudentID);
                    helper.AddElement("SemesterMoralScore", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("SemesterMoralScore", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("SemesterMoralScore", "SupervisedByComment", editor.Comment);
                    helper.AddElement("SemesterMoralScore", "SupervisedByDiff", K12.Data.Decimal.GetString(editor.Diff));

                    helper.AddElement("SemesterMoralScore", "OtherDiff", "");

                    if (editor.OtherDiff != null)
                        helper.AddElement("SemesterMoralScore/OtherDiff", editor.OtherDiff);

                    helper.AddElement("SemesterMoralScore", "TextScore", "");
                    
                    if (editor.TextScore != null)
                        helper.AddElement("SemesterMoralScore/TextScore", editor.TextScore);

                    helper.AddElement("SemesterMoralScore", "Summary", "");

                    if (editor.Summary != null)
                        helper.AddElement("SemesterMoralScore/Summary", editor.Summary);

                    helper.AddElement("SemesterMoralScore", "InitialSummary", "");

                    if (editor.InitialSummary != null)
                        helper.AddElement("SemesterMoralScore/InitialSummary", editor.InitialSummary);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<MoralScoreRecord>> packages = worker.Run(MoralScoreRecords);

            foreach (PackageWorkEventArgs<MoralScoreRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert!=null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecord">學期德行評量記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        public static int Update(MoralScoreRecord MoralScoreRecord)
        {
            List<MoralScoreRecord> Params = new List<MoralScoreRecord>();

            Params.Add(MoralScoreRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecords">多筆學期德行評量記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<MoralScoreRecord> MoralScoreRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<MoralScoreRecord> worker = new MultiThreadWorker<MoralScoreRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<MoralScoreRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("SemesterMoralScore");
    
                    updateHelper.AddElement("SemesterMoralScore", "TextScore", "");
                    if (editor.TextScore!=null)
                        updateHelper.AddElement("SemesterMoralScore/TextScore", editor.TextScore);

                    updateHelper.AddElement("SemesterMoralScore", "Summary", "");
                    if (editor.Summary != null) 
                        updateHelper.AddElement("SemesterMoralScore/Summary", editor.Summary);
    
                    updateHelper.AddElement("SemesterMoralScore", "InitialSummary", "");
                    if (editor.InitialSummary!=null)
                        updateHelper.AddElement("SemesterMoralScore/InitialSummary", editor.InitialSummary);

                    updateHelper.AddElement("SemesterMoralScore", "SupervisedByComment", editor.Comment);
                    updateHelper.AddElement("SemesterMoralScore", "SupervisedByDiff", K12.Data.Decimal.GetString(editor.Diff));

                    updateHelper.AddElement("SemesterMoralScore", "OtherDiff", "");
                    if (editor.OtherDiff != null)
                        updateHelper.AddElement("SemesterMoralScore/OtherDiff", editor.OtherDiff);

                    updateHelper.AddElement("SemesterMoralScore", "Condition");
                    updateHelper.AddElement("SemesterMoralScore/Condition", "ID", editor.ID);
                    updateHelper.AddElement("SemesterMoralScore/Condition", "RefStudentID", editor.RefStudentID);
                    updateHelper.AddElement("SemesterMoralScore/Condition", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("SemesterMoralScore/Condition", "Semester", K12.Data.Int.GetString(editor.Semester));

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<MoralScoreRecord>> packages = worker.Run(MoralScoreRecords);

            foreach (PackageWorkEventArgs<MoralScoreRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate!=null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecord">學期德行評量記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        static public int Delete(MoralScoreRecord MoralScoreRecord)
        {
            return Delete(MoralScoreRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreID">學期德行評量記錄編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        static public int Delete(string MoralScoreID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(MoralScoreID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreRecords">多筆學期德行評量記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="MoralScoreRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<MoralScoreRecord> MoralScoreRecords)
        {
            List<string> Keys = new List<string>();

            foreach (MoralScoreRecord MoralScoreRecord in MoralScoreRecords)
                Keys.Add(MoralScoreRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學期德行評量記錄
        /// </summary>
        /// <param name="MoralScoreIDs">多筆學期德行評量記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> MoralScoreIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (string Key in e.List)
                {
                    helper.AddElement("SemesterMoralScore");
                    helper.AddElement("SemesterMoralScore", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(MoralScoreIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete!=null)
                AfterDelete(null, new DataChangedEventArgs(MoralScoreIDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 新增之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterInsert;

        /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;

        /// <summary>
        /// 刪除之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterDelete;
    }
}