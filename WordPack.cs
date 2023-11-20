using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using SRWords.Articles;

namespace SRWords
{
    public enum Language
    { 
        SERBIAN,
        RUSSIAN
    }
    public class WordPack
    {
        public Language Lang;
        public String Name;
        public String Html;
        public String ReverseWord;
        public String TtsWord;
        int HistoryId;
        Boolean IsAtFavourites;

        private readonly string Setup_SrbAlphabet;
        private readonly string Setup_RusAccent;
        // Признак, показывать ли статьи сербских переводов русского слова или показывать только переводы (в русско-сербском словаре)
        private readonly bool multiRusValuesOpened = true; //false;

        private DataView srbDataView;

        public WordPack() { }
        public WordPack(DataRowView dr, Language lang, DataView srbDataView, String setup_SrbAlphabet, String setup_RusAccent)
        {
            if (dr == null) return;
            this.Lang = lang;
            this.Name = dr["NAME"].ToString();

            // srbDataView - это всегда dataViewSrbLat. Sort='NAME'. Искать всегда на латинице
            this.srbDataView = srbDataView;
            
            this.Setup_SrbAlphabet = setup_SrbAlphabet;
            this.Setup_RusAccent = setup_RusAccent;

            if (lang == Language.SERBIAN)
                GetSrbWord(dr);
            else if (lang == Language.RUSSIAN)
                GetRusWord(dr);
        }


