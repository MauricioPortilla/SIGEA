﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Articulo {
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Articulo.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Articulo->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Articulo->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Adscripcion->Registrar() -> " + exception.Message);
                throw;
            }
        }
    }
}
