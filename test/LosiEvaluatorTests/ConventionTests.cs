using Losi;

namespace LosiEvaluatorTests;

public class ConventionTests
{
    [Test]
    public void PositiveIsTrue()
    {
        var e = new Evaluator();
        Assert.True(e.Eval("1"));
        Assert.True(e.Eval("100"));
        Assert.True(e.Eval("0.0123"));
    }
    
    [Test]
    public void ZeroOrNegativeIsFalse()
    {
        var e = new Evaluator();
        Assert.False(e.Eval("0"));
        Assert.False(e.Eval("-0"));
        Assert.False(e.Eval("-0.0123"));
        Assert.False(e.Eval("-321.123"));
    }
    
    [Test]
    public void MissingVariableIsZero()
    {
        var e = new Evaluator();
        Assert.False(e.Eval("a"));
        Assert.True(e.Eval("a = 0"));
        e.Set("aa", 1);
        Assert.False(e.Eval("a"));
        Assert.True(e.Eval("a = 0"));
        
        Assert.True(e.Eval("a + b + c - 1 = -1"));
    }
}