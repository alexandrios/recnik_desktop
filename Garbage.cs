using System;
using System.Collections.Generic;
using System.Text;

namespace SRWords
{
    class Garbage
    {
        /*
        // Подготовка списка для таблицы "rusref" - русские слова, у которых ссылок на сербские слова > 10.
        // Таблицу планируется использовать в новой версии мобильного приложения
        private void rus10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataView dataViewCyr = new DataView();
            int k = 1000;
            int i = 1;
            string result = "";
            DataTable dt = Data.LoadRusDict();

            dataViewCyr.Table = dt;
            dataViewCyr.Sort = "NAME";

            foreach (DataRowView dr in dataViewCyr)
            {
                string text = dr["srbname"].ToString();
                string[] arr = text.Split(';');
                if (arr.Length > 10)
                {
                    foreach (string srbname in arr)
                    {
                        string kw = Data.GetKw(srbname);
                        string t = "insert into rusref (name, srbname, kw) values('" + dr["name"].ToString() + "','" + srbname + "','" + kw + "');";
                        result += t + "\n";
                    }
                }

                if (i > k) break;
                i++;
            }

            File.WriteAllText("rusrefs.txt", result);
        }

        private void gridAOWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRowView dr = (DataRowView)CurrentBS().Current;
            if (dr != null)
            {
                MessageBox.Show("page" + dr["PNUM"].ToString() + ":" + dr["ANUM"].ToString());

                //MessageBox.Show(dr["XML"].ToString());
                ArticleInfo ai = new ArticleInfo();
                ai.Deserialize(dr["XML"].ToString());
                String s = "";
                foreach (WordInfo wi in ai.Words)
                {
                    s += wi.I + " ";
                }
                MessageBox.Show(s);
            }

            string text = "мора"; // "возаром";
            DataTable dt = null;
            string searchText = text;

            while (true)
            { 
                dt = ADSData.GetTableBySelect("select name, kw, xml from words where name like '" + searchText + "%'");
                if (dt.Rows.Count == 0)
                {
                    if (searchText.Length == 1)
                        break;
                    searchText = searchText.Substring(0, searchText.Length - 1);
                }
                else
                {
                    break;
                }
            }

            if (dt.Rows.Count == 0)
                MessageBox.Show("Not found");
            else
            {
                string xml = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (searchText == dt.Rows[0]["NAME"].ToString())
                    {
                        xml = row["XML"].ToString();
                        break;
                    }
                }

                if (!String.IsNullOrEmpty(xml))
                {
                    string res = Research(xml);
                    MessageBox.Show(res);
                    if (res == "м" || res == "ж" || res == "с")
                    { 
                        // Существительное
                    }
                }
            }
        }

        private string Research(string xml)
        {
            string result = "";

            ArticleInfo ai = new ArticleInfo();
            ai.Deserialize(xml);
            List<String> strWords = new List<string>();
            foreach (WordInfo wordInfo in ai.Words)
            {
                strWords.Add(wordInfo.N.ToString());
            }

            String s = "";

            List<String> imenizeList = new List<string>();
            imenizeList.Add("м");
            imenizeList.Add("ж");
            imenizeList.Add("с");
            foreach (string item in imenizeList)
            {
                int idx = strWords.IndexOf(item);
                if (idx > -1)
                {
                    if (ai.Words[idx + 1].N == "." || ai.Words[idx + 1].N == ",")
                    {
                        s = item;
                        break;
                    }
                }
            }

            if (!String.IsNullOrEmpty(s))
                result = s;

            return result;
        }
        */

    }
}
