using Antlr4.Runtime;
using Losi.grammar;

namespace Losi;

public class Evaluator
{
    private readonly Dictionary<string, double> variables;
    
    private readonly ProgramVisitor programVisitor;

    public Evaluator(Dictionary<string, double>? variables = null)
    {
        this.variables = variables ?? new();
        programVisitor = new ProgramVisitor(this.variables);
    }

    public bool Eval(string expression)
    {
        var charStream = new AntlrInputStream(expression);
        var lexer = new ExpressionGrammarLexer(charStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new ExpressionGrammarParser(tokenStream);
        var tree = parser.program();
        
        return tree.Accept(programVisitor).LogicValue;
    }
    
    public Evaluator Set(string name, double value)
    {
        variables[name] = value;
        return this;
    }
    
    public Evaluator Delete(string name)
    {
        variables.Remove(name);
        return this;
    }
}