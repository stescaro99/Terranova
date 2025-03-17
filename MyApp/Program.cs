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
using System.Text.Json.Serialization;
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
                            for (int i = 0; i < detailedDrink.Ingredients.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(detailedDrink.Ingredients[i]))
                                {
                                    Console.WriteLine($"  - {detailedDrink.Ingredients[i]}");
                                }
                            }
                            Console.WriteLine();
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
            public string IdDrink { get; set; } = string.Empty;
            public string StrDrink { get; set; } = string.Empty;
            public string StrInstructions { get; set; } = string.Empty;

            private string[] ingredients = new string[10];
            public string[] Ingredients
            {
                get
                {
                    return ingredients;
                }
            }

            [JsonPropertyName("strIngredient1")]
            public string StrIngredient1
            {
                set { ingredients[0] = value; }
            }

            [JsonPropertyName("strIngredient2")]
            public string StrIngredient2
            {
                set { ingredients[1] = value; }
            }

            [JsonPropertyName("strIngredient3")]
            public string StrIngredient3
            {
                set { ingredients[2] = value; }
            }

            [JsonPropertyName("strIngredient4")]
            public string StrIngredient4
            {
                set { ingredients[3] = value; }
            }

            [JsonPropertyName("strIngredient5")]
            public string StrIngredient5
            {
                set { ingredients[4] = value; }
            }

            [JsonPropertyName("strIngredient6")]
            public string StrIngredient6
            {
                set { ingredients[5] = value; }
            }

            [JsonPropertyName("strIngredient7")]
            public string StrIngredient7
            {
                set { ingredients[6] = value; }
            }

            [JsonPropertyName("strIngredient8")]
            public string StrIngredient8
            {
                set { ingredients[7] = value; }
            }

            [JsonPropertyName("strIngredient9")]
            public string StrIngredient9
            {
                set { ingredients[8] = value; }
            }

            [JsonPropertyName("strIngredient10")]
            public string StrIngredient10
            {
                set { ingredients[9] = value; }
            }
        }

        public class CocktailApiResponse
        {
            public List<CocktailData> Drinks { get; set; } = new List<CocktailData>();
        }
    }
}