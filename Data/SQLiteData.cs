using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Data;
using System.Diagnostics;

////using Finisar.SQLite; - так было раньше.

//В Reference добавить:
//System.Data.SQLite [ADO.NET 2.0 Data Provider for SQLite]
#if DEMO || SQLITE
using System.Data.SQLite;
#endif

namespace SRWords
{
    public static class SQLiteData
    {
#if DEMO || SQLITE
        private static SQLiteConnection sqliteConnect;
        private static SQLiteTransaction sqliteTransact;
        private static string errMess = "Невозможно установить соединение с базой данных словаря.";

        private static void SetSQLiteConnection()
        {
            // Если соединение уже установлено
            if (sqliteConnect != null && sqliteConnect.State == ConnectionState.Open)
                return;

            string workDir = ScanWord.Utils.GetWorkDirectory();
#if SQLITE
            string path = "Data Source=" + workDir + "SrbNewDict\\SrbBase.db; ";
#endif
#if DEMO
            string path = "Data Source=" + workDir + "SrbDemo\\SrbBase.db; ";
#endif

            sqliteConnect = new SQLiteConnection(path + "Version=3; New=False; Compress=True;");

            try
            {
                sqliteConnect.Open();
                Debug.WriteLine(sqliteConnect);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(errMess + Environment.NewLine + ex.Message, "Ошибка!");
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(errMess + Environment.NewLine + ex.Message, "Ошибка!");
                Process.GetCurrentProcess().Kill();
            }
        }

        public static void StartTransaction()
        {
            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            sqliteTransact = sqliteConnect.BeginTransaction();
        }

        public static void CommitTransaction()
        {
            if (sqliteTransact != null)
                sqliteTransact.Commit();
        }

        public static void RollbackTransaction()
        {
            if (sqliteTransact != null)
                sqliteTransact.Rollback();
        }

        public static DataTable GetTableBySelect(String select)
        {
            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter(select, sqliteConnect);
                da.Fill(ds);
                dt = ds.Tables[0];
                Debug.WriteLine("GetTableBySelect: " + select);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dt;
        }

        public static T? RunCommandScalar<T>(String command) where T : struct
        {
            object result;

            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            try
            {
                SQLiteCommand com = new SQLiteCommand(command, sqliteConnect);
                result = com.ExecuteScalar();
                Debug.WriteLine("RunCommandScalar: " + command);

                if (result != null)
                {
                    if (result != DBNull.Value)
                    {
                        Debug.WriteLine("RunCommandScalar, result: " + result.ToString());
                        return (T)result;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        public static Int64 RunCommandScalarInt(String command)
        {
            object result;

            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            try
            {
                SQLiteCommand com = new SQLiteCommand(command, sqliteConnect);
                result = com.ExecuteScalar();
                Debug.WriteLine("RunCommandScalarInt: " + command);

                if (result != null)
                {
                    if (result != DBNull.Value)
                    {
                        Debug.WriteLine("RunCommandScalarInt, result: " + result.ToString());
                        return (Int64)result;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw new Exception();
            }

            return -1;
        }

        public static String RunCommandString(String command)
        {
            Object result;

            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            try
            {
                SQLiteCommand com = new SQLiteCommand(command, sqliteConnect);
                result = com.ExecuteScalar();
                Debug.WriteLine("RunCommandString: " + command);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw new Exception();
            }

            if (result == null)
            {
                Debug.WriteLine("RunCommandString, result: null");
                return String.Empty;
            }
            else
            {
                Debug.WriteLine("RunCommandString, result: " + result.ToString());
                return result.ToString();
            }
        }

        public static bool RunCommand(String command)
        {
            bool result = false;

            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            try
            {
                SQLiteCommand com = new SQLiteCommand(command, sqliteConnect);
                com.ExecuteNonQuery();
                result = true;
                Debug.WriteLine("RunCommand: " + command);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }

        /// <summary>
        /// Заполняет список по запросу, например:
        /// "select distinct lower(substr(name,1,1)) res from words".
        /// </summary>
        /// <returns>Возвращает число элементов списка</returns>
        public static int RunScriptResList(String script, out List<String> sList)
        {
            return RunScriptResList(script, out sList, false);
        }
        // without_case = true : из "а" и "А" в списке оставлять только "а".
        // Данный параметр введён из-за того, что ф-я lower не работает с UniCode.
        public static int RunScriptResList(String script, out List<String> sList, bool without_case)
        {
            List<String> result = new List<string>();

            sList = result;
            return 0;

            if (sqliteConnect == null || sqliteConnect.State != ConnectionState.Open)
            {
                SetSQLiteConnection();
            }

            SQLiteCommand com = new SQLiteCommand(script, sqliteConnect);
            using (SQLiteDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string value = reader.GetValue(0).ToString();
                    if (without_case)
                    {
                        value = value.ToLower();
                        if (result.IndexOf(value) == -1)
                            result.Add(value);
                    }
                    else
                    {
                        result.Add(value);
                    }
                }
            }

            //if (without_case)
            result.Sort();

            sList = result;
            return result.Count;
        }
#endif
    }
}
