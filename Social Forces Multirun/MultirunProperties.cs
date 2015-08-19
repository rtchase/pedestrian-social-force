using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Social_Forces_Multirun
{
    public class MultirunProperties
    {
        public int design;

        public int Factor1;
        public int Factor2;
        public int Factor3;
        public int Factor4;
        public double[] Flow1;
        public double[] Flow2;
        public double[] A;
        public double[] B;


        public MultirunProperties()
        {

        }

        public MultirunProperties(double[] flow1,double[] flow2, double[] a, double[]b)
        {
            Flow1 = flow1;
            Flow2 = flow2;
            A = a;
            B = b;

            Factor1 = flow1.Count();
            Factor2 = flow2.Count();
            Factor3 = a.Count();
            Factor4 = b.Count();
        }

        public MultirunProperties(double[] flow1, double[] a, double[] b)
        {
            Flow1 = flow1;
            A = a;
            B = b;
            Factor1 = flow1.Count();
            Factor2 = a.Count();
            Factor3 = b.Count();
        }

    }
}
