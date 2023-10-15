using System.Threading.Tasks;
using System.Collections.Generic;

namespace Roulette.Application.Interfaces
{
        public interface IRepository<T>
    {
        Task<bool> AddAsync(T item, string query);
        Task<bool> DeleteAsync(string query, T item);
        Task<IEnumerable<T>> GetAllAsync(string query);
         Task<T> GetByIdAsync(string query);
         Task UpdateAsync(string query);
    }
}