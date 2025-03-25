﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CocktailDebacle.API.Migrations
{
    [DbContext(typeof(CocktailDbContext))]
    [Migration("20250324110032_ConfigureDrinkMapping")]
    partial class ConfigureDrinkMapping
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cocktail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Cocktails");
                });

            modelBuilder.Entity("CocktailUser", b =>
                {
                    b.Property<int>("FavoriteByUsersId")
                        .HasColumnType("int");

                    b.Property<int>("FavoriteCocktailsId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteByUsersId", "FavoriteCocktailsId");

                    b.HasIndex("FavoriteCocktailsId");

                    b.ToTable("UserFavoriteCocktails", (string)null);
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AppPermissions")
                        .HasColumnType("bit");

                    b.PrimitiveCollection<string>("BirthDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CanDrinkAlcohol")
                        .HasColumnType("bit");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(58)
                        .HasColumnType("nvarchar(58)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Cocktail", b =>
                {
                    b.HasOne("User", "CreatedByUser")
                        .WithMany("CreatedCocktails")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("CocktailApiDrink", "Drink", b1 =>
                        {
                            b1.Property<int>("CocktailId")
                                .HasColumnType("int");

                            b1.Property<string>("IdDrink")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_IdDrink")
                                .HasAnnotation("Relational:JsonPropertyName", "IdDrink");

                            b1.Property<string>("StrCategory")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrCategory")
                                .HasAnnotation("Relational:JsonPropertyName", "strCategory");

                            b1.Property<string>("StrDrink")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrDrink")
                                .HasAnnotation("Relational:JsonPropertyName", "strDrink");

                            b1.Property<string>("StrDrinkThumb")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrDrinkThumb")
                                .HasAnnotation("Relational:JsonPropertyName", "strDrinkThumb");

                            b1.Property<string>("StrGlass")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrGlass")
                                .HasAnnotation("Relational:JsonPropertyName", "strGlass");

                            b1.Property<string>("StrIngredient1")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient1")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient1");

                            b1.Property<string>("StrIngredient2")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient2")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient2");

                            b1.Property<string>("StrIngredient3")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient3")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient3");

                            b1.Property<string>("StrIngredient4")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient4")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient4");

                            b1.Property<string>("StrIngredient5")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient5")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient5");

                            b1.Property<string>("StrIngredient6")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrIngredient6")
                                .HasAnnotation("Relational:JsonPropertyName", "strIngredient6");

                            b1.Property<string>("StrInstructions")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrInstructions")
                                .HasAnnotation("Relational:JsonPropertyName", "strInstructions");

                            b1.Property<string>("StrMeasure1")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure1")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure1");

                            b1.Property<string>("StrMeasure2")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure2")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure2");

                            b1.Property<string>("StrMeasure3")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure3")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure3");

                            b1.Property<string>("StrMeasure4")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure4")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure4");

                            b1.Property<string>("StrMeasure5")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure5")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure5");

                            b1.Property<string>("StrMeasure6")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Drink_StrMeasure6")
                                .HasAnnotation("Relational:JsonPropertyName", "strMeasure6");

                            b1.HasKey("CocktailId");

                            b1.ToTable("Cocktails");

                            b1.WithOwner()
                                .HasForeignKey("CocktailId");
                        });

                    b.Navigation("CreatedByUser");

                    b.Navigation("Drink")
                        .IsRequired();
                });

            modelBuilder.Entity("CocktailUser", b =>
                {
                    b.HasOne("User", null)
                        .WithMany()
                        .HasForeignKey("FavoriteByUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cocktail", null)
                        .WithMany()
                        .HasForeignKey("FavoriteCocktailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("CreatedCocktails");
                });
#pragma warning restore 612, 618
        }
    }
}
