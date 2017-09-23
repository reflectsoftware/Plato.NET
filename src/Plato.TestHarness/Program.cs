namespace Plato.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Cache.CachePlaygorund.RunAsync().Wait();
            //RedisTest.RedisPlayground.RunAsync().Wait(); 
            // Mapper.MapperPlayground.RunAsync().Wait();
            // RMQ.RMQPlayground.RunAsync().Wait();
        }
    }
}
