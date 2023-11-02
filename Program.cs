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
            bool isAdmin = false;
                if (args.Length != 0)
                    isAdmin = (args[0].ToUpper() == "ADMIN");

                if (SerialNum.CompareKey())
                {
                    //GC.KeepAlive(mutex);
                    Application.Run(new ListForm(isAdmin));
                }
                else
                {
                    Application.Run(new InitForm());
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