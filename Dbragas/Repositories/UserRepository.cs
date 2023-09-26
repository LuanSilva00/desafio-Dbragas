using Dbragas.Interfaces;
using DBragas.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
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
            users.DeletedAt = DateTime.Now;
            users.IsActive = false;
            _context.Users.Update(users);
        }


        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _context.Users.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<Users> GetById(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<Users> GetByUsername(string username)
        {
            var user = await _context.Users.Where(x => x.Username == username && x.IsActive).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Users> GetByEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email && x.IsActive).FirstOrDefaultAsync();
            return user;
        }

        public void Patch(Users users)
        {
            if (!string.IsNullOrWhiteSpace(users.Password))
            {
                throw new InvalidOperationException("Senha não pode ser atualizada durante a operação de patch.");
            }

            users.IsActive = true;
            _context.Entry(users).State = EntityState.Modified;
        }



        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
