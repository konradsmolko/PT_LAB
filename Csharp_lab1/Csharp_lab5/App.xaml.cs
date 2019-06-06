using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Csharp_lab5
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int Newton()
        {
            Tuple<int, int> values;

            Thread.Sleep(20); // use this for fibonacci!

            // 1A
            Task<int> silnia_K = Task.Factory.StartNew<int>(
                (obj) =>
                {
                    Tuple<int, int> vals = (Tuple<int, int>)obj;

                    int ret = vals.Item1 / vals.Item2;

                    return ret;
                },
                100
            );
            return 0;
        }
    }
}
