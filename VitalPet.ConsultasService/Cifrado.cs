using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VitalPet.ConsultasService
{
    public static class Cifrado
    {
        private static Aes Crear()
        {
            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456");
            return aes;
        }

        public static string Cifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            using (var aes = Crear())
            using (var memoria = new MemoryStream())
            {
                using (var flujo = new CryptoStream(memoria, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var escritor = new StreamWriter(flujo)) escritor.Write(texto);
                return Convert.ToBase64String(memoria.ToArray());
            }
        }

        public static string Descifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            try
            {
                using (var aes = Crear())
                using (var memoria = new MemoryStream(Convert.FromBase64String(texto)))
                using (var flujo = new CryptoStream(memoria, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var lector = new StreamReader(flujo)) return lector.ReadToEnd();
            }
            catch (FormatException) { return texto; }
            catch (CryptographicException) { return texto; }
        }

        public static string Hash(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(texto))).Replace("-", "").ToLowerInvariant();
        }
    }
}
