namespace SRWords
{
    partial class KeyboardForm
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
            this._panel = new System.Windows.Forms.Panel();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.BackColor = System.Drawing.Color.Transparent;
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(294, 174);
            this._panel.TabIndex = 1;
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Interval = 500;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // KeyboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 174);
            this.Controls.Add(this._panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyboardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "  Клавиатура";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyboardForm_FormClosing);
            this.Load += new System.EventHandler(this.KeyboardForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyboardForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.Timer _timer;
    }
}