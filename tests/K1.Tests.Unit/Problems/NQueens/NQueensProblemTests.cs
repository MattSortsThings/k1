using K1.Modelling;
using K1.Problems.Elements;
using K1.Problems.NQueens;
using K1.Tests.Unit.Utils;

namespace K1.Tests.Unit.Problems.NQueens;

public sealed class NQueensProblemTests : UnitTests
{
    [Test]
    [Arguments(2)]
    [Arguments(5)]
    public async Task FromN_returns_NQueensProblem_with_specified_queens_and_board(int n)
    {
        // Arrange
        Block expectedBoard = new(new Square(0, 0), new Dimensions(n, n));

        // Act
        NQueensProblem result = NQueensProblem.FromN(n);

        // Assert
        await Assert.That(result.Queens).IsEqualTo(n);
        await Assert.That(result.Board).IsEqualTo(expectedBoard);
    }

    [Test]
    [MethodDataSource(nameof(GetVariablesTestCases))]
    public async Task GetVariables_returns_queen_indices(GetVariablesTestCase testCase)
    {
        // Arrange
        (NQueensProblem problem, HashSet<int> expectedVariables) = testCase;

        // Assert
        await Assert.That(problem.GetVariables()).IsEquivalentTo(expectedVariables);
    }

    [Test]
    [MethodDataSource(nameof(GetDomainValuesTestCases))]
    public async Task GetDomainValues_returns_all_squares_in_column_matching_queen_index(
        GetDomainValuesTestCase testCase
    )
    {
        // Arrange
        (NQueensProblem problem, int queenIndex, Square[] expectedDomainValues) = testCase;

        // Assert
        await Assert.That(problem.GetDomainValues(queenIndex)).IsEquivalentTo(expectedDomainValues);
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(0, 2)]
    [Arguments(0, 3)]
    [Arguments(1, 2)]
    [Arguments(1, 3)]
    [Arguments(2, 3)]
    public async Task TryGetBinaryPredicate_always_returns_true_and_sets_predicate(int queenIndexA, int queenIndexB)
    {
        // Arrange
        NQueensProblem problem = NQueensProblem.FromN(4);

        // Act
        bool predicateSet = problem.TryGetBinaryPredicate(
            queenIndexA,
            queenIndexB,
            out Func<Square, Square, bool>? predicate
        );

        // Assert
        await Assert.That(predicateSet).IsTrue();
        await Assert.That(predicate).IsNotNull();
    }

    [Test]
    [MethodDataSource(nameof(GetValidSolutionHappyPathTestCases))]
    public async Task ValidSolution_returns_successful_result_given_valid_solution(
        ValidSolutionHappyPathTestCase testCase
    )
    {
        // Arrange
        (NQueensProblem problem, Assignment<int, Square>[] assignments) = testCase;

        // Act
        ValidationResult result = problem.ValidSolution(assignments);

        // Assert
        await Assert.That(result.Successful).IsTrue();
        await Assert.That(result.ErrorMessage).IsNull();
    }

    [Test]
    [MethodDataSource(nameof(GetValidSolutionSadPathTestCases))]
    public async Task ValidSolution_returns_unsuccessful_result_given_invalid_solution(
        ValidSolutionSadPathTestCase testCase
    )
    {
        // Arrange
        (NQueensProblem problem, Assignment<int, Square>[] assignments, string expectedErrorMessage) = testCase;

        // Act
        ValidationResult result = problem.ValidSolution(assignments);

        // Assert
        await Assert.That(result.Successful).IsFalse();
        await Assert.That(result.ErrorMessage).IsEqualTo(expectedErrorMessage);
    }

    public static IEnumerable<Func<GetVariablesTestCase>> GetVariablesTestCases()
    {
        yield return () => new GetVariablesTestCase(NQueensProblem.FromN(2), [0, 1]);
        yield return () => new GetVariablesTestCase(NQueensProblem.FromN(5), [0, 1, 2, 3, 4]);
    }

