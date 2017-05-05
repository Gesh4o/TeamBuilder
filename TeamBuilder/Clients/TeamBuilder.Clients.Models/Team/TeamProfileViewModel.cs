namespace TeamBuilder.Clients.Models.Team
{
    public class TeamProfileViewModel
    {
        public TeamProfileViewModel()
        {
            this.OverviewViewModel = new OverviewViewModel();
            this.UserJoinRequests = new UserJoinRequestsViewModel();
            this.EventInvitationsViewModel = new RequestsViewModel();
            this.SettingsViewModel = new SettingsViewModel();
        }

        public string Section { get; set; }

        public string TeamName { get; set; }

        public OverviewViewModel OverviewViewModel { get; set; }

        public UserJoinRequestsViewModel UserJoinRequests { get; set; }

        public RequestsViewModel EventInvitationsViewModel { get; set; }

        public SettingsViewModel SettingsViewModel { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}