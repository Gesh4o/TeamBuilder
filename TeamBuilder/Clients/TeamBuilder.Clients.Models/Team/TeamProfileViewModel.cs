namespace TeamBuilder.Clients.Models.Team
{
    public class TeamProfileViewModel
    {
        public TeamProfileViewModel()
        {
        }

        public string Section { get; set; }

        public string TeamName { get; set; }

        public string View { get; set; }

        public object ViewData { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}