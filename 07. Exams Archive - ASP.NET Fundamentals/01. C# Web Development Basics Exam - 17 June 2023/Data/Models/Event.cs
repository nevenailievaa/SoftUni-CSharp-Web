namespace Homies.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Homies.Data.Common.Constants.EventConstants;
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(eventNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(eventDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string OrganiserId { get; set; } = null!;

        [ForeignKey(nameof(OrganiserId))]
        public IdentityUser Organiser { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; } = null!;

        public ICollection<EventParticipant> EventsParticipants = new HashSet<EventParticipant>();
    }
}