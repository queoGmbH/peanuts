using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Beschreibt eine Teilnahme an einem <see cref="Peanut" />
    /// </summary>
    public class PeanutParticipation : Entity {
        private readonly DateTime _createdAt;
        private readonly User _createdBy;

        private readonly Peanut _peanut;
        private readonly UserGroupMembership _userGroupMembership;

        private DateTime? _changedAt;
        private User _changedBy;
        private PeanutParticipationState _participationState;
        private PeanutParticipationType _participationType;

        public PeanutParticipation() {
        }

        public PeanutParticipation(Peanut peanut, UserGroupMembership userGroupMembership, PeanutParticipationDto participationDto, EntityCreatedDto entityCreatedDto) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(participationDto, "participationDto");
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            _peanut = peanut;
            _userGroupMembership = userGroupMembership;
            _createdBy = entityCreatedDto.CreatedBy;
            _createdAt = entityCreatedDto.CreatedAt;
            
            Update(participationDto);
        }

        /// <summary>
        ///     Ruft ab, wann die Teilnahme zuletzt geändert wurde.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Ruft ab, wer die Teilnahme zuletzt geändert hat.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Ruft ab, wann die Teilnahme erstellt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, wer die Teilnahme erstellt hat.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        ///     Ruft die Art der Teilnahme ab.
        /// </summary>
        public virtual PeanutParticipationType ParticipationType {
            get { return _participationType; }
        }

        /// <summary>
        /// Ruft den Status der Teilnahme ab.
        /// </summary>
        public virtual PeanutParticipationState ParticipationState {
            get { return _participationState; }
        }

        /// <summary>
        ///     Ruft den Peanuts ab, für den die Teilnahme ist.
        /// </summary>
        public virtual Peanut Peanut {
            get { return _peanut; }
        }

        /// <summary>
        ///     Ruft das Mitglied ab, welches an dem Peanut teilnimmt.
        /// </summary>
        public virtual UserGroupMembership UserGroupMembership {
            get { return _userGroupMembership; }
        }

        public virtual PeanutParticipationDto GetDto() {
            return new PeanutParticipationDto(_participationType, _participationState);
        }

        /// <summary>
        ///     Ändert den Status der Teilnahme.
        /// </summary>
        /// <param name="participationState"></param>
        public virtual void UpdateStatus(PeanutParticipationState participationState, EntityChangedDto entityChanged) {
            Require.NotNull(entityChanged, "entityChanged");

            _participationState = participationState;

            Update(entityChanged);
        }

        private void Update(PeanutParticipationDto participationDto) {
            _participationType = participationDto.ParticipationType;
            _participationState = participationDto.ParticipationState;
        }

        private void Update(EntityChangedDto entityChanged) {
            _changedAt = entityChanged.ChangedAt;
            _changedBy = entityChanged.ChangedBy;
        }

        public virtual void Update(PeanutParticipationDto peanutParticipationDto, EntityChangedDto entityChangedDto) {
            Update(peanutParticipationDto);
            Update(entityChangedDto);
        }
    }
}