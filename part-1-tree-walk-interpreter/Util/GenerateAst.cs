using System.Dynamic;

namespace part_1_tree_walk_interpreter.Util;


// This class is meant to be run as a standalone script that generates the classes 
// necessary for our main program. You may modify this class to be a general purpose 
// util script that can do multiple things.
public class GenerateAst
{
    public static void Run(string outputDir)
    {
        var types = new List<AstType>
        {
            new AstType
            {
                ClassName = "Binary",
                Fields = [ "Expr left", "Token theOperator", "Expr right" ]
            },
            new AstType
            {
                ClassName = "Grouping",
                Fields = [ "Expr expression" ]
            },
            new AstType
            {
                ClassName = "Literal",
                Fields = [ "object value" ]
            },
            new AstType
            {
                ClassName = "Unary",
                Fields = [ "Token theOperator", "Expr right" ]
            }
        };

        defineAst(outputDir, "Expr", types);
    }

    // This does not do it exactly like how it is done in the book since C# is different from java
    private static void defineAst(string outputDir, string baseName, List<AstType> types)
    {

        var fileContents = "";

        fileContents += "namespace part_1_tree_walk_interpreter;\n";
        fileContents += $"abstract class {baseName} {{\n";

        // Making a class for each AstType
        foreach (var type in types)
        {
            // Class definition
            fileContents += $"\tclass {type.ClassName} : {baseName} {{\n";

            // Field definitions
            foreach (var field in type.Fields)
            {
                fileContents += $"\t\tpublic {field} {{ get; set; }}\n";
            }

            // Constructor
            fileContents += $"\t\tpublic {type.ClassName}({string.Join(", ", type.Fields)}) {{\n";
            foreach (var field in type.Fields)
            {
                var fieldName = field.Split(" ")[1];
                fileContents += $"\t\t\tthis.{fieldName} = {fieldName};\n";
            }
            fileContents += $"\t\t}}\n";


            fileContents += $"\t}}\n\n";
        }

        fileContents += $"}}\n";

        var path = outputDir + "/" + baseName + ".cs";
        File.WriteAllText(path, fileContents);
    }
}

class AstType
{
    public required string ClassName { get; set; }
    public required List<string> Fields { get; set; }
}