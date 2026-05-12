using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models;

public class Topic
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
