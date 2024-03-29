namespace SRWords
{
    partial class InitOnlineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitOnlineForm));
            this._panel = new System.Windows.Forms.Panel();
            this._loginGroupBox = new System.Windows.Forms.GroupBox();
            this._loginTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._loginLabel = new System.Windows.Forms.Label();
            this._registrationButton = new System.Windows.Forms.Button();
            this._emailLabel = new System.Windows.Forms.Label();
            this._emailTextBox = new System.Windows.Forms.TextBox();
            this._keyGroupBox = new System.Windows.Forms.GroupBox();
            this._keyLabel = new System.Windows.Forms.Label();
            this._checkKeyButton = new System.Windows.Forms.Button();
            this._keyTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this._codeTextBox = new System.Windows.Forms.TextBox();
            this._codeLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._infoTextBox = new System.Windows.Forms.TextBox();
            this._panel.SuspendLayout();
            this._loginGroupBox.SuspendLayout();
            this._keyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panel.BackColor = System.Drawing.SystemColors.Control;
            this._panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panel.Controls.Add(this._loginGroupBox);
            this._panel.Controls.Add(this._keyGroupBox);
            this._panel.Location = new System.Drawing.Point(11, 12);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(517, 227);
            this._panel.TabIndex = 0;
            // 
            // _loginGroupBox
            // 
            this._loginGroupBox.Controls.Add(this._loginTextBox);
            this._loginGroupBox.Controls.Add(this.label2);
            this._loginGroupBox.Controls.Add(this._loginLabel);
            this._loginGroupBox.Controls.Add(this._registrationButton);
            this._loginGroupBox.Controls.Add(this._emailLabel);
            this._loginGroupBox.Controls.Add(this._emailTextBox);
            this._loginGroupBox.Location = new System.Drawing.Point(6, 3);
            this._loginGroupBox.Name = "_loginGroupBox";
            this._loginGroupBox.Size = new System.Drawing.Size(509, 114);
            this._loginGroupBox.TabIndex = 16;
            this._loginGroupBox.TabStop = false;
            // 
            // _loginTextBox
            // 
            this._loginTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._loginTextBox.Location = new System.Drawing.Point(164, 54);
            this._loginTextBox.Name = "_loginTextBox";
            this._loginTextBox.Size = new System.Drawing.Size(236, 20);
            this._loginTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(366, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "ВНИМАНИЕ! Для регистрациии необходимо подключение к интернету";
            // 
            // _loginLabel
            // 
            this._loginLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._loginLabel.AutoSize = true;
            this._loginLabel.Location = new System.Drawing.Point(6, 57);
            this._loginLabel.Name = "_loginLabel";
            this._loginLabel.Size = new System.Drawing.Size(150, 13);
            this._loginLabel.TabIndex = 2;
            this._loginLabel.Text = "Логин (от 5 до 10 символов)";
            // 
            // _registrationButton
            // 
            this._registrationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._registrationButton.Location = new System.Drawing.Point(410, 53);
            this._registrationButton.Name = "_registrationButton";
            this._registrationButton.Size = new System.Drawing.Size(87, 23);
            this._registrationButton.TabIndex = 4;
            this._registrationButton.Text = "Регистрация";
            this._registrationButton.UseVisualStyleBackColor = true;
            this._registrationButton.Click += new System.EventHandler(this._registrationButton_Click);
            // 
            // _emailLabel
            // 
            this._emailLabel.AutoSize = true;
            this._emailLabel.Location = new System.Drawing.Point(6, 28);
            this._emailLabel.Name = "_emailLabel";
            this._emailLabel.Size = new System.Drawing.Size(55, 13);
            this._emailLabel.TabIndex = 0;
            this._emailLabel.Text = "Ваш email";
            // 
            // _emailTextBox
            // 
            this._emailTextBox.Location = new System.Drawing.Point(67, 25);
            this._emailTextBox.Name = "_emailTextBox";
            this._emailTextBox.Size = new System.Drawing.Size(333, 20);
            this._emailTextBox.TabIndex = 1;
            // 
            // _keyGroupBox
            // 
            this._keyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._keyGroupBox.Controls.Add(this._keyLabel);
            this._keyGroupBox.Controls.Add(this._checkKeyButton);
            this._keyGroupBox.Controls.Add(this._keyTextBox);
            this._keyGroupBox.Location = new System.Drawing.Point(6, 131);
            this._keyGroupBox.Name = "_keyGroupBox";
            this._keyGroupBox.Size = new System.Drawing.Size(509, 80);
            this._keyGroupBox.TabIndex = 7;
            this._keyGroupBox.TabStop = false;
            // 
            // _keyLabel
            // 
            this._keyLabel.AutoSize = true;
            this._keyLabel.Location = new System.Drawing.Point(6, 21);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(33, 13);
            this._keyLabel.TabIndex = 0;
            this._keyLabel.Text = "Ключ";
            // 
            // _checkKeyButton
            // 
            this._checkKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._checkKeyButton.Location = new System.Drawing.Point(366, 44);
            this._checkKeyButton.Name = "_checkKeyButton";
            this._checkKeyButton.Size = new System.Drawing.Size(109, 23);
            this._checkKeyButton.TabIndex = 9;
            this._checkKeyButton.Text = "Проверить ключ";
            this._checkKeyButton.UseVisualStyleBackColor = true;
            this._checkKeyButton.Click += new System.EventHandler(this._checkKeyButton_Click);
            // 
            // _keyTextBox
            // 
            this._keyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._keyTextBox.Location = new System.Drawing.Point(53, 18);
            this._keyTextBox.Name = "_keyTextBox";
            this._keyTextBox.Size = new System.Drawing.Size(422, 20);
            this._keyTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "---";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(155, 285);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 20);
            this.textBox1.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(80, 283);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Прочитать";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(11, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Записать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _codeTextBox
            // 
            this._codeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._codeTextBox.Location = new System.Drawing.Point(171, 245);
            this._codeTextBox.Name = "_codeTextBox";
            this._codeTextBox.ReadOnly = true;
            this._codeTextBox.Size = new System.Drawing.Size(333, 20);
            this._codeTextBox.TabIndex = 6;
            // 
            // _codeLabel
            // 
            this._codeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._codeLabel.AutoSize = true;
            this._codeLabel.Location = new System.Drawing.Point(13, 248);
            this._codeLabel.Name = "_codeLabel";
            this._codeLabel.Size = new System.Drawing.Size(104, 13);
            this._codeLabel.TabIndex = 5;
            this._codeLabel.Text = "Персональный код";
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(453, 453);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Закрыть";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _infoTextBox
            // 
            this._infoTextBox.BackColor = System.Drawing.SystemColors.Control;
            this._infoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this._infoTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this._infoTextBox.Location = new System.Drawing.Point(11, 374);
            this._infoTextBox.Multiline = true;
            this._infoTextBox.Name = "_infoTextBox";
            this._infoTextBox.ReadOnly = true;
            this._infoTextBox.Size = new System.Drawing.Size(425, 102);
            this._infoTextBox.TabIndex = 1;
            this._infoTextBox.Text = resources.GetString("_infoTextBox.Text");
            // 
            // InitOnlineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(539, 483);
            this.Controls.Add(this._infoTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._panel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this._codeTextBox);
            this.Controls.Add(this._codeLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitOnlineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация";
            this.Load += new System.EventHandler(this.InitOnlineForm_Load);
            this._panel.ResumeLayout(false);
            this._loginGroupBox.ResumeLayout(false);
            this._loginGroupBox.PerformLayout();
            this._keyGroupBox.ResumeLayout(false);
            this._keyGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.TextBox _loginTextBox;
        private System.Windows.Forms.Label _loginLabel;
        private System.Windows.Forms.Label _codeLabel;
        private System.Windows.Forms.TextBox _codeTextBox;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TextBox _keyTextBox;
        private System.Windows.Forms.GroupBox _keyGroupBox;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.Button _checkKeyButton;
        private System.Windows.Forms.TextBox _infoTextBox;
        private System.Windows.Forms.TextBox _emailTextBox;
        private System.Windows.Forms.Label _emailLabel;
        private System.Windows.Forms.Button _registrationButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox _loginGroupBox;
    }
}