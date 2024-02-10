namespace Homies.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Build.Framework;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EventParticipant
    {
        //Helper
        [Required]
        public string HelperId { get; set; } = null!;

        [ForeignKey(nameof(HelperId))]
        public IdentityUser Helper { get; set; } = null!;

        //Event
        [Required]
        public int EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
    }
}