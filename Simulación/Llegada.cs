namespace Simulación{
    class Llegada{
        private double tiempoLlegada;

        public Llegada(){
            this.tiempoLlegada = 0;
        }
        public double getTiempoLlegada(){
            return this.tiempoLlegada;
        }
        public void setTiempoLlegada(double t){
            this.tiempoLlegada = t;
        }
    }    
}