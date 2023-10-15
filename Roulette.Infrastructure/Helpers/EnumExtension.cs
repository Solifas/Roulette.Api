using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Helpers
{
    public static class EnumExtensions
    {
        private static readonly Random random = new Random();

        public static T GetRandomEnumValue<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"{typeof(T).FullName} is not an enum type.");
            }

            Array values = Enum.GetValues(enumType);
            int randomIndex = random.Next(0, values.Length);

            return (T)values.GetValue(randomIndex);
        }
    }
}