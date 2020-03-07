using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class IniciarSesionPruebas {
        /// <summary>
        /// Prueba que verifica que permite el inicio de sesión con una cuenta existente.
        /// </summary>
        [TestMethod]
        public void IniciarSesionCuentaExistentePrueba() {
            string usuario = "Test";
            string contrasenia = Herramientas.EncriptarConSHA512("1234");
            Assert.IsTrue(Cuenta.IniciarSesion(usuario, contrasenia, out _));
        }

        /// <summary>
        /// Prueba que verifica que no permite el inicio de sesión con una cuenta inexistente.
        /// </summary>
        [TestMethod]
        public void IniciarSesionCuentaNoExistentePrueba() {
            string usuario = "Test2";
            string contrasenia = Herramientas.EncriptarConSHA512("12345");
            Assert.IsFalse(Cuenta.IniciarSesion(usuario, contrasenia, out _));
        }
    }
}
