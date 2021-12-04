using System.Linq.Expressions;
using MrPigeonStudios.Core.Utility.Expressions.Exceptions;
using MrPigeonStudios.Core.Utility.Expressions.Operators;

namespace MrPigeonStudios.Core.Utility.Expressions.Rules {

    public class UnaryExpressionPlan<T> : IExpressionPlan<T> {
        public IExpressionOperator Operator { get; set; }
        public IExpressionValue<T> Value { get; set; }

        public Expression Compile(Expression origin) {
            var value = Value.Compile(origin);

            if (Operator is not UnaryExpressionOperator unaryOperator) {
                throw new InvalidExpressionOperatorException();
            }

            return unaryOperator.Operation(value);
        }
    }
}