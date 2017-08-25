using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Bildet einen Gruppe (bzw. einen Topf) ab.
    /// </summary>
    /// <remarks>
    ///     Eine Gruppe ist eine Sammlung von Nutzern, die gemeinsam einen Geldtopf verwalten.
    /// </remarks>
    public class UserGroup : Entity {
        private string _additionalInformations;
        private DateTime? _changedAt;
        private User _changedBy;
        private DateTime _createdAt;
        private User _createdBy;
        private string _name;
        private double? _balanceOverdraftLimit;

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserGroup" />.
        /// </summary>
        public UserGroup() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserGroup" />.
        /// </summary>
        /// <param name="userGroupDto">
        ///     <see cref="UserGroupDto" />
        /// </param>
        /// <param name="entityCreatedDto">
        ///     <see cref="EntityCreatedDto" />
        /// </param>
        public UserGroup(UserGroupDto userGroupDto, EntityCreatedDto entityCreatedDto) {
            Update(userGroupDto);
            Update(entityCreatedDto);
        }

        /// <summary>
        ///     Liefert die "sonstigen Informationen".
        /// </summary>
        public virtual string AdditionalInformations {
            get { return _additionalInformations; }
        }

        /// <summary>
        ///     Liefert das Datum an dem das Objekt zuletzt geändert wurde oder <code>null</code>.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Liefert den Nutzer der die letzte Änderung durchgeführt hat oder <code>null</code>.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Liefert das Datum an dem das Objekt erzeugt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Liefert den Nutzer der das Objekt angelegt hat.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        ///     Liefert den Namen der Maklergruppe.
        /// </summary>
        public virtual string Name {
            get { return _name; }
        }

        public virtual string DisplayName {
            get { return _name; }
        }

        /// <summary>
        /// Ruft die in der Gruppe definierte Grenze des Dispos für den Kontostand ab.
        /// </summary>
        /// <remarks>
        /// Beim Über- bzw. Unterschreiten der Grenze, kann es zu </remarks>
        public virtual double? BalanceOverdraftLimit {
            get { return _balanceOverdraftLimit; }
        }

        /// <summary>
        ///     Liefert die Daten in einem DTO.
        /// </summary>
        /// <returns></returns>
        public virtual UserGroupDto GetDto() {
            UserGroupDto userGroupDto = new UserGroupDto(_additionalInformations, _name, _balanceOverdraftLimit);
            return userGroupDto;
        }

        /// <summary>
        ///     Aktualisiert die Daten.
        /// </summary>
        /// <param name="userGroupDto">
        ///     <see cref="UserGroupDto" />
        /// </param>
        /// <param name="entityChangedDto">
        ///     <see cref="EntityChangedDto" />
        /// </param>
        public virtual void Update(UserGroupDto userGroupDto, EntityChangedDto entityChangedDto) {
            Require.NotNull(userGroupDto, nameof(userGroupDto));
            Require.NotNull(entityChangedDto, nameof(entityChangedDto));
            Update(userGroupDto);
            Update(entityChangedDto);
        }

        private void Update(EntityChangedDto entityChangedDto) {
            _changedAt = entityChangedDto.ChangedAt;
            _changedBy = entityChangedDto.ChangedBy;
        }

        private void Update(EntityCreatedDto entityCreatedDto) {
            _createdAt = entityCreatedDto.CreatedAt;
            _createdBy = entityCreatedDto.CreatedBy;
        }

        private void Update(UserGroupDto userGroupDto) {
            _name = userGroupDto.Name;
            _additionalInformations = userGroupDto.AdditionalInformations;
            _balanceOverdraftLimit = userGroupDto.BalanceOverdraftLimit;
        }
    }
}