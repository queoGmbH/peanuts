using System;
using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    [DtoFor(typeof(Peanut))]
    public class PeanutDto {
        public PeanutDto() {
        }

        public PeanutDto(string name, string description, DateTime day, int? maximumParticipations) {
            Day = day;
            Description = description;
            Name = name;
            MaximumParticipations = maximumParticipations;
        }

        /// <summary>
        ///     Ruft den Tag ab, an dem der Peanut durchgeführt wird oder legt diesen fest.
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        ///     Ruft die Beschreibung des Peanuts ab oder legt diese fest.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Ruft die maximale Anzahl von Teilnehmern am Peanut ab oder legt diese fest. NULL, wenn es keine Einschränkung gibt.
        /// </summary>
        public int? MaximumParticipations { get; set; }

        /// <summary>
        ///     Ruft den Namen des Peanuts ab oder legt diesen fest.
        /// </summary>
        public string Name { get; set; }

        public static string[] operator -(PeanutDto source, PeanutDto target) {
            Require.NotNull(source, "source");
            Require.NotNull(target, "target");

            List<string> changes = new List<string>();
            if (source.Name != target.Name) {
                changes.Add(
                    string.Format(
                        "{0}: {1} (war: {2})",
                        Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Name,
                        source.Name,
                        target.Name));
            }
            if (source.Day != target.Day) {
                changes.Add(
                    string.Format(
                        "{0}: {1:d} (war: {2:d})",
                        Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Day,
                        source.Day,
                        target.Day));
            }
            if (source.Description != target.Description) {
                changes.Add(
                    string.Format(
                        "{0}: {1} (war: {2})",
                        Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Description,
                        source.Description,
                        target.Description));
            }
            if (source.MaximumParticipations != target.MaximumParticipations) {
                changes.Add(
                    string.Format(
                        "{0}: {1} (war: {2})",
                        Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_MaximumParticipations,
                        source.MaximumParticipations,
                        target.MaximumParticipations));
            }

            return changes.ToArray();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((PeanutDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = Day.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MaximumParticipations != null ? MaximumParticipations.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(PeanutDto other) {
            return Day.Equals(other.Day) && string.Equals(Description, other.Description) &&
                   string.Equals(Name, other.Name) && Equals(MaximumParticipations, other.MaximumParticipations);
        }
    }
}