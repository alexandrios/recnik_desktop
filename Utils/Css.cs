using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ScanWord;


namespace SRWords
{
    public static class Css
    {
        public static string cssFile = Utils.GetWorkDirectory() + @"\" + Utils.CSS_FILE_NAME;

        private static string MakeParagraphCss()
        {
            string text = "";

            text += ".p1 {padding-left: 10px; margin: .1em 0;}" + Environment.NewLine;
            text += ".p2 {padding-left: 20px; margin: .1em 0;}" + Environment.NewLine;

            return text;
        }

        public static void MakeCss()
        {
            string text = String.Empty;

            text += "body {padding: 5 5 5 5; margin: 5 5 5 5; background-color: " + GetBodyBackColor() +
                "; font: normal 12px Arial;}" + Environment.NewLine;

            text += "a.SrbRef:link {color: #000000; text-decoration: none;}" + /* unvisited link */
                Environment.NewLine;
            text += "a.SrbRef:visited {color: #000000; text-decoration: none;}" + /* visited link */
                Environment.NewLine;
            text += "a.SrbRef:hover {color: #085d9a; text-decoration: none;}" + /* mouse over link */
                Environment.NewLine;
            //text += "a.SrbRef:active  {color: #000000; text-decoration: none;}" + /* selected link */
            //    Environment.NewLine;

            text += "a.SrbRefStrong:link {color: #000000; text-decoration: underline;}" + /* unvisited link */
               Environment.NewLine;
            text += "a.SrbRefStrong:visited {color: #000000; text-decoration: underline;}" + /* visited link */
               Environment.NewLine;
            text += "a.SrbRefStrong:hover {color: #085d9a; text-decoration: underline;}" + /* mouse over link */
               Environment.NewLine;

            text += "a.RusRef:link {background-color: #fffbbb; color: #000000; border-radius: 7px; text-decoration: none;}" +
                Environment.NewLine;

            /* MainWord or IsBegin or ~ se */
            text += ".SrbWord {color: " + GetSrbWordColor() + "; font: normal " + GetSrbWordFontSize() + "px " +
                GetSrbWordFontName() + ";}" + Environment.NewLine;

            /* Valued */
            text += ".RusWord {color: " + GetRusWordColor() + "; font: normal " + GetRusWordFontSize() + "px " +
                GetRusWordFontName() + ";}" + Environment.NewLine;
            //text += ".RusWord {color: #1a3dc1; font: normal 14px Arial;}" + Environment.NewLine;

            /* Everything srb words excluding SrbWord */
            text += ".SrbMemo {color: " + GetSrbMemoColor() + "; font: normal " + GetSrbMemoFontSize() + "px " +
                GetSrbMemoFontName() + ";}" + Environment.NewLine;
            //text += ".SrbMemo {color: #000000; font: normal 14px Lucida Sans Unicode, Arial Unicode MS;}" + Environment.NewLine;

            /* Everything rus words excluding Valued */
            text += ".RusMemo {color: " + GetRusMemoColor() + "; font: normal " + GetRusMemoFontSize() + "px " +
                GetRusMemoFontName() + ";}" + Environment.NewLine;
            //text += ".RusMemo {color: #5b5b5b; font: normal 14px Arial, Times New Roman;}" + Environment.NewLine;

            /* Everything rus words in brackets */
            text += ".RusComment {color: " + GetRusCommentColor() + "; font: normal italic " + GetRusCommentFontSize() + "px " +
                GetRusCommentFontName() + ";}" + Environment.NewLine;
            //text += ".RusComment {color: grey; font: normal italic 14px Arial, Times New Roman;}" + Environment.NewLine;


            /* Punctuation */
            //text += ".Punct {color: " + GetDigitsColor() + "; font: normal " + GetDigitsFontSize() + "px " +
            //    GetDigitsFontName() + ";}" + Environment.NewLine;
            //text += ".Punct {color: #000000; font: normal 14px Arial, Times New Roman;}" + Environment.NewLine;

            /* Arabic digits */
            text += ".Digit {color: " + GetDigitsColor() + "; font: normal " + GetDigitsFontSize() + "px " +
                GetDigitsFontName() + ";}" + Environment.NewLine;
            //text += ".Digit {color: #000000; font: normal 14px Arial, Times New Roman;}" + Environment.NewLine;
            
            /* Roman digits */
            text += ".WordPart {color: " + GetRomanDigitsColor() + "; font: normal " + GetRomanDigitsFontSize() + "px " +
                GetRomanDigitsFontName() + ";}" + Environment.NewLine;
            //text += ".WordPart {color: #000000; font: normal 16px Lucida Sans Typewriter, Lucida Sans Unicode, Arial;}" + Environment.NewLine;

            /* Special characters */
            text += ".Spec {color: " + GetDigitsColor() + "; font: normal " + GetDigitsFontSize() + "px " +
                GetDigitsFontName() + ";}" + Environment.NewLine;
            //text += ".Spec {color: #000000; font: normal 14px Arial, Times New Roman;}" + Environment.NewLine;


            /* Accents common */
            text += ".Accent {color: " + GetRusWordAccentColor() + ";}" + Environment.NewLine;

            /* Accents for RusMemo */
            text += ".RusMemo .Accent {color: " + GetRusMemoAccentColor() +  ";}" + Environment.NewLine;

            /* Блок сербского слова в обратном словаре */
            text += ".SrbBlock {display: block; position: relative; background=\"white\"; padding-top:5px; padding-bottom:5px;}" + Environment.NewLine;

            /* Кнопка */
            text += ".Button1 {font-size: 10px;}" + Environment.NewLine;
            /*color: white; background: rgb(64,199,129);*/

            text += MakeParagraphCss();

            File.WriteAllText(cssFile, text);
        }