    public static IEnumerable<Func<ValidSolutionHappyPathTestCase>> GetValidSolutionHappyPathTestCases()
    {
        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(4),
                [
                    new Assignment<int, Square>(0, new Square(0, 1)),
                    new Assignment<int, Square>(1, new Square(1, 3)),
                    new Assignment<int, Square>(2, new Square(2, 0)),
                    new Assignment<int, Square>(3, new Square(3, 2)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 4)),
                    new Assignment<int, Square>(1, new Square(1, 1)),
                    new Assignment<int, Square>(2, new Square(2, 3)),
                    new Assignment<int, Square>(3, new Square(3, 0)),
                    new Assignment<int, Square>(4, new Square(4, 2)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 2)),
                    new Assignment<int, Square>(1, new Square(1, 0)),
                    new Assignment<int, Square>(2, new Square(2, 3)),
                    new Assignment<int, Square>(3, new Square(3, 1)),
                    new Assignment<int, Square>(4, new Square(4, 4)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 0)),
                    new Assignment<int, Square>(1, new Square(1, 3)),
                    new Assignment<int, Square>(2, new Square(2, 1)),
                    new Assignment<int, Square>(3, new Square(3, 4)),
                    new Assignment<int, Square>(4, new Square(4, 2)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 0)),
                    new Assignment<int, Square>(1, new Square(1, 2)),
                    new Assignment<int, Square>(2, new Square(2, 4)),
                    new Assignment<int, Square>(3, new Square(3, 1)),
                    new Assignment<int, Square>(4, new Square(4, 3)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 2)),
                    new Assignment<int, Square>(1, new Square(1, 4)),
                    new Assignment<int, Square>(2, new Square(2, 1)),
                    new Assignment<int, Square>(3, new Square(3, 3)),
                    new Assignment<int, Square>(4, new Square(4, 0)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 1)),
                    new Assignment<int, Square>(1, new Square(1, 3)),
                    new Assignment<int, Square>(2, new Square(2, 0)),
                    new Assignment<int, Square>(3, new Square(3, 2)),
                    new Assignment<int, Square>(4, new Square(4, 4)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 4)),
                    new Assignment<int, Square>(1, new Square(1, 2)),
                    new Assignment<int, Square>(2, new Square(2, 0)),
                    new Assignment<int, Square>(3, new Square(3, 3)),
                    new Assignment<int, Square>(4, new Square(4, 1)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 3)),
                    new Assignment<int, Square>(1, new Square(1, 1)),
                    new Assignment<int, Square>(2, new Square(2, 4)),
                    new Assignment<int, Square>(3, new Square(3, 2)),
                    new Assignment<int, Square>(4, new Square(4, 0)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 3)),
                    new Assignment<int, Square>(1, new Square(1, 0)),
                    new Assignment<int, Square>(2, new Square(2, 2)),
                    new Assignment<int, Square>(3, new Square(3, 4)),
                    new Assignment<int, Square>(4, new Square(4, 1)),
                ]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                NQueensProblem.FromN(5),
                [
                    new Assignment<int, Square>(0, new Square(0, 1)),
                    new Assignment<int, Square>(1, new Square(1, 4)),
                    new Assignment<int, Square>(2, new Square(2, 2)),
                    new Assignment<int, Square>(3, new Square(3, 0)),
                    new Assignment<int, Square>(4, new Square(4, 3)),
                ]
            );
    }

    public static IEnumerable<Func<GetDomainValuesTestCase>> GetDomainValuesTestCases()
    {
        yield return () =>
            new GetDomainValuesTestCase(
                NQueensProblem.FromN(3),
                0,
                [new Square(0, 0), new Square(0, 1), new Square(0, 2)]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                NQueensProblem.FromN(3),
                1,
                [new Square(1, 0), new Square(1, 1), new Square(1, 2)]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                NQueensProblem.FromN(3),
                2,
                [new Square(2, 0), new Square(2, 1), new Square(2, 2)]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                NQueensProblem.FromN(5),
                0,
                [new Square(0, 0), new Square(0, 1), new Square(0, 2), new Square(0, 3), new Square(0, 4)]
            );
    }

    public static IEnumerable<Func<ValidSolutionSadPathTestCase>> GetValidSolutionSadPathTestCases()
    {
        yield return () =>
            new ValidSolutionSadPathTestCase(NQueensProblem.FromN(2), [], "Solution size is 0, should be 2.");

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0))],
                "Solution size is 1, should be 2."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [
                    new Assignment<int, Square>(0, new Square(0, 0)),
                    new Assignment<int, Square>(1, new Square(1, 0)),
                    new Assignment<int, Square>(2, new Square(2, 0)),
                ],
                "Solution size is 3, should be 2."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(1, 2))],
                "Square (1,2) is outside board."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(2, 1))],
                "Square (2,1) is outside board."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(0, 0))],
                "Squares (0,0) and (0,0) capture each other."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(0, 1))],
                "Squares (0,0) and (0,1) capture each other."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(1, 0))],
                "Squares (0,0) and (1,0) capture each other."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                NQueensProblem.FromN(2),
                [new Assignment<int, Square>(0, new Square(0, 0)), new Assignment<int, Square>(1, new Square(1, 1))],
                "Squares (0,0) and (1,1) capture each other."
            );
    }

    public sealed record GetVariablesTestCase(NQueensProblem Problem, HashSet<int> ExpectedVariables);

    public sealed record GetDomainValuesTestCase(NQueensProblem Problem, int QueenIndex, Square[] ExpectedDomainValues);

    public sealed record ValidSolutionHappyPathTestCase(NQueensProblem Problem, Assignment<int, Square>[] Assignments);

    public sealed record ValidSolutionSadPathTestCase(
        NQueensProblem Problem,
        Assignment<int, Square>[] Assignments,
        string ExpectedErrorMessage
    );
}
