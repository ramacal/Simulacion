namespace Simulación{
    class PuestoSap{


        private double tiempoSalida;

        public PuestoSap(double t){
            this.tiempoSalida = t; //El puesto arranca libre
        }

        public double getTiempoSalida(){
            return this.tiempoSalida;
        }
        public void setTiempoSalida(double t){
            this.tiempoSalida = t;
        }
    }    
}