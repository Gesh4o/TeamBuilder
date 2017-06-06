namespace TeamBuilder.Clients.Models.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TeamBuilder.Data.Models;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 25 letters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 25 letters long.")]
        public string LastName { get; set; }
    }
}
