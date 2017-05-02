namespace TeamBuilder.Clients.Models.Home
{
    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class TeamViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
