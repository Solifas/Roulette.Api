using System.Threading.Tasks;
using System.Collections.Generic;
using Roulette.Domain;

namespace Roulette.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<bool> AddAsync(string query, T item);
        Task<bool> DeleteAsync(string query, T item);
        Task<IEnumerable<T>> GetAllAsync(string query);
        Task<T> Get(string query);
        Task UpdateAsync(string query, T item);
    }
}