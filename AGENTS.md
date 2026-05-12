# Dashboard Project

## Stack
- **Framework**: .NET 10, Blazor Server (Interactive Server mode)
- **Database**: SQLite + EF Core (`IDbContextFactory<AppDbContext>`)
- **UI**: Bootstrap 5, custom dark theme
- **Auth**: None yet (local single-user)

## Conventions
- Razor components in `Components/Pages/` (feature folders) and `Components/Layout/`
- Models in `Models/`, DbContext in `Data/`
- Use `@rendermode InteractiveServer` for pages needing interactivity
- Inject `IDbContextFactory<AppDbContext>` (not DbContext directly)
- Always `await using var db = await DbFactory.CreateDbContextAsync();`
- Navigation: inject `NavigationManager`, call `NavigationManager.NavigateTo()`

## Database
- Connection string in `appsettings.json` → `Data Source=dashboard.db`
- `db.Database.EnsureCreated()` in Program.cs (no migrations yet)
- SQLite file (`*.db`, `*.db-shm`, `*.db-wal`, `*.db-journal`) is gitignored

## Theme
- Dark mode hardcoded (no toggle yet)
- CSS vars in `wwwroot/app.css`: `--bg-primary`, `--bg-secondary`, `--text-primary`, `--accent-color`

## Pages
- `/` — Home
- `/topics` — Topic list (create, delete)
- `/topics/new` — New topic form
- `/topics/{id}` — Topic details with article list (delete articles)
- `/topics/{id}/edit` — Edit topic
- `/topics/{topicId}/articles/new` — New article form (redirects to workflow after save)
- `/articles/{id}` — Article view with delete button + workflow link
- `/articles/{id}/edit` — Edit article
- `/articles/{id}/workflow` — Workflow management (add/remove steps, track completion)
- `/search` — Full-text search across topics and articles

## Workflow
- `WorkflowStep` model: linked to Article, has Name, Description, Order, Completed, CompletedAt
- Workflow page: add steps, click to navigate, mark complete, auto-resets when all done
- `WorkflowSteps` shared component: visual step indicator with clickable steps

## Planned Features
- Script/DB query execution page
- More dashboard tools

## Quick Commands
- `dotnet run` — start dev server
- `dotnet build` — build check
- `dotnet watch run` — hot reload
