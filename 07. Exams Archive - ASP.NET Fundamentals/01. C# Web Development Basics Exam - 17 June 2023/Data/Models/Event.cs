namespace Homies.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using static Homies.Data.Common.Constraints.EventConstraints;

    public class Event
    {
        //Properties
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(eventNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(eventDescriptionMaxLength)]
        public string Description { get; set; } = null!;


        //Organiser
        [Required]
        public string OrganiserId { get; set; } = null!;

        [ForeignKey(nameof(OrganiserId))]
        public IdentityUser Organiser { get; set; } = null!;


        //Dates Info
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }


        //Type
        [Required]
        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; } = null!;


        //Participants
        public ICollection<EventParticipant> EventsParticipants { get; set; } = new HashSet<EventParticipant>();
    }
}