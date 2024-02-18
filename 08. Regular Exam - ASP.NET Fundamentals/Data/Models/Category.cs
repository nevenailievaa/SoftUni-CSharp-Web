namespace SeminarHub.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using static SeminarHub.Data.Common.Constants.CategoryConstants;

    public class Category
    {
        [Key]
        [Comment("This is the Primary Key of the Category.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(categoryNameMaxLength)]
        [Comment("This is the Category's Name.")]
        public string Name { get; set; } = null!;

        [Comment("This is the Category's Seminars - All the Seminars that are in this Category.")]
        public ICollection<Seminar> Seminars = new HashSet<Seminar>();
    }
}