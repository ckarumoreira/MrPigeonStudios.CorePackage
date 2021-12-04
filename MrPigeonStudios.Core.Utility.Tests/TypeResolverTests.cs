using System;
using MrPigeonStudios.Core.Utility.DynamicObjects.TypeResolvers;
using MrPigeonStudios.Core.Utility.DynamicProperties;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class TypeResolverTests {

        [Fact]
        public void DateTimeResolver_DateToString() {
            var resolver = new DateTimeResolver("dd/MM/yyyy");

            var date = DateTime.SpecifyKind(new DateTime(1995, 3, 30), DateTimeKind.Utc);

            var resolved = resolver.Resolve(date);

            Assert.NotNull(resolved);
            Assert.Equal("30/03/1995", resolved);
        }

        [Fact]
        public void DateTimeResolver_InvalidResolverArgs() {
            var exceptionType = typeof(ArgumentNullException);
            Assert.Throws(exceptionType, () => new DateTimeResolver(null));
        }

        [Fact]
        public void DateTimeResolver_InvalidStringToDate() {
            var resolver = new DateTimeResolver("dd/MM/yyyy");

            var resolved = resolver.Resolve("300/13/1995");

            Assert.False(resolved.IsDateTime);
            Assert.Null(resolved.AsObject);
        }

        [Fact]
        public void DateTimeResolver_NotDateToString() {
            var resolver = new DateTimeResolver("dd/MM/yyyy");

            var resolved = resolver.Resolve(DynamicProperty.Create("not datetime"));

            Assert.Null(resolved);
        }

        [Fact]
        public void DateTimeResolver_ValidStringToDate() {
            var resolver = new DateTimeResolver("dd/MM/yyyy");

            var resolved = resolver.Resolve("30/03/1995");
            var expectedDate = DateTime.SpecifyKind(new DateTime(1995, 3, 30), DateTimeKind.Utc);

            Assert.True(resolved.IsDateTime);
            Assert.Equal(expectedDate, resolved.AsDateTime);
        }

        [Theory]
        [InlineData("N", ".", ",", 1000, "1,000.00")]
        [InlineData("N", ",", ".", 1000, "1.000,00")]
        [InlineData("N", ".", ",", 1000.5, "1,000.50")]
        [InlineData("N", ",", ".", 1000.5, "1.000,50")]
        [InlineData("N0", ".", ",", 1000, "1,000")]
        [InlineData("N0", ",", ".", 1000, "1.000")]
        [InlineData("N0", ".", ",", 1000.5, "1,000")]
        [InlineData("N0", ",", ".", 1000.5, "1.000")]
        public void DoubleResolver_DoubleToString(string format, string decimalSeparator, string thousandSeparator, double value, string expected) {
            var resolver = new DoubleResolver(format, decimalSeparator, thousandSeparator);

            var resolved = resolver.Resolve(value);

            Assert.NotNull(resolved);
            Assert.Equal(expected, resolved);
        }

        [Theory]
        [InlineData("N", ".", null)]
        [InlineData("N", null, ",")]
        [InlineData(null, ".", ",")]
        [InlineData("N", null, null)]
        [InlineData(null, ".", null)]
        [InlineData(null, null, ",")]
        [InlineData(null, null, null)]
        public void DoubleResolver_InvalidResolverArgs(string format, string decimalSeparator, string thousandSeparator) {
            var exceptionType = typeof(ArgumentNullException);
            Assert.Throws(exceptionType, () => new DoubleResolver(format, decimalSeparator, thousandSeparator));
        }

        [Fact]
        public void DoubleResolver_InvalidStringToDouble() {
            var resolver = new DoubleResolver("N", ".", ",");

            var resolved = resolver.Resolve("abc");

            Assert.False(resolved.IsDouble);
            Assert.Null(resolved.AsObject);
        }

        [Fact]
        public void DoubleResolver_NotDoubleToString() {
            var resolver = new DoubleResolver("N", ".", ",");

            var resolved = resolver.Resolve(DynamicProperty.Create("not double"));

            Assert.Null(resolved);
        }

        [Theory]
        [InlineData("N", ".", ",", 1000, "1,000.00")]
        [InlineData("N", ",", ".", 1000, "1.000,00")]
        [InlineData("N", ".", ",", 1000.5, "1,000.50")]
        [InlineData("N", ",", ".", 1000.5, "1.000,50")]
        public void DoubleResolver_ValidStringToDouble(string format, string decimalSeparator, string thousandSeparator, double expected, string value) {
            var resolver = new DoubleResolver(format, decimalSeparator, thousandSeparator);

            var resolved = resolver.Resolve(value);

            Assert.True(resolved.IsDouble);
            Assert.Equal(expected, resolved.AsDouble);
        }
    }
}