using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SRWords
{
    //enum TPosiz {pzLeft, pzRight, pzTop, pzBottom, pzNone};

    public delegate void GetCurrentAlphabet(out string rus_srb, out string cyr_lat);
    public delegate void GetLetterFromKeyboard(string letter);
    
    public partial class KeyboardForm : Form
    {
        private string _srbAlphabet;   // "cyr" "lat"
        private string _language;      // "srb" "rus"
        private bool _isCaps;

        private ListForm _listForm;
        private GetCurrentAlphabet _getCurrentAlphabet;
        private GetLetterFromKeyboard _getLetterFromKeyboard;

        private char[] aLow = new char[34]; //  array[1..36] of Integer;
        private char[] aHigh = new char[34]; //: array[1..36] of Integer;
        private Font normalFont = new Font("Arial", 10.5f, FontStyle.Regular);
        private Font bsFont = new Font("Arial", 16f, FontStyle.Bold);

        public KeyboardForm(ListForm listForm,
            GetCurrentAlphabet getCurrentAlphabet, GetLetterFromKeyboard getLetterFromKeyboard)
        {
            this._listForm = listForm;
            this._getCurrentAlphabet = getCurrentAlphabet;
            this._getLetterFromKeyboard = getLetterFromKeyboard;
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void KeyboardForm_Load(object sender, EventArgs e)
        {   
            // Первое позиционирование формы на экране.
            // Для этой формы StartPosition = WindowsDefaultLocation, 
            // в отличие от SyllableForm, где StartPosition = CenterScreen и первого позиционирования нет.
            // 378 - реальная ширина формы после отрисовки букв, смотреть отладчиком в конце процедуры DrawLetters()
            //this.Left = Screen.PrimaryScreen.WorkingArea.Right - 378 - 5;  //this.Width; 
            //this.Top = Screen.PrimaryScreen.WorkingArea.Bottom - 116 - 5;  //this.Height;

            if (this._getCurrentAlphabet != null)
            {
                string language = this._language;
                string srbAlphabet = this._srbAlphabet;
                this._getCurrentAlphabet(out language, out srbAlphabet);
                bool isCaps = Control.IsKeyLocked(Keys.CapsLock);
                if (isCaps != this._isCaps || language != this._language || srbAlphabet != this._srbAlphabet)
                {
                    this._isCaps = isCaps;
                    this._language = language;
                    this._srbAlphabet = srbAlphabet;
                }
            }

            DrawLetters();
        }


        private void _timer_Tick(object sender, EventArgs e)
        {
            if (this._getCurrentAlphabet != null)
            {
                string language = this._language;
                string srbAlphabet = this._srbAlphabet;
                this._getCurrentAlphabet(out language, out srbAlphabet);
                bool isCaps = Control.IsKeyLocked(Keys.CapsLock);
                if (isCaps != this._isCaps || language != this._language || srbAlphabet != this._srbAlphabet)
                {
                    this._isCaps = isCaps;
                    this._language = language;
                    this._srbAlphabet = srbAlphabet;
                    DrawLetters();
                    this._listForm._searchTextBox.Focus();
                }
            }
        }

        private void DrawLetters()
        {
            int l, t, razmer, line, row;
            int MaxLetter;
            bool ReCreate = true; // panel.Controls.IndexOfKey("BUT0") == -1;
            bool vis = Visible;
            Color normalBackColor = Color.White;
            Button B = null;
 
            Visible = false;

            if (ReCreate)
                _panel.Controls.Clear();

            if (_language == "srb")
            {
                // Инициализация сербского алфавита
                MaxLetter = 31;
                SRB_Alphabet_Init();
            }
            else
            {
                // Инициализация русского алфавита
                MaxLetter = 33;
                RUS_Alphabet_Init();
            }

            line = 1;
            row = 1;
            l = 1;
            t = 1;
            razmer = 30; 

            for (int i = 0; i < MaxLetter; i++)
            {
                if (ReCreate)
                {
                    B = new Button();
                    B.Name = "BUT" + i.ToString();
                    B.Width = razmer;
                    B.Height = razmer;
                    B.Font = normalFont;
                    B.BackColor = normalBackColor;
                    B.Click += new EventHandler(B_Click);   
                    _panel.Controls.Add(B);
                }
                else
                {
                    int k = _panel.Controls.IndexOfKey("BUT" + i.ToString());
                    if (k > -1)
                        B = (Button)_panel.Controls[k];
                }

                B.Text = Alphabet_Get(_language, i, _isCaps); 
                B.Left = l;
                B.Top = t;
                l = l + B.Width;
                row++;

                if (_language == "srb")
                {
                    if (line == 1 && row > 12)
                    {
                        l = B.Width / 3 + 1;
                        line++;
                        t = t + B.Height;
                        row = 1;
                    }
                    else if (line == 2 && row > 12)
                    {
                        l = 2 * (B.Width / 3) + 1;
                        line++;
                        t = t + B.Height;
                        row = 1;
                    }
                }
                else if (_language == "rus")
                {
                    if (line == 1 && row > 12)
                    {
                        l = B.Width / 3 + 1;
                        line++;
                        t = t + B.Height;
                        row = 1;
                    }
                    else if (line == 2 && row > 11)
                    {
                        l = 2 * (B.Width / 3) + 1;
                        line++;
                        t = t + B.Height;
                        row = 1;
                    }
                }
            }

            // Установить ширину формы
            this.Width = 12 * (B.Width + 1) + B.Width / 3 - 4 - (_language == "rus" ? 9 : 0);

            // Установить высоту формы
            Rectangle rectAll = this.RectangleToClient(this.Bounds);
            Rectangle rectClient = this.ClientRectangle;
            int captionHeight = rectClient.Top - rectAll.Top;
            this.Height = captionHeight + 3 * (B.Height + 1) + 2;

            B = new Button();
            B.Name = "BUTBS";
            B.Width = razmer + 10;
            B.Height = razmer;
            B.Font = bsFont;
            B.Left = this.Width - B.Width - 7;
            B.Top = t;
            //B.Text = "<--";
            B.Text = "←";
            B.BackColor = normalBackColor;
            B.Click += new EventHandler(B_ClickBackspace);
            _panel.Controls.Add(B);

            Visible = vis;
        }


        void B_Click(object sender, EventArgs e)
        {
            /*
            string letter = (sender as Button).Text;
            if (this._getLetterFromKeyboard != null)
            {
                this._getLetterFromKeyboard(letter);
            }
            */
            this._listForm.SetSearchTextBoxToSelStart((sender as Button).Text);
        }

        void B_ClickBackspace(object sender, EventArgs e)
        {
            //SendKeys.Send("{BS}");
            this._getLetterFromKeyboard("{BS}");
        }

        private string Alphabet_Get(string language, int index, bool caps)
        {
            if (language == "srb")
            {
                if (caps)
                {
                    if (aHigh[index] == '\0')
                    {
                        int r = (int)aLow[index];
                        int value = r - 32;
                        char c = (char)value;
                        return c.ToString();
                    }
                    else
                        return aHigh[index].ToString();
                }
                else
                {
                    return aLow[index].ToString();
                }
            }
            else
            {
                if (caps)
                {
                    if (index == 32)
                        return '\u0401'.ToString(); // Ё
                    else
                    {
                        int r = (int)aLow[index];
                        int value = r - 32;
                        char c = (char)value;
                        return c.ToString();
                    }
                }
                else
                {
                    return aLow[index].ToString();
                }
            }
        }


        private void RUS_Alphabet_Init()
        {
            aLow[0] = '\u0439';   // й
            aLow[1] = '\u0446';   // ц
            aLow[2] = '\u0443';   // у
            aLow[3] = '\u043A';   // к
            aLow[4] = '\u0435';   // е
            aLow[5] = '\u043D';   // н
            aLow[6] = '\u0433';   // г
            aLow[7] = '\u0448';   // ш
            aLow[8] = '\u0449';   // щ
            aLow[9] = '\u0437';   // з
            aLow[10] = '\u0445';   // х
            aLow[11] = '\u044A';   // ъ

            aLow[12] = '\u0444';   // ф
            aLow[13] = '\u044B';   // ы
            aLow[14] = '\u0432';   // в
            aLow[15] = '\u0430';   // а
            aLow[16] = '\u043F';   // п
            aLow[17] = '\u0440';   // р
            aLow[18] = '\u043E';   // о
            aLow[19] = '\u043B';   // л
            aLow[20] = '\u0434';   // д
            aLow[21] = '\u0436';   // ж
            aLow[22] = '\u044D';   // э

            aLow[23] = '\u044F';   // я
            aLow[24] = '\u0447';   // ч
            aLow[25] = '\u0441';   // с
            aLow[26] = '\u043C';   // м
            aLow[27] = '\u0438';   // и
            aLow[28] = '\u0442';   // т
            aLow[29] = '\u044C';   // ь
            aLow[30] = '\u0431';   // б
            aLow[31] = '\u044E';   // ю
            aLow[32] = '\u0451';   // ё   // '\u0401' Ё
        }


        private void SRB_Alphabet_Init()
        { 
            if (this._srbAlphabet == "cyr")
            {
                // Раскладка CYR
                aLow[0] = '\u0459'; aHigh[0] = '\u0409';    // ль
                aLow[1] = '\u045A'; aHigh[1] = '\u040A';    // нь
                aLow[2] = '\u0435'; aHigh[2] = '\0';    // е
                aLow[3] = '\u0440'; aHigh[3] = '\0';    // р
                aLow[4] = '\u0442'; aHigh[4] = '\0';    // т
                aLow[5] = '\u0437'; aHigh[5] = '\0';    // з
                aLow[6] = '\u0443'; aHigh[6] = '\0';    // у
                aLow[7] = '\u0438'; aHigh[7] = '\0';    // и
                aLow[8] = '\u043E'; aHigh[8] = '\0';    // о
                aLow[9] = '\u043F'; aHigh[9] = '\0';  // п
                aLow[10] = '\u0448'; aHigh[10] = '\0';  // ш
                aLow[11] = '\u0452'; aHigh[11] = '\u0402';  // дьжь

                aLow[12] = '\u0430'; aHigh[12] = '\0';  // a
                aLow[13] = '\u0441'; aHigh[13] = '\0';  // с
                aLow[14] = '\u0434'; aHigh[14] = '\0';  // д
                aLow[15] = '\u0444'; aHigh[15] = '\0';  // ф
                aLow[16] = '\u0433'; aHigh[16] = '\0';  // г
                aLow[17] = '\u0445'; aHigh[17] = '\0';  // х
                aLow[18] = '\u0458'; aHigh[18] = '\u0408'; // j
                aLow[19] = '\u043A'; aHigh[19] = '\0';  // к
                aLow[20] = '\u043B'; aHigh[20] = '\0';  // л
                aLow[21] = '\u0447'; aHigh[21] = '\0';  // ч
                aLow[22] = '\u045B'; aHigh[22] = '\u040B';  // чь
                aLow[23] = '\u0436'; aHigh[23] = '\0';  // ж

                aLow[24] = '\u0073'; aHigh[24] = '\u0053';  // s
                aLow[25] = '\u045F'; aHigh[25] = '\u040F';  // дж
                aLow[26] = '\u0446'; aHigh[26] = '\0';  // ц
                aLow[27] = '\u0432'; aHigh[27] = '\0';  // в
                aLow[28] = '\u0431'; aHigh[28] = '\0';  // б
                aLow[29] = '\u043D'; aHigh[29] = '\0';  // н
                aLow[30] = '\u043C'; aHigh[30] = '\0';  // м
            }
            else
            {
                // Раскладка LAT
                aLow[0] = '\u0071'; aHigh[0] = '\u0051';    // q
                aLow[1] = '\u0077'; aHigh[1] = '\u0057';    // w
                aLow[2] = '\u0065'; aHigh[2] = '\u0045';    // e
                aLow[3] = '\u0072'; aHigh[3] = '\u0052';    // r
                aLow[4] = '\u0074'; aHigh[4] = '\u0054';    // t
                aLow[5] = '\u007A'; aHigh[5] = '\u005A';    // z
                aLow[6] = '\u0075'; aHigh[6] = '\u0055';    // u
                aLow[7] = '\u0069'; aHigh[7] = '\u0049';    // i
                aLow[8] = '\u006F'; aHigh[8] = '\u004F';    // o
                aLow[9] = '\u0070'; aHigh[9] = '\u0050';    // p
                aLow[10] = '\u0161'; aHigh[10] = '\u0160';  // s>
                aLow[11] = '\u0111'; aHigh[11] = '\u0110';  // d>

                aLow[12] = '\u0061'; aHigh[12] = '\u0041';  // a
                aLow[13] = '\u0073'; aHigh[13] = '\u0053';  // s
                aLow[14] = '\u0064'; aHigh[14] = '\u0044';  // d
                aLow[15] = '\u0066'; aHigh[15] = '\u0046';  // f
                aLow[16] = '\u0067'; aHigh[16] = '\u0047';  // g
                aLow[17] = '\u0068'; aHigh[17] = '\u0048';  // h
                aLow[18] = '\u006A'; aHigh[18] = '\u004A';  // j
                aLow[19] = '\u006B'; aHigh[19] = '\u004B';  // k
                aLow[20] = '\u006C'; aHigh[20] = '\u004C';  // l
                aLow[21] = '\u010D'; aHigh[21] = '\u010C';  // c>
                aLow[22] = '\u0107'; aHigh[22] = '\u0106';  // c'
                aLow[23] = '\u017E'; aHigh[23] = '\u017D';  // z>

                aLow[24] = '\u0079'; aHigh[24] = '\u0059';  // y
                aLow[25] = '\u0078'; aHigh[25] = '\u0058';  // x
                aLow[26] = '\u0063'; aHigh[26] = '\u0043';  // c
                aLow[27] = '\u0076'; aHigh[27] = '\u0056';  // v
                aLow[28] = '\u0062'; aHigh[28] = '\u0042';  // b
                aLow[29] = '\u006E'; aHigh[29] = '\u004E';  // n
                aLow[30] = '\u006D'; aHigh[30] = '\u004D';  // m
            }
        }

        private void KeyboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this._listForm.HideKeyboardForm();
        }

        private void KeyboardForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.F5)
                Close();
            else if (e.KeyCode == Keys.F7)
                this._listForm.mainFindByLetterToolStripMenuItem.PerformClick();
        }

    }
}
