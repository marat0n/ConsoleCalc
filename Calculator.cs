using ConsoleCalc.CalculatorSyntaxQueue;
using ConsoleCalc.Infrastructure;

namespace ConsoleCalc
{
    public static class Calculator
    {
        private static List<ISyntaxQueueElement> _queueElements = new();
        public static decimal LastResult { get; private set; } = 0;

        private delegate decimal CalculationFunctions(decimal x, decimal y);

        public static void Calculate(List<ISyntaxQueueElement> queue)
        {
            _queueElements = queue;
            if (_queueElements.Count == 1) LastResult = _queueElements[0].Value;
            CalculatePriorityOperations();
            if (_queueElements.Count == 1) LastResult = _queueElements[0].Value;
            CalculateSecondaryOperations();

            LastResult = _queueElements[0].Value;
        }

        private static void CalculateOperation(int index, CalculationFunctions function)
        {
            decimal newResult = function(_queueElements[index-1].Value, _queueElements[index + 1].Value);
            _queueElements.RemoveAt(index + 1);
            _queueElements.RemoveAt(index);
            _queueElements.RemoveAt(index - 1);
            _queueElements.Insert(index - 1, new Number(newResult));
        }

        private static void CalculatePriorityOperations()
        {
            for (int i = 1; i < _queueElements.Count - 1; ++i)
            {
                var curr_el = _queueElements[i];
                if (curr_el is Operator @operator)
                {
                    var type = @operator.OperatorType;
                    if (type == OperatorType.Multiplication)
                        CalculateOperation(i, (decimal x, decimal y) => x * y);

                    else if (type == OperatorType.Division)
                        CalculateOperation(i, (decimal x, decimal y) => x / y);

                    else if (type == OperatorType.Power)
                        CalculateOperation(i, (decimal x, decimal y) =>
                            (decimal)Math.Pow((double)x, (double)y));

                    else if (type == OperatorType.Root)
                        CalculateOperation(i, (decimal x, decimal y) =>
                            (decimal)Math.Pow((double)x, 1 / (double)y));

                    else if (type == OperatorType.Percent)
                        CalculateOperation(i, (decimal x, decimal y) => x / y * 100);

                    else continue;

                    --i;
                }
            }
        }

        private static void CalculateSecondaryOperations()
        {
            for (int i = 1; i < _queueElements.Count - 1; ++i)
            {
                var curr_el = _queueElements[i];
                if (curr_el is Operator @operator)
                {
                    var type = @operator.OperatorType;
                    if (type == OperatorType.Plus)
                        CalculateOperation(i, (decimal x, decimal y) => x + y);

                    else if (type == OperatorType.Minus)
                        CalculateOperation(i, (decimal x, decimal y) => x - y);

                    else continue;

                    --i;
                }
            }
        }

        public static void ClearLastResult() => LastResult = 0;
    }
}
