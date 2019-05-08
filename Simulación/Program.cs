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
            double tiempoFinal = 3600 * 8;
            int NSsap = 0;
            int NSmdsti = 0; 
            double HV = 999999;
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
            while(tiempo < tiempoFinal){
                PuestoMdsti puestoMdstiConMenorSalida = puestosMdsti.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2);
                double menorTPSMdsti = puestoMdstiConMenorSalida.getTiempoSalida();
                PuestoSap puestoSapConMenorSalida = puestosSap.Aggregate((i1,i2) => i1.getTiempoSalida() < i2.getTiempoSalida() ? i1 : i2);
                double menorTPSSap = puestoSapConMenorSalida.getTiempoSalida();
                Llegada llegadaConMenorTiempo = llegadas.Aggregate((i1,i2) => i1.getTiempoLlegada() < i2.getTiempoLlegada() ? i1 : i2);
                double tpll = llegadaConMenorTiempo.getTiempoLlegada();

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

            //PPS1, PPS2, PEC1, PEC2, etc

            //FIN - CALCULO RESULTADOS

            //INICIO - IMPRESION RESULTADOS
            //FIN - RESULTADOS
            
            Console.WriteLine("Fin");
            Escribir.Close();
            Console.ReadLine();
            

        }
    }
}