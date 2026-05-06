using K1.Modelling;

namespace K1.Solving;

public interface IBinaryCspSolver<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    Task<SolvingResult<V, D>> SolveAsync(
        IReadOnlyBinaryCsp<V, D> binaryCsp,
        SearchAlgorithm searchAlgorithm,
        CancellationToken cancellationToken = default
    );
}
