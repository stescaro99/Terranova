using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace CocktailDebacle
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cocktail API V1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation("Cocktail Débâcle WebApp started successfully.");
            logger.LogInformation("Visit: http://localhost:5000/swagger for API documentation");
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cocktail> Cocktails { get; set; }
    }

    public class Cocktail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CocktailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CocktailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cocktail>> GetCocktails()
        {
            return _context.Cocktails.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Cocktail> GetCocktail(int id)
        {
            var cocktail = _context.Cocktails.Find(id);
            if (cocktail == null)
            {
                return NotFound();
            }
            return cocktail;
        }

        [HttpPost]
        public ActionResult<Cocktail> CreateCocktail(Cocktail cocktail)
        {
            _context.Cocktails.Add(cocktail);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCocktail), new { id = cocktail.Id }, cocktail);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCocktail(int id, Cocktail updatedCocktail)
        {
            var cocktail = _context.Cocktails.Find(id);
            if (cocktail == null)
            {
                return NotFound();
            }
            
            cocktail.Name = updatedCocktail.Name;
            cocktail.Ingredients = updatedCocktail.Ingredients;
            cocktail.Instructions = updatedCocktail.Instructions;
            
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCocktail(int id)
        {
            var cocktail = _context.Cocktails.Find(id);
            if (cocktail == null)
            {
                return NotFound();
            }
            
            _context.Cocktails.Remove(cocktail);
            _context.SaveChanges();
            return NoContent();
        }
    }

    public class CocktailTests
    {
        private readonly HttpClient _client;

        public CocktailTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task CreateCocktail_ReturnsCreated()
        {
            var newCocktail = new Cocktail { Name = "Mojito", Ingredients = "Rum, Menta, Zucchero, Lime, Soda", Instructions = "Mescola tutto e servi con ghiaccio" };
            var content = new StringContent(JsonConvert.SerializeObject(newCocktail), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/cocktails", content);
            
            response.EnsureSuccessStatusCode();
        }
    }
}