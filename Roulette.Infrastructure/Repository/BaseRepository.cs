using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;
using Roulette.Application.Interfaces;
using Roulette.Domain;

namespace Roulette.Infrastructure.Repository
{

    public class BaseRepository<T> : IRepository<T>
    {
        private readonly IAppSettings _appSettings;

        public BaseRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<bool> AddAsync(string query, T entity)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            await connection.ExecuteAsync(query, entity);
            return true;
        }

        public async Task<bool> DeleteAsync(string query, T entity)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);

            await connection.ExecuteAsync(query, entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string query)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            return await connection.QueryAsync<T>(query);
        }
        public async Task<dynamic> GetAll2Async(string query)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            return await connection.QueryAsync(query);
        }

        public async Task<T> Get(string query)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            return await connection.QueryFirstAsync<T>(query);
        }

        public async Task UpdateAsync(string query, T entity)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            await connection.ExecuteAsync(query, entity);
        }
    }
}

