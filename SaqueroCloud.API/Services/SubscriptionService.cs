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
        _planRepository          = planRepository;
        _subscriptionRepository  = subscriptionRepository;
        _userRepository          = userRepository;
    }

    // ---- Planes ----

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
            Name        = dto.Name,
            Description = dto.Description,
            Price       = dto.Price,
            MaxUsers    = dto.MaxUsers,
            IsActive    = true
        };
        var created = await _planRepository.CreateAsync(plan);
        return MapPlanToDto(created);
    }

    public async Task<SubscriptionPlanDto?> UpdatePlanAsync(int id, UpdatePlanDto dto)
    {
        var planUpdate = new SubscriptionPlan
        {
            Description = dto.Description ?? string.Empty,
            Price       = dto.Price ?? 0,
            MaxUsers    = dto.MaxUsers ?? 0,
            IsActive    = dto.IsActive ?? true
        };
        var updated = await _planRepository.UpdateAsync(id, planUpdate);
        return updated is null ? null : MapPlanToDto(updated);
    }

    public async Task<bool> DeletePlanAsync(int id)
        => await _planRepository.DeleteAsync(id);

    // ---- Suscripciones ----

    public async Task<ServiceResult<UserSubscriptionDto>> AssignSubscriptionAsync(int userId, AssignSubscriptionDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return ServiceResult<UserSubscriptionDto>.Fail("Usuario no encontrado.");

        var plan = await _planRepository.GetByIdAsync(dto.PlanId);
        if (plan is null || !plan.IsActive)
            return ServiceResult<UserSubscriptionDto>.Fail("Plan no encontrado o inactivo.");

        var activeCount = await _subscriptionRepository.CountActiveByPlanAsync(dto.PlanId);
        if (activeCount >= plan.MaxUsers)
            return ServiceResult<UserSubscriptionDto>.Fail("El plan ha alcanzado el numero maximo de usuarios activos.");

        if (dto.EndDate <= DateTime.UtcNow)
            return ServiceResult<UserSubscriptionDto>.Fail("La fecha de fin debe ser futura.");

        var subscription = new UserSubscription
        {
            UserId    = userId,
            PlanId    = dto.PlanId,
            StartDate = DateTime.UtcNow,
            EndDate   = dto.EndDate,
            IsActive  = true
        };

        var created = await _subscriptionRepository.CreateAsync(subscription);

        return ServiceResult<UserSubscriptionDto>.Ok(new UserSubscriptionDto
        {
            Id         = created.Id,
            UserId     = user.Id,
            UserName   = user.Name,
            UserEmail  = user.Email,
            PlanId     = plan.Id,
            PlanName   = plan.Name,
            PlanPrice  = plan.Price,
            StartDate  = created.StartDate,
            EndDate    = created.EndDate,
            IsActive   = created.IsActive
        });
    }

    public async Task<ServiceResult<UserSubscriptionDto>> UpdateSubscriptionAsync(int subscriptionId, UpdateSubscriptionDto dto)
    {
        var sub = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (sub is null)
            return ServiceResult<UserSubscriptionDto>.Fail("Suscripcion no encontrada.");

        var plan = await _planRepository.GetByIdAsync(dto.PlanId);
        if (plan is null || !plan.IsActive)
            return ServiceResult<UserSubscriptionDto>.Fail("Plan no encontrado o inactivo.");

        if (dto.EndDate <= DateTime.UtcNow)
            return ServiceResult<UserSubscriptionDto>.Fail("La fecha de fin debe ser futura.");

        // Si cambia de plan, validar maxUsers (excluyendo la suscripcion actual)
        if (sub.PlanId != dto.PlanId)
        {
            var activeCount = await _subscriptionRepository.CountActiveByPlanAsync(dto.PlanId);
            if (activeCount >= plan.MaxUsers)
                return ServiceResult<UserSubscriptionDto>.Fail("El plan ha alcanzado el numero maximo de usuarios activos.");
        }

        var updated = await _subscriptionRepository.UpdateAsync(subscriptionId, dto.PlanId, dto.EndDate);
        if (updated is null)
            return ServiceResult<UserSubscriptionDto>.Fail("No se pudo actualizar la suscripcion.");

        return ServiceResult<UserSubscriptionDto>.Ok(MapSubscriptionToDto(updated));
    }

    public async Task<IEnumerable<UserSubscriptionDto>> GetActiveSubscriptionsAsync(int? planId = null)
    {
        var all = await _subscriptionRepository.GetAllActiveAsync();
        if (planId.HasValue)
            all = all.Where(s => s.PlanId == planId.Value);
        return all.Select(MapSubscriptionToDto);
    }

    public async Task<IEnumerable<UserSubscriptionDto>> GetExpiringSubscriptionsAsync(int days)
    {
        var subs = await _subscriptionRepository.GetExpiringAsync(days);
        return subs.Select(MapSubscriptionToDto);
    }

    public async Task<IEnumerable<UserSubscriptionDto>> GetUserSubscriptionsAsync(int userId)
    {
        var subs = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subs.Select(s => new UserSubscriptionDto
        {
            Id        = s.Id,
            UserId    = s.UserId,
            PlanId    = s.Plan.Id,
            PlanName  = s.Plan.Name,
            PlanPrice = s.Plan.Price,
            StartDate = s.StartDate,
            EndDate   = s.EndDate,
            IsActive  = s.IsActive
        });
    }

    public async Task<bool> CancelSubscriptionAsync(int subscriptionId)
        => await _subscriptionRepository.DeactivateAsync(subscriptionId);

    public async Task<IEnumerable<SubscriptionPlanUsageDto>> GetPlanUsageSummaryAsync()
    {
        var plans = await _planRepository.GetAllAsync();
        var result = new List<SubscriptionPlanUsageDto>();

        foreach (var plan in plans)
        {
            var activeCount = await _subscriptionRepository.CountActiveByPlanAsync(plan.Id);
            result.Add(new SubscriptionPlanUsageDto
            {
                PlanId         = plan.Id,
                PlanName       = plan.Name,
                MaxUsers       = plan.MaxUsers,
                ActiveUsers    = activeCount,
                AvailableSlots = Math.Max(0, plan.MaxUsers - activeCount),
                IsFull         = activeCount >= plan.MaxUsers
            });
        }

        return result;
    }

    // ---- Mappers ----

    private static SubscriptionPlanDto MapPlanToDto(SubscriptionPlan plan) => new()
    {
        Id          = plan.Id,
        Name        = plan.Name,
        Description = plan.Description,
        Price       = plan.Price,
        MaxUsers    = plan.MaxUsers,
        IsActive    = plan.IsActive
    };

    private static UserSubscriptionDto MapSubscriptionToDto(UserSubscription sub) => new()
    {
        Id        = sub.Id,
        UserId    = sub.UserId,
        UserName  = sub.User?.Name  ?? string.Empty,
        UserEmail = sub.User?.Email ?? string.Empty,
        PlanId    = sub.Plan?.Id    ?? sub.PlanId,
        PlanName  = sub.Plan?.Name  ?? string.Empty,
        PlanPrice = sub.Plan?.Price ?? 0,
        StartDate = sub.StartDate,
        EndDate   = sub.EndDate,
        IsActive  = sub.IsActive
    };
}
