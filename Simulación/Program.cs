using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Simulación
{
    class Program
    {
        static StreamReader Leer;
        static StreamWriter Escribir;

        static void Main(string[] args)
        {   //INICIO - INICIALIZACION VARIABLES
            Escribir = new StreamWriter("Resultados.txt", true);
            int cantPuestosSap;
            int cantPuestosMdsti;
            double tiempo = 0;
            double tiempoFinal = 3600;

            List<PuestoSap> puestosSap = new List<PuestoSap>(); 
            List<PuestoMdsti> puestosMdsti = new List<PuestoMdsti>();
            List<Llegada> llegadas = new List<Llegada>();

            Console.WriteLine("Trabajo Practico N°6");
            Console.WriteLine(("").PadRight(20, '-'));
            //FIN - INICIALIZACION VARIABLES

            //INICIO - INPUT VARIABLES EXOGENAS
            Console.WriteLine("Variables Exogenas");
            Console.Write("Ingrese N (cant puestos SAP):");
            string n = Console.ReadLine();

            while (!Int32.TryParse(n, out cantPuestosSap))
            {
                Console.WriteLine("Ingrese un valor numerico por favor\n");
                Console.Write("Ingrese N (cant puestos SAP):");
                n = Console.ReadLine();
            }
            cantPuestosSap = Convert.ToInt32(n);

            Console.Write("Ingrese M (cant puestos MDSTI):");
            string m = Console.ReadLine();

            while (!Int32.TryParse(m, out cantPuestosMdsti))
            {
                Console.WriteLine("Ingrese un valor numerico por favor\n");
                Console.Write("Ingrese M (cant puestos MDSTI):");
                m = Console.ReadLine();
            }

            cantPuestosMdsti = Convert.ToInt32(m);
            //FIN - INPUT VARIABLES EXOGENAS

            //INICIO - MOSTRAR VARIABLES INGRESADAS
            Console.WriteLine("N: "+cantPuestosSap);
            Console.WriteLine("M: "+cantPuestosMdsti);
            //FIN - MOSTRAR VARIABLES INGRESADAS

            for(int i=0;i<cantPuestosMdsti;i++){
                //instancio N puestos MDSTI
                puestosMdsti.Add(new PuestoMdsti());
            }
            for(int i=0;i<cantPuestosSap;i++){
                //instancio M puestos SAP
                puestosSap.Add(new PuestoSap());
            }

            //Creo la primer llegada (el constructor de Llegada le setea por default el tiempo 0)
            llegadas.Add(new Llegada());

            //INICIO - LOGICA SIMULACION
            while(tiempo <= tiempoFinal){
                double menorTPSMdsti = puestosMdsti.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2).getTiempoSalida();
                double menorTPSSap = puestosSap.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2).getTiempoSalida();
                double tpll = llegadas.Aggregate((i1,i2) => i1.getTiempoLlegada() < i2.getTiempoLlegada() ? i1 : i2).getTiempoLlegada();

                if(tpll <= menorTPSMdsti && tpll <= menorTPSSap){
                    //Llegada
                    tiempo = tpll;
    

                }else if(menorTPSMdsti <= menorTPSSap){
                    //Salida de puesto MDSTI
                    tiempo = menorTPSMdsti;


                }else{
                    //Salida de puesto SAP
                    tiempo = menorTPSSap;


                }


            }
            //FIN - LOGICA SIMULACION

            //INICIO - IMPRESION RESULTADOS
            //FIN - RESULTADOS

            Console.WriteLine("Fin");
            Escribir.Close();
            Console.ReadLine();
            

        }
    }
}