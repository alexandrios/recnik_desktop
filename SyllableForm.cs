//#define DEMO

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScanWord;

namespace SRWords
{
    public partial class SyllableForm : Form
    {
        // Заполнена ли таблица letters или слоги формировать запросами
#if (!DEMO)
        private bool isLetterTable = true; // Таблица заполнена 
#else
        private bool isLetterTable = false; // Формировать запросами
#endif

        private List<String> sList;
        private List<String> sSign;
        private ListForm _listForm;
        private string _language;
        private int razmer = 10;
        private int interval = 12;

        private Font normalFont = new Font("Arial", 10.5f, FontStyle.Regular);
        //private Font existFont = new Font("Arial", 10.5f, FontStyle.Bold);
        private Font existFont = new Font("Arial", 10.5f, FontStyle.Underline);

        //private Color normalBackColor = SystemColors.Control;
        private Color normalBackColor = Color.White;
        private Color selectBackColor = Color.LightSkyBlue;
        //private Color selectBackColor = Color.LightGoldenrodYellow;
        //private Color stopBackColor = Color.Coral;
        private Color stopBackColor = Color.LightGreen;

        public SyllableForm(ListForm listForm, string language)
        {
            this._listForm = listForm;
            this._language = language;
            InitializeComponent();
        }

        private void SyllableForm_Load(object sender, EventArgs e)
        {
            sList = new List<string>();
            sSign = new List<string>();
            DrawAlphaLetter(null);
        }

