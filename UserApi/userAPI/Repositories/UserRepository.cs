using Microsoft.EntityFrameworkCore;
using userAPI.Entities;
using System.Xml.Linq;
using userAPI.DbContexts;

namespace userAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAPIContext _context;
        public UserRepository(UserAPIContext context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void DeleteUser(User user) { 
        
            _context.Users.Remove(user);
        }

        public Task<bool> DoesUserExists(int id)
        {
            return _context.Users.AnyAsync(u => u.Id == id);
        }

        public Task<bool> DoesUserExists(string name)
        {
            return _context.Users.AnyAsync(u => u.Name == name);
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _context.Users.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserAsync(string name)
        {
            return await _context.Users.Where(e=>e.Name==name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.OrderBy(e=>e.Id).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
