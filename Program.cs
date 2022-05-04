using ConsoleCalc;
using System.Text.RegularExpressions;


decimal lastResult = 0;
bool loggingIsActivated = true;

void TryWriteToLog(string text)
{
    if (loggingIsActivated)
    {
        using StreamWriter file = File.AppendText("/log.txt");
        file.Write(text + "\n");
    }
}

void CleanArgs() =>
    args = Array.Empty<string>();

void Help()
{
    var messageAboutLoggingStatus = loggingIsActivated ?
        "Log of your operations is activated. You can see it in the root directory.\n" :
        "Log of your operations is deactivated.";

    Console.WriteLine(
        "Hi! 💎\n" +
        messageAboutLoggingStatus +
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
        "\tl | log \t- to activate/deactivate logging" +
        "\nCreating your own variables:\n" +
        "\tCommand: var `variableName` `value`\n" +
        "\tExample: var y 10\n"
    );
}

void Clean()
{
    CleanArgs();
    lastResult = 0;
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

void ChangeLoggingActivation()
{
    loggingIsActivated = !loggingIsActivated;
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
    else if (command == "log" | command == "l")
        ChangeLoggingActivation();
}

void ReplaceVariables()
{
    string[] userVars = VariablesController.GetVariablesName();

    for (int i = 0; i < args.Length; ++i)
    {
        if (args[i] == "x")
        {
            args[i] = lastResult.ToString();
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
        lastResult = Calculator.LastResult;
        TryWriteToLog($"Calculated: {parser.Queue.ToString().Trim()} = {Calculator.LastResult}");
    }
    catch (ArgsParserException e)
    {
        Console.WriteLine(e.Message + "\nYou can run `help` command for more details.");

        if (e.InvalidExpression is not null)
            TryWriteToLog($"Unsuccessful try to calculate invalid expression: {string.Join(' ', e.InvalidExpression)}.");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("I can't divide by zero...");
        TryWriteToLog("Dividing by zero.");
    }
    catch (SystemException e)
    {
        Console.WriteLine(e.Message);
        TryWriteToLog($"Exception: {e.Message}.");
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
            TryWriteToLog($"Setted variable `{varName.Value}` equals {varValue.Value}.");
            return;
        }
    }
    Console.WriteLine("You can create the variable just like this: var a 5");
}


TryWriteToLog("New session started at " + DateTime.Now);
while (true)
{
    //lastResult = Calculator.LastResult;
    if (args.Length == 1)
    {
        if (decimal.TryParse(args[0], out decimal lonelyNumber))
        {
            lastResult = lonelyNumber;
        }
        else RunCommand(args[0]);

        CleanArgs();
    }
    if (args.Length > 1)
    {
        if (args[0] == "var") SetVariable();
        else Calculate();
        CleanArgs();
    }
    else if (args.Length == 0)
    {
        Console.Write(lastResult + "> ");
        args = (Console.ReadLine() ?? "").Split(' ');
    }
}
