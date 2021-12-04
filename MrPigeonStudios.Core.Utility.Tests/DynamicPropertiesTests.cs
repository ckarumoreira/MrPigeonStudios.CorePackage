using System;
using MrPigeonStudios.Core.Utility.DynamicProperties;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class DynamicPropertiesTests {

        private sealed class Dummy {
            public DynamicProperty Property { get; set; }
        }

        [Fact]
        public void DynamicProperty_BooleanProperty() {
            var dummy = new Dummy() {
                Property = DynamicProperty.Create(true)
            };

            Assert.IsType<bool>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsBoolean);
        }

        [Fact]
        public void DynamicProperty_DateTimeProperty() {
            var dummy = new Dummy() {
                Property = DateTime.Today
            };

            Assert.IsType<DateTime>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDateTime);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1d)]
        [InlineData(1f)]
        [InlineData((short)1)]
        [InlineData((long)1)]
        [InlineData((byte)1)]
        public void DynamicProperty_DoubleProperty(object number) {
            var dummy = new Dummy() {
                Property = DynamicProperty.Create(number)
            };

            Assert.IsType<double>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
        }

        [Fact]
        public void DynamicProperty_DynamicOnDynamic() {
            var dummy = new Dummy() {
                Property = DynamicProperty.Create(DynamicProperty.Create(1d))
            };

            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
        }

        [Fact]
        public void DynamicProperty_ImplicitOperators() {
            var dummy = new Dummy();

            dummy.Property = true;
            Assert.Equal(true, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsBoolean);
            Assert.True(true == dummy.Property);

            dummy.Property = (byte)1;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            dummy.Property = (short)1;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            dummy.Property = (int)1;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            dummy.Property = (long)1;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            dummy.Property = 1f;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            dummy.Property = 1d;
            Assert.Equal(1d, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);
            Assert.True(1d == dummy.Property);

            var date = new DateTime();
            dummy.Property = date;
            DateTime newDate = dummy.Property;
            Assert.Equal(date, dummy.Property.AsObject);
            Assert.Equal(newDate, dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDateTime);

            dummy.Property = string.Empty;
            string empty = dummy.Property;
            Assert.Equal(string.Empty, dummy.Property.AsObject);
            Assert.Equal(string.Empty, empty);
            Assert.True(dummy.Property.IsString);
        }

        [Fact]
        public void DynamicProperty_ImplicitOperators_Fallback() {
            var dummy = new Dummy();

            dummy.Property = 1d;
            string stringProp = dummy.Property;
            Assert.Equal(default(string), stringProp);

            dummy.Property = 1d;
            DateTime datetimeProp = dummy.Property;
            Assert.Equal(default(DateTime), datetimeProp);

            dummy.Property = string.Empty;
            double doubleProp = dummy.Property;
            Assert.Equal(default(double), doubleProp);

            bool boolProp = dummy.Property;
            Assert.Equal(default(bool), boolProp);
        }

        [Fact]
        public void DynamicProperty_InterchangeValues() {
            var dummy = new Dummy();

            dummy.Property = DynamicProperty.Create(DateTime.Today);
            Assert.IsType<DateTime>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDateTime);

            dummy.Property = DynamicProperty.Create(1d);
            Assert.IsType<double>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsDouble);

            dummy.Property = DynamicProperty.Create(string.Empty);
            Assert.IsType<string>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsString);

            dummy.Property = DynamicProperty.Create(null);
            Assert.Null(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsNull);
        }

        [Fact]
        public void DynamicProperty_NotSupportedProperty() {
            var exceptionType = typeof(InvalidOperationException);
            Assert.Throws(exceptionType, () => DynamicProperty.Create(new object()));
        }

        [Fact]
        public void DynamicProperty_NullProperty() {
            var dummy = new Dummy() {
                Property = DynamicProperty.Create(null)
            };

            Assert.Null(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsNull);
        }

        [Fact]
        public void DynamicProperty_StringProperty() {
            var dummy = new Dummy() {
                Property = DynamicProperty.Create(string.Empty)
            };

            Assert.IsType<string>(dummy.Property.AsObject);
            Assert.True(dummy.Property.IsString);
        }
    }
}