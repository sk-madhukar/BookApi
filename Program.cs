using BookApi.Data;
using BookApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//SQL Server Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Register EF Core
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//Register SQL connection for Dapper
builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(connectionString));

//JWT Configuration
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<BookService>();
builder.Services.AddControllers();


var app = builder.Build();

app.UseMiddleware<BookApi.Middlewares.ExceptionMiddleware>();
app.MapControllers();

app.Run();