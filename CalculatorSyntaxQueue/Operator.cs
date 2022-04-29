using ConsoleCalc.Infrastructure;

namespace ConsoleCalc.CalculatorSyntaxQueue
{
    public class Operator : ISyntaxQueueElement
    {
        public OperatorType OperatorType { get; private set; }
        public decimal Value { get => (decimal)OperatorType; }

        public Operator(OperatorType operatorType)
        {
            OperatorType = operatorType;
        }

        public override string ToString() =>
            ((char)OperatorType).ToString();
    }
}
