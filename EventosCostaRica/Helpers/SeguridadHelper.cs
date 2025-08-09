﻿using System.Security.Cryptography;
using System.Text;

namespace EventosCostaRica.Helpers
{
    public static class SeguridadHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerificarPassword(string inputPassword, string hashGuardado)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash == hashGuardado;
        }
    }
}