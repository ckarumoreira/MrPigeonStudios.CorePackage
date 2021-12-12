using System;
using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions.Operators {

    public sealed class TernaryExpressionOperator : IExpressionOperator {
        public static TernaryExpressionOperator Condition = new() { Keyword = "IIF", Operation = Expression.Condition };

        public string Keyword { get; init; }
        public Func<Expression, Expression, Expression, Expression> Operation { get; init; }
        
        public override string ToString() {
            return Keyword;
        }
    }
}