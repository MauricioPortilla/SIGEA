using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarAsistentePruebas {
        [TestMethod]
        public void RegistrarAsistentePrueba () {
            Asistente asistente = new Asistente {
                nombre = "Juan Carlos",
                paterno = "Suarez",
                materno = "Hernández",
                correo = "juancasu_900@hotmail.com",
                //Actividad = new Collection<Actividad> ().Add(),
                Adscripcion = new Adscripcion {
                    nombreDependencia = "Universidad Veracruzana",
                    direccion = "3ra de Jesus María #45",
                    telefono = "8164284",
                    puesto = "Estudiante"
                }
            };
        }
    }
}
