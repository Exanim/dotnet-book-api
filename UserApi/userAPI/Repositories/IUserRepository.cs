using userAPI.Entities;

namespace userAPI.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void DeleteUser(User user);
        Task<User?> GetUserAsync(int id);
        Task<User?> GetUserAsync(string name);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> DoesUserExists(int id);
        Task<bool> DoesUserExists(string name);
        Task<bool> SaveChangesAsync();

    }
}
