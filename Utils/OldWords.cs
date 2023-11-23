using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using ScanWord;


namespace SRWords
{
    // История ранее просмотренных слов (своя для каждого словаря)
    public class OldWords
    {
        private const int SRBDICT = -1;
        private const int RUSDICT = -2;

        private List<String> list;
        private readonly int dictId = -1;
        private readonly System.Windows.Forms.Timer timer;
        private readonly string ShistoryFile = Utils.GetWorkDirectory() + Setup.HISTORY_FILE_NAME;
        private readonly string RhistoryFile = Utils.GetWorkDirectory() + Setup.HISTORY_RUS_FILE_NAME;
        private int maxLength = 20;
        private int saveDelay = 5;

        public bool IsSrb()
        {
            return this.dictId == SRBDICT ? true : false;
        }

        public bool IsRus()
        {
            return this.dictId == RUSDICT ? true : false;
        }

        public OldWords(int dictId, string _max, string _delay)
        {
            this.dictId = dictId;
            if (int.TryParse(_max, out int tmp))
                this.maxLength = tmp;
            if (int.TryParse(_delay, out tmp))
                this.saveDelay = tmp;

            list = new List<string>();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = saveDelay * 1000;
            timer.Tick += new EventHandler(Timer_Tick);
            DeSerialize();
        }

        /// <summary>
        /// Установка параметров после изменения их в форме "Настройка".
        /// </summary>
        public void SetProperties(string _max, string _delay)
        {
            int tmp;
            if (int.TryParse(_max, out tmp))
                maxLength = tmp;
            if (int.TryParse(_delay, out tmp))
                saveDelay = tmp;

            // Если список больше, чем настройка - уменьшить его
            ResizeListByLength();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            this.Set(timer.Tag.ToString());
            timer.Stop();
        }

        /// <summary>
        /// Возвращает последний элемент списка.
        /// </summary>
        /// <returns></returns>
        public String Get()
        {
            if (list.Count == 0)
                return String.Empty;
            else
                return list[list.Count - 1];
        }

        /// <summary>
        /// Возвращает последний элемент списка.
        /// Если последним является current, то возвращает предпоследний.
        /// </summary>
        /// <returns></returns>
        public String GetLastExclude(String current)
        {
            if (dictId != RUSDICT)
            {
                current = Utils.CyrToLat(current);
            }

            if (list.Count == 0)
                return String.Empty;
            else
            {
                string tmp = list[list.Count - 1];
                if (String.Equals(current, tmp))
                {
                    if (list.Count == 1)
                        return String.Empty;
                    else
                        tmp = list[list.Count - 2];
                }
                return tmp;
            }
        }

        /// <summary>
        /// Возвращает весь список.
        /// </summary>
        /// <returns></returns>
        public List<String> GetAll()
        {
            return list;
        }

        /// <summary>
        /// Возвращает слово в правильном алфавите для сравнения: кириллица для русских слов, латиница - для сербских.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public String CorrectAlphabet(String w, String setup_SrbAlphabet)
        {
            if (dictId == RUSDICT)
                return Utils.LatToCyr(w);
            else
                return setup_SrbAlphabet == "lat" ? Utils.CyrToLat(w) : Utils.LatToCyr(w);
        }

        public Boolean Equals(String t1, String t2)
        {
            return String.Equals(Utils.CyrToLat(t1), Utils.CyrToLat(t2));
        }

        /// <summary>
        /// Помещает элемент в конец списка.
        /// Сохраняет слово в правильном алфавите для сравнения: кириллица для русских слов, латиница - для сербских.
        /// </summary>
        /// <param name="w"></param>
        public void Set(String w)
        {
            if (dictId != RUSDICT)
            {
                w = Utils.CyrToLat(w);
            }

            int pos = list.IndexOf(w);
            if (pos == -1)
            {
                if (list.Count == maxLength)
                {
                    list.Remove(list[0]);
                }
                list.Add(w);
            }
            else
            {
                list.Remove(list[pos]);
                list.Add(w);
            }
        }

        /// <summary>
        /// Помещает элемент в конец списка через <определённое значение> секунд,
        /// если за это время не был выбран другой элемент.
        /// </summary>
        /// <param name="w"></param>
        public void SetWait(String w)
        {
            if (!String.IsNullOrEmpty(w))
            {
                timer.Tag = w;
                timer.Start();
            }
        }

        public void ShowAll()
        {
            string t = "";
            foreach (string w in list)
                t += w + Environment.NewLine;
            System.Windows.Forms.MessageBox.Show(t);
        }

        /// <summary>
        /// Имя файла для хранения истории с учётом словаря.
        /// </summary>
        /// <returns></returns>
        private string GetFileName()
        {
            string xmlFile;
            if (this.dictId == -1)
                xmlFile = this.ShistoryFile;
            else if (this.dictId == -2)
                xmlFile = this.RhistoryFile;
            else
                xmlFile = Path.GetDirectoryName(this.ShistoryFile) + "\\" +
                          Path.GetFileNameWithoutExtension(this.ShistoryFile) + this.dictId.ToString() +
                          Path.GetExtension(this.ShistoryFile);

            return xmlFile;
        }

        public void Serialize()
        {
            string xmlFile = GetFileName();

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<String>));
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xml.Serialize(fs, list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool DeSerialize()
        {
            bool result = false;

            string xmlFile = GetFileName();

            if (File.Exists(xmlFile))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<String>));
                    using (FileStream fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        list = (List<String>)xml.Deserialize(fs);
                        if (list.Count > 0)
                            result = true;

                        // Если список больше, чем настройка - уменьшить его
                        ResizeListByLength();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                list.Clear();
            }

            return result;
        }

        /// <summary>
        /// Если список больше, чем настройка - уменьшить его.
        /// </summary>
        private void ResizeListByLength()
        {
            if (list.Count > maxLength)
            {
                int cntToDel = list.Count - maxLength;
                for (int j = 0; j < cntToDel; j++)
                {
                    list.Remove(list[0]);
                }
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Delete(string name)
        {
            list.Remove(name);
        }
    }
}
