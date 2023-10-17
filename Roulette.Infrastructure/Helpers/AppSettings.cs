using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roulette.Application.Interfaces;

namespace Roulette.Infrastructure.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string ConnectionString { get; set; }
    }
}