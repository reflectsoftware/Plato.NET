namespace Plato.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cache.CachePlaygorund.RunAsync().Wait();
            RedisTest.RedisPlayground.RunAsync().Wait(); 
            // Mapper.MapperPlayground.RunAsync().Wait(); 
            // Messenging.RMQPlayground.RunAsync().Wait();
            // Messenging.AMQPlayground.RunAsync().Wait();
        }
    }
}
