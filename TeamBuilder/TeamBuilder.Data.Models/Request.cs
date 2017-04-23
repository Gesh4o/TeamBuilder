namespace TeamBuilder.Data.Models
{
    public enum Sender
    {
        Event,
        Team
    }

    public class Request
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public bool IsActive { get; set; }

        public Sender Sender { get; set; }
    }
}
