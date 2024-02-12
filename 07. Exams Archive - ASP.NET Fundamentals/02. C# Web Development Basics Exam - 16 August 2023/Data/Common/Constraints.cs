namespace SoftUniBazar.Data.Common
{
    public static class Constraints
    {
        public const string errorMessage = "Field {0} must be between {1} and {2} characters!";
        public const string dateTimeFormat = "yyyy-MM-dd H:mm";

        public static class AdConstraints
        {
            public const int adNameMinLength = 5;
            public const int adNameMaxLength = 25;

            public const int adDescriptionMinLength = 15;
            public const int adDescriptionMaxLength = 250;
        }
        public static class CategoryConstraints
        {
            public const int categoryNameMinLength = 3;
            public const int categoryNameMaxLength = 15;
        }
    }
}