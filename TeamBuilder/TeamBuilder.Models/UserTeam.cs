namespace TeamBuilder.Models
{
    public enum TeamRole
    {
        Member,
        Administrator
    }

    public class UserTeam
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public TeamRole Role { get; set; }
    }
}
