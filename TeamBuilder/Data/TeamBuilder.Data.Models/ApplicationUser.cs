namespace TeamBuilder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.CreatedTeams = new List<Team>();
            this.JoinedTeams = new List<UserTeam>();
            this.CreatedTeams = new List<Team>();
            this.InvitationsSent = new List<Invitation>();
            this.InvitationsReceived = new List<Invitation>();
        }

        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public string ProfilePicturePath { get; set; }

        public virtual ICollection<Team> CreatedTeams { get; set; }

        public virtual ICollection<UserTeam> JoinedTeams { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; }

        public virtual ICollection<Invitation> InvitationsSent { get; set; }

        public virtual ICollection<Invitation> InvitationsReceived { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}