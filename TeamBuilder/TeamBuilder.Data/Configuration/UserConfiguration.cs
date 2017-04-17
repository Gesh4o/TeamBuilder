namespace TeamBuilder.Data.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using TeamBuilder.Models;

    public class UserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public UserConfiguration()
        {
            this.HasMany(u => u.CreatedTeams)
                .WithRequired(t => t.Creator);

            this.HasMany(u => u.JoinedTeams)
                .WithRequired(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .WillCascadeOnDelete(false);

            this.HasMany(u => u.InvitationsSent)
                .WithRequired(i => i.SenderUser)
                .HasForeignKey(i => i.SenderUserId)
                .WillCascadeOnDelete(false);

            this.HasMany(u => u.InvitationsReceived)
                .WithRequired(u => u.InvitedUser)
                .HasForeignKey(i => i.InvitedUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
