namespace Simulación{
    class PuestoMdsti{
        
        private double tiempoSalida;
        private double sumatoriaTiempoOcioso;
        private double inicioTiempoOcioso;

        public PuestoMdsti(double t){
            this.tiempoSalida = t;
            this.sumatoriaTiempoOcioso = 0;
            this.inicioTiempoOcioso = 0;
        }
        public double getTiempoSalida(){
            return this.tiempoSalida;
        }
        public void setTiempoSalida(double t){
            this.tiempoSalida = t;
        }
        public double getSumatoriaTiempoOcioso(){
            return this.sumatoriaTiempoOcioso;
        }
        public void setSumatoriaTiempoOcioso(double t){
            this.sumatoriaTiempoOcioso = t;
        }
        public double getInicioTiempoOcioso(){
            return this.inicioTiempoOcioso;
        }

        public void setInicioTiempoOcioso(double t){
            this.inicioTiempoOcioso = t;
        }
        public void sumarTiempoOcioso(double t){
            this.sumatoriaTiempoOcioso = this.sumatoriaTiempoOcioso + t;
        }
    }    
}