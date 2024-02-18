namespace SeminarHub.Models.Category
{
    using System.ComponentModel.DataAnnotations;
    using static SeminarHub.Data.Common.Constants.CategoryConstants;

    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(categoryNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}