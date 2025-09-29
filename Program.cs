using BookApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BookService>();
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();