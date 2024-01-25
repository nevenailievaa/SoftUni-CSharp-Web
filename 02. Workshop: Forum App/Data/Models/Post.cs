using System.ComponentModel.DataAnnotations;
using static ForumApp.Data.DataConstants.Post;

namespace ForumApp.Data.Models;

public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; } = null!;
}