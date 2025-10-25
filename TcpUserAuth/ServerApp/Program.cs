using System;
using System.Windows.Forms;

namespace ServerApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            

            // Chạy form chính của server
            Application.Run(new FrmServerMain());
        }
    }
}
