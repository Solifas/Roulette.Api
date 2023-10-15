using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;
using Roulette.Application.Interfaces;

namespace Roulette.Infrastructure.Repository
{

    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public async Task<bool> AddAsync(T item, string query)
        {
            using var connection = new SqliteConnection("_appsettings.ConnectionString");
            await connection.ExecuteAsync(query, item);
            return true;
        }

        public async Task<bool> DeleteAsync(string query, T item)
        {
            using var connection = new SqliteConnection("_appsettings.ConnectionString");

            await connection.ExecuteAsync(query,item);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string query)
        {
            using var connection = new SqliteConnection("_appsettings.ConnectionString");
            return await connection.QueryAsync<T>(query);
        }

        public async Task<T> GetByIdAsync(string query)
        {
            using var connection = new SqliteConnection("_appsettings.ConnectionString");
            return await connection.QueryFirstAsync<T>(query);
        }

        public async Task UpdateAsync(string query)
        {
            using var connection = new SqliteConnection("_appsettings.ConnectionString");
            await connection.ExecuteAsync(query);
        }
    }
}

