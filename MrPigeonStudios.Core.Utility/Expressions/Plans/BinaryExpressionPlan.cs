using System.Linq.Expressions;
using MrPigeonStudios.Core.Utility.Expressions.Exceptions;
using MrPigeonStudios.Core.Utility.Expressions.Operators;

namespace MrPigeonStudios.Core.Utility.Expressions.Plans {

    public class BinaryExpressionPlan<T> : IExpressionPlan<T> {
        public IExpressionValue<T> LeftValue { get; set; }
        public IExpressionOperator Operator { get; set; }
        public IExpressionValue<T> RightValue { get; set; }

        public Expression Compile(Expression origin) {
            var left = LeftValue.Compile(origin);
            var right = RightValue.Compile(origin);

            if (Operator is not BinaryExpressionOperator binaryOperator) {
                throw new InvalidExpressionOperatorException();
            }

            return binaryOperator.Operation(left, right);
        }

        public override string ToString() {
            return $"{LeftValue} {Operator} {RightValue}";
        }
    }
}