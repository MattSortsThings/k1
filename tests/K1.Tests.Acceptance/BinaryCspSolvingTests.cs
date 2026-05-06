using K1.Modelling;
using K1.Problems.Elements;
using K1.Problems.NQueens;
using K1.Problems.Sudoku;
using K1.Solving;
using K1.Tests.Acceptance.Utils;

namespace K1.Tests.Acceptance;

public sealed class BinaryCspSolvingTests : AcceptanceTests
{
    [Test]
    [CombinedDataSources]
    public async Task Solver_finds_solution_for_solvable_NQueens_binary_CSP_using_search_algorithm(
        [MethodDataSource(nameof(TraversalStrategies))] TraversalStrategy traversalStrategy,
        [MethodDataSource(nameof(OrderingStrategies))] OrderingStrategy orderingStrategy,
        [MethodDataSource(nameof(SolvableNQueensProblems))] NQueensProblem problem
    )
    {
        // Arrange
        BinaryCspSolver<int, Square> solver = BinaryCspSolver<int, Square>.WithStepDelay(TimeSpan.Zero);
        BinaryCsp<int, Square> binaryCsp = problem.AsBinaryCsp();
        SearchAlgorithm algorithm = new(traversalStrategy, orderingStrategy);

        // Assert
        await Assert.That(binaryCsp.Variables).IsEqualTo(problem.Queens);
        await Assert.That(binaryCsp.ConstraintDensity).IsCloseTo(1, 0.001D);
        await Assert
            .That(algorithm)
            .HasProperty(searchAlgorithm => searchAlgorithm.TraversalStrategy, traversalStrategy)
            .And.HasProperty(searchAlgorithm => searchAlgorithm.OrderingStrategy, orderingStrategy);
        await Assert.That(solver).IsNotNull();
    }

    [Test]
    [CombinedDataSources]
    public async Task Solver_finds_no_solution_for_impossible_NQueens_binary_CSP_using_search_algorithm(
        [MethodDataSource(nameof(TraversalStrategies))] TraversalStrategy traversalStrategy,
        [MethodDataSource(nameof(OrderingStrategies))] OrderingStrategy orderingStrategy,
        [MethodDataSource(nameof(ImpossibleNQueensProblems))] NQueensProblem problem
    )
    {
        // Arrange
        BinaryCspSolver<int, Square> solver = BinaryCspSolver<int, Square>.WithStepDelay(TimeSpan.Zero);
        BinaryCsp<int, Square> binaryCsp = problem.AsBinaryCsp();
        SearchAlgorithm algorithm = new(traversalStrategy, orderingStrategy);

        // Assert
        await Assert.That(binaryCsp.Variables).IsEqualTo(problem.Queens);
        await Assert.That(binaryCsp.ConstraintDensity).IsCloseTo(1, 0.001D);
        await Assert
            .That(algorithm)
            .HasProperty(searchAlgorithm => searchAlgorithm.TraversalStrategy, traversalStrategy)
            .And.HasProperty(searchAlgorithm => searchAlgorithm.OrderingStrategy, orderingStrategy);
        await Assert.That(solver).IsNotNull();
    }

    [Test]
    [CombinedDataSources]
    public async Task Solver_finds_solution_for_solvable_Sudoku_binary_CSP_using_search_algorithm(
        [MethodDataSource(nameof(TraversalStrategies))] TraversalStrategy traversalStrategy,
        [MethodDataSource(nameof(OrderingStrategies))] OrderingStrategy orderingStrategy,
        [MethodDataSource(nameof(SolvableSudokuProblems))] SudokuProblem problem
    )
    {
        // Arrange
        BinaryCspSolver<Square, int> solver = BinaryCspSolver<Square, int>.WithStepDelay(TimeSpan.Zero);
        BinaryCsp<Square, int> binaryCsp = problem.AsBinaryCsp();
        SearchAlgorithm algorithm = new(traversalStrategy, orderingStrategy);

        // Assert
        await Assert.That(binaryCsp.Variables).IsEqualTo(81 - problem.Hints.Count);
        await Assert.That(binaryCsp.ConstraintDensity).IsNotZero();
        await Assert
            .That(algorithm)
            .HasProperty(searchAlgorithm => searchAlgorithm.TraversalStrategy, traversalStrategy)
            .And.HasProperty(searchAlgorithm => searchAlgorithm.OrderingStrategy, orderingStrategy);
        await Assert.That(solver).IsNotNull();
    }

    public static IEnumerable<TraversalStrategy> TraversalStrategies() => Enum.GetValues<TraversalStrategy>();

    public static IEnumerable<OrderingStrategy> OrderingStrategies() => Enum.GetValues<OrderingStrategy>();

    public static IEnumerable<Func<NQueensProblem>> SolvableNQueensProblems()
    {
        yield return () => NQueensProblem.FromN(4);
        yield return () => NQueensProblem.FromN(5);
        yield return () => NQueensProblem.FromN(6);
        yield return () => NQueensProblem.FromN(7);
        yield return () => NQueensProblem.FromN(8);
        yield return () => NQueensProblem.FromN(9);
        yield return () => NQueensProblem.FromN(10);
        yield return () => NQueensProblem.FromN(11);
        yield return () => NQueensProblem.FromN(12);
        yield return () => NQueensProblem.FromN(13);
        yield return () => NQueensProblem.FromN(14);
        yield return () => NQueensProblem.FromN(15);
        yield return () => NQueensProblem.FromN(16);
        yield return () => NQueensProblem.FromN(17);
        yield return () => NQueensProblem.FromN(18);
        yield return () => NQueensProblem.FromN(19);
        yield return () => NQueensProblem.FromN(20);
    }

