namespace MrPigeonStudios.Core.Utility.Expressions {

    public interface IExpressionOperator : IExpressionPart {
        string Keyword { get; }
    }
}