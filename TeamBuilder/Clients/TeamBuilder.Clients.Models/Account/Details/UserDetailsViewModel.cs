namespace TeamBuilder.Clients.Models.Account.Details
{
    using System.Collections.Generic;

    using TeamBuilder.Clients.Models.FriendRequest;

    public class UserDetailsViewModel
    {
        public UserDetailsViewModel()
        {
            this.Friends = new List<FriendViewModel>();
            this.Profile = new ProfileViewModel();
            this.AddOrRemoveFriendViewModel = new AddOrRemoveFriendViewModel();
        }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public bool IsSelf { get; set; }

        public ICollection<FriendViewModel> Friends { get; set; }

        public ProfileViewModel Profile { get; set; }

        public AddOrRemoveFriendViewModel AddOrRemoveFriendViewModel { get; set; }
    }
}
