namespace SaqueroCloud.API.Models.Entities;

public class UserSubscription
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PlanId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = null!;
}
