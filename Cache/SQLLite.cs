using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace K12.Data
{
    public class SQLLite
    {
        private static SQLiteConnection mConnection = null;

        /// <summary>
        /// 取得SQLLiteConnection物件
        /// </summary>
        public static SQLiteConnection Connection
        {
            get
            {
                if (mConnection == null)
                {
                    try
                    {
                        string strDatabasePath = AppDomain.CurrentDomain.BaseDirectory + "K12.Data.s3db";

                        if (System.IO.File.Exists(strDatabasePath))
                            System.IO.File.Delete(strDatabasePath);

                        SQLiteConnectionStringBuilder ConnStrBuilder = new SQLiteConnectionStringBuilder();

                        ConnStrBuilder.DataSource = strDatabasePath;                        

                        //ConnStrBuilder.Password = "12900646";

                        mConnection = new SQLiteConnection(ConnStrBuilder.ConnectionString);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                return mConnection;
            }
        }

        /// <summary>
        /// 執行SQL語法會傳回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                Connection.Open();

                SQLiteCommand mycommand = new SQLiteCommand(Connection);

                mycommand.CommandText = sql;

                SQLiteDataReader reader = mycommand.ExecuteReader();

                dt.Load(reader);

                reader.Close();

                Connection.Close();

            }
            catch
            {

                // Catching exceptions is for communists

            }

            return dt;
        }

        public static int ExecuteNonQuery(string SQL)
        {
            return ExecuteNonQuery(new List<string>() { SQL });
        }

        /// <summary>
        /// 執行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(List<string> SQL)
        {
            int RowsUpdated = -1;

            Connection.Open();

            SQLiteCommand command = new SQLiteCommand(Connection);

            SQLiteTransaction Transaction = Connection.BeginTransaction();

            command.Transaction = Transaction;

            try
            {
                for (int i = 0; i < SQL.Count; i++)
                {
                    command.CommandText = SQL[i];
                    RowsUpdated = command.ExecuteNonQuery();
                }

                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Connection.Close(); 
            }

            return RowsUpdated;
        }

        public static string ExecuteScalar(string sql)
        {
            Connection.Open();

            SQLiteCommand command = new SQLiteCommand(Connection);

            SQLiteTransaction Transaction = Connection.BeginTransaction();

            command.Transaction = Transaction;

            try
            {
                command.CommandText = sql;

                object value = command.ExecuteScalar();

                Transaction.Commit();

                if (value != null)
                    return value.ToString();
            }
            catch (Exception e)
            {
                Transaction.Rollback();
            }
            finally
            {
                Connection.Close();
            }

            return "";
        }
    }
}