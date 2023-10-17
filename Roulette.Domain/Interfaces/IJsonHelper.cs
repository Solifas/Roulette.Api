namespace Roulette.Application.Interfaces
{
    public interface IJsonHelper
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
        
    }
}