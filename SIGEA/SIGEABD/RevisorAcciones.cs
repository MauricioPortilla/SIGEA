using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Revisor {
        public override string ToString() {
            return nombre + " " + paterno + " " + materno ?? "";
        }
    }
}
