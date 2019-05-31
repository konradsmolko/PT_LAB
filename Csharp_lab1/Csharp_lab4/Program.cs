using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace Csharp_lab4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //List<Car> myCars = new List<Car>()
            //{
            //    new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            //    new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            //    new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            //    new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            //    new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            //    new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            //    new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            //    new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            //    new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            //};

            //BindingList<Car> carsBindingList = new BindingList<Car>(myCars);
            //BindingSource carBindingSource = new BindingSource();
            //carBindingSource.DataSource = carsBindingList;
            //dataGridView1.DataSource = carBindingSource;

            //var query_one = myCars.Select((car, index) =>
            //    new {
            //        engineType = (car.Engine.Model == "TDI") ? "diesel" : "petrol",
            //        hppl = car.Engine.HorsePower / car.Engine.Displacement
            //    });

            //var query_two = query_one.GroupBy(
            //    car => car.engineType,
            //    car => car.hppl);

            //foreach (var result in query_two)
            //{
            //    Console.Write(result.Key + ": ");
            //    Console.WriteLine(result.Sum() / result.Count());
            //}

            //var serializer = new XmlSerializer(typeof(List<Car>));
            //var fs = new FileStream("test.xml", FileMode.Create);
            //serializer.Serialize(fs, myCars);
            //fs.Close();
            //fs = new FileStream("test.xml", FileMode.Open);
            //List<Car> theOtherCars = (List<Car>)serializer.Deserialize(fs);
            //fs.Close();

            //Console.WriteLine("Done.");
            //Console.ReadKey();


        }
    }
}
