using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using System;

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

        public async Task<IEnumerable<T>> GetAllAsync(string query, object parameters = null)
        {
            try
            {
                using var connection = new SqliteConnection(_appSettings.ConnectionString);

                return await connection.QueryAsync<T>(query, parameters);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Get(string query, object parameters = null)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            return await connection.QueryFirstAsync<T>(query, parameters);
        }

        public async Task UpdateAsync(string query, T entity)
        {
            using var connection = new SqliteConnection(_appSettings.ConnectionString);
            await connection.ExecuteAsync(query, entity);
        }
    }
}

