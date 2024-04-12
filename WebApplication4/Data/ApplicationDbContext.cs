using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<Ingredient> Ingredient { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
    public DbSet<RecipeRanking> RecipeRanking { get; set; }
    public DbSet<UserRanking> UserRanking { get; set; }
    public DbSet<AppRating> AppRating { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Definiowanie klucza złożonego dla RecipeIngredient
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });
        
        modelBuilder.Entity<RecipeRanking>()
            .HasKey(rr => new { rr.RecipeId, rr.UserId });
    }
}