using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.CacheFolder.Server
{
    public class RedisServer
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private IServer _server;
        private int _currentDatabaseId = 11;
        private string connString = string.Empty;

        public RedisServer(IConfiguration configuration)
        {
            connString = configuration.GetSection("RedisConfiguration:Url")?.Value;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connString);
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseId);
            _server = _connectionMultiplexer.GetServer(connString);
        }


        public IDatabase Database => _database;

        public IEnumerable<RedisKey> GetKeysByPattern(string pattern)
        {
            return _server.Keys(database: _currentDatabaseId, pattern: pattern);
        }

        public void FlushDatabase()
        {
            _connectionMultiplexer.GetServer(connString).FlushAllDatabases();
        }
    }
}
