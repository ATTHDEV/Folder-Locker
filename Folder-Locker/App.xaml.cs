using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FolderLocker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RegistryKey reg_app;
        [STAThread]
        public static void Main()
        {
            reg_app = Registry.CurrentUser.OpenSubKey(@"Software\Folder Locker", true);
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = e.Args;

            if (args.Length > 0)
            {
                string path = args[0];
                if (path.LastIndexOf(".{") == -1)
                {
                    var password_window = new PasswordWindow(args[0]);
                    password_window.Show();
                }
                else
                {
                    var p = System.IO.Path.GetFullPath(args[0]).Replace(@"\", @"\\");
                    var check_password_window = new CheckPasswordWindow(p);
                    check_password_window.Show();
                }

            }
            else
            {
                var main_window = new MainWindow();
                main_window.Show();
            }
        }
    }
}
