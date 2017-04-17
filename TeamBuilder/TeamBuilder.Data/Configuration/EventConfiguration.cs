namespace TeamBuilder.Data.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using TeamBuilder.Models;

    public class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration()
        {
            this.HasMany(e => e.Requests)
                .WithRequired(r => r.Event)
                .HasForeignKey(r => r.EventId)
                .WillCascadeOnDelete(false);

            this.HasMany(e => e.ParticipatingTeams)
                .WithMany(t => t.ParticipatedEvents)
                .Map(
                    ca =>
                        {
                            ca.MapLeftKey("EventId");
                            ca.MapRightKey("TeamId");
                            ca.ToTable("TeamEvents");
                        });
        }
    }
}
