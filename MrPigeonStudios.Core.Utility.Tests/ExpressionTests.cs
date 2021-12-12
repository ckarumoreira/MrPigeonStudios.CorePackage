using MrPigeonStudios.Core.Utility.Expressions;
using MrPigeonStudios.Core.Utility.Expressions.Exceptions;
using MrPigeonStudios.Core.Utility.Expressions.Operators;
using MrPigeonStudios.Core.Utility.Expressions.Plans;
using MrPigeonStudios.Core.Utility.Expressions.Values;
using Xunit;

namespace MrPigeonStudios.Core.Utility.Tests {

    public class ExpressionTests {

        private sealed class Dummy {
            public bool Boolean { get; set; }
            public int Integer { get; set; }
            public Dummy Object { get; set; }
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(1, 2, 3)]
        public void FilterContext_Binary_AddOperator(int valueA, int valueB, int assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<Dummy, int> {
                Value = new BinaryExpressionPlan<Dummy>() {
                    LeftValue = new ConstantExpressionValue<Dummy>(valueA),
                    RightValue = new ConstantExpressionValue<Dummy>(valueB),
                    Operator = BinaryExpressionOperator.Add
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression(dummy);

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void FilterContext_Binary_AndOperator(bool valueA, bool valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.And
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(1d, 1d, 1d)]
        [InlineData(1d, 2d, 0.5d)]
        public void FilterContext_Binary_DivideOperator(double valueA, double valueB, double assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<Dummy, double> {
                Value = new BinaryExpressionPlan<Dummy>() {
                    LeftValue = new ConstantExpressionValue<Dummy>(valueA),
                    RightValue = new ConstantExpressionValue<Dummy>(valueB),
                    Operator = BinaryExpressionOperator.Divide
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression(dummy);

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(1, 2, false)]
        [InlineData(1, 1, false)]
        public void FilterContext_Binary_GreaterOperator(int valueA, int valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.Greater
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(1, 2, false)]
        [InlineData(1, 1, true)]
        public void FilterContext_Binary_GreaterOrEqualOperator(int valueA, int valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.GreaterOrEqual
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(2, 1, false)]
        [InlineData(1, 2, true)]
        [InlineData(1, 1, false)]
        public void FilterContext_Binary_LessOperator(int valueA, int valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.Less
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(2, 1, false)]
        [InlineData(1, 2, true)]
        [InlineData(1, 1, true)]
        public void FilterContext_Binary_LessOrEqualOperator(int valueA, int valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.LessOrEqual
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 2)]
        public void FilterContext_Binary_MultiplyOperator(int valueA, int valueB, int assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<int> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.Multiply
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, false, false)]
        public void FilterContext_Binary_OrOperator(bool valueA, bool valueB, bool assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<bool> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.Or
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 2, -1)]
        public void FilterContext_Binary_SubtractOperator(int valueA, int valueB, int assertion) {
            var dummy = new Dummy() { };

            var context = new ExpressionContext<int> {
                Rule = new BinaryExpressionPlan<Void>() {
                    LeftValue = new ConstantExpressionValue<Void>(valueA),
                    RightValue = new ConstantExpressionValue<Void>(valueB),
                    Operator = BinaryExpressionOperator.Subtract
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression();

            Assert.Equal(assertion, result);
        }

        [Fact]
        public void FilterContext_BinaryFilterWithNonBinaryOperator() {
            var context = new ExpressionContext<Dummy, bool> {
                Value = new BinaryExpressionPlan<Dummy>() {
                    LeftValue = new PropertyExpressionValue<Dummy>("Integer"),
                    RightValue = new PropertyExpressionValue<Dummy>("Integer"),
                    Operator = TernaryExpressionOperator.Condition
                }
            };

            var exceptionType = typeof(InvalidExpressionOperatorException);

            Assert.Throws(exceptionType, context.Compile);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void FilterContext_DeepProperty(int sourceShallowValue, int sourceDeepValue, bool assertion) {
            var dummy = new Dummy() {
                Integer = sourceShallowValue,
                Object = new Dummy() {
                    Integer = sourceDeepValue
                }
            };

            var context = new ExpressionContext<Dummy, bool> {
                Value = new BinaryExpressionPlan<Dummy> {
                    LeftValue = new PropertyExpressionValue<Dummy>("Integer"),
                    RightValue = new PropertyExpressionValue<Dummy>("Object.Integer"),
                    Operator = BinaryExpressionOperator.Equal
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression(dummy);

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void FilterContext_ShallowProperty(int sourceValue, int comparedValue, bool assertion) {
            var dummy = new Dummy() { Integer = sourceValue };

            var context = new ExpressionContext<Dummy, bool> {
                Value = new BinaryExpressionPlan<Dummy>() {
                    LeftValue = new PropertyExpressionValue<Dummy>("Integer"),
                    RightValue = new ConstantExpressionValue<Dummy>(comparedValue),
                    Operator = BinaryExpressionOperator.Equal
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression(dummy);

            Assert.Equal(assertion, result);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void FilterContext_Ternary_Condition(bool condition, object expectedResult) {
            var dummy = new Dummy() {
                Boolean = true,
                Integer = 1,
                Object = new Dummy() {
                    Integer = 2
                }
            };

            var context = new ExpressionContext<Dummy, object> {
                Value = new TernaryExpressionPlan<Dummy>() {
                    FirstValue = new ConstantExpressionValue<Dummy>(condition),
                    SecondValue = new PropertyExpressionValue<Dummy>("Integer"),
                    ThirdValue = new PropertyExpressionValue<Dummy>("Object.Integer"),
                    Operator = TernaryExpressionOperator.Condition
                }
            };

            var compiledExpression = context.Compile();

            var result = compiledExpression(dummy);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FilterContext_TernaryFilterWithNonBinaryOperator() {
            var context = new ExpressionContext<Dummy, bool> {
                Value = new TernaryExpressionPlan<Dummy>() {
                    FirstValue = new PropertyExpressionValue<Dummy>("Integer"),
                    SecondValue = new PropertyExpressionValue<Dummy>("Integer"),
                    ThirdValue = new PropertyExpressionValue<Dummy>("Integer"),
                    Operator = UnaryExpressionOperator.Not
                }
            };

            var exceptionType = typeof(InvalidExpressionOperatorException);

            Assert.Throws(exceptionType, context.Compile);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void FilterContext_Unary_NotOperator(bool source, bool assertion) {
            var dummy = new Dummy { Boolean = source };

            var context = new ExpressionContext<Dummy, bool> {
                Value = new UnaryExpressionPlan<Dummy> {
                    Value = new PropertyExpressionValue<Dummy>("Boolean"),
                    Operator = UnaryExpressionOperator.Not
                }
            };

            var expression = context.Compile();

            var result = expression(dummy);

            Assert.Equal(assertion, result);
        }

        [Fact]
        public void FilterContext_UnaryFilterWithNonBinaryOperator() {
            var context = new ExpressionContext<Dummy, bool> {
                Value = new UnaryExpressionPlan<Dummy>() {
                    Value = new PropertyExpressionValue<Dummy>("Integer"),
                    Operator = TernaryExpressionOperator.Condition
                }
            };

            var exceptionType = typeof(InvalidExpressionOperatorException);

            Assert.Throws(exceptionType, context.Compile);
        }
    }
}