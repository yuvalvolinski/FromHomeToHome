using FromHomeToHome.Backend.Services;
using FromHomeToHome.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:3000");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// רישום רכיבי המערכת ב-DI
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IPlaceImageService, PlaceImageService>();
builder.Services.AddScoped<DbInitializer>();

var app = builder.Build();

// Initialize database on startup
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeDatabaseAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Test endpoints
app.MapGet("/", () => "Hello World!");
app.MapGet("/signup", (string name) => "hello " + name);

// Note: HTTPS redirection disabled for local development
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
