using System;
using System.Collections.Generic;
using System.Linq;
using MrPigeonStudios.Core.Utility.DynamicObjects;
using MrPigeonStudios.Core.Utility.DynamicObjects.Comparers;
using MrPigeonStudios.Core.Utility.DynamicObjects.Generators;

namespace MrPigeonStudios.Core.Utility.Benchmarks.DynamicObjects {

    public class LoadDynamicObjects {

        public class Accessors {
            private DMetadata dynamicMetadata;
            private IEnumerable<DObject> dynamicObjects;
            private Type dynamicObjectType;
            private IEnumerable<PlainObject> plainObjects;
            private IEnumerable<int> range;

            public Accessors(int count) {
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

            public double SumDynamicObjects() => dynamicObjects.Sum(x => x.Get("PropertyA").AsDouble);

            public double SumPlainObjects() => plainObjects.Sum(x => x.PropertyA);
        }

        public class Comparer {
            private DMetadata dynamicMetadata;
            private IEnumerable<DObject> dynamicObjects;
            private Type dynamicObjectType;
            private IEnumerable<int> range;

            public Comparer(int count) {
                range = Enumerable.Range(1, count);
                dynamicMetadata = new DMetadata {
                    Id = Guid.NewGuid(),
                    MappedProperties = new List<DMappedProperty> {
                        DMappedProperty.Create("PropertyA"),
                        DMappedProperty.Create("PropertyB"),
                    }
                };
                dynamicObjectType = DObjectGenerator.GenerateType(dynamicMetadata);
                dynamicObjects = CreateDynamicObjects();
            }

            public List<DObject> CreateDynamicObjects() => range.Select(x => {
                var item = DObjectGenerator.CreateInstance(dynamicObjectType, dynamicMetadata);
                item.Set("PropertyA", x);
                item.Set("PropertyB", x / 10);
                return item;
            }).ToList();

            public List<PlainObject> CreatePlainObjects() => range.Select(x => new PlainObject { PropertyA = x }).ToList();

            public int DistinctWithComparer() => dynamicObjects.Distinct(new DObjectEqualityComparer("PropertyB")).Count();

            public int DistinctWithoutComparer() {
                var values = dynamicObjects.Select(x => x.Get("PropertyB")).Distinct();
                return values.Select(x => dynamicObjects.First(y => y.Get("PropertyB") == x)).Count();
            }
        }

        public class PlainObject {
            public double PropertyA { get; set; }
        }
    }
}