        private void GetSrbWord(DataRowView dr)
        {
            String t = "";

            // Вариант 2: десериализовать статью из БД, а затем сформировать HTML
            if (dr != null && dr["XML"] != DBNull.Value)
            {
                // Считать сериализованный XML-объект Article
                //#if DEMO
                // так как поле XML имеет тип BLOB
                //                byte[] bytesXml = (byte[])dr["XML"];
                //                Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
                //                string xmlString = dstEncodingFormat.GetString(bytesXml);
                //#else
                string xmlString = dr["XML"].ToString();
                //#endif

                ArticleInfo a = new ArticleInfo();

                a = a.Deserialize(xmlString);

                // Сформировать HTML-скрипт: true=создавать ссылки
                t = a.CreateScript(true, this.Setup_SrbAlphabet, this.Setup_RusAccent);
                // Не создавать ссылки в пользовательских словарях
                //t = a.CreateScript((currTableName == "words"), Setup_SrbAlphabet, Setup_RusAccent);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(t);
            String finalString = String.Empty;

            Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
            if (bytes != null)
                finalString = dstEncodingFormat.GetString(bytes);

            if (!File.Exists(Css.cssFile))
            {
                Css.MakeCss();
            }

            finalString = ScanWord.Utils.HTMLStartString() + finalString + ScanWord.Utils.HTMLEndString();

            //_webBrowser.DocumentText = finalString;

            this.Html = finalString;
        }


        private void GetRusWord(DataRowView dr)
        {
            //string img = "gobyref.png";
            string img = "search_icon.png";

            this.ReverseWord = dr["SRBNAME"].ToString();
            string srbname = this.ReverseWord;
            string[] aSrb = srbname.Split(new char[] { ';' });
            List<String> aList = new List<string>();
            for (int i = 0; i < aSrb.Length; i++)
            {
                string s = aSrb[i];
                if (aList.IndexOf(s) == -1)
                    aList.Add(s);
            }
            aList.Sort();

            //wbShowAllToolStripMenuItem.Tag = aList;

            string t = "";
            int numId = 1;
            int cnt = aList.Count;
            string display = cnt == 1 ? "block" : multiRusValuesOpened ? "block" : "none";
            string nameId;

            foreach (string s in aList)
            {
                nameId = s.Replace(" ", "_");
                t += "<a href=\"^" + s + "\" class=\"OpenHide\" " +
                     "style=\"color:black; font:normal 16px Arial;\" " +
                     //"onclick=\"facechange('#" + nameId + "'); return true;\">" +
                     "onclick=\"viewdiv('" + nameId + "'); return true;\">" +
                     (Setup_SrbAlphabet == "lat" ? Utils.CyrToLat(s) : s) + "</a>" +
                     
                    // "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                    // "<button type=\"button\" title=\"Найти слово в сербско-русском словаре\" " +
                    // "onclick=\"location.href='~" + nameId + "'\" class=\"Button1\">Найти</button>" +

                     "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                     "<a href='~" + nameId + "'><img src=\"" + Utils.GetWorkDirectory() + img + "\"" +
                     //" alt=\"Найти слово в сербско-русском словаре\" border=\"0\" width=\"12\" height=\"12\"></a>" +
                     " border=\"0\" width=\"12\" height=\"12\"></a>" +


                     "<div id=\"" + nameId + "\"; class=\"SrbBlock\"; style=\"display:" + display + "\">" +
                     GetHtmlByName(false, dr["NAME"].ToString(), s, cnt) +
                     "</div>";

                if (numId < cnt)
                    t += "<HR>";

                numId++;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(t);
            String finalString = String.Empty;

            Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
            if (bytes != null)
                finalString = dstEncodingFormat.GetString(bytes);

            if (!File.Exists(Css.cssFile))
            {
                Css.MakeCss();
            }

            finalString = HTMLStartString() + finalString + ScanWord.Utils.HTMLEndString();

            //MessageBox.Show(finalString);
            //_webBrowser.DocumentText = finalString;

            this.Html = finalString;
        }

        private string GetHtmlByName(bool doOpen, string rusKey, string wName, int cnt)
        {
            // Если сербских значений > 1, то не раскрывать их и не загружать пока из БД
            //if (cnt > 1) return "#"; // # - признак пустой статьи
            if (!doOpen)
                if (!this.multiRusValuesOpened)
                    return "#"; // # - признак пустой статьи

            string xmlString = "";


            DataRowView[] rows = this.srbDataView.FindRows(Utils.CyrToLat(wName));
            if (rows.Length == 1)
            {
                if (rows[0]["XML"] != DBNull.Value)
                {
#if DEMO
                    // так как поле XML имеет тип BLOB
                    byte[] bytesXml = (byte[])dr["XML"];
                    Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
                    xmlString = dstEncodingFormat.GetString(bytesXml);
#else
                    xmlString = rows[0]["XML"].ToString();
#endif
                }
            }

            if (xmlString.Length > 0)
            {
#if SQLITE
                // Получить объект ArticleInfo
                ArticleInfo a = new ArticleInfo();
                a = a.Deserialize(xmlString);
#else
                // Получить объект ArticleInfo
                ArticleInfo a = new ArticleInfo();
                a = a.Deserialize(xmlString);
#endif
                // Сформировать HTML-скрипт: не создавать ссылки
                //return a.CreateScript(false, Setup_SrbAlphabet, Setup_RusAccent, rusKey);

                // Сформировать HTML-скрипт: true=создавать ссылки
                return a.CreateScript(true, Setup_SrbAlphabet, Setup_RusAccent, rusKey);
            }

            return "";
        }

        private string HTMLStartString()
        {
            return "<HTML><HEAD>" +
                   "<LINK rel=\"stylesheet\" type=\"text/css\" href=\"" +
                   Utils.GetWorkDirectory() + Utils.CSS_FILE_NAME + "\">" + Environment.NewLine +
                   "<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" + Environment.NewLine +

                   //"<script src=\"" + Utils.GetWorkDirectory() + "jquery-1.11.0.min.js\"></script>" + Environment.NewLine +
                   //"<script src=\"" + Utils.GetWorkDirectory() + "jquery-1.4.2.min.js\"></script>" + Environment.NewLine +
                   "<script>" + Environment.NewLine +
                   /*
                   "function facechange (objName) { " +
                    "if ( $(objName).css('display') == 'none' ) { " +
                    "$(objName).animate({height: 'show'}, 0); " +
                    "} else { " +
                    "$(objName).animate({height: 'hide'}, 0); " +
                     "} }" + 
                   */
                   "function viewdiv(id) {" +
                    "var el = document.getElementById(id);" +
                    "if (el.style.display == \"block\") {" +
                    "el.style.display = \"none\";" +
                    "} else {" +
                    "el.style.display = \"block\";" +
                    "}}" +
                    "</script>" + Environment.NewLine +

                   //"</HEAD><BODY style=\"background-color:#f4f4f4;\">" + Environment.NewLine;
                   "</HEAD><BODY style=\"background-color:#ffffff;\">" + Environment.NewLine;
        }
    }
}
