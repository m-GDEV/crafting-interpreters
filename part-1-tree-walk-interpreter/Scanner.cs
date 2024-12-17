namespace part_1_tree_walk_interpreter;

using part_1_tree_walk_interpreter.Enums;

class Scanner {
    private readonly string src;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string src)
    {
        this.src = src;
    }

    public List<Token> ScanTokens() {

        while(!isAtEnd()) {
            start = current;
            scanToken();
        }

        // Add one file 'EOF' token
        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private void ScanToken(string token) {

    }
}