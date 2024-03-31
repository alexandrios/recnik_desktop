//#define DEMO

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.IO.Compression;

namespace SRWords
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //bool only;
            //System.Threading.Mutex mutex = new System.Threading.Mutex(true, "SRWords", out only);
            //if (only)
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

#if SQLITE
            Application.Run(new ListForm(true));
#else

#if DEMO
            Application.Run(new ListForm(false));
#else

            bool isSuccess = false;

            bool isAdmin = false;
            if (args.Length != 0)
                isAdmin = (args[0].ToUpper() == "ADMIN");



            //Application.Run(new InitOnlineForm());
            //SerialNum.SaveFails("10");
            
            //
            //SerialNum.SaveKey("ababababababababababababababababababababababababababab");
            //string storedKey = ADSData.MdFiveGetKey();


            if (String.IsNullOrEmpty(SerialNum.GetLogin()) || String.IsNullOrEmpty(SerialNum.GetEmail()))
            {
                Application.Run(new InitOnlineForm(InitOnlineForm.InitType.LOGIN));
            }
            else
            {
                // Есть донат в бд?
                string don = SerialNum.GetDonate();
                int donate = 0;
                int.TryParse(don, out donate);
                if (donate > 0)
                {
                    if (SerialNum.CompareKey())
                    {
                        isSuccess = true;
                        Application.Run(new ListForm(isAdmin, true));
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
                else
                {
                    if (SerialNum.GetFailsInt() > SerialNum.MAX_FAILCOUNT)
                    {
                        // Вход через ввод ключа
                        isSuccess = true;
                        Application.Run(new InitOnlineForm(InitOnlineForm.InitType.KEY));
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }

                if (!isSuccess)
                {
                    SerialNum.DonateResult donateResult = SerialNum.GetDonationInfo();
                    if (donateResult != SerialNum.DonateResult.FAIL)
                    {
                        Application.Run(new ListForm(isAdmin, donateResult == SerialNum.DonateResult.OK));
                    }
                    else
                    {
                        // Вход через ввод ключа
                        Application.Run(new InitOnlineForm(InitOnlineForm.InitType.KEY));
                    }
                }
            }

#endif
#endif
            //GC.KeepAlive(mutex);
            //}
            //else
            //{
            //    MessageBox.Show("Приложение SRWords уже было запущено ранее.");
            //    Application.Exit();
            //}
        }
    }
}