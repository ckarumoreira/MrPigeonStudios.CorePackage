using System;
using System.Collections.Generic;
using MrPigeonStudios.Core.Utility.DynamicObjects;
using MrPigeonStudios.Core.Utility.DynamicObjects.Generators;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class DynamicObjectsTests {

        [Fact]
        public void DObjectGenerator_Generate() {
            DMetadata metadata = new() {
                Id = Guid.NewGuid(),
                MappedProperties = new List<DMappedProperty>() {
                    DMappedProperty.Create("PropertyOne"),
                    DMappedProperty.Create("PropertyTwo"),
                    DMappedProperty.Create("PropertyThree")
                }
            };
            var type = DObjectGenerator.GenerateType(metadata);
            var newObject = DObjectGenerator.CreateInstance(type, metadata);

            newObject.Set("PropertyOne", 1);
            newObject.Set("PropertyTwo", 2);
            newObject.Set("PropertyThree", 3);

            Assert.Equal(1, newObject.Get("PropertyOne").AsDouble);
            Assert.Equal(2, newObject.Get("PropertyTwo").AsDouble);
            Assert.Equal(3, newObject.Get("PropertyThree").AsDouble);
        }
    }
}