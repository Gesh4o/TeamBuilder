namespace TeamBuilder.Clients.Models.Team
{
    using System.Collections.Generic;

    public class OverviewViewModel
    {
        public string Description { get; set; }

        public ICollection<MemberViewModel> Members { get; set; }
    }
}
