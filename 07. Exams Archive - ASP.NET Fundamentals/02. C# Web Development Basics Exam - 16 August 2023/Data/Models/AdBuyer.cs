namespace SoftUniBazar.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class AdBuyer
    {
        //Ad
        [Required]
        public int AdId { get; set; }

        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;

        //Buyer
        [Required]
        public string BuyerId { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public IdentityUser Buyer { get; set; } = null!;
    }
}