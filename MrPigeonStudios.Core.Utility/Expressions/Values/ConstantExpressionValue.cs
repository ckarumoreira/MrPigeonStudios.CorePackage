using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions.Values {

    public class ConstantExpressionValue<T> : IExpressionValue<T> {
        private readonly Expression _value;

        public ConstantExpressionValue(object value) {
            _value = Expression.Constant(value);
        }

        public Expression Compile(Expression origin) => _value;
    }
}