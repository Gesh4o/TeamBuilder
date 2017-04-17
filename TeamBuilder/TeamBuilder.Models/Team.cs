namespace TeamBuilder.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public Team()
        {
            this.Members = new List<UserTeam>();
            this.ParticipatedEvents = new List<Event>();
            this.Invitations = new List<Invitation>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Acronym { get; set; }

        public virtual ICollection<UserTeam> Members { get; set; }

        public virtual ICollection<Event> ParticipatedEvents { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}