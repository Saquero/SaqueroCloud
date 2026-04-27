namespace SaqueroCloud.API.Models.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;         // Basic, Pro, Enterprise
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MaxUsers { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
