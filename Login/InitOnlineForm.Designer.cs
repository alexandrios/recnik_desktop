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
            this._registrationButton = new System.Windows.Forms.Button();
            this._emailTextBox = new System.Windows.Forms.TextBox();
            this._emailLabel = new System.Windows.Forms.Label();
            this._checkKeyButton = new System.Windows.Forms.Button();
            this._groupBox = new System.Windows.Forms.GroupBox();
            this._keyLabel = new System.Windows.Forms.Label();
            this._keyTextBox = new System.Windows.Forms.TextBox();
            this._codeTextBox = new System.Windows.Forms.TextBox();
            this._codeLabel = new System.Windows.Forms.Label();
            this._loginTextBox = new System.Windows.Forms.TextBox();
            this._loginLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._infoTextBox = new System.Windows.Forms.TextBox();
            this._panel.SuspendLayout();
            this._groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panel.BackColor = System.Drawing.SystemColors.Control;
            this._panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panel.Controls.Add(this._registrationButton);
            this._panel.Controls.Add(this._emailTextBox);
            this._panel.Controls.Add(this._emailLabel);
            this._panel.Controls.Add(this._checkKeyButton);
            this._panel.Controls.Add(this._groupBox);
            this._panel.Controls.Add(this._codeTextBox);
            this._panel.Controls.Add(this._codeLabel);
            this._panel.Controls.Add(this._loginTextBox);
            this._panel.Controls.Add(this._loginLabel);
            this._panel.Location = new System.Drawing.Point(11, 12);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(517, 227);
            this._panel.TabIndex = 0;
            // 
            // _registrationButton
            // 
            this._registrationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._registrationButton.Location = new System.Drawing.Point(417, 41);
            this._registrationButton.Name = "_registrationButton";
            this._registrationButton.Size = new System.Drawing.Size(87, 23);
            this._registrationButton.TabIndex = 4;
            this._registrationButton.Text = "Регистрация";
            this._registrationButton.UseVisualStyleBackColor = true;
            this._registrationButton.Click += new System.EventHandler(this._registrationButton_Click);
            // 
            // _emailTextBox
            // 
            this._emailTextBox.Location = new System.Drawing.Point(74, 13);
            this._emailTextBox.Name = "_emailTextBox";
            this._emailTextBox.Size = new System.Drawing.Size(333, 20);
            this._emailTextBox.TabIndex = 1;
            // 
            // _emailLabel
            // 
            this._emailLabel.AutoSize = true;
            this._emailLabel.Location = new System.Drawing.Point(13, 16);
            this._emailLabel.Name = "_emailLabel";
            this._emailLabel.Size = new System.Drawing.Size(55, 13);
            this._emailLabel.TabIndex = 0;
            this._emailLabel.Text = "Ваш email";
            // 
            // _checkKeyButton
            // 
            this._checkKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkKeyButton.Location = new System.Drawing.Point(395, 191);
            this._checkKeyButton.Name = "_checkKeyButton";
            this._checkKeyButton.Size = new System.Drawing.Size(109, 23);
            this._checkKeyButton.TabIndex = 9;
            this._checkKeyButton.Text = "Проверить ключ";
            this._checkKeyButton.UseVisualStyleBackColor = true;
            // 
            // _groupBox
            // 
            this._groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._groupBox.Controls.Add(this._keyLabel);
            this._groupBox.Controls.Add(this._keyTextBox);
            this._groupBox.Location = new System.Drawing.Point(16, 131);
            this._groupBox.Name = "_groupBox";
            this._groupBox.Size = new System.Drawing.Size(486, 48);
            this._groupBox.TabIndex = 7;
            this._groupBox.TabStop = false;
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
            // _keyTextBox
            // 
            this._keyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._keyTextBox.Location = new System.Drawing.Point(53, 18);
            this._keyTextBox.Name = "_keyTextBox";
            this._keyTextBox.Size = new System.Drawing.Size(422, 20);
            this._keyTextBox.TabIndex = 1;
            // 
            // _codeTextBox
            // 
            this._codeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._codeTextBox.Location = new System.Drawing.Point(171, 102);
            this._codeTextBox.Name = "_codeTextBox";
            this._codeTextBox.ReadOnly = true;
            this._codeTextBox.Size = new System.Drawing.Size(333, 20);
            this._codeTextBox.TabIndex = 6;
            // 
            // _codeLabel
            // 
            this._codeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._codeLabel.AutoSize = true;
            this._codeLabel.Location = new System.Drawing.Point(13, 105);
            this._codeLabel.Name = "_codeLabel";
            this._codeLabel.Size = new System.Drawing.Size(104, 13);
            this._codeLabel.TabIndex = 5;
            this._codeLabel.Text = "Персональный код";
            // 
            // _loginTextBox
            // 
            this._loginTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._loginTextBox.Location = new System.Drawing.Point(171, 42);
            this._loginTextBox.Name = "_loginTextBox";
            this._loginTextBox.Size = new System.Drawing.Size(236, 20);
            this._loginTextBox.TabIndex = 3;
            // 
            // _loginLabel
            // 
            this._loginLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._loginLabel.AutoSize = true;
            this._loginLabel.Location = new System.Drawing.Point(13, 45);
            this._loginLabel.Name = "_loginLabel";
            this._loginLabel.Size = new System.Drawing.Size(150, 13);
            this._loginLabel.TabIndex = 2;
            this._loginLabel.Text = "Логин (от 5 до 10 символов)";
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(453, 344);
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
            this._infoTextBox.Location = new System.Drawing.Point(11, 260);
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
            this.ClientSize = new System.Drawing.Size(539, 374);
            this.Controls.Add(this._infoTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._panel);
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
            this._panel.PerformLayout();
            this._groupBox.ResumeLayout(false);
            this._groupBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox _groupBox;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.Button _checkKeyButton;
        private System.Windows.Forms.TextBox _infoTextBox;
        private System.Windows.Forms.TextBox _emailTextBox;
        private System.Windows.Forms.Label _emailLabel;
        private System.Windows.Forms.Button _registrationButton;
    }
}