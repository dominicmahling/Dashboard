# Dashboard Project

## Stack
- **Framework**: .NET 10, Blazor Server (Interactive Server mode)
- **Database**: SQLite + EF Core (`IDbContextFactory<AppDbContext>`)
- **UI**: Bootstrap 5, Bootstrap Icons, custom dark theme with Inter font
- **Markdown**: Markdig library
- **Auth**: None yet (local single-user)
- **SortableJS**: For drag-and-drop reordering in field editor (loaded via CDN)

## Conventions
- Razor components in `Components/Pages/` (feature folders) and `Components/Layout/`
- Models in `Models/`, DbContext in `Data/`, Services in `Services/`
- Use `@rendermode InteractiveServer` for pages needing interactivity
- Inject `IDbContextFactory<AppDbContext>` — always `await using var db = await DbFactory.CreateDbContextAsync();`
- Navigation: inject `NavigationManager`, call `NavigationManager.NavigateTo()`
- JS interop: `IJSRuntime.InvokeVoidAsync()` — avoid `eval()`, register global functions in `wwwroot/js/app.js`
- Static JS files go in `wwwroot/js/`, loaded via `<script src="@Assets["js/filename.js"]">` in `App.razor`

## Database
- Connection string in `appsettings.json` → `Data Source=dashboard.db`
- `db.Database.Migrate()` + `SeedData.InitializeAsync(factory)` in Program.cs
- EF migrations track schema changes
- SQLite files gitignored
- Seed data in `Data/SeedData.cs` — creates "Getting Started" topic with a 3-step workflow demo (each step is a script step)

## Models

- **Topic**: Id, Title, Description, CreatedAt, Articles (nav)
- **Article**: Id, Title, Content (unused), CreatedAt, UpdatedAt, TopicId, Fields (nav)
- **ArticleField**: Id, FieldType (`text`|`workflow`|`script`), Content, ScriptOutput, Order, ExpandByDefault, ArticleId, Steps (nav)
- **WorkflowStep**: Id, Name, Description, Order, Completed, CompletedAt, CompletedBy, IsScriptStep, Script, ScriptOutput, ArticleFieldId

## Services

- **ScriptExecutionService** (`Services/ScriptExecutionService.cs`): Scoped. Executes PowerShell scripts via `System.Diagnostics.Process`. Calls `powershell.exe -NoProfile -ExecutionPolicy Bypass -` and pipes script via stdin. Returns `ScriptResult` (Success, ExitCode, Output, Error). 60s timeout.

## Pages
- `/` — Home (stats grid with topic/article counts, recent articles list)
- `/topics` — Topic list as card grid (create, delete)
- `/topics/new` — New topic form
- `/topics/{id}` — Topic details with article list as styled rows
- `/topics/{id}/edit` — Edit topic
- `/topics/{topicId}/articles/new` — New article form (creates initial text field)
- `/articles/{id}` — Article view/edit with inline title editing, field editor, markdown rendering
- `/search` — Pill-shaped search input, list-style results with type icons

## Article / Field Editor

An article is made of ordered **fields** (ArticleField), each with type `text`, `workflow`, or `script`.

**View mode:**
- Text fields render as markdown with `block-text` styling (left border accent)
- Workflow fields show as collapsible `block-workflow` cards with summary bar, step list, step completion
  - Each step can be a **script step** (IsScriptStep = true) — shows collapsible "Script" toggle with code preview + Run button + output
- Script fields render as `block-script` cards with code block + Execute button + output
- Completed workflows collapse by default
- Toast notification slides in from right when all workflows done (with reset button)
- Global "Reset all workflows" button when any step is complete
- Fade-in content animation on page load

**Edit mode (click "Edit"):**
- Title becomes inline editable input
- Fields show as blocks with drag handles, up/down arrows, delete button
- Text fields: textarea with markdown hint
- Workflow fields: step list with inline script editor per step (if IsScriptStep is checked)
  - Each step has a "Script step" checkbox — when checked, shows "▶ Editor" toggle
  - Script editor has monospace textarea + "Run script" button + output display + Clear button
  - Unchecking "Script step" clears script content and output
  - Script changes persist when clicking "Done Editing" (SaveAllFields saves WorkflowStep.Script)
- Script fields: monospace textarea + Execute button + output display
- Adding fields via "+ Text" / "+ Workflow" / "+ Script" buttons
- SortableJS drag-and-drop reordering

## Workflow Features
- Steps have name, multiline description, order, completion state
- Current (first incomplete) step shown with blue border + glow (pulse animation)
- Completed steps dimmed with green checkmark + timestamp
- Step connector lines (gradient vertical bars between steps, green when done, accent when current)
- Step done pop animation on marker
- Staggered entrance animation for step list
- Script per step: optional PowerShell script, executtion via ScriptExecutionService
- Reset per workflow or global reset
- CompletedBy field ready for future user system

## Script Execution
- PowerShell scripts run via `ScriptExecutionService` (scoped service)
- Script content stored in `WorkflowStep.Script` or `ArticleField.Content` (for standalone script fields)
- Output stored as JSON-serialized `ScriptResult` in `ScriptOutput`
- Execution tracked per step/field via `executingStepId`/`executingFieldId` state variables
- 60-second timeout, process killed if exceeded

## Visual Design (app.css)
- Fully custom dark theme with CSS variables
- Noise texture overlay on body (SVG grain)
- Gradient text on h1 headings
- Glassmorphism back button (frosted glass, backdrop-filter)
- Gradient borders on hover via ::before pseudo-elements (cards, topic cards, article rows, workflow blocks, search results, etc.)
- Smooth transitions on all interactive elements (0.2s cubic-bezier)
- Custom webkit scrollbar styling
- Button ripple effect (radial gradient overlay)
- Skeleton loading screen styles (.skeleton-grid, .skeleton-card, .skeleton-row)
- CSS-art empty state illustrations (folder, document, magnifying glass)
- Step connectors with gradient lines
- Floating animation on empty state illustrations
- Responsive breakpoints at 768px

## Back Button
- `<a>` tag in MainLayout with class `back-button` (static SSR — no Blazor event handlers, plain HTML)
- Glassmorphism pill with accent-colored icon circle
- JS module `DashboardNavigation` in `wwwroot/js/app.js` dynamically sets `href` to parent URL path and hides button when at root `/`
- Parent path logic: `/topics/1` → `/topics`, `/topics` → `/`, `/` → hidden
- Overrides `history.pushState` + `popstate` to update button on navigation
- Blazor enhanced navigation intercepts the `<a>` click for client-side routing (no full reload)
- Never navigates outside the dashboard — always goes to a parent path within the app

## Migrations
- `20260512203544_InitialCreate` — initial schema
- `20260512204953_AddCompletedByField` — added CompletedBy
- `20260512210726_AddExpandByDefault` — added ExpandByDefault to ArticleField
- `20260516114110_AddScriptOutput` — added ScriptOutput to ArticleField
- `20260516114518_AddStepScriptSupport` — added Script/ScriptOutput to WorkflowStep
- `20260516115155_AddIsScriptStep` — added IsScriptStep to WorkflowStep

## Quick Commands
- `dotnet run` — start dev server
- `dotnet build` — build check
- `dotnet watch run` — hot reload
- `dotnet ef migrations add <name>` — add migration
- `dotnet ef database update` — apply migration
