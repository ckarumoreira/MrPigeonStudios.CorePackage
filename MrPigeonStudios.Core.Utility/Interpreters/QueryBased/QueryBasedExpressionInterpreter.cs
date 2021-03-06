using System;
using System.Collections.Generic;
using MrPigeonStudios.Core.Utility.Expressions;
using MrPigeonStudios.Core.Utility.Expressions.Operators;
using MrPigeonStudios.Core.Utility.Expressions.Plans;
using MrPigeonStudios.Core.Utility.Expressions.Values;

namespace MrPigeonStudios.Core.Utility.Interpreters.QueryBased {

    /// <summary>
    ///
    /// </summary>
    public class QueryBasedExpressionInterpreter : AbstractExpressionInterpreter {

        private interface IInterpreterData {
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="textExpression"></param>
        /// <returns></returns>
        public override ExpressionContext<TIn, TOut> GenerateContext<TIn, TOut>(string textExpression) {
            var operationQueue = ParseToInterpreterData<TIn>(textExpression);

            var reversePolishNotation = GetReversePolishNotation<TIn>(operationQueue);

            return new ExpressionContext<TIn, TOut>() {
                Value = RunReversePolishNotation<TIn>(reversePolishNotation)
            };
        }

        private IExpressionPart CreateBinaryPlan<TIn>(BinaryExpressionOperator @operator, IExpressionPart operandB, IExpressionPart operandA) {
            return new BinaryExpressionPlan<TIn> {
                Operator = @operator,
                LeftValue = operandA as IExpressionValue<TIn>,
                RightValue = operandB as IExpressionValue<TIn>
            };
        }

        private IExpressionPart CreateTernaryPlan<TIn>(TernaryExpressionOperator @operator, IExpressionPart operandC, IExpressionPart operandB, IExpressionPart operandA) {
            return new TernaryExpressionPlan<TIn> {
                Operator = @operator,
                FirstValue = operandA as IExpressionValue<TIn>,
                SecondValue = operandB as IExpressionValue<TIn>,
                ThirdValue = operandC as IExpressionValue<TIn>
            };
        }

        private IExpressionPart CreateUnaryPlan<TIn>(UnaryExpressionOperator @operator, IExpressionPart operand) {
            return new UnaryExpressionPlan<TIn> {
                Operator = @operator,
                Value = operand as IExpressionValue<TIn>
            };
        }

        private IExpressionPart GetFromData(IInterpreterData data) {
            if (data is InterpreterPart part) {
                return part.Value;
            }
            throw new InvalidOperationException($"Can't get expression from '{data}' type.");
        }

        private Queue<IExpressionPart> GetReversePolishNotation<TIn>(Queue<IInterpreterData> operationQueue) {
            Stack<IInterpreterData> operationStack = new();
            Queue<IExpressionPart> formula = new();

            while (operationQueue.Count > 0) {
                var current = operationQueue.Dequeue();
                if (IsDelimiter(current, true)) {
                    operationStack.Push(current);
                } else if (IsDelimiter(current, false)) {
                    while (operationStack.Count > 0 && !IsDelimiter(operationStack.Peek(), true)) {
                        formula.Enqueue(GetFromData(operationStack.Pop()));
                    }
                    operationStack.Pop();
                } else if (IsOperandus<TIn>(current)) {
                    formula.Enqueue(GetFromData(current));
                } else if (IsOperator(current)) {
                    while (operationStack.Count > 0 && !IsDelimiter(operationStack.Peek(), true)/* && Prior(x) <= Prior(stack.Peek())*/) {
                        formula.Enqueue(GetFromData(operationStack.Pop()));
                    }
                    operationStack.Push(current);
                } else {
                    var y = operationStack.Pop();
                    if (!IsDelimiter(y, true)) {
                        formula.Enqueue(GetFromData(y));
                    }
                }
            }

            while (operationStack.Count > 0) {
                formula.Enqueue(GetFromData(operationStack.Pop()));
            }

            return formula;
        }

        private bool IsDelimiter(IInterpreterData data, bool isOpen) {
            return data is InterpreterDelimiter delimiter && delimiter.State == isOpen;
        }

        private bool IsOperandus<TIn>(IInterpreterData data) {
            return data is InterpreterPart part && part.Value is IExpressionValue<TIn>;
        }

        private bool IsOperator(IInterpreterData data) {
            return data is InterpreterPart part && part.Value is IExpressionOperator;
        }

