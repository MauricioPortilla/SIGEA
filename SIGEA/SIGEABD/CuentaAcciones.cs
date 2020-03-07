using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEABD {
    public partial class Cuenta {
        /// <summary>
        /// Verifica si existe en la base de datos una cuenta con los datos especificados.
        /// </summary>
        /// <param name="usuario">Usuario de la cuenta</param>
        /// <param name="contrasenia">Contraseña de la cuenta</param>
        /// <param name="cuentaEncontrada">Cuenta encontrada en la base de datos</param>
        /// <returns>true si encontró una cuenta; false si no</returns>
        public static bool IniciarSesion(string usuario, string contrasenia, out Cuenta cuentaEncontrada) {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    Cuenta cuentaBuscada = sigeaBD.Cuenta.ToList().Find((cuenta) => {
                        return cuenta.usuario == usuario && cuenta.contrasenia == contrasenia;
                    });
                    if (cuentaBuscada == null) {
                        cuentaEncontrada = null;
                        return false;
                    }
                    cuentaEncontrada = cuentaBuscada;
                    return true;
                } catch (EntityException exception) {
                    Console.WriteLine("EntityException@Cuenta->IniciarSesion -> " + exception.Message);
                    throw;
                }
            }
        }
    }
}
