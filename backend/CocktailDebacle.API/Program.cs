using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configura i servizi
builder.Services.AddHttpClient<CocktailApiService>();
builder.Services.AddDbContext<CocktailDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IDeepSeekService, DeepSeekService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cocktail Débâcle API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configura Swagger per l'ambiente di sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cocktail Débâcle API v1");
    });
}

// Applica automaticamente le migrazioni al runtime
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CocktailDbContext>();
    var apiService = scope.ServiceProvider.GetRequiredService<CocktailApiService>();

    if (!context.Database.CanConnect())
    {
        context.Database.Migrate();

        // Populate the database if empty
        if (!context.Cocktails.Any())
        {
            var controller = new CocktailController(context, scope.ServiceProvider.GetRequiredService<IDeepSeekService>());
            await controller.FastPopulate(apiService);
        }
    }
}

// Configura la directory per le immagini caricate
var uploadedImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
if (!Directory.Exists(uploadedImagesPath))
{
    Directory.CreateDirectory(uploadedImagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadedImagesPath),
    RequestPath = "/images"
});

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();