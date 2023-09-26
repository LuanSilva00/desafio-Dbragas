using DBragas.Models;

namespace Dbragas.Interfaces
{
    public interface IUserRepository
    {
        void Add(Users users);

        void Patch(Users users);


        void Delete(Users users);


        Task<Users> GetById(Guid id);

        Task<Users> GetByUsername(string username);

        Task<Users> GetByEmail(string email);

        Task<IEnumerable<Users>> GetAllAsync();
        Task<bool> SaveAllAsync();

    }
}
