using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SIGEA {
    public class Herramientas {

        public static readonly string REGEX_SOLO_NUMEROS = "^[0-9]+$";
        public static readonly string REGEX_SOLO_ENTEROS_Y_FLOTANTES = @"^[-+]?\d*\.?\d{1,}$";
        public static readonly string REGEX_SOLO_LETRAS = "^[a-zA-Záéíóú ]+$";
        public static readonly string REGEX_CORREO = @"\w+@\w+\.\w+$";
        public static readonly string REGEX_FECHA = @"\d{1,2}\/\d{1,2}\/\d{4}$";
        public static readonly string REGEX_HORA = @"\d{2}:\d{2}:\d{2}$";

        /// <summary>
        /// Cifra el texto con cifrado SHA512.
        /// </summary>
        /// <param name="text">Texto a cifrar</param>
        /// <returns>Texto cifrado</returns>
        public static string CifrarConSHA512(string text) {
            return string.Join("", SHA512.Create().ComputeHash(
                Encoding.UTF8.GetBytes(text)
            ).Select(x => x.ToString("x2")).ToArray()).ToLower();
        }
    }
}
