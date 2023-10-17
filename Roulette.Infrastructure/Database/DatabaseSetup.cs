using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Roulette.Application.Interfaces;

namespace Roulette.Infrastructure.Database
{

    public class DatabaseSetup : IDatabaseSetup
    {
        private readonly IAppSettings _appsettings;

        public DatabaseSetup(IAppSettings appsettings)
        {
            _appsettings = appsettings;
        }

        public void Setup()
        {
            using var connection = new SqliteConnection(_appsettings.ConnectionString);
            var table = connection.QueryFirstOrDefault<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='Users';");
            if (table != null && table.Equals("Users", StringComparison.Ordinal))
            {
                //assumption if the Users able exists, all tables exist
                return;
            }

            connection.Open();
            connection.Execute(@"
                    CREATE TABLE Users (
                        Id NVARCHAR(36) PRIMARY KEY,                        
                        Username NVARCHAR(255) NOT NULL,
                        Balance INT NOT NULL
                    );

                    CREATE TABLE Bets (
                        Id NVARCHAR(36) PRIMARY KEY,
                        UserId INT NOT NULL,
                        Amount INT NOT NULL,
                        BetType INT NOT NULL,
                        IsBetConcluded BIT NOT NULL,
                        IsBetWon BIT NOT NULL,
                        FOREIGN KEY (UserID) REFERENCES Users (ID)
                    );

                    CREATE TABLE SpinHistory (
                        Id NVARCHAR(36) PRIMARY KEY,
                        BetType INT NOT NULL,
                        SpinDate DATETIME  NULL
                    );
                    
                    CREATE TABLE PayOuts(
                        Id NVARCHAR(36) PRIMARY KEY,
                        UserId INT NOT NULL,
                        Amount INT NOT NULL,
                        IsRetreived BIT NOT NULL
                    );

                    CREATE TABLE BetHistory (
                        Id NVARCHAR(36) PRIMARY KEY,
                        UserId INT NOT NULL,
                        BetId INT NOT NULL,
                        FOREIGN KEY (UserId) REFERENCES Users(Id),
                        FOREIGN KEY (BetId) REFERENCES Bets(Id)
                    );
                    "
                    );
        }
    }
}

