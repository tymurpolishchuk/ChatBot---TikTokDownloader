using ChatBot_TikTokDownloader;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
var connection = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<DataContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connection, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddControllers();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.EnsureCreated();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.EnsureCreated();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();