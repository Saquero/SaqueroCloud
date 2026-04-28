using SaqueroCloud.API.Models.DTOs;

namespace SaqueroCloud.API.Services.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionPlanDto>> GetAllPlansAsync();
    Task<SubscriptionPlanDto?> GetPlanByIdAsync(int id);
    Task<SubscriptionPlanDto> CreatePlanAsync(CreatePlanDto dto);
    Task<SubscriptionPlanDto?> UpdatePlanAsync(int id, UpdatePlanDto dto);
    Task<bool> DeletePlanAsync(int id);
    Task<UserSubscriptionDto?> AssignSubscriptionAsync(int userId, AssignSubscriptionDto dto);
    Task<IEnumerable<UserSubscriptionDto>> GetActiveSubscriptionsAsync(int? planId = null);
    Task<IEnumerable<UserSubscriptionDto>> GetExpiringSoonAsync(int days);
    Task<IEnumerable<UserSubscriptionDto>> GetUserSubscriptionsAsync(int userId);
    Task<bool> CancelSubscriptionAsync(int subscriptionId);
}


