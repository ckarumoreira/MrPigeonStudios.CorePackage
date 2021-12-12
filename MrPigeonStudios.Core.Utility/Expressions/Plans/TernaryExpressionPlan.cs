using System.Linq.Expressions;
using MrPigeonStudios.Core.Utility.Expressions.Exceptions;
using MrPigeonStudios.Core.Utility.Expressions.Operators;

namespace MrPigeonStudios.Core.Utility.Expressions.Plans {

    public class TernaryExpressionPlan<T> : IExpressionPlan<T> {
        public IExpressionValue<T> FirstValue { get; set; }
        public IExpressionOperator Operator { get; set; }
        public IExpressionValue<T> SecondValue { get; set; }
        public IExpressionValue<T> ThirdValue { get; set; }

        public Expression Compile(Expression origin) {
            var first = FirstValue.Compile(origin);
            var second = Expression.Convert(SecondValue.Compile(origin), typeof(object));
            var third = Expression.Convert(ThirdValue.Compile(origin), typeof(object));

            if (Operator is not TernaryExpressionOperator ternaryOperator) {
                throw new InvalidExpressionOperatorException();
            }

            return ternaryOperator.Operation(first, second, third);
        }

        public override string ToString() {
            return $"{Operator} {FirstValue} {SecondValue} {ThirdValue}";
        }
    }
}