        public static string SrbAlphabetDef()
        {
            return "cyr";
        }
        public static string RusAccentDef()
        {
            return "color";
        }

        public static string ListFontDef()
        {
            return "Arial";
        }
        public static string ListFontSizeDef()
        {
            return "14";
        }
        public static string ListFontColorDef()
        {
            return "#000000";
        }

        public static string GetBodyBackColorDef()
        {
            return "#ffffff";
        }
        private static string GetBodyBackColor()
        {
            string htmlColor = GetBodyBackColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssBodyBackColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetSrbWordColorDef()
        {
            return "#000000";
        }
        private static string GetSrbWordColor()
        {
            string htmlColor = GetSrbWordColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbWordFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetSrbWordFontNameDef()
        {
            return "Lucida Sans Unicode";
        }
        private static string GetSrbWordFontName()
        {
            string fontName = GetSrbWordFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbWordFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetSrbWordFontSizeDef()
        {
            return "18";
        }
        private static string GetSrbWordFontSize()
        {
            string fontSize = GetSrbWordFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbWordFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetRusWordColorDef()
        {
            //return "#400000";
            return "#005100";
        }
        private static string GetRusWordColor()
        {
            string htmlColor = GetRusWordColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusWordFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetRusWordFontNameDef()
        {
            return "Arial";
        }
        private static string GetRusWordFontName()
        {
            string fontName = GetRusWordFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusWordFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetRusWordFontSizeDef()
        {
            return "18";
        }
        private static string GetRusWordFontSize()
        {
            string fontSize = GetRusWordFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusWordFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetSrbMemoColorDef()
        {
            return "#000000";
        }
        private static string GetSrbMemoColor()
        {
            string htmlColor = GetSrbMemoColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbMemoFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetSrbMemoFontNameDef()
        {
            return "Lucida Sans Unicode";
        }
        private static string GetSrbMemoFontName()
        {
            string fontName = GetSrbMemoFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbMemoFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetSrbMemoFontSizeDef()
        {
            return "16";
        }
        private static string GetSrbMemoFontSize()
        {
            string fontSize = GetSrbMemoFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssSrbMemoFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetRusMemoColorDef()
        {
            return "#5b5b5b";
        }
        private static string GetRusMemoColor()
        {
            string htmlColor = GetRusMemoColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusMemoFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetRusMemoFontNameDef()
        {
            return "Arial";
        }
        private static string GetRusMemoFontName()
        {
            string fontName = GetRusMemoFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusMemoFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetRusMemoFontSizeDef()
        {
            return "16";
        }
        private static string GetRusMemoFontSize()
        {
            string fontSize = GetRusMemoFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusMemoFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetRusCommentColorDef()
        {
            //return "Gray";
            return "#0080C0";
        }
        private static string GetRusCommentColor()
        {
            string htmlColor = GetRusCommentColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusCommentFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetRusCommentFontNameDef()
        {
            return "Times New Roman";
        }
        private static string GetRusCommentFontName()
        {
            string fontName = GetRusCommentFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusCommentFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetRusCommentFontSizeDef()
        {
            return "18";
        }
        private static string GetRusCommentFontSize()
        {
            string fontSize = GetRusCommentFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusCommentFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetDigitsColorDef()
        {
            return "#000000";
        }
        private static string GetDigitsColor()
        {
            string htmlColor = GetDigitsColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssDigitsFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetDigitsFontNameDef()
        {
            return "Arial";
        }
        private static string GetDigitsFontName()
        {
            string fontName = GetDigitsFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssDigitsFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetDigitsFontSizeDef()
        {
            return "14";
        }
        private static string GetDigitsFontSize()
        {
            string fontSize = GetDigitsFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssDigitsFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetRomanDigitsColorDef()
        {
            return "#000000";
        }
        private static string GetRomanDigitsColor()
        {
            string htmlColor = GetRomanDigitsColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRomanDigitsFontColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetRomanDigitsFontNameDef()
        {
            return "Lucida Sans Typewriter";
        }
        private static string GetRomanDigitsFontName()
        {
            string fontName = GetRomanDigitsFontNameDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRomanDigitsFont);
            if (!String.IsNullOrEmpty(tmp))
                fontName = tmp;
            return fontName;
        }

        public static string GetRomanDigitsFontSizeDef()
        {
            return "18";
        }
        private static string GetRomanDigitsFontSize()
        {
            string fontSize = GetRomanDigitsFontSizeDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRomanDigitsFontSize);
            if (!String.IsNullOrEmpty(tmp))
                fontSize = tmp;
            return fontSize;
        }

        public static string GetRusWordAccentColorDef()
        {
            //return "Red";
            return "#C10000";
        }
        private static string GetRusWordAccentColor()
        {
            string htmlColor = GetRusWordAccentColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusWordAccentColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string GetRusMemoAccentColorDef()
        {
            //return "#0080FF";
            return "#C10000";
        }
        private static string GetRusMemoAccentColor()
        {
            string htmlColor = GetRusMemoAccentColorDef();
            string tmp = Setup.ReadFromSetup(Setup.CssRusMemoAccentColor);
            if (!String.IsNullOrEmpty(tmp))
                htmlColor = tmp;
            return htmlColor;
        }

        public static string PtToPx(string pt)
        {
            string px = "";
            switch (pt)
            {
                case "6": px = "8"; break;
                case "7": px = "9"; break;
                case "7.5": px = "10"; break;
                case "8": px = "11"; break;
                case "9": px = "12"; break;
                case "10": px = "13"; break;
                case "10.5": px = "14"; break;
                case "11": px = "15"; break;
                case "12": px = "16"; break;
                case "13": px = "17"; break;
                case "13.5": px = "18"; break;
                case "14": px = "19"; break;
                case "14.5": px = "20"; break;
                case "15": px = "21"; break;
                case "16": px = "22"; break;
                case "17": px = "23"; break;
                case "18": px = "24"; break;
                case "19": px = "25"; break;
                case "20": px = "26"; break;
                case "22": px = "29"; break;
                case "24": px = "32"; break;
                case "26": px = "35"; break;
                case "27": px = "36"; break;
                case "28": px = "37"; break;
                case "29": px = "38"; break;
                case "30": px = "40"; break;
                case "32": px = "42"; break;
                case "34": px = "45"; break;
                case "36": px = "48"; break;
            }
            return px;
        }

        public static string PxToPt(string px)
        {
            string pt = "";
            switch (px)
            {
                case "8": pt = "6"; break;
                case "9": pt = "7"; break;
                case "10": pt = "7.5"; break;
                case "11": pt = "8"; break;
                case "12": pt = "9"; break;
                case "13": pt = "10"; break;
                case "14": pt = "10.5"; break;
                case "15": pt = "11"; break;
                case "16": pt = "12"; break;
                case "17": pt = "13"; break;
                case "18": pt = "13.5"; break;
                case "19": pt = "14"; break;
                case "20": pt = "14.5"; break;
                case "21": pt = "15"; break;
                case "22": pt = "16"; break;
                case "23": pt = "17"; break;
                case "24": pt = "18"; break;
                case "25": pt = "19"; break;
                case "26": pt = "20"; break;
                case "29": pt = "22"; break;
                case "32": pt = "24"; break;
                case "35": pt = "26"; break;
                case "36": pt = "27"; break;
                case "37": pt = "28"; break;
                case "38": pt = "29"; break;
                case "40": pt = "30"; break;
                case "42": pt = "32"; break;
                case "45": pt = "34"; break;
                case "48": pt = "36"; break;
            }
            return pt;
        }

    }
}
