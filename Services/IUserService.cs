using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.Services;

public interface IUserService
{
    IReadOnlyCollection<User> GetAll();
    User? GetById(int id);
    User Create(CreateUserRequest request);
    bool Update(int id, UpdateUserRequest request, out User? updatedUser);
    bool Delete(int id);
    bool EmailExists(string email, int? exceptUserId = null);
}
