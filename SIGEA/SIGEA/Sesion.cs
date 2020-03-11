using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEABD;

namespace SIGEA {
    class Sesion {
        /// <summary>
        /// Almacena la información de la cuenta que inició sesión.
        /// </summary>
        public static Cuenta Cuenta;

        /// <summary>
        /// Tipos que puede tomar una Actividad.
        /// </summary>
        public static List<string> TiposActividad = new List<string>() {
            "Taller", "Magistrados", "Mesa redonda", "Conferencia"
        };
    }
}
