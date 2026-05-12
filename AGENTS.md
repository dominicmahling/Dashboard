# Dashboard Project

## Stack
- **Framework**: .NET 10, Blazor Server (Interactive Server mode)
- **Database**: SQLite + EF Core (`IDbContextFactory<AppDbContext>`)
- **UI**: Bootstrap 5, custom dark theme with Inter font
- **Markdown**: Markdig library
- **Auth**: None yet (local single-user)

## Conventions
- Razor components in `Components/Pages/` (feature folders) and `Components/Layout/`
- Models in `Models/`, DbContext in `Data/`
- Use `@rendermode InteractiveServer` for pages needing interactivity
- Inject `IDbContextFactory<AppDbContext>` — always `await using var db = await DbFactory.CreateDbContextAsync();`
- Navigation: inject `NavigationManager`, call `NavigationManager.NavigateTo()`
- JS interop: use ES modules with `import()` via `IJSRuntime`
- SortableJS for drag-and-drop (loaded via CDN in `App.razor`)

## Database
- Connection string in `appsettings.json` → `Data Source=dashboard.db`
- `db.Database.Migrate()` in Program.cs
- EF migrations track schema changes
- SQLite files gitignored

## Theme
- Dark mode hardcoded (no toggle yet)
- CSS vars in `wwwroot/app.css`: `--bg-primary`, `--bg-secondary`, `--text-primary`, `--accent-color`, etc.
- Inter font from Google Fonts
- Workflow steps: card-style blocks with colored markers, glow effects on current step

## Models
- **Topic**: Id, Title, Description, CreatedAt, Articles (nav)
- **Article**: Id, Title, Content (unused), CreatedAt, UpdatedAt, TopicId, Fields (nav)
- **ArticleField**: Id, FieldType (`text`|`workflow`), Content, Order, ExpandByDefault, ArticleId, Steps (nav)
- **WorkflowStep**: Id, Name, Description, Order, Completed, CompletedAt, CompletedBy, ArticleFieldId

## Pages
- `/` — Home
- `/topics` — Topic list (create, delete)
- `/topics/new` — New topic form
- `/topics/{id}` — Topic details with article list
- `/topics/{id}/edit` — Edit topic
- `/topics/{topicId}/articles/new` — New article form (creates initial text field)
- `/articles/{id}` — Article view/edit with inline title editing, field editor, markdown rendering
- `/articles/{id}/edit` — (removed, title edits inline now)
- `/search` — Full-text search across topics and articles

## Field/Block Editor

An article is made of ordered **fields** (ArticleField), each with type `text` or `workflow`.

**View mode:**
- Text fields render as markdown with `block-text` styling (left border accent)
- Workflow fields show as collapsible `block-workflow` cards with summary bar
- Completed workflows collapse by default
- Auto-expand setting per workflow persists in DB
- Toast notification slides in from right when all workflows done (with reset button)
- Global "Reset all workflows" button when any step is complete

**Edit mode (click "Edit"):**
- Title becomes inline editable input
- Fields show as blocks with drag handles, up/down arrows, delete button
- Text fields: textarea with markdown hint
- Workflow fields: step list (name, description, complete/remove), "Auto-expand" checkbox, add step form
- SortableJS drag-and-drop reordering
- Adding fields via "+ Text" / "+ Workflow" buttons

## Workflow Features
- Steps have name, multiline description, order, completion state
- Current (first incomplete) step shown with blue border + glow
- Completed steps dimmed with green checkmark and timestamp
- Step descriptions viewable in both modes
- Reset per workflow or global reset
- CompletedBy field ready for future user system

## Quick Commands
- `dotnet run` — start dev server
- `dotnet build` — build check
- `dotnet watch run` — hot reload
- `dotnet ef migrations add <name>` — add migration
- `dotnet ef database update` — apply migration
