namespace SIGEABD {
    public partial class Revisor {
        /// <summary>
        /// Retorna el nombre completo del Revisor.
        /// </summary>
        /// <returns>Nombre completo del Revisor</returns>
        public override string ToString() {
            return nombre + " " + paterno + " " + materno ?? "";
        }
    }
}
