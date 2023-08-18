using LifeLogger.API.Extensions;
using LifeLogger.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityServiceExtensions(builder.Configuration);
builder.Services.AddSwaggerService();
var app = builder.Build();

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
app.AddGlobalExceptionMiddleware();

SeedDatabase();

app.Run();


void SeedDatabase() {
    try
    {
        using (var scope = app.Services.CreateScope()) {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
    }
    catch (Exception ex)
    { 
        Console.WriteLine($"Heyyyyyy seee this {ex}");
    }
    
}