using SaqueroCloud.API.Models.Entities;

namespace SaqueroCloud.API.Repositories.Interfaces;

public interface IUserSubscriptionRepository
{
    Task<IEnumerable<UserSubscription>> GetAllActiveAsync();
    Task<IEnumerable<UserSubscription>> GetByUserIdAsync(int userId);
    Task<UserSubscription?> GetByIdAsync(int id);
    Task<UserSubscription> CreateAsync(UserSubscription subscription);
    Task<bool> DeactivateAsync(int id);
}
