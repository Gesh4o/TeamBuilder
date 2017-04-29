namespace TeamBuilder.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents combined unit which can participate in <see cref="Event"/> and
    /// is formed/created by <see cref="ApplicationUser"/>.
    /// </summary>
    public class Team
    {
        
        public Team()
        {
            this.Members = new List<UserTeam>();
            this.ParticipatedEvents = new List<Event>();
            this.Invitations = new List<Invitation>();
            this.Requests = new List<Request>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Acronym { get; set; }

        [StringLength(18, MinimumLength = 18)]
        public string ImageFileName { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<UserTeam> Members { get; set; }

        public virtual ICollection<Event> ParticipatedEvents { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}