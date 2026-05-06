using K1.Modelling;

namespace K1.Solving;

public sealed class BinaryCspSolver<V, D> : IBinaryCspSolver<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    private readonly TimeSpan _stepDelay;

    private BinaryCspSolver(TimeSpan stepDelay)
    {
        _stepDelay = stepDelay;
    }

    public async Task<SolvingResult<V, D>> SolveAsync(
        IReadOnlyBinaryCsp<V, D> binaryCsp,
        SearchAlgorithm searchAlgorithm,
        CancellationToken cancellationToken = default
    ) => throw new NotImplementedException();

    public static BinaryCspSolver<V, D> WithStepDelay(TimeSpan stepDelay) => new(stepDelay);
}
