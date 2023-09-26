using Dbragas.Interfaces;
using DBragas.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace Dbragas.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DBragasContext _context;

        public ClientRepository(DBragasContext context)
        {
            _context = context;
        }
        public void Add(Clients clients)
        {
            _context.Clients.Add(clients);
        }

        public void Delete(Clients clients)
        {
            clients.DeletedAt = DateTime.Now;
            clients.IsActive = false;
            _context.Clients.Update(clients);
        }


        public async Task<IEnumerable<Clients>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Clients> GetByEmail(string email)
        {
            var client = await _context.Clients.Where(x => x.Email == email).FirstOrDefaultAsync();
            return client;
        }

        public async Task<Clients> GetById(Guid id)
        {
            var client = await _context.Clients.Where(x => x.Id == id).FirstOrDefaultAsync();
            return null;
        }

        public void Patch(Clients clients)
        {
            _context.Entry(clients).State = EntityState.Modified; 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
