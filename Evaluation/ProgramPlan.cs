using System;
using System.Collections.Generic;
using System.Xml;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 課程規劃類別，提供方法用來取得、新增、修改及刪除課程規劃資訊
    /// </summary>
    public class ProgramPlan
    {
        private const string SELECT_SERVICENAME = "SmartSchool.GraduationPlan.GetDetailList";
        private const string INSERT_SERVICENAME = "SmartSchool.GraduationPlan.Insert";
        private const string UPDATE_SERVICENAME = "SmartSchool.GraduationPlan.Update";
        private const string DELETE_SERVICENAME = "SmartSchool.GraduationPlan.Delete";

        /// <summary>
        /// 取得所有課程規劃明細列表。
        /// </summary>
        /// <returns></returns>
        [SelectMethod("K12.ProgramPlan.SelectAllDetail", "成績.課程規劃")]
        public static List<ProgramSubject> SelectAllDetail()
        {
            List<ProgramSubject> Subjects = new List<ProgramSubject>();

            foreach (ProgramPlanRecord Record in SelectAll())
                foreach (ProgramSubject Subject in Record.Subjects)
                    Subjects.Add(Subject);
            return Subjects;
        }

        /// <summary>
        /// 取得所有課程規劃列表。
        /// </summary>
        /// <returns>List&lt;ProgramPlanRecord&gt;，代表多筆課程規劃記錄物件。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ProgramPlanRecord&gt; records = ProgramPlan.SelectAll();
        /// </example>
        public static List<ProgramPlanRecord> SelectAll()
        {
            return SelectAll<K12.Data.ProgramPlanRecord>();
        }

        /// <summary>
        /// 取得所有課程規劃列表。
        /// </summary>
        /// <returns>List&lt;ProgramPlanRecord&gt;，代表多筆課程規劃記錄物件。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ProgramPlanRecord&gt; records = ProgramPlan.SelectAll();
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T:ProgramPlanRecord,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            List<T> Types = new List<T>();

            request.AddElement(".", "Field", "<ID/><Name/><Content/>", true);
            request.AddElement(".", "Order", "<Name/>", true);

            foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("GraduationPlan"))
            {
                T Type = new T();
                Type.Load(each);
                Types.Add(Type);
            }

            return Types;
        }

        /// <summary>
        /// 根據多筆課程規劃編號取得課程規劃列表。
        /// </summary>
        /// <param name="ProgramPlanIDs">多筆課程規劃編號</param>
        /// <returns>List&lt;ProgramPlanRecord&gt;，代表多筆課程規劃記錄物件。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ProgramPlanRecord&gt; records = ProgramPlan.SelectByIDs(ProgramPlanIDs);
        /// </example>
        public static List<ProgramPlanRecord> SelectByIDs(IEnumerable<string> ProgramPlanIDs)
        {
            return SelectByIDs<K12.Data.ProgramPlanRecord>(ProgramPlanIDs);
        }

        /// <summary>
        /// 根據單筆課程規劃編號取得課程規劃物件。
        /// </summary>
        /// <param name="ProgramPalnID">課程規劃篇號</param>
        /// <returns>ProgramPlanRecord</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        public static ProgramPlanRecord SelectByID(string ProgramPalnID)
        {
            return SelectByID<ProgramPlanRecord>(ProgramPalnID);
        }
        
        /// <summary>
        /// 根據單筆課程規劃編號取得課程規劃物件。
        /// </summary>
        /// <typeparam name="T">課程規劃物件型別</typeparam>
        /// <param name="ProgramPalnID"></param>
        /// <returns>T</returns>
        protected static T SelectByID<T>(string ProgramPalnID) where T : ProgramPlanRecord, new()
        {
            List<string> IDs = new List<string>();

            IDs.Add(ProgramPalnID);

            List<T> Types = SelectByIDs<T>(IDs);

            if (Types.Count > 0)
                return Types[0];
            else
                return null;
        }

        /// <summary>
        /// 根據多筆課程規劃編號取得課程規劃列表。
        /// </summary>
        /// <param name="ProgramPlanIDs">多筆課程規劃編號</param>
        /// <returns>List&lt;ProgramPlanRecord&gt;，代表多筆課程規劃記錄物件。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     List&lt;ProgramPlanRecord&gt; records = ProgramPlan.SelectByIDs(ProgramPlanIDs);
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByIDs<T>(IEnumerable<string> ProgramPlanIDs) where T:ProgramPlanRecord,new()
        {
            DSXmlHelper request = new DSXmlHelper("Request");
            List<T> Types = new List<T>();
            bool execute_required = false;

            request.AddElement(".", "Field", "<ID/><Name/><Content/>", true);
            request.AddElement("Condition");
            request.AddElement("Condition", "IDList");

            foreach (string each in ProgramPlanIDs)
            {
                if (!string.IsNullOrEmpty(each))
                {
                    request.AddElement("Condition/IDList", "ID", each);
                    execute_required = true;
                }
            }

            request.AddElement(".", "Order", "<Name/>", true);

            if (execute_required)
            {
                foreach (XmlElement each in DSAServices.CallService(SELECT_SERVICENAME, new DSRequest(request)).GetContent().GetElements("GraduationPlan"))
                {
                    T Type = new T();
                    Type.Load(each);
                    Types.Add(Type);
                }
            }

            return Types;
        }

        /// <summary>
        /// 新增單筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecord">課程規劃記錄物件</param>
        /// <returns>string，傳回新增物件的系統編號。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        public static string Insert(ProgramPlanRecord ProgramPlanRecord)
        {
            List<ProgramPlanRecord> Params = new List<ProgramPlanRecord>();

            Params.Add(ProgramPlanRecord);

            return Insert(Params)[0];
        }

        /// <summary>
        /// 新增多筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecords">多筆課程規劃記錄物件</param> 
        /// <returns>List&lt;string&gt，傳回新增物件的系統編號列表。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static List<string> Insert(IEnumerable<ProgramPlanRecord> ProgramPlanRecords)
        {
            List<string> result = new List<string>();

            MultiThreadWorker<ProgramPlanRecord> worker = new MultiThreadWorker<ProgramPlanRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ProgramPlanRecord> e)
            {
                DSXmlHelper insertReq = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    insertReq.AddElement("GraduationPlan");
                    insertReq.AddElement("GraduationPlan", "Name", editor.Name);
                    insertReq.AddElement("GraduationPlan", "Content", "<GraduationPlan></GraduationPlan>", true);
                    foreach (var subject in editor.Subjects)
                    {
                        DSXmlHelper helper = new DSXmlHelper("Subject");
                        helper.SetAttribute(".", "GradeYear", K12.Data.Int.GetString(subject.GradeYear));
                        helper.SetAttribute(".", "Semester", K12.Data.Int.GetString(subject.Semester));
                        helper.SetAttribute(".", "Credit", K12.Data.Decimal.GetString(subject.Credit));
                        helper.SetAttribute(".", "Period", K12.Data.Decimal.GetString(subject.Period));
                        helper.SetAttribute(".", "Domain", subject.Domain);
                        helper.SetAttribute(".", "FullName", subject.FullName);
                        helper.SetAttribute(".", "CalcFlag", "" + subject.CalcFlag);
                        helper.SetAttribute(".", "SubjectName", subject.SubjectName);
                        helper.AddElement("Grouping");
                        helper.SetAttribute("Grouping", "RowIndex", "" + subject.RowIndex);
                        helper.SetAttribute("Grouping","startLevel",K12.Data.Int.GetString(subject.StartLevel));

                        //高中專門使用屬性，由騉翔2011/1/16新增
                        helper.SetAttribute(".", "Level", K12.Data.Int.GetString(subject.Level));
                        helper.SetAttribute(".", "NotIncludedInCalc", "" + subject.NotIncludedInCalc);
                        helper.SetAttribute(".", "NotIncludedInCredit", "" + subject.NotIncludedInCredit);
                        helper.SetAttribute(".", "RequiredBy", subject.RequiredBy);
                        helper.SetAttribute(".", "Required", subject.Required ? "必修" : "選修");
                        helper.SetAttribute(".", "Category", subject.Category);
                        helper.SetAttribute(".", "Entry", subject.Entry);

                        insertReq.AddElement("GraduationPlan/Content/GraduationPlan", helper.BaseElement);
                    }
                }

                DSXmlHelper reshelper = DSAServices.CallService(INSERT_SERVICENAME, new DSRequest(insertReq.BaseElement)).GetContent();

                foreach (XmlElement Elm in reshelper.GetElements("NewID"))
                    result.Add(Elm.InnerText);

            };

            List<PackageWorkEventArgs<ProgramPlanRecord>> packages = worker.Run(ProgramPlanRecords);

            foreach (PackageWorkEventArgs<ProgramPlanRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterInsert != null)
                AfterInsert(null, new DataChangedEventArgs(result, ChangedSource.Local));

            if (AfterChange !=null)
                AfterChange(null, new DataChangedEventArgs(result, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新單筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecord">課程規劃記錄物件</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        public static int Update(ProgramPlanRecord ProgramPlanRecord)
        {
            List<ProgramPlanRecord> Params = new List<ProgramPlanRecord>();

            Params.Add(ProgramPlanRecord);

            return Update(Params);
        }

        /// <summary>
        /// 更新多筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecords">多筆課程規劃記錄</param> 
        /// <returns>int，傳回成功更新的筆數。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<ProgramPlanRecord> ProgramPlanRecords)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<ProgramPlanRecord> worker = new MultiThreadWorker<ProgramPlanRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<ProgramPlanRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("Request");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("GraduationPlan");
                    updateHelper.AddElement("GraduationPlan", "Field");
                    updateHelper.AddElement("GraduationPlan/Field", "Name", editor.Name);
                    updateHelper.AddElement("GraduationPlan/Field", "Content", "<GraduationPlan></GraduationPlan>", true);
                    foreach (var subject in editor.Subjects)
                    {
                        DSXmlHelper helper = new DSXmlHelper("Subject");
                        helper.SetAttribute(".", "GradeYear", K12.Data.Int.GetString(subject.GradeYear));
                        helper.SetAttribute(".", "Semester", K12.Data.Int.GetString(subject.Semester));
                        helper.SetAttribute(".", "Credit", K12.Data.Decimal.GetString(subject.Credit));
                        helper.SetAttribute(".", "Period", K12.Data.Decimal.GetString(subject.Period));
                        helper.SetAttribute(".", "Domain", subject.Domain);
                        helper.SetAttribute(".", "FullName", subject.FullName);
                        helper.SetAttribute(".", "Level", K12.Data.Int.GetString(subject.Level));
                        helper.SetAttribute(".", "CalcFlag", "" + subject.CalcFlag);
                        helper.SetAttribute(".", "SubjectName", subject.SubjectName);
                        helper.AddElement("Grouping");
                        helper.SetAttribute("Grouping", "RowIndex", "" + subject.RowIndex);
                        helper.SetAttribute("Grouping", "startLevel", K12.Data.Int.GetString(subject.StartLevel));

                        //高中專門使用屬性，由騉翔2011/1/16新增
                        helper.SetAttribute(".", "Level", K12.Data.Int.GetString(subject.Level));
                        helper.SetAttribute(".", "NotIncludedInCalc", "" + subject.NotIncludedInCalc);
                        helper.SetAttribute(".", "NotIncludedInCredit", "" + subject.NotIncludedInCredit);
                        helper.SetAttribute(".", "RequiredBy", subject.RequiredBy);
                        helper.SetAttribute(".", "Required", subject.Required ? "必修" : "選修");
                        helper.SetAttribute(".", "Category", subject.Category);
                        helper.SetAttribute(".", "Entry", subject.Entry);

                        updateHelper.AddElement("GraduationPlan/Field/Content/GraduationPlan", helper.BaseElement);
                    }
                    updateHelper.AddElement("GraduationPlan", "Condition");
                    updateHelper.AddElement("GraduationPlan/Condition", "ID", editor.ID);

                    IDs.Add(editor.ID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<ProgramPlanRecord>> packages = worker.Run(ProgramPlanRecords);

            foreach (PackageWorkEventArgs<ProgramPlanRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            if (AfterChange !=null)
                AfterChange(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 刪除單筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecord">課程規劃記錄物件</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(ProgramPlanRecord ProgramPlanRecord)
        {
            return Delete(ProgramPlanRecord.ID);
        }

        /// <summary>
        /// 刪除單筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanID">課程規劃編號</param> 
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(string ProgramPlanID)
        {
            List<string> Keys = new List<string>();

            Keys.Add(ProgramPlanID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanRecords">多筆課程規劃記錄物件</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <seealso cref="ProgramPlanRecord"/>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        static public int Delete(IEnumerable<ProgramPlanRecord> ProgramPlanRecords)
        {
            List<string> Keys = new List<string>();

            foreach (ProgramPlanRecord ProgramPlanRecord in ProgramPlanRecords)
                Keys.Add(ProgramPlanRecord.ID);

            return Delete(Keys);
        }

        /// <summary>
        /// 刪除多筆課程規劃記錄
        /// </summary>
        /// <param name="ProgramPlanIDs">多筆課程規劃編號</param>
        /// <returns>int，傳回成功刪除的筆數。</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        ///     
        /// </example>
        [FISCA.Authentication.AutoRetryOnWebException()]
        static public int Delete(IEnumerable<string> ProgramPlanIDs)
        {
            int result = 0;

            MultiThreadWorker<string> worker = new MultiThreadWorker<string>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<string> e)
            {
                DSXmlHelper helper = new DSXmlHelper("Request");

                helper.AddElement(".", "GraduationPlan");


                foreach (string Key in e.List)
                    helper.AddElement("GraduationPlan", "ID", Key);

                result = int.Parse(DSAServices.CallService(DELETE_SERVICENAME, new DSRequest(helper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);

            };

            List<PackageWorkEventArgs<string>> packages = worker.Run(ProgramPlanIDs);

            foreach (PackageWorkEventArgs<string> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterDelete != null)
                AfterDelete(null, new DataChangedEventArgs(ProgramPlanIDs, ChangedSource.Local));

            if (AfterChange!=null)
                AfterChange(null, new DataChangedEventArgs(ProgramPlanIDs, ChangedSource.Local));

            return result;
        }

        public static void OnAfterChange(DataChangedEventArgs EventArgs)
        {
            if (AfterChange != null)
                AfterChange(null, EventArgs);
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
        /// 改變之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterChange;

        //public static void Test()
        //{
        //    ProgramPlan.Instance.SyncAllBackground();

        //    if (Student.Instance.SelectedList.Count <= 0) return;

        //    Student.Instance.SelectedList.FillProgramPlanRecord();
        //    ProgramPlanRecord record = Student.Instance.SelectedList[0].GetProgramPlanRecord();

        //    ProgramPlanRecordEditor editor = record.GetEditor();
        //    editor.Name = editor.Name + "假的";
        //    editor.Save();
        //}
    }

    //public static class ProgramPlan_ExtendMethods
    //{
    //    #region Student Extend Methods
    //    public static ProgramPlanRecord GetProgramPlanRecord(this StudentRecord studentRec)
    //    {
    //        string id = "";
    //        if (studentRec != null) 
    //        if (string.IsNullOrEmpty(studentRec.OverrideProgramPlanID))
    //        {
    //            if (studentRec.Class != null)
    //                return studentRec.Class.GetProgramPlanRecord();
    //        }
    //        else
    //            id = studentRec.OverrideProgramPlanID;

    //        return ProgramPlan.Instance.Items[id];
    //    }

    //    public static void FillProgramPlanRecord(this IEnumerable<StudentRecord> studentRecs)
    //    {
    //        List<string> primaryKeys = new List<string>();
    //        foreach (var item in studentRecs)
    //        {
    //            if (string.IsNullOrEmpty(item.OverrideProgramPlanID))
    //            {
    //                if (item.Class != null && !string.IsNullOrEmpty(item.Class.RefProgramPlanID))
    //                    primaryKeys.Add(item.Class.RefProgramPlanID);
    //            }
    //            else
    //                primaryKeys.Add(item.OverrideProgramPlanID);
    //        }
    //        ProgramPlan.Instance.SyncDataBackground(primaryKeys);
    //    }
    //    #endregion

    //    #region Class Extend Methods
    //    public static ProgramPlanRecord GetProgramPlanRecord(this ClassRecord classRec)
    //    {
    //        if (classRec != null && !string.IsNullOrEmpty(classRec.RefProgramPlanID))
    //            return ProgramPlan.Instance.Items[classRec.RefProgramPlanID];
    //        else
    //            return null;
    //    }

    //    public static void FillProgramPlanRecord(this IEnumerable<ClassRecord> classRecs)
    //    {
    //        List<string> primaryKeys = new List<string>();
    //        foreach (var item in classRecs)
    //        {
    //            primaryKeys.Add(item.RefProgramPlanID);
    //        }
    //        ProgramPlan.Instance.SyncDataBackground(primaryKeys);
    //    }
    //    #endregion
    //}
}