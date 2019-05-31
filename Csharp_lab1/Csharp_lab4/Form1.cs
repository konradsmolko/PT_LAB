using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Car> myCars = new List<Car>()
            {
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            BindingList<Car> carsBindingList = new BindingList<Car>(myCars);
            BindingSource carBindingSource = new BindingSource();
            carBindingSource.DataSource = carsBindingList;
            dataGridView1.DataSource = carBindingSource;

            // ret: ienumerable
            // engineType: A6 && TDI -> diesel; else "petrol"
            // avgHPPL: średnia arytm. {HP/DISP} dla danego engineType
            // orderby avgHPPL descending
            //IEnumerable<string>
            double totalHP =
                (from car in myCars
                 where car.Model == "A6"
                 select car)
                 .Where(car => car.Engine.Model.Contains("TDI"))
                 .Select(car => car.Engine.HorsePower)
                 .Sum();
            //Where(car => car.Engine.Model.Contains("TDI"));

        }
    }

    //public class CustomElement
    //{
    //    private double HPPL;
    //    private string engineType;
    //}

    public class Engine
    {
        private double displacement;
        private double horsePower;
        private string model;

        public Engine() { }

        public Engine(double displacement, int horsePower, string model)
        {
            this.Displacement = displacement;
            this.HorsePower = horsePower;
            this.Model = model;
        }

        public double Displacement
        {
            get { return displacement; }
            set { displacement = value; }
        }
        public double HorsePower
        {
            get { return horsePower; }
            set { horsePower = value; }
        }
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
    }

    public class Car
    {
        private string model;
        private Engine motor;
        private int year;

        public Car() { }

        public Car(string model, Engine motor, int year)
        {
            this.Model = model;
            this.Engine = motor;
            this.Year = year;
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public Engine Engine
        {
            get { return motor; }
            set { motor = value; }
        }
    }
}
