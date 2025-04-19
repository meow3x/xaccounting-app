using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace QueryParser;

[JsonConverter(typeof(JsonStringEnumConverter<Operator>))]
public enum Operator
{
    Equals, // Exact match
    Contains, // Contains (string only)
    In
}

public record SearchTerm(
    string Field,
    Operator Operator,
    object Value)
{
    public T ValueAs<T>() => (T)Value;
}

/*
 * GET {{Api_HostAddress}}/api/chartofaccounts?pageId=0&count=30&\
    q=accountType:[3,4] AND name:Cash in bank AND accountId:100
 */
public class SearchQueryParser
{
    public static IEnumerable<SearchTerm> Parse(string q)
    {
        // Syntax:
        // <searchQuery> = <term> [ AND <term> ]...
        // <searchTerm> = <column>:<searchCriteria> 
        // <searchCriteria> = <literal>|<list>
        // <list> = "[" <literal> [, <literal> ]... "]" 

        // For now lets use this simple approach, convert to recursive parsing later...

        var result = new List<SearchTerm>();
        var splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        var rawTerms = q.Split(["AND"], splitOptions);
        
        foreach (var term in rawTerms)
        {
            var elements = term.Split(':', splitOptions);
            
            // if list, then parse
            if (elements[1][0] == '[')
            {
                if (elements[1].Last() != ']') { throw new ArgumentOutOfRangeException(nameof(q), "Malformed syntax"); }
                var values = elements[1][1..^1].Split(',', splitOptions); // []
                result.Add(
                    new SearchTerm(elements[0],
                    Operator.In,
                    values.Select(int.Parse).ToArray()));
            }
            else
            {
                result.Add(new SearchTerm(elements[0], Operator.Contains, elements[1]));
            }
        }

        return result;
    }
}
