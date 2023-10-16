using AirportDistanceCalculator.Business.CacheFolder.Server;
using AirportDistanceCalculator.Business.Helpers;
using StackExchange.Redis;
using System.Text.Json;

namespace AirportDistanceCalculator.Business.CacheFolder.Service
{
    public class RedisCacheService : ICacheService
    {
        private RedisServer _redisServer;

        public RedisCacheService(RedisServer redisServer)
        {
            _redisServer = redisServer;
        }

        public void Clear()
        {
            _redisServer.FlushDatabase();
        }

        public void DeleteKeysByPattern(string pattern = "*")
        {
            var keys = GetKeysByPattern(pattern);
            foreach(var key in keys)
            {
                Remove(key);
            }
        }

        public T Get<T>(string cacheKey)
        {
            var data = Get(cacheKey);
            if (data.CheckIsNullOrEmpty()) return default;

            return JsonSerializer.Deserialize<T>(data);
        }

        public string Get(string cacheKey)
        {
            return _redisServer.Database.StringGet(cacheKey);
        }

        public List<T> GetByPattern<T>(string pattern = "*")
        {
            var dataList = new List<T>();
            var keys = GetKeysByPattern(pattern);

            foreach(var key in keys)
            {
                dataList.Add(Get<T>(key));
            }

            return dataList;
        }

        public List<string> GetByPattern(string pattern = "*")
        {
            var dataList = new List<string>();
            var keys = GetKeysByPattern(pattern);

            foreach (var key in keys)
            {
                dataList.Add(Get(key));
            }

            return dataList;
        }



        public void Remove(string cacheKey)
        {
            _redisServer.Database.KeyDelete(cacheKey);
        }

        public void Set<T>(string cacheKey, T data)
        {
            var serializedData = JsonSerializer.Serialize(data);
            _redisServer.Database.StringSet(cacheKey, serializedData);
        }

        private IEnumerable<RedisKey> GetKeysByPattern(string pattern = "*")
        {
            return _redisServer.GetKeysByPattern(pattern);
        }
    }
}
