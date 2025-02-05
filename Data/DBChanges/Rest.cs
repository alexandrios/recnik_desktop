using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace SRWords
{
    public class Rest
    {
        private string GetRequest(string url, string parameters, string au = null)
        {
            const string AUTHORIZATION = Const.AUTHORIZATION;
            string currentUser;

            if (!string.IsNullOrEmpty(au))
                currentUser = au;
            else
                currentUser = AUTHORIZATION;

            string data = String.Format("{0}?user={1}&{2}", url, currentUser, parameters);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(data);
            request.Method = "GET";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                throw new Exception("Ошибка соединения с сервером!");
            }

            string content = "";
            if (response != null)
            {
                content = response.StatusCode.ToString();
                if (response.StatusCode.ToString() == "OK")
                {
                    Stream rs = response.GetResponseStream();
                    StreamReader read = new StreamReader(rs, Encoding.UTF8);
                    content = read.ReadToEnd();
                    read.Close();
                    Console.WriteLine(content);
                }
                response.Close();
            }

            return content;
        }

        private string PostRequest(string url, string parameters, string au = null)
        {
            const string AUTHORIZATION = Const.AUTHORIZATION; 
            string currentUser;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.ContentType = "multipart/form-data";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!String.IsNullOrEmpty(au))
                currentUser = au;
            else
                currentUser = AUTHORIZATION;

            string data = String.Format("user={0}&{1}", currentUser, parameters);
            byte[] b = Encoding.UTF8.GetBytes(data);
            request.ContentLength = b.Length;
            HttpWebResponse response;
            try
            {
                request.GetRequestStream().Write(b, 0, b.Length);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                throw new Exception("Ошибка соединения с сервером!");
            }

            string content = "";
            if (response != null)
            {
                content = response.StatusCode.ToString();
                if (response.StatusCode.ToString() == "OK")
                {
                    Stream rs = response.GetResponseStream();
                    StreamReader read = new StreamReader(rs, Encoding.UTF8);
                    content = read.ReadToEnd();
                    read.Close();
                    Console.WriteLine(content);
                }
                response.Close();
            }

            return content;
        }

        /// <summary>
        /// Скрипт возвращает массив корректировок словаря, начиная с N+1 (из таблицы nchanges). Вызывается из Recnik_desktop
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<ChangeInfo> GetNchangesStress(string parameters, string au)
        {
            string jsonString = GetRequest("https://trans.h1n.ru/recnik_desktop_api/RdGetNchangesStress.php", parameters, au);
            List<ChangeInfo> info = JsonConvert.DeserializeObject<List<ChangeInfo>>(jsonString);
            return info;
        }

        /// <summary>
        /// Скрипт записывает информацию о регистрации пользователя. Вызывается из Recnik_desktop
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string InsertRegistrInfo(string parameters, string au)
        {
            return PostRequest("https://trans.h1n.ru/recnik_desktop_api/RdInsertRegistrInfo.php", parameters, au);
        }

        /// <summary>
        /// Скрипт возвращает список донатов пользователя. Вызывается из Recnik_desktop
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<UserDonation> GetUserDonation(string parameters, string au)
        {
            string jsonString = GetRequest("https://trans.h1n.ru/recnik_desktop_api/RdGetUserDonation.php", parameters, au);
            List<UserDonation> info = JsonConvert.DeserializeObject<List<UserDonation>>(jsonString);
            return info;
        }

    }
}
