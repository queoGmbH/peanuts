using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Bildet ein Peanut (Sammelbegriff für Ticket, Vorgang, Gruppenaktion, ...) ab.
    /// </summary>
    /// <remarks>
    ///     Ein Peanut ist eine Aktion, an der sich die Mitglieder der Gruppe, in welcher der Peanut erstellt wurde, in
    ///     verschiedenen <see cref="PeanutParticipationType">Rollen/Arten</see> beteiligen können.
    ///     Die Teilnahme kann verschiedene Status haben, wodurch ein Zustimmungs-Verfahren abgebildet werden kann.
    ///     Wird eine größere Änderung am Peanut vorgenommen, werden alle Zustimmungen zurückgesetzt, und die Teilnehmer müssen
    ///     ihre Zustimmung erneut geben.
    ///     Ein Peanut kann <see cref="IsFixed">"eingefroren"</see> werden, wodurch keine weiteren Änderungen bzw. neue
    ///     Teilnahmen mehr möglich sind.
    ///     Schließlich und endlich kann aus dem Peanut durch einen der
    ///     <see cref="PeanutParticipationType.IsCreditor">Kreditoren</see> eine oder mehrere Rechnungen erstellt werden.
    /// </remarks>
    public class Peanut : Entity {
        private readonly IList<Bill> _bills = new List<Bill>();
        private readonly DateTime _createdAt;
        private readonly User _createdBy;

        private readonly IList<PeanutParticipation> _participations = new List<PeanutParticipation>();
        private int? _maximumParticipations;

        private readonly IList<PeanutRequirement> _requirements = new List<PeanutRequirement>();
        private readonly UserGroup _userGroup;

        private DateTime? _changedAt;
        private User _changedBy;
        private readonly IList<PeanutComment> _comments = new List<PeanutComment>();
        private DateTime _day;
        private string _description;

        private IList<Document> _documents = new List<Document>();
        private string _name;

        private PeanutState _peanutState = PeanutState.Scheduling;

        public Peanut() {
        }

        public Peanut(UserGroup userGroup, PeanutDto peanutDto, IList<RequirementDto> requirements, EntityCreatedDto entityCreatedDto) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(peanutDto, "peanutDto");
            Require.NotNull(entityCreatedDto, "entityCreatedDto");
            Require.NotNull(requirements, "requirements");

            _userGroup = userGroup;
            Update(peanutDto);
            Update(requirements);
            _createdBy = entityCreatedDto.CreatedBy;
            _createdAt = entityCreatedDto.CreatedAt;
        }

        public Peanut(UserGroup userGroup, PeanutDto peanutDto, IList<RequirementDto> requirements,
            IDictionary<UserGroupMembership, PeanutParticipationDto> participators, EntityCreatedDto entityCreatedDto)
            : this(userGroup, peanutDto, requirements, entityCreatedDto) {
            Require.NotNull(participators, "participators");

            foreach (KeyValuePair<UserGroupMembership, PeanutParticipationDto> participation in participators) {
                _participations.Add(new PeanutParticipation(this, participation.Key, participation.Value, entityCreatedDto));
            }
        }

        public virtual IList<Bill> Bills {
            get { return new ReadOnlyCollection<Bill>(_bills); }
        }

        /// <summary>
        ///     Ruft ab, wann der Peanut zuletzt geändert wurde.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Ruft ab, von wem der Peanuts zuletzt geändert wurde.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        public virtual IList<PeanutComment> Comments {
            get { return new ReadOnlyCollection<PeanutComment>(_comments); }
        }

        /// <summary>
        ///     Ruft ab, wann der Peanut erstellt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, wer den Peanut erstellt hat.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        /// Ruft die maximale Anzahl von Teilnehmern am Peanut ab oder NULL, wenn es keine Einschränkung gibt.
        /// </summary>
        public virtual int? MaximumParticipations {
            get { return _maximumParticipations; }
        }

        /// <summary>
        ///     Ruft den Tag ab, an dem der Peanut durchgeführt wird.
        /// </summary>
        public virtual DateTime Day {
            get { return _day; }
        }

        /// <summary>
        ///     Ruft die Beschreibung des Peanuts ab.
        /// </summary>
        public virtual string Description {
            get { return _description; }
        }

        /// <summary>
        ///     Ruft eine schreibgeschützte Kopie der Liste mit Dokumenten/Bildern ab, die diesem Peanut angehängt sind.
        /// </summary>
        public virtual IList<Document> Documents {
            get { return _documents; }
        }

        /// <summary>
        ///     Gibt an, ob das Peanut abgerechnet wurde
        /// </summary>
        public virtual bool IsCleared {
            get { return _bills.Any(); }
        }

        /// <summary>
        ///     Ruft ab, ob der Peanut einen Status hat, in dem er nicht mehr geändert werden kann.
        /// </summary>
        public virtual bool IsFixed {
            get {

                if (_peanutState == PeanutState.Scheduling) {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Ruft ab, ob die maximale Anzahl an Teilnehmern erreicht ist.
        /// </summary>
        public virtual bool IsMaximumParticipationCountReached {
            get {
                if (MaximumParticipations.HasValue) {
                    return ConfirmedParticipationsCount >= MaximumParticipations;
                } else {
                    return false;
                }
            }
        }

        /// <summary>
        /// Ruft die Anzahl der bestätigten Teilnahmen ab.
        /// </summary>
        public virtual int ConfirmedParticipationsCount {
            get { return ConfirmedParticipations.Count; }
        }

        /// <summary>
        ///     Ruft ab, ob der Peanut den Status <see cref="Peanuts.PeanutState.Canceled"/>.
        /// </summary>
        public virtual bool IsCanceled {
            get { return _peanutState == PeanutState.Canceled; }
        }

        /// <summary>
        ///     Ruft den Namen des Peanuts ab.
        /// </summary>
        public virtual string Name {
            get { return _name; }
        }

        /// <summary>
        /// Ruft den aktuellen Status des Peanuts ab.
        /// </summary>
        public virtual PeanutState PeanutState {
            get { return _peanutState; }
        }

        /// <summary>
        ///     Ruft eine schreibgeschützte Kopie der Liste mit Teilnehmern ab.
        /// </summary>
        public virtual IList<PeanutParticipation> Participations {
            get { return new ReadOnlyCollection<PeanutParticipation>(_participations); }
        }

        /// <summary>
        /// Ruft eine schreibgeschützte Liste der bestätigten Teilnehmer ab.
        /// </summary>
        public virtual IList<PeanutParticipation> ConfirmedParticipations {
            get {
                return new ReadOnlyCollection<PeanutParticipation>(_participations.Where(part => part.ParticipationState == PeanutParticipationState.Confirmed).ToList());
            }
        }

        /// <summary>
        ///     Ruft eine Schreibgeschützte Kopie der Liste mit benötigten Sachen für die Durchführung des Peanuts ab.
        /// </summary>
        public virtual IList<PeanutRequirement> Requirements {
            get { return new ReadOnlyCollection<PeanutRequirement>(_requirements); }
        }

        /// <summary>
        ///     Ruft die Gruppe ab, in welcher der Peanut erstellt wurde.
        /// </summary>
        public virtual UserGroup UserGroup {
            get { return _userGroup; }
        }

        /// <summary>
        /// Ruft den Anzeigetext des Peanuts ab. 
        /// </summary>
        /// <remarks>Alternative zum eher technischen <see cref="object.ToString"/></remarks>
        public virtual string DisplayName {
            get { return string.Format("{0} ({1})", _name, LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(typeof(PeanutState), _peanutState.ToString())); }
        }

        /// <summary>
        /// Fügt einen neuen Kommentar zum Peanut hinzu.
        /// </summary>
        /// <param name="updateComment"></param>
        /// <param name="entityCreatedDto"></param>
        /// <returns></returns>
        public virtual PeanutComment AddComment(string updateComment, EntityCreatedDto entityCreatedDto) {
            PeanutComment comment = new PeanutComment(updateComment, entityCreatedDto);
            _comments.Add(comment);

            Update(new EntityChangedDto(entityCreatedDto.CreatedBy, entityCreatedDto.CreatedAt));

            return comment;
        }

        /// <summary>
        ///     Fügt die Dokumente der Liste mit Dokumenten hinzu, sofern sie nicht schon in der Liste enthalten sind.
        /// </summary>
        /// <param name="entityChanged"></param>
        /// <param name="documents"></param>
        public virtual void AddDocuments(EntityChangedDto entityChanged, params Document[] documents) {
            Require.NotNull(entityChanged, "entityChanged");
            Require.NotNull(documents, "documents");

            Update(entityChanged);
            _documents = _documents.Union(documents).ToList();
        }

        /// <summary>
        ///     Fügt Teilnahmen an diesem Peanut hinzu.
        /// </summary>
        /// <param name="entityChanged"></param>
        /// <param name="participations"></param>
        public virtual void AddParticipators(EntityChangedDto entityChanged, params PeanutParticipation[] participations) {
            Require.NotNull(entityChanged, "entityChanged");
            Require.NotNull(participations, "participations");

            Update(entityChanged);
            foreach (PeanutParticipation participation in participations) {
                if (!_participations.Contains(participation)) {
                    _participations.Add(participation);
                }
            }
        }

        /// <summary>
        ///     Rechnet den Peanut ab und fügt die dazugehörige Rechnung bei.
        /// </summary>
        /// <param name="bill">Die Rechnung</param>
        public virtual void Clear(Bill bill) {
            Require.NotNull(bill, "bill");

            if (!_bills.Contains(bill)) {
                _bills.Add(bill);
            }
        }

        /// <summary>
        /// Entfernt die Verknüpfung einer Rechnung mit diesem Peanut.
        /// </summary>
        /// <param name="bill"></param>
        public virtual void RemoveAssociatedBill(Bill bill) {
            if (_bills.Contains(bill)) {
                _bills.Remove(bill);
            }
        }

        /// <summary>
        ///     Markiert den Peanut als fixiert.
        /// </summary>
        /// <param name="peanutState">Der neue Status, welchen der Peanut erhalten soll.</param>
        /// <param name="entityChangedDto"></param>
        public virtual void UpdateState(PeanutState peanutState, EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            _peanutState = peanutState;
            Update(entityChangedDto);
        }

        public virtual PeanutDto GetDto() {
            return new PeanutDto(_name, _description, _day, _maximumParticipations);
        }

        /// <summary>
        ///     Entfernt alle übergebenen Dokumente aus der Liste mit Dokumenten, sofern sie in dieser enthalten sind.
        /// </summary>
        /// <param name="entityChanged"></param>
        /// <param name="documents"></param>
        public virtual void RemoveDocuments(EntityChangedDto entityChanged, params Document[] documents) {
            Require.NotNull(entityChanged, "entityChanged");
            Require.NotNull(documents, "documents");

            Update(entityChanged);
            _documents = _documents.Except(documents).ToList();
        }

        /// <summary>
        ///     Entfernt Teilnahmen aus dem Peanut.
        /// </summary>
        /// <param name="entityChanged"></param>
        /// <param name="participatorsToDelete"></param>
        public virtual void RemoveParticipators(EntityChangedDto entityChanged, params UserGroupMembership[] participatorsToDelete) {
            Require.NotNull(entityChanged, "entityChanged");
            Require.NotNull(participatorsToDelete, "participatorsToDelete");

            Update(entityChanged);
            foreach (UserGroupMembership userGroupMembership in participatorsToDelete) {
                PeanutParticipation participationToDelete = _participations.SingleOrDefault(p => p.UserGroupMembership.Equals(userGroupMembership));
                if (participationToDelete != null) {
                    _participations.Remove(participationToDelete);
                }
            }
        }

        public virtual void Update(PeanutDto peanutDto, IList<RequirementDto> requirements, EntityChangedDto entityChanged) {
            Require.NotNull(peanutDto, "peanutDto");
            Require.NotNull(entityChanged, "entityChanged");
            Require.NotNull(requirements, "requirements");

            Update(peanutDto);
            Update(requirements);
            Update(entityChanged);
        }

        private void Update(IList<RequirementDto> requirements) {
            _requirements.Clear();
            foreach (RequirementDto requirementDto in requirements) {
                _requirements.Add(new PeanutRequirement(requirementDto.Name, requirementDto.Quantity, requirementDto.Unit, requirementDto.Url));
            }
        }

        private void Update(PeanutDto peanutDto) {
            _name = peanutDto.Name;
            _day = peanutDto.Day;
            _description = peanutDto.Description;
            _maximumParticipations = peanutDto.MaximumParticipations;
        }

        private void Update(EntityChangedDto entityChanged) {
            _changedBy = entityChanged.ChangedBy;
            _changedAt = entityChanged.ChangedAt;
        }
    }
}