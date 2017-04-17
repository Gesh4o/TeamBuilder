namespace TeamBuilder.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Microsoft.AspNet.Identity.EntityFramework;

    using TeamBuilder.Data.Configuration;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}