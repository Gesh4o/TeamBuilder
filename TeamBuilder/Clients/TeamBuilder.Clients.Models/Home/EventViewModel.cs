namespace TeamBuilder.Clients.Models.Home
{
    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class EventViewModel : IMapFrom<Event>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StartDate { get; set; }
    }
}
