namespace Library.Data.Models
{
    using MessagePack;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Build.Framework;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("The Reader of the Books in the Library")]
    public class IdentityUserBook
    {
        [Comment("The Book's Collector Id")]
        [Required]
        public string CollectorId { get; set; } = null!;

        [Comment("The Book's Collector")]
        [ForeignKey(nameof(CollectorId))]
        public IdentityUser Collector { get; set; } = null!;


        [Comment("The Collector's Book Id")]
        [Required]
        public int BookId { get; set; }

        [Comment("The Collector's Book")]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;
    }
}