using SaqueroCloud.API.Models.Entities;

namespace SaqueroCloud.API.Repositories.Interfaces;

public interface ISubscriptionPlanRepository
{
    Task<IEnumerable<SubscriptionPlan>> GetAllAsync();
    Task<SubscriptionPlan?> GetByIdAsync(int id);
    Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan);
    Task<SubscriptionPlan?> UpdateAsync(int id, SubscriptionPlan plan);
    Task<bool> DeleteAsync(int id);
}
