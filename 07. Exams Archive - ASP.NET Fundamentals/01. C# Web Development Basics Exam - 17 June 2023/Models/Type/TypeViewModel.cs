namespace Homies.Models.Type
{
    using System.ComponentModel.DataAnnotations;
    using static Homies.Data.Common.Constants;
    using static Homies.Data.Common.Constants.TypeConstants;

    public class TypeViewModel
    {
        public int Id { get; set; }

        [StringLength(typeNameMaxLength, MinimumLength = typeNameMinLength, ErrorMessage = errorMessage)]
        public string Name { get; set; } = null!;
    }
}