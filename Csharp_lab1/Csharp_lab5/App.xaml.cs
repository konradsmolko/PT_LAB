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
    /// Logika ulongerakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ulong Newton_tasks(ulong K, ulong N)
        {
            Tuple<ulong, ulong> values = Tuple.Create(K, N);
            
            Task<ulong> licznik = Task.Factory.StartNew<ulong>(
                (obj) =>
                {
                    Tuple<ulong, ulong> vals = (Tuple<ulong, ulong>)obj;
                    K = vals.Item1;
                    N = vals.Item2;

                    ulong ret = 1;
                    for (ulong i = N; i > N - K; i--)
                        ret *= i;
                    Thread.Sleep(20);
                    return ret;
                }, values);

            Task<ulong> mianownik = Task.Factory.StartNew<ulong>(
                (obj) =>
                {
                    ulong ret = 1;
                    for (ulong i = K; i > 0; i--)
                    {
                        ret *= i;
                    }
                    Thread.Sleep(20);
                    return ret;
                }, K);

            // Accessing the property's get accessor blocks the calling thread until the asynchronous
            // operation is complete; it is equivalent to calling the Wait method.
            ulong l = licznik.Result;
            ulong m = mianownik.Result;
            return l/m;
        }

        public static ulong silnia(ulong start, ulong end)
        {
            ulong ret = 1;
            for (ulong i = end; i > end - start; i--)
                ret *= i;
            Thread.Sleep(20);
            return ret;
        }

        public static ulong Newton_delegates(ulong K, ulong N)
        {
            Func<ulong, ulong, ulong> op = silnia;
            IAsyncResult result_licznik;
            IAsyncResult result_mianownik;
            result_licznik = op.BeginInvoke(K, N, null, null);
            result_mianownik = op.BeginInvoke(K, K, null, null);

            ulong licznik = op.EndInvoke(result_licznik);
            ulong mianownik = op.EndInvoke(result_mianownik);

            return licznik/mianownik;
        }

        public static ulong Newton_async(ulong K, ulong N)
        {


            return 0;
        }
        //Thread.Sleep(20); // use this for fibonacci!

    }
}
