namespace TeamBuilder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        public Event()
        {
            this.ParticipatingTeams = new List<Team>();
            this.Requests = new List<Request>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 50 letters long.")]
        public string Name { get; set; }

        public string ImageFileLocation { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 50 letters long.")]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime EnrollmentEndTime { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Team> ParticipatingTeams { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}