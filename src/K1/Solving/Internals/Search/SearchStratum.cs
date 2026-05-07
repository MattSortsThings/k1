using K1.Modelling;

namespace K1.Solving.Internals.Search;

public sealed record SearchStratum : IAssignment
{
    public int VariableIndex { get; init; }

    public int DomainValueIndex { get; set; }
}
