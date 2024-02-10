namespace Homies.Data.Common
{
    public class Constraints
    {
        public const string requiredErrorMessage = "The {0} field is required!";

        public static class EventConstraints
        {
            public const int eventNameMinLength = 5;
            public const int eventNameMaxLength = 20;
            public const string eventNameErrorMessage = "The {0} field must be between {1} and {2} characters!";

            public const int eventDescriptionMinLength = 15;
            public const int eventDescriptionMaxLength = 150;
            public const string eventDescriptionErrorMessage = "The {0} field must be between {1} and {2} characters!";

            public const string defaultDateFormat = "yyyy-MM-dd H:mm";
        }
        public static class TypeConstraints
        {
            public const int typeNameMinLength = 5;
            public const int typeNameMaxLength = 15;
        }
    }
}