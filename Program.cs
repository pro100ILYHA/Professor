using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ContactForm());
        }
    }
}
