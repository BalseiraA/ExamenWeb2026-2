using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketSystem.API;
using TicketSystem.Application;
using TicketSystem.Infrastructure;
using TicketSystem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- Capas de la aplicación ---
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// --- CORS para el frontend (Vite) ---
const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Autenticación JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// --- Manejo global de excepciones (ProblemDetails) ---
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// --- Swagger con soporte para JWT (botón Authorize) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketSystem API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT (sin la palabra 'Bearer').",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// --- Migración + seed de la base de datos al arrancar ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketSystem API v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz: http://localhost:5227/
    });
}

app.UseCors(CorsPolicy);
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
