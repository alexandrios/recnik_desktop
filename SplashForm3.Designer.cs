namespace SRWords
{
    partial class SplashForm3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm3));
            this._logoPictureBox = new System.Windows.Forms.PictureBox();
            this._topPanel = new System.Windows.Forms.Panel();
            this._infoLabel = new System.Windows.Forms.Label();
            this._centerPanel = new System.Windows.Forms.Panel();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._panel2 = new System.Windows.Forms.Panel();
            this._numWordsLabel = new System.Windows.Forms.Label();
            this._panel3 = new System.Windows.Forms.Panel();
            this._codeSyncLabel = new System.Windows.Forms.Label();
            this._panel1 = new System.Windows.Forms.Panel();
            this._verLabel = new System.Windows.Forms.Label();
            this._panel4 = new System.Windows.Forms.Panel();
            this._statusLabel = new System.Windows.Forms.Label();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).BeginInit();
            this._topPanel.SuspendLayout();
            this._centerPanel.SuspendLayout();
            this._bottomPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._panel2.SuspendLayout();
            this._panel3.SuspendLayout();
            this._panel1.SuspendLayout();
            this._panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // _logoPictureBox
            // 
            this._logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("_logoPictureBox.Image")));
            this._logoPictureBox.Location = new System.Drawing.Point(98, 0);
            this._logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this._logoPictureBox.Name = "_logoPictureBox";
            this._logoPictureBox.Size = new System.Drawing.Size(201, 200);
            this._logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._logoPictureBox.TabIndex = 11;
            this._logoPictureBox.TabStop = false;
            // 
            // _topPanel
            // 
            this._topPanel.BackColor = System.Drawing.Color.White;
            this._topPanel.Controls.Add(this._infoLabel);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(401, 60);
            this._topPanel.TabIndex = 12;
            // 
            // _infoLabel
            // 
            this._infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._infoLabel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._infoLabel.ForeColor = System.Drawing.Color.Black;
            this._infoLabel.Location = new System.Drawing.Point(0, 0);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new System.Drawing.Size(401, 60);
            this._infoLabel.TabIndex = 1;
            this._infoLabel.Text = "Сербско-русский словарь";
            this._infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _centerPanel
            // 
            this._centerPanel.BackColor = System.Drawing.Color.White;
            this._centerPanel.Controls.Add(this._logoPictureBox);
            this._centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._centerPanel.Location = new System.Drawing.Point(0, 60);
            this._centerPanel.Name = "_centerPanel";
            this._centerPanel.Size = new System.Drawing.Size(401, 199);
            this._centerPanel.TabIndex = 13;
            // 
            // _bottomPanel
            // 
            this._bottomPanel.BackColor = System.Drawing.Color.White;
            this._bottomPanel.Controls.Add(this.tableLayoutPanel1);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 259);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(401, 148);
            this._bottomPanel.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._panel4, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 148);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _panel2
            // 
            this._panel2.Controls.Add(this._numWordsLabel);
            this._panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel2.Location = new System.Drawing.Point(3, 40);
            this._panel2.Name = "_panel2";
            this._panel2.Size = new System.Drawing.Size(395, 31);
            this._panel2.TabIndex = 4;
            // 
            // _numWordsLabel
            // 
            this._numWordsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._numWordsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._numWordsLabel.ForeColor = System.Drawing.Color.Black;
            this._numWordsLabel.Location = new System.Drawing.Point(0, 0);
            this._numWordsLabel.Name = "_numWordsLabel";
            this._numWordsLabel.Size = new System.Drawing.Size(395, 31);
            this._numWordsLabel.TabIndex = 3;
            this._numWordsLabel.Text = "_numWordsLabel";
            this._numWordsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _panel3
            // 
            this._panel3.Controls.Add(this._codeSyncLabel);
            this._panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel3.Location = new System.Drawing.Point(3, 77);
            this._panel3.Name = "_panel3";
            this._panel3.Size = new System.Drawing.Size(395, 31);
            this._panel3.TabIndex = 5;
            // 
            // _codeSyncLabel
            // 
            this._codeSyncLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._codeSyncLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._codeSyncLabel.ForeColor = System.Drawing.Color.Black;
            this._codeSyncLabel.Location = new System.Drawing.Point(0, 0);
            this._codeSyncLabel.Name = "_codeSyncLabel";
            this._codeSyncLabel.Size = new System.Drawing.Size(395, 31);
            this._codeSyncLabel.TabIndex = 4;
            this._codeSyncLabel.Text = "_codeSyncLabel";
            this._codeSyncLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _panel1
            // 
            this._panel1.Controls.Add(this._verLabel);
            this._panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel1.Location = new System.Drawing.Point(3, 3);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(395, 31);
            this._panel1.TabIndex = 6;
            // 
            // _verLabel
            // 
            this._verLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._verLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._verLabel.ForeColor = System.Drawing.Color.Black;
            this._verLabel.Location = new System.Drawing.Point(0, 0);
            this._verLabel.Name = "_verLabel";
            this._verLabel.Size = new System.Drawing.Size(395, 31);
            this._verLabel.TabIndex = 2;
            this._verLabel.Text = "_verLabel";
            this._verLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _panel4
            // 
            this._panel4.Controls.Add(this._statusLabel);
            this._panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel4.Location = new System.Drawing.Point(3, 114);
            this._panel4.Name = "_panel4";
            this._panel4.Size = new System.Drawing.Size(395, 31);
            this._panel4.TabIndex = 7;
            // 
            // _statusLabel
            // 
            this._statusLabel.BackColor = System.Drawing.Color.Transparent;
            this._statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._statusLabel.ForeColor = System.Drawing.Color.Coral;
            this._statusLabel.Location = new System.Drawing.Point(0, 0);
            this._statusLabel.Margin = new System.Windows.Forms.Padding(0);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(395, 31);
            this._statusLabel.TabIndex = 3;
            this._statusLabel.Text = "_statusLabel";
            this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // SplashForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 407);
            this.Controls.Add(this._centerPanel);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this._topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm3";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashForm3_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).EndInit();
            this._topPanel.ResumeLayout(false);
            this._centerPanel.ResumeLayout(false);
            this._centerPanel.PerformLayout();
            this._bottomPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._panel2.ResumeLayout(false);
            this._panel3.ResumeLayout(false);
            this._panel1.ResumeLayout(false);
            this._panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _logoPictureBox;
        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.Panel _centerPanel;
        private System.Windows.Forms.Panel _bottomPanel;
        private System.Windows.Forms.Label _infoLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _verLabel;
        private System.Windows.Forms.Label _statusLabel;
        private System.Windows.Forms.Panel _panel2;
        private System.Windows.Forms.Panel _panel3;
        private System.Windows.Forms.Panel _panel1;
        private System.Windows.Forms.Panel _panel4;
        private System.Windows.Forms.Label _codeSyncLabel;
        private System.Windows.Forms.Label _numWordsLabel;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}