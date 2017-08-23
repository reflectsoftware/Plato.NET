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

    public interface IMyMapper : IMapper<MapperTestClass1, MapperTestClass2>
    {
    }

    public class MyMapper : IMyMapper
    {
        public void Map(MapperTestClass1 source, MapperTestClass2 target)
        {
            target.TheName = source.Name;
            target.TheAddress = source.Address;
        }
    }

    public class MapperPlayground
    {
        static public Task RunAsync()
        {
            var class1 = new MapperTestClass1 { Name = "Ross", Address = "123 Main" };

            var mapper = new MyMapper() as IMyMapper;
            var class2a = mapper.Map(class1);
            var class2b = mapper.Map(class1, (source, target) =>
            {
            });

            var target1 = new MapperTestClass2 { TheName = "Ross", TheAddress = "123 Main" };

            mapper.Map(class1, target1, (source, target) =>
            {
            });
            
            return Task.CompletedTask;
        }
    }
}
