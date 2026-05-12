using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models;

public class Article
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int TopicId { get; set; }

    public Topic Topic { get; set; } = null!;

    public ICollection<ArticleField> Fields { get; set; } = new List<ArticleField>();
}
