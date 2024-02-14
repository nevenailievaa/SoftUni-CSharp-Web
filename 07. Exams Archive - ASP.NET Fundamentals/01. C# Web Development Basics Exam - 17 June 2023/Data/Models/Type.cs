namespace Homies.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Homies.Data.Common.Constants.TypeConstants;
    public class Type
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(typeNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Event> Events = new HashSet<Event>();
    }
}