using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaqueroCloud.API.Models.DTOs;
using SaqueroCloud.API.Services.Interfaces;

namespace SaqueroCloud.API.Controllers;

[ApiController]
[Route("api/subscription-plans")]
[Produces("application/json")]
public class SubscriptionPlansController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionPlansController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    /// <summary>
    /// Obtiene todos los planes disponibles (publico)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionPlanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _subscriptionService.GetAllPlansAsync();
        return Ok(plans);
    }

    /// <summary>
    /// Obtiene un plan por ID (publico)
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var plan = await _subscriptionService.GetPlanByIdAsync(id);
        if (plan is null)
            return NotFound(new { message = $"No se encontro el plan con id {id}" });

        return Ok(plan);
    }

    /// <summary>
    /// Crea un nuevo plan (solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePlanDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _subscriptionService.CreatePlanAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Actualiza un plan existente (solo Admin)
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlanDto dto)
    {
        var updated = await _subscriptionService.UpdatePlanAsync(id, dto);
        if (updated is null)
            return NotFound(new { message = $"No se encontro el plan con id {id}" });

        return Ok(updated);
    }

    /// <summary>
    /// Elimina un plan (solo Admin)
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _subscriptionService.DeletePlanAsync(id);
        if (!deleted)
            return NotFound(new { message = $"No se encontro el plan con id {id}" });

        return NoContent();
    }
}

