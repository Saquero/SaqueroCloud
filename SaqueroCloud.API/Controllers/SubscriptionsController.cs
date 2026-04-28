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

    /// <summary>
    /// Obtiene todas las suscripciones activas (solo Admin)
    /// </summary>
    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserSubscriptionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive([FromQuery] int? planId = null)
    {
        var subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync(planId);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Obtiene suscripciones activas que caducan pronto
    /// </summary>
    [HttpGet("expiring-soon")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserSubscriptionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpiringSoon([FromQuery] int days = 7)
    {
        if (days <= 0)
            return BadRequest(new { message = "El numero de dias debe ser mayor que cero." });

        var subscriptions = await _subscriptionService.GetExpiringSoonAsync(days);
        return Ok(subscriptions);
    }
    /// <summary>
    /// Obtiene suscripciones de un usuario
    /// </summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<UserSubscriptionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(userId);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Asigna una suscripcion a un usuario (solo Admin)
    /// </summary>
    [HttpPost("assign/{userId:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserSubscriptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Assign(int userId, [FromBody] AssignSubscriptionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _subscriptionService.AssignSubscriptionAsync(userId, dto);
        if (result is null)
            return NotFound(new { message = "No se pudo asignar la suscripcion. Revisa que el usuario exista y que el plan este activo." });

        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Cancela una suscripcion (solo Admin)
    /// </summary>
    [HttpPatch("{subscriptionId:int}/cancel")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int subscriptionId)
    {
        var cancelled = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
        if (!cancelled)
            return NotFound(new { message = $"No se encontro la suscripcion con id {subscriptionId}" });

        return NoContent();
    }
}


