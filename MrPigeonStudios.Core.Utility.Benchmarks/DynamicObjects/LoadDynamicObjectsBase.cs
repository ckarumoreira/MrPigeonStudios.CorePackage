using System;
using System.Collections.Generic;
using System.Linq;
using MrPigeonStudios.Core.Utility.DynamicObjects;
using MrPigeonStudios.Core.Utility.DynamicObjects.Generators;

namespace MrPigeonStudios.Core.Utility.Benchmarks.DynamicObjects {

    public class LoadDynamicObjectsBase {

        public class PlainObject {
            public double PropertyA { get; set; }
        }

        private DMetadata dynamicMetadata;
        private IEnumerable<DObject> dynamicObjects;
        private Type dynamicObjectType;
        private IEnumerable<PlainObject> plainObjects;
        private IEnumerable<int> range;

        public LoadDynamicObjectsBase(int count) {
            range = Enumerable.Range(1, count);
            dynamicMetadata = new DMetadata {
                Id = Guid.NewGuid(),
                MappedProperties = new List<DMappedProperty> {
                    DMappedProperty.Create("PropertyA")
                }
            };
            dynamicObjectType = DObjectGenerator.GenerateType(dynamicMetadata);
            plainObjects = CreatePlainObjects();
            dynamicObjects = CreateDynamicObjects();
        }

        public List<DObject> CreateDynamicObjects() => range.Select(x => {
            var item = DObjectGenerator.CreateInstance(dynamicObjectType, dynamicMetadata);
            item.Set("PropertyA", x);
            return item;
        }).ToList();

        public List<PlainObject> CreatePlainObjects() => range.Select(x => new PlainObject { PropertyA = x }).ToList();

        // [Benchmark]
        public double SumDynamicObjects() => dynamicObjects.Sum(x => x.Get("PropertyA").AsDouble);

        // [Benchmark]
        public double SumPlainObjects() => plainObjects.Sum(x => x.PropertyA);
    }
}