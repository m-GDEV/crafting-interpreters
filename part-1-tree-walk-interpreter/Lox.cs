using System.Runtime.InteropServices;
using part_1_tree_walk_interpreter.Enums;

namespace part_1_tree_walk_interpreter;

class Lox
{
    static bool hadError = false;

    static void Main(string[] args)
    {

        if (args.Count() > 1)
        {
            Console.WriteLine("Usage: jlox [script]");
            Environment.Exit(64);
        }
        else if (args.Count() == 1)
        {
            runFile(args[0]);
        }
        else
        {
            runPrompt();
        }
    }

    static void runFile(string filename)
    {
        var contents = File.ReadAllText(filename);
        run(contents);
        if (hadError)
        {
            Environment.Exit(65);
        }
    }

    static void runPrompt()
    {
        List<string> commands = [""];

        while (true)
        {
            commands.Add("");
            Console.Write("> ");

            ConsoleKey currentChar = Console.ReadKey(false).Key;
            int currentCommandIndex = commands.Count - 1;

            while (currentChar != ConsoleKey.Enter)
            {
                currentChar = Console.ReadKey(false).Key;

                switch (currentChar)
                {
                    case ConsoleKey.UpArrow:
                        if (currentCommandIndex - 1 > 0)
                        {
                            currentCommandIndex -= 1;
                            ClearConsole();
                            Console.Write("> " + commands[currentCommandIndex]);
                            continue;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentCommandIndex + 1 < commands.Count)
                        {
                            currentCommandIndex += 1;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (commands[currentCommandIndex].Length > 0)
                        {
                            commands[currentCommandIndex] = commands[currentCommandIndex].Substring(0, commands[currentCommandIndex].Length - 1);
                        }
                        break;

                    case ConsoleKey.Enter:
                        break;
                    default:
                        commands[currentCommandIndex] += currentChar.ToString().ToLower();
                        break;

                }
                ClearConsole();
                Console.Write("> " + commands[currentCommandIndex]);
            }
            Console.WriteLine();

            string command = commands[currentCommandIndex];
            if (command == "exit" || command == "quit")
            {
                break;
            }
            run(command);
            hadError = false;
        }
    }

    static void run(string programLines)
    {
        var scanner = new Scanner(programLines);
        var tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine($"Line: {token.line}: {token.lexeme} | {token.type} | {token.literal}");
        }
    }

    public static void error(int line, string message)
    {
        report(line, "", message);
    }

    static void report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where} : {message}");
        hadError = true;
    }

    // Misc methods i need 
    static void ClearConsole()
    {
        int maxLineLength = 100;

        // Go back to beginning of line
        for (int i = 0; i < maxLineLength; i++)
        {
            Console.Write('\r');
        }
        // Print a bunch of spaces to make it look empty
        for (int i = 0; i < maxLineLength; i++)
        {
            Console.Write(' ');
        }
        // Go back to beginning of line (its now ready for something else to print)
        for (int i = 0; i < maxLineLength; i++)
        {
            Console.Write('\r');
        }

    }
}

