namespace Library.Data.Common;

public static class Constraints
{
    public static class BookConstraints
    {
        //Title
        public const int BookTitleMinLength = 10;
        public const int BookTitleMaxLength = 50;

        //Author
        public const int BookAuthorMinLength = 5;
        public const int BookAuthorMaxLength = 50;

        //Description
        public const int BookDescriptionMinLength = 5;
        public const int BookDescriptionMaxLength = 5000;

        //Rating
        public const double BookRatingMinValue = 0.00;
        public const double BookRatingMaxValue = 10.00;
    }

    public static class CategoryConstraints
    {
        //Name
        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 50;
    }
}