using SaqueroCloud.API.Models.Entities;

namespace SaqueroCloud.API.Repositories.Interfaces;

public interface IUserSubscriptionRepository
{
    Task<IEnumerable<UserSubscription>> GetAllActiveAsync();
    Task<IEnumerable<UserSubscription>> GetExpiringAsync(int days);
    Task<IEnumerable<UserSubscription>> GetByUserIdAsync(int userId);
    Task<UserSubscription?> GetByIdAsync(int id);
    Task<int> CountActiveByPlanAsync(int planId);
    Task<UserSubscription> CreateAsync(UserSubscription subscription);
    Task<UserSubscription?> UpdateAsync(int id, int planId, DateTime endDate);
    Task<bool> DeactivateAsync(int id);
}
