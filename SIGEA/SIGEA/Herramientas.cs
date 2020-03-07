using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIGEA {
    public class Herramientas {
        public static string EncriptarConSHA512(string text) {
            return string.Join("", SHA512.Create().ComputeHash(
                Encoding.UTF8.GetBytes(text)
            ).Select(x => x.ToString("x2")).ToArray()).ToLower();
        }
    }
}
