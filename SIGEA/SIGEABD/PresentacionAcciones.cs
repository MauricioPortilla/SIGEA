using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {

    public partial class Presentacion {
        public override string ToString() {
            return fechaPresentacion + ", "+ horaInicio + " - " + horaFin;
        }
    }
}
