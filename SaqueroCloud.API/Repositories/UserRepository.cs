using Microsoft.EntityFrameworkCore;
using SaqueroCloud.API.Data;
using SaqueroCloud.API.Models.Entities;
using SaqueroCloud.API.Repositories.Interfaces;

namespace SaqueroCloud.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(int id, User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return null;

        if (!string.IsNullOrWhiteSpace(updatedUser.Name))
            user.Name = updatedUser.Name;

        if (!string.IsNullOrWhiteSpace(updatedUser.Email))
            user.Email = updatedUser.Email;

        user.IsActive = updatedUser.IsActive;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
