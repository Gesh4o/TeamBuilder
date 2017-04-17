namespace TeamBuilder.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using TeamBuilder.Models;

    public class TeamBuilderContext : IdentityDbContext<ApplicationUser>
    {
        public TeamBuilderContext()
            : base("TeamBuilder", throwIfV1Schema: false)
        {
        }

        public static TeamBuilderContext Create()
        {
            return new TeamBuilderContext();
        }
    }
}