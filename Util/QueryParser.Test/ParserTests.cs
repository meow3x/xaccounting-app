using System.Runtime.InteropServices;
using System.Text.Json;

namespace QueryParser.Test;

[TestClass]
public sealed class ParserTests
{
    private class _DummyEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0; 
    }

    [TestMethod]
    public void Test_CanParseQuery()
    {
        var parser = new SearchQueryParser();

        string q = "accountType:[32, 45, 1999] AND name:Cash in bank AND accountId:100";

        var opts = new JsonSerializerOptions { WriteIndented = true };
        //var result = parser.Parse(q);

        //Console.WriteLine(JsonSerializer.Serialize(result, options: opts));

        //Assert.AreEqual("accountType", result[0].Field);
        //CollectionAssert.AreEquivalent(new string[] { "32", "45", "1999" }, (string[])result[0].Value);
        
    }

    
    [TestMethod]
    public void Test_IntegerPredicate()
    {
        var s = new SearchTerm("age", Operator.In, new int[] { 23, 24, 25, 26 });
        var m = new IntegerVariantProcessor<_DummyEntity>();
        var p = m.CreatePredicate(e => e.Age, s);

        var instance = new _DummyEntity() { Age = 23 };
        var instance2 = new _DummyEntity() { Age = 26 };
        Assert.IsNotNull(p);

        var fn = p.Compile();

        Assert.IsTrue(fn(new _DummyEntity { Age = 23 }));
        Assert.IsFalse(fn(new _DummyEntity {  Age = 15 }));
    }

    [TestMethod]
    public void Test_Predicate_String_Contains()
    {
        var s = new SearchTerm("name", Operator.Contains, "DOE");

        var m = new StringVariantProcessor<_DummyEntity>();
        var p = m.CreatePredicate(e => e.Name, s);

        Assert.IsNotNull(p);

        Console.WriteLine(p.ToString());
        var fn = p.Compile();

        Assert.IsTrue(fn(new _DummyEntity { Name = "John Doe Jr." }));
        Assert.IsTrue(fn(new _DummyEntity { Name = "John Doerothy Jr." }));
        Assert.IsFalse(fn(new _DummyEntity { Name = "John Jr." }));
    }
}
