using BookApi.Data;
using BookApi.Services;

var builder = WebApplication.CreateBuilder(args);

//SQL Server Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Register EF Core
//builder.Services.AddDbContext<AppDbContext>(o)

builder.Services.AddSingleton<BookService>();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();