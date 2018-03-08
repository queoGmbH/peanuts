using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    [DtoFor(typeof(Peanut))]
    public class PeanutDto {
        public PeanutDto() {
        }

        public PeanutDto(string name, string description, DateTime day, int? maximumParticipations, string externalLinks) {
            Day = day;
            Description = description;
            Name = name;
            MaximumParticipations = maximumParticipations;
            ExternalLinks = externalLinks;
        }

        /// <summary>
        ///     Ruft den Tag ab, an dem der Peanut durchgeführt wird oder legt diesen fest.
        /// </summary>
        [Required]
        public DateTime Day { get; set; }

        /// <summary>
        ///     Ruft die Beschreibung des Peanuts ab oder legt diese fest.
        /// </summary>
        [StringLength(4000)]
        public string Description { get; set; }

        /// <summary>
        /// Ruft durch Leerzeichen oder Zeilenumbruch getrennte Links ab, die evtl. weitere Informationen zum Peanut enthalten oder legt diese fest. 
        /// Bsp. können zum Beispiel Links zu Rezepten in Kochportalen, Links zu Veranstaltungen oder ähnlichem sein.
        /// </summary>
        [StringLength(4000)]
        public string ExternalLinks { get; set; }

        /// <summary>
        ///     Ruft die maximale Anzahl von Teilnehmern am Peanut ab oder legt diese fest. NULL, wenn es keine Einschränkung gibt.
        /// </summary>
        public int? MaximumParticipations { get; set; }

        /// <summary>
        ///     Ruft den Namen des Peanuts ab oder legt diesen fest.
        /// </summary>
        [Required]
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
            if (source.ExternalLinks != target.ExternalLinks) {
                changes.Add(
                    string.Format(
                        "{0}: {1} (war: {2})",
                        Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_ExternalLinks,
                        source.ExternalLinks,
                        target.ExternalLinks));
            }

            return changes.ToArray();
        }

        protected bool Equals(PeanutDto other) {
            return Day.Equals(other.Day) && string.Equals(Description, other.Description) && string.Equals(ExternalLinks, other.ExternalLinks) && MaximumParticipations == other.MaximumParticipations && string.Equals(Name, other.Name);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeanutDto)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                int hashCode = Day.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExternalLinks != null ? ExternalLinks.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ MaximumParticipations.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}