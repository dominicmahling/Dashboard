using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models;

public class ArticleField
{
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string FieldType { get; set; } = "text";

    public string? Content { get; set; }

    public bool ExpandByDefault { get; set; }

    public int Order { get; set; }

    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
}
