namespace part_1_tree_walk_interpreter;

using System.Data.Common;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using part_1_tree_walk_interpreter.Enums;

public class Keyword(string identifier, TokenType type)
{
    public string Identifier = identifier;
    public TokenType Type = type;
}

class Scanner
{
    private readonly string src;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    private static readonly HashSet<Keyword> keywords =
    [
        new Keyword("and",    TokenType.AND),
        new Keyword("class",  TokenType.CLASS),
        new Keyword("else",   TokenType.ELSE),
        new Keyword("false",  TokenType.FALSE),
        new Keyword("for",    TokenType.FOR),
        new Keyword("fun",    TokenType.FUN),
        new Keyword("if",     TokenType.IF),
        new Keyword("nil",    TokenType.NIL),
        new Keyword("or",     TokenType.OR),
        new Keyword("print",  TokenType.PRINT),
        new Keyword("return", TokenType.RETURN),
        new Keyword("super",  TokenType.SUPER),
        new Keyword("this",   TokenType.THIS),
        new Keyword("true",   TokenType.TRUE),
        new Keyword("var",    TokenType.VAR),
        new Keyword("while",  TokenType.WHILE),
    ];

    public Scanner(string src)
    {
        this.src = src;
    }

    public List<Token> ScanTokens()
    {

        while (!isAtEnd())
        {
            start = current;
            ScanToken();
        }

        // Add one file 'EOF' token
        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private void ScanToken()
    {
        char c = advance();
        switch (c)
        {
            case '(': addToken(TokenType.LEFT_PAREN); break;
            case ')': addToken(TokenType.RIGHT_PAREN); break;
            case '{': addToken(TokenType.LEFT_BRACE); break;
            case '}': addToken(TokenType.RIGHT_BRACE); break;
            case ',': addToken(TokenType.COMMA); break;
            case '.': addToken(TokenType.DOT); break;
            case '-': addToken(TokenType.MINUS); break;
            case '+': addToken(TokenType.PLUS); break;
            case ';': addToken(TokenType.SEMICOLON); break;
            case '*': addToken(TokenType.STAR); break;

            // The match statements look 1 character ahead and check if the current char plus the next
            // would create a viable token type
            case '!':
                addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            // Special case for comments
            case '/':
                // A comment goes until the end of the line.
                // NOTE: we do not add any tokens for the comment they essentially disappear
                if (match('/'))
                {
                    while (peek() != '\n' && !isAtEnd()) advance();
                }
                // A C-style comment goes until the end of the line.
                // NOTE: we do not add any tokens for the comment they essentially disappear
                else if (match('*')) {
                    char currentChar = peek();
                    while (currentChar != '\n' && currentChar != '*' && !isAtEnd()) {
                        currentChar = advance();
                    }

                    switch (currentChar) {
                        case '\n':
                            Lox.error(line, "Cannot end C-style comment with newline");
                            break;
                        case '*':
                            // check if next char is /
                            if (!match('/')) {
                               Lox.error(line, "Missing '/' to terminate C-style comment");
                            }
                            break;

                        default:
                            Lox.error(line, "Unrecognized terminator at end of C-style comment");
                            break;
                    }


                }
                else
                {
                    addToken(TokenType.SLASH);
                }
                break;
            // Do nothing when we encounter whitespace and newlines
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            case '\n':
                line++;
                break;

            // Complex sequences of chars 
            case '"': processString(); break;

            // If we don't recognize the character, report an error
            default:
                if (isDigit(c))
                {
                    processNumber();
                }
                // This implicitly only allows indentifiers (language keywords + user defined variable names) 
                // to begin with letters and not numbers
                else if (isAlpha(c))
                {
                    processIdentifier();
                }
                else
                {
                    Lox.error(line, "Unexpected character.");
                }
                break;
        }
    }

    // Helper methods used by ScanToken
    private void processString()
    {
        while (peek() != '"' && !isAtEnd())
        {
            // This allows multi-line strings
            if (peek() == '\n')
            {
                line++;
            }
            advance();
        }

        if (isAtEnd())
        {
            Lox.error(line, "Unterminated string.");
            return;
        }

        // Closing "
        advance();

        string val = getSubstring(src, start + 1, current - 1);
        addToken(TokenType.STRING, val);
    }

    private void processNumber()
    {
        // This method stops before a newline. Therefore the number must be on one line
        while (isDigit(peek()))
        {
            advance();
        }

        if (peek() == '.' && isDigit(peekNext()))
        {
            advance();
            while (isDigit(peek()))
            {
                advance();
            }
        }

        addToken(TokenType.NUMBER, double.Parse(getSubstring(src, start, current)));
    }

    // Process identifiers (variable names, etc). There is a special case for reserved identifiers (i.e. keywords)
    private void processIdentifier()
    {
        // Process the whole identifier before trying to match with a keyword
        // This means 'orsomething' will not match the keyword 'or'
        while (isAlphaNumeric(peek())) advance();

        TokenType defaultType = TokenType.IDENTIFIER;

        var iden = getSubstring(src, start, current);
        var keyword = keywords.Where(s => s.Identifier == iden).FirstOrDefault();

        // If there is a keyword associated with this identifier, change the token type 
        if (keyword != null)
        {
            defaultType = keyword.Type;
        }

        addToken(defaultType);
    }

    // Helper Methods
    private bool isAtEnd()
    {
        return current >= src.Count();
    }

    private char advance()
    {
        return src[current++];
    }

    private void addToken(TokenType type)
    {
        addToken(type, null);
    }
    private void addToken(TokenType type, object? literal)
    {
        string text = getSubstring(src, start, current);
        var token = new Token(type, text, literal, line);
        tokens.Add(token);
    }
    private bool match(char expected)
    {
        if (isAtEnd()) return false;
        if (src[current] != expected) return false;

        current++;
        return true;
    }

    private char peek()
    {
        if (isAtEnd()) return '\0';
        // should this not be current + 1 ?
        return src[current];
    }

    private char peekNext()
    {
        if (current + 1 >= src.Count()) return '\0';

        return src[current + 1];
    }

    // Just checking based on ASCII i think 
    private bool isDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    // Just checking based on ASCII i think 
    private bool isAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') ||
       (c >= 'A' && c <= 'Z') ||
        c == '_';
    }

    private bool isAlphaNumeric(char c)
    {
        return isAlpha(c) || isDigit(c);
    }

    private string getSubstring(string s, int begin, int end) {
        if (end > s.Length || start > s.Length) {
            throw new InvalidOperationException("End or Start position cannot be larger that string length");
        }
        else if (begin < 0 || end < 0) {
            throw new InvalidOperationException("Negative slices are not supported");
        }

        var substring = "";

        for (int i = begin; i < end; i++) {
            substring += s[i];
        }

        return substring;

    }

}