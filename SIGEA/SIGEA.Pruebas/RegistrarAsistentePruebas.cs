using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;
using System.Collections.ObjectModel;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarAsistentePruebas {
        public ObservableCollection<Actividad>
            ActividadesObservableCollection { get; } =
            new ObservableCollection<Actividad>();

        [TestMethod]
        public void RegistrarAsistentePrueba () {
            ActividadesObservableCollection.Add(new Actividad {
                nombre = "Programacion HG",
                descripcion = "Charla sobre programación basica en P.O.O.",
                tipo = "Conferencia Magistral"
            });

            Asistente asistente = new Asistente {
                nombre = "Juan Carlos",
                paterno = "Suarez",
                materno = "Hernández",
                correo = "juancasu_900@hotmail.com",
                Actividad = ActividadesObservableCollection,
                Adscripcion = new Adscripcion {
                    nombreDependencia = "Universidad Veracruzana",
                    direccion = "3ra de Jesus María #45",
                    telefono = "8164284",
                    puesto = "Estudiante"
                }
            };
            Assert.IsTrue(asistente.Registrar());
        }
    }
}
