using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonException = System.Text.Json.JsonException;


namespace CocktailDebacle
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Console.WriteLine("Starting Cocktail Débâcle WebApp...");
            Console.WriteLine("Server running on: http://localhost:5000");
            Console.WriteLine("Swagger UI available at: http://localhost:5000/swagger");
            
            await FetchCocktailData(args.Length > 0 ? args[0] : string.Empty);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task FetchCocktailData(string str)
        {
            using var client = new HttpClient();
            string url = "https://www.thecocktaildb.com/api/json/v1/1/";

            if (!string.IsNullOrEmpty(str))
            {
                url += $"search.php?s={str}";
            }
            else
            {
                url += "filter.php?c=Cocktail"; // This will fetch all cocktails
            }

            var response = await client.GetStringAsync(url);

            try
            {
                var data = JsonSerializer.Deserialize<CocktailFilterResponse>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Console.WriteLine("\nCocktails retrieved from API:");
                if (data?.Drinks != null && data.Drinks.Count > 0)
                {
                    foreach (var drink in data.Drinks)
                    {
                        var detailResponse = await client.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drink.IdDrink}");
                        var detailData = JsonSerializer.Deserialize<CocktailDetailResponse>(detailResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (detailData?.Drinks != null && detailData.Drinks.Count > 0)
                        {
                            var detailedDrink = detailData.Drinks[0];
                            Console.WriteLine($"- {detailedDrink.StrDrink}: {detailedDrink.StrInstructions}");
                        }

                        // deley per evitare le troppe richieste in poco tempo
                        await Task.Delay(500);
                    }
                }
                else
                {
                    Console.WriteLine("No cocktails found.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                Console.WriteLine("Response JSON:");
                Console.WriteLine(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
            }
        }

        public class CocktailFilterResponse
        {
            public List<CocktailFilterData> Drinks { get; set; } = new List<CocktailFilterData>();
        }

        public class CocktailFilterData
        {
            public string IdDrink { get; set; } = string.Empty;
            public string StrDrink { get; set; } = string.Empty;
        }

        public class CocktailDetailResponse
        {
            public List<CocktailData> Drinks { get; set; } = new List<CocktailData>();
        }

        public class CocktailData
        {
            public string StrDrink { get; set; } = string.Empty;
            public string StrInstructions { get; set; } = string.Empty;
        }
    }

    public class CocktailApiResponse
    {
        public List<CocktailData> Drinks { get; set; } = new List<CocktailData>();
    }


    public class CocktailData
    {
        public string StrDrink { get; set; } = string.Empty;
        public string StrInstructions { get; set; } = string.Empty;
    }
}
