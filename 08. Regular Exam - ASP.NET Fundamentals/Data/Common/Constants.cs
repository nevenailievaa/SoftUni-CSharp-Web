namespace SeminarHub.Data.Common
{
    public static class Constants
    {
        public const string defaultDateTimeFormat = "yyyy-MM-dd H:mm";
        public const string errorMessage = "{0} must be between {2} and {1} characters!";

        public static class SeminarConstants
        {
            public const int seminarTopicMinLength = 3;
            public const int seminarTopicMaxLength = 100;

            public const int seminarLecturerMinLength = 5;
            public const int seminarLecturerMaxLength = 60;

            public const int seminarDetailsMinLength = 10;
            public const int seminarDetailsMaxLength = 500;

            public const int seminarDurationMinRange = 30;
            public const int seminarDurationMaxRange = 180;
        }
        public static class CategoryConstants
        {
            public const int categoryNameMinLength = 3;
            public const int categoryNameMaxLength = 50;
        }
    }
}
