namespace ConsoleCalc.CalculatorSyntaxQueue
{
    public class SyntaxQueue
    {
        private readonly List<ISyntaxQueueElement> _queue;

        public List<ISyntaxQueueElement> ClonedQueue
        {
            get => new(_queue);
        }

        public ISyntaxQueueElement LastElement
        {
            get => _queue.Last();
        }

        public int Length
        {
            get => _queue.Count;
        }

        public bool IsEmpty
        {
            get => _queue.Count == 0;
        }

        public SyntaxQueue()
        {
            _queue = new List<ISyntaxQueueElement>();
        }

        public void AddElement(ISyntaxQueueElement newElement)
        {
            _queue.Add(newElement);
        }

        public override string ToString()
        {
            string result = string.Empty;
            string space = " ";

            foreach (var element in _queue)
                result += element + space;

            return result;
        }
    }
}
