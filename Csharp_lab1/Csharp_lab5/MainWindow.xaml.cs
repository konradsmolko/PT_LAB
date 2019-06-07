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

namespace Csharp_lab5
{
    /// <summary>
    /// Logika ulongerakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // this.DNS_return.Text = "Kappa";
            InitializeComponent();
        }

        private void Btn_newton_tasks(object sender, RoutedEventArgs e)
        {
            ulong ret;
            ulong.TryParse(this.Newton_K.Text, out ulong K);
            ulong.TryParse(this.Newton_N.Text, out ulong N);
            if (K > N) return;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ret = App.Newton_tasks(K, N);
            watch.Stop();
            this.time_taken.Content = ((double)watch.ElapsedMilliseconds / 1000).ToString() + " seconds";

            Tasks_amount.Text = ret.ToString();
        }

        private void Btn_newton_delegates(object sender, RoutedEventArgs e)
        {
            
            ulong ret;
            ulong.TryParse(this.Newton_K.Text, out ulong K);
            ulong.TryParse(this.Newton_N.Text, out ulong N);
            if (K > N) return;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ret = App.Newton_delegates(K, N);
            watch.Stop();
            this.time_taken.Content = ((double)watch.ElapsedMilliseconds / 1000).ToString() + " seconds";

            Delegates_amount.Text = ret.ToString();
        }

        private void Btn_newton_async(object sender, RoutedEventArgs e)
        {
            ulong ret;
            ulong.TryParse(this.Newton_K.Text, out ulong K);
            ulong.TryParse(this.Newton_N.Text, out ulong N);
            if (K > N) return;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ret = App.Newton_async(K, N);
            watch.Stop();
            this.time_taken.Content = ((double)watch.ElapsedMilliseconds / 1000).ToString() + " seconds";

            Async_await_amount.Text = ret.ToString();
        }
    }
}
