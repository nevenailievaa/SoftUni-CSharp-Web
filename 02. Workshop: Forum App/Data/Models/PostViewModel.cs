namespace ForumApp.Data.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}