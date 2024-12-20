namespace part_1_tree_walk_interpreter;

using part_1_tree_walk_interpreter.Enums;

class Token {
  public readonly TokenType type;
  public readonly string lexeme;
  public readonly object? literal;
  public readonly int line; 

  public Token(TokenType type, string lexeme, object? literal, int line) {
    this.type = type;
    this.lexeme = lexeme;
    this.literal = literal;
    this.line = line;
  }

  public string toString() {
    return type + " " + lexeme + " " + literal;
  }
}