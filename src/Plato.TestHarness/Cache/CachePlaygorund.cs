using Nito.AsyncEx;
using Plato.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plato.TestHarness.Cache
{
    public class MyPoolObject
    {
        public string Name { get; set; }
    }
    

    public class MyPoolAsync : GenericObjectPoolAsync<MyPoolObject, object>
    {
        private static int _count = 0;

        public MyPoolAsync(            
            int initialPoolSize,
            int maxGrowSize) : base(initialPoolSize, maxGrowSize, null)
        {
        }

        protected override void Initialize(object settings)
        {                        
        }

        protected override Task<MyPoolObject> CreatePoolObjectAsync()
        {
            return Task.FromResult(new MyPoolObject { Name = $"Ross:{_count++}" });
        }
    }        

    public static class CachePlaygorund
    {
        static private Task TestPoolAsync()
        {
            var myPool = new MyPoolAsync(3, 5);
            var rnd = new Random((int)DateTime.Now.Ticks);
            var tasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                var task = Task.Run(async () =>
                {
                    for (var j = 0; j < 10; j++)
                    {
                        using (var xxx = await myPool.ContainerAsync())
                        {
                            Console.WriteLine(xxx.Instance.Name);
                            await Task.Delay(rnd.Next(1000));
                        }
                    }
                });

                tasks.Add(task);
            }

            Console.WriteLine("Waiting...");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Done");

            return Task.CompletedTask;
        }

        static private Task TestNonAsyncCacheAsync()
        {
            using (var localCache = new LocalMemoryCache())
            {
                var cacheKey = "mykey";
                var value = localCache.Get(cacheKey, (name, args) =>
                {
                    return new CacheDataInfo<string>
                    {
                        KeepAlive= TimeSpan.FromHours(4),
                        NewCacheData = "Ross"
                    };
                });

                Console.WriteLine(value);
            }
            
            return Task.CompletedTask;
        }
        
        static public async Task RunAsync()
        {
            // await TestPoolAsync();
            await TestNonAsyncCacheAsync();
        }
    }
}
