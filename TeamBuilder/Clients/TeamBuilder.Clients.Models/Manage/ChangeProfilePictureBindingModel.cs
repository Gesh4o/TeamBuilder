namespace TeamBuilder.Clients.Models.Manage
{
    using System.Web;

    using FluentValidation.Attributes;

    using TeamBuilder.Clients.Models.Account.Validation;

    [Validator(typeof(UpdateProfilePictureValidator))]
    public class ChangeProfilePictureBindingModel
    {
        public HttpPostedFileBase NewProfilePicture { get; set; }
    }
}
