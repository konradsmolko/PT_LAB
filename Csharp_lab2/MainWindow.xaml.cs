using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WC = System.Windows.Controls;
using WF = System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Csharp_lab2
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is WC.MenuItem temp))
                return;
            switch (temp.Header.ToString())
            {
                case "_Open":
                    OpenDir();
                        break;
                case "_Test":
                    break;
                case "_Exit":
                    Close();
                    break;

                case "Open":
                    OpenItem(treeViewRoot.SelectedItem);
                    break;
                case "Create":
                    CreateItem(treeViewRoot.SelectedItem);
                    break;
                case "Delete":
                    DeleteItem(treeViewRoot.SelectedItem);
                    break;

                default:
                    break;
            }
        }

        private void OpenItem(object selectedItem)
        {
            if (selectedItem is WC.TreeViewItem item && item.Tag is FileSystemInfo fsi)
                System.Diagnostics.Process.Start(@fsi.FullName);
        }

        private void CreateItem(object selectedItem)
        {
            if (!(selectedItem is WC.TreeViewItem item))
                return;
            if (item.Tag is DirectoryInfo dir)
            {

            }
            else if (item.Tag is FileInfo file)
            {

            }
        }

        private void DeleteItem(object selectedItem)
        {
            if (!(selectedItem is WC.TreeViewItem item))
                return;
            if (item.Tag is DirectoryInfo dir)
            {
                DeleteDirectory(dir);
            }
            else if (item.Tag is FileInfo file)
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
                file.Delete();
            }
        }

        private void DeleteDirectory(DirectoryInfo root)
        {
            foreach (var directory in root.GetDirectories())
            {
                DeleteDirectory(directory);
            }
            foreach (var file in root.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
                file.Delete();
            }
            root.Attributes &= ~FileAttributes.ReadOnly;
            root.Delete();
        }

        private void OpenDir()
        {
            using (var dialog = new WF.FolderBrowserDialog() { Description = "Select directory to open" })
            {
                WF.DialogResult result = dialog.ShowDialog();
                if (result == WF.DialogResult.OK)
                {
                    WC.TreeViewItem root = GenerateTree(new DirectoryInfo(dialog.SelectedPath));
                    treeViewRoot.Items.Clear();
                    treeViewRoot.Items.Add(root);
                }
            }
        }

        private  WC.TreeViewItem GenerateTree(DirectoryInfo root)
        {
            WC.TreeViewItem treeViewRoot = new WC.TreeViewItem
            {
                Header = root.Name,
                Tag = root
            };

            foreach (var directory in root.GetDirectories())
            {
                var item = GenerateTree(directory);
                treeViewRoot.Items.Add(item);
            }

            foreach (var file in root.GetFiles())
            {
                var item = new WC.TreeViewItem
                {
                    Header = file.Name,
                    Tag = file
                };
                treeViewRoot.Items.Add(item);
            }

            return treeViewRoot;
        }

        private void TreeContextMenu_ContextMenuOpening(object sender, WC.ContextMenuEventArgs e)
        {
            if (treeViewRoot.SelectedItem != null && treeViewRoot.SelectedItem is WC.TreeViewItem item)
            {
                if (item.Tag is DirectoryInfo)
                {
                    menuOpen.IsEnabled = false;
                    menuCreate.IsEnabled = true;
                }
                else if (item.Tag is FileInfo)
                {
                    menuOpen.IsEnabled = true;
                    menuCreate.IsEnabled = false;
                }
            }
            else
            {
                menuOpen.IsEnabled = false;
                menuCreate.IsEnabled = false;
            }
        }
    }
}
