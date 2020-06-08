using System;
using System.Data.Entity.Core;
using System.Linq;

namespace SIGEABD {
    public partial class Cuenta {
        /// <summary>
        /// Verifica si existe en la base de datos una cuenta con los datos especificados.
        /// </summary>
        /// <param name="usuario">Usuario de la cuenta</param>
        /// <param name="contrasenia">Contraseña de la cuenta</param>
        /// <param name="cuentaEncontrada">Cuenta encontrada en la base de datos</param>
        /// <returns>true si encontró una cuenta; false si no</returns>
        public static void IniciarSesion(string usuario, string contrasenia, Action<Cuenta> callbackExitoso) {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    Cuenta cuentaBuscada = sigeaBD.Cuenta.ToList().Find((cuenta) => {
                        return cuenta.usuario == usuario && cuenta.contrasenia == contrasenia;
                    });
                    callbackExitoso(cuentaBuscada);
                } catch (EntityException exception) {
                    Console.WriteLine("EntityException@Cuenta->IniciarSesion() -> " + exception.Message);
                    throw;
                }
            }
        }
    }
}
