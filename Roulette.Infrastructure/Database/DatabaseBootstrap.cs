using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Roulette.Application.Interfaces;

namespace Roulette.Infrastructure.Database
{

    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly IAppsettings _appsettings;

        public DatabaseBootstrap(IAppsettings appsettings)
        {
            _appsettings = appsettings;
        }

        public void Setup()
        {
            using var connection = new SqliteConnection("Data Source=Roulette1.sqlite");
            var table = connection.QueryFirstOrDefault<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='Roulette1';");
            if (table != null && table.Equals("Roulette", StringComparison.Ordinal))
            {
                return;
            }

            connection.Open();
            connection.Execute(@"
                    CREATE TABLE SpinHistory (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        BetType BIT NOT NULL,
                        SpinDate DATETIME NOT NULL
                    );
                    CREATE TABLE Bet (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL,
                        Amount INT NOT NULL,
                        BetType BIT NOT NULL,
                        FOREIGN KEY (UserId) REFERENCES UserAccount(UserId)
                    );
                    CREATE TABLE UserAccount (
                        UserId INT IDENTITY(1,1) PRIMARY KEY,
                        Balance INT NOT NULL,
                        BetType INT NOT NULL
                    );
                    CREATE TABLE BetHistory (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL,
                        BetId INT NOT NULL,
                        FOREIGN KEY (UserId) REFERENCES UserAccount(UserId),
                        FOREIGN KEY (BetId) REFERENCES Bet(Id)
                    );
                    CREATE TABLE PayOut(
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL,
                        Amount INT NOT NULL,
                        IsRetreived BIT NOT NULL
                    );
                    "
                    );
        }
    }
}

