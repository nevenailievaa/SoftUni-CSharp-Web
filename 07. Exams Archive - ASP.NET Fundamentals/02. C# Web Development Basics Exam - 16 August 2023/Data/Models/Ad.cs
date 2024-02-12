namespace SoftUniBazar.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static SoftUniBazar.Data.Common.Constraints.AdConstraints;

    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(adNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(adDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        //Owner
        [Required]
        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public IdentityUser Owner { get; set; } = null!;


        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        //Category
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}