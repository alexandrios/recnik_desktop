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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitOnlineForm));
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._checkKeyButton = new System.Windows.Forms.Button();
            this._keyLabel = new System.Windows.Forms.Label();
            this._keyTextBox = new System.Windows.Forms.TextBox();
            this._emailInfoButton = new System.Windows.Forms.Button();
            this._loginInfoButton = new System.Windows.Forms.Button();
            this._loginTextBox = new System.Windows.Forms.TextBox();
            this._loginLabel = new System.Windows.Forms.Label();
            this._registrationButton = new System.Windows.Forms.Button();
            this._emailLabel = new System.Windows.Forms.Label();
            this._emailTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._panel = new System.Windows.Forms.Panel();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._keyPanel = new System.Windows.Forms.Panel();
            this._loginPanel = new System.Windows.Forms.Panel();
            this._panel.SuspendLayout();
            this._keyPanel.SuspendLayout();
            this._loginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "1396823_circle_info_information_letter_icon.png");
            // 
            // _checkKeyButton
            // 
            this._checkKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkKeyButton.BackColor = System.Drawing.Color.SteelBlue;
            this._checkKeyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._checkKeyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._checkKeyButton.ForeColor = System.Drawing.Color.White;
            this._checkKeyButton.Location = new System.Drawing.Point(129, 103);
            this._checkKeyButton.Name = "_checkKeyButton";
            this._checkKeyButton.Size = new System.Drawing.Size(138, 33);
            this._checkKeyButton.TabIndex = 3;
            this._checkKeyButton.Text = "Проверка ключа";
            this._checkKeyButton.UseVisualStyleBackColor = false;
            this._checkKeyButton.Click += new System.EventHandler(this._checkKeyButton_Click);
            // 
            // _keyLabel
            // 
            this._keyLabel.AutoSize = true;
            this._keyLabel.Location = new System.Drawing.Point(5, 27);
            this._keyLabel.Name = "_keyLabel";
            this._keyLabel.Size = new System.Drawing.Size(33, 13);
            this._keyLabel.TabIndex = 0;
            this._keyLabel.Text = "Ключ";
            // 
            // _keyTextBox
            // 
            this._keyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._keyTextBox.Location = new System.Drawing.Point(53, 24);
            this._keyTextBox.Name = "_keyTextBox";
            this._keyTextBox.Size = new System.Drawing.Size(327, 20);
            this._keyTextBox.TabIndex = 1;
            // 
            // _emailInfoButton
            // 
            this._emailInfoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._emailInfoButton.FlatAppearance.BorderSize = 0;
            this._emailInfoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this._emailInfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this._emailInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._emailInfoButton.ImageKey = "1396823_circle_info_information_letter_icon.png";
            this._emailInfoButton.ImageList = this._imageList;
            this._emailInfoButton.Location = new System.Drawing.Point(355, 58);
            this._emailInfoButton.Name = "_emailInfoButton";
            this._emailInfoButton.Size = new System.Drawing.Size(23, 23);
            this._emailInfoButton.TabIndex = 5;
            this._emailInfoButton.TabStop = false;
            this._toolTip.SetToolTip(this._emailInfoButton, "Необходимо ввести действующий E-mail адрес.");
            this._emailInfoButton.UseVisualStyleBackColor = false;
            this._emailInfoButton.Click += new System.EventHandler(this._emailInfoButton_Click);
            // 
            // _loginInfoButton
            // 
            this._loginInfoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._loginInfoButton.FlatAppearance.BorderSize = 0;
            this._loginInfoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this._loginInfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this._loginInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._loginInfoButton.ImageKey = "1396823_circle_info_information_letter_icon.png";
            this._loginInfoButton.ImageList = this._imageList;
            this._loginInfoButton.Location = new System.Drawing.Point(355, 24);
            this._loginInfoButton.Name = "_loginInfoButton";
            this._loginInfoButton.Size = new System.Drawing.Size(23, 23);
            this._loginInfoButton.TabIndex = 2;
            this._loginInfoButton.TabStop = false;
            this._toolTip.SetToolTip(this._loginInfoButton, "Длина логина должна быть от 5 до 10 символов.\r\nРазрешены латинские буквы, цифры и" +
        " знак подчеркивания.");
            this._loginInfoButton.UseVisualStyleBackColor = false;
            this._loginInfoButton.Click += new System.EventHandler(this._loginInfoButton_Click);
            // 
            // _loginTextBox
            // 
            this._loginTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this._loginTextBox.Location = new System.Drawing.Point(196, 24);
            this._loginTextBox.Name = "_loginTextBox";
            this._loginTextBox.Size = new System.Drawing.Size(153, 20);
            this._loginTextBox.TabIndex = 1;
            this._loginTextBox.Leave += new System.EventHandler(this._loginTextBox_Leave);
            // 
            // _loginLabel
            // 
            this._loginLabel.AutoSize = true;
            this._loginLabel.Location = new System.Drawing.Point(4, 27);
            this._loginLabel.Name = "_loginLabel";
            this._loginLabel.Size = new System.Drawing.Size(150, 13);
            this._loginLabel.TabIndex = 0;
            this._loginLabel.Text = "Логин (от 5 до 10 символов)";
            // 
            // _registrationButton
            // 
            this._registrationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._registrationButton.BackColor = System.Drawing.Color.SteelBlue;
            this._registrationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._registrationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._registrationButton.ForeColor = System.Drawing.Color.White;
            this._registrationButton.Location = new System.Drawing.Point(128, 103);
            this._registrationButton.Name = "_registrationButton";
            this._registrationButton.Size = new System.Drawing.Size(138, 33);
            this._registrationButton.TabIndex = 6;
            this._registrationButton.Text = "Регистрация";
            this._registrationButton.UseVisualStyleBackColor = false;
            this._registrationButton.Click += new System.EventHandler(this._registrationButton_Click);
            // 
            // _emailLabel
            // 
            this._emailLabel.AutoSize = true;
            this._emailLabel.Location = new System.Drawing.Point(4, 61);
            this._emailLabel.Name = "_emailLabel";
            this._emailLabel.Size = new System.Drawing.Size(35, 13);
            this._emailLabel.TabIndex = 3;
            this._emailLabel.Text = "E-mail";
            // 
            // _emailTextBox
            // 
            this._emailTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this._emailTextBox.Location = new System.Drawing.Point(56, 58);
            this._emailTextBox.Name = "_emailTextBox";
            this._emailTextBox.Size = new System.Drawing.Size(293, 20);
            this._emailTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(42, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Для успешной регистрациии необходим интернет!";
            // 
            // _panel
            // 
            this._panel.BackColor = System.Drawing.Color.White;
            this._panel.Controls.Add(this._loginPanel);
            this._panel.Controls.Add(this.label2);
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(415, 204);
            this._panel.TabIndex = 0;
            // 
            // _keyPanel
            // 
            this._keyPanel.BackColor = System.Drawing.Color.White;
            this._keyPanel.Controls.Add(this._checkKeyButton);
            this._keyPanel.Controls.Add(this._keyLabel);
            this._keyPanel.Controls.Add(this._keyTextBox);
            this._keyPanel.Location = new System.Drawing.Point(14, 36);
            this._keyPanel.Name = "_keyPanel";
            this._keyPanel.Size = new System.Drawing.Size(386, 151);
            this._keyPanel.TabIndex = 1;
            // 
            // _loginPanel
            // 
            this._loginPanel.Controls.Add(this._emailInfoButton);
            this._loginPanel.Controls.Add(this._registrationButton);
            this._loginPanel.Controls.Add(this._emailLabel);
            this._loginPanel.Controls.Add(this._loginInfoButton);
            this._loginPanel.Controls.Add(this._emailTextBox);
            this._loginPanel.Controls.Add(this._loginLabel);
            this._loginPanel.Controls.Add(this._loginTextBox);
            this._loginPanel.Location = new System.Drawing.Point(14, 36);
            this._loginPanel.Name = "_loginPanel";
            this._loginPanel.Size = new System.Drawing.Size(386, 151);
            this._loginPanel.TabIndex = 1;
            // 
            // InitOnlineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(415, 204);
            this.Controls.Add(this._keyPanel);
            this.Controls.Add(this._panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitOnlineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация";
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this._keyPanel.ResumeLayout(false);
            this._keyPanel.PerformLayout();
            this._loginPanel.ResumeLayout(false);
            this._loginPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox _keyTextBox;
        private System.Windows.Forms.Label _keyLabel;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Button _emailInfoButton;
        private System.Windows.Forms.Button _loginInfoButton;
        private System.Windows.Forms.TextBox _loginTextBox;
        private System.Windows.Forms.Label _loginLabel;
        private System.Windows.Forms.Button _registrationButton;
        private System.Windows.Forms.Label _emailLabel;
        private System.Windows.Forms.TextBox _emailTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.Button _checkKeyButton;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Panel _loginPanel;
        private System.Windows.Forms.Panel _keyPanel;
    }
}