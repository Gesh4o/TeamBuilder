namespace TeamBuilder.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public Team()
        {
            this.Members = new List<ApplicationUser>();
            this.ParticipatedEvents = new List<Event>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Acronym { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ICollection<Event> ParticipatedEvents { get; set; }

        public int CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}