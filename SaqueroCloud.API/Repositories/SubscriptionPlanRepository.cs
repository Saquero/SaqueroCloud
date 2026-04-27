using Microsoft.EntityFrameworkCore;
using SaqueroCloud.API.Data;
using SaqueroCloud.API.Models.Entities;
using SaqueroCloud.API.Repositories.Interfaces;

namespace SaqueroCloud.API.Repositories;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly AppDbContext _context;

    public SubscriptionPlanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetAllAsync()
    {
        return await _context.SubscriptionPlans.AsNoTracking().ToListAsync();
    }

    public async Task<SubscriptionPlan?> GetByIdAsync(int id)
    {
        return await _context.SubscriptionPlans.FindAsync(id);
    }

    public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan)
    {
        _context.SubscriptionPlans.Add(plan);
        await _context.SaveChangesAsync();
        return plan;
    }

    public async Task<SubscriptionPlan?> UpdateAsync(int id, SubscriptionPlan updatedPlan)
    {
        var plan = await _context.SubscriptionPlans.FindAsync(id);
        if (plan is null) return null;

        if (!string.IsNullOrWhiteSpace(updatedPlan.Description))
            plan.Description = updatedPlan.Description;

        if (updatedPlan.Price > 0)
            plan.Price = updatedPlan.Price;

        if (updatedPlan.MaxUsers > 0)
            plan.MaxUsers = updatedPlan.MaxUsers;

        plan.IsActive = updatedPlan.IsActive;

        await _context.SaveChangesAsync();
        return plan;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var plan = await _context.SubscriptionPlans.FindAsync(id);
        if (plan is null) return false;

        _context.SubscriptionPlans.Remove(plan);
        await _context.SaveChangesAsync();
        return true;
    }
}
