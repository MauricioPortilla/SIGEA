using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarAutorPruebas {
        /// <summary>
        /// Prueba que verifica que se registra un autor con apellido materno y su adscripción.
        /// </summary>
        [TestMethod]
        public void RegistrarAutorConApellidoMaternoPrueba() {
            Adscripcion adscripcion = new Adscripcion {
                nombreDependencia = "Dependencia 1",
                direccion = "Dirección dependencia 1",
                puesto = "Puesto dependencia 1",
                telefono = "2281909090",
                Autor = new Collection<Autor> {
                    new Autor {
                        nombre = "Nombre autor",
                        paterno = "Paterno autor",
                        materno = "Materno autor",
                        correo = "Correo autor",
                        telefono = "2282898989"
                    }
                }
            };
            Assert.IsTrue(adscripcion.Registrar());
        }

        /// <summary>
        /// Prueba que verifica que se registra un autor con adscripción y sin apellido materno.
        /// </summary>
        [TestMethod]
        public void RegistrarAutorSinApellidoMaternoPrueba() {
            Adscripcion adscripcion = new Adscripcion {
                nombreDependencia = "Dependencia 2",
                direccion = "Dirección dependencia 2",
                puesto = "Puesto dependencia 2",
                telefono = "2281909090",
                Autor = new Collection<Autor> {
                    new Autor {
                        nombre = "Nombre autor 2",
                        paterno = "Paterno autor 2",
                        materno = null,
                        correo = "Correo autor 2",
                        telefono = "2282898989"
                    }
                }
            };
            Assert.IsTrue(adscripcion.Registrar());
        }
    }
}
