namespace TeamBuilder.Clients.Models.Team
{
    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class TeamEditBindingModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Acronym { get; set; }

        public string Description { get; set; }
    }
}
