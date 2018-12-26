using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FolderLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string current_path;
        RegistryKey folder_locker;

        public MainWindow()
        {
            InitializeComponent();
            folder_locker = Registry.CurrentUser.OpenSubKey(@"Software\Folder Locker", true);
            var path = folder_locker.GetValue("path");
            if (!path.Equals(""))
            {
                status_label.Visibility = Visibility.Visible;
                current_path = path.ToString();
                if (current_path.LastIndexOf(".{") == -1)
                {
                    textBox1.Content = current_path;
                    status_label.Content = "folder is unlock.";
                    button1.Content = "Lock";
                }
                else
                {
                    textBox1.Content = current_path.Substring(0, current_path.LastIndexOf("."));
                    status_label.Content = "folder is lock.";
                    button1.Content = "Unlock";
                }
            }
            else
            {
                status_label.Content = "";
            }
            //initRegister();
        }

        private void initRegister()
        {
            var folder_locker = Registry.ClassesRoot.OpenSubKey(@"Folder\Shell\Folder Locker", true);
            if (folder_locker == null)
            {
                folder_locker = Registry.ClassesRoot.CreateSubKey(@"Folder\Shell\Folder Locker");
            }
            var command = folder_locker.OpenSubKey(@"command", true);
            if (command == null)
            {
                command = folder_locker.CreateSubKey(@"command");
            }
            command.SetValue("", System.Reflection.Assembly.GetExecutingAssembly().Location + " %1");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = textBox1.Content.ToString();
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (!t.Equals(""))
            {
                dialog.SelectedPath = System.IO.Path.GetFullPath(t).Replace(@"\\", @"\");
            }
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                current_path = System.IO.Path.GetFullPath(dialog.SelectedPath).Replace(@"\", @"\\");
                if (current_path.LastIndexOf(".{") == -1)
                {
                    textBox1.Content = current_path;
                    button1.Content = "Lock";
                    status_label.Content = "folder is unlock.";
                }
                else
                {
                    textBox1.Content = current_path.Substring(0, current_path.LastIndexOf("."));
                    button1.Content = "Unlock";
                    status_label.Content = "folder is lock.";
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (current_path.LastIndexOf(".{") == -1)
            {
                string path = current_path;
                var p = new PasswordWindow(path);
                p.ShowDialog();
                if (p.status)
                {
                    current_path = p.path;
                    button1.Content = "Unlock";
                    folder_locker.SetValue("path", current_path);
                    status_label.Content = "folder is lock.";
                }
            }
            else
            {
                var c = new CheckPasswordWindow(current_path, false);
                c.ShowDialog();
                if (c.status)
                {
                    DirectoryInfo d = new DirectoryInfo(current_path);
                    var folder = c.GetRegistryKey();
                    folder.DeleteValue(current_path);
                    current_path = current_path.Substring(0, current_path.LastIndexOf("."));
                    d.MoveTo(current_path);
                    folder_locker.SetValue("path", current_path);
                    MessageBox.Show("Folder is unlock.", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
                    button1.Content = "Lock";
                    status_label.Content = "folder is unlock.";
                   
                }
            }
        }
    }
}