using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {

    [TestClass]
    public class RegistrarEventoPruebas {

        [TestMethod]
        public void RegistrarEventoPrueba () {

            Evento evento = new Evento {

                 nombre = "Escuela de Verano IS",
                 sede = "Facultad de Estadistica e Informatica ",
                 cuota = 900,
                 fechaInicio = new DateTime(22/05/2020),
                 fechaFin = new DateTime(29/05/2020)
            };

            Assert.IsTrue(evento.Registrar());
        }
    }
}
