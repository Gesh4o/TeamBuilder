namespace TeamBuilder.Clients.Models.Team
{
    using System.Collections.Generic;

    public class UserJoinRequestsViewModel
    {
        public ICollection<UserRequestViewModel> Users { get; set; }

        public int TeamId { get; set; }
    }
}