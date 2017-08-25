using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    [DtoFor(typeof(PeanutParticipation))]
    public class PeanutParticipationDto {

        /// <summary>
        /// Für ModelBinding.
        /// </summary>
        public PeanutParticipationDto() {
        }

        public PeanutParticipationDto(PeanutParticipationType participationType, PeanutParticipationState participationState) {
            ParticipationType = participationType;
            ParticipationState = participationState;
        }

        /// <summary>
        /// Ruft die Art der Teilnahme ab oder legt diesen fest.
        /// </summary>
        public PeanutParticipationType ParticipationType { get; set; }

        /// <summary>
        /// Ruft den Status der Teilnahme ab oder legt diesen fest.
        /// </summary>
        public PeanutParticipationState ParticipationState { get; set; }

        protected bool Equals(PeanutParticipationDto other) {
            return Equals(ParticipationType, other.ParticipationType) && ParticipationState == other.ParticipationState;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeanutParticipationDto)obj);
        }

        public override int GetHashCode() {
            unchecked { return ((ParticipationType != null ? ParticipationType.GetHashCode() : 0) * 397) ^ (int)ParticipationState; }
        }
    }
}