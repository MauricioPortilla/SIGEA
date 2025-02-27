﻿using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace SIGEABD {
    public partial class Comite {
        /// <summary>
        /// Registra el comité en la base de datos.
        /// </summary>
        /// <returns>true si se registró; false si no</returns>
        public bool Registrar() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    foreach (var organizador in Organizadores) {
                        sigeaBD.Organizador.Attach(organizador);
                    }
                    sigeaBD.Comite.Add(this);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Comite->Registrar() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Comite->Registrar() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Comite->Registrar() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Asigna un Organizador al Comité.
        /// </summary>
        /// <param name="organizador">Organizador que se asignará</param>
        /// <returns>true si se asignó con éxito; false si no</returns>
        public bool AsignarOrganizador(Organizador organizador) {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var comite = sigeaBD.Comite.Find(id_comite);
                    sigeaBD.Organizador.Attach(organizador);
                    comite.Organizadores.Add(organizador);
                    return sigeaBD.SaveChanges() != 0;
                }
            } catch (DbUpdateException dbUpdateException) {
                Console.WriteLine("DbUpdateException@Comite->AsignarOrganizador() -> " + dbUpdateException.Message);
                throw;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@Comite->AsignarOrganizador() -> " + entityException.Message);
                throw;
            } catch (Exception exception) {
                Console.WriteLine("Exception@Comite->AsignarOrganizador() -> " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Retorna el nombre del comité.
        /// </summary>
        /// <returns>Nombre del comité</returns>
        public override string ToString() {
            return nombre;
        }
    }
}
