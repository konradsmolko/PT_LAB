using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab1
{
    [Serializable]
    public class FileComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length == y.Length)
                return Comparer.Default.Compare(x, y);
            else
                return x.Length > y.Length ? 1 : -1;
        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(args[0]);
            if (di.Exists)
            {
                Print(di);
                Console.WriteLine("\nNajstarszy plik: " + di.GetOldestDir());
                SortedDictionary<string, long> dict = di.ContentToICollection();
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("DataFile.dat", FileMode.Create))
                try
                {
                    formatter.Serialize(fs, dict);
                    // formatter.Serialize(Console.OpenStandardOutput(), dict); // Nope.
                }
                catch (System.Runtime.Serialization.SerializationException e)
                {
                    Console.WriteLine("Błąd serializacji: " + e.Message);
                }
                dict = null;
                using (FileStream fs = new FileStream("DataFile.dat", FileMode.Open))
                try
                {
                    dict = (SortedDictionary<string, long>) formatter.Deserialize(fs);
                }
                catch (System.Runtime.Serialization.SerializationException e)
                {
                    Console.WriteLine("Błąd deserializacji: " + e.Message);
                }
                if(dict != null)
                    Print(dict);
            }
            else
            {
                Console.WriteLine("ERROR: Folder not found!");
            }
            Console.ReadKey();
        }

        static void Print(DirectoryInfo dir, int depth = 0)
        {
            const string tabbing = "    ";
            string tabs = "";
            for (int i = 0; i < depth; i++)
                tabs += tabbing;
            int size = dir.GetFiles().Length + dir.GetDirectories().Length;
            Console.WriteLine(tabs + dir.Name.ToString() + " (" + size + ") " + dir.GetDOSAttr());
            if (!size.Equals(0))
            {
                tabs += tabbing;
                foreach (FileInfo file in dir.GetFiles())
                {
                    Console.WriteLine(tabs + file.Name.ToString() + " " + file.Length + " bajtow " + file.GetDOSAttr());
                }
            }
            foreach (DirectoryInfo folder in dir.GetDirectories())
            {
                Print(folder, depth + 1);
            }
        }

        static void Print(SortedDictionary<string, long> dict)
        {
            foreach (KeyValuePair<string, long> item in dict)
            {
                Console.WriteLine(item.Key + " -> " + item.Value);
            }
        }

        public static DateTime GetOldestDir(this DirectoryInfo di)
        {
            DateTime oldest = DateTime.Now.ToLocalTime();
            foreach (DirectoryInfo subdi in di.GetDirectories())
            {
                DateTime time = subdi.GetOldestDir();
                if (time < oldest)
                {
                    oldest = time;
                }
            }
            foreach (FileInfo file in di.GetFiles())
            {
                DateTime time = file.LastAccessTime.ToLocalTime();
                if (time < oldest)
                {
                    oldest = time;
                }
            }

            return oldest;
        }

        public static string GetDOSAttr(this FileSystemInfo fsi)
        {
            FileAttributes fattr = fsi.Attributes;
            string dosattr = "";
            
            if ((fattr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                dosattr += "R";
            else
                dosattr += "-";
            if ((fattr & FileAttributes.Archive) == FileAttributes.Archive)
                dosattr += "A";
            else
                dosattr += "-";
            if ((fattr & FileAttributes.System) == FileAttributes.System)
                dosattr += "S";
            else
                dosattr += "-";
            if ((fattr & FileAttributes.Hidden) == FileAttributes.Hidden)
                dosattr += "H";
            else
                dosattr += "-";

            return dosattr;
        }

        public static int CompareTo(this FileSystemInfo fsi)
        {
            return 0;
        }

        public static SortedDictionary<string, long> ContentToICollection(this DirectoryInfo di)
        {
            FileInfo[] files = di.GetFiles();
            DirectoryInfo[] dirs = di.GetDirectories();
            SortedDictionary<string, long> collection = new SortedDictionary<string, long>(new FileComparer());
            foreach (FileInfo item in files)
            {
                if (collection.ContainsKey(item.Name))
                    Console.WriteLine("UWAGA: Pomijam plik: " + item.Name);
                else
                    collection.Add(item.Name, item.Length);
            }
            foreach (DirectoryInfo item in dirs)
            {
                if (collection.ContainsKey(item.Name))
                    Console.WriteLine("UWAGA: Pomijam folder: " + item.Name);
                else
                    collection.Add(item.Name, item.GetFileSystemInfos().LongLength);
            }

            return collection;
        }
    }
}