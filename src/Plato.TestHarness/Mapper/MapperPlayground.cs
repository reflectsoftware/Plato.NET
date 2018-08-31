using Plato.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plato.TestHarness.Mapper
{
    public class MapperTestClass1
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class MapperTestClass2
    {
        public string TheName { get; set; }
        public string TheAddress { get; set; }
    }

    public interface IMyMapper : 
        IMapper<MapperTestClass1, MapperTestClass2>,
        IMapperAsync<MapperTestClass1, MapperTestClass2>
    {
    }

    public class MyMapper : IMyMapper
    {
        public void Map(MapperTestClass1 source, MapperTestClass2 target)
        {
            target.TheName = source.Name;
            target.TheAddress = source.Address;
        }

        public async Task MapAsync(MapperTestClass1 source, MapperTestClass2 target)
        {
            target.TheName = source.Name;
            target.TheAddress = source.Address;

            await Task.Delay(0);
        }
    }

    public class MapperPlayground
    {
        static public async Task RunAsync()
        {
            var class1 = new MapperTestClass1 { Name = "Ross", Address = "123 Main" };
            var mapper = new MyMapper() as IMyMapper;
            var target1 = new MapperTestClass2 { TheName = "Ross", TheAddress = "123 Main" };

            var class2a = mapper.Map(class1);
            var class2b = mapper.Map(class1, (source, target) =>
            {
            });

            var class2c = mapper.Map(class1, target1, (source, target) =>
            {
            });

            // async 
            var class3a = await mapper.MapAsync(class1);
            var class3b = await mapper.MapAsync(class1, async (source, target) =>
            {
                await Task.Delay(0);
            });

            var class3c = await mapper.MapAsync(class1, target1, async (source, target) =>
            {
                await Task.Delay(0);
            });
        }
    }
}
