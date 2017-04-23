namespace TeamBuilder.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;

    using TeamBuilder.Models;

    public class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            this.HasMany(t => t.Members)
                .WithRequired(ut => ut.Team)
                .HasForeignKey(ut => ut.TeamId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.ParticipatedEvents)
                .WithMany(e => e.ParticipatingTeams);

            this.HasMany(t => t.Requests)
                .WithRequired(r => r.Team)
                .HasForeignKey(r => r.TeamId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.Invitations)
                .WithRequired(i => i.Team);
        }
    }
}
