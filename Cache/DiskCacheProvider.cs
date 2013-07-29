using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace K12.Data
{
    /// <summary>
    /// 提供本機資料庫的快取功能
    /// </summary>
    public class DiskCacheProvider : ICacheProvider
    {
        private List<string> EntityNames = new List<string>();

        #region Private 成員

        private string ToCommaList(IEnumerable<string> Values)
        {
            StringBuilder Result = new StringBuilder();

            foreach (string Value in Values)
            {
                if (Result.Length > 0)
                    Result.Append(",");
                Result.Append(Value);
            }

            return Result.ToString();
        }

        #endregion

        #region ICacheProvider 成員

        public Dictionary<string,string> SelectAll(string EntityName)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            StringBuilder QueryBuilder = new StringBuilder();
            QueryBuilder.AppendFormat("select * from {0}",EntityName);

            DataTable Table = SQLLite.GetDataTable(QueryBuilder.ToString());

            for (int i = 0; i < Table.Rows.Count; i++)
                Result.Add(Table.Rows[i][0].ToString(), Table.Rows[i][1].ToString());

            return Result; 
        }

        public CacheSet<string> SelectByIDs(string EntityName, System.Collections.Generic.IEnumerable<string> EntityIDs)
        {
            CacheSet<string> Result = new CacheSet<string>();

            if (!EntityNames.Contains(EntityName))
            {
                Result.WantIDs = EntityIDs.ToList<string>();

                return Result;
            }

            List<string> IDs = new List<string>();
            List<string> Contents = new List<string>();

            StringBuilder QueryBuilder = new StringBuilder();
            QueryBuilder.AppendFormat("select * from {0} where ID in ({1}) order by ID", EntityName, ToCommaList(EntityIDs));

            DataTable Table = SQLLite.GetDataTable(QueryBuilder.ToString());

            for (int i=0; i < Table.Rows.Count; i++)
            {
                IDs.Add(Table.Rows[i][0].ToString());
                Contents.Add(Table.Rows[i][1].ToString());
            }

            Result.Records = Contents;

            Result.WantIDs = EntityIDs.Except<string>(IDs).ToList<string>();

            return Result;
        }

        public void Set(string EntityName, object Record)
        {
            Set(EntityName, new List<object>() { Record });
        }

        public void Set(string EntityName, System.Collections.IEnumerable Records)
        {
            #region 檢查資料表是否存在，若不存在則建立，並且加入到已建立的Entity列表
            if (!EntityNames.Contains(EntityName))
            {
                StringBuilder TableBuilder = new StringBuilder();

                TableBuilder.AppendFormat("CREATE TABLE IF NOT EXISTS [{0}] ([ID] INTEGER  NOT NULL PRIMARY KEY ,[CONTENT] TEXT NOT NULL);", EntityName);

                //TableBuilder.AppendFormat("CREATE UNIQUE INDEX [{0}Index] ON [{0}]([ID]  DESC)",EntityName);

                SQLLite.ExecuteScalar(TableBuilder.ToString());

                EntityNames.Add(EntityName);
            }
            #endregion

            List<string> IDs = new List<string>();

            List<string> QueryList = new List<string>();

            foreach (var Record in Records)
            {
                StringBuilder QueryBuilder = new StringBuilder();

                string Content = Record.ToString();
                string ID = ParseID(Content);
                IDs.Add(ID);

                //QueryBuilder.AppendLine("PRAGMA synchronous = OFF;"); //加此行就不行用Transaction，會加快但是不能用Transaction
                QueryBuilder.AppendFormat("insert or replace into {0} values('{1}','{2}');", EntityName, ID, Content);
                QueryList.Add(QueryBuilder.ToString());
            }

            SQLLite.ExecuteNonQuery(QueryList);
        }

        public void Insert(string EntityName, object Record)
        {
            Insert(EntityName, new List<object>() { Record });
        }
        
        public void Insert(string EntityName, System.Collections.IEnumerable Records)
        {
            #region 檢查資料表是否存在，若不存在則建立，並且加入到已建立的Entity列表
            if (!EntityNames.Contains(EntityName))
            {
                StringBuilder TableBuilder = new StringBuilder();

                TableBuilder.AppendFormat("CREATE TABLE IF NOT EXISTS [{0}] ([ID] INTEGER  NOT NULL PRIMARY KEY ,[CONTENT] TEXT NOT NULL);", EntityName);

                //TableBuilder.AppendFormat("CREATE UNIQUE INDEX [{0}Index] ON [{0}]([ID]  DESC)",EntityName);

                SQLLite.ExecuteScalar(TableBuilder.ToString());

                EntityNames.Add(EntityName);
            }
            #endregion


            List<string> IDs = new List<string>();

            List<string> QueryList = new List<string>();

            foreach (var Record in Records)
            {
                StringBuilder QueryBuilder = new StringBuilder();

                string Content = Record.ToString();
                string ID = ParseID(Content);
                IDs.Add(ID);

                //QueryBuilder.AppendLine("PRAGMA synchronous = OFF;"); //加此行就不行用Transaction，會加快但是不能用Transaction
                QueryBuilder.AppendFormat("insert or ignore into {0} values('{1}','{2}');",EntityName, ID, Content);
                QueryList.Add(QueryBuilder.ToString());
            }

            SQLLite.ExecuteNonQuery(QueryList);
        }

        public void Update(string EntityName, object Record)
        {
            Update(EntityName, new List<object>() { Record });
        }

        public void Update(string EntityName, System.Collections.IEnumerable Records)
        {
            List<string> IDs = new List<string>();

            List<string> QueryList = new List<string>();

            foreach (var Record in Records)
            {
                StringBuilder QueryBuilder = new StringBuilder();

                string Content = Record.ToString();
                string ID = ParseID(Content);
                IDs.Add(ID);

                //QueryBuilder.AppendLine("PRAGMA synchronous = OFF;"); //加此行就不行用Transaction，會加快但是不能用Transaction
                QueryBuilder.AppendFormat("update {0} set Content='{2}' where ID ='{1}';", EntityName, ID, Content);
                QueryList.Add(QueryBuilder.ToString());
            }

            SQLLite.ExecuteNonQuery(QueryList);
        }

        public void Delete(string EntityName, string EntityID)
        {
            Delete(EntityName, new List<string>() { EntityID });
        }

        public void Delete(string EntityName, System.Collections.Generic.IEnumerable<string> EntityIDs)
        {
            StringBuilder QueryBuilder = new StringBuilder();

            QueryBuilder.AppendFormat("delete from {0} where ID in ({1})",EntityName,ToCommaList(EntityIDs));

            SQLLite.ExecuteScalar(QueryBuilder.ToString());
        }

        public void Delete(string EntityName)
        {
            StringBuilder QueryBuilder = new StringBuilder();

            QueryBuilder.AppendFormat("delete from {0}", EntityName);

            SQLLite.ExecuteScalar(QueryBuilder.ToString());
        }

        public virtual string ParseID(string Content)
        {
            int StartIndex = Content.IndexOf("ID=\"");

            int EndIndex = Content.IndexOf('\"', StartIndex+4);

            string Result = Content.Substring(StartIndex+4, EndIndex - (StartIndex+4));

            if (string.IsNullOrEmpty(Result))
                throw new Exception("無法解析出ID");

            return Result;

            //StringReader StrReader = new StringReader(Content);

            //XmlTextReader TextReader = new XmlTextReader(StrReader);

            //try
            //{
                
            //    TextReader.WhitespaceHandling = WhitespaceHandling.None;

            //    TextReader.Read(); TextReader.Read();

            //    string ID = TextReader.GetAttribute("ID");

            //    if (string.IsNullOrEmpty(ID))
            //        throw new Exception("內容中不存在ID" + Content);

            //    return ID;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}finally
            //{
            //    TextReader.Close();
            //}
        }
        #endregion
    }
}