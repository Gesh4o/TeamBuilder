namespace TeamBuilder.Clients.Models.Team.Validation
{
    using FluentValidation;

    using TeamBuilder.Clients.Common;
    using TeamBuilder.Clients.Models.Team;

    public enum ContentType
    {
        png,
        jpeg
    }

    public class TeamAdditionValidator : AbstractValidator<TeamAddBindingModel>
    {
        public TeamAdditionValidator()
        {
            this.RuleFor(team => team.Name)
                .NotEmpty()
                .Must(
                    name =>
                        name.Length <= ServerConstants.Models.MaxTeamNameLength
                        && name.Length >= ServerConstants.Models.MinTeamNameLength)
                .WithMessage(
                    string.Format(
                        ServerConstants.ErrorMessages.StringLengthMustBeInRange,
                        "Name",
                        ServerConstants.Models.MaxTeamNameLength,
                        ServerConstants.Models.MinTeamNameLength));

            this.RuleFor(team => team.Acronym)
                .NotEmpty()
                .Must(
                    acronym =>
                        acronym.Length == ServerConstants.Models.TeamAcronymLength)
                .WithMessage(
                    string.Format(
                        ServerConstants.ErrorMessages.StringLengthMustBe,
                        "Acronym",
                        ServerConstants.Models.TeamAcronymLength));

            this.RuleFor(team => team.Description)
                .NotEmpty()
                .Must(
                    description =>
                        description.Length <= ServerConstants.Models.MaxTeamDescriptionLength
                        && description.Length >= ServerConstants.Models.MinTeamDescriptionLength)
                .WithMessage(
                    string.Format(
                        ServerConstants.ErrorMessages.StringLengthMustBeInRange,
                        "Description",
                        ServerConstants.Models.MaxTeamDescriptionLength,
                        ServerConstants.Models.MinTeamDescriptionLength));

            this.RuleFor(team => team.Image)
                .Must(
                    image =>
                        image == null ||
                        (image.ContentLength < ServerConstants.Models.MaxTeamLogoSizeInBytes
                            && (image.ContentType == $"image/{ContentType.jpeg.ToString()}"
                                || image.ContentType == $"image/{ContentType.png.ToString()}")))
                .WithMessage(ServerConstants.ErrorMessages.FileMustBeImage);
        }
    }
}
