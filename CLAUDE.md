# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore MiApp.slnx

# Build solution
dotnet build MiApp.slnx

# Run API (listens on http://localhost:5223)
dotnet run --project MiApp.API/MiApp.API.csproj --launch-profile http
```

The SQLite database (`miapp.db`) is auto-created and seeded on first run — no manual setup required.

## Architecture

This is a **Clean Architecture** ASP.NET Core 10 API with four layers:

- **MiApp.Domain** — entities (`User`), enums (`Role`), and repository interfaces. No dependencies on other layers.
- **MiApp.Application** — CQRS via MediatR. Features are organized under `Features/<Area>/Commands|Queries`. FluentValidation is wired as a MediatR pipeline behavior (`ValidationBehavior`) that runs before every handler. Defines service interfaces (`IJwtTokenService`, `IPasswordHasher`).
- **MiApp.Infrastructure** — EF Core + SQLite (`AppDbContext`), repository implementations, BCrypt password hashing (`BcryptPasswordHasher`), JWT generation (`JwtTokenService`), and `DbSeeder` for initial users.
- **MiApp.API** — ASP.NET Core host. Controllers dispatch through MediatR (`ISender`). `GlobalExceptionHandler` converts `ValidationException` → 400 and `UnauthorizedAccessException` → 401 using ProblemDetails.

### Adding a new feature

Follow the existing Login pattern:
1. Create `Features/<Area>/Commands/<Name>/<Name>Command.cs` — define the MediatR `IRequest` record.
2. Create `<Name>CommandHandler.cs` implementing `IRequestHandler`.
3. Create `<Name>CommandValidator.cs` with FluentValidation rules (auto-registered, auto-executed via pipeline).
4. Inject `ISender` in the controller and call `_sender.Send(command)`.

### Authentication & Authorization

JWT tokens expire after 8 hours. Roles (`Admin`, `Reader`) are embedded as `ClaimTypes.Role` claims. Controllers use `[Authorize(Roles = "Admin")]` or `[Authorize(Roles = "Reader")]`.

Seed credentials (dev only):
- Admin: `admin@miapp.com` / `Admin123!`
- Reader: `reader@miapp.com` / `Reader123!`

### CORS

Allowed origins are configured in `appsettings.json` under `Cors:AllowedOrigins`. Development defaults allow `localhost:3000` and `localhost:3001`.

## API Testing

A Bruno collection is in the `bruno/` folder with requests grouped under `Auth/`, `Admin/`, and `Reader/`. Use the `local` environment (`bruno/environments/local.bru`). Login first, then use the returned token for protected endpoints.
