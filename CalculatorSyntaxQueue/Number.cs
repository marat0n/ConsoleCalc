namespace ConsoleCalc.CalculatorSyntaxQueue
{
    public class Number : ISyntaxQueueElement
    {
        public decimal Value { get; private set; }

        public Number(decimal value)
            => Value = value;

        public override string ToString() =>
            Value.ToString();
    }
}
