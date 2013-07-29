using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生獎懲類別，提供方法用來取得、新增、修改及刪除學生獎懲資訊
    /// </summary>
    public class Discipline
    {
        private const string SELECT_SERVICENAME = "SmartSchool.Student.Discipline.GetDiscipline";
        private const string INSERT_SERVICENAME = "SmartSchool.Student.Discipline.Insert";
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.Discipline.Update";
        private const string DELETE_SERVICENAME = "SmartSchool.Student.Discipline.Delete";

        /// <summary>
        /// 取得所有學生獎懲記錄物件列表。
        /// </summary>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectAll();
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        [SelectMethod("K12.Discipline.SelectAll", "學務.獎懲")]
        public static List<DisciplineRecord> SelectAll()
        {
            return SelectAll<K12.Data.DisciplineRecord>();
        }

        /// <summary>
        /// 取得所有學生獎懲記錄物件列表。
        /// </summary>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎勵記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectAll();
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         System.Console.WriteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : DisciplineRecord, new()
        {
            List<T> Types = new List<T>();

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field>");
            req.Append("<Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //Invoke DSA Services and parse the response doc into MeritRecord objects.
            foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);
            }

            return Types;
        }
        
        /// <summary>
        /// 根據多筆學生編號取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         Console.WrlteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<DisciplineRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.DisciplineRecord>(StudentIDs);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : DisciplineRecord, new()
        {
            List<T> Types = new List<T>();

            bool haskey = false;

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");
            foreach (string key in StudentIDs)
            {
                req.Append("<RefStudentID>" + key + "</RefStudentID>");
                haskey = true;
            }

            req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //如果有傳學生ID進來
            if (haskey)
            {
                //Invoke DSA Services and parse the response doc into DisciplineRecord objects.
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        /// <summary>
        /// 根據多筆學生編號及登錄日期取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartRegisterDate">登錄開始日期</param>
        /// <param name="EndRegisterDate">登錄結束日期</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectRegisterDate(StudentIDs,StartRegisterDate,EndRegisterDate);
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         Console.WrlteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("此方法已不再使用。")] 
        public static List<DisciplineRecord> SelectRegisterDate(IEnumerable<string> StudentIDs, DateTime? StartRegisterDate, DateTime? EndRegisterDate)
        {
            return SelectByRegisterDate(StudentIDs, StartRegisterDate, EndRegisterDate);
        }

        /// <summary>
        /// 根據多筆學生編號及登錄日期取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartRegisterDate">登錄開始日期</param>
        /// <param name="EndRegisterDate">登錄結束日期</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectRegisterDate(StudentIDs,StartRegisterDate,EndRegisterDate);
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         Console.WrlteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<DisciplineRecord> SelectByRegisterDate(IEnumerable<string> StudentIDs, DateTime? StartRegisterDate, DateTime? EndRegisterDate)
        {
            return K12.Data.Discipline.SelectByRegisterDate<DisciplineRecord>(StudentIDs, StartRegisterDate, EndRegisterDate);
        }


        /// <summary>
        /// 根據多筆學生編號及登錄日期取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartRegisterDate">登錄開始日期</param>
        /// <param name="EndRegisterDate">登錄結束日期</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByRegisterDate<T>(IEnumerable<string> StudentIDs, DateTime? StartRegisterDate, DateTime? EndRegisterDate) where T : DisciplineRecord, new()
        {
            List<T> Types = new List<T>();

            bool haskey = false;

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                foreach (string key in StudentIDs)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        req.Append("<RefStudentID>" + key + "</RefStudentID>");
                        haskey = true;
                    }
                }
            }

            if (StartRegisterDate != null)
            {
                req.Append("<StartRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartRegisterDate) + "</StartRegisterDate>");
                haskey = true;
            }

            if (EndRegisterDate != null)
            {
                req.Append("<EndRegisterDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndRegisterDate) + "</EndRegisterDate>");
                haskey = true;
            }

            req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //如果有傳學生ID進來
            if (haskey)
            {
                //Invoke DSA Services and parse the response doc into DisciplineRecord objects.
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        /// <summary>
        /// 根據多筆學生編號及發生日期取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartOccurDate">發生開始日期</param>
        /// <param name="EndOccurDate">發生結束日期</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;DisciplineRecord&gt; records = Discipline.SelectByOccurDate(StudentIDs,StartOccurDate,EndOccurDate);
        ///     
        ///     foreach(DisciplineRecord record in records)
        ///         Console.WrlteLine(record.RefStudentID);
        ///     </code>
        /// </example>
        public static List<DisciplineRecord> SelectByOccurDate(IEnumerable<string> StudentIDs, DateTime? StartOccurDate, DateTime? EndOccurDate)
        {
            return K12.Data.Discipline.SelectByOccurDate<DisciplineRecord>(StudentIDs, StartOccurDate, EndOccurDate);
        }


        /// <summary>
        /// 根據多筆學生編號及發生日期取得學生獎懲記錄物件列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生編號</param>
        /// <param name="StartOccurDate">發生開始日期</param>
        /// <param name="EndOccurDate">發生結束日期</param>
        /// <returns>List&lt;DisciplineRecord&gt;，代表多筆學生獎懲記錄物件。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByOccurDate<T>(IEnumerable<string> StudentIDs,DateTime? StartOccurDate,DateTime? EndOccurDate) where T : DisciplineRecord, new()
        {
            List<T> Types = new List<T>();

            bool haskey = false;

            //建立 Request Document.
            StringBuilder req = new StringBuilder("<SelectRequest><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                foreach (string key in StudentIDs)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        req.Append("<RefStudentID>" + key + "</RefStudentID>");
                        haskey = true;
                    }
                }
            }

            if (StartOccurDate != null)
            {
                req.Append("<StartDate>" + K12.Data.DateTimeHelper.ToDisplayString(StartOccurDate) + "</StartDate>");
                haskey = true;
            }

            if (EndOccurDate != null)
            {
                req.Append("<EndDate>" + K12.Data.DateTimeHelper.ToDisplayString(EndOccurDate) + "</EndDate>");
                haskey = true;
            }

            req.Append("</Condition><Order><RefStudentID /><OccurDate>desc</OccurDate></Order></SelectRequest>");

            //如果有傳學生ID進來
            if (haskey)
            {
                //Invoke DSA Services and parse the response doc into DisciplineRecord objects.
                foreach (XmlElement item in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(req.ToString())).GetContent().GetElements("Discipline"))
                {
                    T Type = new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        /// <summary>
        /// 新增單筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineRecord">學生獎懲記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         DisciplineRecord newrecord = new DisciplineRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         strng NewID = Discipline.Insert(newrecord)
        ///         DisciplineRecord record = Discipline.SelectByID(NewID);
        ///         System.Console.Writeln(record.RefStudentID);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為學生記錄編號（RefStudentID）、學年度（SchoolYear）、學期（Semester）、缺曠日期（OccurDate）。
        /// </remarks>
        public static string Insert(DisciplineRecord DisciplineRecord)
        {
            List<DisciplineRecord> Params = new List<DisciplineRecord>();

            Params.Add(DisciplineRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineRecords">多筆學生獎懲記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         DisciplineRecord record = new DisciplineRecord();
        ///         newrecord.RefStudentID = RefStudentID;
        ///         newrecord.SchoolYear = SchoolYear;
        ///         newrecord.Semester = Semester;
        ///         newrecord.OccurDate = DateTime.Today;
        ///         
        ///         List&lt;DisciplineRecord&gt; records = new List&lt;DisciplineRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = Discipline.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<DisciplineRecord> DisciplineRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<DisciplineRecord> worker = new MultiThreadWorker<DisciplineRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<DisciplineRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("InsertRequest");

                foreach (var editor in e.List)
                {
                    helper.AddElement("Discipline");
                    helper.AddElement("Discipline", "RefStudentID", editor.RefStudentID);
                    helper.AddElement("Discipline", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("Discipline", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("Discipline", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));
                    helper.AddElement("Discipline", "RegisterDate", K12.Data.DateTimeHelper.ToDisplayString(editor.RegisterDate));
                    helper.AddElement("Discipline", "Reason", editor.Reason);
                    helper.AddElement("Discipline", "MeritFlag", editor.MeritFlag);
                    helper.AddElement("Discipline", "Type", "1");
                    helper.AddElement("Discipline", "Detail", GetDetailContent(editor), true);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<DisciplineRecord>> packages = worker.Run(DisciplineRecords);

            foreach (PackageWorkEventArgs<DisciplineRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineRecord">學生獎懲記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     DisciplineRecord record = Discipline.SelectByStudentIDs(StudentIDs)[0];
        ///     record.OccurDate = DateTime.Today;
        ///     int UpdateCount = Discipline.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(DisciplineRecord DisciplineRecord)
        {
            List<DisciplineRecord> Params = new List<DisciplineRecord>();

            Params.Add(DisciplineRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生獎勵記錄
        /// </summary>
        /// <param name="DisciplineRecords">多筆學生獎勵記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     DisciplineRecord record = Discipline.SelectByStudentIDs(StudentIDs)[0];
        ///     record.OccurDate = DateTime.Today;
        ///     List&lt;DisciplineRecord&gt; records = new List&lt;DisciplineRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = Discipline.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<DisciplineRecord> DisciplineRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<DisciplineRecord> worker = new MultiThreadWorker<DisciplineRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<DisciplineRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateRequest");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Discipline");
                    updateHelper.AddElement("Discipline", "Field");
                    updateHelper.AddElement("Discipline/Field", "RefStudentID", editor.RefStudentID);
                    updateHelper.AddElement("Discipline/Field", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("Discipline/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    updateHelper.AddElement("Discipline/Field", "OccurDate", K12.Data.DateTimeHelper.ToDisplayString(editor.OccurDate));
                    updateHelper.AddElement("Discipline/Field", "RegisterDate", K12.Data.DateTimeHelper.ToDisplayString(editor.RegisterDate));
                    updateHelper.AddElement("Discipline/Field", "Reason", editor.Reason);
                    updateHelper.AddElement("Discipline/Field", "MeritFlag", editor.MeritFlag);
                    updateHelper.AddElement("Discipline/Field", "Type", "1");
                    updateHelper.AddElement("Discipline/Field", "Detail", GetDetailContent(editor), true);
                    updateHelper.AddElement("Discipline", "Condition");
                    updateHelper.AddElement("Discipline/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<DisciplineRecord>> packages = worker.Run(DisciplineRecords);

            foreach (PackageWorkEventArgs<DisciplineRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineRecord">學生獎懲記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;DisciplineRecord&gt; records = Discipline.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Discipline.Delete(records[0]);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(DisciplineRecord DisciplineRecord)
        {
            return Delete(DisciplineRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineID">學生獎懲記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Discipline.Delete(DisciplineID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string DisciplineID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(DisciplineID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineRecords">多筆學生獎懲記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="DisciplineRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;DisciplineRecord&gt; records = Discipline.SelectByStudentIDs(StudentIDs);
        ///       int DeleteCount = Discipline.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<DisciplineRecord> DisciplineRecords)
        {
            List<string> Keys = new List<string>();

            foreach (DisciplineRecord DisciplineRecord in DisciplineRecords)
                Keys.Add(DisciplineRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生獎懲記錄
        /// </summary>
        /// <param name="DisciplineIDs">多筆學生獎懲記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = Discipline.Delete(DisciplineIDs);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> DisciplineIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper deleteHelper = new DSXmlHelper("DeleteRequest");

                foreach (string Key in e.List)
                {
                    deleteHelper.AddElement("Discipline");
                    deleteHelper.AddElement("Discipline", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(deleteHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(DisciplineIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(DisciplineIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(DisciplineIDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 組合出 Detail 節點的內容。新增和修改的 Request Doc 都會用到。
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        private static string GetDetailContent(DisciplineRecord editor)
        {
            DSXmlHelper helper = new DSXmlHelper("Discipline");
            XmlElement meritelement = helper.AddElement("Merit");

            meritelement.SetAttribute("A", K12.Data.Int.GetString(editor.MeritA));
            meritelement.SetAttribute("B", K12.Data.Int.GetString(editor.MeritB));
            meritelement.SetAttribute("C", K12.Data.Int.GetString(editor.MeritC));

            XmlElement demeritelement = helper.AddElement("Demerit");

            demeritelement.SetAttribute("A", K12.Data.Int.GetString(editor.DemeritA));
            demeritelement.SetAttribute("B", K12.Data.Int.GetString(editor.DemeritB));
            demeritelement.SetAttribute("C", K12.Data.Int.GetString(editor.DemeritC));
            demeritelement.SetAttribute("Cleared", editor.Cleared);
            demeritelement.SetAttribute("ClearDate", K12.Data.DateTimeHelper.ToDisplayString(editor.ClearDate));
            demeritelement.SetAttribute("ClearReason", editor.ClearReason);

            return helper.GetRawXml();
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

        /// <summary>
        /// 資料改變之後所觸發的事件，新增、更新、刪除都會觸發
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterChange;
    }
}