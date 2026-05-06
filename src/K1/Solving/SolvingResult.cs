using K1.Modelling;

namespace K1.Solving;

public sealed record SolvingResult<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    public required SearchAlgorithm SearchAlgorithm { get; init; }

    public required IReadOnlyList<Assignment<V, D>> Assignments { get; init; }

    public required int InitializingSteps { get; init; }

    public required int AdvancingSteps { get; init; }

    public required int BacktrackingSteps { get; init; }
}
