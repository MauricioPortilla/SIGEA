using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarArticuloPruebas {
        /// <summary>
        /// Prueba que verifica que se registra un artículo y se relaciona con
        /// sus respectivos autores.
        /// </summary>
        [TestMethod]
        public void RegistrarArticuloPrueba() {
            Articulo articulo = new Articulo {
                titulo = "Articulo 1",
                anio = 2020,
                keywords = "software",
                resumen = "Resumen breve",
                id_track = 1,
                archivo = "nombreArchivoCifrado.pdf",
                estado = "Pendiente",
                AutorArticulo = new Collection<AutorArticulo> {
                    new AutorArticulo {
                        id_autor = 1,
                        fecha = DateTime.Now
                    }
                }
            };
            Assert.IsTrue(articulo.Registrar());
        }
    }
}
