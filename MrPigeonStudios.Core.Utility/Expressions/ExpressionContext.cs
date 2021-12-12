using System;
using System.Linq.Expressions;

namespace MrPigeonStudios.Core.Utility.Expressions {

    public sealed class ExpressionContext<T, TOut> {
        public IExpressionValue<T> Value { get; set; }

        public Func<T, TOut> Compile() {
            var source = Expression.Parameter(typeof(T), "source");
            var expression = Expression.Lambda<Func<T, TOut>>(Value.Compile(source), source);
            return expression.Compile();
        }
    }

    public sealed class ExpressionContext<TOut> {
        public IExpressionPlan<Void> Rule { get; set; }

        public Func<TOut> Compile() {
            var source = Expression.Parameter(typeof(Void), "source");
            var expression = Expression.Lambda<Func<Void, TOut>>(Rule.Compile(source), source);
            return () => expression.Compile()(Void.Instance);
        }
    }
}