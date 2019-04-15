using System;
using System.IO;

namespace Simulación
{
    class Program
    {
        static StreamReader Leer;
        static StreamWriter Escribir;

        static void Main(string[] args)
        {   //INICIO - INICIALIZACION VARIABLES
            Escribir = new StreamWriter("Resultados.txt", true);
            Console.WriteLine("Trabajo Practico N°6");
            Console.WriteLine(("").PadRight(20, '-'));
            int d1;
            //FIN - INICIALIZACION VARIABLES

            //INICIO - INPUT VARIABLES EXOGENAS
            Console.WriteLine("Variables Exogenas");
            Console.Write("Ingrese Dato 1: ");
            string userInput = Console.ReadLine();
            while (!Int32.TryParse(userInput, out d1))
            {
                Console.WriteLine("Ingrese un valor numerico por favor");
                Console.Write("Ingrese Dato 1: ");
                userInput = Console.ReadLine();
            }

            d1 = Convert.ToInt32(userInput);
            //FIN - INPUT VARIABLES EXOGENAS

            Escribir.WriteLine(d1);
            //INICIO - INPUT VARIABLES ENDOGENAS
            //FIN - INPUT VARIABLES EXOGENAS

            //INICIO - INPUT VARIABLES EXOGENAS
            //FIN - INPUT VARIABLES EXOGENAS

            //INICIO - MOSTRAR VARIABLES INGRESADAS PARA CONTINUAR
            //FIN - MOSTRAR VARIABLES INGRESADAS PARA CONTINUAR

            //INICIO - LOGICA SIMULACION
            //FIN - LOGICA SIMULACION

            //INICIO - IMPRESION RESULTADOS
            Console.WriteLine(d1);
            //FIN - RESULTADOS

            Console.WriteLine("Fin");
            Escribir.Close();
            Console.ReadLine();
            

        }
    }
}