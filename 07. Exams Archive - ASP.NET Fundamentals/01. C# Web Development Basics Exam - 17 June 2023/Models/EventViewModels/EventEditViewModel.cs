namespace Homies.Models.EventViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Homies.Models.TypeViewModels;
    using static Homies.Data.Common.Constraints;
    using static Homies.Data.Common.Constraints.EventConstraints;

    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = requiredErrorMessage)]
        [StringLength(eventNameMaxLength, MinimumLength = eventNameMinLength, ErrorMessage = eventNameErrorMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = requiredErrorMessage)]
        [StringLength(eventDescriptionMaxLength, MinimumLength = eventDescriptionMinLength, ErrorMessage = eventDescriptionErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = requiredErrorMessage)]
        public string Start { get; set; } = null!;

        [Required(ErrorMessage = requiredErrorMessage)]
        public string End { get; set; } = null!;

        [Required(ErrorMessage = requiredErrorMessage)]
        public int TypeId { get; set; }

        [Required(ErrorMessage = requiredErrorMessage)]
        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}