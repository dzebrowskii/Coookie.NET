using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<AppRating> AppRatings { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeRanking> RecipeRankings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}