//#define DEMO

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Net;

namespace SRWords
{
    public static class SerialNum
    {
        public const int MAX_FAILCOUNT = 9;
        const int MAX_DONATE_INTERVAL = 14;

        public static bool Draw2Spinning()
        {
            #if (!DEMO)
            string storedKey = ADSData.MdFiveGetKey();
            string computedKey = MakeKeyByLoginInfo(GetLoginHex(), DensityWeber(), false);
            return String.Equals(storedKey, computedKey);
            #else
            return true;
            #endif
        }

        #if (!DEMO)
        private static string getVolumeID(string drive)
        {
            if (drive == string.Empty)
            {
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                drive = drive.Substring(0, drive.Length - 2);
            }

            return getVolumeSerial(drive);
        }

        private static string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private static string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }

        private static string getPhysicalMemory()
        {
            string memory = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (memory == "")
                {
                    if (queryObj["BankLabel"] != null)
                        memory += queryObj["BankLabel"].ToString();
                    if (queryObj["Capacity"] != null)
                        memory += queryObj["Capacity"].ToString();
                    if (queryObj["Speed"] != null)
                        memory += queryObj["Speed"].ToString();
                    break;
                }
            }

            return memory;
        }

        private static string getMd5Hash(string input)
        {
            // создаем объект этого класса. Отмечу, что он создается не через new, а вызовом метода Create
            MD5 md5Hasher = MD5.Create();
            // Преобразуем входную строку в массив байт и вычисляем хэш
            byte[] bytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            // Создаем новый Stringbuilder (Изменяемую строку) для набора байт
            StringBuilder sBuilder = new StringBuilder();
            // Преобразуем каждый байт хэша в шестнадцатеричную строку
            for (int i = 0; i < bytes.Length; i++)
            {    //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа    
                sBuilder.Append(bytes[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        // Считать реальную информацию о компьютере.
        public static string DensityWeber()
        {
            string volumeId = getVolumeID("C");
            string cpuId = getCPUID();
            string memoryId = getPhysicalMemory();
            //string result = volumeId + "|" + cpuId + "|" + memoryId;
            string result = SplitWidthOrientation(volumeId) + cpuId + SplitWidthOrientation(memoryId);
            return result;
        }

        // Это ключ, который я высылаю по почте, и с которым сравнивает программа.
        // Он должен где-то храниться.
        public static string OrphoStruct(string getParam)
        {
            string tmp = getMd5Hash(getParam);
            tmp = tmp.ToUpper();
            string result = "";
            for (int i = 0; i < 8; i++)
            {
                result += tmp.Substring(4 * i, 4) + (i < 7 ? " " : "");
            }
            return result;
        }

        public static string OrphoGetting(string getParam)
        {
            string tmp = getMd5Hash(getParam);
            tmp = tmp.ToLower();
            return tmp;
        }

        // То, что присылает пользователь мне.
        public static string RestansDay()
        {
            string result = "";
            string volumeId = getVolumeID("C");
            string cpuId = getCPUID();
            string memoryId = getPhysicalMemory();
            result = cpuId.Substring(0, 3) + memoryId.Substring(11, memoryId.Length - 11) +
                "O" + SplitWidthOrientation(volumeId) + "Y" + 
                SplitWidthOrientation(cpuId.Substring(3, cpuId.Length - 3)) + "L" + cpuId.Length + "Z" +
                SplitWidthOrientation(memoryId.Substring(0, 5)) + SplitWidthOrientation(memoryId.Substring(5, 6));
            return result;
        }

        // Я преобразовываю присланное в реальную информацию.
        public static string Stores(string thisWay)
        {
            string volumeId = "";
            string cpuId = "";
            string memoryId = "";

            if (String.IsNullOrEmpty(thisWay))
                return "";

            cpuId = thisWay.Substring(0, 3);
            string tmp = thisWay.Substring(thisWay.Length - 11, 11);
            memoryId = Scambling2(tmp.Substring(0, 5)) + Scambling2(tmp.Substring(5, 6));
            tmp = thisWay.Substring(0, thisWay.Length - 11);
            if (tmp.Substring(tmp.Length - 1, 1) != "Z")
            {
                MessageBox.Show("Z-Ошибка!");
                return "";
            }
            int k = tmp.LastIndexOf("L");
            if (k == -1)
            {
                MessageBox.Show("L-Ошибка!");
                return "";
            }
            string tmp2 = tmp.Substring(k + 1, tmp.Length - (k + 2));
            tmp = tmp.Substring(0, k);
            int cpuLength = int.Parse(tmp2);
            cpuId += Scambling2(tmp.Substring(tmp.Length - (cpuLength - 3), cpuLength - 3));

            k = tmp.LastIndexOf("Y");
            if (k == -1)
            {
                MessageBox.Show("Y-Ошибка!");
                return "";
            }
            int j = tmp.LastIndexOf("O");
            if (j == -1)
            {
                MessageBox.Show("O-Ошибка!");
                return "";
            }
            volumeId = Scambling2(tmp.Substring(j + 1, k - j - 1));

            tmp = tmp.Substring(0, j);
            memoryId += tmp.Substring(3);

            //string result = volumeId + "|" + cpuId + "|" + memoryId;
            string result = SplitWidthOrientation(volumeId) + cpuId + SplitWidthOrientation(memoryId);
            return result;
        }

        public static string SplitWidthOrientation(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                ch = DecimalOff(ch);
                result = result + ch.ToString();
            }

            return result;
        }

        private static char DecimalOff(char ch)
        {
            switch (Char.ToUpper(ch))
            { 
                case '0' :
                    return '6';
                case '1' :
                    return 'D';
                case '2' :
                    return '3';
                case '3' :
                    return 'F';
                case '4' :
                    return '1';
                case '5' :
                    return '9';
                case '6' :
                    return 'B';
                case '7' :
                    return 'E';
                case '8' :
                    return '8';
                case '9' :
                    return 'A';
                case 'A' :
                    return 'C';
                case 'B' :
                    return '2';
                case 'C' :
                    return '4';
                case 'D' :
                    return '5';
                case 'E' :
                    return '0';
                case 'F' :
                    return '7';
                case '_' :
                    return '*';
                case '*' :
                    return '_';
                default :
                    return ch;
            }
        }

        public static string Scambling2(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                ch = FreisingToFit(ch);
                result = result + ch.ToString();
            }

            return result;
        }

        private static char FreisingToFit(char ch)
        {
            switch (Char.ToUpper(ch))
            {
                case '6':
                    return '0';
                case 'D':
                    return '1';
                case '3':
                    return '2';
                case 'F':
                    return '3';
                case '1':
                    return '4';
                case '9':
                    return '5';
                case 'B':
                    return '6';
                case 'E':
                    return '7';
                case '8':
                    return '8';
                case 'A':
                    return '9';
                case 'C':
                    return 'A';
                case '2':
                    return 'B';
                case '4':
                    return 'C';
                case '5':
                    return 'D';
                case '0':
                    return 'E';
                case '7':
                    return 'F';
                case '_':
                    return '*';
                case '*':
                    return '_';
                default:
                    return ch;
            }
        }

        public static void LoginToHex(string login, out string result, out int loginLength)
        {
            result = "";
            loginLength = login.Length;
            for (int i = 0; i < loginLength; i++)
            {
                Char ch = login[i];
                int code = Convert.ToInt16(ch);
                string hex = code.ToString("x4");
                result += hex;
            }

            result = SplitWidthOrientation(result).ToLower();
        }

        public static string GetLogin()
        {
            string result = "";
            string login = ADSData.MdFiveGetLogin();
            login = Scambling2(login);

            int len = login.Length;
            for (int i = 0; i < len / 4; i++)
            {
                string hex = login.Substring(i * 4, 4);
                int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                Char ch = Convert.ToChar(code);
                result += ch.ToString();
            }

            return result;
        }

        public static string GetLoginHex()
        {
            return ADSData.MdFiveGetLogin();
        }

        public static void SaveLogin(string login)
        {
            string result;
            int loginLength;
            LoginToHex(login, out result, out loginLength);
            ADSData.MdFiveSetLogin(loginLength, (loginLength + 5).ToString("x"), result);
        }

        public static void SaveDonate(string donate)
        {
            string result;
            int donateLength;
            DonateToHex(donate, out result, out donateLength);
            ADSData.MdFiveSetDonate(donateLength, (donateLength + 3).ToString("x"), result);
        }

        public static string GetDonate()
        {
            string result = "";
            string donate = ADSData.MdFiveGetDonate();
            donate = Scambling2(donate);

            int len = donate.Length;
            for (int i = 0; i < len / 4; i++)
            {
                string hex = donate.Substring(i * 4, 4);
                int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                Char ch = Convert.ToChar(code);
                result += ch.ToString();
            }

            return result;
        }


        public static void SaveFails(string fc)
        {
            string result;
            int fcLength;
            DonateToHex(fc, out result, out fcLength);
            ADSData.MdFiveSetFails(fcLength, (fcLength + 3).ToString("x"), result);
        }

        public static string GetFails()
        {
            string result = "";
            string fc = ADSData.MdFiveGetFails();
            fc = Scambling2(fc);

            int len = fc.Length;
            for (int i = 0; i < len / 4; i++)
            {
                string hex = fc.Substring(i * 4, 4);
                int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                Char ch = Convert.ToChar(code);
                result += ch.ToString();
            }

            return result;
        }

        public static int GetFailsInt()
        {
            // Прочитать failCount 
            string s = GetFails();
            int failCount;
            if (!int.TryParse(s, out failCount))
                failCount = SerialNum.MAX_FAILCOUNT;

            return failCount;
        }

        public static string GetEmail()
        {
            return ADSData.MdFiveGetEmail();
        }

        public static void SaveEmail(string email)
        {
            ADSData.MdFiveSetEmail(email);
        }

        /// <summary>
        /// На основании информации о компьютере и сохраненном логине генерируем ключ.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string MakeKeyByLoginInfo(string login, string info, bool isUpper)
        {
            string konto = "";
            int k = info.Length > login.Length ? info.Length : login.Length;
            for (int i = 0; i < k; i++)
            {
                if (i < info.Length)
                    konto += info[i];
                if (i < login.Length)
                    konto += login[i];
            }
            
            if (isUpper)
                return OrphoStruct(konto);
            else
                return OrphoGetting(konto);
        }

        /// <summary>
        /// Функция проверяет сравнивает сгенерированный ключ с ключом из бд. 
        /// Проверка происходит при запуске приложения.
        /// </summary>
        /// <returns></returns>
        public static bool CompareKey()
        {
            string storedKey = ADSData.MdFiveGetKey();
            string computedKey = MakeKeyByLoginInfo(GetLoginHex(), DensityWeber(), false);
            return String.Equals(storedKey, computedKey);
        }

        public static void GenerateAndSaveKey()
        {
            // Рассчитать ключ
            string computedKey = MakeKeyByLoginInfo(GetLoginHex(), DensityWeber(), false);

            // Записать ключ в бд
            ADSData.MdFiveSetKey(computedKey);
        }

        private static void ServerWarning(string response)
        {
            // Проверить интернет
            if (!InternetAvailable())
            {
                MessageBox.Show("Проверьте соединение с интернетом!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string t = response + Environment.NewLine + "unexpected response from the server";
                MessageBox.Show(t, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool InternetAvailable()
        {
            string strServer = "http://google.com";
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);
                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть! 
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, инета нет
                    rspFP.Close();
                    return false;
                }
            }
            catch (WebException)
            {
                // Ошибка, интернета у нас нет.
                return false;
            }
        }

        public static bool SendRegistration(string login, string email)
        {
            // Отослать логин, email, персональный код на сервер
            string parameters = String.Format("login={0}&email={1}&perscode={2}", login, email, RestansDay());
            ScanWord.Rest rest = new ScanWord.Rest();
            string response = "";
            try
            {
                response = rest.InsertRegistrInfo(parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (response != "OK")
            {
                ServerWarning(response);
                return false;
            }

            return true;
        }

        public static bool ReadRegistration(string login, string email)
        {
            // Убедиться, что информация записана на сервере в БД
            string parameters = String.Format("login={0}&email={1}&perscode={2}", login, email, RestansDay());
            ScanWord.Rest rest = new ScanWord.Rest();
            List<ScanWord.UserDonation> donation = null;
            try
            {
                donation = rest.GetUserDonation(parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            if (donation == null || donation.Count == 0)
            {
                // запись не найдена на сервере
                ServerWarning("0");
                return false;
            }

            return true;
        }

        public static DonateResult GetDonationInfo()
        {
            ScanWord.Rest rest = new ScanWord.Rest();
            string parameters = String.Format("login={0}&email={1}&perscode={2}", GetLogin(), GetEmail(), RestansDay());
            List<ScanWord.UserDonation> donation = null;
            try
            {
                donation = rest.GetUserDonation(parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Невозможно получить информацию о регистрации.", 
                    "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (donation == null || donation.Count == 0)
            {
                // запись не найдена на сервере
                ServerWarning("0");

                // Прочитать failCount 
                string s = GetFails();
                int failCount;
                if (!int.TryParse(s, out failCount))
                    failCount = MAX_FAILCOUNT;
                // Увеличить счетчик failCount
                failCount++;
                // Записать в бд failCount
                SaveFails(failCount.ToString());

                if (failCount <= MAX_FAILCOUNT)
                {
                    return DonateResult.NORMAL;
                }
                else
                {
                    MessageBox.Show("Превышено число запусков без подключения к интернету!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DonateResult.FAIL;
                }
            }
            else
            {
                // TODO: сделать, чтобы с сервера донат приходил в зашифрованном виде!
                int donate = 0;
                int.TryParse(donation[0].donate, out donate);
                if (donate > 0)
                {
                    // Записать донат в бд (в другом зашифрованном виде)
                    SaveDonate(donate.ToString());

                    // Сгенерировать ключ и сохранить его в бд
                    GenerateAndSaveKey();

                    // Обнулить в бд failCount
                    SaveFails("0");

                    // при первом выполнении этого условия сказать СПАСИБО
                    MessageBox.Show("Спасибо за Ваш донат!" + Environment.NewLine + "Приятной работы со словарем!", 
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return DonateResult.OK;
                }
                else
                {
                    DateTime date = DateTime.ParseExact(donation[0].date, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    string note = donation[0].note;
                    int interval = Convert.ToInt32(Math.Floor((DateTime.Now - date).TotalDays));

                    if (interval <= MAX_DONATE_INTERVAL)
                    {
                        //MessageBox.Show("Не забудьте сделать донат!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return DonateResult.NORMAL;
                    }
                    else
                    {
                        // Записать в бд failCount
                        SaveFails((MAX_FAILCOUNT + 1).ToString());
                        MessageBox.Show("Истекло время работы без завершения регистрации!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return DonateResult.FAIL;
                    }
                }
            }
        }

        public static void DonateToHex(string donate, out string result, out int donateLength)
        {
            result = "";
            donateLength = donate.Length;
            for (int i = 0; i < donateLength; i++)
            {
                Char ch = donate[i];
                int code = Convert.ToInt16(ch);
                string hex = code.ToString("x4");
                result += hex;
            }

            result = SplitWidthOrientation(result).ToLower();
        }

        public enum DonateResult
        { 
            OK,
            NORMAL,
            FAIL
        }

#endif
    }
}
