using ConsoleCalc.CalculatorSyntaxQueue;
using ConsoleCalc.Infrastructure;

namespace ConsoleCalc
{
    public class ArgsParser
    {
        public SyntaxQueue Queue { get; private set; }

        public ArgsParser(string[] args)
        {
            Queue = new SyntaxQueue();
            if (args.Length < 3) throw new NotEnoughExpressionElementsException(args);

            foreach (var arg in args)
            {
                if (decimal.TryParse(arg, out decimal numValue))
                {
                    if (Queue.Length > 0)
                        if (Queue.LastElement is Number) throw new ArgsParserException(args);
                    Queue.AddElement(new Number(numValue));
                    continue;
                }
                else if (ParseToChar(arg, out char opValue))
                {
                    if (Queue.Length == 0) throw new ArgsParserException(args);
                    if (Queue.LastElement is Operator) throw new ArgsParserException(args);

                    if (Enum.IsDefined(typeof(OperatorType), (int)opValue))
                    {
                        Queue.AddElement(new Operator((OperatorType)opValue));
                        continue;
                    }
                }

                throw new ArgsParserException();
            }

            if (Queue.LastElement is Operator) throw new LastExpressionElementIsOperatorException(args);
        }

        private static bool ParseToChar(string str, out char result)
        {
            if (str.Length == 1)
            {
                result = str[0];
                return true;
            }

            throw new ArgsParserException();
        }
    }


    public class ArgsParserException : SystemException
    {
        public string[]? InvalidExpression { get; protected set; }

        public override string Message => "You can use only numbers and +, -, *, /, ^, ~ symbols.";

        public ArgsParserException() { }
        public ArgsParserException(string[] invalidExpression) =>
            InvalidExpression = invalidExpression;

    }

    public class NotEnoughExpressionElementsException : ArgsParserException
    {
        public NotEnoughExpressionElementsException(string[] invalidExpression) : base(invalidExpression) =>
            InvalidExpression = invalidExpression;

        public override string Message => "Not enough setted elements in expression. Minimal expression contains 2 numbers and 1 operator.";
    }

    public class LastExpressionElementIsOperatorException : ArgsParserException
    {
        public LastExpressionElementIsOperatorException(string[] invalidExpression) : base(invalidExpression) =>
            InvalidExpression = invalidExpression;

        public override string Message => "Last expression elements can't be an operator.";
    }
}