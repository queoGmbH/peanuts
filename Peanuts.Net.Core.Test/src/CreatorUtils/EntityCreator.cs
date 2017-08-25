using System;
using System.Linq;
using System.Security.Cryptography;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {

    /// <summary>
    /// Basisklasse für EntityCreator-Klassen. 
    /// </summary>
    public class EntityCreator {
        private readonly Random _random;

        public EntityCreator() {
            _random = new Random();
        }

        /// <summary>
        ///     Erstellt einen Hash von einem String
        /// </summary>
        /// <returns></returns>
        protected string GetRandomPasswordHash() {
            string password = GetRandomString(10);
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8)) {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        ///     Gibt einen zufälligen String zurück
        /// </summary>
        /// <param name="length">Die Länge des zufälligen Strings</param>
        /// <returns></returns>
        protected string GetRandomString(int length) {
            string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        ///     Gibt eine zufällige Zahl mit einer bestimmten Anzahl an Stellen zurück
        /// </summary>
        /// <param name="length">Die Länge des zufälligen Strings</param>
        /// <returns></returns>
        protected long GetRandomNumber(int length) {
            string chars = @"0123456789";
            string randomNumberAsString = new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
            return long.Parse(randomNumberAsString);
        }
    }
}