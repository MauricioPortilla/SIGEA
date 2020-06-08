using System.Collections.Generic;
using SIGEABD;

namespace SIGEA {
    class Sesion {
        /// <summary>
        /// Almacena la información de la Cuenta que inició sesión.
        /// </summary>
        public static Cuenta Cuenta;

        /// <summary>
        /// Almacena la información del Revisor asociado a la Cuenta que inició sesión.
        /// </summary>
        public static Revisor Revisor;

        /// <summary>
        /// Almacena la información del Organizador asociado a la Cuenta que inició sesión.
        /// </summary>
        public static Organizador Organizador;

        /// <summary>
        /// Almacena la información del Evento que se esta utilizando
        /// </summary>
        public static Evento Evento;

        /// <summary>
        /// Almacena la información del Comite que se esta utilizando
        /// </summary>
        public static Comite Comite;

        /// <summary>
        /// Tipos que puede tomar una Actividad.
        /// </summary>
        public static readonly List<string> TIPOS_ACTIVIDAD = new List<string>() {
            "Taller", "Magistrados", "Mesa redonda", "Conferencia"
        };

        /// <summary>
        /// Grados de expertíz que puede tener un Revisor al evaluar un Artículo.
        /// </summary>
        public static readonly Dictionary<int, string> GRADOS_EXPERTIZ = new Dictionary<int, string>() {
            { 1, "Bajo (1)" }, { 2, "Medio (2)" }, { 3, "Alto (3)" }
        };

        /// <summary>
        /// Calificación mínima en una evaluación artículo.
        /// </summary>
        public static readonly int MIN_CALIFICACION_EVALUACION_ARTICULO = 1;

        /// <summary>
        /// Calificación máxima en una evaluación artículo.
        /// </summary>
        public static readonly int MAX_CALIFICACION_EVALUACION_ARTICULO = 3;
    }
}
