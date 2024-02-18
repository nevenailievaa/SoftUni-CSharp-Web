namespace Library.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using static Library.Data.Common.Constraints.CategoryConstraints;

    [Comment("The Categories for the Books in the Library")]
    public class Category
    {
        [Comment("The Category's Primary Key")]
        [Key]
        public int Id { get; set; }

        [Comment("The Category's Name")]
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("The Category's Book Collection")]
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}