using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.MyUsers
{
    public class MyUserListViewModel
    {
        public MyUserListViewModel(IList<Core.Domain.Users.User> users,
            IList<Core.Domain.ProposedUsers.ProposedUser> proposedUsers)
        {
            Users = users;
            ProposedUsers = proposedUsers;
        }

        public IList<Core.Domain.Users.User> Users { get; set; }

        public IList<Core.Domain.ProposedUsers.ProposedUser> ProposedUsers { get; set; }
    }
}