using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Social_Forces_Multirun
{
    public class SimplePedData
    {
        double[] _X = new double[3001];

        public double[] X
        {
            get { return _X; }
            set { _X = value; }
        }
        double[] _Y = new double[3001];

        public double[] Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        int _id = new int();

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        int _entry = new int();

        public int Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }
        int _exit = new int();

        public int Exit
        {
            get { return _exit; }
            set { _exit = value; }
        }
        Ellipse _circle = new Ellipse();

        public Ellipse Circle
        {
            get { return _circle; }
            set { _circle = value; }
        }

        public SimplePedData(int id) 
        {
            _circle.Height = 4;
            _circle.Width = 4;
            _circle.Fill = System.Windows.Media.Brushes.Black;
            //_circle.Visibility = System.Windows.Visibility.Hidden;
            _id = id;
        }

    }
}
