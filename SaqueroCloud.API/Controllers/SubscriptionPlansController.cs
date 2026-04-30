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

    /// <summary>Lista todos los planes (publico).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _subscriptionService.GetAllPlansAsync());

    /// <summary>Obtiene un plan por ID (publico).</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var plan = await _subscriptionService.GetPlanByIdAsync(id);
        if (plan is null) return NotFound(new { message = $"Plan {id} no encontrado." });
        return Ok(plan);
    }

    /// <summary>Crea un nuevo plan (Admin).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePlanDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _subscriptionService.CreatePlanAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza un plan existente (Admin).</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlanDto dto)
    {
        var updated = await _subscriptionService.UpdatePlanAsync(id, dto);
        if (updated is null) return NotFound(new { message = $"Plan {id} no encontrado." });
        return Ok(updated);
    }

    /// <summary>Elimina un plan (Admin).</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _subscriptionService.DeletePlanAsync(id);
            if (!deleted) return NotFound(new { message = $"Plan {id} no encontrado." });
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest(new { message = "No se puede eliminar el plan porque tiene suscripciones asociadas." });
        }
    }
}
