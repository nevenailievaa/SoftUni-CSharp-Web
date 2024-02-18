namespace SeminarHub.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static SeminarHub.Data.Common.Constants.SeminarConstants;

    public class Seminar
    {
        [Key]
        [Comment("This is the Primary Key of the Seminar.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(seminarTopicMaxLength)]
        [Comment("This is the Seminar's Topic name.")]
        public string Topic { get; set; } = null!;

        [Required]
        [MaxLength(seminarLecturerMaxLength)]
        [Comment("This is the Seminar's Lecturer name.")]
        public string Lecturer { get; set; } = null!;

        [Required]
        [MaxLength(seminarDetailsMaxLength)]
        [Comment("This is the Seminar's Details. It's like a description of the Seminar.")]
        public string Details { get; set; } = null!;

        [Required]
        [Comment("This is the Organizer's Primary Key.")]
        public string OrganizerId { get; set; } = null!;

        [ForeignKey(nameof(OrganizerId))]
        [Comment("This is the Seminar's Organizer - IdentityUser that created it.")]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        [Comment("This is the Seminar's Date and Time.")]
        public DateTime DateAndTime { get; set; }

        [Range(seminarDurationMinRange, seminarDurationMaxRange)]
        [Comment("This is the Seminar's Duration in minutes.")]
        public int? Duration { get; set; }

        [Required]
        [Comment("This is the Category's Primary Key.")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [Comment("This is the Seminar's Category.")]
        public Category Category { get; set; } = null!;

        [Comment("This is the Current Seminar's Participants - All the Users that joined this Seminar.")]
        public ICollection<SeminarParticipant> SeminarParticipants = new HashSet<SeminarParticipant>();
    }
}