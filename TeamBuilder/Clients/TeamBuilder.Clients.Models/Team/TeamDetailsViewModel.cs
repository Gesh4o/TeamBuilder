namespace TeamBuilder.Clients.Models.Team
{
    using System.ComponentModel;

    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class TeamDetailsViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Acronym { get; set; }

        public string Description { get; set; }

        [DisplayName("Logo")]
        public string ImageContent { get; set; }

        public string ImageFileName { get; set; }
    }
}
