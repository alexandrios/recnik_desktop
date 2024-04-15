using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SRWords
{
    public partial class SetupForm : Form
    {
        public bool IsChanged;
        public int CurrentPage = -1;
        public string alphabetFromMenu = String.Empty;

        //private bool isChangedCss = false;
        private string dictMode = String.Empty;
#if DEMO || SQLITE
        private Int64 dictEditId = -1;
#else
        private int dictEditId = -1;
#endif

        private ApplyChangesDelegate _applyChanges;

        public SetupForm(ApplyChangesDelegate ApplyChanges) : this()
        {
            _applyChanges = ApplyChanges;
        }

        public SetupForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            IsChanged = false;
            //isChangedCss = false;

            SetFontsToComboBox();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            LoadParameters(false);
            LoadUserDicts();

            SetCurrentTabPage();
        }

        /// <summary>
        /// isCSSdefault == true -> вернуть стандартные настройки оформления для карточки.
        /// </summary>
        /// <param name="isCSSdefault"></param>
        private void LoadParameters(bool isCSSdefault)
        {
            Color color;
            string tmp;

            if (!isCSSdefault) // Оформление списка не зависит от оформления карточки по умолчанию
            {
                // Если Setup.xml пуст, то определить значения по умолчанию
                string srbAlphabet;
                if (!String.IsNullOrEmpty(alphabetFromMenu))
                    srbAlphabet = alphabetFromMenu;
                else
                    srbAlphabet = Setup.ReadFromSetup(Setup.SrbAlphabet);
                if (srbAlphabet != "cyr" && srbAlphabet != "lat")
                    srbAlphabet = "cyr";

                cyrRadioButton.Checked = srbAlphabet == "cyr";
                latRadioButton.Checked = srbAlphabet == "lat";

                string rusAccent = Setup.ReadFromSetup(Setup.RusAccent);
                if (rusAccent != "sign" && rusAccent != "color" && rusAccent != "none")
                    rusAccent = "color";

                accentSignRadioButton.Checked = rusAccent == "sign";
                accentColorRadioButton.Checked = rusAccent == "color";
                accentNoneRadioButton.Checked = rusAccent == "none";


                string listFont = Setup.ReadFromSetup(Setup.ListFont);
                string listFontSize = Setup.ReadFromSetup(Setup.ListFontSize);
                string listFontColor = Setup.ReadFromSetup(Setup.ListFontColor);

                if (String.IsNullOrEmpty(listFont))
                    listFont = Css.ListFontDef();

                if (String.IsNullOrEmpty(listFontSize))
                    listFontSize = Css.ListFontSizeDef();

                if (String.IsNullOrEmpty(listFontColor))
                    listFontColor = Css.ListFontColorDef();

                SetFontSizeColor(ref _listFontsComboBox, listFont, ref _listFontSizeComboBox, listFontSize,
                    ref _listFontColorButton, listFontColor);


                string oldWordsMax = Setup.ReadFromSetup(Setup.OldWordsMax);
                if (String.IsNullOrEmpty(oldWordsMax))
                    oldWordsMax = "20";
                _historyMaxNumericUpDown.Value = decimal.Parse(oldWordsMax);

                string oldWordsDelay = Setup.ReadFromSetup(Setup.OldWordsDelay);
                if (String.IsNullOrEmpty(oldWordsDelay))
                    oldWordsDelay = "5";
                _historyDelayNumericUpDown.Value = decimal.Parse(oldWordsDelay);

                //string oldWordsNoAuto = Setup.ReadFromSetup(Setup.OldWordsNoAuto);
                //if (String.IsNullOrEmpty(oldWordsNoAuto))
                //    oldWordsNoAuto = "0";
                //_noAutoHistoryCheckBox.Checked = oldWordsNoAuto == "1" ? true : false;
                //_noAutoHistoryCheckBox.Checked = false;

                string confirmClose = Setup.ReadFromSetup(Setup.ConfirmClose);
                if (String.IsNullOrEmpty(confirmClose))
                    confirmClose = "1";
                _confirmCloseCheckBox.Checked = confirmClose == "1";

                string loadRusWhileStart = Setup.ReadFromSetup(Setup.LoadRusWhileStart);
                if (String.IsNullOrEmpty(loadRusWhileStart))
                    loadRusWhileStart = "1";
                _loadRusWhileStartCheckBox.Checked = loadRusWhileStart == "1";
                //string opacitySyllable = Setup.ReadFromSetup(Setup.OpacitySyllable);
                //if (String.IsNullOrEmpty(opacitySyllable))
                //    opacitySyllable = "90";

                //_opacityTrackBar.Value = int.Parse(opacitySyllable);
                //GetOpacityText(); 
            }


            // CSS настройки --(оформление карточки)------------------------------
            color = ColorTranslator.FromHtml(Css.GetBodyBackColorDef());
            if (!isCSSdefault)
            {
                tmp = Setup.ReadFromSetup(Setup.CssBodyBackColor);
                if (!String.IsNullOrEmpty(tmp))
                {
                    color = ColorTranslator.FromHtml(tmp);
                }
            }
            _cssBodyBackColorButton.BackColor = color;

            SetFontSizeColor(isCSSdefault, ref _cssMainFontsComboBox, Setup.CssSrbWordFont, ref _cssMainFontSizeComboBox, Setup.CssSrbWordFontSize, 
                ref _cssMainFontColorButton, Setup.CssSrbWordFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssRusWordFontsComboBox, Setup.CssRusWordFont, ref _cssRusWordFontSizeComboBox, Setup.CssRusWordFontSize,
                ref _cssRusWordColorButton, Setup.CssRusWordFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssSrbMemoFontsComboBox, Setup.CssSrbMemoFont, ref _cssSrbMemoFontSizeComboBox, Setup.CssSrbMemoFontSize,
                ref _cssSrbMemoColorButton, Setup.CssSrbMemoFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssRusMemoFontsComboBox, Setup.CssRusMemoFont, ref _cssRusMemoFontSizeComboBox, Setup.CssRusMemoFontSize,
                ref _cssRusMemoColorButton, Setup.CssRusMemoFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssRusCommentFontsComboBox, Setup.CssRusCommentFont, ref _cssRusCommentFontSizeComboBox, Setup.CssRusCommentFontSize,
                ref _cssRusCommentColorButton, Setup.CssRusCommentFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssDigitsFontsComboBox, Setup.CssDigitsFont, ref _cssDigitsFontSizeComboBox, Setup.CssDigitsFontSize,
                ref _cssDigitsColorButton, Setup.CssDigitsFontColor);

            SetFontSizeColor(isCSSdefault, ref _cssRomanDigitsFontsComboBox, Setup.CssRomanDigitsFont, ref _cssRomanDigitsFontSizeComboBox, Setup.CssRomanDigitsFontSize,
                ref _cssRomanDigitsColorButton, Setup.CssRomanDigitsFontColor);

            color = ColorTranslator.FromHtml(Css.GetRusWordAccentColorDef()); 
            if (!isCSSdefault)
            {
                tmp = Setup.ReadFromSetup(Setup.CssRusWordAccentColor);
                if (!String.IsNullOrEmpty(tmp))
                {
                    color = ColorTranslator.FromHtml(tmp);
                }
            }
            _cssRusWordAccentColorButton.BackColor = color;

            color = ColorTranslator.FromHtml(Css.GetRusMemoAccentColorDef()); 
            if (!isCSSdefault)
            {
                tmp = Setup.ReadFromSetup(Setup.CssRusMemoAccentColor);
                if (!String.IsNullOrEmpty(tmp))
                {
                    color = ColorTranslator.FromHtml(tmp);
                }
            }
            _cssRusMemoAccentColorButton.BackColor = color;
            // CSS настройки --(оформление карточки)------------------------------
        }

        /// <summary>
        /// Подготовка контролов оформления списка.
        /// </summary>
        private void SetFontSizeColor(ref ComboBox _fontComboBox, string _font, ref ComboBox _sizeComboBox,
            string _size, ref Button _colorButton, string _color)
        {
            int index;

            index = _fontComboBox.FindString(_font);
            if (index > -1)
                _fontComboBox.SelectedIndex = index;
            else
                _fontComboBox.SelectedIndex = _fontComboBox.FindString("Arial");

            index = _sizeComboBox.FindString(_size);
            if (index > -1)
                _sizeComboBox.SelectedIndex = index;
            else
                _sizeComboBox.SelectedIndex = 1;

            Color color = Color.Black;
            color = ColorTranslator.FromHtml(_color);
            _colorButton.BackColor = color;
        }

        /// <summary>
        /// Подготовка контролов оформления карточки.
        /// isCSSdefault == true -> вернуть стандартные настройки оформления для карточки.
        /// </summary>
        private void SetFontSizeColor(bool isCSSdefault, ref ComboBox _fontComboBox, string _fontSetup, ref ComboBox _sizeComboBox, string _sizeSetup, 
            ref Button _colorButton, string _colorSetup)
        {
            int index;
            string _font = "", _fontSize = "", _fontColor = "";
            Color color;

            // Если брать настройки из Setup.xml
            if (!isCSSdefault)
            {
                _font = Setup.ReadFromSetup(_fontSetup);
                _fontSize = Setup.ReadFromSetup(_sizeSetup);
                _fontColor = Setup.ReadFromSetup(_colorSetup);
            }

            if (isCSSdefault || String.IsNullOrEmpty(_font))
            {
                switch (_fontSetup)
                {
                    case Setup.CssSrbWordFont:
                        _font = Css.GetSrbWordFontNameDef();
                        break;
                    case Setup.CssRusWordFont:
                        _font = Css.GetRusWordFontNameDef();
                        break;
                    case Setup.CssSrbMemoFont:
                        _font = Css.GetSrbMemoFontNameDef();
                        break;
                    case Setup.CssRusMemoFont:
                        _font = Css.GetRusMemoFontNameDef();
                        break;
                    case Setup.CssRusCommentFont:
                        _font = Css.GetRusCommentFontNameDef();
                        break;
                    case Setup.CssDigitsFont:
                        _font = Css.GetDigitsFontNameDef();
                        break;
                    case Setup.CssRomanDigitsFont:
                        _font = Css.GetRomanDigitsFontNameDef();
                        break;
                    default:
                        _font = "Arial";
                        break;
                }
            }

            if (isCSSdefault || String.IsNullOrEmpty(_fontSize))
            {
                switch (_sizeSetup)
                {
                    case Setup.CssSrbWordFontSize:
                        _fontSize = Css.GetSrbWordFontSizeDef();
                        break;
                    case Setup.CssRusWordFontSize:
                        _fontSize = Css.GetRusWordFontSizeDef();
                        break;
                    case Setup.CssSrbMemoFontSize:
                        _fontSize = Css.GetSrbMemoFontSizeDef();
                        break;
                    case Setup.CssRusMemoFontSize:
                        _fontSize = Css.GetRusMemoFontSizeDef();
                        break;
                    case Setup.CssRusCommentFontSize:
                        _fontSize = Css.GetRusCommentFontSizeDef();
                        break;
                    case Setup.CssDigitsFontSize:
                        _fontSize = Css.GetDigitsFontSizeDef();
                        break;
                    case Setup.CssRomanDigitsFontSize:
                        _fontSize = Css.GetRomanDigitsFontSizeDef();
                        break;
                    default:
                        _fontSize = "14";
                        break;
                }
            }

            if (isCSSdefault || String.IsNullOrEmpty(_fontColor))
            {
                switch (_colorSetup)
                {
                    case Setup.CssSrbWordFontColor:
                        _fontColor = Css.GetSrbWordColorDef();
                        break;
                    case Setup.CssRusWordFontColor:
                        _fontColor = Css.GetRusWordColorDef();
                        break;
                    case Setup.CssSrbMemoFontColor:
                        _fontColor = Css.GetSrbMemoColorDef();
                        break;
                    case Setup.CssRusMemoFontColor:
                        _fontColor = Css.GetRusMemoColorDef();
                        break;
                    case Setup.CssRusCommentFontColor:
                        _fontColor = Css.GetRusCommentColorDef();
                        break;
                    case Setup.CssDigitsFontColor:
                        _fontColor = Css.GetDigitsColorDef();
                        break;
                    case Setup.CssRomanDigitsFontColor:
                        _fontColor = Css.GetRomanDigitsColorDef();
                        break;
                    default:
                        _fontColor = "#000000";
                        break;
                }
            }

            // Если всё-таки настройка - пуста, то присвоить ей самые простые умолчания :)
            if (String.IsNullOrEmpty(_font))
                _fontComboBox.SelectedIndex = _fontComboBox.FindString("Arial");
            else
            {
                index = _fontComboBox.FindString(_font);
                if (index > -1)
                    _fontComboBox.SelectedIndex = index;
                else
                    _fontComboBox.SelectedIndex = _fontComboBox.FindString("Arial");
            }

            if (String.IsNullOrEmpty(_fontSize))
                _sizeComboBox.Text = "14";
            else
            {
                index = _sizeComboBox.FindString(_fontSize);
                if (index > -1)
                    _sizeComboBox.SelectedIndex = index;
                else
                    _sizeComboBox.Text = "14";
            }

            if (String.IsNullOrEmpty(_fontColor))
                color = ColorTranslator.FromHtml("#000000");
            else
            {
                color = ColorTranslator.FromHtml(_fontColor);
            }
            _colorButton.BackColor = color;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            SaveParameters();
            IsChanged = true;

            if (_applyChanges != null)
                _applyChanges();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveParameters();
            IsChanged = true;
            Close();
        }

        private void SaveParameters()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Setup.WriteToSetup(Setup.SrbAlphabet, cyrRadioButton.Checked ? "cyr" : "lat");
                Setup.WriteToSetup(Setup.RusAccent, accentSignRadioButton.Checked ? "sign" :
                    (accentColorRadioButton.Checked ? "color" : "none"));

                Setup.WriteToSetup(Setup.ListFont, _listFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.ListFontSize, _listFontSizeComboBox.SelectedItem.ToString());
                //Setup.WriteToSetup(Setup.ListFontColor, _listFontColorButton.BackColor.ToArgb().ToString());
                Setup.WriteToSetup(Setup.ListFontColor, ColorTranslator.ToHtml(_listFontColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssBodyBackColor, ColorTranslator.ToHtml(_cssBodyBackColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssSrbWordFont, _cssMainFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssSrbWordFontSize, _cssMainFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssSrbWordFontColor, ColorTranslator.ToHtml(_cssMainFontColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssRusWordFont, _cssRusWordFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusWordFontSize, _cssRusWordFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusWordFontColor, ColorTranslator.ToHtml(_cssRusWordColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssSrbMemoFont, _cssSrbMemoFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssSrbMemoFontSize, _cssSrbMemoFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssSrbMemoFontColor, ColorTranslator.ToHtml(_cssSrbMemoColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssRusMemoFont, _cssRusMemoFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusMemoFontSize, _cssRusMemoFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusMemoFontColor, ColorTranslator.ToHtml(_cssRusMemoColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssRusCommentFont, _cssRusCommentFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusCommentFontSize, _cssRusCommentFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRusCommentFontColor, ColorTranslator.ToHtml(_cssRusCommentColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssDigitsFont, _cssDigitsFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssDigitsFontSize, _cssDigitsFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssDigitsFontColor, ColorTranslator.ToHtml(_cssDigitsColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssRomanDigitsFont, _cssRomanDigitsFontsComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRomanDigitsFontSize, _cssRomanDigitsFontSizeComboBox.SelectedItem.ToString());
                Setup.WriteToSetup(Setup.CssRomanDigitsFontColor, ColorTranslator.ToHtml(_cssRomanDigitsColorButton.BackColor));

                Setup.WriteToSetup(Setup.CssRusWordAccentColor, ColorTranslator.ToHtml(_cssRusWordAccentColorButton.BackColor));
                Setup.WriteToSetup(Setup.CssRusMemoAccentColor, ColorTranslator.ToHtml(_cssRusMemoAccentColorButton.BackColor));

                //Setup.WriteToSetup(Setup.OpacitySyllable, _opacityTrackBar.Value.ToString());

                Setup.WriteToSetup(Setup.OldWordsMax, _historyMaxNumericUpDown.Value.ToString());
                Setup.WriteToSetup(Setup.OldWordsDelay, _historyDelayNumericUpDown.Value.ToString());
                //Setup.WriteToSetup(Setup.OldWordsNoAuto, _noAutoHistoryCheckBox.Checked ? "1" : "0");

                Setup.WriteToSetup(Setup.ConfirmClose, _confirmCloseCheckBox.Checked ? "1" : "0");
                Setup.WriteToSetup(Setup.LoadRusWhileStart , _loadRusWhileStartCheckBox.Checked ? "1" : "0");

                //if (isChangedCss)
                    Css.MakeCss();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SetFontsToComboBox()
        {
            FontFamily[] fontsArray = FontFamily.Families;
            for (int i = 0; i < fontsArray.Length; i++)
            {
                Font font = null;
                try
                {
                    font = new Font(fontsArray[i].Name, /*_listFontsComboBox.Font.Size*/ 16);
                }
                catch //(Exception ex)
                {
                    //MessageBox.Show(fontsArray[i].Name + Environment.NewLine + ex.Message);
                }

                if (font != null)
                {
                    _listFontsComboBox.Items.Add(font.Name);
                    _cssMainFontsComboBox.Items.Add(font.Name);
                    _cssRusWordFontsComboBox.Items.Add(font.Name);
                    _cssSrbMemoFontsComboBox.Items.Add(font.Name);
                    _cssRusMemoFontsComboBox.Items.Add(font.Name);
                    _cssRusCommentFontsComboBox.Items.Add(font.Name);
                    _cssDigitsFontsComboBox.Items.Add(font.Name);
                    _cssRomanDigitsFontsComboBox.Items.Add(font.Name);
                }
            }
            //MessageBox.Show("Всего шрифтов: " + _listFontsComboBox.Items.Count.ToString());
        }

        private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(dictMode))
            {
                NeedEndEditDict();

                e.Cancel = true;
                return;
            }

            // Сохранить номер текущей страницы
            Setup.WriteToSetup(Setup.CurrentTabPage, _tabControl.SelectedIndex.ToString());
            dictMode = "";
            dictEditId = -1;
        }

        private void NeedEndEditDict()
        {
            //1.
            /*
            _saveDictButton.BackColor = Color.LightPink;
            Wait(0.25);
            //_saveDictButton.BackColor = Color.Transparent;
            _cancelDictButton.BackColor = Color.LightPink;
            Wait(0.25);
            //_cancelDictButton.BackColor = Color.Transparent;
            */

            //2.
            _saveDictButton.BackColor = Color.Red; // LightPink;
            _cancelDictButton.BackColor = Color.Red;
            Wait(0.1);
            _saveDictButton.BackColor = Color.Transparent;
            _cancelDictButton.BackColor = Color.Transparent;
            Wait(0.1);
            _saveDictButton.BackColor = Color.Red;
            _cancelDictButton.BackColor = Color.Red;
            Wait(0.1);
            _saveDictButton.BackColor = Color.Transparent;
            _cancelDictButton.BackColor = Color.Transparent;
            Wait(0.1);
            _saveDictButton.BackColor = Color.Red;
            _cancelDictButton.BackColor = Color.Red;
            Wait(0.1);
            _saveDictButton.BackColor = Color.Transparent;
            _cancelDictButton.BackColor = Color.Transparent;

            MessageBox.Show("Необходимо закончить редактирование\nпользовательского словаря.", "Внимание!",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            _saveDictButton.BackColor = Color.Transparent;
            _cancelDictButton.BackColor = Color.Transparent;
        }

        private void Wait(double seconds)
        {
            int ticks = System.Environment.TickCount + (int)Math.Round(seconds * 1000.0);
            while (System.Environment.TickCount < ticks)
            {
                Application.DoEvents();
            }
        } 


        private void _tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tabControl.TabPages[_tabControl.SelectedIndex].Name == "_viewTabPage")
                _listFontsComboBox.Focus();
        }

        /// <summary>
        /// Установить текущую страницу.
        /// </summary>
        private void SetCurrentTabPage()
        {
            int index;
            if (this.CurrentPage == -1)
            {
                string tmp = Setup.ReadFromSetup(Setup.CurrentTabPage);
                if (!int.TryParse(tmp, out index))
                    index = 0;
            }
            else
            {
                index = this.CurrentPage;
            }

            _tabControl.SelectedIndex = index;
        }

        private void _cssBodyBackColorButton_Click(object sender, EventArgs e)
        {
            _colorDialog.Color = (sender as Button).BackColor;
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                //if ((sender as Button).BackColor != _colorDialog.Color)
                //    isChangedCss = true;

                (sender as Button).BackColor = _colorDialog.Color;   
            }
        }

        private void _cssDefaultButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вернуть стандартные настройки для оформления карточки?", 
                "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoadParameters(true);
            }
        }

        //private void _opacityTrackBar_Scroll(object sender, EventArgs e)
        //{
        //    GetOpacityText();
        //}

        //private void GetOpacityText()
        //{
        //    opacityGroupBox.Text = "Непрозрачность окна поиска по буквам : " + _opacityTrackBar.Value.ToString() + "%";
        //}

        private void _noAutoHistoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //_labelDelay1.Enabled = !_noAutoHistoryCheckBox.Checked;
            //_labelDelay2.Enabled = !_noAutoHistoryCheckBox.Checked;
            //_historyDelayNumericUpDown.Enabled = !_noAutoHistoryCheckBox.Checked;
        }

        private void LoadUserDicts()
        {
            _dictBindingSource.DataSource = null;
            _dictBindingSource.DataSource = Data.LoadTableUserDicts();
            _dictDataGridView.Columns[0].Visible = false;
        }

        private void ReloadUserDicts()
        {
#if DEMO || SQLITE
            Int64 id = -1;
#else
            int id = -1;
#endif
            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
#if DEMO || SQLITE
                id = (Int64)dr["ID"];
#else
                id = (int)dr["ID"];
#endif
            }
            
            LoadUserDicts();
            int pos = _dictBindingSource.Find("ID", id);
            if (pos != -1)
                _dictBindingSource.Position = pos;
        }

        private void _dictDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "BGR")
            {
                try
                {
                    Color color = ColorTranslator.FromHtml(e.Value.ToString());
                    e.CellStyle.BackColor = color;
                }
                catch //(Exception ex)
                {
                    //MessageBox.Show("Ошибка чтения цвета фона.\n" + ex.Message, "Ошибка!", 
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CellStyle.BackColor = Color.White;
                }
                
                e.Value = "";
            }
        }

        private void _addDictButton_Click(object sender, EventArgs e)
        {
            dictMode = "add";
            dictEditId = -1;
            _buttonPanel.Enabled = false;
            _editPanel.Visible = true;
            _dictDataGridView.Enabled = false;
            //_pictureBox.Image = (sender as Button).Image;
            _pictureBox.Image = _imageList.Images[0];
            _captionLabel.Text = (sender as Button).Text;

            okButton.Enabled = false;
            cancelButton.Enabled = false;
            applyButton.Enabled = false;

            _nameDictTextBox.Text = String.Empty;
            _backColorDictButton.BackColor = Color.White;
            _nameDictTextBox.Focus();
        }

        private void _renameDictButton_Click(object sender, EventArgs e)
        {
            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
                dictMode = "edit";
#if DEMO || SQLITE               
                dictEditId = (Int64)dr["ID"];
#else
                dictEditId = (int)dr["ID"];
#endif
                _buttonPanel.Enabled = false;
                _editPanel.Visible = true;
                _dictDataGridView.Enabled = false;
                //_pictureBox.Image = (sender as Button).Image;
                _pictureBox.Image = _imageList.Images[1];
                _captionLabel.Text = (sender as Button).Text;

                okButton.Enabled = false;
                cancelButton.Enabled = false;
                applyButton.Enabled = false;

                _nameDictTextBox.Text = dr["NAME"].ToString();
                string color = dr["BGR"].ToString();
                if (String.IsNullOrEmpty(color))
                    color = "White";
                try
                {
                    _backColorDictButton.BackColor = ColorTranslator.FromHtml(color);
                }
                catch
                {
                    _backColorDictButton.BackColor = Color.White;
                }
                _nameDictTextBox.Focus();
            }
        }

        private void _backColorDictButton_Click(object sender, EventArgs e)
        {
            _colorDialog.Color = (sender as Button).BackColor;
            _colorDialog.AnyColor = true;
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                //if ((sender as Button).BackColor != _colorDialog.Color)
                //    isChangedCss = true;

                (sender as Button).BackColor = _colorDialog.Color;
            }
        }

        private void SetupForm_Shown(object sender, EventArgs e)
        {
            _editPanel.Visible = false;
            _buttonPanel.Enabled = true;
            _dictDataGridView.Enabled = true;
        }

        private void _cancelDictButton_Click(object sender, EventArgs e)
        {
            _buttonPanel.Enabled = true;
            _dictDataGridView.Enabled = true;
            _editPanel.Visible = false;
            dictMode = "";
            dictEditId = -1;

            okButton.Enabled = true;
            cancelButton.Enabled = true;
            applyButton.Enabled = true;
        }

        private void _saveDictButton_Click(object sender, EventArgs e)
        {
            string name = _nameDictTextBox.Text.Trim();
            string bgr = ColorTranslator.ToHtml(_backColorDictButton.BackColor);

            // Проверки наименования словаря
            if (String.IsNullOrEmpty(name))
            {
                MessageBox.Show("Нужно указать наименование\nпользовательского словаря.", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _nameDictTextBox.Focus();
                return;
            }
            else
            {
                if (!Data.IsUniqueNameUserDict(dictEditId, name))
                {
                    MessageBox.Show("Такое наименование уже есть\n" +
                                    "у другого пользовательского словаря.\n" +
                                    "Укажите уникальное наименование.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _nameDictTextBox.Focus();
                    return;
                }
            }

            // Сохранение
            try
            {
                if (dictMode == "edit")
                {
                    Data.UpdateUserDict(dictEditId, name, bgr);
                }
                if (dictMode == "add")
                {
                    Data.InsertUserDict(name, bgr);
                }

                ReloadUserDicts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _buttonPanel.Enabled = true;
                _dictDataGridView.Enabled = true;
                _editPanel.Visible = false;
                dictMode = "";
                dictEditId = -1;

                okButton.Enabled = true;
                cancelButton.Enabled = true;
                applyButton.Enabled = true;
            }
        }

        private void _tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!String.IsNullOrEmpty(dictMode))
            {
                NeedEndEditDict();
                e.Cancel = true;
            }
        }

        private void _transferDictButton_Click(object sender, EventArgs e)
        {
#if DEMO || SQLITE
            Int64 id = -1;
#else
            int id = -1;
#endif
            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
#if DEMO || SQLITE
                id = (Int64)dr["ID"];
#else
                id = (int)dr["ID"];
#endif
            }

            if (id != -1)
            {
                DataTable dt = Data.LoadTableUserDicts();
                if (dt != null)
                {
                    _dictMenuStrip.Items.Clear();
                    
                    foreach (DataRow row in dt.Rows)
                    {
#if DEMO || SQLITE
                        if ((Int64)row["ID"] != id)
#else
                        if ((int)row["ID"] != id)
#endif
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem(row["NAME"].ToString());
#if DEMO || SQLITE
                            item.Tag = (Int64)row["ID"];
#else
                            item.Tag = (int)row["ID"];
#endif
                            item.Click += new EventHandler(dictItem_Click);
                            _dictMenuStrip.Items.Add(item);
                        }
                    }

                    if (_dictMenuStrip.Items.Count > 0)
                        _dictMenuStrip.Show(MousePosition);
                }
            }
        }

        void dictItem_Click(object sender, EventArgs e)
        {
            string text = "";

            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
                text += "Переместить все слова из словаря \"" + dr["NAME"].ToString() + "\"\n" +
                    "в словарь \"" + (sender as ToolStripMenuItem).Text + "\"?";
            }

            if (!String.IsNullOrEmpty(text))
            {
                if (MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                { 
                    // Переместить слова
                    try
                    {
#if DEMO || SQLITE
                        Int64 id = (Int64)dr["ID"];
                        Int64 idTo = (Int64)(sender as ToolStripMenuItem).Tag;
#else
                        int id = (int)dr["ID"];
                        int idTo = (int)(sender as ToolStripMenuItem).Tag;
#endif
                        Data.TransferWordsToOtherDict(id, idTo);

                        // Удалить историю
                        DeleteHistoryFile(id);

                        ReloadUserDicts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }
        }

        private void _clearDictButton_Click(object sender, EventArgs e)
        {
            string text = "";

            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
                text += "Удалить все слова из словаря \"" + dr["NAME"].ToString() + "\"?";
            }

            if (!String.IsNullOrEmpty(text))
            {
                if (MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    // Удалить слова
                    try
                    {
#if DEMO || SQLITE
                        Int64 id = (Int64)dr["ID"];
#else
                        int id = (int)dr["ID"];
#endif
                        Data.ClearUserDict(id);

                        // Удалить историю
                        DeleteHistoryFile(id);

                        ReloadUserDicts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void _delDictButton_Click(object sender, EventArgs e)
        {
            string text = "";

            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr != null)
            {
#if DEMO || SQLITE
                if ((Int64)dr["CNT"] > 0)
#else
                if ((int)dr["CNT"] > 0)
#endif
                {
                    MessageBox.Show("Словарь содержит слова.\nПеред удалением нужно сначала очистить его.",
                        "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                text += "Удалить словарь \"" + dr["NAME"].ToString() + "\"?";
            }

            if (!String.IsNullOrEmpty(text))
            {
                if (MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    // Удалить словарь
                    try
                    {
#if DEMO || SQLITE
                        Int64 id = (Int64)dr["ID"];
#else
                        int id = (int)dr["ID"];
#endif
                        Data.DeleteUserDict(id);
                        
                        // Удалить историю
                        DeleteHistoryFile(id);

                        ReloadUserDicts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private static void DeleteHistoryFile(Int64 id)
        {
            string saveFile = ScanWord.Utils.GetWorkDirectory() + Setup.HISTORY_FILE_NAME;
            string xmlFile = System.IO.Path.GetDirectoryName(saveFile) + "\\" +
              System.IO.Path.GetFileNameWithoutExtension(saveFile) + id.ToString() +
              System.IO.Path.GetExtension(saveFile);
            System.IO.File.Delete(xmlFile);
        }

        private void SetEnableButtons()
        {
            int cnt = _dictBindingSource.Count;
            DataRowView dr = (DataRowView)_dictBindingSource.Current;
            if (dr == null)
            {
                _renameDictButton.Enabled = cnt > 0;
                _transferDictButton.Enabled = cnt > 1;
                _clearDictButton.Enabled = cnt > 0;
                _delDictButton.Enabled = cnt > 0;
            }
            else
            {
                _renameDictButton.Enabled = cnt > 0;
#if DEMO || SQLITE
                _transferDictButton.Enabled = cnt > 1 && (Int64)dr["CNT"] > 0;
                _clearDictButton.Enabled = cnt > 0 && (Int64)dr["CNT"] > 0;
#else
                _transferDictButton.Enabled = cnt > 1 && (int)dr["CNT"] > 0;
                _clearDictButton.Enabled = cnt > 0 && (int)dr["CNT"] > 0;
#endif
                _delDictButton.Enabled = cnt > 0;
            }
        }

        private void _dictBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            SetEnableButtons();
        }

        private void _dictBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            SetEnableButtons();
        }
    }
}