using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Organizador {
        /// <summary>
        /// Retorna el nombre completo del organizador.
        /// </summary>
        /// <returns>Nombre completo del organizador</returns>
        public override string ToString() {
            return nombre + " " + paterno + " " + (materno ?? "");
        }
    }
}
