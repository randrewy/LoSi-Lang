using Losi;

namespace LosiEvaluatorTests;

public class Tests
{
    [Test]
    [TestCase("0", false)]
    [TestCase("-0", false)]
    [TestCase("-0.001", false)]
    [TestCase("-1", false)]
    [TestCase("-321", false)]
    [TestCase("1", true)]
    [TestCase("0.0001", true)]
    [TestCase("321", true)]
    public void CastLiteralToBool(string value, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(value) == result);
    }

    [Test]
    [TestCase(0, false)]
    [TestCase(-0, false)]
    [TestCase(-0.001, false)]
    [TestCase(-1, false)]
    [TestCase(-321, false)]
    [TestCase(1, true)]
    [TestCase(0.0001, true)]
    [TestCase(321, true)]
    public void CastVariableToBool(double value, bool result)
    {
        var e = new Evaluator();
        e.Set("var", value);

        Assert.That(e.Eval("var") == result);
    }

    [Test]
    [TestCase("0 - 1", false)]
    [TestCase("-1 + 0", false)]
    [TestCase("1/-1", false)]
    [TestCase("1/(-1)", false)]
    [TestCase("-1/(-1)", true)]
    [TestCase("0 + -1", false)]
    [TestCase("0 - -1", true)]
    public void CastExpressionToBool(string value, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(value) == result);
    }

    [Test]
    [TestCase(" -1  AND  -2", false)]
    [TestCase("  1  AND  -2", false)]
    [TestCase(" -1  AND   2", false)]
    [TestCase("  1  AND   2", true)]
    [TestCase("(-1) AND  -2", false)]
    [TestCase("  1  AND (-2)", false)]
    [TestCase("(-1) AND  (2)", false)]
    [TestCase(" (1  AND   2)", true)]
    [TestCase("-1 OR -2", false)]
    [TestCase(" 1 OR -2", true)]
    [TestCase("-1 OR  2", true)]
    [TestCase(" 1 OR  2", true)]
    [TestCase("(-1) OR (-2)", false)]
    [TestCase("( 1  OR  -2)", true)]
    [TestCase("(-1) OR   2", true)]
    [TestCase(" 1   OR  (2)", true)]
    public void BooleanOperatorsTests(string expression, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(expression) == result);
    }

    [Test]
    [TestCase("NOT 1", false)]
    [TestCase("NOT 2", false)]
    [TestCase("NOT 0", true)]
    [TestCase("NOT -1", true)]
    [TestCase("NOT (1)", false)]
    [TestCase("(NOT 2)", false)]
    [TestCase("(NOT (0))", true)]
    [TestCase("NOT -(1)", true)]
    public void UnaryBooleanOperatorsTests(string expression, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(expression) == result);
    }

    [Test]
    [TestCase("-1 <  -2", false)]
    [TestCase("-1 <= -2", false)]
    [TestCase("-1 >  -2", true)]
    [TestCase("-1 >= -2", true)]
    [TestCase("-1 <   2", true)]
    [TestCase("-1 <=  2", true)]
    [TestCase("-1 >   2", false)]
    [TestCase("-1 >=  2", false)]
    [TestCase(" 1 <   2", true)]
    [TestCase(" 1 <=  2", true)]
    [TestCase(" 1 >   2", false)]
    [TestCase(" 1 >=  2", false)]
    [TestCase(" 1 <   1", false)]
    [TestCase(" 1 <=  1", true)]
    [TestCase(" 1 >   1", false)]
    [TestCase(" 1 >=  1", true)]
    public void ComparisonsTest(string expression, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(expression) == result);
    }
    
    [Test]
    [TestCase("1 AND 1 OR 0", true)]
    [TestCase("0 AND 1 OR 0", false)]
    [TestCase("1 AND 0 OR 0", false)]
    [TestCase("0 AND 0 OR 0", false)]
    [TestCase("0 AND 0 OR 1", true)]
    [TestCase("0 AND (0 OR 1)", false)]
    [TestCase("0 OR 1 AND 1", true)]
    [TestCase("0 OR 0 AND 1", false)]
    [TestCase("0 OR 1 AND 0", false)]
    [TestCase("0 OR 0 AND 0", false)]
    [TestCase("1 OR 0 AND 0", true)]
    [TestCase("(1 OR 0) AND 0", false)]
    [TestCase("0 OR 1 AND 0", false)]
    [TestCase("0 OR 1 AND NOT 0", true)]
    [TestCase("(0 OR 1 AND 0) AND (0 OR 1 AND NOT 0)", false)]
    public void BooleanExpressionTest(string expression, bool result)
    {
        var e = new Evaluator();
        Assert.That(e.Eval(expression) == result);
    }
    
    [Test]
    [TestCase("(a - b) * c > d", true)]
    [TestCase("(b + c = 7 OR NOT d < 10)", true)]
    [TestCase("(e = 1 OR (a / b) > c)", true)]
    [TestCase("(a - b) * c > d AND (b + c = 7 OR NOT d < 10) AND (e = 1 OR (a / b) > c)", true)]
    [TestCase("(x + y) * z > w", true)]
    [TestCase("(y - z = -1 OR NOT w = 2)", true)]
    [TestCase("(v = 5 OR (x / y) > z)", true)]
    [TestCase("(z * w < x OR NOT (v - w = 3)", false)]
    [TestCase("(z * w < x OR NOT (v - w != 3)", true)]
    [TestCase("(x + y) * z > w AND (y - z = -1 OR NOT w = 2) AND (v = 5 OR (x / y) > z) AND (z * w < x OR NOT (v - w = 3))", false)]
    [TestCase("(x + y) * z > w AND (y - z = -1 OR NOT w = 2) AND (v = 5 OR (x / y) > z) AND (z * w < x OR NOT (v - w != 3))", true)]
    public void ExampleTest(string expression, bool result)
    {
        var e = new Evaluator()
            .Set("a", 10)
            .Set("b", 5)
            .Set("c", 2)
            .Set("d", 8)
            .Set("e", 1)
            .Set("x", 7)
            .Set("y", 3)
            .Set("z", 4)
            .Set("w", 2)
            .Set("v", 5);
        Assert.That(e.Eval(expression) == result);
    }
}
 