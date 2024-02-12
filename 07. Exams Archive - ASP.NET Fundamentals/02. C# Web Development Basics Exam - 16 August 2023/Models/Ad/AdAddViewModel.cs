namespace SoftUniBazar.Models.Ad
{
    using SoftUniBazar.Models.Category;
    using System.ComponentModel.DataAnnotations;
    using static SoftUniBazar.Data.Common.Constraints;
    using static SoftUniBazar.Data.Common.Constraints.AdConstraints;

    public class AdAddViewModel
    {
        [Required]
        [StringLength(adNameMaxLength, MinimumLength = adNameMinLength, ErrorMessage = errorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(adDescriptionMaxLength, MinimumLength = adDescriptionMinLength, ErrorMessage = errorMessage)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "{0} must be a positive number!")]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        //Category
        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
