namespace TeamBuilder.Models
{
    public class Invitation
    {
        public Invitation()
        {
            this.IsActive = true;
        }

        public int Id { get; set; }

        public int SenderUserId { get; set; }

        public virtual ApplicationUser SenderUser { get; set; }

        public int InvitedUserId { get; set; }

        public virtual ApplicationUser InvitedUser { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public bool IsActive { get; set; }
    }
}