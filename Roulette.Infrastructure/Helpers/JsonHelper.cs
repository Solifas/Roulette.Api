


using System;
using Roulette.Application.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Helpers
{

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
                return null;
            }
        }
    }

}