namespace TeamBuilder.Clients.Common
{
    public class ServerConstants
    {
        public const string InfrastructureAssembly = "TeamBuilder.Clients.Infrastructure";
        public const string DataServicesAssembly = "TeamBuilder.Services.Data";
        public const string ModelsClientsAssembly = "TeamBuilder.Clients.Models";

        public const string ManagerRole = "Manager";
        public const string AdminRole = "Admin";

        public static class Models
        {
            #region Team
            public const int MinTeamNameLength = 5;
            public const int MaxTeamNameLength = 32;

            public const int TeamAcronymLength = 3;

            public const int MinTeamDescriptionLength = 5;
            public const int MaxTeamDescriptionLength = 1024;

            public const int MaxTeamLogoSizeInBytes = 1048576;
            #endregion
        }

        public static class ErrorMessages
        {
            public const string StringLengthMustBeInRange = "{0} length must be between {1} and {2}.";

            public const string StringLengthMustBe = "{0} must be {1} symbols long.";

            public const string FileMustBeImage = "Uploaded file must be an image (png/jpg).";
        }
    }
}
