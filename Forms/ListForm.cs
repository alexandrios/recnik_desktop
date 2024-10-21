using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using mshtml;
using SRWords.Articles;
using System.Diagnostics;
using System.Threading;

namespace SRWords
{
    public delegate void ApplyChangesDelegate();

    public partial class ListForm : Form
    {
        private bool isApplicationTerminated = true; // при использовании _notifyIcon присвоить false;

        //private PrivateFontCollection private_fonts = new PrivateFontCollection();
        private SyllableForm syllableForm;
        private SetupForm setupForm;
        private KeyboardForm keyboardForm;

        // В каком алфавите ("cyr" или "lat") находится основной словрь в dtSrb
        private string baseAlphabet;

        // Таблица, в которую загружается основной словарь
        private DataTable dtSrb;

        // Таблица, в которую загружается русский словарь
        private DataTable dtRus;

        // Признак, показывать ли статьи сербских переводов русского слова или показывать только переводы (в русско-сербском словаре)
        private bool multiRusValuesOpened = true; //false;

        // Признак того, что сейчас загружено: основной, пользовательский или русско-сербский словарь
        private string currTableName = "words"; //"dict" //"rus";
        public string CurrTableName
        {
            get { return currTableName; }
            set
            {
                currTableName = value;
                if (value == "rus")
                    _webBrowser.ContextMenuStrip = _wbRusContextMenuStrip;
                else
                    _webBrowser.ContextMenuStrip = _wbContextMenuStrip;
            }
        }

        // ДатаВью с соответствующей сортировкой в зависимости от алфавита для основного словаря
        DataView dataViewSrbLat = new DataView();
        DataView dataViewSrbCyr = new DataView();

        // ДатаВью с соответствующей сортировкой в зависимости от алфавита для пользовательского словаря
        DataView dataViewDictLat = new DataView();
        DataView dataViewDictCyr = new DataView();

        // ID пользовательского словаря (-1 = основной словарь)
        public int currDictId = -1;

        // Текущее слово, показываемое в браузере
        //public SRBWord currWord;
        public WordPack currWord;

        // История ранее просмотренных слов (своя для каждого словаря)
        public OldWords oldWords;

        public string Setup_SrbAlphabet = "";
        public string Setup_RusAccent = "";
        public string Setup_ListFont = "";
        public string Setup_ListFontSize = "";
        public string Setup_ListFontColor = "";
        //public bool Setup_MultiRusValuesOpened = true;

        public string OldWords_Max = "20";
        public string OldWords_Delay = "5";

        public string ConfirmClose = "1";
        public string LoadRusWhileStart = "1";

        // Сохраняет кол-во слов в польз. словаре перед загрузкой формы настроек
        public int tmpWordsCountUserDict;

        public static bool IsAdmin;
        public static bool IsDonated;

        public ListForm(bool isAdmin, bool isDonated)
        {
            IsAdmin = isAdmin;
            IsDonated = isDonated;

            SplashForm3.ShowSplashScreen();
            SplashForm3.SetStatus("Загрузка настроек ...");
            
            InitializeComponent();

            if (IsDonated)
            {
                _donatePanel.Visible = false;
                _webBrowser.Dock = DockStyle.Fill;
            }

#if DEMO
            _demoLabel.Visible = true;
            _demoLinkLabel.Visible = true;
#endif

            // http://www.csharpcoderr.com/2013/10/windows-form-font.html
            //InitializeFonts();

            //_notifyIcon.Visible = false;

            RestoreFormProperties();

            // Отключить контекстное меню браузера 
            _webBrowser.IsWebBrowserContextMenuEnabled = false;

            //_webBrowser.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
            //_webBrowser.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            // Отключить горячие клавиши браузера 
            _webBrowser.WebBrowserShortcutsEnabled = false;
            LoadParameters(true);

            SetKeyboardInputLanguageForSRB(Setup_SrbAlphabet);

            // Создание формы перенесено в процедуру: ShowSyllableForm()
            //syllableForm = new SyllableForm(this, Setup_SrbAlphabet);
            //SetOpacitySyllable();

            // Создание формы перенесено в процедуру: ShowKeyboardForm()
            //keyboardForm = new KeyboardForm(this, GetCurrAlpabet, GetKeyboardLetter);
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            SplashForm3.SetStatus("Проверка изменений на сервере ...");

            // Обращение за списком изменений к серверу - в отдельном потоке
            Thread remoteProcess = new Thread(ChangesFromServer.GetChangesFromServer);
            remoteProcess.Start();

            // Дождаться окончания загрузки изменений с сервера
            remoteProcess.Join();

            if (LoadRusWhileStart == "1")
            {
                // Загрузить русско-сербский словарь
                SplashForm3.SetStatus("Загрузка русского словаря ...");

                dtRus = Data.LoadRusDict();
                _rusBindingSource.DataSource = dtRus;
                _rusBindingSource.Sort = "NAME";
                _rusDataGridView.Visible = true;
                _rusDataGridView.Visible = false;
            }

            SplashForm3.SetStatus("Загрузка сербского словаря ...");

            // Загрузить сербский словарь
            LoadData();

            // Подготовить окно настроек
            SplashForm3.SetStatus("Восстановление параметров ...");

            setupForm = new SetupForm(ApplyChangedParams);

            Thread.Sleep(500);

            SplashForm3.SetStatus("Подготовка к запуску...");
            Thread.Sleep(500);

            // Закрыть окно приветствия
            SplashForm3.CloseForm();
        }

        private void ListForm_Shown(object sender, EventArgs e)
        {
            _searchTextBox.Focus();
            CurrentDGV().DefaultCellStyle.SelectionBackColor = SystemColors.InactiveCaption;

            // Восстановить текущее слово
            string tmp = Setup.ReadFromSetup("LastWord");
            if (!String.IsNullOrEmpty(tmp))
                SearchWord(tmp);

            // Восстановить активный пользовательский словарь
            tmp = Setup.ReadFromSetup("CurrDictId");
            if (!String.IsNullOrEmpty(tmp))
            {
                if (int.TryParse(tmp, out int d))
                {
                    Data.LoadListUserDicts(out List<int> sId);
                    if (sId.Contains(d))
                        currDictId = d;
                }
            }

            this.Activate();
            //_notifyIcon.Visible = true;
            ShowCurrentWord();
        }

        /*
        private void SetOpacitySyllable()
        {
            if (syllableForm != null)
            {
                string opacitySyllable = Setup.ReadFromSetup(Setup.OpacitySyllable);
                if (String.IsNullOrEmpty(opacitySyllable))
                    opacitySyllable = "90";
                syllableForm.Opacity = double.Parse(opacitySyllable) / 100;
            }
        }
        */

        /// <summary>
        /// Изменение языка ввода для сербского словаря.
        /// </summary>
        /// <param name="alphabet"></param>
        public void SetKeyboardInputLanguageForSRB(string alphabet)
        {
            if (alphabet == "cyr")
            {
                cyrToolStripMenuItem.Checked = true;
                latToolStripMenuItem.Checked = false;
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
            }
            else
            {
                cyrToolStripMenuItem.Checked = false;
                latToolStripMenuItem.Checked = true;
                //InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("sr-Latn-CS"));
            }
            /*
            sr          sr-Latn-CS  Serbian
            sr-Cyrl-BA  sr-Cyrl-BA  Serbian (Cyrillic) (Bosnia and Herzegovina)
            sr-Cyrl-CS  sr-Cyrl-CS  Serbian (Cyrillic, Serbia)
            sr-Latn-BA  sr-Latn-BA  Serbian (Latin) (Bosnia and Herzegovina)
            sr-Latn-CS  sr-Latn-CS  Serbian (Latin, Serbia)
            */
        }

