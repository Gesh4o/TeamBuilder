namespace TeamBuilder.Clients.Models.Team
{
    using System.ComponentModel;

    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class TeamDetailsViewModel : IMapFrom<Team>
    {
        public TeamDetailsViewModel()
        {
            TeamProfileViewModel = new TeamProfileViewModel();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Acronym { get; set; }

        public SendJoinRequestViewModel SendJoinRequestViewModel { get; set; }

        public TeamProfileViewModel TeamProfileViewModel { get; set; }  

        public bool CanSentJoinRequest { get; set; }

        [DisplayName("Logo")]
        public string LogoUrl { get; set; }

        public string ImageFileName { get; set; }
    }
}
