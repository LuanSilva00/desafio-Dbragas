using Dbragas.Interfaces;
using DBragas.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace Dbragas.Repositories
{
    public class TypeClientRepository : ITypeClientRepository
    {
        private readonly DBragasContext _context;

        public TypeClientRepository(DBragasContext context)
        {
            _context = context;
        }
        public void Add(TypeClients TypeClients)
        {
            _context.TypeClients.Add(TypeClients);
        }

        public void Delete(TypeClients TypeClients)
        {
            TypeClients.DeletedAt = DateTime.Now;
            TypeClients.IsActive = false;
            _context.TypeClients.Update(TypeClients);
        }


        public async Task<IEnumerable<TypeClients>> GetAllAsync()
        {
            return await _context.TypeClients.ToListAsync();
        }


        public async Task<TypeClients> GetById(Guid id)
        {
            var TypeClient = await _context.TypeClients.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (TypeClient != null)
            {
                return TypeClient;
            }
            return null;
        }

        public async Task<TypeClients> GetByName(string name)
        {
            var TypeClient = await _context.TypeClients
                .Where(x => x.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            return TypeClient;
        }


        public void Patch(TypeClients TypeClients)
        {
            _context.Entry(TypeClients).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
