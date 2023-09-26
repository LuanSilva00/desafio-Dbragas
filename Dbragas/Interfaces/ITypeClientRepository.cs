using DBragas.Models;

namespace Dbragas.Interfaces
{
    public interface ITypeClientRepository
    {
        void Add(TypeClients TypeClients);
        void Patch(TypeClients TypeClients);

        void Delete(TypeClients TypeClients);

        Task<TypeClients> GetById(Guid id);

        Task<TypeClients> GetByName(string name);

        Task<IEnumerable<TypeClients>> GetAllAsync();

        Task<bool> SaveAllAsync();


    }
}
