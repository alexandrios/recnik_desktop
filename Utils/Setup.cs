using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Forms;

namespace SRWords
{
    public static class Setup
    {
        public const string HISTORY_FILE_NAME = "Shistory.xml";
        public const string HISTORY_RUS_FILE_NAME = "Rhistory.xml";

        public const string CurrentTabPage = "CurrentTabPage";
        public const string SrbAlphabet = "SrbAlphabet";
        public const string RusAccent = "RusAccent";

        public const string ListFont = "ListFont";
        public const string ListFontSize = "ListFontSize";
        public const string ListFontColor = "ListFontColor";

        public const string CssBodyBackColor = "CssBodyBackColor";
        
        public const string CssSrbWordFont = "CssSrbWordFont";
        public const string CssSrbWordFontSize = "CssSrbWordFontSize";
        public const string CssSrbWordFontColor = "CssSrbWordFontColor";

        public const string CssRusWordFont = "CssRusWordFont";
        public const string CssRusWordFontSize = "CssRusWordFontSize";
        public const string CssRusWordFontColor = "CssRusWordFontColor";

        public const string CssSrbMemoFont = "CssSrbMemoFont";
        public const string CssSrbMemoFontSize = "CssSrbMemoFontSize";
        public const string CssSrbMemoFontColor = "CssSrbMemoFontColor";

        public const string CssRusMemoFont = "CssRusMemoFont";
        public const string CssRusMemoFontSize = "CssRusMemoFontSize";
        public const string CssRusMemoFontColor = "CssRusMemoFontColor";

        public const string CssRusCommentFont = "CssRusCommentFont";
        public const string CssRusCommentFontSize = "CssRusCommentFontSize";
        public const string CssRusCommentFontColor = "CssRusCommentFontColor";

        public const string CssDigitsFont = "CssDigitsFont";
        public const string CssDigitsFontSize = "CssDigitsFontSize";
        public const string CssDigitsFontColor = "CssDigitsFontColor";

        public const string CssRomanDigitsFont = "CssRomanDigitsFont";
        public const string CssRomanDigitsFontSize = "CssRomanDigitsFontSize";
        public const string CssRomanDigitsFontColor = "CssRomanDigitsFontColor"; 
        
        public const string CssRusWordAccentColor = "CssRusWordAccentColor";
        public const string CssRusMemoAccentColor = "CssRusMemoAccentColor";

        //public const string OpacitySyllable = "OpacitySyllable";

        public const string OldWordsMax = "OldWordsMaxLength";
        public const string OldWordsDelay = "OldWordsDelay";

        public const string ConfirmClose = "ConfirmClose";
        public const string LoadRusWhileStart = "LoadRusWhileStart";

        private static string setupFile = Utils.GetWorkDirectory() + "Setup.xml";
        private static XmlSerializableDictionary<String, String> settings = 
            new XmlSerializableDictionary<string, string>();

        public static string ReadFromSetup(string key)
        {
            string result = "";
            try
            {
                if (DeSerialize())
                {
                    if (settings.ContainsKey(key))
                    {
                        result = settings[key];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        public static void WriteToSetup(string key, string value)
        {
            try
            {
                if (settings.Count == 0)
                {
                    if (File.Exists(setupFile))
                    {
                        DeSerialize();
                    }
                }

                if (settings.ContainsKey(key))
                {
                    settings[key] = value;
                }
                else
                {
                    settings.Add(key, value);
                }

                if (settings.Count > 0)
                    Serialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void Serialize()
        {
            //if (!File.Exists(setupFile))
            //    return;

            string xmlFile = setupFile;
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(XmlSerializableDictionary<String, String>));
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xml.Serialize(fs, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static bool DeSerialize()
        {
            bool result = false;

            string xmlFile = setupFile;
            if (File.Exists(xmlFile))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(XmlSerializableDictionary<String, String>));
                    using (FileStream fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        settings = (XmlSerializableDictionary<String, String>)xml.Deserialize(fs);
                        if (settings.Count > 0)
                            result = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return result;
        }

    }
}
