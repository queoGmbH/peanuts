using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserCreateCommand {
        public ProposedUserCreateCommand() {
            ProposedUserDataDto = new ProposedUserDataDto();
            ProposedUserContactDto = new ProposedUserContactDto();
        }

        public ProposedUserContactDto ProposedUserContactDto { get; set; }

        public ProposedUserDataDto ProposedUserDataDto { get; set; }

        [Required]
        public string Username { get; set; }
    }
}