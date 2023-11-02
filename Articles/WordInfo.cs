using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SRWords.Articles
{
    [Serializable]
    class WordInfo
    {
        public WordInfo()
        {
        }
        public WordInfo(WordInfo other)
        {
            this.n = other.n;
            this.i = other.i;
            this.wt = other.wt;
            this.Accent = other.Accent;
            this.Owner = other.Owner;
            this.l = other.l;
            this.V = other.V;
        }
        public WordInfo(String key)
        {
            String[] parts = key.Split('$');
            if (parts.Length > 1)
            {
                getSets(parts[1]);
            }
            else
            {
                getSets(null);
            }

            this.i = parts[0];
            this.n = getName(parts[0]);
        }

        private void getSets(String set)
        {
            if (set == null)
            {
                wt = WTypeEnum.tP;
                l = LangEnum.lN;
                v = false;
                r = "";
            }
            else
            {
                wt = WTypeEnum.tW;
                l = LangEnum.lN;
                v = false;
                r = "";
                for (int i = 0; i < set.Length; i++)
                {
                    switch (set[i])
                    {
                        case 'D':
                            wt = WTypeEnum.tD;
                            break;
                        case 'M':
                            wt = WTypeEnum.tM;
                            break;
                        case 'N':
                            wt = WTypeEnum.tN;
                            break;
                        case 'P':
                            wt = WTypeEnum.tP;
                            break;
                        case 'S':
                            wt = WTypeEnum.tS;
                            break;
                        case 'R':
                            l = LangEnum.lR;
                            break;
                        case 'C':
                            l = LangEnum.lS;
                            break;
                        case 'V':
                            v = true;
                            break;
                        case 'L':
                            r = "REF";
                            break;
                    }
                }
            }
        }

        private String getName(String name)
        {
            String res = "";
            for (int i = 0; i < name.Length; i++)
            {
                switch (name[i])
                {
                    case '_':
                        Accent[i] = AT.aL;
                        break;
                    case '\'':
                    case '\u2019':
                        if (l == LangEnum.lS)
                            Accent[i] = AT.aLU;
                        else
                            Accent[i] = AT.aS; // Долгое восходящее
                        break;
                    case '\u005E':
                        Accent[i] = AT.aLD; // Долгое нисходящее
                        break;
                    case '\u201B':
                        Accent[i] = AT.aSU; // Краткое восходящее
                        break;
                    case '\u201C':
                        Accent[i] = AT.aSD; // Краткое нисходящее
                        break;
                    default:
                        if (name[i] != '|')
                            res += name[i];
                        break;
                }
            }

            return res;
        }

        public WordInfo CloneWord(ArticleInfo owner)
        {
            WordInfo newWord = new WordInfo();

            newWord.n = this.n;
            newWord.i = this.i;
            newWord.wt = this.wt;
            newWord.Accent = this.Accent;
            newWord.l = this.l;
            newWord.V = this.V;

            newWord.Owner = owner;

            return newWord;
        }

        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CompareWord(WordInfo other)
        {
            if (!String.Equals(this.N, other.N))
                return false;

            if (!String.Equals(this.I, other.I))
                return false;

            if (this.L != other.L)
                return false;

            if (this.WT != other.WT)
                return false;

            if (this.Accent.Count != other.Accent.Count)
                return false;

            foreach (KeyValuePair<int, AT> pair in this.Accent)
            {
                if (!other.Accent.ContainsKey(pair.Key))
                    return false;
                else
                {
                    AT accentType = other.Accent[pair.Key];
                    if (accentType != pair.Value)
                        return false;
                }
            }

            if (this.V != other.V)
                return false;

            return true;
        }

        // Классификация слова
        public enum WTypeEnum
        {
            tN, // Неопределенная
            tW, // Слово
            tD, // Цифра
            tP, // Знак пунктуации
            tM, // Слово + пунктуация
            tS  // Спецсимвол
        }

        // Тип языка
        public enum LangEnum
        {
            lN, // Межязыковой символ
            lR, // Русский
            lS  // Сербский
        }

        // Тип ударения
        public enum AT
        {
            aLU, // Долгое восходящее
            aLD, // Долгое нисходящее
            aSU, // Краткое восходящее
            aSD, // Краткое нисходящее
            aL,  // Заударная долгота
            aS   // Русское ударение
        }

        //private val accent = ArrayMap<Int, AT>()
        public Dictionary<int, AT> Accent = new Dictionary<int, AT>();


        [NonSerialized]
        [XmlIgnore()]
        public ArticleInfo Owner;

        /// <summary>
        /// Слово без ударения, так как оно видно в ListForm.
        /// </summary>
        private string n;
        public string N
        {
            get { return n; }
            set { n = value; }
        }

        /// <summary>
        /// Слово вместе со служебными символами (ударениями и т. п.).
        /// </summary>
        private string i;
        public string I
        {
            get { return i; }
            set
            {
                i = value;
                //ClearWord();
                //WordAnalize();
            }
        }


        private WTypeEnum wt;
        public WTypeEnum WT
        {
            get { return wt; }
            set { wt = value; }
        }

        /// <summary>
        /// Язык.
        /// </summary>
        private LangEnum l;
        public LangEnum L
        {
            get { return l; }
            set
            {
                l = value;
                if (l == LangEnum.lS)
                {
                    v = false;
                }

                // Изменение написания слова в зависимости от языкового статуса
                this.WordAnalizeByLanguage();
            }
        }

        /// <summary>
        /// Значимый перевод.
        /// </summary>
        private bool v;
        public bool V
        {
            get { return v; }
            set
            {
                v = value;
                if (v)
                    L = LangEnum.lR;
#if (!USE_TYPE_RUS)
                else if (L == LangEnum.lR)
                    L = LangEnum.lN;
#endif
            }
        }

        private string r;
        public string R
        {
            get { return r; }
            set { r = value; }
        }


        /*
        override String toString()
        {
            val result = n + "|" + i + "|" + t.toString() + "|" + l.toString() + "|" + v.toString() + "|" + r;
            return result;
        }*/

        /// <summary>
        /// Изменение написания слова в зависимости от языкового статуса.
        /// </summary>
        private void WordAnalizeByLanguage()
        {
            bool isChange = false;
            if (this.l == LangEnum.lS)
            {
                if (this.i.IndexOf("ё") > -1)
                {
                    this.i = this.i.Replace("ё", "е_");
                    isChange = true;
                }
                if (this.i.IndexOf("й") > -1)
                {
                    this.i = this.i.Replace("й", "и_");
                    isChange = true;
                }
            }
            else
            {
                if (this.i.IndexOf("е_") > -1)
                {
                    this.i = this.i.Replace("е_", "ё");
                    isChange = true;
                }
                if (this.i.IndexOf("и_") > -1)
                {
                    this.i = this.i.Replace("и_", "й");
                    isChange = true;
                }
            }

            if (isChange)
            {
                //RecountAccents(this.i);

                // Перерисовать словарную статью
                //if (ChangeInput != null)
                //     ChangeInput(this);
            }
        }

        /*
        /// <summary>
        /// Заново заполнить Accents после изменения input.
        /// </summary>
        private void RecountAccents(string pInput)
        {
            string word = pInput;
            string name = "";

            this.A.Clear();

            for (int i = 0; i < word.Length; i++)
            {
                char ch = word[i];
                char chNext = '\u0000';
                if (i < word.Length - 1)
                    chNext = word[i + 1];

                switch (ch)
                {
                    case '_': // заударное &#x0304;
                        if (name != null)
                        {
                            if (!A.ContainsKey(name.Length))
                                A.Add(name.Length, AccentTypeEnum.atLine);
                        }
                        break;

                    case '\'': // апостроф русский
                        if (name != null)
                        {
                            if (!A.ContainsKey(name.Length))
                                A.Add(name.Length, AccentTypeEnum.atSimple);
                        }
                        break;

                    case '\u25b3': // треугольник [долгое нисходящее] #x0311;
                    case '\u005E':
                        if (name != null)
                        {
                            if (!A.ContainsKey(name.Length))
                                A.Add(name.Length, AccentTypeEnum.atLongDown);
                        }
                        break;

                    case '\u2018': //‘ закорючка вверх [краткое восходящее] &#x0300; 
                    case '\u201B':
                        if (name != null)
                        {
                            if (!A.ContainsKey(name.Length))
                                A.Add(name.Length, AccentTypeEnum.atSmallUp);
                        }
                        break;

                    case '\u2019':  //’ запятая вниз
                        if (name != null)
                        {
                            if (chNext == ch) // две [краткое нисходящее] &#x030F;
                            {
                                if (!A.ContainsKey(name.Length))
                                    A.Add(name.Length, AccentTypeEnum.atSmallDown);
                                i++;
                            }
                            else              // одна [долгое восходящее] &#x0301;
                            {
                                if (!A.ContainsKey(name.Length))
                                    A.Add(name.Length, AccentTypeEnum.atLongUp);
                            }
                        }
                        break;

                    case '\u201F':  // “ вариант двойной кавычки до перехода на Courier New
                    case '\u201C':  // “ после перехода на Courier New
                        if (name != null)
                        {
                            if (!A.ContainsKey(name.Length))
                                A.Add(name.Length, AccentTypeEnum.atSmallDown);
                        }
                        break;

                    default:
                        if (Char.IsDigit(ch))
                        {
                            name += ch;
                        }
                        else if (Char.IsPunctuation(ch) || ch == '!' || ch == '?' || ch == '~')
                        {
                            name += ch;
                        }
                        else if (Char.IsLetter(ch))
                        {
                            name += ch;
                        }
                        else if (ch == '◊' || ch == '≈')
                        {
                            name += ch;
                        }
                        break;
                } // switch
            } // for

            this.n = name;
        }
        */
    }
}
