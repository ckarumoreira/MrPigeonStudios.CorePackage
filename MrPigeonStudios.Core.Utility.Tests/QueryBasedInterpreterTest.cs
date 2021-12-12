using MrPigeonStudios.Core.Utility.Interpreters.QueryBased;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class QueryBasedInterpreterTest {

        private class Dummy {
            public double DoubleValue { get; set; }
            public string TextValue { get; set; }
        }

        [Theory]
        [InlineData("[DoubleValue] + [DoubleValue] + [DoubleValue]", 1, 3)]
        [InlineData("[DoubleValue] + [DoubleValue] - [DoubleValue]", 1, 1)]
        [InlineData("[DoubleValue] / [DoubleValue] - [DoubleValue]", 2, -1)]
        [InlineData("3 + 5 / 2", 0, 4)]
        [InlineData("3 + (5 / 2)", 0, 5.5d)]
        [InlineData("(3 + 5) / 2", 0, 4)]
        public void QueryBased_MultiArithmetic(string expression, double input, double expectedResult) {
            var value = new Dummy() {
                DoubleValue = input
            };

            var queryBasedInterpreter = new QueryBasedExpressionInterpreter();

            var context = queryBasedInterpreter.GenerateContext<Dummy, double>(expression);

            var function = context.Compile();

            var result = function(value);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("[DoubleValue]", 1)]
        [InlineData("[DoubleValue]", 2)]
        [InlineData("[DoubleValue]", 3)]
        public void QueryBased_SimpleAccess(string expression, double expectedResult) {
            var value = new Dummy() {
                DoubleValue = expectedResult
            };

            var queryBasedInterpreter = new QueryBasedExpressionInterpreter();

            var context = queryBasedInterpreter.GenerateContext<Dummy, double>(expression);

            var function = context.Compile();

            var result = function(value);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("[DoubleValue] + [DoubleValue]", 1, 2)]
        [InlineData("[DoubleValue] + [DoubleValue]", 2, 4)]
        [InlineData("[DoubleValue] + [DoubleValue]", 3, 6)]
        [InlineData("[DoubleValue] * 2", 1, 2)]
        [InlineData("[DoubleValue] * 2", 2, 4)]
        [InlineData("[DoubleValue] * 2", 3, 6)]
        [InlineData("[DoubleValue] / 2", 2, 1)]
        [InlineData("[DoubleValue] / 2", 4, 2)]
        [InlineData("[DoubleValue] / 2", 6, 3)]
        public void QueryBased_SimpleArithmetic(string expression, double input, double expectedResult) {
            var value = new Dummy() {
                DoubleValue = input
            };

            var queryBasedInterpreter = new QueryBasedExpressionInterpreter();

            var context = queryBasedInterpreter.GenerateContext<Dummy, double>(expression);

            var function = context.Compile();

            var result = function(value);

            Assert.Equal(expectedResult, result);
        }
    }
}