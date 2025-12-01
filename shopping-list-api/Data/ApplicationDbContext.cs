using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Models;

namespace ShoppingListApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    public DbSet<ShoppingListUser> ShoppingListUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // Configure Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure ShoppingList
        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        // Configure ShoppingListItem
        modelBuilder.Entity<ShoppingListItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Quantity).HasMaxLength(50);

            entity.HasOne(e => e.ShoppingList)
                .WithMany(s => s.Items)
                .HasForeignKey(e => e.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.AddedByUser)
                .WithMany(u => u.AddedItems)
                .HasForeignKey(e => e.AddedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ShoppingListUser (many-to-many join table)
        modelBuilder.Entity<ShoppingListUser>(entity =>
        {
            entity.HasKey(e => new { e.ShoppingListId, e.UserId });

            entity.HasOne(e => e.ShoppingList)
                .WithMany(s => s.ShoppingListUsers)
                .HasForeignKey(e => e.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.ShoppingListUsers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
