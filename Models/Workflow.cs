using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models;

public class Workflow
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int Position { get; set; }

    public int ArticleId { get; set; }

    public Article Article { get; set; } = null!;

    public ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
}
