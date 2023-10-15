


using System;
using Roulette.Application.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Helpers{

    public class JsonHelper : IJsonHelper
    {
        public T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // Handle deserialization error, e.g., log or throw an exception.
                Console.WriteLine("Deserialization Error: " + ex.Message);
                return default(T);
            }
        }

        public string Serialize<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                // Handle serialization error, e.g., log or throw an exception.
                Console.WriteLine("Serialization Error: " + ex.Message);
                return null;
            }
        }
    }

}