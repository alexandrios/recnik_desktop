//#define DEMO

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Advantage.Data.Provider;


namespace SRWords
{
    public static class ADSData
    {
        private static AdsConnection adsConnect;
        private static AdsTransaction adsTransact;
        private static string errMess = "Невозможно установить соединение с базой данных словаря.";

        private static void SetADSConnection()
        {
            // Если соединение уже установлено
            if (adsConnect != null && adsConnect.State == ConnectionState.Open)
                return;

            adsConnect = MakeConnect();
            try
            {
                adsConnect.Open();
            }
            catch (AdsException ex)
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

        private static AdsConnection MakeConnect()
        {
            AdsConnection con;
            string path = String.Empty;
            string workDir = ScanWord.Utils.GetWorkDirectory();
            
            workDir += "SrbDict";

            //if (Directory.Exists(workDir + "Srb500"))
            //    path = "data source=" + workDir + "Srb500\\SrbDict.add;";
            //else if (Directory.Exists(workDir + "Srb5000"))
            //    path = "data source=" + workDir + "Srb5000\\SrbDict.add;";
            //else 
            if (Directory.Exists(workDir))
                path = "data source=" + workDir + "\\SrbDict.add;";

            string p1 = "password";
            string p2 = "Tltv" + (200/(32/(2*2))).ToString() + "Xfcjd";
            string p3 = "Gjtpljv";
            string pass = p1 + "=" + p2 + "-" + p3 + ";";

            con = new AdsConnection(path +
                                        "tabletype=ADT; servertype=local;" +
                                        "user id=adssys; " + pass +
                                        "CharType=russian_vfp_ci_as_1251");
            return con;
        }

        public static void StartTransaction()
        {
            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            { 
                SetADSConnection(); 
            }

            adsTransact = adsConnect.BeginTransaction();
        }

        public static void CommitTransaction()
        {
            if (adsTransact != null)
                adsTransact.Commit();
        }

        public static void RollbackTransaction()
        {
            if (adsTransact != null)
                adsTransact.Rollback();
        }

        public static DataTable GetTableBySelect(String select)
        {
            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                // С помощью AdsHelper
                //ds = AdsHelper.ExecuteDataset(adsConnect, CommandType.Text, select);
                //dt = ds.Tables[0];

                // Обычный способ
                AdsDataAdapter da = new AdsDataAdapter(select, adsConnect);
                da.Fill(ds);
                dt = ds.Tables[0];
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

            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            try
            {
                AdsCommand com = new AdsCommand(command, adsConnect);
                result = com.ExecuteScalar();

                if (result != null)
                {
                    if (result != DBNull.Value)
                    {
                        return (T?)result;
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

            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            try
            {
                AdsCommand com = new AdsCommand(command, adsConnect);
                result = com.ExecuteScalar();

                if (result != null)
                {
                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt64(result);
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

            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            try
            {
                AdsCommand com = new AdsCommand(command, adsConnect);
                result = com.ExecuteScalar();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw new Exception();
            }

            if (result == null)
                return String.Empty;
            else
                return result.ToString();
        }

        public static bool RunCommand(String command)
        {
            bool result = false;

            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            try
            {
                AdsCommand com = new AdsCommand(command, adsConnect);
                com.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }

        /// <summary>
        /// Заполняет список по запросу, например:
        /// "select distinct lcase(left(name,1)) res from words".
        /// </summary>
        /// <returns>Возвращает число элементов списка</returns>
        public static int RunScriptResList(String script, out List<String> sList)
        {
            List<String> result = new List<string>();

            if (adsConnect == null || adsConnect.State != ConnectionState.Open)
            {
                SetADSConnection();
            }

            AdsCommand com = new AdsCommand(script, adsConnect);
            using (AdsDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(reader.GetValue(0).ToString());
                }
            }

            sList = result;
            return result.Count;
        }

        #if (!DEMO)
        /// <summary>
        /// Заполнение таблицы valpost мусором.
        /// </summary>
        private static void MdFiveLoadData()
        {
            string s, t1, t2, t3, seed;
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 1; i <= 51; i++)
            {
                t1 = ""; t2 = ""; t3 = "";
                for (int j = 1; j < 5; j++)
                {
                    s = (random.Next(i * j) * DateTime.Now.Millisecond).ToString() + DateTime.Now.ToLongTimeString(); 
                    seed = SerialNum.OrphoGetting(s);
                    t1 += seed;
                    s = (random.Next(i * j) * DateTime.Now.Millisecond).ToString() + DateTime.Now.ToLongTimeString(); 
                    seed = SerialNum.OrphoGetting(s);
                    t2 += seed;
                    if (i == 7 && j == 1) t2 = "5" + t2.Substring(0, t2.Length - 1);
                    s = (random.Next(i * j) * DateTime.Now.Millisecond).ToString() + DateTime.Now.ToLongTimeString(); 
                    seed = SerialNum.OrphoGetting(s);
                    t3 += seed;
                }
                string cmd = "insert into valpost (PREFIX, EXISTANCE, POSTFIX) values('" + t1 + "', '" + t2 + "', '" + t3 + "')";
                RunCommand(cmd);
            }
        }

        /// <summary>
        /// Сохранение логина.
        /// </summary>
        public static void MdFiveSetLogin(int loginLength, string lenHex, string login)
        {
            string cmd = "select count(*) cnt from valpost";
            int? cnt = RunCommandScalar<int>(cmd);
            if (cnt != null && cnt == 0)
            {
                MdFiveLoadData();
                cnt = RunCommandScalar<int>(cmd);
            }

            if (cnt != null && cnt == 51)
            {
                cmd = "update valpost set existance='" + lenHex + "'+substring(existance, 2, 127) where id=7";
                RunCommand(cmd);

                cmd = "update valpost set postfix=substring(postfix, 1, " + loginLength.ToString() +
                    ")+'" + login + "'+ substring(postfix, 1, (128-" + loginLength.ToString() + "-" +
                    login.Length.ToString() + ")) where id=" + (7 + loginLength).ToString();
                RunCommand(cmd);
            }
            else
            {
                throw new Exception("Initialization error (0051).");
            }
        }

        /// <summary>
        /// Получить логин из бд.
        /// </summary>
        /// <returns></returns>
        public static string MdFiveGetLogin()
        {
            string cmd = "select count(*) cnt from valpost";
            int? cnt = RunCommandScalar<int>(cmd);
            if (cnt != null && cnt == 0)
            {
                return String.Empty;
            }

            int loginLength = 0;
            int len = 0;
            List<String> sList = new List<string>();
            cmd = "select substring(existance,1,1) res from valpost where id=7";
            if (RunScriptResList(cmd, out sList) == 1)
            {
                len = int.Parse(sList[0], System.Globalization.NumberStyles.HexNumber);
                loginLength = len - 5;
            }

            if (loginLength >= 5 && loginLength <= 10)
            {
                cmd = "select substring(postfix," + (loginLength + 1).ToString() +
                        "," + (loginLength * 4).ToString() +
                        ") res from valpost where id=" + (7 + loginLength).ToString();
                if (RunScriptResList(cmd, out sList) == 1)
                {
                    return sList[0];
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Получить ключ из бд.
        /// </summary>
        /// <returns></returns>
        public static string MdFiveGetKey()
        {
            string result = String.Empty;
            string cmd = "select count(*) cnt from valpost";
            int? cnt = RunCommandScalar<int>(cmd);
            if (cnt != null && cnt == 0)
            {
                return String.Empty;
            }

            List<String> sList = new List<string>();
            cmd = "select substring(prefix," + MdFiveGetLogin().Length.ToString() + 
                    "+1,8*4) res from valpost where id=12";
            if (RunScriptResList(cmd, out sList) == 1)
            {
                result = sList[0];
            }

            return result;
        }

        /// <summary>
        /// Сохранить ключ в бд.
        /// </summary>
        /// <param name="key"></param>
        public static void MdFiveSetKey(string key)
        {
            string cmd = "select count(*) cnt from valpost";
            int? cnt = RunCommandScalar<int>(cmd);
            if (cnt != null && cnt == 0)
            {
                return;
            }

            if (cnt != null && cnt == 51)
            {
                int loginLength = MdFiveGetLogin().Length;
                cmd = "update valpost set prefix=substring(prefix, 1, " + loginLength.ToString() +
                    ")+'" + key + "'+ substring(prefix, 1, (128-" + loginLength.ToString() + "-" +
                    key.Length.ToString() + ")) where id=" + (7 + 5).ToString();
                RunCommand(cmd);
            }
            else
            {
                throw new Exception("Initialization error (0051).");
            }
        }
        #endif
    }
}