        public void LoadParameters(bool startMode)
        {
            string old_SrbAlphabet = Setup_SrbAlphabet;
            string old_RusAccent = Setup_RusAccent;
            string old_ListFont = Setup_ListFont;
            string old_ListFontSize = Setup_ListFontSize;
            string old_ListFontColor = Setup_ListFontColor;

            try
            {
                Cursor = Cursors.WaitCursor;
                string tmp;

                tmp = Setup.ReadFromSetup(Setup.OldWordsMax);
                if (!String.IsNullOrEmpty(tmp))
                    OldWords_Max = tmp;

                tmp = Setup.ReadFromSetup(Setup.OldWordsDelay);
                if (!String.IsNullOrEmpty(tmp))
                    OldWords_Delay = tmp;

                tmp = Setup.ReadFromSetup(Setup.ConfirmClose);
                if (!String.IsNullOrEmpty(tmp))
                    ConfirmClose = tmp;

                tmp = Setup.ReadFromSetup(Setup.LoadRusWhileStart);
                if (!String.IsNullOrEmpty(tmp))
                    LoadRusWhileStart = tmp;

                // Применить изменения к объекту
                if (oldWords != null)
                {
                    oldWords.SetProperties(OldWords_Max, OldWords_Delay);
                }

                Setup_SrbAlphabet = Setup.ReadFromSetup(Setup.SrbAlphabet);
                Setup_RusAccent = Setup.ReadFromSetup(Setup.RusAccent);

                if (String.IsNullOrEmpty(Setup_SrbAlphabet)) Setup_SrbAlphabet = Css.SrbAlphabetDef();
                if (String.IsNullOrEmpty(Setup_RusAccent)) Setup_RusAccent = Css.RusAccentDef();

                Setup_ListFont = Setup.ReadFromSetup(Setup.ListFont);
                Setup_ListFontSize = Setup.ReadFromSetup(Setup.ListFontSize);
                Setup_ListFontColor = Setup.ReadFromSetup(Setup.ListFontColor);

                // Если Setup.xml пуст, то определить значения по умолчанию
                if (String.IsNullOrEmpty(Setup_ListFont)) Setup_ListFont = Css.ListFontDef();
                if (String.IsNullOrEmpty(Setup_ListFontSize)) Setup_ListFontSize = Css.ListFontSizeDef();
                if (String.IsNullOrEmpty(Setup_ListFontColor)) Setup_ListFontColor = Css.ListFontColorDef();

                if (startMode)
                {
                    Css.MakeCss();

                    CultureInfo newCInfo = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                    newCInfo.NumberFormat.NumberDecimalSeparator = ".";
                    System.Threading.Thread.CurrentThread.CurrentCulture = newCInfo;

                    Font font = new Font(Setup_ListFont, float.Parse(Css.PxToPt(Setup_ListFontSize), NumberStyles.AllowDecimalPoint));
                    _wordsDataGridView.Font = font;
                    _rusDataGridView.Font = font;
                    _dictDataGridView.Font = font;
                    _searchTextBox.Font = font;
                    _oldContextMenuStrip.Font = font;

                    _wordsDataGridView.RowTemplate.Height = (int)font.GetHeight() + 4;
                }       
                else
                {
                    if (old_SrbAlphabet != Setup_SrbAlphabet)
                    {
                        Change_Alphabet();
                    }

                    if (old_ListFont != Setup_ListFont || old_ListFontSize != Setup_ListFontSize)
                    {
                        CultureInfo newCInfo = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                        newCInfo.NumberFormat.NumberDecimalSeparator = ".";
                        System.Threading.Thread.CurrentThread.CurrentCulture = newCInfo;

                        Font font = new Font(Setup_ListFont, float.Parse(Css.PxToPt(Setup_ListFontSize), NumberStyles.AllowDecimalPoint));
                        _wordsDataGridView.Font = font;
                        _rusDataGridView.Font = font;
                        _dictDataGridView.Font = font;
                        _searchTextBox.Font = font;
                        _oldContextMenuStrip.Font = font;


                        /* Изменить высоту ячейки грида!
                        float int_old_ListFontSize = float.Parse(old_ListFontSize);
                        float int_new_ListFontSize = float.Parse(Setup_ListFontSize);
                        _wordsDataGridView.RowTemplate.Height = (int)(22f / 16f * int_new_ListFontSize);

                        //_wordsDataGridView.RowTemplate.Height = (int)(_wordsDataGridView.RowTemplate.Height / int_old_ListFontSize * int_new_ListFontSize);
                        //_wordsDataGridView.RowTemplate.Height = (int)(float.Parse(Setup_ListFontSize) * 1.5F);
                        //_wordsDataGridView.RowTemplate.Height = (int)(float.Parse(Css.PxToPt(Setup_ListFontSize), NumberStyles.AllowDecimalPoint));
                        _demoLabel.Text = _wordsDataGridView.RowTemplate.Height.ToString();
                        _demoLabel.Visible = true;
                        */
                        _wordsDataGridView.RowTemplate.Height = (int)font.GetHeight() + 4;
                    }
                    if (old_ListFontColor != Setup_ListFontColor)
                    {
                        //_wordsDataGridView.ForeColor = Color.FromArgb(int.Parse(Setup_ListFontColor));
                        _wordsDataGridView.ForeColor = ColorTranslator.FromHtml(Setup_ListFontColor);
                        _rusDataGridView.ForeColor = ColorTranslator.FromHtml(Setup_ListFontColor);
                        _dictDataGridView.ForeColor = ColorTranslator.FromHtml(Setup_ListFontColor);
                    }

                    //SetOpacitySyllable();

                    // Применить настройки пользовательских словарей
                    ApplyDictParameters();

                    ///*
                    // Изменить высоту ячейки грида!
                    if (old_ListFontSize != Setup_ListFontSize)
                    {
                        object ds = _wordsDataGridView.DataSource;
                        _wordsDataGridView.DataSource = null;
                        _wordsDataGridView.DataSource = ds;

                    }
                    else
                    {
                    //*/
                        // Перейти к текущему слову
                        ShowCurrentWord();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Применить настройки пользовательских словарей.
        /// </summary>
        private void ApplyDictParameters()
        {
            // Проверить, существует ли ещё текущий пользовательский словарь
            string dictName = Data.GetNameForDict(currDictId);
            if (!String.IsNullOrEmpty(dictName))
            {
                // Проверить ситуацию: в словаре не стало слов, или наоборот
                if (tmpWordsCountUserDict != Data.GetWordsCountUserDict(currDictId))
                {
                    if (currTableName == "dict")
                        LoadData("dict", currDictId);
                }
                else
                {
                    if (currTableName == "dict")
                    {
                        // Фон пользовательского словаря
                        Color color = ColorTranslator.FromHtml(Data.GetBGRForDict(currDictId));
                        _dictDataGridView.BackgroundColor = color;
                        _dictDataGridView.DefaultCellStyle.BackColor = color;

                        this.Text = "[ " + dictName + " ]";
#if DEMO
                        this.Text += " (Демо-версия)";
#endif
                    }
                }
            }
            else
            {
                // Такой пользовательский словарь больше не существует - загрузить основной
                currDictId = -1;
                if (currTableName == "dict")
                {
                    // Отменить сохранение истории словаря, раз он был удалён
                    oldWords = null;
                    // Загрузить основной словарь
                    baseItem_Click(null, null);
                }
            }
        }

        /// <summary>
        /// Смена сербского алфавита: кириллица - латиница.
        /// </summary>
        private void Change_Alphabet()
        {
            if (currTableName == "words" || currTableName == "dict")
            {
                SetSearchTextBox(_searchTextBox.Text);

                // Загрузить словарь
                LoadData(currTableName, currDictId, false);

                int pos;
                //if (Setup_SrbAlphabet == "cyr")
                //    pos = CurrentBS().Find("NAME_CYR", Utils.LatToCyr(currWord.Name));
                //else
                    pos = CurrentBS().Find("NAME", currWord.Name);

                if (pos > -1)
                    CurrentBS().Position = pos;

                if (currTableName == "words" || currTableName == "dict")
                {
                    // Позиционировать слово в середину грида
                    SetCenterFoundRow();

                    // Изменить язык ввода 
                    SetKeyboardInputLanguageForSRB(Setup_SrbAlphabet);

                    // Изменить окно поиска по буквам
                    ReloadSyllableForm();
                }

                // Окно виртуальной клавиатуры изменится само, по таймеру
            }
            //else
            //{
                ShowCurrentWord();
            //}
        }

        /// <summary>
        /// Изменить окно поиска по буквам согласно новым настройкам.
        /// </summary>
        private void ReloadSyllableForm()
        {
            if (syllableForm != null)
            {
                bool isSyllableShown = false;
                if (_searchByLetterToolStripButton.Checked) // form is shown
                {
                    isSyllableShown = true;
                    HideSyllableForm();
                }
                int _top = syllableForm.Top;
                int _left = syllableForm.Left;
                syllableForm.Dispose();
                syllableForm = new SyllableForm(this, Setup_SrbAlphabet);
                syllableForm.StartPosition = FormStartPosition.Manual;
                syllableForm.Top = _top;
                syllableForm.Left = _left;
                if (isSyllableShown)
                    ShowSyllableForm();
            }
        }

        /// <summary>
        /// Загрузка данных в грид.
        /// </summary>
        private void LoadData()
        {
            LoadData(this.currTableName, this.currDictId, true);
        }
        private void LoadData(string tableName, int dictId)
        {
            LoadData(tableName, dictId, true);
        }
        private void LoadData(string tableName, int dictId, bool doAfter)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (tableName == "rus")
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    if (dtRus == null)
                    {
                        dtRus = Data.LoadRusDict();
                        _rusBindingSource.DataSource = dtRus;
                        _rusBindingSource.Sort = "NAME";
                    }

                    // Белый фон грида
                    _rusDataGridView.BackgroundColor = Color.White;
                    _rusDataGridView.DefaultCellStyle.BackColor = Color.White;

                    _rusDataGridView.Visible = true;
                    _wordsDataGridView.Visible = false;
                    _dictDataGridView.Visible = false;

                    this.Text = "Русско-сербский словарь";
#if DEMO
                    this.Text += " (Демо-версия)";
#endif
                } 
                else if (tableName == "words")
                {
                    if (dtSrb == null)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("sr-Latn-CS");
                        dtSrb = Data.LoadAllDictCL(tableName);

                        dataViewSrbCyr.Table = dtSrb;
                        dataViewSrbCyr.Sort = "NAME_CYR";
                        dataViewSrbLat.Table = dtSrb;
                        dataViewSrbLat.Sort = "NAME";
                    }

                    // Белый фон грида основного словаря
                    _wordsDataGridView.BackgroundColor = Color.White;
                    _wordsDataGridView.DefaultCellStyle.BackColor = Color.White;

                    _wordsDataGridView.Visible = true;
                    _dictDataGridView.Visible = false;
                    _rusDataGridView.Visible = false;

                    this.Text = "Сербско-русский словарь";
#if DEMO
                    this.Text += " (Демо-версия)";
#endif
                }
                
                else if (tableName == "dict")
                {
                    // Пользовательские словари всегда загружать из БД при их вызове
                    DataTable dtDict;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("sr-Latn-CS");
                    dtDict = Data.LoadUserDictCL(dictId);
                    
                    dataViewDictCyr.Table = dtDict;
                    dataViewDictCyr.Sort = "NAME_CYR";
                    dataViewDictLat.Table = dtDict;
                    dataViewDictLat.Sort = "NAME";

                    // Фон пользовательского словаря
                    Color color = ColorTranslator.FromHtml(Data.GetBGRForDict(dictId));
                    _dictDataGridView.BackgroundColor = color;
                    _dictDataGridView.DefaultCellStyle.BackColor = color;

                    this.Text = "[ " + Data.GetNameForDict(currDictId) + " ]";
#if DEMO
                    this.Text += " (Демо-версия)";
#endif
                    _wordsDataGridView.Visible = false;
                    _rusDataGridView.Visible = false;
                    _dictDataGridView.Visible = true;
                }

                // Установка алфавита для сербского языка cyr - lat
                baseAlphabet = Setup_SrbAlphabet;

                // Сортировка
                if (tableName == "words" || tableName == "dict")
                {
                    _wordsDataGridView.CellFormatting -= _wordsDataGridView_CellFormatting;
                    _dictDataGridView.CellFormatting -= _wordsDataGridView_CellFormatting;
                    if (Setup_SrbAlphabet == "cyr")
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sr-Cyrl-CS");
                        CurrentBS().DataSource = (tableName == "words") ? dataViewSrbCyr : dataViewDictCyr;
                        CurrentBS().Sort = "NAME_CYR";
                    }
                    else
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sr-Latn-CS");
                        CurrentBS().DataSource = (tableName == "words" ) ? dataViewSrbLat : dataViewDictLat;
                        CurrentBS().Sort = "NAME";
                    }
                    _wordsDataGridView.CellFormatting += _wordsDataGridView_CellFormatting;
                    _dictDataGridView.CellFormatting += _wordsDataGridView_CellFormatting;
                }

