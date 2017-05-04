namespace TeamBuilder.Clients.Models.Manage
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
