using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaqueroCloud.API.Models.DTOs;
using SaqueroCloud.API.Services.Interfaces;

namespace SaqueroCloud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    /// <summary>Obtiene todas las suscripciones activas. Filtra por planId opcional.</summary>
    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActive([FromQuery] int? planId)
    {
        var subs = await _subscriptionService.GetActiveSubscriptionsAsync(planId);
        return Ok(subs);
    }

    /// <summary>Obtiene suscripciones proximas a caducar.</summary>
    [HttpGet("expiring-soon")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetExpiringSoon([FromQuery] int days = 30)
    {
        if (days < 1 || days > 365)
            return BadRequest(new { message = "El parametro days debe estar entre 1 y 365." });

        var subs = await _subscriptionService.GetExpiringSubscriptionsAsync(days);
        return Ok(subs);
    }

    /// <summary>Resumen de uso por plan.</summary>
    [HttpGet("summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _subscriptionService.GetPlanUsageSummaryAsync();
        return Ok(summary);
    }

    /// <summary>Obtiene suscripciones de un usuario concreto.</summary>
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var subs = await _subscriptionService.GetUserSubscriptionsAsync(userId);
        return Ok(subs);
    }

    /// <summary>Asigna una suscripcion a un usuario. Valida maxUsers del plan.</summary>
    [HttpPost("assign/{userId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(int userId, [FromBody] AssignSubscriptionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _subscriptionService.AssignSubscriptionAsync(userId, dto);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return StatusCode(StatusCodes.Status201Created, result.Data);
    }

    /// <summary>Actualiza plan y/o fecha fin de una suscripcion existente.</summary>
    [HttpPut("{subscriptionId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int subscriptionId, [FromBody] UpdateSubscriptionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _subscriptionService.UpdateSubscriptionAsync(subscriptionId, dto);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(result.Data);
    }

    /// <summary>Cancela una suscripcion activa.</summary>
    [HttpPatch("{subscriptionId:int}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int subscriptionId)
    {
        var cancelled = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        if (!cancelled)
            return NotFound(new { message = $"Suscripcion {subscriptionId} no encontrada." });

        return NoContent();
    }
}
