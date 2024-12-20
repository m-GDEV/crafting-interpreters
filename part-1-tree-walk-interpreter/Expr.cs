namespace part_1_tree_walk_interpreter;
abstract class Expr
{
	class Binary : Expr
	{
		public Expr left { get; set; }
		public Token theOperator { get; set; }
		public Expr right { get; set; }
		public Binary(Expr left, Token theOperator, Expr right)
		{
			this.left = left;
			this.theOperator = theOperator;
			this.right = right;
		}
	}

	class Grouping : Expr
	{
		public Expr expression { get; set; }
		public Grouping(Expr expression)
		{
			this.expression = expression;
		}
	}

	class Literal : Expr
	{
		public object value { get; set; }
		public Literal(object value)
		{
			this.value = value;
		}
	}

	class Unary : Expr
	{
		public Token theOperator { get; set; }
		public Expr right { get; set; }
		public Unary(Token theOperator, Expr right)
		{
			this.theOperator = theOperator;
			this.right = right;
		}
	}

}
