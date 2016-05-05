using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TestSQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);              
            Application.Run(new frmReadNews());
        }
    }
}
