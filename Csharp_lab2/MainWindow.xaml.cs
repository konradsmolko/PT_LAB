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

        private void menuClicked(object sender, RoutedEventArgs e)
        {
            WC.MenuItem temp;
            try
            {
                temp = sender as WC.MenuItem;
            }
            catch
            {
                return;
            }
            switch (temp.Header.ToString())
            {
                case "_Open":
                    OpenDir();
                        break;
                case "_Exit":
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void OpenDir()
        {
            using (var dialog = new WF.FolderBrowserDialog())
            {
                WF.DialogResult result = dialog.ShowDialog();
                if (true)
                {
                    ShowContents(new WC.TreeViewItem
                    {
                        Name = "TreeRoot",
                        Header = new DirectoryInfo(dialog.SelectedPath).Name,
                        Tag = dialog.SelectedPath
                    });
                }
            }
        }

        private void ShowContents(WC.TreeViewItem root)
        {
            
        }
    }
}
