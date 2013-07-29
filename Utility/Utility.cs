using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Synchronization;

namespace K12.Data.Utility
{
    /// <summary>
    /// Data Access Layer所用到的工具函式
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 根據學年度及學期取得DBHelper的條件
        /// </summary>
        /// <param name="SchoolYearSemester"></param>
        /// <returns></returns>
        internal static string ToLessThanRequest(this SchoolYearSemester SchoolYearSemester)
        {
            StringBuilder req = new StringBuilder();

            //若是學期為2，直接下條件，取得學年度及第二學期之前的資料
            if (SchoolYearSemester.Semester == 2)
            {
                req.Append("<Or>");
                req.Append("<And>");
                req.Append("<SchoolYearLessEqual>"+SchoolYearSemester.SchoolYear+"</SchoolYearLessEqual>");
                req.Append("<SemesterLessEqual>"+SchoolYearSemester.Semester +"</SemesterLessEqual>");
                req.Append("</And>");
                req.Append("</Or>");
            }//若是學期為1，需下兩個條件；第一個條件為上個學年及上個學年第二學期之前的資料，第二個條件為取得目前學年度及第一學期的資料。
            else if (SchoolYearSemester.Semester == 1)
            {
                req.Append("<Or>");

                req.Append("<And>");
                req.Append("<SchoolYearLessEqual>" + (SchoolYearSemester.SchoolYear-1) + "</SchoolYearLessEqual>");
                req.Append("<SemesterLessEqual>" + 2 + "</SemesterLessEqual>");
                req.Append("</And>");

                req.Append("<And>");
                req.Append("<SchoolYear>" + SchoolYearSemester.SchoolYear + "</SchoolYear>");
                req.Append("<Semester>" + 1 + "</Semester>");
                req.Append("</And>");

                req.Append("</Or>");
            }

            return req.ToString();
        }

        /// <summary>
        /// 判斷集合是否為null或是空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> Values)
        {
            return Values == null || Values.ToList().Count == 0;
        }

        /// <summary>
        /// 將集合轉為上層的型別集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="DerivedList"></param>
        /// <returns></returns>
        public static List<T> GetBaseList<T, U>(IEnumerable<U> DerivedList) where U : T
        {
            List<T> BaseList = new List<T>();

            foreach (T u in DerivedList)
                BaseList.Add(u);

            return BaseList;
        }

        public static string GetRandomStr()
        {
            return (new System.Random()).NextDouble().ToString();
        }

        public static List<ChangeEntry> GetChangeEntries(string TableName,ChangeAction Action,IEnumerable<string> Keys)
        {
            List<ChangeEntry> ChangeEntries = new List<ChangeEntry>();

            foreach (string Key in Keys)
            {
                ChangeEntry Entry = new ChangeEntry();

                Entry.Action = Action;
                Entry.TableName = TableName;
                Entry.DataID = Key;
                Entry.Count = 1;

                ChangeEntries.Add(Entry);
            }

            return ChangeEntries;
        }

        public static List<string> AddSeperator(this IEnumerable<string> Source,string Seperator)
        {
            List<string> Result= new List<string>();

            Source.ToList().ForEach(x=>Result.Add(Seperator + x +Seperator));

            return Result;
        }
    }
}