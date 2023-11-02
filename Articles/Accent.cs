using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords.Articles
{
    public static class Accent
    {
        public static string TextToHtml(string text)
        {
            if (String.IsNullOrEmpty(text)) return text;

            // Старый тип ударений
            text = text.Replace('\u2019'.ToString() + '\u2019'.ToString(), "&#x030F;"); //[краткое нисходящее]
            text = text.Replace('\u201F'.ToString(), "&#x030F;"); //[краткое нисходящее]

            text = text.Replace('\u2019'.ToString(), "&#x0301;"); //[долгое восходящее]
            text = text.Replace('\u2018'.ToString(), "&#x0300;"); //[краткое восходящее]
            text = text.Replace('\u25b3'.ToString(), "&#x0311;"); //[долгое нисходящее]

            // Новый тип ударений
            text = text.Replace('\u201C'.ToString(), "&#x030F;"); //[краткое нисходящее]
            text = text.Replace('\u2019'.ToString(), "&#x0301;"); //[долгое восходящее]
            text = text.Replace('\u201B'.ToString(), "&#x0300;"); //[краткое восходящее]
            text = text.Replace('\u005E'.ToString(), "&#x0311;"); //[долгое нисходящее]

            text = text.Replace("_", "&#x0304;"); //заударное
            text = text.Replace("'", "&#x0301;"); //[простое === долгое восходящее]
            return text;
        }

        public static string RemoveAccents(string text)
        {
            // Старый тип ударений
            text = text.Replace('\u2019'.ToString() + '\u2019'.ToString(), ""); //[краткое нисходящее]
            text = text.Replace('\u201F'.ToString(), ""); //[краткое нисходящее]

            text = text.Replace('\u2019'.ToString(), ""); //[долгое восходящее]
            text = text.Replace('\u2018'.ToString(), ""); //[краткое восходящее]
            text = text.Replace('\u25b3'.ToString(), ""); //[долгое нисходящее]

            // Новый тип ударений
            text = text.Replace('\u201C'.ToString(), ""); //[краткое нисходящее]
            text = text.Replace('\u2019'.ToString(), ""); //[долгое восходящее]
            text = text.Replace('\u201B'.ToString(), ""); //[краткое восходящее]
            text = text.Replace('\u005E'.ToString(), ""); //[долгое нисходящее]

            text = text.Replace("_", ""); //заударное
            text = text.Replace("'", ""); //[простое === долгое восходящее]
            return text;
        }

        public static bool IsAccentChar(Char ch)
        {
            if (ch == '\u201C' || ch == '\u2019' || ch == '\u201B' || ch == '\u005E' || ch == '_')
                return true;
            return false;
        }
    }
}
