using Newtonsoft.Json;
using StackExchange.Redis;

namespace DistributedLock
{
    internal class Program
    {

        static void Main(string[] args)
        {
            IDatabase database = CreateRedisConnection
                .CreateConnection()
                .GetDatabase();

            var redisClient = new RedisClient(database);
            var productsDatabase = new ProductsDatabase();
            var threads = new List<Task>();
            var json = ProductsData.products;

            List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(json);

            foreach (var product in products)
            {
                var t = new Task(() =>
                {
                    bool isLocked = false;

                    try
                    {
                        while (!isLocked)
                        {
                            var result = redisClient.SetLock(product.Id);
                            if (result)
                            {
                                isLocked = true;
                                Console.WriteLine($"Lock acquired for product : {product.Id} ");
                                productsDatabase.SaveProduct(product);
                            }
                            else
                            {
                                Console.WriteLine($"Lock failed for product : {product.Id}. Retrying...");

                            }
                            Thread.Sleep(1000);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lock Exception : {ex}");
                    }
                    
                    redisClient.ReleaseLock(product.Id);
                });
                threads.Add(t);
                t.Start();
            }
            Task.WaitAll(threads.ToArray());

            Console.WriteLine($"Lock acquired for: {productsDatabase.Products.Count} products");
        }
    }
}