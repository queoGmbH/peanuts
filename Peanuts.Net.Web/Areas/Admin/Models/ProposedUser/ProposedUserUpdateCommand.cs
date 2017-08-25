using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserUpdateCommand {
        public ProposedUserUpdateCommand() {
            ProposedUserDataDto = new ProposedUserDataDto();
            ProposedUserContactDto = new ProposedUserContactDto();
        }

        public ProposedUserUpdateCommand(Core.Domain.ProposedUsers.ProposedUser user) {
            Require.NotNull(user, "user");

            UserName = user.UserName;
            ProposedUserDataDto = user.GetUserDataDto();
            ProposedUserContactDto = user.GetUserContactDto();
        }

        public ProposedUserContactDto ProposedUserContactDto { get; set; }

        public ProposedUserDataDto ProposedUserDataDto { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}