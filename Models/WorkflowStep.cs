using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models;

public class WorkflowStep
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int Order { get; set; }

    public bool Completed { get; set; }

    public DateTime? CompletedAt { get; set; }

    [MaxLength(100)]
    public string? CompletedBy { get; set; }

    public bool IsScriptStep { get; set; }

    public string? Script { get; set; }

    public string? ScriptOutput { get; set; }

    public int ArticleFieldId { get; set; }
    public ArticleField ArticleField { get; set; } = null!;
}
