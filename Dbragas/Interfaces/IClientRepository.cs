using DBragas.Models;

namespace Dbragas.Interfaces
{
    public interface IClientRepository
    {
        void Add(Clients clients);
        void Patch(Clients clients);

        void Delete(Clients clients);

        Task<Clients> GetById(Guid id);

        Task<Clients> GetByEmail(string email);

        Task<IEnumerable<Clients>> GetAllAsync();

        Task<bool> SaveAllAsync();


    }
}
