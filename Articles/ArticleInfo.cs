using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords.Articles
{
    [Serializable]
    class ArticleInfo
    {
        //[XmlArray("Имя_коллекции")]
        //[XmlArrayItem("Имя_элемента")]
        public List<WordInfo> Words;

        public WordInfo MW;

        public int Num;

        /*
        public int Top
        {
            get
            {
                return (Num - 1) * (23 + 10);
            }
        }
        */

        #region Конструкторы
        public ArticleInfo()
            : this(0, new List<WordInfo>())
        {
        }

        public ArticleInfo(int articleCount, List<WordInfo> words)
        {
            this.Num = articleCount;

            this.Words = new List<WordInfo>();
            foreach (WordInfo w in words)
            {
                w.Owner = this;
                this.Words.Add(new WordInfo(w));
            }

            this.SelectMainWord();
        }
        #endregion


        public void SelectMainWord()
        {
            if (this.Words.Count == 0)
                return;

            this.MW = this.Words[0];
            this.Words[0].L = WordInfo.LangEnum.lS;
        }


        /// <summary>
        /// Заменить слово на коллекцию слов.
        /// </summary>
        /// <param name="oldWord"></param>
        /// <param name="newWords"></param>
        public void ReplaceWords(WordInfo oldWord, List<WordInfo> newWords)
        {
            // Если одно и пустое слово
            if (newWords.Count == 1 && newWords[0].N == "")
            {
                // Удалить слово
                this.Words.RemoveAt(this.Words.IndexOf(oldWord));
                return;
            }

            // Удаление пустых слов из коллекции.
            List<WordInfo> newWordsWithoutEmpty = new List<WordInfo>();
            foreach (WordInfo word in newWords)
                if (word.N != "")
                    newWordsWithoutEmpty.Add(word);

            // Добавить ссылку на статью
            foreach (WordInfo word in newWordsWithoutEmpty)
                word.Owner = oldWord.Owner;

            // Вставить коллекцию слов
            this.Words.InsertRange(this.Words.IndexOf(oldWord), newWordsWithoutEmpty);

            // Удалить слово
            this.Words.RemoveAt(this.Words.IndexOf(oldWord));
        }

        /// <summary>
        /// Вставить коллекцию слов после слова afterWord.
        /// </summary>
        /// <param name="afterWord"></param>
        /// <param name="newWords"></param>
        public void InsertWords(WordInfo afterWord, List<WordInfo> newWords)
        {
            // Удаление пустых слов из коллекции.
            List<WordInfo> newWordsWithoutEmpty = new List<WordInfo>();
            foreach (WordInfo word in newWords)
                if (word.N != "")
                    newWordsWithoutEmpty.Add(word);

            // Добавить ссылку на статью
            foreach (WordInfo word in newWordsWithoutEmpty)
                word.Owner = afterWord.Owner;

            // Вставить коллекцию слов
            this.Words.InsertRange(this.Words.IndexOf(afterWord) + 1, newWordsWithoutEmpty);
        }


        #region Процедуры, которые настраивают вид формируемого HTML-скрипта.
        /// <summary>
        /// Создать ссылки для сербских слов.
        /// </summary>
        /// <param name="isCreateRef"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private string MakeReference(bool isCreateRef, WordInfo w, String alphabet, String refType)
        {
            string result;

            if (isCreateRef && !String.IsNullOrEmpty(w.R))
            {
                if (refType == "1")
                    result = "<A HREF=\"@" + w.N + "\" CLASS=\"SrbRefStrong\">" + GetSrbWordForHTML(w, alphabet) + "</A>";
                else
                    result = "<A HREF=\"@" + w.N + "\" CLASS=\"SrbRef\">" + GetSrbWordForHTML(w, alphabet) + "</A>";
            }
            else
            {
                result = GetSrbWordForHTML(w, alphabet);
            }

            return result;
        }

        /// <summary>
        /// Получить строку для сербского слова при формировании HTML-скрипта.
        /// Можно преобразовать слово в латиницу.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private string GetSrbWordForHTML(WordInfo w, String alphabet)
        {
            return GetSrbWordForHTML(w.I, alphabet);
        }
        private string GetSrbWordForHTML(String s, String alphabet)
        {
            if (alphabet == "lat")
                return Utils.CyrToLat(Accent.TextToHtml(s));
            //return Utils.CyrToLat(Accent.RemoveAccents(s));
            //return Utils.CyrToLat(ColorAccentSrb(s));
            else
                return Accent.TextToHtml(s);
            //return Accent.RemoveAccents(s);
            //return ColorAccentSrb(s);
        }

        /// <summary>
        /// Выделить ударение цветом (для сербских слов).
        /// Пока не используется.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private string ColorAccentSrb(String word)
        {
            string result = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (Accent.IsAccentChar(word[i]))
                    result = result.Substring(0, result.Length - 1) +
                        "<SPAN CLASS=\"Accent\">" +
                        result.Substring(result.Length - 1, 1) +
                        "</SPAN>";
                else
                    result += word[i];
            }

            return result;
        }

        /// <summary>
        /// Получить строку для русского слова при формировании HTML-скрипта.
        /// Учитываются настройки, как показывать ударение.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private string GetRusWordForHTML(WordInfo w, String rusAccentType)
        {
            if (rusAccentType == "sign")
                //return w.I;        // Вместе с ударениями 
                return w.I.Replace('’', '\u0301');  // Новый вид русского ударения
            else if (rusAccentType == "color")
                return ColorAccent(w); // Выделить ударение цветом
            else
                return w.N;         // Без ударений
        }

        /// <summary>
        /// Выделить ударение цветом (для русских слов).
        /// Здесь не используется свойство word.Accents, хотя можно бы... хотя нет, не надо.
        /// Не надо, потому что при преобразовании кириллицы в латиницу одна буква может превратиться в две,
        /// а, значит, нужно будет менять свойство word.Accents, сдвигая некоторые ударения.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private string ColorAccent(WordInfo w)
        {
            string result = "";
            for (int i = 0; i < w.I.Length; i++)
            {
                if (w.I.Substring(i, 1) == '\u2019'.ToString()) /*"’"*/
                    result = result.Substring(0, result.Length - 1) +
                        "<SPAN CLASS=\"Accent\">" +
                        result.Substring(result.Length - 1, 1) +
                        "</SPAN>";
                /*
                else if (w.I.Substring(i, 1) == "ё")
                    result += "<SPAN CLASS=\"Accent\">" + w.I[i] + "</SPAN>";
                */
                else
                    result += w.I[i];
            }

            return result;
        }

        /// <summary>
        /// Как показывать тильду: как тильду, или как главное слово.
        /// </summary>
        private WordInfo ShowTildaOrWord(bool showTilda, WordInfo tildaWord, WordInfo mainWord)
        {
            if (showTilda)
                return tildaWord;
            else
                return mainWord;
        }
        #endregion


        /// <summary>
        /// Получить строку ключевых слов, разделенных точкой с запятой.
        /// </summary>
        /// <returns></returns>
        public string GetValuedWordsString()
        {
            string result = String.Empty;
            string buf = String.Empty;

            foreach (WordInfo w in this.Words)
            {
                if (w.V)
                {
                    if (w.WT == WordInfo.WTypeEnum.tW || w.WT == WordInfo.WTypeEnum.tD || w.WT == WordInfo.WTypeEnum.tM)
                    {
                        if (w.N.Substring(w.N.Trim().Length - 1) == "-")
                            buf = buf + w.N;
                        else
                            buf = buf + w.N + " ";
                    }
                    else
                    {
                        if (w.N.Substring(w.N.Trim().Length - 1) == "-")
                            buf = buf.Trim() + w.N;
                        else if (w.N.Substring(w.N.Trim().Length - 1) == "(")
                            buf = buf + w.N;
                        else
                            buf = buf.Trim() + w.N + " ";
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(buf))
                        result += buf.Trim() + ";";

                    buf = String.Empty;
                }
            }

            if (!String.IsNullOrEmpty(buf))
                result += buf.Trim() + ";";

            if (!String.IsNullOrEmpty(result))
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        /// <summary>
        /// Клонирование статьи.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public ArticleInfo CloneArticle()
        {
            ArticleInfo newArticle = new ArticleInfo();

            newArticle.Words = new List<WordInfo>();
            foreach (WordInfo w in this.Words)
                newArticle.Words.Add(w.CloneWord(newArticle));

            newArticle.Num = this.Num;
            newArticle.MW = this.MW;

            return newArticle;
        }

        /// <summary>
        /// Сравнение статей.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CompareArticle(ArticleInfo other)
        {
            if (this.Words.Count != other.Words.Count)
                return false;

            for (int i = 0; i < this.Words.Count; i++)
            {
                if (!this.Words[i].CompareWord(other.Words[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Поиск строки во всех Words статьи.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<int> Search(string s)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < this.Words.Count; i++)
            {
                if (this.Words[i].N.Contains(s))
                    result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// Процедуры для формирования сообщений.
        /// </summary>
        /// <returns></returns>
        private string WarningTitul()
        {
            return "Статья (" + this.Num.ToString() + ") \"" + this.MW.N + "\" ";
        }

        private string WarningTitul(string name)
        {
            return WarningTitul() + ": в слове \"" + name + "\" ";
        }

        ///// <summary>
        ///// Внести необходимые изменения в статью (в том числе - обязательно - перед сохранением в БД).
        ///// </summary>
        //public bool ChangeArticleBeforeExport(ref List<String> messList)
        //{
        //    bool result = false;

        //    for (int i = 0; i < this.Words.Count; i++)
        //    {
        //        WordInfo w = this.Words[i];

        //        // Замена старых ударений на новые
        //        if (w.ReplaceOldAccents())
        //        {
        //            messList.Add(WarningTitul(w.N) + "сделана замена старых ударений на новые.");
        //            result = true;
        //        }

        //        // Замена неправильных Jj
        //        if (w.N.Contains("J") || w.N.Contains("j"))
        //        {
        //            w.N = w.N.Replace('J', '\u0408');
        //            w.N = w.N.Replace('j', '\u0458');
        //            w.ReplaceInput(w.I.Replace('J', '\u0408'));
        //            w.ReplaceInput(w.I.Replace('j', '\u0458'));
        //            messList.Add(WarningTitul(w.N) + "сделана замена неправильных Jj.");
        //            result = true;
        //        }

        //        // Замена З) на 3)
        //        if (i < this.Words.Count - 1)
        //        {
        //            if (w.N == "З" && this.Words[i + 1].N == ")")
        //            {
        //                w.N = "3";
        //                w.ReplaceInput("3");
        //                w.WT = WordTypeEnum.wtDigit;
        //                messList.Add(WarningTitul(w.N) + "сделана замена З на 3." + Environment.NewLine);
        //                result = true;
        //            }
        //            else if (w.N == "3" && this.Words[i + 1].N == ")" && w.WT != WordTypeEnum.wtDigit)
        //            {
        //                w.WT = WordTypeEnum.wtDigit;
        //                messList.Add(WarningTitul(w.N) + "3 - присвоен числовой тип." + Environment.NewLine);
        //                result = true;
        //            }
        //        }

        //        // Замена латинских букв, похожих написанием на русские
        //        List<Char> latLetters = new List<char>();
        //        latLetters.AddRange(new Char[] { 'a', 'c', 'e', 'o', 'p' });
        //        List<Char> cyrLetters = new List<char>();
        //        cyrLetters.AddRange(new Char[] { 'а', 'с', 'е', 'о', 'р' });
        //        for (int j = 0; j < latLetters.Count; j++)
        //        {
        //            if (w.N.Contains(latLetters[j].ToString()))
        //            {
        //                w.N = w.N.Replace(latLetters[j], cyrLetters[j]);
        //                w.ReplaceInput(w.I.Replace(latLetters[j], cyrLetters[j]));
        //                messList.Add(WarningTitul(w.N) + "сделана замена латинских букв, похожих написанием на русские.");
        //                result = true;
        //            }
        //        }

        //        // Если слово содержит "~", то оно точно должно быть сербским 
        //        if (w.N.Contains("~") && w.L != LanguageEnum.leSrb)
        //        {
        //            w.L = LanguageEnum.leSrb;
        //            messList.Add(WarningTitul(w.N) + "помечено как сербское.");
        //            result = true;
        //        }

        //        // Замена неправильных тире на правильные
        //        if (w.N.Contains("—"))
        //        {
        //            w.N = w.N.Replace("—", "-");
        //            w.ReplaceInput(w.I.Replace("—", "-"));
        //            messList.Add(WarningTitul(w.N) + "сделана замена неправильных тире на правильные.");
        //            result = true;
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Сериализация статьи в строку.
        /// </summary>
        /// <returns></returns>
        //public string Serialize()
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArticleInfo));

        //    using (TextWriter textWriter = new StringWriter())
        //    {
        //        xmlSerializer.Serialize(textWriter, this);
        //        return textWriter.ToString();
        //    }
        //}

        // Парсер новой версии словарной статьи
        //private fun parseWordInfoSF(xml: String)
        //{
        //    val keys = xml.split("#").toTypedArray()
        //    for (key in keys)
        //    {
        //        if (key.isNotEmpty())
        //        {
        //            words.add(WordInfo(key))
        //        }
        //    }
        //}

        /// <summary>
        /// Десериализация из строки в статью.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public ArticleInfo Deserialize(string xmlString)
        {
            if (String.IsNullOrEmpty(xmlString))
                return this;

            this.Words.Clear();

            String[] keys = xmlString.Split('#');
            for (int i = 0; i < keys.Length; i++)
            {
                if (!String.IsNullOrEmpty(keys[i]))
                {
                    this.Words.Add(new WordInfo(keys[i]));
                }
            }

            if (this.Words.Count > 0) this.MW = this.Words[0];

            return this;
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArticleInfo));

            //using (StringReader stringReader = new StringReader(xmlString))
            //{
            //    return (ArticleInfo)xmlSerializer.Deserialize(stringReader);
            //}
        }

        /// <summary>
        /// Возвращает true, если полное ключевое словосочетание по индексу слова в Art.W
        /// совпадает с rusKey. (Сравнение производится без пробелов).
        /// </summary>
        /// <returns></returns>
        private bool EqualFullValuedByPart(string rusKey, int index)
        {
            if (!this.Words[index].V)
                return false;

            string result = this.Words[index].N;
            int i;
            WordInfo w;

            for (i = index - 1; i >= 0; i--)
            {
                w = this.Words[i];
                if (w.V)
                    result = w.N + " " + result;
                else
                    break;
            }

            for (i = index + 1; i < this.Words.Count; i++)
            {
                w = this.Words[i];
                if (w.V)
                    result += " " + w.N;
                else
                    break;
            }

            return String.Equals(rusKey.Replace(" ", ""), result.Replace(" ", ""));
        }

        /*
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(ref long x);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        extern static short QueryPerformanceFrequency(ref long x);
        */

        /// <summary>
        /// Создать HTML-скрипт для статьи.
        /// </summary>
        /// <returns></returns>
        public string CreateScript()
        {
            return CreateScript(false, "cyr", "color");
        }
        public string CreateScript(bool isCreateRef, string alphabet, string rusAccentType)
        {
            return CreateScript(isCreateRef, alphabet, rusAccentType, "");
        }
        public string CreateScript(bool isCreateRef, string alphabet, string rusAccentType, string rusKey)
        {
            /*
            long ctr1 = 0, ctr2 = 0, freq = 0;
            QueryPerformanceCounter(ref ctr1);
            */
            const string YELLOW_BGR = "#fffbbb";

            string SP = Environment.NewLine;
            StringBuilder buf = new StringBuilder();
            String acrValue = "";
            String refType = "";
            WordInfo prevWord = null;
            WordInfo prevPrevWord = null;
            WordInfo w;
            WordInfo nextWord = null;
            String prevSpan = "";
            bool isBegin = true;     // продолжается первая строка
            bool isNote = false;     // текст внутри ()
            bool isBracket = false;  // текст внутри () на первой строке 
            bool isBracketSquare = false; // текст внутри []
            bool wasVerbSe = false;  // конструкция "глагол се"
            bool isPhrase = false;   // встретился символ "◊"
            bool isAcronym = false;  // слово из списка акронимов
            bool isSM = false;       // см или ср на первой строке
            bool isParag = false;    // открыт тег <P>
            int k = 0;

            String goesValued = ""; // Строка, где идут подряд ключевые слова (русские) - для организации ссылок

            // Считать словарь акронимов
            //List<String> acronymsList = Utils.GetListAcronyms();

            //string mainWord = this.MW.N;
            string mainWord = this.Words[0].N;

            for (int i = 0; i < this.Words.Count; i++)
            {
                w = this.Words[i];

                if (i < this.Words.Count - 1)
                    nextWord = this.Words[i + 1];
                else
                    nextWord = null;

                // Перевод на новую строку вручную
                if (w.N == "@")
                {
                    if (isParag)
                    {
                        buf.Append("</P>");
                        isParag = false;
                    }
                    buf.Append("<BR>");
                    isBegin = true;

                    isSM = false;
                    continue;
                }

                acrValue = "";
                isAcronym = IsWordAcronym(i, ref acrValue);

                // Является ли слово акронимом
                //int acrPos = acronymsList.IndexOf(w.N);
                //isAcronym = (acrPos > -1);
                //acrValue = "";
                //if (acrPos > -1)
                //{
                //    if (Utils.Acronyms.ContainsKey(acronymsList[acrPos] + "."))
                //        acrValue = Utils.Acronyms[acronymsList[acrPos] + "."];
                //} 

                // Если это - главное слово статьи
                if (k == 0)
                {
                    buf.Append("<SPAN CLASS=\"SrbWord\">");
                    buf.Append(GetSrbWordForHTML(w, alphabet));
                    buf.Append("</SPAN>");
                }
                else
                {
                    if (w.N == "см" || w.N == "ср")
                    {
                        if (prevWord.N != "(")
                            buf.Append(SP);
                        buf.Append("&nbsp;");
                        buf.Append("<SPAN CLASS=\"RusComment\">");
                        buf.Append(GetRusWordForHTML(w, rusAccentType));
                        buf.Append("</SPAN>");
                        if (isBegin)
                            isSM = true;
                    }
                    else
                        if (Utils.IsRomanDigit(w.N))
                    {
                        if (isBegin && w.N == "I")
                        {
                            buf.Append(SP);
                            buf.Append("&nbsp;");
                            buf.Append("<SPAN CLASS=\"WordPart\">");
                            buf.Append(w.N);
                            buf.Append("</SPAN>");
                        }
                        else
                        {
                            if (!isBracket && // добавлено спец. из-за слова "запльувати"
                                (prevWord.N.ToLower() == mainWord.ToLower() ||
                                 prevWord.N.ToLower() == mainWord.ToLower() + "!" ||
                                 prevWord.N.ToLower() + "!" == mainWord.ToLower() ||
                                 prevWord.N.ToLower() == mainWord.ToLower() + "?" ||
                                 prevWord.N.ToLower() + "?" == mainWord.ToLower()))
                            {
                                // Здесь сделать замену предыдущего слова
                                string strBuf = buf.ToString();
                                int pos = strBuf.LastIndexOf("<SPAN");
                                strBuf = strBuf.Substring(0, pos);
                                buf = new StringBuilder(strBuf);
                                if (isParag)
                                {
                                    buf.Append("</P>");
                                    isParag = false;
                                }
                                buf.Append("<BR><BR>");
                                buf.Append("<SPAN CLASS=\"SrbWord\">");
                                buf.Append(GetSrbWordForHTML(prevWord, alphabet));
                                buf.Append("</SPAN>");
                                buf.Append(SP);
                                buf.Append("&nbsp;");
                                buf.Append("<SPAN CLASS=\"WordPart\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                                isBegin = true;
                                isSM = false;
                            }
                            else if (prevPrevWord.N == mainWord && prevWord.N == "се")
                            {
                                //int pos = buf.LastIndexOf("<SPAN");
                                //buf = buf.Substring(0, pos);
                                string strBuf = buf.ToString();
                                int pos = strBuf.LastIndexOf("<SPAN");
                                strBuf = strBuf.Substring(0, pos);
                                buf = new StringBuilder(strBuf);
                                if (isParag)
                                {
                                    buf.Append("</P>");
                                    isParag = false;
                                }
                                buf.Append("<BR><BR>");
                                buf.Append("<SPAN CLASS=\"SrbWord\">");
                                buf.Append(GetSrbWordForHTML(prevPrevWord, alphabet));
                                buf.Append(GetSrbWordForHTML(prevWord, alphabet));
                                buf.Append("</SPAN>");
                                buf.Append(SP);
                                buf.Append("&nbsp;");
                                buf.Append("<SPAN CLASS=\"WordPart\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                                isBegin = true;
                                if (isParag)
                                {
                                    buf.Append("</P>");
                                    isParag = false;
                                }
                                isSM = false;
                            }
                            else
                            {
                                buf.Append(SP);
                                buf.Append("<SPAN CLASS=\"WordPart\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }
                        }
                    }
                    else if (w.N == "се" && prevWord.N == "~" && !wasVerbSe)
                    {
                        // если главное слово заканчивается на "се"
                        if (this.MW.I.Length >= 3 &&
                            this.MW.I.Substring(this.MW.I.Length - 3, 3).Trim() == "се")
                        {
                            if (isBegin)
                            {
                                // Здесь сделать замену предыдущего слова
                                string strBuf = buf.ToString();
                                int pos = strBuf.LastIndexOf("<SPAN");
                                strBuf = strBuf.Substring(0, pos);
                                buf = new StringBuilder(strBuf);
                                buf.Append("<BR>");
                                buf.Append("<SPAN CLASS=\"SrbMemo\">");
                                buf.Append(GetSrbWordForHTML(prevWord, alphabet));
                                buf.Append("</SPAN>");
                                isBegin = false;
                            }
                            buf.Append(SP);
                            buf.Append("<SPAN CLASS=\"SrbMemo\">");
                            buf.Append(GetSrbWordForHTML(w.N, alphabet));
                            buf.Append("</SPAN>");
                        }
                        // если главное слово - непереходный глагол (без "се"), а теперь начинается переходный
                        else
                        {
                            // если это точно глагол + се, а не какой-нибудь фразеологизм. 
                            // на самом деле, условие, довольно, слабое, если будет нужно - потом усложним:
                            // если следующее слово за "се" - не сербское. 
                            if (i + 1 < this.Words.Count && this.Words[i + 1].L != WordInfo.LangEnum.lS)
                            {
                                string strBuf = buf.ToString();
                                int pos = strBuf.LastIndexOf("<SPAN");
                                strBuf = strBuf.Substring(0, pos);
                                buf = new StringBuilder(strBuf);
                                if (isParag)
                                {
                                    buf.Append("</P>");
                                    isParag = false;
                                }
                                buf.Append("<BR><BR>");
                                buf.Append("<SPAN CLASS=\"SrbWord\">");
                                buf.Append(GetSrbWordForHTML(ShowTildaOrWord(false, prevWord, this.MW), alphabet));
                                buf.Append("</SPAN>");
                                buf.Append(SP);
                                buf.Append("<SPAN CLASS=\"SrbWord\">");
                                buf.Append(GetSrbWordForHTML(w.N, alphabet));
                                buf.Append("</SPAN>");
                                wasVerbSe = true;

                                isBegin = true;

                                isSM = false;
                            }
                            // это сербский пример "~ се + [нешто]" у слова без "се". 
                            // Такое редко, но встречается, например, слово "доко‛пати" (72;5).
                            else
                            {
                                buf.Append(SP);
                                buf.Append("<SPAN CLASS=\"SrbMemo\">");
                                buf.Append(GetSrbWordForHTML(w.N, alphabet));
                                buf.Append("</SPAN>");
                            }
                        }
                    }
                    else if (w.WT == WordInfo.WTypeEnum.tP)
                    {
                        if (w.N == "(" || w.N == "~" || w.N == "-" || w.N == "[")
                        {
                            buf.Append(SP);
                        }

                        if (w.N == "(")
                        {
                            isBracket = true;
                            if (!isBegin && !(w.L == WordInfo.LangEnum.lS))
                            {
                                isNote = true;
                            }
                        }

                        if (w.N == "[")
                        {
                            isBracketSquare = true;
                        }

                        if (isNote || isBracketSquare)
                        {
                            buf.Append("<SPAN CLASS=\"RusComment\">");
                            buf.Append(w.N);
                            buf.Append("</SPAN>");
                        }
                        else
                        {
                            if (w.N == "~")
                            {
                                if (isBegin)
                                {
                                    buf.Append("<SPAN CLASS=\"SrbWord\">");
                                    buf.Append(w.N);
                                    buf.Append("</SPAN>");
                                }
                                else
                                {
                                    buf.Append("<SPAN CLASS=\"SrbMemo\">");
                                    buf.Append(w.N);
                                    buf.Append("</SPAN>");
                                }
                            }
                            else if (w.N == ":")
                            {
                                if (isBegin && !isBracket)
                                {
                                    buf.Append("<BR>");
                                    isBegin = false;
                                    //buf += "<SPAN CLASS=\"Punct\">" + w.N + "</SPAN>";
                                    //buf += "<SPAN CLASS=\"" + prevSpan + "\">" + w.N + "</SPAN>";
                                    buf.Append("<SPAN CLASS=\"SrbMemo\">");
                                    buf.Append(w.N);
                                    buf.Append("</SPAN>");
                                }
                                else
                                {
                                    //buf += "<SPAN CLASS=\"Punct\">" + w.N + "</SPAN>";
                                    buf.Append("<SPAN CLASS=\"");
                                    buf.Append(prevSpan);
                                    buf.Append("\">");
                                    buf.Append(w.N);
                                    buf.Append("</SPAN>");
                                }
                            }

                            else if (w.N == "(" || w.N == ")")
                            {
                                buf.Append("<SPAN CLASS=\"RusMemo\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }

                            // если это - последняя точка 
                            else if (w.N == "." && i == this.Words.Count - 1)
                            {
                                buf.Append("<SPAN CLASS=\"RusMemo\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }
                            // если это - точка после римской цифры
                            else if (w.N == "." && Utils.IsRomanDigit(prevWord.N))
                            {
                                buf.Append("<SPAN CLASS=\"RusMemo\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }
                            else
                            {
                                //prevWord.WT
                                //buf += "<SPAN CLASS=\"Punct\">" + w.N + "</SPAN>";

                                buf.Append("<SPAN CLASS=\"");
                                buf.Append(prevSpan);
                                buf.Append("\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");

                                //---
                                //if (w.N == "." && Utils.IsRomanDigit(prevWord.N) && isBegin)
                                //    isBegin = false;
                                //---
                            }
                        }

                        if (w.N == ")")
                        {
                            isBracket = false;
                            isSM = false;
                            if (!isBegin)
                            {
                                isNote = false;
                            }
                        }

                        if (w.N == "]")
                        {
                            isBracketSquare = false;
                        }

                        // если знак пунктуации в составе ключевого слова: блок реализации ссылок на русские ключевые слова
                        if (w.V)
                        {
                            goesValued += w.N;
                            if (nextWord == null || !nextWord.V)
                            {
                                if (!String.IsNullOrEmpty(goesValued))
                                {
                                    int pos = buf.ToString().LastIndexOf("@R@");
                                    string href = "<SPAN CLASS=\"RusWord\">";
                                    href += "<A HREF=\"@@" + /*LangUtils.removeBraces(goesValued)*/ goesValued + "\" CLASS=\"RusRef\">";
                                    href += "</SPAN>";
                                    buf = new StringBuilder(buf.ToString().Substring(0, pos) + href + buf.ToString().Substring(pos + 3));
                                    goesValued = "";
                                }
                                buf.Append("</A>");
                            }
                        }
                    }
                    else if (w.WT == WordInfo.WTypeEnum.tW || w.WT == WordInfo.WTypeEnum.tM) // ах! - wtMixed
                    {
                        // не надо пробелов после скобок и в конструкциях типа: 'крестьянин-' 'арендатор' 
                        if (prevWord.N != "(" &&
                            !(prevWord.WT == WordInfo.WTypeEnum.tM &&
                             prevWord.N.Substring(prevWord.N.Length - 1, 1) == "-"))
                        {
                            buf.Append(SP);
                        }

                        if (w.L == WordInfo.LangEnum.lS)
                        {
                            if (prevWord != null && prevPrevWord != null &&
                                prevWord.N == "." && (prevPrevWord.N == "см" || prevPrevWord.N == "ср"))
                                refType = "1";
                            else
                                refType = "";

                            if (isBegin)
                            {
                                buf.Append("<SPAN CLASS=\"SrbWord\">");
                                buf.Append(MakeReference(isCreateRef, w, alphabet, refType));
                                buf.Append("</SPAN>");
                            }
                            else
                            {
                                if (isPhrase)
                                {
                                    buf.Append("<SPAN CLASS=\"Spec\">");
                                    buf.Append(MakeReference(isCreateRef, w, alphabet, refType));
                                    buf.Append("</SPAN>");
                                }
                                else
                                {
                                    buf.Append("<SPAN CLASS=\"SrbMemo\">");
                                    buf.Append(MakeReference(isCreateRef, w, alphabet, refType));
                                    buf.Append("</SPAN>");
                                }
                            }
                        }
                        else if (w.L == WordInfo.LangEnum.lR)
                        {
                            isPhrase = false;

                            if (isBegin && !isBracket)
                            {
                                buf.Append("<BR>");
                                isBegin = false;
                            }

                            if (w.V)
                            {
                                // Обратный словарь: выделить русское слово (Русско-сербский: раскрываем статьи сербских ключевых слов)
                                // Актуально только в desktop-версии, в мобильном приложении rusKey всегда null
                                /*
                                if (rusKey.Length > 0 && EqualFullValuedByPart(rusKey, i))
                                {
                                    
                                    //string href = "<SPAN CLASS=\"RusWord\" STYLE=\"background-color=" + YELLOW_BGR + ";\">";
                                    //href += "<A HREF=\"@@" + rusKey + "\" CLASS=\"RusRef\">";
                                    //href += "</SPAN>";
                                    //buf.Append(href);
                                    
                                    buf.Append("<SPAN CLASS=\"RusWord\" STYLE=\"background-color=" + YELLOW_BGR + ";\">");
                                    buf.Append(GetRusWordForHTML(w, rusAccentType));
                                    buf.Append("</SPAN>");
                                }
                                else
                                {
                                */
                                    // блок реализации ссылок на русские ключевые слова
                                    if (String.IsNullOrEmpty(goesValued))
                                    {
                                        if (nextWord == null || !nextWord.V)
                                        {
                                            string href = "<SPAN CLASS=\"RusWord\">";
                                            href += "<A HREF=\"@@" + /*LangUtils.removeBraces(w.N)*/ w.N + "\" CLASS=\"RusRef\">";
                                            //href += "</SPAN>";
                                            buf.Append(href);
                                        }
                                        else
                                        {
                                            buf.Append("@R@");
                                            goesValued = w.N;
                                        }
                                    }
                                    else
                                    {
                                        if (goesValued.Substring(goesValued.Length - 1) != "-")
                                        {
                                            if (goesValued.Length >= 3 && goesValued.Substring(goesValued.Length - 3) != "..." || goesValued.Length < 3)
                                            {
                                                goesValued += " ";
                                            }
                                        }
                                        goesValued += w.N;
                                    }

                                    //buf.Append("<SPAN CLASS=\"RusWord\">");
                                    buf.Append(GetRusWordForHTML(w, rusAccentType));
                                    //buf.Append("</SPAN>");

                                    // блок реализации ссылок на русские ключевые слова
                                    if (nextWord == null || !nextWord.V)
                                    {
                                        if (!String.IsNullOrEmpty(goesValued))
                                        {
                                            int pos = buf.ToString().LastIndexOf("@R@");
                                            string href = "<SPAN CLASS=\"RusWord\">";
                                            href += "<A HREF=\"@@" + /*LangUtils.removeBraces(goesValued)*/ goesValued + "\" CLASS=\"RusRef\">";
                                            //href += "</SPAN>";
                                            //string href = "<A HREF=\"@@" + /*LangUtils.removeBraces(goesValued)*/ goesValued + "\" CLASS=\"RusRef\">";
                                            buf = new StringBuilder(buf.ToString().Substring(0, pos) + href + buf.ToString().Substring(pos + 3));
                                            goesValued = "";
                                        }
                                        buf.Append("</A>");
                                        buf.Append("</SPAN>");
                                    }
                                //}
                            }
                            else
                            {
                                if (isNote || isBracketSquare)
                                {
                                    buf.Append("<SPAN CLASS=\"RusComment\">");
                                    buf.Append(GetRusWordForHTML(w, rusAccentType));
                                    buf.Append("</SPAN>");
                                }
                                else
                                {
                                    buf.Append("<SPAN CLASS=\"RusMemo\">");
                                    buf.Append(GetRusWordForHTML(w, rusAccentType));
                                    buf.Append("</SPAN>");
                                }
                            }
                        }
                        else if (w.L == WordInfo.LangEnum.lN) // В основном: русские слова с белым фоном
                        {
                            isPhrase = false;

                            if (isBegin && !isBracket)
                            {
                                if (w.N != "м" && w.N != "ж" && w.N != "с" && w.N != "х")
                                {
                                    //if (acronymsList.IndexOf(w.N) == -1)
                                    if (!isAcronym)
                                    // Если использовать это условие, то все прим. главного слова будут
                                    // располагаться на первой строке вместе со словом.
                                    // Иначе они будут переноситься на вторую строку.
                                    {
                                        buf.Append("<BR>");
                                        isBegin = false;
                                    }
                                }
                            }

                            if (isBegin && isAcronym) // акроним в первой строке
                            {
                                if (!isBracket)
                                {
                                    buf.Append(SP);
                                    buf.Append("&nbsp;");
                                }
                                //buf += "<SPAN CLASS=\"RusComment\">" + GetRusWordForHTML(w, rusAccentType) + "</SPAN>";
                                buf.Append("<SPAN CLASS=\"RusComment\"");
                                buf.Append((!String.IsNullOrEmpty(acrValue) ? "TITLE=\"" + acrValue + "\"" : " "));
                                buf.Append(">");
                                buf.Append(GetRusWordForHTML(w, rusAccentType));
                                buf.Append("</SPAN>");
                            }
                            // ---- 29.04.2016 в виде эксперимента ----
                            else if (isAcronym && nextWord != null  // акроним не в первой строке
                                && (nextWord.N == "." || w.N == "и" || (w.N == "с" && acrValue == "с"))
                                && (prevWord.N.Substring(prevWord.N.Length - 1) != "-" || // и не напр. "кого-л"
                                    w.N == "х" || w.N == "д"))                          // кроме с.-х. и ж.-д.
                            {
                                if (!isBracket)
                                {
                                    buf.Append(SP);
                                    buf.Append("&nbsp;");
                                }
                                //buf += "<SPAN CLASS=\"RusComment\">" + GetRusWordForHTML(w, rusAccentType) + "</SPAN>";
                                buf.Append("<SPAN CLASS=\"RusComment\"");
                                buf.Append((!String.IsNullOrEmpty(acrValue) ? "TITLE=\"" + acrValue + "\"" : " "));
                                buf.Append(">");
                                buf.Append(GetRusWordForHTML(w, rusAccentType));
                                buf.Append("</SPAN>");
                            }
                            // ---- 29.04.2016 в виде эксперимента ----
                            else
                            {
                                if (w.V) // Разве слово с белым фоном может быть Valued ??? Ну да ладно...
                                {
                                    buf.Append("<SPAN CLASS=\"RusWord\">");
                                    buf.Append(GetRusWordForHTML(w, rusAccentType));
                                    buf.Append("</SPAN>");
                                    goesValued += w.N;
                                }
                                else
                                {
                                    if (isNote || isBracketSquare)
                                    {
                                        buf.Append("<SPAN CLASS=\"RusComment\">");
                                        buf.Append(GetRusWordForHTML(w, rusAccentType));
                                        buf.Append("</SPAN>");
                                    }
                                    // Если это: а) или б) ...
                                    else if ((w.N == "а" || w.N == "б" || w.N == "в" || w.N == "г") &&
                                                nextWord != null && nextWord.N == ")" && !isBracket)
                                    {
                                        if (isParag)
                                        {
                                            buf.Append("</P>");
                                            isParag = false;
                                        }
                                        buf.Append("<P CLASS=\"P2\"><SPAN CLASS=\"RusMemo\">");
                                        buf.Append(w.N);
                                        buf.Append("</SPAN>");
                                        isParag = true;
                                    }
                                    else
                                    {
                                        if (prevSpan == "SrbMemo")
                                        {
                                            buf.Append(SP);
                                            buf.Append("&nbsp;");
                                        }
                                        buf.Append("<SPAN CLASS=\"RusMemo\">");
                                        buf.Append(GetRusWordForHTML(w, rusAccentType));
                                        buf.Append("</SPAN>");
                                    }
                                }
                            }
                        }
                    }
                    else if (w.WT == WordInfo.WTypeEnum.tD)
                    {
                        if (isBegin && !isBracket)
                        {
                            if (!isSM)
                            {
                                buf.Append("<BR>");
                                isBegin = false;
                            }
                        }

                        if (prevWord.N != "(")
                            buf.Append(SP);

                        if (w.V)
                        {
                            buf.Append("<SPAN CLASS=\"RusWord\">");
                            buf.Append(GetRusWordForHTML(w, rusAccentType));
                            buf.Append("</SPAN>");
                            goesValued += " " + w.N;
                        }
                        else
                        {
                            /*  ANDROID!!! - закомментировано!!!
                            if (isNote || isBracketSquare)
                            {
                                buf.Append("<SPAN CLASS=\"RusComment\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }
                            else
                            */
                                // Если это: 1) или 2) ...
                                if (nextWord != null && nextWord.N == ")" && !isBracket)
                            {
                                if (isParag)
                                {
                                    buf.Append("</P>");
                                    isParag = false;
                                }
                                buf.Append("<P CLASS=\"P1\"><SPAN CLASS=\"Digit\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                                isParag = true;
                            }
                            else
                            {
                                buf.Append("<SPAN CLASS=\"Digit\">");
                                buf.Append(w.N);
                                buf.Append("</SPAN>");
                            }
                        }
                    }
                    else if (w.WT == WordInfo.WTypeEnum.tS)  // Например, '◊' или '≈'
                    {
                        if (isBegin && !isBracket)
                        {
                            buf.Append("<BR>");
                            isBegin = false;
                        }

                        if (w.N == "◊")
                        {
                            if (isParag)
                            {
                                buf.Append("</P>");
                                isParag = false;
                            }
                            buf.Append("<P CLASS=\"P1\"><SPAN CLASS=\"RusMemo\">");
                            buf.Append(w.N);
                            buf.Append("</SPAN>");
                            isParag = true;
                        }
                        else
                        {
                            buf.Append(SP);
                            buf.Append("<SPAN CLASS=\"Spec\">");
                            buf.Append(w.N);
                            buf.Append("</SPAN>");
                        }
                        // Пока не используется
                        //if (w.N == "◊")
                        //{
                        //    isPhrase = true;
                        //}
                    }
                    else  // wtNone '1941 - 1945'
                    {
                        buf.Append(SP);
                        buf.Append("<SPAN CLASS=\"RusMemo\">");
                        buf.Append(w.N);
                        buf.Append("</SPAN>");
                    }
                }

                prevPrevWord = prevWord;
                prevWord = w;

                prevSpan = GetSpanClass(buf.ToString());

                k++;
            }

            // Если остался незакрытый параграф
            if (isParag)
                buf.Append("</P>");

            // Заменить ~ на реальные слова
            //buf = buf.Replace("~", GetSrbWordForHTML(GetRoot(this.MW.Input), alphabet));

            /*
            QueryPerformanceCounter(ref ctr2);
            QueryPerformanceFrequency(ref freq);
            buf.Append("<p>");
            buf.Append("Время постройки: ");
            buf.Append((ctr2 - ctr1) * 1.0 / freq);
            buf.Append(" сек.");
            */

            return buf.ToString();
        }


        /// <summary>
        /// Является ли слово акронимом. Выясняется истинное значение акронима, даже у многословных сокращений.
        /// Акронимом является только сокращение с точкой в конце.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="acrValue"></param>
        /// <returns></returns>
        private bool IsWordAcronym(int pos, ref string acrValue)
        {
            bool result = false;
            acrValue = "";

            WordInfo w = this.Words[pos];
            string name;
            if (w.N == "с" || w.N == "х" || w.N == "ж" || w.N == "д" || w.N == "в" || w.N == "знач")
            {
                if (w.N == "с" || w.N == "ж" || w.N == "в")
                {
                    if (pos + 4 < this.Words.Count)
                    {
                        if (w.N == "с")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos + 2].N == "-")
                                    if (this.Words[pos + 3].N == "х")
                                        if (this.Words[pos + 4].N == ".")
                                        {
                                            name = "с.-х.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "ж")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos + 2].N == "-")
                                    if (this.Words[pos + 3].N == "д")
                                        if (this.Words[pos + 4].N == ".")
                                        {
                                            name = "ж.-д.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "в")
                        {
                            if (this.Words[pos + 1].N == "знач")
                                if (this.Words[pos + 2].N == ".")
                                {
                                    name = "в знач.";
                                    if (Utils.Acronyms.ContainsKey(name))
                                    {
                                        acrValue = Utils.Acronyms[name];
                                        return true;
                                    }
                                }
                        }
                    }
                }
                else if (w.N == "х" || w.N == "д" || w.N == "знач")
                {
                    if (pos - 3 > -1)
                    {
                        if (w.N == "х")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos - 1].N == "-")
                                    if (this.Words[pos - 2].N == ".")
                                        if (this.Words[pos - 3].N == "с")
                                        {
                                            name = "с.-х.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "д")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos - 1].N == "-")
                                    if (this.Words[pos - 2].N == ".")
                                        if (this.Words[pos - 3].N == "ж")
                                        {
                                            name = "ж.-д.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "знач")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos - 1].N == "в")
                                {
                                    name = "в знач.";
                                    if (Utils.Acronyms.ContainsKey(name))
                                    {
                                        acrValue = Utils.Acronyms[name];
                                        return true;
                                    }
                                }
                        }
                    }
                }
            }

            if (!result)
            {
                if (w.N == "и" || w.N == "т" || w.N == "п")
                {
                    if (pos + 4 < this.Words.Count)
                    {
                        if (w.N == "и")
                        {
                            if (this.Words[pos + 1].N == "т")
                                if (this.Words[pos + 2].N == ".")
                                    if (this.Words[pos + 3].N == "п")
                                        if (this.Words[pos + 4].N == ".")
                                        {
                                            name = "и т. п.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "т")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos + 2].N == "п")
                                    if (this.Words[pos + 3].N == ".")
                                        if (this.Words[pos - 1].N == "и")
                                        {
                                            name = "и т. п.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                        if (w.N == "п")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos - 1].N == ".")
                                    if (this.Words[pos - 2].N == "т")
                                        if (this.Words[pos - 3].N == "и")
                                        {
                                            name = "и т. п.";
                                            if (Utils.Acronyms.ContainsKey(name))
                                            {
                                                acrValue = Utils.Acronyms[name];
                                                return true;
                                            }
                                        }
                        }
                    }
                }
            }

            if (!result)
            {
                if (w.N == "и" || w.N == "др")
                {
                    if (pos + 2 < this.Words.Count)
                    {
                        if (w.N == "и")
                        {
                            if (this.Words[pos + 1].N == "др")
                                if (this.Words[pos + 2].N == ".")
                                {
                                    name = "и др.";
                                    if (Utils.Acronyms.ContainsKey(name))
                                    {
                                        acrValue = Utils.Acronyms[name];
                                        return true;
                                    }
                                }
                        }
                        if (w.N == "др")
                        {
                            if (this.Words[pos + 1].N == ".")
                                if (this.Words[pos - 1].N == "и")
                                {
                                    name = "и др.";
                                    if (Utils.Acronyms.ContainsKey(name))
                                    {
                                        acrValue = Utils.Acronyms[name];
                                        return true;
                                    }
                                }
                        }
                    }
                }
            }

            if (!result)
            {
                if (w.N == "с")  // с мест. п. | с вин. п.  и т. д.
                {
                    if (pos + 4 < this.Words.Count)
                    {
                        if (this.Words[pos + 2].N == ".")
                            if (this.Words[pos + 3].N == "п")
                            {
                                if (this.Words[pos + 4].N == ".")
                                {
                                    acrValue = "с";
                                    return true;
                                }
                            }
                    }
                }
            }

            if (!result)
            {
                name = w.N + ".";
                if (Utils.Acronyms.ContainsKey(name))
                {
                    acrValue = Utils.Acronyms[name];
                    return true;
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает имя класса последнего SPAN, например, "SrbMemo".
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        private string GetSpanClass(string buf)
        {
            int i = buf.LastIndexOf("<SPAN CLASS=");
            int j = buf.IndexOf(">", i + 13);
            string span = buf.Substring(i + 13, j - (i + 13 + 1));

            while (span.Substring(0, 3) == "Acc")
            {
                buf = buf.Substring(0, i);
                i = buf.LastIndexOf("<SPAN CLASS=");
                j = buf.IndexOf(">", i + 13);
                span = buf.Substring(i + 13, j - (i + 13 + 1));
            }

            // Убрать из SPAN опцию STYLE (она есть у некоторых RusWords в обратном русско-сербском словаре):
            i = span.IndexOf("STYLE");
            if (i > -1)
            {
                span = span.Substring(0, i).Trim();
                span = span.Replace("\"", "");
            }

            return span;
        }

        /// <summary>
        /// Возвращает основу слова до символов "||".
        /// </summary>
        /// <param name="wordName"></param>
        /// <returns></returns>
        private string GetRoot(string wordName)
        {
            //TODO
            // Сначала убрать ударения из wordName!
            wordName = Accent.RemoveAccents(wordName);

            int pos = wordName.IndexOf("||");
            if (pos > -1)
                return wordName.Substring(0, pos);
            else
                return wordName;
        }


        //public void GetFromArticle(ArticleInfo article)
        //{
        //    this.Words = new List<WordInfo>();
        //    foreach (WordInfo w in article.Words)
        //        this.Words.Add(GetFromWord(w));

        //    this.Num = article.Number;
        //    this.MW = GetFromWord(article.MainWord);
        //}

        public WordInfo GetFromWord(WordInfo word)
        {
            WordInfo wd = new WordInfo();

            wd.N = word.N;
            wd.I = word.I;
            wd.WT = word.WT;
            wd.Accent = word.Accent;
            wd.L = word.L;
            wd.V = word.V;
            wd.R = word.R;

            wd.Owner = this;

            return wd;
        }

    }
}
