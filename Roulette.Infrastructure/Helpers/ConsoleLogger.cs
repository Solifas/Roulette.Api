using System;
using Roulette.Domain.Interfaces;

namespace Roulette.Infrastructure.Helpers
{
    public class ConsoleLogger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[WARNING] {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public void LogError(Exception exception, string message)
        {
            Console.WriteLine($"[ERROR] {message}");
            Console.WriteLine($"[EXCEPTION] {exception.Message}");
        }
    }
}

