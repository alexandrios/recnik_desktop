using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace SRWords
{
    public static class Data
    {
        public static String GetDBVersion()
        {
#if DEMO || SQLITE
            return "";
#else
            string sql = "select db_version from changes where id = 0";
            DataTable dataTable = ADSData.GetTableBySelect(sql);
            return dataTable.Rows[0]["DB_VERSION"].ToString();
#endif
        }

        public static String GetKw(string name)
        {
#if DEMO || SQLITE
            string sql = "select kw from words where name ='" + Articles.Utils.CyrToLat(name) + "'";
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            string sql = "select kw from words where name ='" + name + "'";
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable.Rows[0]["KW"].ToString();
        }

        /// <summary>
        /// Загрузить словарь в кириллице и латинице.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable LoadAllDictCL(string tableName)
        {
            //string sql = "select id, name, name_lat, anum, pnum, xml from " + tableName;
#if DEMO
            string sql = "select id, name, name_lat, kw, xml from " + tableName;
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#elif SQLITE
            // для программы: NAME - кириллица; NAME_LAT - латиница (а в БД: NAME - латиница; NAME_LAT - нет поля)
            string sql = "select id, name name_lat, '' name, kw, xml from " + tableName;
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
            foreach (DataRow row in dataTable.Rows)
            {
                row["NAME"] = SRWords.Articles.Utils.LatToCyr(row["NAME_LAT"].ToString());
            }
#else
            string sql = "select name, name_rus, stress, kw, xml from " + tableName;
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable;
        }

        public static DataTable LoadAllDictCyr(string tableName)
        {
            //string sql = "select id, name, anum, pnum, xml from " + tableName;
            string sql = "select id, name, kw, xml from " + tableName;
#if DEMO || SQLITE
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable;
        }

        public static DataTable LoadAllDictLat(string tableName)
        {
            //string sql = "select id, name_lat name, anum, pnum, xml from " + tableName;
            string sql = "select id, name_lat name, kw, xml from " + tableName;
#if DEMO || SQLITE
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable;
        }

        public static DataTable LoadRusDict()
        {
            string sql = "select name, srbname from ruswords";
#if DEMO || SQLITE
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable;
        }

        // Пользовательские словари -------------------------------------[START]

        /// <summary>
        /// Список пользовательских словарей в виде таблицы.
        /// </summary>
        /// <returns></returns>
        public static DataTable LoadTableUserDicts()
        {
            //string sql = "select Id, Name, bitmap, bgr, (select count(*) from dict where iddict=s.id) cnt from sysd s order by name";
#if DEMO || SQLITE
            string sql = "select Id, Name, bitmap, bgr, cast((select count(*) from dict where iddict=s.id) as integer) cnt from sysd s order by name";
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            string sql = "select Id, Name, bitmap, bgr, (select count(*) from dict where iddict=s.id) cnt from sysd s order by name";
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            // Сортировка
            dataTable.DefaultView.Sort = "name";
            return dataTable;
        }

        /// <summary>
        /// Список пользовательских словарей в виде списка наименований.
        /// </summary>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int LoadListUserDicts(out List<String> sList)
        {
            string sql = "select name res from sysd order by name";
#if DEMO || SQLITE
            return SQLiteData.RunScriptResList(sql, out sList);
#else
            return ADSData.RunScriptResList(sql, out sList);
#endif
        }

        /// <summary>
        /// Список пользовательских словарей в виде списка ИД.
        /// </summary>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int LoadListUserDicts(out List<int> sList)
        {
            string sql = "select id res from sysd order by name";
            List<String> list = new List<string>();
#if DEMO || SQLITE
            int result = SQLiteData.RunScriptResList(sql, out list);
#else
            int result = ADSData.RunScriptResList(sql, out list);
#endif
            sList = new List<int>();
            foreach (String s in list)
                sList.Add(int.Parse(s));

            return result;
        }

        /// <summary>
        /// Список пользовательских словарей в виде списков наименований и ИД.
        /// </summary>
        /// <param name="sList"></param>
        /// <param name="sId"></param>
        /// <returns></returns>
        public static int LoadListUserDicts(out List<String> sList, out List<String> sId)
        {
            sList = new List<string>();
            sId = new List<string>();
            int result = 0;
            string sql = "select id, name from sysd order by name";
#if DEMO || SQLITE
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            if (dataTable != null)
            {
                result = dataTable.Rows.Count;
                foreach (DataRow row in dataTable.Rows)
                {
                    sList.Add(row["name"].ToString());
                    sId.Add(row["id"].ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// Загрузить пользовательский словарь в кириллице и латинице.
        /// </summary>
        /// <param name="idDict"></param>
        /// <returns></returns>
        public static DataTable LoadUserDictCL(int idDict)
        {
            string sql = "select w.Name, w.Name_rus, w.Xml, d.Status, d.Rw " +
                "from words w inner join dict d on w.Name = d.Name where d.IdDict=" + idDict.ToString();
#if DEMO || SQLITE
            DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
            DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
            return dataTable;
        }

        /// <summary>
        /// Создать пользовательский словарь.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="idWord"></param>
        public static void InsertUserDict(string nameDict, string bgr)
        {
            string cmd = "insert into sysd (name, bgr) values('" + nameDict + "', '" + bgr + "')";
#if DEMO || SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        /// <summary>
        /// Исправить настройки пользовательского словаря.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="nameDict"></param>
        /// <param name="bgr"></param>
        public static void UpdateUserDict(Int64 idDict, string nameDict, string bgr)
        {
            string cmd = "update sysd set name='" + nameDict + "', bgr='" + bgr + "' where id=" + idDict.ToString();
#if DEMO || SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        /// <summary>
        /// Проверка, уникально ли наименование пользовательского словаря.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="nameDict"></param>
        /// <returns></returns>
        public static bool IsUniqueNameUserDict(Int64 idDict, string nameDict)
        {
            bool result = true;

            string cmd = "select count(*) cnt from sysd where name='" + nameDict + "' and id<>" + idDict.ToString();
#if DEMO || SQLITE
            Int64? cnt = SQLiteData.RunCommandScalar<Int64>(cmd);
#else
            int? cnt = ADSData.RunCommandScalar<int>(cmd);
#endif
            if (cnt != null && cnt != -1)
            {
                if ((int)cnt > 0)
                    result = false;
            }

            return result;
        }

        /// <summary>
        /// Перенести все слова из одного пользовательского словаря в другой.
        /// </summary>
        /// <param name="fromDict"></param>
        /// <param name="toDict"></param>
        public static void TransferWordsToOtherDict(Int64 fromDict, Int64 toDict)
        {
            string cmd;
            ADSData.StartTransaction();

            try
            {
                // Таблица слов, которые есть в обоих словарях
                string sql = "select rowid id /*d.ID*/ from dict d where d.IDDICT=" + fromDict.ToString() +
                    " and (select count(*) from dict where IDDICT=" + toDict.ToString() +
                    " and IDW=d.IDW)<>0";
#if DEMO || SQLITE
                DataTable dataTable = SQLiteData.GetTableBySelect(sql);
#else
                DataTable dataTable = ADSData.GetTableBySelect(sql);
#endif
                foreach (DataRow row in dataTable.Rows)
                {
                    cmd = "delete from dict where ID=" + row["ID"].ToString();
#if DEMO || SQLITE
                    SQLiteData.RunCommand(cmd);
#else
                    ADSData.RunCommand(cmd);
#endif
                }

                // Теперь остались только уникальные слова - изменить у них ИД словаря
                cmd = "update dict set IDDICT=" + toDict.ToString() + " where IDDICT=" + fromDict.ToString();
#if DEMO || SQLITE
                SQLiteData.RunCommand(cmd);
#else
                ADSData.RunCommand(cmd);
                ADSData.CommitTransaction();
#endif
            }
            catch (Exception e)
            {
#if DEMO || SQLITE
                ADSData.RollbackTransaction();
#endif
                throw e;
            }

        }

        /// <summary>
        /// Очистить пользовательский словарь.
        /// </summary>
        /// <param name="idDict"></param>
        public static void ClearUserDict(Int64 idDict)
        {
            string cmd = "delete from dict where IDDICT=" + idDict.ToString();
#if DEMO || SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        /// <summary>
        /// Удалить пользовательский словарь.
        /// </summary>
        /// <param name="idDict"></param>
        public static void DeleteUserDict(Int64 idDict)
        {
            string cmd = "delete from sysd where id=" + idDict.ToString();
#if DEMO || SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }

        /// <summary>
        /// Вернуть фон пользовательского словаря по его ID.
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        public static string GetBGRForDict(Int64 dictId)
        {
            string sql = "select bgr from sysd where id=" + dictId.ToString();
#if DEMO || SQLITE
            return SQLiteData.RunCommandString(sql);
#else
            return ADSData.RunCommandString(sql);
#endif
        }

        /// <summary>
        /// Вернуть наименование пользовательского словаря по его ID.
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        public static string GetNameForDict(Int64 dictId)
        {
            string sql = "select name from sysd where id=" + dictId.ToString();
#if DEMO || SQLITE
            return SQLiteData.RunCommandString(sql);
#else
            return ADSData.RunCommandString(sql);
#endif
        }

        /// <summary>
        /// Вернуть количество слов в пользовательском словаре.
        /// </summary>
        /// <param name="idDict"></param>
        /// <returns></returns>
        public static int GetWordsCountUserDict(int idDict)
        {
            string cmd = "select count(*) cnt from dict where IDDICT=" + idDict.ToString();
#if DEMO || SQLITE
            Int64? cnt = SQLiteData.RunCommandScalar<Int64>(cmd);
#else
            int? cnt = ADSData.RunCommandScalar<int>(cmd);
#endif
            if (cnt != null)
            {
                return (int)cnt;
            }
            return 0;
        }

        /// <summary>
        /// Проверить, есть ли слово в пользовательском словаре.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="idWord"></param>
        /// <returns></returns>
        public static bool IsWordInUserDict(int idDict, int idWord)
        {
            string cmd = "select count(*) cnt from dict where IDDICT=" + idDict.ToString() +
                            " and IDW=" + idWord.ToString();
#if DEMO || SQLITE
            Int64? cnt = SQLiteData.RunCommandScalar<Int64>(cmd);
#else
            int? cnt = ADSData.RunCommandScalar<int>(cmd);
#endif

            return (cnt != null && cnt > 0);
        }

        /// <summary>
        /// Поместить слово в пользовательский словарь.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="idWord"></param>
        public static void AddInUserDict(int idDict, int idWord)
        {
#if DEMO || SQLITE
            string cmd = "insert into dict (IDDICT, IDW, DATEADD, STATUS, RW) " +
                "Values(" + idDict.ToString() + "," + idWord.ToString() + ", datetime('now'), 0, '')";
            SQLiteData.RunCommand(cmd);
#else
            string cmd = "insert into dict (IDDICT, IDW, DATEADD, STATUS, RW) " +
                "Values(" + idDict.ToString() + "," + idWord.ToString() + ", CURRENT_TIMESTAMP(), 0, '')";
            ADSData.RunCommand(cmd);
#endif
        }

        /// <summary>
        /// Удалить слово из пользовательского словаря.
        /// </summary>
        /// <param name="idDict"></param>
        /// <param name="idWord"></param>
        public static void DelFromUserDict(int idDict, int idWord)
        {
            string cmd = "delete from dict where IDDICT=" + idDict.ToString() +
                            " and IDW=" + idWord.ToString();
#if DEMO || SQLITE
            SQLiteData.RunCommand(cmd);
#else
            ADSData.RunCommand(cmd);
#endif
        }
        // Пользовательские словари -------------------------------------[END]


        /// <summary>
        /// Поиск по буквам с использованием заранее подготовленной таблицы letters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sLett"></param>
        /// <param name="sSign"></param>
        public static void SyllableListCyr(string text, out List<String> sLett, out List<String> sSign)
        {
            if (String.IsNullOrEmpty(text))
                text = "@";

            // Доработка из-за того, что конечный пробел теряется, напр.: "грстите " превращается в "грстите"
            text = text.Replace(' ', '@');

            string sql = "select value res from letters where letter='" + text + "'";
            string value = ADSData.RunCommandString(sql);

            sLett = new List<string>();
            sSign = new List<string>();
            string[] v = value.Split(new char[] { '|' });
            for (int i = 0; i < v.Length; i++)
            {
                if (!String.IsNullOrEmpty(v[i]))
                {
                    string[] parts = v[i].Split(new char[] { ',' });
                    sLett.Add(parts[0].Replace('@', ' '));
                    sSign.Add(parts[1]);
                }
            }
        }

        /// <summary>
        /// Поиск по буквам без заранее подготовленной таблицы letters. Функция-1.
        /// </summary>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int SyllableFirstLettersCyr(out List<String> sList)
        {
#if DEMO || SQLITE
            string sql = "select distinct substr(name,1,1) res from words";
            return SQLiteData.RunScriptResList(sql, out sList, true);
#else
            string sql = "select distinct lcase(left(name,1)) res from words";
            return ADSData.RunScriptResList(sql, out sList);
#endif
        }

        /// <summary>
        /// Поиск по буквам без заранее подготовленной таблицы letters. Функция-2.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int SyllableNextLettersCyr(string text, out List<String> sList)
        {
#if DEMO || SQLITE
            // Такой извращённый запрос из-за того, что ф-я lower не работает с UniCode
            String sql = "select distinct substr(name,1," + (text.Length + 1).ToString() +
                        ") res from words where name like '" + text + "%'" +
                        " union select distinct substr(name,1," + (text.Length + 1).ToString() +
                        ") res from words where name like '" + text.Substring(0, 1).ToUpper() + text.Substring(1) + "%'";
            // Тритий параметр (true) из-за того, что ф-я lower не работает с UniCode
            return SQLiteData.RunScriptResList(sql, out sList, true);
#else
            //String sql = "select distinct lcase(left(name," + (text.Length + 1).ToString() +
            //            ")) res from words where lcase(name) like '" + text + "%'";

            // Доработки из-за того, что конечный пробел теряется, напр.: "грстите " превращается в "грстите"
            text = text.Replace(" ", "_");
            String sql = "select distinct lcase(left(replace(name,' ','@')," + (text.Length + 1).ToString() +
                        ")) res from words where lcase(name) like '" + text + "%'";
            int result = ADSData.RunScriptResList(sql, out sList);
            for (int i = 0; i < sList.Count; i++)
            {
                sList[i] = sList[i].Replace("@", " ");
            }
            return result;
#endif
        }

        /// <summary>
        /// Поиск по буквам без заранее подготовленной таблицы letters. Функция-1. Латиница.
        /// </summary>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int SyllableFirstLettersLat(out List<String> sList)
        {
#if DEMO || SQLITE
            string sql = "select distinct lower(substr(name_lat,1,1)) res from words";
            return SQLiteData.RunScriptResList(sql, out sList);
#else
            string sql = "select distinct lcase(left(name_lat,1)) res from words";
            return ADSData.RunScriptResList(sql, out sList);
#endif
        }

        /// <summary>
        /// Поиск по буквам без заранее подготовленной таблицы letters. Функция-2. Латиница.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sList"></param>
        /// <returns></returns>
        public static int SyllableNextLettersLat(string text, out List<String> sList)
        {
#if DEMO || SQLITE
            //text = ScanWord.Utils.LatToCyr(text);
            String sql = "select distinct lower(substr(name_lat,1," + (text.Length + 1).ToString() +
                        ")) res from words where lower(name_lat) like '" + text + "%'";
            return SQLiteData.RunScriptResList(sql, out sList);
            /*  
            for (int i = 0; i < sList.Count; i++)
            {
                sList[i] = Utils.CyrToLat(sList[i]);
            }
            sList.Sort();
            */
#else
            // Доработки из-за того, что конечный пробел теряется, напр.: "грстите " превращается в "грстите"
            text = text.Replace(" ", "_");
            String sql = "select distinct lcase(left(replace(name_lat,' ','@')," + (text.Length + 1).ToString() +
                        ")) res from words where lcase(name_lat) like '" + text + "%'";
            int result = ADSData.RunScriptResList(sql, out sList);
            for (int i = 0; i < sList.Count; i++)
            {
                sList[i] = sList[i].Replace("@", " ");
            }
            return result;
#endif
        }

        /// <summary>
        /// Количество слов.
        /// </summary>
        /// <returns></returns>
        public static int VolumeDB()
        {
            string cmd = "select count(*) cnt from words";
#if DEMO || SQLITE
            Int64? cnt = SQLiteData.RunCommandScalar<Int64>(cmd);
#else
            int? cnt = ADSData.RunCommandScalar<int>(cmd);
#endif
            if (cnt != null && cnt != -1)
            {
                return (int)cnt;
            }
            return 0;
        }

        public static int GetChangeId()
        {
            int result = 10000;
            string cmd = "select change_id from changes where id=0";
#if SQLITE
            Int64? cnt = SQLiteData.RunCommandScalar<Int64>(cmd);
#else
            int? cnt = ADSData.RunCommandScalar<int>(cmd);
#endif
            result = (int)cnt;
            return result;
        }

    }
}