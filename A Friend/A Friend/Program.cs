using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Friend
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //Application.ThreadException += myHandler;
            Application.Run(new FormApplication());            
        }

        /*
        static void myHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //do something
        }
        */
    }
}
