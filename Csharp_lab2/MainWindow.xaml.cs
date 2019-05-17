using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            MenuItem temp;
            try
            {
                temp = sender as MenuItem;
            }
            catch
            {
                return;
            }
            switch (temp.Header.ToString())
            {
                case "_Open":

                    break;
                case "_Exit":
                    Close();
                    break;
                default:
                    break;
            }
        }
    }
}
