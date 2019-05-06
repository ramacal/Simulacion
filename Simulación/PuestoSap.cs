namespace Simulación{
    class PuestoSap{

        public const int HV = 999999;
        private double tiempoSalida;

        public PuestoSap(){
            this.tiempoSalida = HV; //El puesto arranca libre
        }

        public double getTiempoSalida(){
            return this.tiempoSalida;
        }
        public void setTiempoSalida(double t){
            this.tiempoSalida = t;
        }
    }    
}