using Microsoft.EntityFrameworkCore;
using SaqueroCloud.API.Models.Entities;

namespace SaqueroCloud.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasDefaultValue("User");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasOne(us => us.User)
                  .WithMany(u => u.Subscriptions)
                  .HasForeignKey(us => us.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(us => us.Plan)
                  .WithMany(p => p.UserSubscriptions)
                  .HasForeignKey(us => us.PlanId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed: Admin por defecto
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Name = "Manu Admin",
            Email = "admin@saquerocloud.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
            Role = "Admin",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsActive = true
        });

        // Seed: Planes de suscripcion
        modelBuilder.Entity<SubscriptionPlan>().HasData(
            new SubscriptionPlan { Id = 1, Name = "Basic", Description = "Plan basico para equipos pequenos", Price = 9.99m, MaxUsers = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new SubscriptionPlan { Id = 2, Name = "Pro", Description = "Plan profesional con mas capacidad", Price = 29.99m, MaxUsers = 25, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new SubscriptionPlan { Id = 3, Name = "Enterprise", Description = "Plan empresarial sin limites", Price = 99.99m, MaxUsers = 999, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
