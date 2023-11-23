//#define DEMO

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SRWords
{
	// The SplashScreen class definition.  AKO Form
	public partial class SplashForm : Form
	{
		#region Member Variables
		// Threading
		private static SplashForm ms_frmSplash = null;
		private static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .05;
		private double m_dblOpacityDecrement = .08;
		private const int TIMER_INTERVAL = 5;

		// Status and progress bar
		private string m_sStatus;
		private string m_sTimeRemaining;
		private double m_dblCompletionFraction = 0.0;
		private Rectangle m_rProgress;

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		private double m_dblPBIncrementPerTimerInterval = .015;

		// Self-calibration support
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		private ArrayList m_alPreviousCompletionFraction;
		private ArrayList m_alActualTimes = new ArrayList();
		private DateTime m_dtStart;
		private bool m_bFirstLaunch = false;
		private bool m_bDTSet = false;

        private int _volumeDB = Data.VolumeDB();

        // Показывать ли бегущую строку
#if DEMO
        public bool isRunLine = false;
#else
        public bool isRunLine = true;
#endif
        #endregion Member Variables

        /// <summary>
		/// Constructor
		/// </summary>
		public SplashForm()
		{
			InitializeComponent();

#if DEMO
            _userLabel.Text = "";
            _verLabel.Text = "Демо 2.0.0";
#else
            _userLabel.Text = SerialNum.GetLogin();

			int change_id = Data.GetChangeId();
			_codeSyncLabel.Text = "Номер синхронизации " + change_id.ToString();
			
			string db_version = Data.GetDBVersion();
			_verLabel.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "_" + db_version;

			//AssemblyProduct
			// System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			//System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
#endif
			this.Opacity = 0.0; 
			UpdateTimer.Interval = TIMER_INTERVAL;
			UpdateTimer.Start();
			this.ClientSize = this.BackgroundImage.Size;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
		}

		#region Public Static Methods
		// A static method to create the thread and 
		// launch the SplashScreen.
		static public void ShowSplashScreen()
		{
			// Make sure it's only launched once.
			if (ms_frmSplash != null)
				return;
			ms_oThread = new Thread(new ThreadStart(SplashForm.ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.SetApartmentState(ApartmentState.STA);
			ms_oThread.Start();
			while (ms_frmSplash == null || ms_frmSplash.IsHandleCreated == false)
			{
				System.Threading.Thread.Sleep(TIMER_INTERVAL);
			}
		}

		// Close the form without setting the parent.
		static public void CloseForm()
		{
			if (ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
			{
				// Make it start going away.
				ms_frmSplash.m_dblOpacityIncrement = -ms_frmSplash.m_dblOpacityDecrement;
			}
			ms_oThread = null;	// we don't need these any more.
			ms_frmSplash = null;
		}

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}

		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			if (ms_frmSplash == null)
				return;

			ms_frmSplash.m_sStatus = newStatus;

			//if (setReference)
			//	ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if (ms_frmSplash == null)
				return;
			ms_frmSplash.SetReferenceInternal();

		}
		#endregion Public Static Methods

		#region Private Methods

		// A private entry point for the thread.
		static private void ShowForm()
		{
			ms_frmSplash = new SplashForm();
			Application.Run(ms_frmSplash);
		}

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if (m_bDTSet == false)
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
				ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			if (m_alPreviousCompletionFraction != null && m_iIndex < m_alPreviousCompletionFraction.Count)
				m_dblCompletionFraction = (double)m_alPreviousCompletionFraction[m_iIndex++];
			else
				m_dblCompletionFraction = (m_iIndex > 0) ? 1 : 0;
		}

		// Utility function to return elapsed Milliseconds since the 
		// SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}

		// Function to read the checkpoint intervals from the previous invocation of the
		// splashscreen from the XML file.
		private void ReadIncrements()
		{
			string sPBIncrementPerTimerInterval = SplashScreenXMLStorage.Interval;
			double dblResult;

			if (Double.TryParse(sPBIncrementPerTimerInterval, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblResult) == true)
				m_dblPBIncrementPerTimerInterval = dblResult;
			else
				m_dblPBIncrementPerTimerInterval = .0015;

			string sPBPreviousPctComplete = SplashScreenXMLStorage.Percents;

			if (sPBPreviousPctComplete != "")
			{
				string[] aTimes = sPBPreviousPctComplete.Split(null);
				m_alPreviousCompletionFraction = new ArrayList();

				for (int i = 0; i < aTimes.Length; i++)
				{
					double dblVal;
					if (Double.TryParse(aTimes[i], System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblVal) == true)
						m_alPreviousCompletionFraction.Add(dblVal);
					else
						m_alPreviousCompletionFraction.Add(1.0);
				}
			}
			else
			{
				m_bFirstLaunch = true;
				m_sTimeRemaining = "";
			}
		}

		// Method to store the intervals (in percent complete) from the current invocation of
		// the splash screen to XML storage.
		private void StoreIncrements()
		{
			string sPercent = "";
			double dblElapsedMilliseconds = ElapsedMilliSeconds();
			for (int i = 0; i < m_alActualTimes.Count; i++)
				sPercent += ((double)m_alActualTimes[i] / dblElapsedMilliseconds).ToString("0.####", System.Globalization.NumberFormatInfo.InvariantInfo) + " ";

			SplashScreenXMLStorage.Percents = sPercent;

			m_dblPBIncrementPerTimerInterval = 1.0 / (double)m_iActualTicks;

			SplashScreenXMLStorage.Interval = m_dblPBIncrementPerTimerInterval.ToString("#.000000", System.Globalization.NumberFormatInfo.InvariantInfo);
		}

		public static SplashForm GetSplashScreen()
		{
			return ms_frmSplash;
		}

		#endregion Private Methods

		#region Event Handlers
		// Tick Event handler for the Timer control.  Handle fade in and fade out and paint progress bar. 
		private void UpdateTimer_Tick(object sender, System.EventArgs e)
		{
			lblStatus.Text = m_sStatus;

			// Calculate opacity
			if (m_dblOpacityIncrement > 0)		// Starting up splash screen
			{
				m_iActualTicks++;
				if (this.Opacity < 1)
					this.Opacity += m_dblOpacityIncrement;
			}
			else // Closing down splash screen
			{
				if (this.Opacity > 0)
					this.Opacity += m_dblOpacityIncrement;
				else
				{
					StoreIncrements();
					UpdateTimer.Stop();
					this.Close();
				}
			}

			// Paint progress bar
			if (m_bFirstLaunch == false && m_dblLastCompletionFraction < m_dblCompletionFraction)
			{
				m_dblLastCompletionFraction += m_dblPBIncrementPerTimerInterval;
				int width = (int)Math.Floor(pnlStatus.ClientRectangle.Width * m_dblLastCompletionFraction);
				int height = pnlStatus.ClientRectangle.Height;
				int x = pnlStatus.ClientRectangle.X;
				int y = pnlStatus.ClientRectangle.Y;
				if (width > 0 && height > 0)
				{
					m_rProgress = new Rectangle(x, y, width, height);
					if (!pnlStatus.IsDisposed)
					{
						Graphics g = pnlStatus.CreateGraphics();
						LinearGradientBrush brBackground = new LinearGradientBrush(m_rProgress, Color.FromArgb(58, 96, 151), Color.FromArgb(181, 237, 254), LinearGradientMode.Horizontal);
						g.FillRectangle(brBackground, m_rProgress);
						g.Dispose();
					}
					int iSecondsLeft = 1 + (int)(TIMER_INTERVAL * ((1.0 - m_dblLastCompletionFraction) / m_dblPBIncrementPerTimerInterval)) / 1000;
					m_sTimeRemaining = (iSecondsLeft == 1) ? string.Format("1 second remaining") : string.Format("{0} seconds remaining", iSecondsLeft);
				}
			}
			lblTimeRemaining.Text = m_sTimeRemaining;
		}

		// Close the form if they double click on it.
		private void SplashScreen_DoubleClick(object sender, System.EventArgs e)
		{
			// Use the overload that doesn't set the parent form to this very window.
			CloseForm();
		}
		#endregion Event Handlers


        private void _crossPictureBox_Click(object sender, EventArgs e)
        {
            //CloseForm();
            Close();
        }

        private void _crossPictureBox_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void _crossPictureBox_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void SplashForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (_crossPictureBox.Visible)
            {
                base.Capture = false;
                Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref m);
            }
        }

        int maxLab = 0;

        private void _linesTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i <= maxLab; i++)
            {
                int k = _linesPanel.Controls.IndexOfKey("label" + i.ToString());
                (_linesPanel.Controls[k] as Label).Top = (_linesPanel.Controls[k] as Label).Top - 1;

                if (i == maxLab && (_linesPanel.Controls[k] as Label).Bottom < 0)
                    SetToStartPosition();
            }
        }

        private void MakeLabels()
        {
            List<string> lines = new List<string>(_linesTextBox.Lines);

#if DEMO
            lines.Insert(7, "Количество слов");
            lines.Insert(8, "в демонстрационной версии:");
            lines.Insert(9, "<Vol.>.");
            lines.Insert(10, "Полная версия словаря содержит");
            lines.Insert(11, "более 45000 слов.");
#else
            lines.Insert(7, "Количество слов в словаре: <Vol.>.");
#endif
            /*
            lines.Add("");
            lines.Add("Версия словаря 1.0.0");
            lines.Add("Версия оболочки 2.1.0");
            lines.Add("");
            lines.Add("Зарегистрирована для пользователя:");
            lines.Add(SerialNum.GetLogin()); 
            */

            maxLab = lines.Count;
            for (int i = 1; i <= maxLab; i++)
            {
                Label lab = new Label();
                lab.AutoSize = true;
                lab.Name = "label" + i.ToString();
                //lab.Text = _linesTextBox.Lines[i - 1];
                lab.Text = lines[i - 1];
                lab.Text = lab.Text.Replace("<Vol.>", _volumeDB.ToString());
                lab.BackColor = Color.Black;
                lab.ForeColor = Color.WhiteSmoke; // Coral; // White;
                lab.Location = new System.Drawing.Point(7, 88);
                Font normalFont = new Font("Arial", 10.5f, FontStyle.Regular);
                lab.Font = normalFont;
                //lab.Size = new System.Drawing.Size(84, 13);
                lab.TabIndex = i;
                lab.Visible = false;
                _linesPanel.Controls.Add(lab);
            }
        }

        private void SetToStartPosition()
        {
            int startPos = _linesPanel.Height + 5;
            int offset = (_linesPanel.Controls[0] as Label).Font.Height;
            for (int i = 1; i <= maxLab; i++)
            {
                int k = _linesPanel.Controls.IndexOfKey("label" + i.ToString());
                (_linesPanel.Controls[k] as Label).Top = startPos + i * offset;
                (_linesPanel.Controls[k] as Label).Visible = true;
            }
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            if (isRunLine)
            {
                MakeLabels();
                SetToStartPosition();
                _linesTimer.Start();
            }
        }

        private void _serblangLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://serblang.ru");
        }

        private void _emailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("Mailto:alex27@mail.ru"); 
        }
    }

	#region Auxiliary Classes 
	/// <summary>
	/// A specialized class for managing XML storage for the splash screen.
	/// </summary>
	internal class SplashScreenXMLStorage
	{
		private static string ms_StoredValues = "SplashScreen.xml";
		private static string ms_DefaultPercents = "";
		private static string ms_DefaultIncrement = ".015";


		// Get or set the string storing the percentage complete at each checkpoint.
		static public string Percents
		{
			get { return GetValue("Percents", ms_DefaultPercents); }
			set { SetValue("Percents", value); }
		}
		// Get or set how much time passes between updates.
		static public string Interval
		{
			get { return GetValue("Interval", ms_DefaultIncrement); }
			set { SetValue("Interval", value); }
		}

		// Store the file in a location where it can be written with only User rights. (Don't use install directory).
		static private string StoragePath
		{
			get {return Path.Combine(Application.UserAppDataPath, ms_StoredValues);}
		}

		// Helper method for getting inner text of named element.
		static private string GetValue(string name, string defaultValue)
		{
			if (!File.Exists(StoragePath))
				return defaultValue;

			try
			{
				XmlDocument docXML = new XmlDocument();
				docXML.Load(StoragePath);
				XmlElement elValue = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
				return (elValue == null) ? defaultValue : elValue.InnerText;
			}
			catch
			{
				return defaultValue;
			}
		}

		// Helper method for setting inner text of named element.  Creates document if it doesn't exist.
		static public void SetValue(string name,
			 string stringValue)
		{
			XmlDocument docXML = new XmlDocument();
			XmlElement elRoot = null;
			if (!File.Exists(StoragePath))
			{
				elRoot = docXML.CreateElement("root");
				docXML.AppendChild(elRoot);
			}
			else
			{
				docXML.Load(StoragePath);
				elRoot = docXML.DocumentElement;
			}
			XmlElement value = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
			if (value == null)
			{
				value = docXML.CreateElement(name);
				elRoot.AppendChild(value);
			}
			value.InnerText = stringValue;
			docXML.Save(StoragePath);
		}
	}
	#endregion Auxiliary Classes

}

