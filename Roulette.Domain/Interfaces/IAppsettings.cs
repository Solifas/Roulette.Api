using System;
namespace Roulette.Application.Interfaces
{
    public interface IAppSettings
    {
        string ConnectionString { get; set; }
        int TimeOfSpinInSeconds { get; set; }
    }
}