        private Queue<IInterpreterData> ParseToInterpreterData<TIn>(string textExpression) {
            Stack<string> accumulated = new();
            Queue<IInterpreterData> parts = new();

            // add control character to evaluate ending exp
            textExpression += '\0';

            var currentAccumulated = string.Empty;

            var interpreterStatus = new InterpreterStatus();

            for (var i = 0; i < textExpression.Length; i++) {
                var currentChar = textExpression[i];

                if (interpreterStatus.IsAvailable) {
                    // skip spaces outside of relevant scopes
                    if (currentChar == ' ') {
                        continue;
                    }

                    // open expression scope
                    if (currentChar == '(') {
                        accumulated.Push(currentAccumulated);
                        currentAccumulated = string.Empty;
                        parts.Enqueue(new InterpreterDelimiter { State = true });
                        continue;
                    }

                    // open attribute scope
                    if (currentChar == '[') {
                        interpreterStatus.IsDeclaringAttribute = true;
                        accumulated.Push(currentAccumulated);
                        currentAccumulated = string.Empty;
                        continue;
                    }

                    // open number scope
                    if (interpreterStatus.IsValidDigit(currentChar)) {
                        interpreterStatus.IsDeclaringNumber = true;
                        accumulated.Push(currentAccumulated);
                        currentAccumulated = currentChar.ToString();
                        continue;
                    }

                    // open string scope
                    if (currentChar == '\'') {
                        interpreterStatus.IsDeclaringString = true;
                        accumulated.Push(currentAccumulated);
                        currentAccumulated = string.Empty;
                        continue;
                    }

                    // close expression scope
                    if (currentChar == ')') {
                        currentAccumulated = accumulated.Pop();
                        parts.Enqueue(new InterpreterDelimiter { State = false });
                        continue;
                    }
                } else {
                    // close attribute scope
                    if (interpreterStatus.IsDeclaringAttribute && currentChar == ']') {
                        parts.Enqueue(new InterpreterPart { Value = ValidateAttribute<TIn>(currentAccumulated) });
                        currentAccumulated = accumulated.Pop();
                        interpreterStatus.IsDeclaringAttribute = false;
                        continue;
                    }

                    // close number scope
                    if (interpreterStatus.IsDeclaringNumber && !interpreterStatus.IsValidDigit(currentChar)) {
                        var number = interpreterStatus.IsValidNumber(currentAccumulated) ? double.Parse(currentAccumulated) : throw new InvalidOperationException($"'{currentAccumulated}' is not a valid number.");
                        parts.Enqueue(new InterpreterPart { Value = new ConstantExpressionValue<TIn>(number) });
                        currentAccumulated = accumulated.Pop();
                        interpreterStatus.IsDeclaringNumber = false;
                        i--;
                        continue;
                    }

                    // close string scope
                    if (interpreterStatus.IsDeclaringString && currentChar == '\'') {
                        parts.Enqueue(new InterpreterPart { Value = new ConstantExpressionValue<TIn>(currentAccumulated) });
                        currentAccumulated = accumulated.Pop();
                        interpreterStatus.IsDeclaringString = false;
                        continue;
                    }
                }

                currentAccumulated += textExpression[i];

                // match operation
                var operationCandidate = ValidateOperation<TIn>(currentAccumulated);
                if (operationCandidate != null) {
                    parts.Enqueue(new InterpreterPart { Value = operationCandidate });
                    currentAccumulated = string.Empty;
                }
            }

            return parts;
        }

        private IExpressionValue<TIn> RunReversePolishNotation<TIn>(Queue<IExpressionPart> parts) {
            Stack<IExpressionPart> stack = new();

            while (parts.Count > 0) {
                var part = parts.Dequeue();
                if (part is IExpressionOperator @operator) {
                    stack.Push(@operator switch {
                        UnaryExpressionOperator unary => CreateUnaryPlan<TIn>(unary, stack.Pop()),
                        BinaryExpressionOperator binary => CreateBinaryPlan<TIn>(binary, stack.Pop(), stack.Pop()),
                        TernaryExpressionOperator ternary => CreateTernaryPlan<TIn>(ternary, stack.Pop(), stack.Pop(), stack.Pop()),
                        _ => throw new InvalidOperationException($"Operators of type '{@operator}' are currently unsupported.")
                    });
                } else {
                    stack.Push(part);
                }
            }

            return stack.Pop() as IExpressionValue<TIn>;
        }

        private IExpressionValue<TIn> ValidateAttribute<TIn>(string expressionPart) {
            return new PropertyExpressionValue<TIn>(expressionPart);
        }

        private IExpressionOperator ValidateOperation<TIn>(string expressionPart) {
            return expressionPart switch {
                "!" => UnaryExpressionOperator.Not,
                "+" => BinaryExpressionOperator.Add,
                "&&" => BinaryExpressionOperator.And,
                "/" => BinaryExpressionOperator.Divide,
                "==" => BinaryExpressionOperator.Equal,
                ">" => BinaryExpressionOperator.Greater,
                ">=" => BinaryExpressionOperator.GreaterOrEqual,
                "<" => BinaryExpressionOperator.Less,
                "<=" => BinaryExpressionOperator.LessOrEqual,
                "*" => BinaryExpressionOperator.Multiply,
                "!=" => BinaryExpressionOperator.NotEqual,
                "||" => BinaryExpressionOperator.Or,
                "-" => BinaryExpressionOperator.Subtract,
                "IIF" => TernaryExpressionOperator.Condition,
                // "SUM" => LinqExpressionOperator.Sum,
                _ => null
            };
        }

        private struct InterpreterDelimiter : IInterpreterData {
            public bool State { get; set; }
        }

        private struct InterpreterPart : IInterpreterData {
            public IExpressionPart Value { get; set; }
        }

        private struct InterpreterStatus {
            public bool IsAvailable => !IsDeclaringString && !IsDeclaringNumber && !IsDeclaringAttribute;
            public bool IsDeclaringAttribute { get; set; }
            public bool IsDeclaringNumber { get; set; }
            public bool IsDeclaringString { get; set; }

            public bool IsValidDigit(char currentChar) {
                return currentChar == '.' || (currentChar >= '0' && currentChar <= '9');
            }

            public bool IsValidNumber(string numberCandidate) {
                return double.TryParse(numberCandidate, out _);
            }
        }
    }
}