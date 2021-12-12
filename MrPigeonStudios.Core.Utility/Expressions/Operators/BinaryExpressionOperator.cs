using System;
using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions.Operators {

    public sealed class BinaryExpressionOperator : IExpressionOperator {
        public static BinaryExpressionOperator Add = new() { Operation = Expression.Add, Keyword = "+" };
        public static BinaryExpressionOperator And = new() { Operation = Expression.AndAlso, Keyword = "&&" };
        public static BinaryExpressionOperator Divide = new() { Operation = Expression.Divide, Keyword = "/" };
        public static BinaryExpressionOperator Equal = new() { Operation = Expression.Equal, Keyword = "==" };
        public static BinaryExpressionOperator Greater = new() { Operation = Expression.GreaterThan, Keyword = ">" };
        public static BinaryExpressionOperator GreaterOrEqual = new() { Operation = Expression.GreaterThanOrEqual, Keyword = ">=" };
        public static BinaryExpressionOperator Less = new() { Operation = Expression.LessThan, Keyword = "<" };
        public static BinaryExpressionOperator LessOrEqual = new() { Operation = Expression.LessThanOrEqual, Keyword = "<=" };
        public static BinaryExpressionOperator Multiply = new() { Operation = Expression.Multiply, Keyword = "+" };
        public static BinaryExpressionOperator NotEqual = new() { Operation = Expression.NotEqual, Keyword = "!=" };
        public static BinaryExpressionOperator Or = new() { Operation = Expression.OrElse, Keyword = "||" };
        public static BinaryExpressionOperator Subtract = new() { Operation = Expression.Subtract, Keyword = "-" };
        public string Keyword { get; init; }
        public Func<Expression, Expression, Expression> Operation { get; init; }

        public override string ToString() {
            return Keyword;
        }
    }
}