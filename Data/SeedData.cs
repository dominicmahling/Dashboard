using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IDbContextFactory<AppDbContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();

        if (await db.Topics.AnyAsync())
            return;

        var topic = new Topic
        {
            Title = "Getting Started",
            Description = "Example topic with workflow and script demos",
            CreatedAt = DateTime.UtcNow
        };
        db.Topics.Add(topic);
        await db.SaveChangesAsync();

        var article = new Article
        {
            Title = "Workflow with Script Steps",
            TopicId = topic.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.Articles.Add(article);
        await db.SaveChangesAsync();

        var wfField = new ArticleField
        {
            FieldType = "workflow",
            ExpandByDefault = true,
            Order = 0,
            ArticleId = article.Id
        };
        db.ArticleFields.Add(wfField);
        await db.SaveChangesAsync();

        var step1 = new WorkflowStep
        {
            Name = "Open Browser",
            Description = "Opens Google in your default browser",
            Order = 0,
            ArticleFieldId = wfField.Id,
            IsScriptStep = true,
            Script = "# Open browser to a URL\nStart-Process \"https://www.google.com\"\nWrite-Host \"Browser opened successfully!\""
        };
        db.WorkflowSteps.Add(step1);

        var step2 = new WorkflowStep
        {
            Name = "Show System Info",
            Description = "Displays basic system information",
            Order = 1,
            ArticleFieldId = wfField.Id,
            IsScriptStep = true,
            Script = "# Display system info\nWrite-Host \"Computer: $env:COMPUTERNAME\"\nWrite-Host \"OS: $((Get-CimInstance Win32_OperatingSystem).Caption)\"\nWrite-Host \"RAM: $([math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB, 2)) GB\""
        };
        db.WorkflowSteps.Add(step2);

        var step3 = new WorkflowStep
        {
            Name = "Get Date & Time",
            Description = "Shows current date, time and timezone",
            Order = 2,
            ArticleFieldId = wfField.Id,
            IsScriptStep = true,
            Script = "# Show current date/time info\nWrite-Host \"Date: $(Get-Date -Format 'yyyy-MM-dd')\"\nWrite-Host \"Time: $(Get-Date -Format 'HH:mm:ss')\"\nWrite-Host \"UTC: $((Get-Date).ToUniversalTime().ToString('yyyy-MM-dd HH:mm:ss'))\"\nWrite-Host \"Timezone: $((Get-TimeZone).DisplayName)\""
        };
        db.WorkflowSteps.Add(step3);

        await db.SaveChangesAsync();

        var textField = new ArticleField
        {
            FieldType = "text",
            Content = "## About Script Steps\n\nThis workflow demonstrates **script steps** — each step can have an optional PowerShell script attached.\n\n### How it works\n- Click the **Script** button on any step to expand the script editor\n- Write or paste a PowerShell script\n- Click **Run script** to execute it\n- Output (stdout/stderr) is shown inline\n- Toggle completion with the **Done** button\n\n### Example steps\n1. **Open Browser** — launches your default browser to Google\n2. **Show System Info** — displays computer name, OS, and RAM\n3. **Get Date & Time** — shows current date, time, and timezone",
            Order = 1,
            ArticleId = article.Id
        };
        db.ArticleFields.Add(textField);

        await db.SaveChangesAsync();
    }
}
