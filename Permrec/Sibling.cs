using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using FISCA.DSAUtil;
using K12.Data.Utility;

namespace K12.Data
{
    /// <summary>
    /// 手足名單類別，提供方法用來取得及修改手足名單資訊
    /// </summary>
    public class Sibling
    {
        private const string UPDATE_SERVICENAME = "SmartSchool.Student.QuickUpdate";

        [SelectMethod("K12.Parent.SelectAll", "學籍.學生家長及監護人")]
        public static List<SiblingRecord> SelectAll()
        {
            return SelectAll<K12.Data.SiblingRecord>();
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectAll<T>() where T : SiblingRecord, new()
        {
            List<T> Types = new List<T>();

            QueryHelper helper = new QueryHelper();

            DataTable Table = helper.Select("select id,sibling_info from student");

            foreach (DataRow Row in Table.Rows)
            {
                T Type = new T();
                Type.Load(Row);
                Types.Add(Type);
            }

            return Types;
        }

        public static SiblingRecord SelectByStudent(StudentRecord Student)
        {
            return SelectByStudent<K12.Data.SiblingRecord>(Student);
        }

        protected static T SelectByStudent<T>(StudentRecord Student) where T : SiblingRecord, new()
        {
            return SelectByStudentID<T>(Student.ID);
        }

        public static SiblingRecord SelectByStudentID(string StudentID)
        {
            return SelectByStudentID<K12.Data.SiblingRecord>(StudentID);
        }

        protected static T SelectByStudentID<T>(string StudentID) where T : SiblingRecord, new()
        {
            List<string> Keys = new List<string>();

            Keys.Add(StudentID);

            return SelectByStudentIDs<T>(Keys)[0];
        }

        public static List<SiblingRecord> SelectByStudents(IEnumerable<StudentRecord> Students)
        {
            return SelectByStudents<K12.Data.SiblingRecord>(Students);
        }

        protected static List<T> SelectByStudents<T>(IEnumerable<StudentRecord> Students) where T : SiblingRecord, new()
        {
            List<string> Keys = new List<string>();

            foreach (StudentRecord student in Students)
                Keys.Add(student.ID);

            return SelectByStudentIDs<T>(Keys);
        }

        public static List<SiblingRecord> SelectByStudentIDs(List<string> StudentIDs)
        {
            return SelectByStudentIDs<K12.Data.SiblingRecord>(StudentIDs);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        protected static List<T> SelectByStudentIDs<T>(IEnumerable<string> StudentIDs) where T : SiblingRecord, new()
        {
            List<T> Types = new List<T>();

            QueryHelper helper = new QueryHelper();

            DataTable Table = helper.Select("select id,sibling_info from student where id in (" + string.Join(",", StudentIDs.ToArray()) + ")");

            foreach (DataRow Row in Table.Rows)
            {
                T Type = new T();
                Type.Load(Row);
                Types.Add(Type);
            }

            return Types;
        }

        public static int Update(SiblingRecord ParentRecord)
        {
            List<SiblingRecord> Params = new List<SiblingRecord>();

            Params.Add(ParentRecord);

            return Update(Params);
        }

        [FISCA.Authentication.AutoRetryOnWebException()]
        public static int Update(IEnumerable<SiblingRecord> Records)
        {
            int result = 0;

            List<string> IDs = new List<string>();

            MultiThreadWorker<SiblingRecord> worker = new MultiThreadWorker<SiblingRecord>();
            worker.MaxThreads = 3;
            worker.PackageSize = 100;
            worker.PackageWorker += delegate(object sender, PackageWorkEventArgs<SiblingRecord> e)
            {
                DSXmlHelper updateHelper = new DSXmlHelper("UpdateStudentList");

                foreach (var editor in e.List)
                {
                    updateHelper.AddElement("Student");
                    updateHelper.AddElement("Student", "Field");
                    updateHelper.AddElement("Student/Field", "SiblingInfo");
                    updateHelper.AddElement("Student/Field/SiblingInfo", "SiblingList");

                    foreach (SiblingItem item in editor.SiblingItems)
                    {                
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList", "SiblingInfo");
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","Name",item.Name);
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","Relationship",item.Relationship);
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","School",item.SchoolName);
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","SchoolLocation",item.SchoolLocation);
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","ClassName",item.ClassName);
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","Birthdate",K12.Data.DateTimeHelper.ToDisplayString(item.BirthDate));
                        updateHelper.AddElement("Student/Field/SiblingInfo/SiblingList/SiblingInfo","Memo",item.Memo);
                    }

                    updateHelper.AddElement("Student", "Condition");
                    updateHelper.AddElement("Student/Condition", "ID", editor.RefStudentID);

                    IDs.Add(editor.RefStudentID);
                }

                result = int.Parse(DSAServices.CallService(UPDATE_SERVICENAME, new DSRequest(updateHelper.BaseElement)).GetContent().GetElement("ExecuteCount").InnerText);
            };

            List<PackageWorkEventArgs<SiblingRecord>> packages = worker.Run(Records);

            foreach (PackageWorkEventArgs<SiblingRecord> each in packages)
                if (each.HasException)
                    throw each.Exception;

            if (AfterUpdate != null)
                AfterUpdate(null, new DataChangedEventArgs(IDs, ChangedSource.Local));

            return result;
        }

        /// <summary>
        /// 更新之後所觸發的事件
        /// </summary>
        static public event EventHandler<DataChangedEventArgs> AfterUpdate;
    }
}