using System.Text;
using api.src.Data;
using api.src.Interfaces;
using api.src.Models;
using api.src.Repository;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt => 
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;  
    }
).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(
    opt => 
    {
        opt.DefaultAuthenticateScheme = 
        opt.DefaultChallengeScheme =
        opt.DefaultForbidScheme = 
        opt.DefaultScheme =
        opt.DefaultSignInScheme =
        opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt => {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Signingkey"] ?? throw new ArgumentNullException("JWT:Signingkey"))),
        };
        //TODO: PARA LOS ESTUDIANTES, ENVES DE USAR APPSETTINGS.JSON, USAR ENVIRONMENT VARIABLES
    });


string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=app.db"; //True, false
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
    DataSeeder.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();



