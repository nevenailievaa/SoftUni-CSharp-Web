namespace SeminarHub.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SeminarParticipant
    {
        [Required]
        [Comment("This is the Seminar's Primary Key.")]
        public int SeminarId { get; set; }

        [ForeignKey(nameof(SeminarId))]
        [Comment("This is the Seminar.")]
        public Seminar Seminar { get; set; } = null!;


        [Required]
        [Comment("This is the Participant's Primary Key.")]
        public string ParticipantId { get; set; } = null!;

        [ForeignKey(nameof(ParticipantId))]
        [Comment("This is the Participant.")]
        public IdentityUser Participant { get; set; } = null!;
    }
}