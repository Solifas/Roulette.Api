using Roulette.Application.Interfaces;

namespace Roulette.Infrastructure.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string ConnectionString { get; set; }
        public int TimeOfSpinInSeconds { get; set; }
    }
}