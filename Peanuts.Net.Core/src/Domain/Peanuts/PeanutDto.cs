using System;
using System.Collections.Generic;
using System.Text;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    [DtoFor(typeof(Peanut))]
    public class PeanutDto {
        public PeanutDto() {
        }

        public PeanutDto(string name, string description, DateTime day) {
            Day = day;
            Description = description;
            Name = name;
        }

        /// <summary>
        /// Ruft den Tag ab, an dem der Peanut durchgeführt wird oder legt diesen fest.
        /// </summary>
        public virtual DateTime Day { get; set; }

        /// <summary>
        /// Ruft die Beschreibung des Peanuts ab oder legt diese fest.
        /// </summary>
        public virtual string Description {
            get; set;
        }

        /// <summary>
        /// Ruft den Namen des Peanuts ab oder legt diesen fest.
        /// </summary>
        public virtual string Name {
            get; set;
        }

        protected bool Equals(PeanutDto other) {
            return Day.Equals(other.Day) && string.Equals(Description, other.Description) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeanutDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = Day.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static string[] operator- (PeanutDto source, PeanutDto target) {
            Require.NotNull(source, "source");
            Require.NotNull(target, "target");

            List<string> changes = new List<string>();
            if (source.Name != target.Name) {
                changes.Add(string.Format("{0}: {1} (war: {2})", Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Name, source.Name, target.Name));
            }
            if (source.Day != target.Day) {
                changes.Add(string.Format("{0}: {1:d} (war: {2:d})", Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Day, source.Day, target.Day));
            }
            if (source.Description != target.Description) {
                changes.Add(string.Format("{0}: {1} (war: {2})", Resources_Domain.label_Com_QueoFlow_Peanuts_Net_Core_Domain_Peanuts_Peanut_Description, source.Description, target.Description));
            }
            
            return changes.ToArray();
        }
    }
}