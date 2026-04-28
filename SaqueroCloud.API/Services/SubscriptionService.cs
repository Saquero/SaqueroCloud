using SaqueroCloud.API.Models.DTOs;
using SaqueroCloud.API.Models.Entities;
using SaqueroCloud.API.Repositories.Interfaces;
using SaqueroCloud.API.Services.Interfaces;

namespace SaqueroCloud.API.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;

    public SubscriptionService(
        ISubscriptionPlanRepository planRepository,
        IUserSubscriptionRepository subscriptionRepository,
        IUserRepository userRepository)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<SubscriptionPlanDto>> GetAllPlansAsync()
    {
        var plans = await _planRepository.GetAllAsync();
        return plans.Select(MapPlanToDto);
    }

    public async Task<SubscriptionPlanDto?> GetPlanByIdAsync(int id)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        return plan is null ? null : MapPlanToDto(plan);
    }

    public async Task<SubscriptionPlanDto> CreatePlanAsync(CreatePlanDto dto)
    {
        var plan = new SubscriptionPlan
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            MaxUsers = dto.MaxUsers,
            IsActive = true
        };

        var created = await _planRepository.CreateAsync(plan);
        return MapPlanToDto(created);
    }

    public async Task<SubscriptionPlanDto?> UpdatePlanAsync(int id, UpdatePlanDto dto)
    {
        var planUpdate = new SubscriptionPlan
        {
            Description = dto.Description ?? string.Empty,
            Price = dto.Price ?? 0,
            MaxUsers = dto.MaxUsers ?? 0,
            IsActive = dto.IsActive ?? true
        };

        var updated = await _planRepository.UpdateAsync(id, planUpdate);
        return updated is null ? null : MapPlanToDto(updated);
    }

    public async Task<bool> DeletePlanAsync(int id)
    {
        return await _planRepository.DeleteAsync(id);
    }

    public async Task<UserSubscriptionDto?> AssignSubscriptionAsync(int userId, AssignSubscriptionDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) return null;

        var plan = await _planRepository.GetByIdAsync(dto.PlanId);
        if (plan is null || !plan.IsActive) return null;

        var subscription = new UserSubscription
        {
            UserId = userId,
            PlanId = dto.PlanId,
            StartDate = DateTime.UtcNow,
            EndDate = dto.EndDate,
            IsActive = true
        };

        var created = await _subscriptionRepository.CreateAsync(subscription);

        return new UserSubscriptionDto
        {
            Id = created.Id,
            UserName = user.Name,
            UserEmail = user.Email,
            PlanName = plan.Name,
            PlanPrice = plan.Price,
            StartDate = created.StartDate,
            EndDate = created.EndDate,
            IsActive = created.IsActive
        };
    }

    public async Task<IEnumerable<UserSubscriptionDto>> GetActiveSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllActiveAsync();
        return subscriptions.Select(MapSubscriptionToDto);
    }

    public async Task<IEnumerable<UserSubscriptionDto>> GetExpiringSoonAsync(int days)
    {
        var limitDate = DateTime.UtcNow.AddDays(days);
        var subscriptions = await _subscriptionRepository.GetAllActiveAsync();

        return subscriptions
            .Where(s => s.EndDate <= limitDate)
            .Select(MapSubscriptionToDto);
    }
    public async Task<IEnumerable<UserSubscriptionDto>> GetUserSubscriptionsAsync(int userId)
    {
        var subscriptions = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subscriptions.Select(s => new UserSubscriptionDto
        {
            Id = s.Id,
            UserName = string.Empty,
            UserEmail = string.Empty,
            PlanName = s.Plan.Name,
            PlanPrice = s.Plan.Price,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            IsActive = s.IsActive
        });
    }

    public async Task<bool> CancelSubscriptionAsync(int subscriptionId)
    {
        return await _subscriptionRepository.DeactivateAsync(subscriptionId);
    }

    private static SubscriptionPlanDto MapPlanToDto(SubscriptionPlan plan) => new()
    {
        Id = plan.Id,
        Name = plan.Name,
        Description = plan.Description,
        Price = plan.Price,
        MaxUsers = plan.MaxUsers,
        IsActive = plan.IsActive
    };

    private static UserSubscriptionDto MapSubscriptionToDto(UserSubscription sub) => new()
    {
        Id = sub.Id,
        UserName = sub.User.Name,
        UserEmail = sub.User.Email,
        PlanName = sub.Plan.Name,
        PlanPrice = sub.Plan.Price,
        StartDate = sub.StartDate,
        EndDate = sub.EndDate,
        IsActive = sub.IsActive
    };
}

