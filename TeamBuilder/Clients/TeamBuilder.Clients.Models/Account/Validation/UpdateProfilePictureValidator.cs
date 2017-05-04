namespace TeamBuilder.Clients.Models.Account.Validation
{
    using FluentValidation;

    using TeamBuilder.Clients.Common;
    using TeamBuilder.Clients.Common.Validation;
    using TeamBuilder.Clients.Models.Manage;

    public class UpdateProfilePictureValidator : AbstractValidator<ChangeProfilePictureBindingModel>
    {
        public UpdateProfilePictureValidator()
        {
            this.RuleFor(team => team.NewProfilePicture)
                .Must(img => img != null)
                .WithMessage(string.Format(ServerConstants.ErrorMessages.PropertyIsRequired, "Image"))
                .Must(
                    image => image != null &&
                        image.ContentLength < ServerConstants.Models.MaxProfilePictureSizeInBytes
                        && (image.ContentType == $"image/{ImageContentType.jpeg.ToString()}"
                            || image.ContentType == $"image/{ImageContentType.png.ToString()}"))
                .WithMessage(ServerConstants.ErrorMessages.FileMustBeImage);
        }
    }
}
