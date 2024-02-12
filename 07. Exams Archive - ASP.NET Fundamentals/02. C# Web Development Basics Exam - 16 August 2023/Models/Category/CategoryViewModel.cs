namespace SoftUniBazar.Models.Category
{
    using System.ComponentModel.DataAnnotations;
    using static SoftUniBazar.Data.Common.Constraints;
    using static SoftUniBazar.Data.Common.Constraints.CategoryConstraints;

    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(categoryNameMaxLength, MinimumLength = categoryNameMinLength, ErrorMessage = errorMessage)]
        public string Name { get; set; } = null!;
    }
}