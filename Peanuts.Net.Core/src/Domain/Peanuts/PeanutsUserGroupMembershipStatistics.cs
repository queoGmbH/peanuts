using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    public class PeanutsUserGroupMembershipStatistics {
        public PeanutsUserGroupMembershipStatistics(UserGroupMembership member, IList<Peanut> allPeanutsInGroup, IList<PeanutParticipation> peanutParticipationsOfMember) {
            
            /*Gruppe*/
            TotalPeanutsInGroup = allPeanutsInGroup.Count;
            CanceledPeanutsInGroup = allPeanutsInGroup.Count(p => p.IsCanceled);
            PeanutsCreatedByMember = allPeanutsInGroup.Count(p => p.CreatedBy.Equals(member.User));
            CurrentPeanutCount = allPeanutsInGroup.Count(p => p.PeanutState != PeanutState.Canceled && p.PeanutState != PeanutState.Realized);
            AvarageParticipationsCount =
                    allPeanutsInGroup.Where(p => p.PeanutState != PeanutState.Canceled).Average(p => p.Participations.Count(part => part.ParticipationState == PeanutParticipationState.Confirmed));
            DonePeanutsInGroup = allPeanutsInGroup.Count(p => p.PeanutState == PeanutState.Realized);

            /*Teilnahmen*/
            ParticipationsOnCanceledPeanutsInGroup =
                    peanutParticipationsOfMember.Count(part => part.ParticipationState == PeanutParticipationState.Confirmed && part.Peanut.IsCanceled);
            PeanutParticipationCountTotal = allPeanutsInGroup.Count(p => !p.IsCanceled);
            ParticipationsOnDonePeanutsInGroup = allPeanutsInGroup.Count(p => p.PeanutState == PeanutState.Realized);
            ParticipationsByType = peanutParticipationsOfMember
                    .Where(part => !part.Peanut.IsCanceled)
                    .GroupBy(part => part.ParticipationType)
                    .ToDictionary(g => g.Key, g => g.Count());

            PriceDevelopment = CalculatePriceDevelopment(member, allPeanutsInGroup);

            Dictionary<Peanut, double> orderedByPrice = PriceDevelopment.OrderBy(dev => dev.Value.Price).ToDictionary(o => o.Key, o => o.Value.Price);

            

            MinPriceTop5 = orderedByPrice.Take(5).Select(obp => new PeanutAvaragePrice(obp.Key, obp.Value)).ToList();
            MaxPriceTop5 = orderedByPrice.Reverse().Take(5).Select(obp => new PeanutAvaragePrice(obp.Key, obp.Value)).ToList();
        }

        /// <summary>
        ///     Ruft die durchschnittliche Anzahl von Teilnehmern an Peanuts in der Gruppe ab.
        /// </summary>
        public double AvarageParticipationsCount { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der abgesagten Peanuts in der Gruppe ab.
        /// </summary>
        public int CanceledPeanutsInGroup { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der aktuellen Peanuts ab.
        ///     Damit sind Peanuts gemeint, die nicht als <see cref="PeanutState.Realized" /> oder
        ///     <see cref="PeanutState.Canceled" /> markiert sind.
        /// </summary>
        public int CurrentPeanutCount { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der in der Gruppe durchgeführten Peanuts ab.
        /// </summary>
        public int DonePeanutsInGroup { get; private set; }

        /// <summary>
        ///     Ruft den Peanut mit dem höchsten Preis pro Teilnehmer ab.
        /// </summary>
        public IList<PeanutAvaragePrice> MaxPriceTop5 { get; private set; }

        /// <summary>
        ///     Ruft den Peanut mit dem niedrigsten Preis pro Teilnehmer ab.
        /// </summary>
        public IList<PeanutAvaragePrice> MinPriceTop5 { get; private set; }

        public class PeanutAvaragePrice {
            /// <inheritdoc />
            public PeanutAvaragePrice(Peanut peanut, double avaragePrice) {
                Peanut = peanut;
                AvaragePrice = avaragePrice;
            }

            /// <summary>
            /// Ruft den Peanut ab.
            /// </summary>
            public Peanut Peanut { get; }

            /// <summary>
            /// Ruft den durchschnittlichen Preis pro Teilnehmer für den Peanut ab.
            /// </summary>
            public double AvaragePrice { get; }

            /// <inheritdoc />
            public override string ToString() {
                return string.Format("{0:C} ({1})", AvaragePrice, string.Format("{0} am {1:d}", Peanut.Name, Peanut.Day));
            }
        }

        /// <summary>
        ///     Ruft die Anzahl der Teilnahmen je nach Teilnahmeart ab.
        /// </summary>
        public IDictionary<PeanutParticipationType, int> ParticipationsByType { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der abgesagten Peanuts in der Gruppe ab, an denen der Nutzer teilnehmen wollte.
        /// </summary>
        public int ParticipationsOnCanceledPeanutsInGroup { get; private set; }

        public int ParticipationsOnDonePeanutsInGroup { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der Teilnahmen an Peanuts ab.
        ///     Abgesagt Peanuts werden nicht berücksichtigt.
        /// </summary>
        public int PeanutParticipationCountTotal { get; private set; }

        /// <summary>
        ///     Ruft die Anzahl der vom Mitglied erstellten Peanuts ab.
        /// </summary>
        public int PeanutsCreatedByMember { get; private set; }

        public IDictionary<Peanut, PeanutPriceDevelopmentItem> PriceDevelopment { get; }

        /// <summary>
        ///     Ruft die Anzahl aller Peanuts in der Gruppe ab.
        /// </summary>
        public int TotalPeanutsInGroup { get; private set; }

        private static IDictionary<Peanut, PeanutPriceDevelopmentItem> CalculatePriceDevelopment(UserGroupMembership member, IList<Peanut> allPeanutsInGroup) {
            IList<Peanut> relevantPeanuts = allPeanutsInGroup.Where(p => p.IsCanceled == false && p.IsCleared).OrderBy(p => p.Day).ToList();
            IDictionary<Peanut, PeanutPriceDevelopmentItem> priceDevelopment = new Dictionary<Peanut, PeanutPriceDevelopmentItem>();

            double avaragePeanutsPrice = 0;
            for (int i = 0; i < relevantPeanuts.Count; i++) {
                IList<Peanut> takenPeanuts = relevantPeanuts.Take(i + 1).ToList();
                Peanut peanut = relevantPeanuts[i];
                double peanutPrice = peanut.Bills.Sum(b => b.Amount / b.DebitorCount);
                avaragePeanutsPrice = (avaragePeanutsPrice * i + peanutPrice) / (i + 1);
                PeanutPriceDevelopmentItem devItem = new PeanutPriceDevelopmentItem(peanutPrice, avaragePeanutsPrice);
                priceDevelopment.Add(peanut, devItem);
            }

            return priceDevelopment;
        }


        
    }



}