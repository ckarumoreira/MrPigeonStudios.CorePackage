using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions {

    public interface IExpressionValue<T> : IExpressionPart {

        public Expression Compile(Expression origin);
    }
}