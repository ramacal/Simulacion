using System;
using System.Collections.Generic;
using System.Text;

namespace Simulación
{
    class Fdp
    {
        private double r1;
        private double r2;
        private double x1;
        private double y1;

        private double geometrica(double p, double a, double b)
        {
            return Math.Pow(a, p) * b;
        }

        public double CalcularFdpIA(double a, double b)
        { int ia = -1;          
          while (ia == -1)
          {
              r1 = new Random().NextDouble();
              r2 = new Random().NextDouble();
              x1 = (b - a) * r1 + a;
              y1 = geometrica(0, 0.97, 0.03) * r2;
              if (geometrica(x1, 0.97, 0.03) <= y1)
                    ia = 1;
          }
            return x1;
        }

        public double CalcularFdpTAMDSTI(double a, double b)
        {
            int ia = -1;
            while (ia == -1)
            {
                r1 = new Random().NextDouble();
                r2 = new Random().NextDouble();
                x1 = (b - a) * r1 + a;
                y1 = geometrica(0, 0.9957, 0.0043) * r2;
                if (geometrica(x1, 0.9957, 0.0043) <= y1)
                    ia = 1;
            }
            return x1;
        }

        public double CalcularFdpTASAP(double a, double b)
        {
            int ia = -1;
            while (ia == -1)
            {
                r1 = new Random().NextDouble();
                r2 = new Random().NextDouble();
                x1 = (b - a) * r1 + a;
                y1 = geometrica(0, 0.99567, 0.00433) * r2;
                if (geometrica(x1, 0.99567, 0.00433) <= y1)
                    ia = 1;
            }
            return x1;
        }    
    }
     
}

