using AuthService.Data;
using AuthService.Repositories;
using AuthService.Services;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddPostgreDb<AuthDbContext>(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"])),
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateAudience = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace AuthService
{
    // Этот класс нужен для WebApplicationFactory<TEntryPoint>
    public partial class Program { }
}