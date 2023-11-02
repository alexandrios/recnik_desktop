using System.Windows.Forms;

namespace SRWords
{
	partial class SplashForm : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this._userLabel = new System.Windows.Forms.Label();
            this._verLabel = new System.Windows.Forms.Label();
            this._infoLabel = new System.Windows.Forms.Label();
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this._crossPictureBox = new System.Windows.Forms.PictureBox();
            this._linesTextBox = new System.Windows.Forms.TextBox();
            this._linesPanel = new System.Windows.Forms.Panel();
            this._linesTimer = new System.Windows.Forms.Timer(this.components);
            this._serblangLinkLabel = new System.Windows.Forms.LinkLabel();
            this._emailLinkLabel = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._codeSyncLabel = new System.Windows.Forms.Label();
            this.pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._crossPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.Coral;
            this.lblStatus.Location = new System.Drawing.Point(255, 279);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(226, 14);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
            // 
            // pnlStatus
            // 
            this.pnlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStatus.BackColor = System.Drawing.Color.Transparent;
            this.pnlStatus.Controls.Add(this._codeSyncLabel);
            this.pnlStatus.Controls.Add(this._userLabel);
            this.pnlStatus.Controls.Add(this._verLabel);
            this.pnlStatus.Controls.Add(this._infoLabel);
            this.pnlStatus.Controls.Add(this.lblTimeRemaining);
            this.pnlStatus.ForeColor = System.Drawing.Color.White;
            this.pnlStatus.Location = new System.Drawing.Point(3, 311);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(560, 37);
            this.pnlStatus.TabIndex = 1;
            this.pnlStatus.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
            // 
            // _userLabel
            // 
            this._userLabel.AutoSize = true;
            this._userLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this._userLabel.Location = new System.Drawing.Point(4, 12);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Size = new System.Drawing.Size(35, 13);
            this._userLabel.TabIndex = 2;
            this._userLabel.Text = "label2";
            // 
            // _verLabel
            // 
            this._verLabel.AutoSize = true;
            this._verLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this._verLabel.Location = new System.Drawing.Point(4, 0);
            this._verLabel.Name = "_verLabel";
            this._verLabel.Size = new System.Drawing.Size(35, 13);
            this._verLabel.TabIndex = 1;
            this._verLabel.Text = "label1";
            // 
            // _infoLabel
            // 
            this._infoLabel.AutoSize = true;
            this._infoLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._infoLabel.ForeColor = System.Drawing.Color.Black;
            this._infoLabel.Location = new System.Drawing.Point(252, 9);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new System.Drawing.Size(256, 19);
            this._infoLabel.TabIndex = 0;
            this._infoLabel.Text = "СЕРБСКО-РУССКИЙ СЛОВАРЬ";
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.Black;
            this.lblTimeRemaining.Location = new System.Drawing.Point(165, 11);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(80, 15);
            this.lblTimeRemaining.TabIndex = 2;
            this.lblTimeRemaining.Text = "Time remaining";
            this.lblTimeRemaining.Visible = false;
            this.lblTimeRemaining.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // _crossPictureBox
            // 
            this._crossPictureBox.Image = global::SRWords.Properties.Resources.крестик;
            this._crossPictureBox.Location = new System.Drawing.Point(528, 12);
            this._crossPictureBox.Name = "_crossPictureBox";
            this._crossPictureBox.Size = new System.Drawing.Size(27, 27);
            this._crossPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._crossPictureBox.TabIndex = 3;
            this._crossPictureBox.TabStop = false;
            this._crossPictureBox.Visible = false;
            this._crossPictureBox.Click += new System.EventHandler(this._crossPictureBox_Click);
            this._crossPictureBox.MouseEnter += new System.EventHandler(this._crossPictureBox_MouseEnter);
            this._crossPictureBox.MouseLeave += new System.EventHandler(this._crossPictureBox_MouseLeave);
            // 
            // _linesTextBox
            // 
            this._linesTextBox.Location = new System.Drawing.Point(3, 2);
            this._linesTextBox.Multiline = true;
            this._linesTextBox.Name = "_linesTextBox";
            this._linesTextBox.Size = new System.Drawing.Size(100, 20);
            this._linesTextBox.TabIndex = 5;
            this._linesTextBox.Text = resources.GetString("_linesTextBox.Text");
            this._linesTextBox.Visible = false;
            // 
            // _linesPanel
            // 
            this._linesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._linesPanel.BackColor = System.Drawing.Color.Black;
            this._linesPanel.ForeColor = System.Drawing.Color.White;
            this._linesPanel.Location = new System.Drawing.Point(3, 133);
            this._linesPanel.Name = "_linesPanel";
            this._linesPanel.Size = new System.Drawing.Size(245, 166);
            this._linesPanel.TabIndex = 4;
            // 
            // _linesTimer
            // 
            this._linesTimer.Interval = 35;
            this._linesTimer.Tick += new System.EventHandler(this._linesTimer_Tick);
            // 
            // _serblangLinkLabel
            // 
            this._serblangLinkLabel.ActiveLinkColor = System.Drawing.Color.White;
            this._serblangLinkLabel.AutoSize = true;
            this._serblangLinkLabel.BackColor = System.Drawing.Color.Black;
            this._serblangLinkLabel.DisabledLinkColor = System.Drawing.Color.White;
            this._serblangLinkLabel.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this._serblangLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._serblangLinkLabel.LinkColor = System.Drawing.Color.White;
            this._serblangLinkLabel.Location = new System.Drawing.Point(439, 242);
            this._serblangLinkLabel.Name = "_serblangLinkLabel";
            this._serblangLinkLabel.Size = new System.Drawing.Size(121, 42);
            this._serblangLinkLabel.TabIndex = 8;
            this._serblangLinkLabel.TabStop = true;
            this._serblangLinkLabel.Text = "serblang.ru\r\nОбучение языку";
            this.toolTip1.SetToolTip(this._serblangLinkLabel, "Обучение сербскому языку");
            this._serblangLinkLabel.UseCompatibleTextRendering = true;
            this._serblangLinkLabel.UseMnemonic = false;
            this._serblangLinkLabel.VisitedLinkColor = System.Drawing.Color.White;
            this._serblangLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._serblangLinkLabel_LinkClicked);
            // 
            // _emailLinkLabel
            // 
            this._emailLinkLabel.ActiveLinkColor = System.Drawing.Color.White;
            this._emailLinkLabel.AutoSize = true;
            this._emailLinkLabel.BackColor = System.Drawing.Color.Black;
            this._emailLinkLabel.DisabledLinkColor = System.Drawing.Color.White;
            this._emailLinkLabel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._emailLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._emailLinkLabel.LinkColor = System.Drawing.Color.White;
            this._emailLinkLabel.Location = new System.Drawing.Point(439, 209);
            this._emailLinkLabel.Name = "_emailLinkLabel";
            this._emailLinkLabel.Size = new System.Drawing.Size(111, 23);
            this._emailLinkLabel.TabIndex = 9;
            this._emailLinkLabel.TabStop = true;
            this._emailLinkLabel.Text = "alex27@mail.ru";
            this.toolTip1.SetToolTip(this._emailLinkLabel, "Написать разработчику");
            this._emailLinkLabel.UseCompatibleTextRendering = true;
            this._emailLinkLabel.UseMnemonic = false;
            this._emailLinkLabel.VisitedLinkColor = System.Drawing.Color.White;
            this._emailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._emailLinkLabel_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 300;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // _codeSyncLabel
            // 
            this._codeSyncLabel.AutoSize = true;
            this._codeSyncLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this._codeSyncLabel.Location = new System.Drawing.Point(4, 24);
            this._codeSyncLabel.Name = "_codeSyncLabel";
            this._codeSyncLabel.Size = new System.Drawing.Size(35, 13);
            this._codeSyncLabel.TabIndex = 3;
            this._codeSyncLabel.Text = "label3";
            // 
            // SplashForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.LightGray;
            this.BackgroundImage = global::SRWords.Properties.Resources.splash;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(567, 357);
            this.Controls.Add(this._emailLinkLabel);
            this.Controls.Add(this._serblangLinkLabel);
            this.Controls.Add(this._linesTextBox);
            this.Controls.Add(this._linesPanel);
            this.Controls.Add(this._crossPictureBox);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlStatus);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.DoubleClick += new System.EventHandler(this.SplashScreen_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplashForm_MouseDown);
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._crossPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblTimeRemaining;
		private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.Panel pnlStatus;
        public PictureBox _crossPictureBox;
        private TextBox _linesTextBox;
        private Panel _linesPanel;
        private Timer _linesTimer;
        private Label _infoLabel;
        private Label _userLabel;
        private Label _verLabel;
        private LinkLabel _emailLinkLabel;
        private ToolTip toolTip1;
        private LinkLabel _serblangLinkLabel;
        private Label _codeSyncLabel;
        //        private RunLine runLine1;
    }
}