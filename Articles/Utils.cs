using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace SRWords.Articles
{
    public static class Utils
    {
        public static Dictionary<String, String> Acronyms = ReadAcronyms();

        public static string GetWorkDirectory()
        {
            //return Environment.CurrentDirectory + "\\";
            return Application.StartupPath + "\\";
        }

        public static string ReadFromXml(string xmlFile, string tag, string attr)
        {
            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(xmlFile);
            }
            catch
            {
                return "-1";
            }

            XmlNodeList nodes = document.DocumentElement.GetElementsByTagName(tag);
            if (nodes.Count != 0)
            {
                try
                {
                    XmlAttributeCollection attrs = nodes[0].Attributes;
                    return attrs[attr].Value;
                }
                catch
                {
                    return "-1";
                }
            }
            else return "-1";
        }

        public static void WriteToXml(string xmlFile, string tag, string attr, string value)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlFile);

            if (tag == "")
            {
                if (document.DocumentElement.HasAttribute(attr))
                    document.DocumentElement.SetAttribute(attr, value);
                else
                {
                    XmlAttribute attribute = document.CreateAttribute(attr); // создаём атрибут
                    attribute.Value = value; // устанавливаем значение атрибута
                    document.DocumentElement.Attributes.Append(attribute); // добавляем атрибут
                }
            }
            else
            {
                XmlNodeList nodes = document.DocumentElement.GetElementsByTagName(tag);
                if (nodes.Count != 0)
                {
                    XmlNode node = nodes[0];
                    XmlAttribute attribute = document.CreateAttribute(attr); // создаём атрибут
                    attribute.Value = value; // устанавливаем значение атрибута
                    node.Attributes.Append(attribute);
                }
                else
                {
                    XmlNode element = document.CreateElement(tag);
                    document.DocumentElement.AppendChild(element); // указываем родителя
                    XmlAttribute attribute = document.CreateAttribute(attr); // создаём атрибут
                    attribute.Value = value; // устанавливаем значение атрибута
                    element.Attributes.Append(attribute); // добавляем атрибут
                }
            }

            /*
            XmlNode element = document.CreateElement(tag);
            document.DocumentElement.AppendChild(element); // указываем родителя
            XmlAttribute attribute = document.CreateAttribute(attr); // создаём атрибут
            attribute.Value = value; // устанавливаем значение атрибута
            element.Attributes.Append(attribute); // добавляем атрибут
            */

            /*Добавляем в запись данные:
            XmlNode subElement1 = document.CreateElement("subElement1"); // даём имя
            subElement1.InnerText = "Hello"; // и значение
            element.AppendChild(subElement1); // и указываем кому принадлежит

            Ещё добавляем:

            XmlNode subElement2 = document.CreateElement("subElement2");
            subElement2.InnerText = "Dear";
            element.AppendChild(subElement2);

            XmlNode subElement3 = document.CreateElement("subElement3");
            subElement3.InnerText = "Habr";
            element.AppendChild(subElement3);
            */

            document.Save(xmlFile);
        }

        /// <summary>
        /// Возвращает первое и последнее слово страницы.
        /// </summary>
        /// <returns></returns>
        public static string GetDiapazonWords(string xmlFile)
        {
            // Версия: из XML-файла
            string result = ReadFromXml(xmlFile, "Information", "Diapazon");

            if (result == "-1")
                return "";
            else
                return " (" + result + ")";
        }

        /// <summary>
        /// Чтение числа, находящегося внутри строки символов. Читается первое встреченное число.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetNumberFromStr(string text)
        {
            int result = 0;
            string buf = "";
            bool foundNumber = false;

            foreach (Char c in text)
            {
                if (Char.IsDigit(c)) // Если встретилась цифра
                {
                    // Установить флаг
                    foundNumber = true;

                    // Считываем число
                    buf += c.ToString();
                }
                else // Встретился нечисловой символ
                {
                    // Если число уже считывалось, то выйти из цикла
                    if (foundNumber)
                        break;
                }
            }

            if (buf != "")
                result = int.Parse(buf);

            return result;
        }

        /// <summary>
        /// Возвращает true, если в строке только арабские или римские (IVX) цифры.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsOnlyDigit(string text)
        {
            bool result = true;

            foreach (Char c in text)
            {
                if (!Char.IsDigit(c))
                {
                    if (c != 'I' && c != 'V' && c != 'X')
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает true, если строка является римской цифрой от I до X.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsRomanDigit(string t)
        {
            if (t == "I" || t == "II" || t == "III" || t == "IV" || t == "V" || t == "VI" || t == "VII" ||
                t == "VIII" || t == "IX" || t == "X")
                return true;

            return false;
        }

        /// <summary>
        /// Возвращает словарь акронимов.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, String> ReadAcronyms()
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();

            // Открываем файл для текстового чтения
            using (StreamReader stream = new StreamReader(Utils.GetWorkDirectory() + "acronyms.txt", System.Text.Encoding.Default))
            {
                string streamInput = null;

                // Читаем из файла информацию
                while ((streamInput = stream.ReadLine()) != null)
                {
                    string key = streamInput.Substring(0, streamInput.IndexOf("="));
                    string value = streamInput.Substring(streamInput.IndexOf("=") + 1);
                    dict.Add(key, value);
                }
            }

            return dict;
        }

        /// <summary>
        /// Возвращает список токенов акронимов.
        /// </summary>
        /// <returns></returns>
        public static List<String> GetListAcronyms()
        {
            if (Acronyms.Count == 0)
                Acronyms = ReadAcronyms();

            List<String> list = new List<string>();

            foreach (String key in Acronyms.Keys)
            {
                string[] tokens = key.Split(new Char[] { ' ' });
                foreach (String token in tokens)
                {
                    if (token.EndsWith("."))
                        list.Add(token.Substring(0, token.Length - 1));
                    else
                        list.Add(token);
                }
            }

            return list;
        }

        public const string CSS_FILE_NAME = "style.css";

        public static string HTMLStartString()
        {
            return "<HTML><HEAD>" +
                   "<LINK rel=\"stylesheet\" type=\"text/css\" href=\"" + GetWorkDirectory() + CSS_FILE_NAME + "\">" + Environment.NewLine +
                   "<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" + Environment.NewLine +

                   //"<title>Шрифт</title><style>@font-face {font-family: PTC55F; src: url(PTC55F.ttf); }" +
                   //" .SrbWord  { color: #000000; font: normal 18px PTC55F;} </style>" +

                   "</HEAD><BODY>" + Environment.NewLine;
        }

        public static string HTMLEndString()
        {
            return Environment.NewLine + "</BODY></HTML>";
        }

        /// <summary>
        /// Преобразование строки из кириллицы в латиницу.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CyrToLat(string text)
        {
            string result = "";

            for (int i = 0; i < text.Length; i++)
            {
                result += CharsCyrToLat(text[i]);
            }

            return result;
        }

        /// <summary>
        /// Преобразование кириллических символов в латинские.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string CharsCyrToLat(char c)
        {
            string r;
            switch (c)
            {
                case 'а': r = "a"; break;
                case 'б': r = "b"; break;
                case 'в': r = "v"; break;
                case 'г': r = "g"; break;
                case 'д': r = "d"; break;
                case 'ђ': r = "đ"; break;
                case 'е': r = "e"; break;
                case 'ж': r = "ž"; break;
                case 'з': r = "z"; break;
                case 'и': r = "i"; break;
                case '\u0458': r = "j"; break;
                case 'к': r = "k"; break;
                case 'л': r = "l"; break;
                case 'љ': r = "lj"; break;
                case 'м': r = "m"; break;
                case 'н': r = "n"; break;
                case 'њ': r = "nj"; break;
                case 'о': r = "o"; break;
                case 'п': r = "p"; break;
                case 'р': r = "r"; break;
                case 'с': r = "s"; break;
                case 'т': r = "t"; break;
                case 'ћ': r = "ć"; break;
                case 'у': r = "u"; break;
                case 'ф': r = "f"; break;
                case 'х': r = "h"; break;
                case 'ц': r = "c"; break;
                case 'ч': r = "č"; break;
                case 'џ': r = "dž"; break;
                case 'ш': r = "š"; break;

                case 'А': r = "A"; break;
                case 'Б': r = "B"; break;
                case 'В': r = "V"; break;
                case 'Г': r = "G"; break;
                case 'Д': r = "D"; break;
                case 'Ђ': r = "Đ"; break;
                case 'Е': r = "E"; break;
                case 'Ж': r = "Ž"; break;
                case 'З': r = "Z"; break;
                case 'И': r = "I"; break;
                case '\u0408': r = "J"; break;
                case 'К': r = "K"; break;
                case 'Л': r = "L"; break;
                case 'Љ': r = "Lj"; break;
                case 'М': r = "M"; break;
                case 'Н': r = "N"; break;
                case 'Њ': r = "Nj"; break;
                case 'О': r = "O"; break;
                case 'П': r = "P"; break;
                case 'Р': r = "R"; break;
                case 'С': r = "S"; break;
                case 'Т': r = "T"; break;
                case 'Ћ': r = "Ć"; break;
                case 'У': r = "U"; break;
                case 'Ф': r = "F"; break;
                case 'Х': r = "H"; break;
                case 'Ц': r = "C"; break;
                case 'Ч': r = "Č"; break;
                case 'Џ': r = "Dž"; break;
                case 'Ш': r = "Š"; break;

                default: r = c.ToString(); break;
            }

            return r;
        }

        /// <summary>
        /// Преобразование строки из латиницы в кириллицу.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string LatToCyr(string text)
        {
            string result = "";
            string s;
            int i = 0;
            while (i < text.Length)
            {
                s = text[i].ToString();
                if (i < text.Length - 1)
                {
                    string t = text.Substring(i, 2);
                    if (t == "lj" || t == "Lj" || t == "nj" || t == "Nj" || t == "dž" || t == "Dž")
                    {
                        s = t;
                        i++;
                    }
                }
                result += CharsLatToCyr(s);
                i++;
            }

            return result;
        }

        /// <summary>
        /// Преобразование латинских символов в кириллические.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string CharsLatToCyr(string s)
        {
            string r;
            switch (s)
            {
                case "a": r = "а"; break;
                case "b": r = "б"; break;
                case "v": r = "в"; break;
                case "g": r = "г"; break;
                case "d": r = "д"; break;
                case "đ": r = "ђ"; break;
                case "e": r = "е"; break;
                case "ž": r = "ж"; break;
                case "z": r = "з"; break;
                case "i": r = "и"; break;
                case "j": r = "\u0458"; break;
                case "k": r = "к"; break;
                case "l": r = "л"; break;
                case "lj": r = "љ"; break;
                case "m": r = "м"; break;
                case "n": r = "н"; break;
                case "nj": r = "њ"; break;
                case "o": r = "о"; break;
                case "p": r = "п"; break;
                case "r": r = "р"; break;
                case "s": r = "с"; break;
                case "t": r = "т"; break;
                case "ć": r = "ћ"; break;
                case "u": r = "у"; break;
                case "f": r = "ф"; break;
                case "h": r = "х"; break;
                case "c": r = "ц"; break;
                case "č": r = "ч"; break;
                case "dž": r = "џ"; break;
                case "š": r = "ш"; break;

                case "A": r = "А"; break;
                case "B": r = "Б"; break;
                case "V": r = "В"; break;
                case "G": r = "Г"; break;
                case "D": r = "Д"; break;
                case "Đ": r = "Ђ"; break;
                case "E": r = "Е"; break;
                case "Ž": r = "Ж"; break;
                case "Z": r = "З"; break;
                case "I": r = "И"; break;
                case "J": r = "\u0408"; break;
                case "K": r = "К"; break;
                case "L": r = "Л"; break;
                case "Lj": r = "Љ"; break;
                case "M": r = "М"; break;
                case "N": r = "Н"; break;
                case "Nj": r = "Њ"; break;
                case "O": r = "О"; break;
                case "P": r = "П"; break;
                case "R": r = "Р"; break;
                case "S": r = "С"; break;
                case "T": r = "Т"; break;
                case "Ć": r = "Ћ"; break;
                case "U": r = "У"; break;
                case "F": r = "Ф"; break;
                case "H": r = "Х"; break;
                case "C": r = "Ц"; break;
                case "Č": r = "Ч"; break;
                case "Dž": r = "Џ"; break;
                case "Š": r = "Ш"; break;

                default: r = s; break;
            }

            return r;
        }

        public static string BySrbAlphabet(string text, string settings)
        {
            return settings == "lat" ? CyrToLat(text) : LatToCyr(text);
        }

    }
}
