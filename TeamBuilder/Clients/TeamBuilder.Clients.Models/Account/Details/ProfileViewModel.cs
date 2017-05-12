namespace TeamBuilder.Clients.Models.Account.Details
{
    using System.Collections.Generic;

    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            this.Activities = new List<string>();
            this.Teams = new List<TeamListViewModel>();
            this.Events = new List<EventListViewModel>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public IEnumerable<string> Activities { get; set; }

        public IEnumerable<TeamListViewModel> Teams { get; set; }

        public IEnumerable<EventListViewModel> Events { get; set; }

        public string Section { get; set; }

        public bool IsAuthenticated { get; set; }

        public string View { get; set; }

        public object Data { get; set; }
    }
}
