using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLock
{
    internal class RedisClient
    {
        private readonly IDatabase _database;
        int lockExpireSeconds = 10;
        string _key = "testlock";

        public RedisClient(IDatabase database)
        {
            _database = database;
        }

        public bool SetLock(int id) 
        {
            return _database.StringSet(_key, id, TimeSpan.FromSeconds(lockExpireSeconds), When.NotExists);
        }

       public bool ReleaseLock(int id)
       {
            return _database.KeyDelete(_key);
       }

    }
}
