using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRWords
{
    public static class ChangesFromServer
    {
        /// <summary>
        /// Получить изменения БД с сервера и выполнить их.
        /// </summary>
        public static void GetChangesFromServer()
        {
            try
            {
                List<ChangeInfo> info = GetChangesList();
                if (info == null)
                    return;

                Debug.WriteLine("Получены изменения с сервера: " + info.Count.ToString());

                if (info.Count > 0)
                {
                    Debug.WriteLine("Начинаем применять изменения, полученные с сервера");
                    DBService.MakeChangesFromServer(info, new Repository2());
                }
            }
            catch (Exception ex)
            {
                // нет соединения с сервером
                Debug.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Получить изменения БД с сервера.
        /// </summary>
        static private List<ChangeInfo> GetChangesList()
        {
            int change_id = Data.GetChangeId();
            Debug.WriteLine("Текущий код синхронизации = " + change_id.ToString());
            string parameters = String.Format("change_id={0}", change_id.ToString());
            Rest rest = new Rest();
            Debug.WriteLine("Обращение к серверу за списком изменений...");
            List<ChangeInfo> info = rest.GetNchangesStress(parameters, Const.AUTHORIZATION);
            return info;
        }
    }
}
