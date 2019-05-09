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
            double tiempoFinal = 3600 * 8 * 5 * 4 * 3;
            int NSsap = 0;
            int NSmdsti = 0; 
            double HV = int.MaxValue;
            double stllMdsti = 0;
            double stllSap = 0;
            double stsMdsti = 0;
            double stsSap = 0;
            double staMdsti = 0;
            double staSap = 0;
            int NTSap = 0;
            int NTMdsti = 0;
            int cantArrep = 0;

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
                puestosMdsti.Add(new PuestoMdsti(HV));
            }
            for(int i=0;i<cantPuestosSap;i++){
                //instancio M puestos SAP
                puestosSap.Add(new PuestoSap(HV));
            }

            //Creo la primer llegada
            llegadas.Add(new Llegada(0));

            //INICIO - LOGICA SIMULACION
            while(tiempo < tiempoFinal || NSsap > 0 || NSmdsti > 0){
                PuestoMdsti puestoMdstiConMenorSalida = puestosMdsti.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2);
                double menorTPSMdsti = puestoMdstiConMenorSalida.getTiempoSalida();
                PuestoSap puestoSapConMenorSalida = puestosSap.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2);
                double menorTPSSap = puestoSapConMenorSalida.getTiempoSalida();
                Llegada llegadaConMenorTiempo = llegadas.Aggregate((i1,i2) => i1.getTiempoLlegada() < i2.getTiempoLlegada() ? i1 : i2);
                double tpll = llegadaConMenorTiempo.getTiempoLlegada();

                if(tiempo >= tiempoFinal){
                    //Vaciamiento
                    Console.Write("Salida por vaciamiento: \n");
                    tpll = HV;
                }

                if(tpll <= menorTPSMdsti && tpll <= menorTPSSap){
                    //Llegada
                    tiempo = tpll;

                    double IA = new Fdp().CalcularFdpIA(0,1000);
                    tpll = tiempo + IA;
                    llegadas.Add(new Llegada(tpll));
                    llegadas.Remove(llegadaConMenorTiempo);

                    Random random = new Random();
                    double r = random.NextDouble();

                    double r2 = random.NextDouble(); //para el arrep (10%)              
                    if(r2 >= 0.1){
                        if(r <= 0.1){
                            Console.Write("Entro a sap tiempo: "+tiempo+"\n");
                            NSsap++;
                            NTSap++;
                            stllSap = stllSap + tiempo;

                            if(NSsap <= cantPuestosSap){

                                Console.Write("Me atiendo en sap tiempo: "+tiempo+"\n");
                                PuestoSap puestoLibre = puestosSap.Find(i => i.getTiempoSalida() == HV);
                                double ito = puestoLibre.getInicioTiempoOcioso();
                                puestoLibre.sumarTiempoOcioso(tiempo - ito);
                                double TAsap = new Fdp().CalcularFdpTASAP(30,900);
                                puestoLibre.setTiempoSalida(tiempo + TAsap);
                                staSap = staSap + TAsap;
                            }
                        }else{
                            Console.Write("Entro a mdsti tiempo: "+tiempo+"\n");
                            NSmdsti++;
                            NTMdsti++;
                            stllMdsti = stllMdsti + tiempo;

                            if(NSmdsti <= cantPuestosMdsti){
                                Console.Write("Me atiendo en mdsti tiempo: "+tiempo+"\n");
                                PuestoMdsti puestoLibre = puestosMdsti.Find(i => i.getTiempoSalida() == HV);
                                double ito = puestoLibre.getInicioTiempoOcioso();
                                puestoLibre.sumarTiempoOcioso(tiempo - ito);
                                double TAmdsti = new Fdp().CalcularFdpTAMDSTI(30,3000);
                                puestoLibre.setTiempoSalida(tiempo + TAmdsti);
                                staMdsti = staMdsti + TAmdsti;
                            }
                        }
                    }else{
                        //se arrepiente
                        Console.Write("ME ARREPIENTO en tiempo: "+tiempo+"\n");
                        cantArrep++;
                    }
        

                }else if(menorTPSMdsti <= menorTPSSap){
                    Console.Write("Salgo de mdsti tiempo: "+tiempo+"\n");
                    //Salida de puesto MDSTI
                    tiempo = menorTPSMdsti;
                    NSmdsti--;
                    stsMdsti = stsMdsti + tiempo;

                    if(NSmdsti >= cantPuestosMdsti){
                        double TAMdsti = new Fdp().CalcularFdpTAMDSTI(30,3000);
                        puestoMdstiConMenorSalida.setTiempoSalida(tiempo + TAMdsti);
                        staMdsti = staMdsti + TAMdsti;
                    }else{
                        puestoMdstiConMenorSalida.setInicioTiempoOcioso(tiempo);
                        puestoMdstiConMenorSalida.setTiempoSalida(HV);
                    }


                }else{
                    Console.Write("Salgo de sap tiempo: "+tiempo+"\n");
                    //Salida de puesto SAP
                    tiempo = menorTPSSap;
                    NSsap--;
                    stsSap = stsSap + tiempo;

                    if(NSsap >= cantPuestosSap){
                        double TAsap = new Fdp().CalcularFdpTASAP(30,900);
                        puestoSapConMenorSalida.setTiempoSalida(tiempo + TAsap);
                        staSap = staSap + TAsap;
                    }else{
                        puestoSapConMenorSalida.setInicioTiempoOcioso(tiempo);
                        puestoSapConMenorSalida.setTiempoSalida(HV);
                    }

                }


            }
            //FIN - LOGICA SIMULACION

            //INICIO - CALCULO RESULTADOS
            /*
                staSap: Sumatoria Tiempo Atención SAP.
                staMdsti: Sumatoria Tiempo Atención MDSTI.
                stoSap: Sumatoria Tiempo Ocioso SAP. 
                stoMdsti: Sumatoria Tiempo Ocioso MDSTI.
                stsSap: Sumatoria de salidas SAP.
                stsMdsti: Sumatoria de salidas MDSTI.
             */

            //PPSSAP y PPSMDSTI: Promedio de permanencia en el sistema SAP/MDSTI
            double ppsSap = (stsSap - stllSap) / NTSap;
            double ppsMdsti = (stsMdsti - stllMdsti) / NTMdsti;

            //PECSAP y PECMDSTI: Promedio de espera en cola SAP/MDSTI
            double pecSap = (Math.Round(stsSap,6) - Math.Round(stllSap,6) - Math.Round(staSap,6)) / NTSap;
            double pecMdsti = (Math.Round(stsMdsti,6) - Math.Round(stllMdsti,6) - Math.Round(staMdsti,6)) / NTMdsti;

            
            //double stoSap = puestosSap.Sum(x=> x.getSumatoriaTiempoOcioso());
            //double stoMdsti = puestosMdsti.Sum(x => x.getSumatoriaTiempoOcioso());
            
            //PASAP y PAMDSTI: Porcentaje Arrepentidos respecto del total de personas que ingresaron a atenderse por SAP/MDSTI
            double pa = cantArrep * 100 / (NTSap + cantArrep + NTMdsti);
            //FIN - CALCULO RESULTADOS

            //INICIO - IMPRESION RESULTADOS
            Console.WriteLine(("").PadRight(40, '-'));
            Console.WriteLine("Impresión de Resultados");
            Console.WriteLine(("").PadRight(23, '-') + "\n");
            Console.WriteLine("Promedio de permanencia en el sistema SAP/MDSTI");
            Console.WriteLine(("").PadRight(47, '-'));
            Console.WriteLine("PPSSAP: " + ppsSap);
            Console.WriteLine("PPSMDSTI: " + ppsMdsti + "\n");
            Console.WriteLine("Promedio de espera en cola SAP/MDSTI");
            Console.WriteLine(("").PadRight(47, '-'));
            Console.WriteLine("PECSAP: " + pecSap);
            Console.WriteLine("PECMDSTI: " + pecMdsti + "\n");
            Console.WriteLine("Promedio de Tiempo Ocioso en cola SAP/MDSTI");
            Console.WriteLine(("").PadRight(47, '-'));
            //Console.WriteLine("PTOSAP: " + ptoSap);
            //Console.WriteLine("PTOMDSTI: " + ptoMdsti + "\n");
            Console.WriteLine("Porcentaje Arrepentidos respecto del total de personas que ingresaron a atenderse por SAP/MDSTI");
            Console.WriteLine(("").PadRight(47, '-'));
            Console.WriteLine("PA: " + pa + "\n");
            Console.WriteLine("Cantidad arrepentidos: " + cantArrep + "\n");
            Console.WriteLine("NTSap: " + NTSap + "\n");
            Console.WriteLine("NTMdsti: " + NTMdsti + "\n");

            //PTOSAP y PTOMDSTI: Porcentaje de tiempo ocioso de cada puesto de atención SAP y MDSTI
            int j = 1;
            double pto = 0;
            foreach (PuestoSap pSap in puestosSap){
                pto = (pSap.getSumatoriaTiempoOcioso() * 100)/tiempo;
                Console.WriteLine("PTOsap("+j+"): "+pto+"\n");
                j++;
            }
            j = 1;
            foreach (PuestoMdsti pMdsti in puestosMdsti){
                pto = (pMdsti.getSumatoriaTiempoOcioso() * 100)/tiempo;
                Console.WriteLine("PTOmdsti("+j+"): "+pto+"\n");
                j++;
            }
            //FIN - RESULTADOS

            Console.WriteLine("Fin");
Escribir.Close();
Console.ReadLine();


}
}
}
 