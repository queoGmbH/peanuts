using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Bildet einen Kommentar zu einem Peanut ab.
    /// </summary>
    public class PeanutComment {
        private readonly DateTime _createdAt;
        private readonly User _createdBy;
        private DateTime? _changedAt;
        private User _changedBy;

        private string _comment;

        public PeanutComment() {
        }

        public PeanutComment(string comment, EntityCreatedDto entityCreatedDto) {
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            Update(comment);

            _createdBy = entityCreatedDto.CreatedBy;
            _createdAt = entityCreatedDto.CreatedAt;
        }

        /// <summary>
        ///     Ruft ab, wann der Kommentar zuletzt bearbeitet wurde.
        ///     NULL, wenn es bisher keine Bearbeitung gab.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Ruft ab, wer den Kommentar zuletzt bearbeitet hat.
        ///     NULL, wenn es bisher keine Bearbeitung gab.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Ruft den Kommentartext ab.
        /// </summary>
        public virtual string Comment {
            get { return _comment; }
        }

        /// <summary>
        ///     Ruft ab, wann der Kommentar erstellt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, wer den Kommentar erstellt hat.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        public virtual void Update(string comment, EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            Update(comment);

            _changedBy = entityChangedDto.ChangedBy;
            _changedAt = entityChangedDto.ChangedAt;
        }

        private void Update(string comment) {
            Require.NotNullOrWhiteSpace(comment, "comment");

            _comment = comment;
        }
    }
}