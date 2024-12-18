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
        try
        {
            var contents = File.ReadAllText(filename);
            run(contents);
            if (hadError)
            {
                Environment.Exit(65);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Cannot open file, it does not exist!");
            Environment.Exit(65);
        }
    }

    static void runPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var command = Console.ReadLine() ?? "";
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
            Console.WriteLine($"Line: {token.line}: {token.lexeme}\t\t{token.type}\t{token.literal}");
        }
    }

    public static void error(int line, string message)
    {
        report(line, "", message);
    }

    static void report(int line, string where, string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"[line {line}] Error {where} : {message}\n");
        hadError = true;
        Console.ForegroundColor = ConsoleColor.White;
    }
}

