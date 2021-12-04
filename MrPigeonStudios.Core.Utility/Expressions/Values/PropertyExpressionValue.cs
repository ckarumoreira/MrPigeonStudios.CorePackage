using System.Collections.Generic;
using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions.Values {

    public class PropertyExpressionValue<T> : IExpressionValue<T> {
        private readonly string accessor;
        private Expression _value;

        public PropertyExpressionValue(string propertyAccessor) {
            accessor = propertyAccessor;
        }

        public Expression Compile(Expression origin) {
            if (_value == null) {
                var propertyQueue = new Queue<string>(accessor.Split('.'));
                _value = origin;

                while (propertyQueue.Count > 0) {
                    _value = Expression.Property(_value, propertyQueue.Dequeue());
                }
            }

            return _value;
        }
    }
}