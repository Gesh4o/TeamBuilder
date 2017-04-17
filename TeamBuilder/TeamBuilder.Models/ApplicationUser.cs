namespace TeamBuilder.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}