﻿using MsgPack;
using MsgPack.Serialization;
using Newtonsoft.Json.Linq;
using Plato.Cache;
using Plato.Redis;
using Plato.Redis.Collections;
using Plato.Redis.Containers;
using Plato.Redis.Serializers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plato.TestHarness.RedisTest
{
    public class RedisPlayground
    {
        public class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class MySerializeData
        {
            public string Data1 { get; set; }
            public string Data2 { get; set; }
            public List<string> List { get; set; }
        }

        public class MySerializeData2
        {
            public string Data { get; set; }
            public List<TestClass> List { get; set; }
        }

        static void Test1(RedisConnection redisConnection)
        {
            try
            {
                var db = redisConnection.GetDatabase();
                db.PingAsync().Wait();

                var list = new RedisList<string>(db, "test");
                list.Clear();

                list.Add("one");
                list.Add("two");
                list.Add("three");

                list[1] = "Ross";

                var bData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                db.StringSet("some_data", bData); //, TimeSpan.FromSeconds(20));

                foreach (var str in list)
                {
                    Console.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void Test2(RedisConnection redisConnection)
        {
            while (true)
            {
                try
                {
                    var db = redisConnection.GetDatabase();
                    db.PingAsync().Wait();

                    int count = 0;
                    while (true)
                    {
                        try
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                var key = $"key{i + 1}";
                                var value = db.StringGetAsync(key).Result;
                                Console.WriteLine($"{key} = {value}");
                            }

                            Console.WriteLine($"count: {count++} -------------------------------------");
                            System.Threading.Thread.Sleep(500);
                        }
                        catch (RedisServerException ex)
                        {
                            Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                            System.Threading.Thread.Sleep(1000);
                        }
                        catch (RedisConnectionException ex)
                        {
                            Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                            System.Threading.Thread.Sleep(1000);
                        }
                        catch (RedisException ex)
                        {
                            Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                }
                catch (RedisServerException ex)
                {
                    Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                    System.Threading.Thread.Sleep(1000);
                }
                catch (RedisConnectionException ex)
                {
                    Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                    System.Threading.Thread.Sleep(1000);
                }
                catch (RedisException ex)
                {
                    Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        static void Test3(RedisConnection redisConnection)
        {
            while (true)
            {
                try
                {
                    var db = redisConnection.GetDatabase();
                    var t2 = db.PingAsync().Result;

                    int count = 0;
                    while (true)
                    {
                        try
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                var key = $"key{i + 1}";
                                var value = (string)db.StringGetAsync(key).Result;
                                Console.WriteLine($"{key} = {value}");
                            }

                            Console.WriteLine($"count: {count++} -------------------------------------");
                            System.Threading.Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                            System.Threading.Thread.Sleep(2000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"XXX - {ex.GetType().Name} - {ex.Message}");
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        static void Test4(RedisConnection redisConnection)
        {
            var mpSerializer = new MsgPackRedisSerializer();

            try
            {
                var db = redisConnection.GetDatabase();
                db.PingAsync().Wait();

                var list = new RedisList<string>(db, "test", mpSerializer);
                list.Clear();

                list.Add("one");
                list.Add("two");
                list.Add("three");

                list[1] = "Ross";


                foreach (var str in list)
                {
                    Console.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void SerializerTests(RedisConnection redisConnection)
        {
            try
            {
                var json = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}sample.json");

                var myData = new MySerializeData
                {
                    Data1 = json,
                    Data2 = json,
                    List = new List<string>
                    {
                        json,
                        json,
                        json,
                        json
                    }
                };

                var myData2 = new MySerializeData2
                {
                    Data = json,
                    List = new List<TestClass>(),
                };

                for (var i = 0; i < 10; i++)
                {
                    myData2.List.Add(new TestClass
                    {
                        Id = i,
                        Name = $"Ross: {i}"
                    });
                }

                var jsonSerializer = new JsonRedisSerializer();
                var mpSerializer = new MsgPackRedisSerializer();

                //var r1 = jsonSerializer.Serialize(myData2);
                //var r2 = mpSerializer.Serialize(myData2);                

                //var db = redisConnection.GetDatabase();
                //var list = new XRedisList<MySerializeData>(db, "test", mpSerializer);
                //list.Clear();

                //list.Add(myData);
                //list.Add(myData);
                //list.Add(myData);

                const int Iteration = 1000;

                while (true)
                {
                    Console.WriteLine("Ready...");

                    var key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.KeyChar == 'q')
                    {
                        break;
                    }
                    else if (key.KeyChar == 'c')
                    {
                        Console.Clear();

                    }
                    else if (key.KeyChar == 'j')
                    {
                        var db = redisConnection.GetDatabase();
                        var list = new RedisList<MySerializeData>(db, "test", jsonSerializer);
                        list.Clear();

                        var sw = new Stopwatch();
                        sw.Start();
                        for (var i = 0; i < Iteration; i++)
                        {
                            list.Add(myData);
                            var d = list[0];
                        }
                        sw.Stop();
                        Console.WriteLine($"{sw.ElapsedMilliseconds} msec for {Iteration} iterations using Json serializer");
                    }
                    else if (key.KeyChar == 'm')
                    {
                        var db = redisConnection.GetDatabase();
                        var list = new RedisList<MySerializeData>(db, "test", mpSerializer);
                        list.Clear();

                        var sw = new Stopwatch();
                        sw.Start();
                        for (var i = 0; i < Iteration; i++)
                        {
                            list.Add(myData);
                            var d = list[0];
                        }
                        sw.Stop();
                        Console.WriteLine($"{sw.ElapsedMilliseconds} msec for {Iteration} iterations using MsgPack serializer");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task TestLockAsyncThread(RedisConnection redisConnection, string name = "test")
        {
            try
            {
                var rnd = new Random((int)DateTime.Now.Ticks);
                var db = redisConnection.GetDatabase();
                var lockAcquisition = new RedisCacheKeyLockAcquisition();

                for (var i = 0; i < 5; i++)
                {
                    using (var dbLock = await lockAcquisition.AcquireLockAsync(db, "testkey"))
                    {
                        if (dbLock.Locked)
                        {
                            var lockFor = rnd.Next(1, 2);
                            Console.WriteLine($"{name}: Lock obtained for: {lockFor} seconds");
                            Thread.Sleep(TimeSpan.FromSeconds(lockFor));
                            // Thread.Sleep(TimeSpan.FromSeconds(20));
                        }
                        else
                        {
                            Console.WriteLine($"{name} Couldn't obtained lock");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void LockThread(object _data)
        {
            var data = (Tuple<RedisConnection, string>)_data;
            TestLockAsyncThread(data.Item1, data.Item2).Wait();
        }

        static void LockThreadTest(RedisConnection redisConnection)
        {
            var data1 = new Tuple<RedisConnection, string>(redisConnection, "Ross");
            var data2 = new Tuple<RedisConnection, string>(redisConnection, "Tammy");

            var t1 = new Thread(LockThread);
            var t2 = new Thread(LockThread);

            t1.Start(data1);
            t2.Start(data2);

            t1.Join();
            t2.Join();
        }

        static bool TerminateTestCacheAsync;
        static async Task TestCacheAsync(RedisConnection redisConnection, string threadName)
        {
            try
            {
                var rnd = new Random((int)DateTime.Now.Ticks);
                var json = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}sample.json");
                var lockAcquisition = new RedisCacheKeyLockAcquisition();
                var cache = new RedisCache(redisConnection, lockAcquisition);

                while (!TerminateTestCacheAsync)
                {
                    var data = await cache.GetAsync("data1", async (name, args) =>
                    {
                        Console.WriteLine($"{threadName} locked and creating data **************");

                        var myData = new MySerializeData2
                        {
                            Data = json,
                            List = new List<TestClass>(),
                        };

                        for (var i = 0; i < 10; i++)
                        {
                            myData.List.Add(new TestClass
                            {
                                Id = i,
                                Name = $"Ross: {i}"
                            });
                        }

                        return await Task.FromResult(new CacheDataInfo<MySerializeData2>
                        {
                            NewCacheData = myData,
                            KeepAlive = TimeSpan.FromSeconds(5)
                        });
                    });

                    Console.WriteLine($"{threadName} received data");
                    Thread.Sleep(rnd.Next(500));
                }

                Console.WriteLine($"{threadName} terminated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void CacheThread(object _data)
        {
            var data = (Tuple<RedisConnection, string>)_data;
            TestCacheAsync(data.Item1, data.Item2).Wait();
        }

        static void CacheThreadTest(RedisConnection redisConnection)
        {
            TerminateTestCacheAsync = false;
            var data1 = new Tuple<RedisConnection, string>(redisConnection, "Ross");
            var data2 = new Tuple<RedisConnection, string>(redisConnection, "Tammy");
            var data3 = new Tuple<RedisConnection, string>(redisConnection, "Madeline");
            var data4 = new Tuple<RedisConnection, string>(redisConnection, "Natalie");

            var t1 = new Thread(CacheThread);
            var t2 = new Thread(CacheThread);
            var t3 = new Thread(CacheThread);
            var t4 = new Thread(CacheThread);

            t1.Start(data1);
            t2.Start(data2);
            t3.Start(data3);
            t4.Start(data4);

            Console.ReadKey();
            TerminateTestCacheAsync = true;

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
        }

        public partial class IntegrationEntity
        {
            public Guid IntegrationId { get; set; }
            public Guid AppId { get; set; }
            public int IntegrationTypeId { get; set; }
            public int ChannelId { get; set; }
            public string Name { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime? ModifiedDateTime { get; set; }
            public DateTime? DeletedDateTime { get; set; }
            public string Configuration { get; set; }
            public bool Enabled { get; set; }
            public AppEntity App { get; set; }
        }


        public partial class AppEntity
        {
            public AppEntity()
            {
                Integrations = new HashSet<IntegrationEntity>();
            }

            public Guid AppId { get; set; }
            public Guid TenantId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime? ModifiedDateTime { get; set; }
            public DateTime? DeletedDateTime { get; set; }
            public bool Enabled { get; set; }            
            public ICollection<IntegrationEntity> Integrations { get; set; }
        }


        public class Crap<TValue>
        {
            public TValue Value { get; set; }
        }

        public class Crap: Crap<object>
        { 
        }


        public static Task<TValue> GetAsync<TValue>(RedisDictionary<string, Crap> _dictionary, string key, Func<string, TValue> onAdd = null)
        {            
            var result = _dictionary.GetOrAdd(key, (keyname) =>
            {
                var value = default(TValue);
                if (onAdd != null)
                {
                    value = onAdd.Invoke(keyname);
                }

                return new Crap { Value = value };
            });

            var xxx = _dictionary.ValueSerializer.Deserialize<Crap<TValue>>(result);

            return Task.FromResult(xxx.Value);
        }

        public static Task SetAsync<TValue>(RedisDictionary<string, Crap> _dictionary, string key, TValue value)
        {
            _dictionary.Add(key, new Crap { Value = value });

            return Task.CompletedTask;
        }

        public static Task<TValue> GetAsyncX<TValue>(RedisDictionary<string, object> _dictionary, string key, Func<string, TValue> onAdd = null)
        {
            var result = _dictionary.GetOrAdd(key, (keyname) =>
            {
                var value = default(TValue);
                if (onAdd != null)
                {
                    value = onAdd.Invoke(keyname);
                }

                return value;
            });

            var xxx = _dictionary.ValueSerializer.Deserialize<TValue>(result);

            return Task.FromResult(xxx);
        }

        public static Task SetAsyncX<TValue>(RedisDictionary<string, object> _dictionary, string key, TValue value)
        {
            _dictionary.Add(key, value);

            return Task.CompletedTask;
        }

        static public async Task RunAsync()
        {
            // string connectionString = "127.0.0.1:30001,127.0.0.1:30002,127.0.0.1:30003,127.0.0.1:30004,127.0.0.1:30005,127.0.0.1:30006";
            string connectionStrings = "localhost:6379,abortConnect=False";

            try
            {             
                using (var redisConnection = new RedisConnection(connectionStrings))
                {
                    var d = new RedisDictionary<string, Crap>(redisConnection.GetDatabase(), "test", new JsonRedisSerializer()); // new MsgPackRedisSerializer());        
                    var dtest = new Dictionary<int, string>();
                    dtest.Add(1, "Ross");
                    dtest.Add(2, "Tammy");

                    var v1 = await GetAsync(d, "age", name => 2);
                    await SetAsync(d, "age", (v1 + 2));
                    v1 = await GetAsync(d, "age", name => 3);

                    var v2 = await GetAsync(d, "dtest", name => dtest);                    
                    v2 = await GetAsync(d, "dtest", name => dtest);
                    


                    ////var v1 = await GetAsync(d, "age", name => (decimal)2);
                    ////await SetAsync(d, "age", (v1 + 2));
                    ////v1 = await GetAsync(d, "age", name => (decimal)3);

                    //return;

                    var lockAcquisition = new RedisCacheKeyLockAcquisition();
                    var cacheContainer = new StringRedisCacheContainer(redisConnection, "RedisCache"); // new HashRedisCacheContainer(redisConnection);
                    var cacheSeriizer = new MsgPackRedisSerializer(); // new JsonRedisSerializer();
                    var cache = new RedisCache(redisConnection, lockAcquisition, cacheContainer, cacheSeriizer);

                    var xxx = await cache.GetAsync("app:app:00000000-0000-0000-0000-0000000000a1", async (name, args) =>
                    {
                        var app = new AppEntity
                        {
                            AppId = Guid.NewGuid(),
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Description = "Hello",
                            Enabled = true,
                            Name = "test",
                            TenantId = Guid.Empty,
                        };

                        await Task.Delay(0);

                        return new CacheDataInfo<AppEntity>
                        {
                            KeepAlive = TimeSpan.FromHours(1),
                            NewCacheData = app, //"Hello, Ross"
                        };

                        // return (CacheDataInfo<string>)null;
                    });

                    // cache.Remove("test");


                    //CacheThreadTest(redisConnection);
                    //LockThreadTest(redisConnection);
                    //SerializerTests(redisConnection);
                    //Test4(redisConnection);
                    //Test1(redisConnection);
                    //Test3(redisConnection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
