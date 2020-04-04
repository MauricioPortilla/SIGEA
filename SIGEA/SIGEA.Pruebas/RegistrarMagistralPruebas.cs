using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarMagistralPruebas {
        [TestMethod]
        public void RegistrarMAgistralPrueba () {
            Magistral magistral = new Magistral {
                nombre = "Juan Carlos",
                paterno = "Suarez",
                materno = "Hernández",
                correo = "juancasu_900@hotmail.com",
                telefono = "2283085074",
                lugarOrigen = "Coatepec, Veracruz"
            };
            Assert.IsTrue(magistral.Registrar());
        }
    }
}
