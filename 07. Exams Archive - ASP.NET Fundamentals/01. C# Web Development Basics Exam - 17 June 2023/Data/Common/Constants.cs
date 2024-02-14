namespace Homies.Data.Common
{
    public static class Constants
    {
        public const string dateTimeFormat = "yyyy-MM-dd H:mm";
        public const string errorMessage = "Field {0} must be between {2} and {1} characters!";

        public static class EventConstants
        {
            public const int eventNameMinLength = 5;
            public const int eventNameMaxLength = 20;

            public const int eventDescriptionMinLength = 15;
            public const int eventDescriptionMaxLength = 150;
        }

        public static class TypeConstants
        {
            public const int typeNameMinLength = 5;
            public const int typeNameMaxLength = 15;
        }
    }
}