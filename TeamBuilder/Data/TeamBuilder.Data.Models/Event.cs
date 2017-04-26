namespace TeamBuilder.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public Event()
        {
            this.ParticipatingTeams = new List<Team>();
            this.Requests = new List<Request>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageFileLocation { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Team> ParticipatingTeams { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}