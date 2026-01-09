using System;
using System.Windows.Forms;

namespace TowerDefenseWinforms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "ThreadException");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(e.ExceptionObject?.ToString() ?? "Unknown", "UnhandledException");
            };

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Main Catch");
            }
        }
    }
}
