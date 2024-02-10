namespace Homies.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Homies.Data.Common.Constraints.TypeConstraints;

    public class Type
    {
        //Properties
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(typeNameMaxLength)]
        public string Name { get; set; } = null!;

        //Events
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}