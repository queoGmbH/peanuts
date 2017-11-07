using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut
{
    /// <summary>
    ///     Command zur Erstellung eines Peanuts.
    /// </summary>
    public class PeanutCreateCommand
    {
        /// <summary>
        ///     Ruft das Dto mit Informationen über das zu erstellende Peanut ab oder legt dieses fest.
        /// </summary>
        public PeanutDto PeanutDto { get; set; }

        /// <summary>
        ///     Ruft die Gruppe ab, in welcher der Peanut erstellt werden soll oder legt diese fest.
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup { get; set; }

        /// <summary>
        ///     Ruft die Voraussetzungen für den Peanut ab oder legt diese fest.
        /// </summary>
        public IDictionary<string, RequirementDto> Requirements { get; set; }

        public PeanutCreateCommand()
        {
            PeanutDto = new PeanutDto();
            Requirements = new Dictionary<string, RequirementDto>();

        }
    }
}