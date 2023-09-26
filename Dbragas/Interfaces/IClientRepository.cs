using DBragas.Models;

namespace Dbragas.Interfaces
{
    public interface IClientRepository
    {
        void Add(Clients clients);
        void Patch(Clients clients);

        void Delete(Clients clients);

        Task<Clients> GetAll();

        Task<Clients> GetByEmail();

        Task<IEnumerable<Clients>> GetAllAsync();

        Task<bool> SaveAllAsync();


    }
}