                // Сохранить историю для предыдущего словаря 
                if (oldWords != null)
                    oldWords.Serialize();
                
                if (tableName == "rus")
                {
                    // Изменить язык ввода для русского языка
                    InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
                }

                // Загрузить историю для текущего словаря
                oldWords = new OldWords((currTableName == "words" ? -1 : currTableName == "rus" ? -2 :currDictId), OldWords_Max, OldWords_Delay); 

                // Прятать/показывать некоторые пункты меню в зависимости от словаря
                SetEnableMenuItems(tableName);

                if (doAfter)
                {
                    LoadData_DoAfter();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Действия после загрузки словаря. Одинаковы для основного и пользовательских словарей.
        /// </summary>
        private void LoadData_DoAfter()
        {
            if (currTableName != "rus")
            {
                string wName = Setup_SrbAlphabet == "lat" ? Utils.CyrToLat(oldWords.Get()) : oldWords.Get();
                int pos = CurrentBS().Find("NAME", wName);
                if (pos > -1)
                    CurrentBS().Position = pos;
            }
            else
            {
                string wName = oldWords.Get();
                int pos = CurrentBS().Find("NAME", wName);
                if (pos > -1)
                    CurrentBS().Position = pos;
            }

            // Перейти к текущему слову
            ShowCurrentWord();

            // Позиционировать слово в середину грида
            SetCenterFoundRow();
        }

        /// <summary>
        /// Прятать/показывать некоторые пункты меню в зависимости от словаря.
        /// </summary>
        /// <param name="tableName"></param>
        private void SetEnableMenuItems(string tableName)
        {
            if (tableName == "words" || tableName == "dict")
            {
                //foreach (ToolStripItem item in findToolStripMenuItem.DropDownItems)
                //    item.Enabled = true;

                _backToolStripSplitButton.Enabled = true;
                _searchByLetterToolStripButton.Enabled = true;

                if (tableName == "words")
                {
                    mainFindByLetterToolStripMenuItem.Visible = true;
                    toolStripSeparator3.Visible = true;
                    mainSaveToUserDictToolStripMenuItem.Visible = true;
                    gridSaveToUserDictToolStripMenuItem.Visible = true;
                    wbSaveToUserDictToolStripMenuItem.Visible = true;

                    mainDelFromUserDictToolStripMenuItem.Visible = false;
                    gridDelFromUserDictToolStripMenuItem.Visible = false;
                    wbDelFromUserDictToolStripMenuItem.Visible = false;
                }
                else if (tableName == "dict")
                {
                    mainSaveToUserDictToolStripMenuItem.Visible = false;
                    gridSaveToUserDictToolStripMenuItem.Visible = false;
                    wbSaveToUserDictToolStripMenuItem.Visible = false;

                    mainDelFromUserDictToolStripMenuItem.Visible = true;
                    gridDelFromUserDictToolStripMenuItem.Visible = true;
                    wbDelFromUserDictToolStripMenuItem.Visible = true;
                }
            }
            else if (tableName == "rus")
            {
                //foreach (ToolStripItem item in findToolStripMenuItem.DropDownItems)
                //    item.Enabled = false;

                mainSaveToUserDictToolStripMenuItem.Visible = false;
                mainFindByLetterToolStripMenuItem.Visible = false;
                toolStripSeparator3.Visible = false;
            }
        }

        /// <summary>
        /// Переход на другую строку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _srbBindingSource_PositionChanged(object sender, EventArgs e)
        {
            ShowCurrentWord();
        }

        public void ShowCurrentWord()
        {
            DataRowView dr = (DataRowView)CurrentBS().Current;

            if (dr != null)
            {
                currWord = new WordPack(dr, currTableName == "rus" ? Language.RUSSIAN : Language.SERBIAN,
                    dataViewSrbLat, Setup_SrbAlphabet, Setup_RusAccent);

                // для организации hide/open all srb key words
                if (currTableName == "rus")
                {
                    string srbname = dr["SRBNAME"].ToString();
                    string[] aSrb = srbname.Split(new char[] { ';' });
                    List<String> aList = new List<string>();
                    for (int i = 0; i < aSrb.Length; i++)
                    {
                        string s = aSrb[i];
                        if (aList.IndexOf(s) == -1)
                            aList.Add(s);
                    }
                    aList.Sort();
                    wbShowAllToolStripMenuItem.Tag = aList;
                }

                if (oldWords != null)
                    oldWords.SetWait(currWord.Name);
            }
            else
            {
                currWord = new WordPack();
            }

            _webBrowser.DocumentText = currWord.Html;

            // Синхронизировать строку поиска
            if (!isSearchTextBoxEditing)
            {
                if (currWord.Name != null)
                {
                    _searchTextBox.TextChanged -= _searchTextBox_TextChanged;
                    SetSearchTextBox(currWord.Name);
                    _searchTextBox.ForeColor = SystemColors.WindowText;
                    _searchTextBox.TextChanged += _searchTextBox_TextChanged;
                }
            }
        }

        /*
        public void ShowCurrentWord_old()
        {
            DataRowView dr = (DataRowView)CurrentBS().Current;

            if (currTableName == "rus")
            {
                ShowCurrentRusWord(dr);
                return;
            }

            if (dr != null)
            {
                currWord = new SRBWord(dr);
                if (oldWords != null && !oldWords.IsRus())
                    oldWords.SetWait(currWord.Name);
            }
            else
            {
                currWord = new SRBWord();
            }

            // Смена на алфавитной панели
            //if (!String.IsNullOrEmpty(dr["NAME"].ToString()))
            //{
            //String letter = dr["NAME"].ToString().Substring(0, 1);
            //alphaPanel.ChangeCurrentItem(letter.ToUpper());
            //}

            String t = "";

            // Вариант 1: взять уже готовый HTML из БД
            // Недостаток: нельзя динамически учитывать настройки оформления
            //if (dr["SCRIPT"] != null)
            //{
            //    #if DEMO
            //    byte[] bytesScript = (byte[])dr["SCRIPT"];
            //    Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
            //    t = dstEncodingFormat.GetString(bytesScript);
            //    #else
            //    t = dr["SCRIPT"].ToString();
            //    #endif
            //}
            

            // Вариант 2: десериализовать статью из БД, а затем сформировать HTML
            if (dr != null && dr["XML"] != DBNull.Value)
            {
                // Считать сериализованный XML-объект Article
//#if DEMO
                // так как поле XML имеет тип BLOB
//                byte[] bytesXml = (byte[])dr["XML"];
//                Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
//                string xmlString = dstEncodingFormat.GetString(bytesXml);
//#else
                string xmlString = dr["XML"].ToString();
                //#endif

                // Получить объект Article
                //ScanWord.Article a = new ScanWord.Article();
                
                ArticleInfo a = new ArticleInfo();

                a = a.Deserialize(xmlString);

                // Сформировать HTML-скрипт: true=создавать ссылки
                //t = a.CreateScript(true, Setup_SrbAlphabet, Setup_RusAccent);
                // Не создавать ссылки в пользовательских словарях
                t = a.CreateScript((currTableName == "words"), Setup_SrbAlphabet, Setup_RusAccent);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(t);
            ShowWord(bytes);

            // Синхронизировать строку поиска
            if (!isSearchTextBoxEditing)
            {
                if (currWord.Name != null)
                {
                    _searchTextBox.TextChanged -= _searchTextBox_TextChanged;
                    //_searchTextBox.Text = (Setup_SrbAlphabet == "lat" ? Utils.CyrToLat(currWord.Name) : Utils.LatToCyr(currWord.Name));
                    //SetSearchTextBox(Setup_SrbAlphabet == "lat" ? Utils.CyrToLat(currWord.Name) : Utils.LatToCyr(currWord.Name));
                    SetSearchTextBox(currWord.Name);
                    _searchTextBox.ForeColor = SystemColors.WindowText;
                    _searchTextBox.TextChanged += _searchTextBox_TextChanged;
                }
            }
        }
        */

        /*
        private void ShowWord(byte[] bytes)
        {
            String finalString = String.Empty;

            //Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
            if (bytes != null)
                finalString = dstEncodingFormat.GetString(bytes);

            if (!File.Exists(Css.cssFile))
            {
                Css.MakeCss();
            }

            finalString = ScanWord.Utils.HTMLStartString() + finalString + ScanWord.Utils.HTMLEndString();

            //MessageBox.Show(finalString);

            // Древний способ передачи через сохранение файла
            //string tempFile = Environment.CurrentDirectory + "\\temp.html";
            //File.WriteAllText(tempFile, finalString); 
            //_webBrowser.Navigate(tempFile);

            _webBrowser.DocumentText = finalString;
        }
        */

        /// <summary>
        /// Поиск по началу строки.
        /// </summary>
        /// <param name="text"></param>
        public bool SearchWord(string searchString)
        {
            // По умолчанию - поиск точный (без игнорирования больших и маленьких букв)
            return SearchWord(searchString, false);
        }
        public bool SearchWord(string searchString, bool ignoreCase)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (currTableName != "rus")
                {
                    searchString = Utils.CyrToLat(searchString); 
                }

                foreach (DataRowView dr in CurrentBS().List) 
                {
                    bool isFound = false;
                    if (ignoreCase)
                        isFound = dr["NAME"].ToString().ToUpper().StartsWith(searchString.ToUpper());
                    else
                        isFound = dr["NAME"].ToString().StartsWith(searchString);

                    // Закомментировано 24.11.2016
                    //if (dr["NAME"].ToString().ToUpper().StartsWith(searchString.ToUpper()))
                    // не находит слово дженова из F7 (так как в словаре оно начинается с большой буквы!)
                    //if (dr["NAME"].ToString().StartsWith(searchString))  
                    if (isFound)
                    {
                        // Ситуация, когда, например из слова "друштво" пытаемся перейти по ссылке на слово "Друштво"
                        // и попадаем на то же слово "друштво". В этом случае считать результатом поиска false
                        //if (CurrentBS().Position != CurrentBS().List.IndexOf(dr))
                        //{
                        CurrentBS().Position = CurrentBS().List.IndexOf(dr);

                        // Позиционирование найденного слова в середину грида
                        SetCenterFoundRow();

                        result = true;
                        break;
                        //}
                    }
                }
            }