    public static IEnumerable<Func<NQueensProblem>> ImpossibleNQueensProblems()
    {
        yield return () => NQueensProblem.FromN(2);
        yield return () => NQueensProblem.FromN(3);
    }

    public static IEnumerable<Func<SudokuProblem>> SolvableSudokuProblems()
    {
        yield return () =>
            SudokuProblem.Parse(
                """
                _ 2 3 4 5 6 _ _ _
                _ 5 6 _ 8 9 1 2 3
                7 8 _ _ _ _ _ _ 6
                _ 3 4 _ 6 7 _ _ _
                _ _ _ 8 9 1 _ _ _
                8 9 1 _ 3 4 _ _ _
                _ 4 5 _ _ _ 9 1 2
                _ 7 _ 9 _ _ _ 4 5
                9 1 2 _ _ _ 6 7 8
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ 2 _ _ 5 6 _ _ _
                _ 5 6 _ 8 9 _ 2 3
                7 8 _ _ _ _ _ _ 6
                _ 3 4 _ 6 7 _ _ _
                _ _ _ 8 _ 1 _ _ _
                8 _ 1 _ 3 4 _ _ _
                _ 4 5 _ _ _ 9 1 2
                _ 7 _ 9 _ _ _ 4 5
                _ _ 2 _ _ _ _ _ 8
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                1 _ 2 5 _ _ _ 4 3
                4 7 _ 1 _ 2 _ 5 _
                _ _ _ _ _ _ 2 9 1
                9 _ 3 4 _ _ 5 8 6
                6 _ _ _ _ 5 _ 2 _
                _ 5 _ _ _ _ 1 7 _
                3 _ 5 8 _ _ 9 6 2
                _ 2 6 3 _ 9 8 _ 5
                8 _ _ _ 5 _ _ _ _
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ _ 2 5 _ _ _ 4 3
                4 7 _ 1 _ 2 _ _ _
                _ _ _ _ _ _ 2 9 1
                9 _ 3 4 _ _ 5 _ 6
                6 _ _ _ _ 5 _ 2 _
                _ 5 _ _ _ _ 1 7 _
                3 _ 5 8 _ _ 9 6 2
                _ 2 6 3 _ 9 8 _ 5
                _ _ _ _ 5 _ _ _ _
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ 2 _ 3 4 _ 8 _ 1
                4 _ 3 _ _ 1 _ 5 _
                8 _ 1 7 _ 6 3 4 9
                _ _ 5 _ _ 9 1 _ 2
                2 1 _ _ _ 8 _ 6 4
                _ _ _ _ 6 _ 5 _ _
                5 6 _ 9 1 3 4 _ _
                _ 4 2 6 _ _ _ 1 _
                1 _ 7 _ _ _ 6 _ 3
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ 9 3 4 2 7 5 _ _
                _ _ 7 _ 1 5 3 _ _
                _ 2 4 6 8 _ _ _ 7
                3 _ _ 7 6 _ 2 1 9
                6 _ _ _ _ _ _ _ _
                _ _ _ 1 3 _ _ _ _
                4 _ 5 8 _ _ 9 2 6
                _ _ 1 2 _ 6 _ 7 3
                _ 7 _ _ _ 9 8 _ _
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ _ 6 _ _ _ 8 _ 2
                7 _ _ 4 2 8 _ 9 6
                2 1 _ _ 3 _ 7 _ 5
                _ 3 1 _ _ _ 9 8 _
                _ _ _ 1 _ _ _ _ 7
                8 2 _ 9 5 _ _ _ 3
                3 _ _ _ _ 2 _ 6 _
                _ 8 5 _ 7 6 _ _ _
                9 _ 2 5 _ 1 3 _ 8
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ 4 9 _ 3 1 _ _ _
                _ 3 _ _ 7 _ 9 8 _
                8 _ _ _ 4 _ _ _ _
                _ _ 6 1 5 7 8 _ 2
                _ 5 _ 2 8 4 1 _ 6
                _ 1 _ _ _ _ _ _ 5
                _ 8 5 7 _ 3 4 _ 9
                _ _ 2 _ _ 5 3 7 _
                _ _ 4 _ 2 _ 5 _ 1
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                8 3 _ _ _ _ 1 _ _
                _ _ _ _ _ 2 3 _ _
                1 _ _ _ 5 _ _ _ 4
                9 8 _ 1 _ 5 _ 7 2
                2 5 7 9 _ _ _ 3 1
                6 1 3 7 2 8 _ 4 _
                4 2 _ 5 _ 1 _ _ 3
                _ 7 8 _ _ 9 _ _ 5
                _ 6 _ 4 _ _ _ _ _
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                _ 6 _ 3 _ _ 4 1 _
                1 8 5 _ 2 _ 7 _ 3
                _ _ _ 5 _ _ 9 2 8
                _ 9 6 8 _ 2 _ 5 7
                2 1 _ _ 4 _ 3 _ _
                _ 5 _ _ _ 6 _ 8 4
                5 _ _ _ _ 4 6 _ _
                _ _ _ 6 1 3 5 4 _
                _ _ 9 _ _ 7 _ _ _
                """
            );

        yield return () =>
            SudokuProblem.Parse(
                """
                9 6 2 3 _ _ 4 1 5
                1 8 5 _ 2 _ 7 _ 3
                _ _ _ 5 _ _ 9 2 8
                _ 9 6 8 _ 2 _ 5 7
                _ 1 _ _ 4 _ 3 _ _
                _ 5 _ _ _ 6 _ 8 4
                5 _ _ _ _ 4 6 _ _
                _ _ _ 6 1 _ 5 4 _
                _ _ 9 _ _ 7 _ _ _
                """
            );
    }
}
