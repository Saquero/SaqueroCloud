using SaqueroCloud.API.Models.DTOs;
using SaqueroCloud.API.Models.Entities;
using SaqueroCloud.API.Repositories.Interfaces;
using SaqueroCloud.API.Services.Interfaces;

namespace SaqueroCloud.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var userUpdate = new User
        {
            Name = dto.Name ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            IsActive = dto.IsActive ?? true
        };

        var updated = await _userRepository.UpdateAsync(id, userUpdate);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
