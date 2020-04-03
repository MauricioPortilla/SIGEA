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
                 sede = "Facultad de estadistica e Informatica",
                 cuota = 900.00,
                 fechaInicio = new DateTime(25/6/2018),
                 fechaFin = new DateTime(2/7/2018)
            };
            Assert.IsTrue(evento.Registrar());
        }
    }
}