            return result;
        }

        public bool SearchEqual(string searchString)
        {
            /*
            bool result = false;

            if (CurrentBS().Find("NAME", searchString) > -1)
                result = true;

            return result;
            */
            return CurrentBS().Find("NAME", searchString) > -1;
        }

        /// <summary>
        /// Переход по ссылке.
        /// </summary>
        /// <param name="refName"></param>
        public bool GoToReference(string refName)
        {
            oldWords.Set(currWord.Name);
            bool isFound = SearchWord(refName);
            return isFound;
        }

        /// <summary>
        /// Нажатие клавиш.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                if (!_searchTextBox.Focused)
                    _searchTextBox.Focus();
                else
                    CurrentDGV().Focus();
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                if (!_searchTextBox.Focused)
                {
                    _searchTextBox.Focus();
                    // Закомментировал 24.11.2016
                    //_searchTextBox.SelectionStart = _searchTextBox.Text.Length;
                    SendKeys.Send(e.KeyChar.ToString());
                    e.Handled = true;
                }
            }

            if ((e.KeyChar >= 'А' && e.KeyChar <= 'я') || (e.KeyChar == 'ё') || (e.KeyChar == 'Ё')
                || (e.KeyChar == ' ') || (e.KeyChar == '-')
                || (e.KeyChar == 'љ') || (e.KeyChar == 'Љ') || (e.KeyChar == 'њ') || (e.KeyChar == 'Њ')
                || (e.KeyChar == 'ђ') || (e.KeyChar == 'Ђ') || (e.KeyChar == 'ћ') || (e.KeyChar == 'Ћ')
                || (e.KeyChar == 'џ') || (e.KeyChar == 'Џ') || (e.KeyChar == '\u0458') || (e.KeyChar == '\u0408')
                || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z')
                || (e.KeyChar == 'đ') || (e.KeyChar == 'ž') || (e.KeyChar == 'ć') || (e.KeyChar == 'č')
                || (e.KeyChar == 'š') || (e.KeyChar == 'Đ') || (e.KeyChar == 'Ž') || (e.KeyChar == 'Ć')
                || (e.KeyChar == 'Č') || (e.KeyChar == 'Š')
               )
            {
                if (!_searchTextBox.Focused)
                {
                    _searchTextBox.Focus();
                    // Закомментировал 24.11.2016
                    //_searchTextBox.SelectionStart = _searchTextBox.Text.Length;
                    SendKeys.Send(e.KeyChar.ToString());

                    e.Handled = true;
                }
            }
        }

        private void ListForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F1)
            //    referenceToolStripMenuItem.PerformClick();

            if (e.KeyCode == Keys.Insert)
                //if (currTableName == "words")
                    mainSaveToUserDictToolStripMenuItem.PerformClick();

            if (e.KeyCode == Keys.Delete && e.Control)
                if (currTableName == "dict")
                    mainDelFromUserDictToolStripMenuItem.PerformClick();

            if (e.KeyCode == Keys.F7)
                if (currTableName == "words")
                    _searchByLetterToolStripButton.PerformClick();

            if (e.KeyCode == Keys.F5)
                keyBoardToolStripMenuItem.PerformClick();

            if (e.KeyCode == Keys.F9)
                _setupToolStripButton.PerformClick();

            if (e.KeyCode == Keys.B && e.Control)
                _backToolStripSplitButton.PerformButtonClick();

            if (e.KeyCode == Keys.Escape)
                if (syllableForm != null && syllableForm.Visible)
                {
                    HideSyllableForm();
                    _searchTextBox.Focus();
                }

            if (e.KeyCode == Keys.S && e.Control)
            {
                // Загрузить основной словарь
                baseItem_Click(null, null);
                //e.Handled = true;
                CurrentDGV().Focus();
            }

            if (e.KeyCode == Keys.R && e.Control)
            {
                // Загрузить русско-сербский словарь
                rusItem_Click(null, null);
                //e.Handled = true;
                CurrentDGV().Focus();
            }

            if (e.KeyCode == Keys.U && e.Control)
            {
                if (currTableName == "dict")
                    return;
                // Загрузить пользовательский словарь
                CurrTableName = "words";
                _dictToolStripSplitButton_ButtonClick(null, null);
                //e.Handled = false;
                CurrentDGV().Focus();
                //dictItem_Click(null, null);
            }

            if (e.KeyCode == Keys.K && e.Alt)
                cyrToolStripMenuItem.PerformClick();

            if (e.KeyCode == Keys.L && e.Alt)
                latToolStripMenuItem.PerformClick();
        }

        private void _webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.F1));

            if (e.KeyCode == Keys.P && e.Control)
                _webBrowser.ShowPrintPreviewDialog();

            //if (e.KeyCode == Keys.F && e.Control)
            //    _webBrowser. ShowPageSetupDialog();

            //if (e.KeyCode == Keys.Add && e.Control)
                //ListForm_KeyDown(sender, new KeyEventArgs(Keys.Add | Keys.Control));
            if (e.KeyCode == Keys.Insert)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.Insert));

            //if (e.KeyCode == Keys.Subtract && e.Control)
            if (e.KeyCode == Keys.Delete && e.Control)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.Subtract | Keys.Control));

            if (e.KeyCode == Keys.F7)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.F7));

            if (e.KeyCode == Keys.F5)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.F5)); 

            if (e.KeyCode == Keys.F9)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.F9 | Keys.Control));

            if (e.KeyCode == Keys.B && e.Control)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.B | Keys.Control));

            if (e.KeyCode == Keys.Escape)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.Escape));

            if (e.KeyCode == Keys.S && e.Control)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.S | Keys.Control));

            if (e.KeyCode == Keys.R && e.Control)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.R | Keys.Control));

            if (e.KeyCode == Keys.U && e.Control)
                ListForm_KeyDown(sender, new KeyEventArgs(Keys.U | Keys.Control));

            //MessageBox.Show(e.KeyData.ToString());
            /*
            //MessageBox.Show(e.KeyData.ToString());
            if (((char)e.KeyValue >= 'А' && (char)e.KeyValue <= 'я') || ((char)e.KeyValue == 'ё') || ((char)e.KeyValue == 'Ё')
                || ((char)e.KeyValue == ' '))
            {
                mainForm.SetTextBox(e.KeyValue.ToString());
                //e.Handled = true;
            } 
            */ 
        }

        public void SetSearchTextBox(string txt)
        {
            if (currTableName == "rus")
                _searchTextBox.Text = txt;
            else
            {
                if (Setup_SrbAlphabet == "lat")
                {
                    _searchTextBox.Text = Utils.CyrToLat(txt);
                }
                else
                {
                    _searchTextBox.Text = Utils.LatToCyr(txt);
                }
            }
        }

        public void SetSearchTextBoxToSelStart(string s)
        {
            string text = _searchTextBox.Text.Trim();
            int pos = _searchTextBox.SelectionStart;
            string newText = text.Substring(0, pos) + s + text.Substring(pos);
            SetSearchTextBox(newText);
            _searchTextBox.SelectionStart = pos + 1;
        }

        bool isSearchTextBoxEditing = false;
        
        /// <summary>
        /// Изменение строки поиска.
        /// </summary>
        private void _searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = _searchTextBox.Text.Trim();
            int pos = _searchTextBox.SelectionStart; 

            string newText = AutoReplacer(text, ref pos);
            if (String.Compare(text, newText) != 0)
            {
                text = newText;
                _searchTextBox.TextChanged -= _searchTextBox_TextChanged;
                SetSearchTextBox(text);

                _searchTextBox.SelectionStart = pos;
                _searchTextBox.TextChanged += _searchTextBox_TextChanged;
            }

            isSearchTextBoxEditing = true;
            try
            {
                if (SearchWord(text, true))
                {
                    _searchTextBox.ForeColor = SystemColors.WindowText;
                }
                else
                {
                    _searchTextBox.ForeColor = SystemColors.GrayText;
                }
            }
            finally 
            {
                isSearchTextBoxEditing = false;
            }
        }

        /// <summary>
        /// Замена символов при вводе в строку поиска.
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string AutoReplacer(string txt, ref int pos)
        {
            string result = txt;

            if (currTableName == "rus")
                return result;

            if (Setup_SrbAlphabet == "cyr")
            {
                result = AutoRepl(result, "ль", "љ", ref pos);
                result = AutoRepl(result, "лЬ", "љ", ref pos);
                result = AutoRepl(result, "Ль", "Љ", ref pos);
                result = AutoRepl(result, "ЛЬ", "Љ", ref pos);
                result = AutoRepl(result, "нь", "њ", ref pos);
                result = AutoRepl(result, "нЬ", "њ", ref pos);
                result = AutoRepl(result, "Нь", "Њ", ref pos);
                result = AutoRepl(result, "НЬ", "Њ", ref pos);
                result = AutoRepl(result, "дь", "ђ", ref pos);
                result = AutoRepl(result, "дЬ", "ђ", ref pos);
                result = AutoRepl(result, "Дь", "Ђ", ref pos);
                result = AutoRepl(result, "ДЬ", "Ђ", ref pos);
                result = AutoRepl(result, "чь", "ћ", ref pos);
                result = AutoRepl(result, "чЬ", "ћ", ref pos);
                result = AutoRepl(result, "Чь", "Ћ", ref pos);
                result = AutoRepl(result, "ЧЬ", "Ћ", ref pos);
                result = AutoRepl(result, "дж", "џ", ref pos);
                result = AutoRepl(result, "дЖ", "џ", ref pos);
                result = AutoRepl(result, "Дж", "Џ", ref pos);
                result = AutoRepl(result, "ДЖ", "Џ", ref pos);
                result = AutoRepl(result, "й", '\u0458'.ToString(), ref pos);
                result = AutoRepl(result, "Й", '\u0408'.ToString(), ref pos);
            }

            if (Setup_SrbAlphabet == "lat")
            {
                result = AutoRepl(result, "d'", "đ", ref pos);
                result = AutoRepl(result, "D'", "Đ", ref pos);
                result = AutoRepl(result, "z'", "ž", ref pos);
                result = AutoRepl(result, "Z'", "Ž", ref pos);
                result = AutoRepl(result, "c'", "ć", ref pos);
                result = AutoRepl(result, "C'", "Ć", ref pos);
                result = AutoRepl(result, "ć'", "č", ref pos);
                result = AutoRepl(result, "Ć'", "Č", ref pos);
                result = AutoRepl(result, "s'", "š", ref pos);
                result = AutoRepl(result, "S'", "Š", ref pos);
            }

            return result;
        }

        private string AutoRepl(string source, string findStr, string replStr, ref int pos)
        {
            string result = source;

            int k = result.IndexOf(findStr);
            while (k > -1)
            {
                result = result.Substring(0, k) + replStr + result.Substring(k + findStr.Length);
                if (k <= pos && findStr.Length > replStr.Length)
                {
                    pos--;
                }

                k = result.IndexOf(findStr);
            }

            return result;
        }

        /// <summary>
        /// Позиционирование найденного слова в середину грида (по возможности).
        /// </summary>
        private void SetCenterFoundRow()
        {
            if (CurrentDGV().CurrentRow == null)
                return;

            int displayedRows = CurrentDGV().DisplayedRowCount(false);
            int firstDisplayedRow = CurrentDGV().FirstDisplayedScrollingRowIndex;
            int currentRow = CurrentDGV().CurrentRow.Index;

            int newFirstDisplayedRow = currentRow - (displayedRows / 2);

            try
            {
                CurrentDGV().FirstDisplayedScrollingRowIndex = newFirstDisplayedRow;
            }
            catch {} // Если нельзя спозиционировать, то и не надо!
        }

        /// <summary>
        /// Изменение строки поиска.
        /// </summary>
        /// <param name="s"></param>
        public void SetTextBox(string s)
        {
            if (s == "BACK")
            {
                if (_searchTextBox.Text.Trim().Length > 0)
                    _searchTextBox.Text = _searchTextBox.Text.Substring(0, this._searchTextBox.Text.Trim().Length - 1);
            }
            else
                _searchTextBox.Text += s;

            _searchTextBox.SelectionStart = _searchTextBox.Text.Length;
        }

        private void _wordsDataGridView_Enter(object sender, EventArgs e)
        {
            (sender as DataGridView).DefaultCellStyle.SelectionBackColor = SystemColors.GradientActiveCaption; // .Highlight;
        }

        private void _wordsDataGridView_Leave(object sender, EventArgs e)
        {
            (sender as DataGridView).DefaultCellStyle.SelectionBackColor = SystemColors.InactiveCaption;
        }

        /// <summary>
        /// Обработка управляющих клавиш. Работает только для класса MyTextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                CurrentDGV().Focus();
                SendKeys.Send("{UP}");
            }
            else if (e.KeyCode == Keys.Down)
            {
                CurrentDGV().Focus();
                SendKeys.Send("{DOWN}");
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                CurrentDGV().Focus();
                SendKeys.Send("{PGUP}");
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                CurrentDGV().Focus();
                SendKeys.Send("{PGDN}");
            }
            else if (e.KeyCode == Keys.Return)
            {
                _searchTextBox_TextChanged(_searchTextBox, new EventArgs());
            }
        }

        private void _wordsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            // Устранение перехода на другую строку по нажатию Enter
            if (e.KeyCode == Keys.Return)
                e.Handled = true;

            if (e.KeyCode == Keys.Tab)
                e.Handled = true;
         }

        /// <summary>
        /// Контекстное меню для грида вызывается правой кнопкой мыши. 
        /// Для того, чтобы одновременно происходило позиционирование в гриде.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _gridContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            gridBackToolStripMenuItem.Visible = true;
            gridHistToolStripMenuItem.Visible = true;
            gridSaveHistToolStripMenuItem.Visible = true;
            toolStripSeparator5.Visible = true;
            gridSaveToUserDictToolStripMenuItem.Visible = true;
            gridDelFromUserDictToolStripMenuItem.Visible = true;

            if (currTableName == "words")
            {
                gridDelFromUserDictToolStripMenuItem.Visible = false;
            }
            if (currTableName == "rus")
            {
                toolStripSeparator5.Visible = false;
                gridSaveToUserDictToolStripMenuItem.Visible = false;
                gridDelFromUserDictToolStripMenuItem.Visible = false;
            }
            if (currTableName == "dict")
            {
                gridSaveToUserDictToolStripMenuItem.Visible = false;
            }

            ContextMenuStrip tContextMenu = (ContextMenuStrip)sender;
            Point tLocation = new Point(tContextMenu.Left, tContextMenu.Top);
            tLocation = CurrentDGV().PointToClient(Cursor.Position);
            DataGridView.HitTestInfo tHitTestInfo = CurrentDGV().HitTest(tLocation.X, tLocation.Y);
            if (tHitTestInfo.Type == DataGridViewHitTestType.Cell)
            {
                CurrentDGV()[tHitTestInfo.ColumnIndex, tHitTestInfo.RowIndex].Selected = true;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Контекстное меню ранее просмотренных слов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _oldContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _oldContextMenuStrip.Items.Clear();

            List<String> sList = oldWords.GetAll();

            // Если список истории пуст или в нем только одно слово, которое совпадает с текущим, то не показывать меню
            if (sList.Count == 0 ||
                    (sList.Count == 1 && oldWords.Equals(sList[0], currWord.Name)))
            {
                e.Cancel = true;
                return;
            }

            foreach (String s in sList)
            {
                if (!oldWords.Equals(s, currWord.Name))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(oldWords.CorrectAlphabet(s, Setup_SrbAlphabet));
                    item.Click += new EventHandler(OldItem_Click);
                    _oldContextMenuStrip.Items.Add(item);
                }
            }

            // Почему-то при первом обращении меню не показывалось. Проблема решилась так:
            e.Cancel = false;
        }

        /// <summary>
        /// Переход к ранее просмотренному слову.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OldItem_Click(object sender, EventArgs e)
        {
            oldWords.Set(currWord.Name);
            SearchWord((sender as ToolStripMenuItem).Text);
        }

        /// <summary>
        /// Переход к предыдущему слову.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            string name = oldWords.GetLastExclude(currWord.Name);
            oldWords.Set(currWord.Name);
            SearchWord(name);
        }

        /// <summary>
        /// Сохранить текущее слово перед выходом из программы.
        /// </summary>
        public void SaveLastWord()
        {
            Setup.WriteToSetup("LastWord", Setup_SrbAlphabet == "lat" ? Utils.LatToCyr(currWord.Name) : currWord.Name);
        }

        /// <summary>
        /// Сохранить текущий пользовательский словарь перед выходом из программы.
        /// </summary>
        public void SaveDictId()
        {
            Setup.WriteToSetup("CurrDictId", currDictId.ToString());
        }

        private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = false;
            string url = e.Url.ToString();
            if (url.Contains("^"))
            {
                string s = url.Substring(url.IndexOf("^") + 1);
                
                // Для русско-сербского: подгружаем статьи по мере необходимости
                if (currTableName == "rus")
                {
                    HtmlDocument doc = (sender as WebBrowser).Document;
                    HtmlElement element = doc.GetElementById(s.Replace(" ", "_"));
                    if (element != null)
                    {
                        if (element.InnerText == "#")
                        {
                            // Загрузить статью
                            DataRowView dr = (DataRowView)_rusBindingSource.Current;
                            if (dr != null)
                            {
                                element.InnerHtml = GetHtmlByName(true, dr["NAME"].ToString(), s, 0);
                            }
                        }
                    }
                }
                e.Cancel = true;
            }
            else if (url.Contains("@@"))
            {
                string s = url.Substring(url.IndexOf("@@") + 2);
                rusItem_Click(null, null);
                GoToReference(s);
                e.Cancel = true;
            }
            else if (url.Contains("@"))
            {
                string s = url.Substring(url.IndexOf("@") + 1);

                // В пользовательских словарях по ссылкам не переходим
                if (currTableName == "dict")
                {
                    e.Cancel = true;
                }
                else if (currTableName == "rus")
                {
                    baseItem_Click(null, null);
                    GoToReference(s);
                    e.Cancel = true;
                }
                else if (currTableName == "words")
                {
                    if (!GoToReference(s))
                        e.Cancel = true;
                }
            }
            else if (url.Contains("~")) // Переход по кнопке к слову в сербско-русском словаре
            {
                string s = url.Substring(url.IndexOf("~") + 1).Replace("_", " ");
                //MessageBox.Show(s);
                baseItem_Click(null, null);
                GoToReference(s);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Переходы по ссылкам.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            /*
            string url = e.Url.ToString();
            if (url.Contains("@"))
            {
                string s = url.Substring(url.IndexOf("@") + 1);
                //MessageBox.Show(s);

                GoToReference(s);
            }
            */ 
        }

        /// <summary>
        /// Процедура передаваемая в форму настроек для применения настроек.
        /// (в качестве делегата ApplyChangesDelegate()).
        /// </summary>
        private void ApplyChangedParams()
        {
            LoadParameters(false);
        }

        private void ListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isApplicationTerminated)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                if (ConfirmClose == "1")
                {
                    e.Cancel =
                        (MessageBox.Show("Закрыть словарь?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                            DialogResult.No);
                }
                else
                    e.Cancel = false;
            }
        }

        private void ListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Сохранить историю для текущего словаря 
            if (oldWords != null)
                oldWords.Serialize();

            SaveLastWord();
            SaveDictId();
            SaveFormProperties();
            Application.Exit();
        }

        /// <summary>
        /// Сохранить положение формы.
        /// </summary>
        private void SaveFormProperties()
        { 
            if (this.WindowState == FormWindowState.Maximized)
            {
                Setup.WriteToSetup("ListForm_WindowState", "Maximized");
                // Если сохраняем максимизированное окно, то не нужно сохранять его размеры, так как 
                // они будут максимальные, и при новом старте при возврате окна к нормальному состоянию
                // размеры окна не изменятся. Поэтому при новом старте берём старые сохранённые размеры
            }
            else
            {
                Setup.WriteToSetup("ListForm_WindowState", "Normal");
                Setup.WriteToSetup("ListForm_Width", this.Width.ToString());
                Setup.WriteToSetup("ListForm_Height", this.Height.ToString());
                Setup.WriteToSetup("ListForm_Top", this.Top.ToString());
                Setup.WriteToSetup("ListForm_Left", this.Left.ToString());
                Setup.WriteToSetup("ListForm_SDistance", _splitContainer.SplitterDistance.ToString());
            }
            // Minimized не учитывается. Если закрыли минимизированное окно, то открывать будем всё-равно Normal
        }

        /// <summary>
        /// Восстановить положение формы.
        /// </summary>
        private void RestoreFormProperties()
        {
            int width;
            if (int.TryParse(Setup.ReadFromSetup("ListForm_Width"), out width))
                this.Width = width;

            int height;
            if (int.TryParse(Setup.ReadFromSetup("ListForm_Height"), out height))
                this.Height = height;

            int top;
            if (int.TryParse(Setup.ReadFromSetup("ListForm_Top"), out top))
                this.Top = top;

            int left;
            if (int.TryParse(Setup.ReadFromSetup("ListForm_Left"), out left))
                this.Left = left;

            int distance;
            if (int.TryParse(Setup.ReadFromSetup("ListForm_SDistance"), out distance))
                _splitContainer.SplitterDistance = distance;

            string state = Setup.ReadFromSetup("ListForm_WindowState");
            if (state == "Maximized")
            {
                this.WindowState = FormWindowState.Maximized;
            }

            // Если сбились координаты
            if (top < 0 || left < 0)
                this.SetBounds((Screen.GetWorkingArea(this).Width - this.Width) / 2,
                    (Screen.GetWorkingArea(this).Height - this.Height) / 2,
                    this.Width, this.Height);
        }

        /*
        private void InitializeFonts()
        {
            //LoadFont();
            //_searchTextBox.Font = new Font(private_fonts.Families[0], 10);
            //_wordsDataGridView.DefaultCellStyle.Font = new Font(private_fonts.Families[0], 10);
            //_oldContextMenuStrip.Font = new Font(private_fonts.Families[0], 9);
            
            //_label1.UseCompatibleTextRendering = true;

            //_searchTextBox.Text = "сто\u030Fит  и стои\u0301т";
            //MessageBox.Show("сто\u030Fит  и стои\u0301т");

            foreach(Control ctrl in _wordsDataGridView.Controls)
                if (ctrl.GetType() == typeof(VScrollBar))
                    ctrl.Width = 200;
        }
        */
        /*
        private void LoadFont()
        {
            using (MemoryStream fontStream = new MemoryStream(Properties.Resources.PTC55F))
            {
                // create an unsafe memory block for the font data
                System.IntPtr data = System.Runtime.InteropServices.Marshal.AllocCoTaskMem((int)fontStream.Length);
                // create a buffer to read in to
                byte[] fontdata = new byte[fontStream.Length];
                // read the font data from the resource
                fontStream.Read(fontdata, 0, (int)fontStream.Length);
                // copy the bytes to the unsafe memory block
                System.Runtime.InteropServices.Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);
                // pass the font to the font collection
                private_fonts.AddMemoryFont(data, (int)fontStream.Length);
                // close the resource stream
                fontStream.Close();
                // free the unsafe memory
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(data);
            }
        }
        */ 

        private void _splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            CurrentDGV().Focus();
        }

        private void _searchByLetterTtoolStripButton_Click(object sender, EventArgs e)
        {
            if (!_searchByLetterToolStripButton.Checked)
            {
                ShowSyllableForm();
            }
            else
            {
                HideSyllableForm();
            }
        }

        private void ShowSyllableForm()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (syllableForm == null)
                {
                    syllableForm = new SyllableForm(this, Setup_SrbAlphabet);
                    //SetOpacitySyllable();
                }
                _searchByLetterToolStripButton.Checked = true;
                mainFindByLetterToolStripMenuItem.Checked = true;
                syllableForm.Show(this);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        public void HideSyllableForm()
        {
            if (syllableForm != null)
            {
                _searchByLetterToolStripButton.Checked = false;
                mainFindByLetterToolStripMenuItem.Checked = false;
                syllableForm.Hide();
            }
            _searchTextBox.Focus();
        }

        public void DisableSyllableForm()
        {
            HideSyllableForm();
            _searchByLetterToolStripButton.Enabled = false;
            mainFindByLetterToolStripMenuItem.Enabled = false;
        }

        public void EnableSyllableForm()
        {
            _searchByLetterToolStripButton.Enabled = true;
            mainFindByLetterToolStripMenuItem.Enabled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplashForm3 form = new SplashForm3();
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.SetStatusInfo(IsDonated);
            form.ShowDialog();
        }

        private void mainExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainFindByLetterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _searchByLetterToolStripButton.PerformClick();
        }

        private void mainHistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backToolStripSplitButton.ShowDropDown();
        }

        private void mainBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backToolStripSplitButton.PerformButtonClick();
        }

        private void _setupToolStripButton_Click(object sender, EventArgs e)
        {
            setupForm.CurrentPage = -1;
            LoadSetupForm();
        }

        private void mainSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupForm.CurrentPage = 0;
            LoadSetupForm();
        }

        private void oformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupForm.CurrentPage = 1;
            LoadSetupForm();
        }

        private void dictSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupForm.CurrentPage = 2;
            LoadSetupForm();
        }

        private void LoadSetupForm()
        {
            tmpWordsCountUserDict = Data.GetWordsCountUserDict(currDictId);
            setupForm.alphabetFromMenu = Setup_SrbAlphabet;
            setupForm.IsChanged = false;
            setupForm.ShowDialog();
            bool isChanged = setupForm.IsChanged;

            this.Refresh();

            if (isChanged)
            {
                // Применить параметры
                LoadParameters(false);
            }
            else
            {
                // Применить только параметры для пользовательских словарей
                ApplyDictParameters();
            }

            _searchTextBox.Focus();
        }

        /*
        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string workDir = ScanWord.Utils.GetWorkDirectory();
            if (File.Exists(workDir + "SRWords.pdf"))
                Help.ShowHelp(this, "SRWords.pdf");
            else
                MessageBox.Show("Не найден файл справки.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        */

        private void cyrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string old_SrbAlphabet = Setup_SrbAlphabet;
            Setup_SrbAlphabet = "cyr";
            cyrToolStripMenuItem.Checked = true;
            latToolStripMenuItem.Checked = false;
            if (old_SrbAlphabet != Setup_SrbAlphabet)
            {
                Change_Alphabet();
            }
        }

        private void latToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string old_SrbAlphabet = Setup_SrbAlphabet;
            Setup_SrbAlphabet = "lat";
            cyrToolStripMenuItem.Checked = false;
            latToolStripMenuItem.Checked = true;
            if (old_SrbAlphabet != Setup_SrbAlphabet)
            {
                Change_Alphabet();
            }
        }

        private void mainSaveHistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(currWord.Name))
                oldWords.Set(currWord.Name);
        }

        private void ListForm_Resize(object sender, EventArgs e)
        {
            //if (WindowState == FormWindowState.Minimized)
            //{
            //    Hide();
            //}
            //else 
            if ((_splitContainer.Width < _splitContainer.Panel1.Width + _splitContainer.Panel2.Width) &&
                _splitContainer.Panel2.Width == _splitContainer.Panel2MinSize)
                _splitContainer.SplitterDistance = this.Width - _splitContainer.Panel2MinSize;
        }

        /// <summary>
        /// Контекстное меню пользовательских словарей.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dictContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _dictContextMenuStrip.Items.Clear();

            ToolStripMenuItem item = new ToolStripMenuItem("Сербско-русский словарь");
            item.Click += new EventHandler(baseItem_Click);
            item.ShortcutKeys = Keys.S | Keys.Control;
            _dictContextMenuStrip.Items.Add(item);

            item = new ToolStripMenuItem("Русско-сербский словарь");
            item.Click += new EventHandler(rusItem_Click);
            item.ShortcutKeys = Keys.R | Keys.Control;
            _dictContextMenuStrip.Items.Add(item);

            _dictContextMenuStrip.Items.Add(new ToolStripSeparator());

            // Получить список пользовательских словарей
            List<String> sDict = new List<string>();
            List<String> sId = new List<string>();
            int cnt = Data.LoadListUserDicts(out sDict, out sId);
            if (cnt > 0)
            {
                for (int i = 0; i < sDict.Count; i++)
                {
                    item = new ToolStripMenuItem(sDict[i]);
                    item.Tag = sId[i];
                    item.Click += new EventHandler(dictItem_Click);
                    if (currDictId.ToString() == sId[i])
                    {
                        item.Checked = true;
                        //item.ShortcutKeys = Keys.U | Keys.Control;
                    }
                    _dictContextMenuStrip.Items.Add(item);
                }
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Нет ни одного пользовательского словаря!", "Внимание!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            e.Cancel = false;
        }

        private void _dictToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            if (currTableName != "dict")
            //if (currTableName == "words")
            {
                if (currDictId != -1)
                {
                    if (oldWords != null && !oldWords.IsRus())
                        if (!String.IsNullOrEmpty(currWord.Name))
                            oldWords.Set(currWord.Name);
                    /*
                    if (oldWords != null)
                    {
                        if (!String.IsNullOrEmpty(currWord.Name))
                            oldWords.Set(currWord.Name);
                    }
                    */

                    CurrTableName = "dict";
                    LoadData("dict", currDictId);

                    DisableSyllableForm();

                    _langLeftLabel.Text = "Сербский";
                    _langRightLabel.Text = "Русский";
                }
                else
                { 
                    // Вывести меню пользовательских словарей
                    _dictToolStripSplitButton.ShowDropDown();
                }
            }
            else
            {
                if (oldWords != null)
                {
                    if (!String.IsNullOrEmpty(currWord.Name))
                        oldWords.Set(currWord.Name);
                }

                baseItem_Click(null, null);
            }
        }

        private void _mainMenuStrip_MenuActivate(object sender, EventArgs e)
        {
            dictToolStripMenuItem.DropDownItems.Clear();

            ToolStripMenuItem item = new ToolStripMenuItem("Сербско-русский словарь");
            item.Click += new EventHandler(baseItem_Click);
            item.ShortcutKeys = Keys.S | Keys.Control;
            dictToolStripMenuItem.DropDownItems.Add(item);

            item = new ToolStripMenuItem("Русско-сербский словарь");
            item.Click += new EventHandler(rusItem_Click);
            item.ShortcutKeys = Keys.R | Keys.Control;
            dictToolStripMenuItem.DropDownItems.Add(item);

            // Получить список пользовательских словарей
            List<String> sDict = new List<string>();
            List<String> sId = new List<string>();
            int cnt = Data.LoadListUserDicts(out sDict, out sId);
            if (cnt > 0)
            {
                dictToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                for (int i = 0; i < sDict.Count; i++ )
                {
                    item = new ToolStripMenuItem(sDict[i]);
                    item.Tag = sId[i];
                    item.Click += new EventHandler(dictItem_Click);
                    if (currDictId.ToString() == sId[i])
                    {
                        item.Checked = true;
                        //item.ShortcutKeys = Keys.U | Keys.Control;
                    }
                    dictToolStripMenuItem.DropDownItems.Add(item);
                }
            }
        }

        void baseItem_Click(object sender, EventArgs e)
        {
            if (currTableName == "words")
                return;

            if (oldWords != null && !oldWords.IsRus())
                if (!String.IsNullOrEmpty(currWord.Name))
                    oldWords.Set(currWord.Name);

            // Загрузить словарь
            CurrTableName = "words";
            LoadData();

            // Разрешить поиск по буквам
            EnableSyllableForm();

            _langLeftLabel.Text = "Сербский";
            _langRightLabel.Text = "Русский";

            int delay = 20;
            _switchPictureBox.Image = Properties.Resources.switch_135 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_90 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_45 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_0 as Bitmap;
            _panelReverse.Refresh();
        }

        void rusItem_Click(object sender, EventArgs e)
        {
            if (currTableName == "rus")
                return;

            if (oldWords != null && !oldWords.IsRus())
                if (!String.IsNullOrEmpty(currWord.Name))
                    oldWords.Set(currWord.Name);

            CurrTableName = "rus";
            LoadData("rus", 0);

            DisableSyllableForm();

            _langLeftLabel.Text = "Русский";
            _langRightLabel.Text = "Сербский";

            int delay = 20;
            _switchPictureBox.Image = Properties.Resources.switch_45 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_90 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_135 as Bitmap;
            _panelReverse.Refresh();
            Thread.Sleep(delay);
            _switchPictureBox.Image = Properties.Resources.switch_0 as Bitmap;
            _panelReverse.Refresh();

        }

        void dictItem_Click(object sender, EventArgs e)
        {
            int idDict = int.Parse((sender as ToolStripMenuItem).Tag.ToString());

            if (currTableName == "dict" && currDictId == idDict)
                return;

            if (oldWords != null && !oldWords.IsRus())
                if (!String.IsNullOrEmpty(currWord.Name))
                    oldWords.Set(currWord.Name);

            CurrTableName = "dict";
            currDictId = idDict;
            LoadData("dict", idDict);

            DisableSyllableForm();

            _langLeftLabel.Text = "Сербский";
            _langRightLabel.Text = "Русский";
        }

        private void mainSaveToUserDictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currTableName == "dict")
                return;
            
            if (currTableName == "rus")
            {
                MessageBox.Show("Добавлять в пользовательские словари\n" +
                    "можно только сербские слова.",
                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currDictId == -1)
            {
                MessageBox.Show("Не выбран активный пользовательский словарь!\n" +
                    "Выберите в меню \"Словари\" пользовательский словарь,\n" +
                    "затем вернитесь в основной словарь и добавляйте слова.",
                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currDictId < -1) // Вернее, (sysd.system == 1)
            { 
                // Это - системные словари. Их нельзя изменять!
                return;
            }

            // 2023
            /*
            if (Data.IsWordInUserDict(currDictId, currWord.Id))
            {
                MessageBox.Show("Слово \"" + currWord.Name + "\" уже есть в словаре [" +
                            Data.GetNameForDict(currDictId) + "].",
                            "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Добавить слово \"" + currWord.Name + "\" в словарь [" +
                            Data.GetNameForDict(currDictId) + "]?", "Внимание!",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Data.AddInUserDict(currDictId, currWord.Id);
            }
            */

            Data.AddInUserDict(currDictId, currWord.Name);
        }

        private void mainClearHistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить историю для загруженного словаря?", "Внимание!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                oldWords.Clear();
        }

        private void mainDelFromUserDictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currTableName == "dict")
            {
                if (CurrentBS().Count > 0)
                {
                    if (MessageBox.Show("Удалить слово \"" + currWord.Name + "\" из словаря [" +
                            Data.GetNameForDict(currDictId) + "]?", "Внимание!",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // 2023
                        //Data.DelFromUserDict(currDictId, currWord.Id);

                        Data.DelFromUserDict(currDictId, currWord.Name);

                        // Удалить слово из истории
                        oldWords.Delete(Setup_SrbAlphabet == "lat" ? Utils.LatToCyr(currWord.Name) : currWord.Name);

                        LoadData("dict", currDictId);
                    }
                }
            }
        }


        private DataGridView CurrentDGV()
        {
            switch (currTableName)
            {
                case "words": return _wordsDataGridView;
                case "rus": return _rusDataGridView;
                case "dict": return _dictDataGridView;
                default: return null;
            }
        }

        private BindingSource CurrentBS()
        {
            switch (currTableName)
            {
                case "words" : return _srbBindingSource;
                case "rus" : return _rusBindingSource;
                case "dict": return _dictBindingSource;
                default: return null;
            }
        }

        private void _rusBindingSource_PositionChanged(object sender, EventArgs e)
        {
            ShowCurrentWord();
        }

        /*
        private void ShowCurrentRusWord(DataRowView dr)
        {
            if (dr == null)
                return;

            if (oldWords != null && oldWords.IsRus())
                oldWords.SetWait(dr["NAME"].ToString());

            string srbname = dr["SRBNAME"].ToString();
            string[] aSrb = srbname.Split(new char[] { ';' });
            List<String> aList = new List<string>();
            for (int i = 0; i < aSrb.Length; i++)
            {
                string s = aSrb[i];
                if (aList.IndexOf(s) == -1)
                    aList.Add(s);
            }
            aList.Sort();

            wbShowAllToolStripMenuItem.Tag = aList;

            string t = "";
            int numId = 1;
            int cnt = aList.Count;
            string display =  cnt == 1 ? "block" : multiRusValuesOpened ? "block" : "none";
            string nameId;
            //t += "<table>";
            foreach (string s in aList)
            {
                //t += "<tr><td>";
                nameId = s.Replace(" ", "_");
                t += "<a href=\"@" + s + "\" class=\"OpenHide\" " +
                     "style=\"color:black; font:normal 16px Arial;\" " +
                     //"onclick=\"facechange('#" + nameId + "'); return true;\">" +
                     "onclick=\"viewdiv('" + nameId + "'); return true;\">" +
                     //s + "</a>" +
                     (Setup_SrbAlphabet == "lat" ? Utils.CyrToLat(s) : s) + "</a>" +

                     //"</td><td width=\"20px\"></td><td align=\"right\">"+
                     "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +

                     "<button type=\"button\" title=\"Найти слово в сербско-русском словаре\" " +
                     "onclick=\"location.href='~" + nameId + "'\" class=\"Button1\">Найти</button>" +

                     //"</td></tr><tr>" +

                     "<div id=\"" + nameId + "\"; class=\"SrbBlock\"; style=\"display:" + display + "\">" +
                     GetHtmlByName(false, dr["NAME"].ToString(), s, cnt) +
                     "</div>";
                     
                     //"</tr>";

                if (numId < cnt)
                    t += "<HR>";

                numId++;
            }
            //t += "</table>";

            byte[] bytes = Encoding.UTF8.GetBytes(t);
            ShowRusWord(bytes);

            // Синхронизировать строку поиска
            if (!isSearchTextBoxEditing)
            {
                _searchTextBox.TextChanged -= _searchTextBox_TextChanged;
                //_searchTextBox.Text = dr["NAME"].ToString();
                SetSearchTextBox(dr["NAME"].ToString());
                _searchTextBox.ForeColor = SystemColors.WindowText;
                _searchTextBox.TextChanged += _searchTextBox_TextChanged;
            }
        }
        */

        private string GetHtmlByName(bool doOpen, string rusKey, string wName, int cnt)
        {
            // Если сербских значений > 1, то не раскрывать их и не загружать пока из БД
            //if (cnt > 1) return "#"; // # - признак пустой статьи
            if (!doOpen)
                if (!multiRusValuesOpened) 
                    return "#"; // # - признак пустой статьи

            string xmlString = "";
            //int pos = _srbBindingSource.Find("NAME", baseAlphabet == "lat" ? Utils.CyrToLat(wName) : wName);
            if (_srbBindingSource.Current != null)
            {
                int pos = _srbBindingSource.Find("NAME", wName);
                if (pos > -1)
                {
                    DataRowView dr = (DataRowView)_srbBindingSource.List[pos];
                    if (dr != null && dr["XML"] != DBNull.Value)
                    {
#if DEMO
                    // так как поле XML имеет тип BLOB
                    byte[] bytesXml = (byte[])dr["XML"];
                    Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
                    xmlString = dstEncodingFormat.GetString(bytesXml);
#else
                        xmlString = dr["XML"].ToString();
#endif
                    }
                }
            }

            if (xmlString.Length > 0)
            {
#if SQLITE
                // Получить объект ArticleInfo
                ArticleInfo a = new ArticleInfo();
                a = a.Deserialize(xmlString);
#else
                // Получить объект Art
                //ScanWord.Art a = new ScanWord.Art();
                // Объект ArticleInfo
                ArticleInfo a = new ArticleInfo();
                a = a.Deserialize(xmlString);
#endif
                // Сформировать HTML-скрипт: не создавать ссылки
                return a.CreateScript(false, Setup_SrbAlphabet, Setup_RusAccent, rusKey);
            }

            return "";
        }

        /*
        private void ShowRusWord(byte[] bytes)
        {
            String finalString = String.Empty;

            Encoding dstEncodingFormat = Encoding.GetEncoding("utf-8");
            if (bytes != null)
                finalString = dstEncodingFormat.GetString(bytes);

            if (!File.Exists(Css.cssFile))
            {
                Css.MakeCss();
            }

            finalString = HTMLStartString() + finalString + ScanWord.Utils.HTMLEndString();

            //MessageBox.Show(finalString);

            _webBrowser.DocumentText = finalString;
        }
        */

        /*
        private string HTMLStartString()
        {
            return "<HTML><HEAD>" +
                   "<LINK rel=\"stylesheet\" type=\"text/css\" href=\"" +
                   Utils.GetWorkDirectory() + Utils.CSS_FILE_NAME + "\">" + Environment.NewLine +
                   "<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" + Environment.NewLine +
                   
                   //"<script src=\"" + Utils.GetWorkDirectory() + "jquery-1.11.0.min.js\"></script>" + Environment.NewLine +
                   //"<script src=\"" + Utils.GetWorkDirectory() + "jquery-1.4.2.min.js\"></script>" + Environment.NewLine +
                   "<script>" + Environment.NewLine +
                   (*
                   "function facechange (objName) { " +
                    "if ( $(objName).css('display') == 'none' ) { " +
                    "$(objName).animate({height: 'show'}, 0); " +
                    "} else { " +
                    "$(objName).animate({height: 'hide'}, 0); " +
                     "} }" + 
                   *)
                   "function viewdiv(id) {" +
                    "var el = document.getElementById(id);" +
                    "if (el.style.display == \"block\") {" +
                    "el.style.display = \"none\";" +
                    "} else {" +
                    "el.style.display = \"block\";" +
                    "}}" +
                    "</script>" + Environment.NewLine +

                   "</HEAD><BODY style=\"background-color:#f4f4f4;\">" + Environment.NewLine;
        }
        */
        private void _wbContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            wbBackToolStripMenuItem.Visible = true;
            wbHistToolStripMenuItem.Visible = true;
            wbSaveHistToolStripMenuItem.Visible = true;
            wbCopyToolStripSeparator.Visible = true;
            wbUserToolStripSeparator.Visible = true;

            wbShowHideToolStripSeparator.Visible = false;
            wbShowAllToolStripMenuItem.Visible = false;
            wbHideAllToolStripMenuItem.Visible = false;

            if (currTableName == "words")
            {
                wbSaveToUserDictToolStripMenuItem.Visible = true;
                wbDelFromUserDictToolStripMenuItem.Visible = false;
            }
            else if (currTableName == "dict")
            {
                wbSaveToUserDictToolStripMenuItem.Visible = false;
                wbDelFromUserDictToolStripMenuItem.Visible = true;
            }

            // Доступность пункта "Копировать"
            wbCopyToolStripMenuItem.Enabled = GetWBSelectedText().Length > 0;
        }

        private void _wbRusContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            // Доступность пункта "Копировать"
            _wbRusCopyToolStripMenuItem.Enabled = GetWBSelectedText().Length > 0;
        }

