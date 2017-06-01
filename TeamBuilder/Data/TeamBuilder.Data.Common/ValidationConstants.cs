namespace TeamBuilder.Data.Common
{
    public class ValidationConstants
    {
        // User
        public const int UserMinimumUserNameLength = 3;
        public const int UserMaximumUserNameLength = 50;
        public const int UserMinimumNameLength = 2;
        public const int UserMaximumNameLength = 25;
        public const int UserMinimumPasswordLength = 6;
        public const int UserMaximumPasswordLength = 50;
        public const string UserPasswordMismatchErrorMessage = "The password and confirmation password do not match";

        // Error messages
        public const string RequiredErrorMessage = "The {0} field is required";
        public const string RangeErrorMessage = "The {0} field must be between {1} and {2}";
        public const string MinLengthErrorMessage = "The {0} field must be at least {1} characters long";
        public const string MaxLengthErrorMessage = "The {0} field cannot be more than {1} characters long";
        public const string ModelInvalidErrorMessage = "Insertion failed. Model not valid!";

    }
}
