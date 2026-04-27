using Microsoft.EntityFrameworkCore;
using SaqueroCloud.API.Data;
using SaqueroCloud.API.Models.Entities;
using SaqueroCloud.API.Repositories.Interfaces;

namespace SaqueroCloud.API.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly AppDbContext _context;

    public UserSubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserSubscription>> GetAllActiveAsync()
    {
        return await _context.UserSubscriptions
            .Include(us => us.User)
            .Include(us => us.Plan)
            .Where(us => us.IsActive && us.EndDate > DateTime.UtcNow)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<UserSubscription>> GetByUserIdAsync(int userId)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Plan)
            .Where(us => us.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserSubscription?> GetByIdAsync(int id)
    {
        return await _context.UserSubscriptions
            .Include(us => us.User)
            .Include(us => us.Plan)
            .FirstOrDefaultAsync(us => us.Id == id);
    }

    public async Task<UserSubscription> CreateAsync(UserSubscription subscription)
    {
        _context.UserSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var subscription = await _context.UserSubscriptions.FindAsync(id);
        if (subscription is null) return false;

        subscription.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
