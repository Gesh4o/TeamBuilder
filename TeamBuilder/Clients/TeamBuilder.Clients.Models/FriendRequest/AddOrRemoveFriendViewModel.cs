namespace TeamBuilder.Clients.Models.FriendRequest
{
    public class AddOrRemoveFriendViewModel
    {
        public bool IsFriend { get; set; }

        public string SourceId { get; set; }

        public string DestinationId { get; set; }
    }
}
