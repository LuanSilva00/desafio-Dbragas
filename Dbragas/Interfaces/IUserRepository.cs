using DBragas.Models;

namespace Dbragas.Interfaces
{
    public interface IUserRepository
    {
        void Add(Users users);

        public void Patch(Users users);


        void Delete(Users users);


        Task<Users> GetById(Guid id);

        Task<IEnumerable<Users>> GetAllAsync();
        Task<bool> SaveAllAsync();

    }
}
