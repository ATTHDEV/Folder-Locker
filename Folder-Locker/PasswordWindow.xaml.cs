using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {

        RegistryKey folder;

        public string register_id = ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";

        public bool status { get; set; }

        public string path { get; set; }

        public PasswordWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            status = false;
            folder = App.reg_app.OpenSubKey("Folder",true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _p1 = p1.Password;
            string _p2 = p2.Password;
            if (_p1 == _p2 && _p1.Length > 0)
            {
                DirectoryInfo d = new DirectoryInfo(path);
                path = d.Parent.FullName + "\\" + d.Name + register_id;
                d.MoveTo(path);
                folder.SetValue(path, _p1);
                MessageBox.Show("Folder is lock.", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
                status = true;
                Close();
            }
        }

        private void UpdatePassword(object sender, RoutedEventArgs e)
        {
            string _p1 = p1.Password;
            string _p2 = p2.Password;

            if (_p1 != _p2)
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
