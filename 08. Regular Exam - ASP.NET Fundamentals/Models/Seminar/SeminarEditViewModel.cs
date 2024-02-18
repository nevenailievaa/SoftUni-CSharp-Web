namespace SeminarHub.Models.Seminar
{
    using SeminarHub.Models.Category;
    using System.ComponentModel.DataAnnotations;
    using static SeminarHub.Data.Common.Constants;
    using static SeminarHub.Data.Common.Constants.SeminarConstants;

    public class SeminarEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(seminarTopicMaxLength, MinimumLength = seminarTopicMinLength, ErrorMessage = errorMessage)]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(seminarLecturerMaxLength, MinimumLength = seminarLecturerMinLength, ErrorMessage = errorMessage)]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(seminarDetailsMaxLength, MinimumLength = seminarDetailsMinLength, ErrorMessage = errorMessage)]
        public string Details { get; set; } = null!;

        [Required]
        public string DateAndTime { get; set; } = null!;

        [Range(seminarDurationMinRange, seminarDurationMaxRange, ErrorMessage = "{0} must be in range {1}-{2} minutes!")]
        public int? Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
    }
}