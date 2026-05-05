using System.Diagnostics.CodeAnalysis;
using K1.Modelling;

namespace K1.Tests.Unit.Utils;

public sealed class AllDifferentProblem : Dictionary<string, char[]>, IBinaryCspModel<string, char>
{
    public IReadOnlySet<string> GetVariables() => Keys.ToHashSet();

    public IEnumerable<char> GetDomainValues(string v) => this[v];

    public bool TryGetBinaryPredicate(
        string v1,
        string v2,
        [NotNullWhen(true)] out Func<char, char, bool>? binaryPredicate
    )
    {
        binaryPredicate = Unequal;

        return true;
    }

    private static bool Unequal(char c1, char c2) => c1 != c2;
}
