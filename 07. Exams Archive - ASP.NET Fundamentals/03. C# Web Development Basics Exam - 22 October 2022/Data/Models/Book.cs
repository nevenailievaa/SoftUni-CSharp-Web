namespace Library.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Library.Data.Common.Constraints.BookConstraints;

    [Comment("Books for the Library")]
    public class Book
    {
        [Comment("The Primary Key of the Book")]
        [Key]
        public int Id { get; set; }

        [Comment("The Title of the Book")]
        [Required]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("The Author of the Book")]
        [Required]
        [MaxLength(BookAuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Comment("The Description of the Book")]
        [Required]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Comment("The Cover of the Book")]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("The Rating of the Book")]
        [Required]
        [Range(BookRatingMinValue, BookRatingMaxValue)]
        public decimal Rating { get; set; }

        [Comment("The Book's Category Id")]
        [Required]
        public int CategoryId { get; set; }

        [Comment("The Book's Category")]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Comment("The Book's Users")]
        public ICollection<IdentityUserBook> UsersBooks { get; set; } = new HashSet<IdentityUserBook>();
    }
}