namespace TeamBuilder.Clients.Models.Account
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using TeamBuilder.Clients.Infrastructure.Validation.Attributes;
    using TeamBuilder.Models;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression("^([a-zA-Z0-9][_]?)*[a-zA-Z0-9]$", ErrorMessage = "Username may contain only letters, digits and \"_\" (can't appear in the beginning/end).")]
        public string Username { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        [DateRange(MinValue = "01/01/1900", MaxValue = "12/31/9999", ErrorMessage = "Date must be after {0}.")]
        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}