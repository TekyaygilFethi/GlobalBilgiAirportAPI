using AirportDistanceCalculator.Data.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.CacheFolder.Service
{
    public interface ICacheService
    {
        T Get<T>(string cacheKey);
        string Get(string cacheKey);

        List<T> GetByPattern<T>(string pattern = "*");

        List<string> GetByPattern(string pattern = "*");

        void Set<T>(string cacheKey, T data);

        void DeleteKeysByPattern(string pattern = "*");

        void Remove(string cacheKey);

        void Clear();
    }
}
