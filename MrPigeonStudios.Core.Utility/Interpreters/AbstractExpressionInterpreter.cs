using MrPigeonStudios.Core.Utility.Expressions;

namespace MrPigeonStudios.Core.Utility.Interpreters {

    public abstract class AbstractExpressionInterpreter {

        public abstract ExpressionContext<TIn, TOut> GenerateContext<TIn, TOut>(string textExpression);
    }
}