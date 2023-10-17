using System.Threading.Tasks;
using System.Collections.Generic;
using Roulette.Domain;

namespace Roulette.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(string query, T item);
        Task<bool> DeleteAsync(string query, T item);
        Task<IEnumerable<T>> GetAllAsync(string query, object parameters = null);
        Task<T> Get(string query, object parameters = null);
        Task UpdateAsync(string query, T item);
    }
}