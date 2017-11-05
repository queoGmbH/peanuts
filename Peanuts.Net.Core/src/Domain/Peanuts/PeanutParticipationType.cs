using System;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Beschreibt eine Art, wie ein Nutzer an einem Peanut teilnehmen kann.
    /// </summary>
    public class PeanutParticipationType : Entity {

        protected PeanutParticipationType()
        {
        }

        /// <summary>
        ///     Ruft ab, wann die Teilnahmeart zuletzt geändert wurde.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Ruft ab, von wem die Teilnahmeart zuletzt geändert wurde.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Ruft ab, wann die Teilnahmeart erstellt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, von wem die Teilnahmeart erstellt wurde.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        public virtual string DisplayName {
            get { return _name; }
        }

        /// <summary>
        ///     Ruft ab, ob es sich bei der Art der Teilnahme um einen Einkäufer handelt.
        /// </summary>
        public virtual bool IsCreditor {
            get { return _isCreditor; }
        }

        /// <summary>
        ///     Ruft ab, ob es sich bei der Art der Teilnahme um einen Produzenten/Koch handelt.
        /// </summary>
        public virtual bool IsProducer {
            get { return _isProducer; }
        }

        /// <summary>
        ///     Ruft die maximale Anzahl an Teilnehmern dieses Typs ab.
        ///     Bei NULL dürfen unendlich viele Teilnehmer dieses Typs mitmachen.
        /// </summary>
        public virtual int? MaxParticipatorsOfType {
            get { return _maxParticipatorsOfType; }
        }

        /// <summary>
        ///     Ruft den Namen des Teilnahmetyps ab.
        /// </summary>
        public virtual string Name {
            get { return _name; }
        }

        public virtual UserGroup UserGroup
        {
            get { return _userGroup; }
        }
#pragma warning disable 649
        private readonly DateTime _createdAt;
        private readonly User _createdBy;

        private DateTime? _changedAt;
        private User _changedBy;
        private bool _isCreditor;
        private bool _isProducer;
        private int _maxParticipatorsOfType;

        private string _name;
        private readonly UserGroup _userGroup;
 

        public PeanutParticipationType(PeanutParticipationTypeDto participationTypeDto, UserGroup usergroup, EntityCreatedDto entityCreatedDto)
        {
            Update(participationTypeDto);
            _createdAt = entityCreatedDto.CreatedAt;
            _createdBy = entityCreatedDto.CreatedBy;
            _userGroup = usergroup;
        }

        private void Update(PeanutParticipationTypeDto participationTypeDto)
        {
            _name = participationTypeDto.Name;
            _isCreditor = participationTypeDto.IsCreditor;
            _isProducer = participationTypeDto.IsProducer;
            _maxParticipatorsOfType = participationTypeDto.MaxParticipatorsOfType;
        }

#pragma warning restore 649
    }
}