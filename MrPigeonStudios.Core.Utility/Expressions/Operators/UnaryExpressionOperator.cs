using System;
using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions.Operators {

    public sealed class UnaryExpressionOperator : IExpressionOperator {
        public static UnaryExpressionOperator Not = new() { Keyword = "!", Operation = Expression.Not };

        public string Keyword { get; init; }
        public Func<Expression, Expression> Operation { get; init; }
    }
}