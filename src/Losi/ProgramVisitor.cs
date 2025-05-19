using System.Globalization;
using Losi.grammar;

namespace Losi;

public class ProgramVisitor : ExpressionGrammarBaseVisitor<VisitResult>
{
    private readonly double epsilon;

    private readonly Dictionary<string, double> variables;

    public ProgramVisitor(Dictionary<string, double> variables, double epsilon = double.Epsilon)
    {
        this.variables = variables;
        this.epsilon = epsilon;
    }

    public override VisitResult VisitProgram(ExpressionGrammarParser.ProgramContext context)
    {
        return Visit(context.logicalExpression());
    }
    
    public override VisitResult VisitAndLogicalExpr(ExpressionGrammarParser.AndLogicalExprContext context)
    {
        var left = Visit(context.logicalExpression(0)).LogicValue;
        var right = Visit(context.logicalExpression(1)).LogicValue;
        return new VisitResult(left && right);
    }
    
    public override VisitResult VisitOrLogicalExpr(ExpressionGrammarParser.OrLogicalExprContext context)
    {
        var left = Visit(context.logicalExpression(0)).LogicValue;
        var right = Visit(context.logicalExpression(1)).LogicValue;
        return new VisitResult(left || right);
    }
    
    public override VisitResult VisitNotLogicalExpr(ExpressionGrammarParser.NotLogicalExprContext context)
    {
        return new VisitResult(!Visit(context.logicalExpression()).LogicValue);
    }

    public override VisitResult VisitParenLogicalExpr(ExpressionGrammarParser.ParenLogicalExprContext context)
    {
        // LPAREN logicalExpression RPAREN
        return Visit(context.logicalExpression());
    }

    public override VisitResult VisitValueCastExpr(ExpressionGrammarParser.ValueCastExprContext context)
    {
        var value = Visit(context.mathExpression()).DoubleValue;
        return new VisitResult(value > 0);
    }

    public override VisitResult VisitBinaryComparisonExpr(ExpressionGrammarParser.BinaryComparisonExprContext context)
    {
        double left = Visit(context.mathExpression(0)).DoubleValue;
        double right = Visit(context.mathExpression(1)).DoubleValue;

        switch (context.op.Type)
        {
            case ExpressionGrammarParser.GT:
                return new VisitResult(left > right);
            case ExpressionGrammarParser.LT:
                return new VisitResult(left < right);
            case ExpressionGrammarParser.EQ:
                return new VisitResult(Math.Abs(left - right) <= epsilon);
            case ExpressionGrammarParser.GTE:
                return new VisitResult(left >= right);
            case ExpressionGrammarParser.LTE:
                return new VisitResult(left <= right);
            case ExpressionGrammarParser.NEQ:
                return new VisitResult(Math.Abs(left - right) > epsilon);
            default:
                throw new InvalidOperationException($"Unknown BinaryComparison operator: {context.op.Type}");
        }
    }

    public override VisitResult VisitMulDivExrp(ExpressionGrammarParser.MulDivExrpContext context)
    {
        double left = Visit(context.mathExpression(0)).DoubleValue;
        double right = Visit(context.mathExpression(1)).DoubleValue;

        switch (context.op.Type)
        {
            case ExpressionGrammarParser.MUL:
                return new VisitResult(left * right);
            case ExpressionGrammarParser.DIV:
                return new VisitResult(left / right);
            default:
                throw new InvalidOperationException($"Unknown multiplicative operator: {context.op.Type}");
        }
    }

    public override VisitResult VisitUnaryMinusExpr(ExpressionGrammarParser.UnaryMinusExprContext context)
    {
        return new VisitResult(-Visit(context.mathAtom()).DoubleValue);
    }

    public override VisitResult VisitPlusMinusExrp(ExpressionGrammarParser.PlusMinusExrpContext context)
    {
        double left = Visit(context.mathExpression(0)).DoubleValue;
        double right = Visit(context.mathExpression(1)).DoubleValue;

        switch (context.op.Type)
        {
            case ExpressionGrammarParser.PLUS:
                return new VisitResult(left + right);
            case ExpressionGrammarParser.MINUS:
                return new VisitResult(left - right);
            default:
                throw new InvalidOperationException($"Unknown additive operator: {context.op.Type}");
        }
    }

    public override VisitResult VisitMathAtom(ExpressionGrammarParser.MathAtomContext context)
    {
        var identifierNode = context.IDENTIFIER();
        if (identifierNode != null)
        {
            variables.TryGetValue(identifierNode.GetText(), out var number);
            return new VisitResult(number);
        }
        
        var numberNode = context.NUMBER();
        if (numberNode != null)
        {
            return new VisitResult(double.Parse(numberNode.GetText(), CultureInfo.InvariantCulture));
        }
       
        // LPAREN mathExpression RPAREN 
        return Visit(context.mathExpression());
    }
}

public class VisitResult
{
    public VisitResult(bool logicValue)
    {
        LogicValue = logicValue;
    }

    public VisitResult(double doubleValue)
    {
        DoubleValue = doubleValue;
    }

    public double DoubleValue { get; set; }
    
    public bool LogicValue { get; set; }
}