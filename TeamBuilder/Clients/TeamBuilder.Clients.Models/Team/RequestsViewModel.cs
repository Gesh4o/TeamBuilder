namespace TeamBuilder.Clients.Models.Team
{
    using System.Collections.Generic;

    public class RequestsViewModel
    {
        public ICollection<int> Events { get; set; }

        public int TeamId { get; set; }
    }
}