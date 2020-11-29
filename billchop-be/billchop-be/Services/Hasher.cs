using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BillChopBE.Services
{
    public static class Hasher
    {
        public static string GetHashed(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}