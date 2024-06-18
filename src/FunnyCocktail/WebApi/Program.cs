using Application.Interfaces;
using Application.Services;
using Domain.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

const string PolicyName = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Development";
var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{env}.json")
    .Build();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAuthenticationConfirmService, AuthenticationConfirmService>();
builder.Services.AddScoped<IAuthenticationSendingService, AuthenticationSendingService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICocktailService, CocktailService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddDbContextPool<ApplicationDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration[$"ConnectionStrings:{env}"], b => b.MigrationsAssembly("WebApi"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PolicyName, config =>
    {
        config.WithOrigins(env == "Production" ? "https://funnycocktail.com" : "http://localhost:8000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,

            ValidIssuer = builder.Configuration[$"JWT:ValidIssuer:{env}"],
            ValidAudience = builder.Configuration[$"JWT:ValidAudience:{env}"],

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration[$"JWT:Secret:{env}"]!)),

            ValidateIssuerSigningKey = true
        };
    }
);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "FunnyCocktail",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Example: \"Bearer [token]\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();   
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(PolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "images")),
    RequestPath = "/images"
});

app.Run();