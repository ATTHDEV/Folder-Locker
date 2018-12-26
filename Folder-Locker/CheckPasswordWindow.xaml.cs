using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FolderLocker
{
    /// <summary>
    /// Interaction logic for CheckPasswordWindow.xaml
    /// </summary>
    public partial class CheckPasswordWindow : Window
    {
        RegistryKey folder;
        string path;
        public bool status { get; set; }
        public string password;
        public bool flag { get; set; }

        public CheckPasswordWindow(string path)
        {
            flag = true;
            InitializeComponent();
            init(path);
        }

        public CheckPasswordWindow(string path, bool flag)
        {
            this.flag = flag;
            InitializeComponent();
            init(path);

        }

        private void init(string path)
        {
            folder = App.reg_app.OpenSubKey("Folder", true);
            this.path = path;
            password = folder.GetValue(path).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var input_password = p.Password;
            if (password.Equals(input_password))
            {
                if (flag)
                {
                    DirectoryInfo d = new DirectoryInfo(path);
                    var current_path = path.Substring(0, path.LastIndexOf("."));
                    d.MoveTo(current_path);
                    //Process.Start(current_path);
                    //System.Threading.Thread.Sleep(500);
                    //DirectoryInfo d2 = new DirectoryInfo(current_path);
                    //d2.MoveTo(path);
                }
                Close();
                status = true;
            }
            else
            {
                MessageBox.Show("Password is not valid.", "Warning !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public RegistryKey GetRegistryKey()
        {
            return folder;
        }

        private void UpdatePassword(object sender, RoutedEventArgs e)
        {
            string input_password = p.Password;
            if (input_password.Equals(""))
            {
                warn.Visibility = Visibility.Hidden;
                status = false;
                return;
            }

            if (input_password != password)
            {
                warn.Visibility = Visibility.Visible;
                status = false;
            }
            else
            {
                warn.Visibility = Visibility.Hidden;
                status = true;
            }

        }
    }
}
