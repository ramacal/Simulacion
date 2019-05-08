namespace Simulación{
    class Llegada{
        private double tiempoLlegada;

        public Llegada(double t){
            this.tiempoLlegada = t;
        }
        public double getTiempoLlegada(){
            return this.tiempoLlegada;
        }
        public void setTiempoLlegada(double t){
            this.tiempoLlegada = t;
        }
    }    
}