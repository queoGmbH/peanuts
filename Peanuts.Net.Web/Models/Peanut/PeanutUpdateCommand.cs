using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Ändern eines Peanuts.
    /// </summary>
    public class PeanutUpdateCommand {

        public PeanutUpdateCommand() {
            PeanutDto = new PeanutDto();
            Requirements = new Dictionary<string, RequirementDto>();
            PeanutCommentCreateCommand = new PeanutCommentCreateCommand();
            PeanutState = PeanutState.Scheduling;
        }

        public PeanutUpdateCommand(Core.Domain.Peanuts.Peanut peanut) {
            Require.NotNull(peanut, "peanut");
            
            PeanutDto = peanut.GetDto();
            Requirements = peanut.Requirements.ToDictionary(r => Guid.NewGuid().ToString(), r => r.GetDto());
            PeanutCommentCreateCommand = new PeanutCommentCreateCommand();
            PeanutState = peanut.PeanutState;
        }

        /// <summary>
        /// Ruft das Command zum Erstellen eines neuen Kommentars am Peanut ab oder legt dieses fest.
        /// </summary>
        public PeanutCommentCreateCommand PeanutCommentCreateCommand { get; set; }

        /// <summary>
        /// Ruft das Dto mit Informationen über das zu erstellende Peanut ab oder legt dieses fest.
        /// </summary>
        public PeanutDto PeanutDto {
            get; set;
        }
        
        /// <summary>
        /// Ruft die Voraussetzungen für den PEanut ab oder legt diese fest.
        /// </summary>
        public IDictionary<string, RequirementDto> Requirements {
            get; set;
        }
        
        /// <summary>
        /// Ruft den aktuellen Status des Peanuts ab oder legt den neuen Status des Peanuts fest.
        /// </summary>
        public PeanutState PeanutState { get; set; }
    }
}