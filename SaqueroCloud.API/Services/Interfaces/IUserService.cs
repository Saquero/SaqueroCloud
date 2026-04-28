using SaqueroCloud.API.Models.DTOs;

namespace SaqueroCloud.API.Services.Interfaces;

public interface IUserService
{
    Task<PagedResultDto<UserDto>> GetAllUsersAsync(int page, int pageSize);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<bool> DeleteUserAsync(int id);
}

