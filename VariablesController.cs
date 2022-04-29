namespace ConsoleCalc
{
    public static class VariablesController
    {
        private static readonly Dictionary<string, string> _variables = new();
        private static readonly ArgumentOutOfRangeException argumentOutOfRangeException = new();

        public static void SetVariable(string name, string value)
        {
            if (_variables.ContainsKey(name))
                _variables[name] = value;

            else _variables.Add(name, value);
        }

        public static string[] GetVariablesName() =>
            _variables.Keys.ToArray();

        public static string GetValueByVarName(string name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];

            throw argumentOutOfRangeException;
        }
    }
}