        private int DrawAlphaLetter(Button currentButton)
        {
            Control[] controls;
            Button B;
            Size size;
            String context;
            int level;
            int ind = 0;

            Cursor = Cursors.WaitCursor;
            try
            {
                int tstart = 5;
                int lstart = 3;
                int lastHeight = 0;
                int l;

                if (currentButton == null)
                {
                    context = "";
                    level = 0;
                }
                else
                {
                    context = currentButton.Text;
                    level = Int32.Parse(currentButton.Name.Substring(3, currentButton.Name.IndexOf("_") - 3));
                }

                if (!String.IsNullOrEmpty(context))
                {
                    for (int i = _panel.Controls.Count - 1; i >= 0; i--)
                    {
                        int intres = -1;
                        String s = _panel.Controls[i].Name.Substring(3, _panel.Controls[i].Name.IndexOf("_") - 3);
                        if (Int32.TryParse(s, out intres))
                        {
                            // Удалить кнопки выше уровня, на котором находится кнопка, которую нажали
                            if (intres > level)
                            {
                                _panel.Controls.Remove(_panel.Controls[i]);
                            }

                            // Вернуть размер всем кнопкам уровня нажатой кнопки
                            if (intres == level)
                            {
                                B = _panel.Controls[i] as Button;
                                B.Top = tstart + (level - 1) * (razmer + interval); //

                                // Ищем слово в БД
                                //if (_listForm.SearchEqual(B.Text))
                                //    B.Font = existFont;
                                //else
                                //    B.Font = normalFont;

                                size = TextRenderer.MeasureText(B.Text, B.Font);
                               
                                if (B.Width != razmer + size.Width)
                                {
                                    int k = Math.Abs(size.Width + razmer - B.Width);
                                    B.Left += k / 2;
                                    B.Width = razmer + size.Width;
                                }

                                B.Height = razmer + size.Height;

                                if (B.BackColor != stopBackColor)
                                    B.BackColor = normalBackColor;
                            }
                        }
                    }
                }

                // Подготовить список для кнопок текущего уровня
                if (!isLetterTable)
                    sList = GetChilds(ref context);
                else
                    GetChilds2(ref context, out sList, out sSign);

                if (sList.Count == 0)
                    return 0;

                string firstButtonInRowName = "";

                // Сформировать текущий уровень кнопок
                int t = tstart + level * (razmer + interval);
                l = lstart;
                for (int i = 0; i < sList.Count; i++)
                {
                    B = new Button();
                    B.FlatStyle = FlatStyle.Standard;
                    B.Name = "BUT" + (level + 1).ToString() + "_" + ind.ToString();
                    if (i == 0) firstButtonInRowName = B.Name;
                    B.Tag = level + 1;
                    B.Text = sList[i];

                    // Ищем слово в БД
                    if (!isLetterTable)
                    {
                        if (_listForm.SearchEqual(sList[i]))
                            B.Font = existFont;
                        else
                            B.Font = normalFont;
                    }
                    else
                    {
                        if (sSign[i] != "0")
                            B.Font = existFont;
                        else
                            B.Font = normalFont;
                    }

                    if (!isLetterTable)
                    {
                        string tmp = B.Text;
                        if (GetChilds(ref tmp).Count == 0)
                            B.BackColor = stopBackColor;
                        else
                            B.BackColor = normalBackColor;
                    }
                    else
                    {
                        if (sSign[i] == "2")
                            B.BackColor = stopBackColor;
                        else
                            B.BackColor = normalBackColor;
                    }

                    size = TextRenderer.MeasureText(B.Text, B.Font);
                    lastHeight = size.Height;

                    B.Width = razmer + size.Width;
                    B.Height = razmer + size.Height;

                    B.Left = l;
                    B.Top = t;
                    B.Click += new EventHandler(B_Click);
                    B.BringToFront();

                    _panel.Controls.Add(B);

                    l += B.Width;
                    ind++;
                }

                if (!String.IsNullOrEmpty(firstButtonInRowName))
                    (_panel.Controls[firstButtonInRowName] as Button).Focus();

                // Записать ширину ряда в первую кнопку ряда
                controls = _panel.Controls.Find("BUT" + (level + 1).ToString() + "_0", false);
                if (controls.Length == 1)
                {
                    (controls[0] as Button).Tag = l;
                }

                // Определение max ширины рядов
                int maxW = 0;
                for (int i = 1; i <= level + 1; i++)
                {
                    controls = _panel.Controls.Find("BUT" + i.ToString() + "_0", false);
                    if (controls.Length == 1)
                    {
                        int len = -1;
                        if (Int32.TryParse((controls[0] as Button).Tag.ToString(), out len))
                        {
                            if (len > maxW)
                                maxW = len;
                        }
                    }
                }

                // Установить ширину формы
                this.Width = maxW + 9 + lstart - 1;

                // Установить высоту формы
                Rectangle rectAll = this.RectangleToClient(this.Bounds);
                Rectangle rectClient = this.ClientRectangle;
                int captionHeight = rectClient.Top - rectAll.Top;
                this.Height = captionHeight + t + razmer + lastHeight + tstart;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            return ind;
        }

        /// <summary>
        /// Подготовить список для кнопок текущего уровня (с использованием заранее подготовленной таблицы letters).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sList"></param>
        /// <param name="sSign"></param>
        private void GetChilds2(ref string context, out List<string> sList, out List<string> sSign)
        {
            sList = new List<string>();
            sSign = new List<string>();

            if (_language == "cyr")
                Data.SyllableListCyr(context, out sList, out sSign);
            else
            {
                Data.SyllableListCyr(Utils.LatToCyr(context), out sList, out sSign);
                for (int i = 0; i < sList.Count; i++)
                {
                    sList[i] = Utils.CyrToLat(sList[i]);
                }

                // Сортировка (только для lat)
                List<string> sTemp = new List<string>();
                for (int i = 0; i < sList.Count; i++)
                {
                    sTemp.Add(sList[i].PadRight(25) + sSign[i]);
                }
                sTemp.Sort();
                for (int i = 0; i < sList.Count; i++)
                {
                    sList[i] = sTemp[i].Substring(0, 25).Trim();
                    sSign[i] = sTemp[i].Substring(25, 1);
                }
            }


            return;
        }

        /// <summary>
        /// Подготовить список для кнопок текущего уровня (с поиском слов в таблице words).
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<String> GetChilds(ref string context)
        {
            List<String> sList = new List<string>();
            string cur_context = context;

            do
            {
                if (String.IsNullOrEmpty(context))
                {
                    if (_language == "cyr")
                        Data.SyllableFirstLettersCyr(out sList);
                    else
                        Data.SyllableFirstLettersLat(out sList);
                }
                else
                {
                    if (_language == "cyr")
                        Data.SyllableNextLettersCyr(cur_context, out sList);
                        //Data.SyllableNextLettersCyrLinq(cur_context, out sList, _listForm.dtCurrent);
                        //Data.SyllableNextLettersCyrFromDT(cur_context, out sList, _listForm.dtCurrent);
                    else
                        Data.SyllableNextLettersLat(cur_context, out sList);
                }

                //if (sList.Count == 1 && context == sList[0].Trim())
                if (sList.Count == 1 && context == sList[0])
                {
                    sList.Clear();
                    break;
                }

                // Убрать слова, в которых количество букв не равно cur_context.Length + 1
                for (int i = sList.Count - 1; i >= 0; i--)
                {
                    //if (sList[i].Trim().Length != cur_context.Length + 1)
                    if (sList[i].Length != cur_context.Length + 1)
                        sList.Remove(sList[i]);
                }

                if (sList.Count == 1)
                {
                    // Если такое слово есть - его показывать
                    if (_listForm.SearchEqual(sList[0]))
                        break;
                    else
                    {
                        //if (cur_context.Length < sList[0].Trim().Length)
                        if (cur_context.Length < sList[0].Length)
                        {
                            cur_context = sList[0];
                        }
                        else
                            break;
                    }
                }
            } while (sList.Count == 1);

            context = cur_context;

            return sList;
        }

        void B_Click(object sender, EventArgs e)
        {
            Button B = sender as Button;
            DrawClickedButton(B);
            this._listForm.SetSearchTextBox(B.Text);
        }

        void DrawClickedButton(Button B)
        {
            if (B.BackColor != stopBackColor)
            {
                int cntChilds = DrawAlphaLetter(B);
                if (cntChilds == 0)
                {
                    B.BackColor = stopBackColor;
                }
                else
                {
                    B.BackColor = selectBackColor;
                }

                B.Height += 2;
                B.Top -= 1;

                Size size = TextRenderer.MeasureText(B.Text, B.Font);
                int k = size.Width + razmer - B.Width;
                B.Width = size.Width + razmer;
                B.Left -= k / 2;
                B.BringToFront();
            }
        }

        private void SyllableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this._listForm.HideSyllableForm();
        }

        private void SyllableForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F7)
                Close();
            else if (e.KeyCode == Keys.F5)
                this._listForm.keyBoardToolStripMenuItem.PerformClick();
        }

    }
}