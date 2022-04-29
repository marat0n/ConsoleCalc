using ConsoleCalc;
using System.Text.RegularExpressions;
using System.Text;


void WriteToLog(string text)
{
    using StreamWriter file = File.AppendText("/log.txt");
    file.Write(text + "\n");
}

void CleanArgs() =>
    args = Array.Empty<string>();

static void Help()
{
    Console.WriteLine(
        "Hi! 💎\n" +
        "Log of your operations is activated. You can see it in the root directory.\n" +
        "\nOperations that you can use:\n" +
        "\t+ — for addition\n" +
        "\t- — for subtraction\n" +
        "\t* — for multiplication\n" +
        "\t/ — for division\n" +
        "\t^ — for rasing to power\n" +
        "\t~ — for rooting\n" +
        "\t% — for getting percent\n" +
        "Example: 2 + 5 * 4 - 1 / 2 + 3 ^ 2 ~ 2\n" +
        "Or you can type the `x` to manipulate the result of last expression\n" +
        "\nYou also can use these commands instead of expressions:\n" +
        "\tc | cls \t— clear result and console\n" +
        "\th | help\t- to get this instructions :)\n" +
        "\te | exit\t— for exit\n" +
        "\tv | vars\t— to get list of created variables" +
        "\nCreating your own variables:\n" +
        "\tCommand: var `variableName` `value`\n" +
        "\tExample: var y 10\n"
    );
}

void Clean()
{
    CleanArgs();
    Calculator.ClearLastResult();
    Console.Clear();
}

void Exit()
{
    Console.WriteLine("Bye!");
    Environment.Exit(0);
}

void GetVariables()
{
    foreach (var v in VariablesController.GetVariablesName())
        Console.WriteLine(v + ": " + VariablesController.GetValueByVarName(v));

    Console.WriteLine();
}

void RunCommand(string command)
{
    if (command == "cls" | command == "c")
        Clean();
    else if (command == "help" | command == "h")
        Help();
    else if (command == "exit" | command == "e")
        Exit();
    else if (command == "vars" | command == "v")
        GetVariables();
}

void ReplaceVariables()
{
    string[] userVars = VariablesController.GetVariablesName();

    for (int i = 0; i < args.Length; ++i)
    {
        if (args[i] == "x")
        {
            args[i] = Calculator.LastResult.ToString();
            continue;
        }
        
        foreach (var v in userVars)
        {
            if (args[i] == v)
            {
                args[i] = VariablesController.GetValueByVarName(v);
                break;
            }
        }
    }
}

void Calculate()
{
    try
    {
        ReplaceVariables();
        ArgsParser parser = new(args);
        Calculator.Calculate(parser.Queue.ClonedQueue);
        WriteToLog($"Calculated: {parser.Queue.ToString().Trim()} = {Calculator.LastResult}");
    }
    catch (ArgsParserException e)
    {
        Console.WriteLine(e.Message + "\nYou can run `help` command for more details.");

        if (e.InvalidExpression is not null)
            WriteToLog($"Unsuccessful try to calculate invalid expression: {string.Join(' ', e.InvalidExpression)}.");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("I can't divide by zero...");
        WriteToLog("Dividing by zero.");
    }
    catch (SystemException e)
    {
        Console.WriteLine(e.Message);
        WriteToLog($"Exception: {e.Message}.");
    }
}

void SetVariable()
{
    if (args.Length == 3)
    {
        Match varName = new Regex(@"^[a-zA-Z]\w*$").Match(args[1]);
        Match varValue = new Regex(@"^-{0,1}[\d]+$").Match(args[2]);

        if (varName.Success & varValue.Success)
        {
            if (varName.Value == "x")
            {
                Console.WriteLine("Impossible creation variable with name `x`");
                return;
            }
            VariablesController.SetVariable(varName.Value, varValue.Value);
            WriteToLog($"Setted variable `{varName.Value}` equals {varValue.Value}.");
            return;
        }
    }
    Console.WriteLine("You can create the variable just like this: var a 5");
}


WriteToLog("New session started at " + DateTime.Now);
while (true)
{
    if (args.Length == 1)
    {
        RunCommand(args[0]);
        CleanArgs();
    }
    else if (args.Length > 1)
    {
        if (args[0] == "var") SetVariable();
        else Calculate();
        CleanArgs();
    }
    else
    {
        Console.Write(Calculator.LastResult + "> ");
        args = (Console.ReadLine() ?? "").Split(' ');
    }
}
