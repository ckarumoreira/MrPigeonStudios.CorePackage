using System;

namespace MrPigeonStudios.Core.Utility.Expressions.Exceptions {

    public sealed class InvalidExpressionOperatorException : Exception {

        public InvalidExpressionOperatorException() : base("Invalid operation for this filter.") {
        }
    }
}