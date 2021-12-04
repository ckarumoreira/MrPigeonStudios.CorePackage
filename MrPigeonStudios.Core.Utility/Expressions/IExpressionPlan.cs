namespace MrPigeonStudios.Core.Utility.Expressions {

    public interface IExpressionPlan<T> : IExpressionValue<T> {
        public IExpressionOperator Operator { get; set; }
    }
}