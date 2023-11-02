using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SRWords
{
    public partial class AlphaPanel : Panel
    {
        enum StatusImage
        {
            siNone,
            siMoved,
            siSelected
        }

        private List<PictureBox> pb = new List<PictureBox>();
        private PictureBox currentPB = null;

        /// <summary>
        /// Список всех элементов.
        /// </summary>
        private List<String> alphaList;
        public List<String> AlphaList
        {
            get { return alphaList; }
            set
            {
                alphaList = value;
                InitializeAlpha();
            }
        }

        /// <summary>
        /// Событие, которое происходит при выборе элемента.
        /// </summary>
        public event EventHandler AlphaSelected;

        /// <summary>
        /// Сгенерировать событие.
        /// </summary>
        private void OnAlphaSelected(String letter)
        {
            if (AlphaSelected != null)
                AlphaSelected(letter, new EventArgs());
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AlphaPanel()
        {
            InitializeComponent();

            this.BackColor = Color.Transparent;// White;

            AlphaList = null;
        }

        /// <summary>
        /// Инициализация контрола.
        /// </summary>
        private void InitializeAlpha()
        {
            if (alphaList != null)
            {
                ClearPB();

                int k = 0;

                for (int i = 0; i < alphaList.Count; i++)
                {
                    PictureBox p = new PictureBox();
                    Image image = Properties.Resources.box16; // small;
                    p.Size = new Size(image.Width, image.Height);
                    p.Image = image;
                    p.Left = 0;
                    p.Top = k;
                    p.Name = "p" + i.ToString();
                    p.Tag = StatusImage.siNone;
                    p.Paint += new PaintEventHandler(p_Paint);
                    p.MouseEnter += new EventHandler(p_MouseEnter);
                    p.MouseLeave += new EventHandler(p_MouseLeave);
                    p.Click += new EventHandler(p_Click);
                    k += image.Height - 1;
                    this.Controls.Add(p);
                    pb.Add(p);

                    if (i == 0)
                    {
                        p.Tag = StatusImage.siSelected;
                        p.Image = Properties.Resources.box16blue; // smallblue;
                        currentPB = p;
                    }
                }
            }
        }


        #region События PictureBox
        void p_Click(object sender, EventArgs e)
        {
            // Второй параметр true означает, что нужно сгенерировать событие
            SelectItem(sender as PictureBox, true);
        }

        /// <summary>
        /// Смена текущего элемента.
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="eventing"></param>
        private void SelectItem(PictureBox pictureBox, bool eventing)
        {
            if ((StatusImage)pictureBox.Tag != StatusImage.siSelected) // или (pictureBox != currentPB)
            {
                pictureBox.Image = Properties.Resources.box16blue; // smallblue;
                pictureBox.Tag = StatusImage.siSelected;
                currentPB.Image = Properties.Resources.box16; // small;
                currentPB.Tag = StatusImage.siNone;
                currentPB = pictureBox;

                // Если нужно сгенерировать событие об изменении текущего элемента
                if (eventing)
                {
                    int index = int.Parse(currentPB.Name.Substring(1));
                    OnAlphaSelected(alphaList[index]);
                }
            }

            Invalidate();
        }

        void p_MouseLeave(object sender, EventArgs e)
        {
            if ((StatusImage)(sender as PictureBox).Tag != StatusImage.siSelected)
            {
                Cursor = Cursors.Default;
                (sender as PictureBox).Tag = StatusImage.siNone;

                PictureBox p = (sender as PictureBox);
                p.Invalidate();
            }
        }

        void p_MouseEnter(object sender, EventArgs e)
        {
            if ((StatusImage)(sender as PictureBox).Tag != StatusImage.siSelected)
            {
                Cursor = Cursors.Hand;
                (sender as PictureBox).Tag = StatusImage.siMoved;

                PictureBox p = (sender as PictureBox);
                p.Invalidate();
            }
        }

        void p_Paint(object sender, PaintEventArgs e)
        {
            if ((StatusImage)(sender as PictureBox).Tag == StatusImage.siMoved)
            {
                using (Font myFont = new Font("Arial", 9))
                {
                    Brush brush;
                    brush = Brushes.DarkRed;

                    int index = int.Parse((sender as PictureBox).Name.Substring(1));
                    e.Graphics.DrawString(alphaList[index], myFont, brush, new Point(4, 1));
                }
            }
            else
            {
                using (Font myFont = new Font("Arial", 8))
                {
                    Brush brush;

                    if ((StatusImage)(sender as PictureBox).Tag == StatusImage.siMoved ||
                        (StatusImage)(sender as PictureBox).Tag == StatusImage.siSelected)
                        brush = Brushes.DarkRed;
                    else
                        brush = Brushes.LightBlue;

                    int index = int.Parse((sender as PictureBox).Name.Substring(1));
                    e.Graphics.DrawString(alphaList[index], myFont, brush, new Point(4, 1));
                }
            }
        }
        #endregion
        

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        /// <summary>
        /// Изменение текущего элемента извне контрола.
        /// </summary>
        /// <param name="selectedLetter"></param>
        public void ChangeCurrentItem(String selectedLetter)
        {
            if (alphaList == null) 
                return;
            
            int index = alphaList.IndexOf(selectedLetter);
            if (index >= 0)
            {
                // Второй параметр false означает, что не нужно генерировать событие
                SelectItem(pb[index], false);
            }
        }

        /// <summary>
        /// Получение списка элементов из DataTable.
        /// </summary>
        /// <param name="dt"></param>
        public void FillAlphaList(DataTable dt)
        {
            List<string> list = new List<string>();
            //String prevStr = "?";
            foreach (DataRow dr in dt.Rows)
            {
                if (!String.IsNullOrEmpty(dr["NAME"].ToString()))
                {
                    String currStr = dr["NAME"].ToString().Substring(0, 1).ToUpper();
                    //if (currStr != prevStr)
                    if (!list.Contains(currStr))
                    {
                        list.Add(currStr);
                        //prevStr = currStr;
                    }
                }
            }
            list.Sort();
            this.AlphaList = list;
        }

        /// <summary>
        /// Очистка контролов с панели.
        /// </summary>
        private void ClearPB()
        {
            foreach (PictureBox pictBox in this.pb)
            {
                this.Controls.Remove(pictBox);
            }
            this.pb.Clear();
        }


        // Из ListForm.cs
        /*
        private void alphaPanel_AlphaSelected(object sender, EventArgs e)
        {
            SearchWord(sender.ToString());
        }
        */

        /// <summary>
        /// Скрыть/Показать алфавитную панель.
        /// </summary>
        /*
        private void showAlphaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _wordsDataGridView.Invalidate();
            //alphaPanel.Invalidate();

            //alphaPanel.Visible = showAlphaToolStripMenuItem.Checked;
            //if (!alphaPanel.Visible)
            //{
                //_wordsDataGridView.Location = new Point(0, _wordsDataGridView.Location.Y);
                //_wordsDataGridView.Size = new Size(_wordsDataGridView.Width + alphaPanel.Width, _wordsDataGridView.Height);
            //}
            //else
            //{
                //_wordsDataGridView.Location = new Point(alphaPanel.Width, _wordsDataGridView.Location.Y);
                //_wordsDataGridView.Size = new Size(_wordsDataGridView.Width - alphaPanel.Width, _wordsDataGridView.Height);
            //}

            _wordsDataGridView.Focus();
        }
        */
    }
}
