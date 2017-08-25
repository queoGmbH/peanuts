using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    public class PeanutPriceDevelopmentItem {
        public PeanutPriceDevelopmentItem(double price, double avaragePrice) {
            AvaragePrice = Math.Round(avaragePrice, 2);
            Price = Math.Round(price, 2);
        }

        /// <summary>
        /// Ruft den durchschnittlichen Preis pro Teilnehmer eines Peanuts bis dahin ab.
        /// </summary>
        public double AvaragePrice { get; set; }

        /// <summary>
        /// Ruft den Preis pro Teilnehmer eines Peanuts ab.
        /// </summary>
        public double Price { get; set; }

    }
}