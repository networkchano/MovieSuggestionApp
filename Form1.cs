using System;
using System.Windows.Forms;

namespace MovieSuggestionApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Application.Run(new MainForm()); // Make sure MainForm is your main form class
            public Form1()
        {
            InitializeComponent();

            LoadMovies();
        }
    }
    }
}