#region методы для реализации notifyIcon 
        /*
        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Restore();
        }

        private void Restore()
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void _notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            (sender as NotifyIcon).Text = "Сербско-русский словарь"; // this.Text;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restore();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isApplicationTerminated = true;
            Close();
        }
        */ 
#endregion

        /*
        private void SendMail()
        {
            System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("aaa@yandex.ru", "User");
            System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress("alex27@mail.ru");
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);
            message.Subject = "";
            message.Body = "";

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.mail.ru");
            client.Timeout = 2000;
            client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            //client.UseDefaultCredentials = true;

            client.Send(message);
        }
        */

        private void wbCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string buf = GetWBSelectedText();
            if (buf.Length > 0)
            {
                Clipboard.SetText(buf);
            }
        }

        /// <summary>
        /// Возвращает выделенный текст в браузере.
        /// </summary>
        /// <returns></returns>
        private string GetWBSelectedText()
        {
            IHTMLDocument2 htmlDocument = _webBrowser.Document.DomDocument as IHTMLDocument2;
            IHTMLSelectionObject currentSelection = htmlDocument.selection;
            if (currentSelection != null)
            {
                IHTMLTxtRange range = currentSelection.createRange() as IHTMLTxtRange;
                if (range != null)
                {
                    if (range.text == null)
                        return String.Empty;
                    else
                        return range.text;
                }
            }

            return String.Empty;
        }

        private void wbShowAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object obj = wbShowAllToolStripMenuItem.Tag;
            if (obj == null)
                return;

            List<String> aList = obj as List<string>;
            if (aList == null)
                return;

            HtmlDocument doc = _webBrowser.Document;

            Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string s in aList)
                {
                    HtmlElement element = doc.GetElementById(s.Replace(" ", "_"));
                    if (element != null)
                    {
                        multiRusValuesOpened = true;
                        if (element.InnerText == "~") // Если статья ещё не загружена
                        {
                            // Загрузить статью
                            DataRowView dr = (DataRowView)_rusBindingSource.Current;
                            if (dr != null)
                            {
                                element.InnerHtml = GetHtmlByName(true, dr["NAME"].ToString(), s, 0);
                            }
                        }

                        // Показать статью
                        element.Style = "display:block";

                        HtmlElement image = doc.GetElementById(Const.IMG_EXPAND_PREFIX + Utils.CyrToLat(s).Replace(" ", "_"));
                        if (image != null)
                        {
                            image.SetAttribute("src", Utils.GetWorkDirectory() + Const.IMG_EXPAND_LESS);
                            image.SetAttribute("alt", Const.HIDE_ITEM);
                        }
                    }
                }
            }
            finally 
            {
                Cursor = Cursors.Default;
            }
        }

        private void wbHideAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
           object obj = wbShowAllToolStripMenuItem.Tag;
           if (obj == null)
               return;

            List<String> aList = obj as List<string>;
            if (aList == null)
                return;

            HtmlDocument doc = _webBrowser.Document;

            foreach (string s in aList)
            {
                HtmlElement element = doc.GetElementById(s.Replace(" ", "_"));
                if (element != null)
                {
                    // Скрыть статью
                    element.Style = "display:none";
                    multiRusValuesOpened = false;

                    HtmlElement image = doc.GetElementById(Const.IMG_EXPAND_PREFIX + Utils.CyrToLat(s).Replace(" ", "_"));
                    if (image != null)
                    {
                        image.SetAttribute("src", Utils.GetWorkDirectory() + Const.IMG_EXPAND_MORE);
                        image.SetAttribute("alt", Const.SHOW_ITEM);
                    }
                }
            }
        }

        /*
        private void _demoLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("Mailto:alex27@mail.ru"); 
        }
        */

        private void GetCurrAlpabet(out string rus_srb, out string cyr_lat)
        {
            rus_srb = currTableName == "rus" ? "rus" : "srb";
            cyr_lat = baseAlphabet;
        }

        private void GetKeyboardLetter(string letter)
        {
            if (letter == "{BS}")
            { 
                ListForm_KeyPress(this, new KeyPressEventArgs('\b'));
            }
            else
                ListForm_KeyPress(this, new KeyPressEventArgs(Char.Parse(letter)));
        }

        private void keyBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!keyBoardToolStripMenuItem.Checked)
            {
                ShowKeyboardForm();
            }
            else
            {
                HideKeyboardForm();
            }
        }

        private void ShowKeyboardForm()
        {
            _searchTextBox.Focus();
            if (keyboardForm == null)
            {
                keyboardForm = new KeyboardForm(this, GetCurrAlpabet, GetKeyboardLetter);
            }
            keyboardForm.Show(this);
            //keyBoardToolStripMenuItem.Click -= keyBoardToolStripMenuItem_Click;
            keyBoardToolStripMenuItem.Checked = true;
            //keyBoardToolStripMenuItem.Click += keyBoardToolStripMenuItem_Click;
        }

        public void HideKeyboardForm()
        {
            keyboardForm.Hide();
            keyBoardToolStripMenuItem.Checked = false;
            _searchTextBox.Focus();
        }

        private void _wordsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (this.currTableName != "rus")
            //{
                if (Setup_SrbAlphabet == "lat")
                {
                    e.Value = Utils.CyrToLat(e.Value.ToString());
                }
                else
                {
                    e.Value = Utils.LatToCyr(e.Value.ToString());
                }
            //}
        }

        private void _searchTextBox_SizeChanged(object sender, EventArgs e)
        {
            int oldHeight = _panelTextBox.Height;
            _panelTextBox.Size = new Size(_panelTextBox.Width, _searchTextBox.Height + 8);
            _panelGrid.Height += (oldHeight - _panelTextBox.Height);
        }

        private void _switchPictureBox_Click(object sender, EventArgs e)
        {
            SwitchLanguage();
        }


        private void SwitchLanguage()
        {
            if (currTableName == "words")
            {
                rusItem_Click(null, null);
            }
            else
            {
                baseItem_Click(null, null);
            }

            CurrentDGV().Focus();
        }

        private void rus10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mail.SendMail();
        }

        private void _speechButton_Click(object sender, EventArgs e)
        {
            TextToSpeech.Say(currTableName, currWord.Name);
        }
    }
}
