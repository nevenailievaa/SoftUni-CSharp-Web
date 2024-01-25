using static ForumApp.Data.DataConstants.Post;
using System.ComponentModel.DataAnnotations;

namespace ForumApp.Data.Models;

public class PostFormModel
{
    [Required]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
    public string Content { get; set; } = null!;
}