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
                url += "random.php";
            }

            var response = await client.GetStringAsync(url);

            try
            {
                var data = JsonSerializer.Deserialize<CocktailApiResponse>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
                            Console.WriteLine("Ingredients:");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient1)) Console.WriteLine($"  - {detailedDrink.StrIngredient1}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient2)) Console.WriteLine($"  - {detailedDrink.StrIngredient2}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient3)) Console.WriteLine($"  - {detailedDrink.StrIngredient3}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient4)) Console.WriteLine($"  - {detailedDrink.StrIngredient4}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient5)) Console.WriteLine($"  - {detailedDrink.StrIngredient5}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient6)) Console.WriteLine($"  - {detailedDrink.StrIngredient6}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient7)) Console.WriteLine($"  - {detailedDrink.StrIngredient7}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient8)) Console.WriteLine($"  - {detailedDrink.StrIngredient8}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient9)) Console.WriteLine($"  - {detailedDrink.StrIngredient9}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient10)) Console.WriteLine($"  - {detailedDrink.StrIngredient10}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient11)) Console.WriteLine($"  - {detailedDrink.StrIngredient11}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient12)) Console.WriteLine($"  - {detailedDrink.StrIngredient12}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient13)) Console.WriteLine($"  - {detailedDrink.StrIngredient13}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient14)) Console.WriteLine($"  - {detailedDrink.StrIngredient14}");
                            if (!string.IsNullOrEmpty(detailedDrink.StrIngredient15)) Console.WriteLine($"  - {detailedDrink.StrIngredient15}");
                        }

                        // Delay to avoid too many requests in a short time
                        await Task.Delay(500);
                    }
                }
                else
                {
                    Console.WriteLine("No cocktails found.");
                }
                Console.WriteLine();
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

            public string IdDrink { get; set; } = string.Empty;
            public string StrInstructions { get; set; } = string.Empty;
            public string StrIngredient1 { get; set; } = string.Empty;
            public string StrIngredient2 { get; set; } = string.Empty;
            public string StrIngredient3 { get; set; } = string.Empty;
            public string StrIngredient4 { get; set; } = string.Empty;
            public string StrIngredient5 { get; set; } = string.Empty;
            public string StrIngredient6 { get; set; } = string.Empty;
            public string StrIngredient7 { get; set; } = string.Empty;
            public string StrIngredient8 { get; set; } = string.Empty;
            public string StrIngredient9 { get; set; } = string.Empty;
            public string StrIngredient10 { get; set; } = string.Empty;
            public string StrIngredient11 { get; set; } = string.Empty;
            public string StrIngredient12 { get; set; } = string.Empty;
            public string StrIngredient13 { get; set; } = string.Empty;
            public string StrIngredient14 { get; set; } = string.Empty;
            public string StrIngredient15 { get; set; } = string.Empty;
        }

        public class CocktailApiResponse
        {
            public List<CocktailData> Drinks { get; set; } = new List<CocktailData>();
        }
    }
}