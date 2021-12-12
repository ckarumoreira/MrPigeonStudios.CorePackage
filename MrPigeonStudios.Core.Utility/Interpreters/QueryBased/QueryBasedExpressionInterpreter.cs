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
            var context = new ExpressionContext<TIn, TOut>();
            var operationQueue = GetOperationQueue<TIn>(textExpression);

            var reversePolishQueue = GetReversePolishQueue<TIn>(operationQueue);

            Stack<IExpressionPart> stack = new();

            while (reversePolishQueue.Count > 0) {
                var part = reversePolishQueue.Dequeue();
                if (part is IExpressionOperator) {
                    if (part is UnaryExpressionOperator unary) {
                        var operandA = stack.Pop() as IExpressionValue<TIn>;

                        stack.Push(new UnaryExpressionPlan<TIn> {
                            Operator = unary,
                            Value = operandA
                        });
                    } else if (part is BinaryExpressionOperator binary) {
                        var operandA = stack.Pop() as IExpressionValue<TIn>;
                        var operandB = stack.Pop() as IExpressionValue<TIn>;

                        stack.Push(new BinaryExpressionPlan<TIn> {
                            Operator = binary,
                            LeftValue = operandB,
                            RightValue = operandA
                        });
                    } else if (part is TernaryExpressionOperator ternary) {
                        var operandA = stack.Pop() as IExpressionValue<TIn>;
                        var operandB = stack.Pop() as IExpressionValue<TIn>;
                        var operandC = stack.Pop() as IExpressionValue<TIn>;

                        stack.Push(new TernaryExpressionPlan<TIn> {
                            Operator = ternary,
                            FirstValue = operandC,
                            SecondValue = operandB,
                            ThirdValue = operandA
                        });
                    }
                } else {
                    stack.Push(part);
                }
            }

            context.Value = stack.Pop() as IExpressionValue<TIn>;

            return context;
        }

        private Queue<IInterpreterData> GetOperationQueue<TIn>(string textExpression) {
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

        private Queue<IExpressionPart> GetReversePolishQueue<TIn>(Queue<IInterpreterData> operationQueue) {
            Stack<IInterpreterData> stack = new();
            Queue<IInterpreterData> formula = new();

            while (operationQueue.Count > 0) {
                var current = operationQueue.Dequeue();
                if (IsDelimiter(current, true)) {
                    stack.Push(current);
                } else if (IsDelimiter(current, false)) {
                    while (stack.Count > 0 && !IsDelimiter(stack.Peek(), true)) {
                        formula.Enqueue(stack.Pop());
                    }
                    stack.Pop();
                } else if (IsOperandus<TIn>(current)) {
                    formula.Enqueue(current);
                } else if (IsOperator(current)) {
                    while (stack.Count > 0 && !IsDelimiter(stack.Peek(), true)/* && Prior(x) <= Prior(stack.Peek())*/) {
                        formula.Enqueue(stack.Pop());
                    }
                    stack.Push(current);
                } else {
                    var y = stack.Pop();
                    if (!IsDelimiter(y, true)) {
                        formula.Enqueue(y);
                    }
                }
            }

            while (stack.Count > 0) {
                formula.Enqueue(stack.Pop());
            }

            Queue<IExpressionPart> parts = new();

            while (formula.Count > 0) {
                if (formula.Dequeue() is InterpreterPart part) {
                    parts.Enqueue(part.Value);
                }
            }

            return parts;
        }

        private bool IsDelimiter(IInterpreterData data, bool isOpen) {
            if (data is InterpreterDelimiter delimiter)
                return delimiter.State == isOpen;
            return false;
        }

        private bool IsOperandus<TIn>(IInterpreterData data) {
            if (data is InterpreterPart part)
                return part.Value is IExpressionValue<TIn>;
            return false;
        }

        private bool IsOperator(IInterpreterData data) {
            if (data is InterpreterPart part)
                return part.Value is IExpressionOperator;
            return false;
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