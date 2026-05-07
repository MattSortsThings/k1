using System.Diagnostics;
using K1.Modelling;
using K1.Solving.Internals.Ordering;
using K1.Solving.Internals.Search;
using K1.Solving.Internals.Traversal;

namespace K1.Solving;

public sealed class BinaryCspSolver<V, D> : IBinaryCspSolver<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    private readonly ISearchSpace<V, D> _searchSpace;
    private readonly TimeSpan _stepDelay;
    private int _assigningSteps;
    private int _backtrackingSteps;
    private int _initializingSteps;
    private IOrderingStrategy _orderingStrategy;
    private SearchStatus _searchStatus;
    private ITraversalStrategy _traversalStrategy;

    private BinaryCspSolver(
        ISearchSpace<V, D> searchSpace,
        TimeSpan stepDelay,
        ITraversalStrategy traversalStrategy,
        IOrderingStrategy orderingStrategy
    )
    {
        _searchSpace = searchSpace;
        _stepDelay = stepDelay;
        _traversalStrategy = traversalStrategy;
        _orderingStrategy = orderingStrategy;
    }

    public async Task<SolvingResult<V, D>> SolveAsync(
        IReadOnlyBinaryCsp<V, D> binaryCsp,
        SearchAlgorithm searchAlgorithm,
        CancellationToken cancellationToken = default
    )
    {
        ThrowIfNullOrEmpty(binaryCsp);
        ConfigureStrategies(searchAlgorithm);
        Setup(binaryCsp);

        try
        {
            return await SearchAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            Teardown();
        }
    }

    public static BinaryCspSolver<V, D> WithStepDelay(TimeSpan stepDelay)
    {
        ISearchSpace<V, D> searchSpace = null!;
        ITraversalStrategy traversalStrategy = null!;
        IOrderingStrategy orderingStrategy = null!;

        return new BinaryCspSolver<V, D>(searchSpace, stepDelay, traversalStrategy, orderingStrategy);
    }

    private void ConfigureStrategies(SearchAlgorithm algorithm)
    {
        _traversalStrategy = algorithm.TraversalStrategy.CreateStrategy();
        _orderingStrategy = algorithm.OrderingStrategy.CreateStrategy();
    }

    private void Setup(IReadOnlyBinaryCsp<V, D> binaryCsp)
    {
        _searchSpace.Populate(binaryCsp);
        _searchStatus = SearchStatus.Initializing;
    }

    private async Task<SolvingResult<V, D>> SearchAsync(CancellationToken cancellationToken)
    {
        SolvingResult<V, D>? result = null;

        while (result is null)
        {
            Report();
            await Task.Delay(_stepDelay, cancellationToken).ConfigureAwait(false);
            switch (_searchStatus)
            {
                case SearchStatus.Assigning:
                    ExecuteAssigningStep();
                    _assigningSteps++;

                    break;
                case SearchStatus.Backtracking:
                    ExecuteBacktrackingStep();
                    _backtrackingSteps++;

                    break;
                case SearchStatus.Initializing:
                    ExecuteInitializingStep();
                    _initializingSteps++;

                    break;
                case SearchStatus.Solved:
                case SearchStatus.NoSolution:
                    result = CreateSolvingResult();

                    break;
                case SearchStatus.None:
                default:
                    throw new UnreachableException("Unreachable state.");
            }
        }

        return result;
    }

    private void Report()
    {
        int totalSteps = _initializingSteps + _assigningSteps + _backtrackingSteps;

        Console.WriteLine(
            $"{totalSteps}: {_searchStatus} (I={_initializingSteps}, A={_assigningSteps}, B={_backtrackingSteps})"
        );
    }

    private void ExecuteInitializingStep() { }

    private void ExecuteAssigningStep() { }

    private void ExecuteBacktrackingStep() { }

    private SolvingResult<V, D> CreateSolvingResult()
    {
        return new SolvingResult<V, D>
        {
            SearchAlgorithm = new SearchAlgorithm(_traversalStrategy.Identifier, _orderingStrategy.Identifier),
            Assignments = _searchSpace.GetMappedAssignments(),
            InitializingSteps = _initializingSteps,
            AssigningSteps = _assigningSteps,
            BacktrackingSteps = _backtrackingSteps,
        };
    }

    private void Teardown()
    {
        _searchSpace.Reset();
        _initializingSteps = 0;
        _assigningSteps = 0;
        _backtrackingSteps = 0;
        _searchStatus = SearchStatus.None;
    }

    private static void ThrowIfNullOrEmpty(IReadOnlyBinaryCsp<V, D> binaryCsp)
    {
        ArgumentNullException.ThrowIfNull(binaryCsp);

        if (binaryCsp.Variables == 0)
        {
            throw new ArgumentException("Binary CSP is empty.");
        }
    }
}
