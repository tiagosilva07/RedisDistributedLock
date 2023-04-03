using StackExchange.Redis;


namespace DistributedLock
{

    internal static class CreateRedisConnection
    {

        public static ConnectionMultiplexer CreateConnection()
        {
            var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" }
            });

            return redis;
        }
    }
}
