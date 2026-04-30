using System.ComponentModel.DataAnnotations;

namespace SaqueroCloud.API.Models.DTOs;

public class SubscriptionPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MaxUsers { get; set; }
    public bool IsActive { get; set; }
}

public class CreatePlanDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int MaxUsers { get; set; }
}

public class UpdatePlanDto
{
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? MaxUsers { get; set; }
    public bool? IsActive { get; set; }
}

public class AssignSubscriptionDto
{
    [Required]
    public int PlanId { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}

public class UpdateSubscriptionDto
{
    [Required]
    public int PlanId { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}

public class UserSubscriptionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public decimal PlanPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class SubscriptionPlanUsageDto
{
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public int MaxUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int AvailableSlots { get; set; }
    public bool IsFull { get; set; }
}

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static ServiceResult<T> Fail(string message) =>
        new() { Success = false, Message = message };
}
