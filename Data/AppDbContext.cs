using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
}
