using System;
using System.Linq;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public abstract class Builder<TBuild> : IBuilder<TBuild> {
        private static Random _random = new Random(DateTime.Now.GetHashCode());

        public abstract TBuild Build();

        public static implicit operator TBuild(Builder<TBuild> builder) {
            return builder.Build();
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

        /// <summary>
        ///     Gibt einen zufälligen String zurück
        /// </summary>
        /// <param name="length">Die Länge des zufälligen Strings</param>
        /// <returns></returns>
        protected string GetRandomString(int length) {
            string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}