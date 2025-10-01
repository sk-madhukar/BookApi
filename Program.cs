using BookApi.Data;
using BookApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//SQL Server Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Register EF Core
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//Register SQL connection for Dapper
builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(connectionString));

builder.Services.AddSingleton<BookService>();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();