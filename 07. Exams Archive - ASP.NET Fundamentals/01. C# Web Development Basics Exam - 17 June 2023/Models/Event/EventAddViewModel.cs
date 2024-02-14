namespace Homies.Models.Event
{
    using Homies.Models.Type;
    using System.ComponentModel.DataAnnotations;
    using static Homies.Data.Common.Constants;
    using static Homies.Data.Common.Constants.EventConstants;

    public class EventAddViewModel
    {
        [Required]
        [StringLength(eventNameMaxLength, MinimumLength = eventNameMinLength, ErrorMessage = errorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(eventDescriptionMaxLength, MinimumLength = eventDescriptionMinLength, ErrorMessage = errorMessage)]
        public string Description { get; set; } = null!;

        [Required]
        public string Start { get; set; } = null!;

        [Required]
        public string End { get; set; } = null!;

        [Required]
        public int TypeId { get; set; }
        public ICollection<TypeViewModel> Types { get; set; } = new HashSet<TypeViewModel>();
    }
}