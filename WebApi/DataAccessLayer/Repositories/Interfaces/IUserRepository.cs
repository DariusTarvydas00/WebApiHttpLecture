using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByUserName(string username);
        Task Delete(int id);
        Task Create(User user);
        Task Update(User user);
    }
}