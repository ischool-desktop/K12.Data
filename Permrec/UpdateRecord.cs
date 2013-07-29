using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 學生異動記錄類別，提供方法用來取得、新增、修改及刪除學生異動記錄
    /// </summary>
    public class UpdateRecord
    {
        private const string SELECT_UPDATERECORD = "SmartSchool.Student.UpdateRecord.GetDetailList";
        private const string UPDATE_UPDATERECORD = "SmartSchool.Student.UpdateRecord.Update";
        private const string INSERT_UPDATERECORD = "SmartSchool.Student.UpdateRecord.Insert";
        private const string DELET_DELETERECORD = "SmartSchool.Student.UpdateRecord.Delete";

        /// <summary>
        /// 取得所有學生異動記錄列表。
        /// </summary>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectAll();
        ///     
        ///     foreach(UpdateRecordRecord record in records)
        ///         Console.WrlteLine(record.StudentName);
        ///     </code>
        /// </example>
        [SelectMethod("K12.UpdateRecord.SelectAll", "學籍.異動記錄")]
        public static List<UpdateRecordRecord> SelectAll()
        {
            return SelectAll<UpdateRecordRecord>();
        }


        /// <summary>
        /// 取得所有學生異動記錄列表。
        /// </summary>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectAll();
        ///     </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:UpdateRecordRecord,new()
        {
            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition/><Order/></Request>");

            List<T> Types = new List<T>();

            foreach (XmlElement item in DSAServices.CallService(SELECT_UPDATERECORD, new DSRequest(req.ToString())).GetContent().GetElements("UpdateRecord"))
            {
                T Type = new T();
                Type.Load(item);
                Types.Add(Type);             
            }

            return Types;
        }

        /// <summary>
        /// 根據單筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄列表。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; record = UpdateRecord.SelectByStudentID(StudentID);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        /// <remarks>若是StudentID不則在則會傳回null</remarks>
        public static List<UpdateRecordRecord> SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<UpdateRecordRecord>(StudentID);
        }

        /// <summary>
        /// 根據單筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="StudentID">學生記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄列表。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudentID(StudentID);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentID<T>(string StudentID) where T:UpdateRecordRecord,new()
        {
            List<string> Params = new List<string>();

            Params.Add(StudentID);

            return SelectByStudentIDs<T>(Params.ToArray());
        }

        /// <summary>
        /// 根據單筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄列表。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; record = UpdateRecord.SelectByStudent(Student);
        ///     
        ///     if (record != null)
        ///        System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        public static List<UpdateRecordRecord> SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<UpdateRecordRecord>(Student);
        }


        /// <summary>
        /// 根據單筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="Student">學生記錄物件</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄列表。</returns>
        /// <seealso cref="StudentRecord"/>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudentID(StudentID);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudent<T>(StudentRecord Student) where T:UpdateRecordRecord,new()
        {
            List<StudentRecord> Params = new List<StudentRecord>();

            Params.Add(Student);

            return SelectByStudents<T>(Params);
        }

        /// <summary>
        /// 根據多筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudentIDs(StudentIDs);
        ///     
        ///     foreach(UpdateRecordRecord record in records)
        ///         System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        public static List<UpdateRecordRecord> SelectByStudentIDs(IEnumerable<string> StudentIDs)
        {
            return SelectByStudentIDs<UpdateRecordRecord>(StudentIDs); 
        }


        /// <summary>
        /// 根據多筆學生編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="StudentIDs">多筆學生記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudentIDs(StudentIDs);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T:UpdateRecordRecord,new()
        {
            return GetUpdateRecords<T>(null,(IEnumerable<string>)StudentIDs,null);
        }

        /// <summary>
        /// 根據多筆異動記錄編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="IDs">多筆異動記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByIDs(IDs);
        ///     
        ///     foreach(UpdateRecordRecord record in records)
        ///         System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        public static List<UpdateRecordRecord> SelectByIDs(IEnumerable<string> IDs)
        {
            return SelectByIDs<UpdateRecordRecord>(IDs);
        }

        /// <summary>
        /// 根據多筆異動記錄編號取得學生異動記錄列表。
        /// </summary>
        /// <param name="IDs">多筆異動記錄編號</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByIDs(IDs);
        ///     
        ///     foreach(UpdateRecordRecord record in records)
        ///         System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        protected static List<T> SelectByIDs<T>(IEnumerable<string> IDs) where T : UpdateRecordRecord, new()
        {
            return GetUpdateRecords<T>(IDs,null,null);
        }

        /// <summary>
        /// 根據多筆學生物件取得學生異動記錄列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudents(Students);
        ///     
        ///     foreach(UpdateRecordRecord record in records)
        ///         System.Console.WriteLine(record.StudentName);
        ///     </code>
        /// </example>
        public static List<UpdateRecordRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<UpdateRecordRecord>(Students);
        }

        /// <summary>
        /// 根據多筆學生物件取得學生異動記錄列表。
        /// </summary>
        /// <param name="Students">多筆學生記錄物件</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByStudents(Students);
        ///     </code>
        /// </example>
        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T:UpdateRecordRecord,new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return GetUpdateRecords<T>(null,Keys,null);
        }

        /// <summary>
        /// 根據異動代碼取得學生異動記錄列表。
        /// </summary>
        /// <param name="UpdateCodes">多個異動代碼</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByUpdateCodes(UpdateCodes);
        ///     </code>
        /// </example>
        public static List<UpdateRecordRecord> SelectByUpdateCodes(IEnumerable<string> UpdateCodes)
        {
            return SelectByUpdateCodes<UpdateRecordRecord>(UpdateCodes);
        }

        /// <summary>
        /// 根據異動代碼取得學生異動記錄列表。
        /// </summary>
        /// <param name="UpdateCodes">多個異動代碼</param>
        /// <returns>List&lt;UpdateRecordRecord&gt;，代表多筆學生異動記錄物件。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByUpdateCodes(UpdateCodes);
        ///     </code>
        /// </example>
        protected static List<T> SelectByUpdateCodes<T>(IEnumerable<string> UpdateCodes) where T : UpdateRecordRecord, new()
        {
            return GetUpdateRecords<T>(null, null,UpdateCodes);
        }

        /// <summary>
        /// 新增單筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecord">學生異動記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         UpdateRecordRecord newrecord = new UpdateRecordRecord();
        ///         newrecord.StudentID = StudentID;
        ///         newrecord.UpdateDate = "2009/9/9";
        ///         strng NewID = UpdateRecord.Insert(newrecord)
        ///         UpdateRecordRecord record = UpdateRecord.SelectByID(NewID);
        ///         System.Console.Writeln(record.Name);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 1.新增一律傳回新增物件的編號。
        /// 2.新增必填欄位為學生編號及異動日期。
        /// </remarks>
        public static string Insert(UpdateRecordRecord UpdateRecordRecord)
        {
            List<UpdateRecordRecord> Params = new List<UpdateRecordRecord>();

            Params.Add(UpdateRecordRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecord">多筆學生異動記錄物件</param> 
        /// <returns>List&lt;string&gt;，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///         UpdateRecordRecord record = new UpdateRecordRecord();
        ///         record.StudentID = StudentID;
        ///         record.UpdateDate = "2009/9/9";
        ///         List&lt;UpdateRecordRecord&gt; records = new List&lt;UpdateRecordRecord&gt;();
        ///         records.Add(record);
        ///         List&lt;string&gt; NewID = UpdateRecord.Insert(records)
        ///     </code>
        /// </example>
        public static List<string> Insert(IEnumerable<UpdateRecordRecord> UpdateRecordRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<UpdateRecordRecord> worker = new MultiThreadWorker<UpdateRecordRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<UpdateRecordRecord> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    if (editor.UpdateDate.Equals(String.Empty) || editor.StudentID.Equals(String.Empty))
                        throw new Exception("學生編號(StudentID)或異動日期(UpdateDate)不得為空白");

                    helper.AddElement("UpdateRecord");
                    helper.AddElement("UpdateRecord", "Field");

                    helper.AddElement("UpdateRecord/Field","SchoolYear",K12.Data.Int.GetString(editor.SchoolYear));
                    helper.AddElement("UpdateRecord/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    helper.AddElement("UpdateRecord/Field", "RefStudentID", editor.StudentID);
                    helper.AddElement("UpdateRecord/Field", "Name", editor.StudentName);
                    helper.AddElement("UpdateRecord/Field", "StudentNumber", editor.StudentNumber);
                    helper.AddElement("UpdateRecord/Field", "Gender", editor.Gender);
                    helper.AddElement("UpdateRecord/Field", "IDNumber", editor.IDNumber);
                    helper.AddElement("UpdateRecord/Field", "Birthdate", editor.Birthdate);
                    helper.AddElement("UpdateRecord/Field", "GradeYear", editor.GradeYear);

                    //解析異動日期並且再輸出成字串，確保日期格式正確
                    string strUpdateDate = K12.Data.DateTimeHelper.ToDisplayString(K12.Data.DateTimeHelper.Parse(editor.UpdateDate));

                    helper.AddElement("UpdateRecord/Field", "UpdateDate", strUpdateDate);
                    helper.AddElement("UpdateRecord/Field", "UpdateCode", editor.UpdateCode);
                    helper.AddElement("UpdateRecord/Field", "UpdateDescription", editor.UpdateDescription);
                    helper.AddElement("UpdateRecord/Field", "ADDate", editor.ADDate);
                    helper.AddElement("UpdateRecord/Field", "ADNumber", editor.ADNumber);
                    helper.AddElement("UpdateRecord/Field", "LastADDate", editor.LastADDate);
                    helper.AddElement("UpdateRecord/Field", "LastADNumber", editor.LastADNumber);
                    helper.AddElement("UpdateRecord/Field", "Comment", editor.Comment);
                    helper.AddElement("UpdateRecord/Field","Department",editor.Department);
                    helper.AddElement("UpdateRecord/Field", "ContextInfo", GetContextInfoXml(editor.Attributes), true);
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_UPDATERECORD, new DSRequest(helper.BaseElement)).GetContent();
                
                foreach(XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<UpdateRecordRecord>> packages = worker.Run(UpdateRecordRecords);

            foreach (PackageWorkEventArgs<UpdateRecordRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));


            return result;
        }

        /// <summary>
        /// 更新單筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecord">學生異動記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     UpdateRecordRecord record = UpdateRecord.SelectByID(ClassID);
        ///     record.StudentName = (new System.Random()).NextDouble().ToString();
        ///     int UpdateCount = UpdateRecord.Update(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        public static int Update(UpdateRecordRecord UpdateRecordRecord)
        {
            List<UpdateRecordRecord> Params = new List<UpdateRecordRecord>();

            Params.Add(UpdateRecordRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecords">多筆學生異動記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///     UpdateRecordRecord record = UpdateRecord.SelectByID(UpdateRecordID);
        ///     record.StudentName = (new System.Random()).NextDouble().ToString();
        ///     List&lt;UpdateRecordRecord&gt; records = new List&lt;UpdateRecordRecord&gt;();
        ///     records.Add(record);
        ///     int UpdateCount = UpdateRecord.Update(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功更新的筆數。</remarks>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<UpdateRecordRecord> UpdateRecordRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<UpdateRecordRecord> worker = new MultiThreadWorker<UpdateRecordRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<UpdateRecordRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("UpdateRecord");
                    updateHelper.AddElement("UpdateRecord", "Field");

                    updateHelper.AddElement("UpdateRecord/Field", "RefStudentID", editor.StudentID);
                    updateHelper.AddElement("UpdateRecord/Field", "SchoolYear", K12.Data.Int.GetString(editor.SchoolYear));
                    updateHelper.AddElement("UpdateRecord/Field", "Semester", K12.Data.Int.GetString(editor.Semester));
                    updateHelper.AddElement("UpdateRecord/Field", "Name", editor.StudentName);
                    updateHelper.AddElement("UpdateRecord/Field", "StudentNumber", editor.StudentNumber);
                    updateHelper.AddElement("UpdateRecord/Field", "Gender", editor.Gender);
                    updateHelper.AddElement("UpdateRecord/Field", "IDNumber", editor.IDNumber);
                    updateHelper.AddElement("UpdateRecord/Field", "Birthdate", editor.Birthdate);
                    updateHelper.AddElement("UpdateRecord/Field", "GradeYear", editor.GradeYear);

                    //解析異動日期並且再輸出成字串，確保日期格式正確
                    string strUpdateDate = K12.Data.DateTimeHelper.ToDisplayString(K12.Data.DateTimeHelper.Parse(editor.UpdateDate));

                    updateHelper.AddElement("UpdateRecord/Field", "UpdateDate", strUpdateDate);
                    updateHelper.AddElement("UpdateRecord/Field", "UpdateCode", editor.UpdateCode);
                    updateHelper.AddElement("UpdateRecord/Field", "UpdateDescription", editor.UpdateDescription);
                    updateHelper.AddElement("UpdateRecord/Field", "ADDate", editor.ADDate);
                    updateHelper.AddElement("UpdateRecord/Field", "ADNumber", editor.ADNumber);
                    updateHelper.AddElement("UpdateRecord/Field", "LastADDate", editor.LastADDate);
                    updateHelper.AddElement("UpdateRecord/Field", "LastADNumber", editor.LastADNumber);
                    updateHelper.AddElement("UpdateRecord/Field", "Comment", editor.Comment);
                    updateHelper.AddElement("UpdateRecord/Field", "Department", editor.Department);
                    updateHelper.AddElement("UpdateRecord/Field", "ContextInfo", GetContextInfoXml(editor.Attributes), true);
                    updateHelper.AddElement("UpdateRecord", "Condition");
                    updateHelper.AddElement("UpdateRecord/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result=int.Parse(DSAServices.CallService(UPDATE_UPDATERECORD, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<UpdateRecordRecord>> packages = worker.Run(UpdateRecordRecords);

            foreach (PackageWorkEventArgs<UpdateRecordRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }


        /// <summary>
        /// 刪除多筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecords">多筆學生異動記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="UpdateRecordRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       List&lt;UpdateRecordRecord&gt; records = UpdateRecord.SelectByIDs(UpdateRecordIDs);
        ///       int DeleteCount = UpdateRecord.Delete(records);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(IEnumerable<UpdateRecordRecord> UpdateRecordRecords)
        {
            List<string> Keys = new List<string>();

            foreach (UpdateRecordRecord updateRecordRecord in UpdateRecordRecords)
                Keys.Add(updateRecordRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordIDs">多筆學生異動記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///    <code>
        ///    int DeleteCount = UpdateRecord.Delete(UpdateRecordIDs);
        ///    </code>
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> UpdateRecordIDs)
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
                    helper.AddElement("UpdateRecord");
                    helper.AddElement("UpdateRecord", "ID", Key);
                }

                result = int.Parse(DSAServices.CallService(DELET_DELETERECORD, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(UpdateRecordIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(UpdateRecordIDs, ChangedSource.Local));

            if (AfterChange != null)
                AfterChange(null, new DataChangedEventArgs(UpdateRecordIDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordRecord">單筆學生異動記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       UpdateRecordRecord&gt; record = UpdateRecord.SelectByID(UpdateRecordID);
        ///       int DeleteCount = UpdateRecord.Delete(record);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(UpdateRecordRecord UpdateRecordRecord)
        {
            return Delete(UpdateRecordRecord.ID);
        }

        /// <summary>
        /// 刪除單筆學生異動記錄
        /// </summary>
        /// <param name="UpdateRecordID">單筆學生異動記錄編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     <code>
        ///       int DeleteCount = UpdateRecord.Delete(UpdateRecordID);
        ///     </code>
        /// </example>
        /// <remarks>傳回值為成功刪除的筆數。</remarks>
        static public int Delete(string UpdateRecordID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(UpdateRecordID);

            return Delete(Keys);
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

        [FISCA.Authentication.AutoRetryOnWebException()]
        private static List<T> GetUpdateRecords<T>(IEnumerable<string> IDs,IEnumerable<string> StudentIDs,IEnumerable<string> UpdateCodes) where T:UpdateRecordRecord,new()
        {
            bool haskey = false;
            StringBuilder req = new StringBuilder("<Request><Field><All/></Field><Condition>");

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(IDs))
            {
                foreach (string ID in IDs)
                {
                    if (!string.IsNullOrEmpty(ID))
                    {
                        req.Append("<ID>"+ID+"</ID>");
                        haskey = true;
                    }
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(StudentIDs))
            {
                foreach (string StudentID in StudentIDs)
                {
                    if (!string.IsNullOrEmpty(StudentID))
                    {
                        req.Append("<RefStudentID>" + StudentID + "</RefStudentID>");
                        haskey = true;
                    }
                }
            }

            if (!K12.Data.Utility.Utility.IsNullOrEmpty(UpdateCodes))
            {
                foreach (string UpdateCode in UpdateCodes)
                {
                    if (!string.IsNullOrEmpty(UpdateCode))
                    {
                        req.Append("<UpdateCode>"+UpdateCode+"</UpdateCode>");
                        haskey = true;
                    }                    

                    if (haskey)
                        req.Append("<Special><NonADNumber/><StudentNotDeleted/></Special>");
                }
            }

            req.Append("</Condition><Order></Order></Request>");

            List<T> Types = new List<T>();
            
            if (haskey)
            {
                foreach (XmlElement item in DSAServices.CallService(SELECT_UPDATERECORD, new DSRequest(req.ToString())).GetContent().GetElements("UpdateRecord"))
                {
                    T Type= new T();
                    Type.Load(item);
                    Types.Add(Type);
                }
            }
            return Types;
        }

        private static string GetContextInfoXml(AutoDictionary autoDictionary)
        {
            if (autoDictionary == null) return "<ContextInfo/>";

            StringBuilder builder = new StringBuilder();
            builder.Append("<ContextInfo>");
            foreach (KeyValuePair<string, string> each in autoDictionary)
            {
                builder.AppendFormat("<{0}> {1}</{2}>", each.Key, each.Value, each.Key);
            }
            builder.Append("</ContextInfo>");

            return builder.ToString();
        }
    }
}