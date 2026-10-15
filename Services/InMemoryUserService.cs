using System.Collections.Concurrent;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.Services;

public sealed class InMemoryUserService : IUserService
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private int _nextId = 0;

    public IReadOnlyCollection<User> GetAll() =>
        _users.Values.OrderBy(user => user.Id).ToArray();

    public User? GetById(int id) =>
        _users.TryGetValue(id, out var user) ? user : null;

    public User Create(CreateUserRequest request)
    {
        var id = Interlocked.Increment(ref _nextId);
        var user = new User
        {
            Id = id,
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Age = request.Age
        };

        _users[id] = user;
        return user;
    }

    public bool Update(int id, UpdateUserRequest request, out User? updatedUser)
    {
        updatedUser = null;

        if (!_users.ContainsKey(id))
            return false;

        var replacement = new User
        {
            Id = id,
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Age = request.Age
        };

        _users[id] = replacement;
        updatedUser = replacement;
        return true;
    }

    public bool Delete(int id) => _users.TryRemove(id, out _);

    public bool EmailExists(string email, int? exceptUserId = null) =>
        _users.Values.Any(user =>
            user.Id != exceptUserId &&
            string.Equals(user.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
}
