namespace TeamBuilder.Clients.Models.Team
{
    using System.Collections.Generic;

    public class SettingsViewModel
    {
        public int TeamId { get; set; }

        public ICollection<MemberWithRoleViewModel> Members { get; set; }
    }
}