using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions {

    public interface IExpressionValue<T> {

        public Expression Compile(Expression origin);
    }
}