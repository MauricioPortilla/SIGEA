namespace SIGEABD {

    public partial class Presentacion {
        /// <summary>
        /// Retorna la fecha, la hora de inicio y la hora de fin.
        /// </summary>
        /// <returns>Fecha, hora de inicio y hora de fin</returns>
        public override string ToString() {
            return fechaPresentacion + ", "+ horaInicio + " - " + horaFin;
        }
    }
}
