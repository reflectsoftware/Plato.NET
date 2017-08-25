namespace Plato.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            // RedisTest.RedisPlayground.RunAsync().Wait();
            //Mapper.MapperPlayground.RunAsync().Wait();
            RMQ.RMQPlayground.RunAsync().Wait();
        }
    }
}
