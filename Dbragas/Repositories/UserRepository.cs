using Dbragas.Interfaces;
using DBragas.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Dbragas.Repositories
{
    public class UserRepository : IUserRepository 
    {
        private readonly DBragasContext _context;

        public UserRepository(DBragasContext context)
        {
            _context = context;
        }
        public void Add(Users users)
        {
            _context.Users.Add(users);
        }

        public void Delete(Users users)
        {
            _context.Users.Remove(users);
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetById(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(user != null)
            {
                user.Password = null;
                return user;    
            }
            throw new KeyNotFoundException();
        }

        public void Patch(Users users)
        {
            _context.Entry(users).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
