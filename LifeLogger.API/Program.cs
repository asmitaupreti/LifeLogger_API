using LifeLogger.API.Extensions;
using LifeLogger.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

//CORS policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                            "http://localhost:5174",
                            "http://127.0.0.1:5173",
                            "https://*.example.com"
                            )
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                      });
});

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

// app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
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