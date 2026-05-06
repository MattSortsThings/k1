using K1.Modelling;
using K1.Problems.Elements;
using K1.Problems.Sudoku;
using K1.Tests.Unit.Utils;

namespace K1.Tests.Unit.Problems.Sudoku;

public sealed class SudokuProblemTests : UnitTests
{
    [Test]
    [MethodDataSource(nameof(GetVariablesTestCases))]
    public async Task GetVariables_returns_all_empty_squares(GetVariablesTestCase testCase)
    {
        // Arrange
        (SudokuProblem problem, HashSet<Square> expectedSquares) = testCase;

        // Act
        IReadOnlySet<Square> result = problem.GetVariables();

        // Assert
        await Assert.That(result).IsEquivalentTo(expectedSquares);
    }

    [Test]
    [MethodDataSource(nameof(GetDomainValuesTestCases))]
    public async Task GetDomainValues_returns_all_numbers_from_1_to_9_except_obstructing_hints(
        GetDomainValuesTestCase testCase
    )
    {
        // Arrange
        (SudokuProblem problem, Square square, int[] expectedNumbers) = testCase;

        // Act
        IEnumerable<int> result = problem.GetDomainValues(square);

        // Assert
        await Assert.That(result).IsEquivalentTo(expectedNumbers);
    }

    [Test]
    public async Task TryGetBinaryPredicate_returns_true_and_sets_predicate_when_squares_share_column()
    {
        // Arrange
        SudokuProblem problem = SudokuProblem.Parse(
            """
            _ 2 3 4 5 6 7 8 9
            4 5 6 7 8 9 1 2 3
            7 8 9 1 2 3 4 5 6
            2 3 4 5 6 7 8 9 1
            5 6 7 8 9 1 2 3 4
            8 9 1 2 3 4 5 6 4
            3 4 5 6 7 8 9 1 2
            _ 7 8 9 1 2 3 4 5
            9 1 2 3 4 5 6 7 8
            """
        );

        Square square1 = new(0, 0);
        Square square2 = new(0, 7);

        // Act
        bool predicateSet = problem.TryGetBinaryPredicate(square1, square2, out Func<int, int, bool>? predicate);

        // Assert
        await Assert.That(predicateSet).IsTrue();
        await Assert.That(predicate).IsNotNull();
    }

    [Test]
    public async Task TryGetBinaryPredicate_returns_true_and_sets_predicate_when_squares_share_row()
    {
        // Arrange
        SudokuProblem problem = SudokuProblem.Parse(
            """
            _ 2 3 4 5 6 7 _ 9
            4 5 6 7 8 9 1 2 3
            7 8 9 1 2 3 4 5 6
            2 3 4 5 6 7 8 9 1
            5 6 7 8 9 1 2 3 4
            8 9 1 2 3 4 5 6 4
            3 4 5 6 7 8 9 1 2
            6 7 8 9 1 2 3 4 5
            9 1 2 3 4 5 6 7 8
            """
        );

        Square square1 = new(0, 0);
        Square square2 = new(7, 0);

        // Act
        bool predicateSet = problem.TryGetBinaryPredicate(square1, square2, out Func<int, int, bool>? predicate);

        // Assert
        await Assert.That(predicateSet).IsTrue();
        await Assert.That(predicate).IsNotNull();
    }

    [Test]
    public async Task TryGetBinaryPredicate_returns_true_and_sets_predicate_when_squares_share_sector()
    {
        // Arrange
        SudokuProblem problem = SudokuProblem.Parse(
            """
            _ 2 3 4 5 6 7 8 9
            4 5 6 7 8 9 1 2 3
            7 _ 9 1 2 3 4 5 6
            2 3 4 5 6 7 8 9 1
            5 6 7 8 9 1 2 3 4
            8 9 1 2 3 4 5 6 4
            3 4 5 6 7 8 9 1 2
            6 7 8 9 1 2 3 4 5
            9 1 2 3 4 5 6 7 8
            """
        );

        Square square1 = new(0, 0);
        Square square2 = new(1, 2);

        // Act
        bool predicateSet = problem.TryGetBinaryPredicate(square1, square2, out Func<int, int, bool>? predicate);

        // Assert
        await Assert.That(predicateSet).IsTrue();
        await Assert.That(predicate).IsNotNull();
    }

    [Test]
    public async Task TryGetBinaryPredicate_returns_false_when_squares_do_not_share_column_row_or_sector()
    {
        // Arrange
        SudokuProblem problem = SudokuProblem.Parse(
            """
            _ 2 3 4 5 6 7 8 9
            4 5 6 _ 8 9 1 2 3
            7 8 9 1 2 3 4 5 6
            2 3 4 5 6 7 8 9 1
            5 6 7 8 9 1 2 3 4
            8 9 1 2 3 4 5 6 4
            3 4 5 6 7 8 9 1 2
            6 7 8 9 1 2 3 4 5
            9 1 2 3 4 5 6 7 8
            """
        );

        Square square1 = new(0, 0);
        Square square2 = new(3, 1);

        // Act
        bool predicateSet = problem.TryGetBinaryPredicate(square1, square2, out Func<int, int, bool>? predicate);

        // Assert
        await Assert.That(predicateSet).IsFalse();
        await Assert.That(predicate).IsNull();
    }

    [Test]
    [MethodDataSource(nameof(ValidSolutionHappyPathTestCases))]
    public async Task ValidSolution_returns_successful_result_given_valid_solution(
        ValidSolutionHappyPathTestCase testCase
    )
    {
        // Arrange
        (SudokuProblem problem, Assignment<Square, int>[] assignments) = testCase;

        // Act
        ValidationResult result = problem.ValidSolution(assignments);

        // Assert
        await Assert.That(result.Successful).IsTrue();
        await Assert.That(result.ErrorMessage).IsNull();
    }

    [Test]
    [MethodDataSource(nameof(ValidSolutionSadPathTestCases))]
    public async Task ValidSolution_returns_unsuccessful_result_given_invalid_solution(
        ValidSolutionSadPathTestCase testCase
    )
    {
        // Arrange
        (SudokuProblem problem, Assignment<Square, int>[] assignments, string expectedErrorMessage) = testCase;

        // Act
        ValidationResult result = problem.ValidSolution(assignments);

        // Assert
        await Assert.That(result.Successful).IsFalse();
        await Assert.That(result.ErrorMessage).IsEqualTo(expectedErrorMessage);
    }

    public static IEnumerable<Func<GetVariablesTestCase>> GetVariablesTestCases()
    {
        yield return () =>
            new GetVariablesTestCase(
                SudokuProblem.Parse(
                    """
                    1 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 7 8
                    """
                ),
                [new Square(0, 8), new Square(2, 2)]
            );

        yield return () =>
            new GetVariablesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                [
                    new Square(0, 0),
                    new Square(0, 8),
                    new Square(1, 0),
                    new Square(2, 0),
                    new Square(2, 2),
                    new Square(7, 8),
                    new Square(8, 8),
                ]
            );
    }

    public static IEnumerable<Func<GetDomainValuesTestCase>> GetDomainValuesTestCases()
    {
        yield return () =>
            new GetDomainValuesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                new Square(0, 0),
                [1]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 6 7 8 _
                    4 5 6 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                new Square(0, 0),
                [1, 9]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 6 7 8 _
                    _ _ _ 7 8 9 1 2 3
                    _ _ _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                new Square(0, 0),
                [1, 9]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 _ 7 8 _
                    4 _ 6 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    _ 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                new Square(0, 0),
                [1, 9]
            );

        yield return () =>
            new GetDomainValuesTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ _ 4 5 _ 7 8 _
                    4 _ _ 7 8 9 1 2 3
                    7 8 _ 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    _ 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 _ _
                    """
                ),
                new Square(0, 0),
                [1, 6, 9]
            );
    }

    public static IEnumerable<Func<ValidSolutionHappyPathTestCase>> ValidSolutionHappyPathTestCases()
    {
        yield return () =>
            new ValidSolutionHappyPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 1)]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 1), new Assignment<Square, int>(new Square(1, 0), 2)]
            );

        yield return () =>
            new ValidSolutionHappyPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ _ 3 4 5 6 7 8 9
                    _ 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [
                    new Assignment<Square, int>(new Square(0, 0), 1),
                    new Assignment<Square, int>(new Square(0, 1), 4),
                    new Assignment<Square, int>(new Square(1, 0), 2),
                ]
            );
    }

    public static IEnumerable<Func<ValidSolutionSadPathTestCase>> ValidSolutionSadPathTestCases()
    {
        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 7 8
                    """
                ),
                [],
                "Solution size is 0, should be 2."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 1)],
                "Solution size is 1, should be 2."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    _ 1 2 3 4 5 6 7 8
                    """
                ),
                [
                    new Assignment<Square, int>(new Square(0, 0), 1),
                    new Assignment<Square, int>(new Square(0, 8), 9),
                    new Assignment<Square, int>(new Square(0, 1), 1),
                ],
                "Solution size is 3, should be 2."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 0)],
                "Numbered square (0,0) [0] is out of range."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 10)],
                "Numbered square (0,0) [10] is out of range."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 9), 9)],
                "Numbered square (0,9) [9] is outside grid."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(9, 0), 9)],
                "Numbered square (9,0) [9] is outside grid."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(8, 8), 9)],
                "Square (8,8) is filled twice."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 _
                    """
                ),
                [new Assignment<Square, int>(new Square(8, 8), 9), new Assignment<Square, int>(new Square(8, 8), 9)],
                "Square (8,8) is filled twice."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    4 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 2)],
                "Obstructing squares (0,0) and (0,3) have same number [2]."
            );

        yield return () =>
            new ValidSolutionSadPathTestCase(
                SudokuProblem.Parse(
                    """
                    _ 2 3 4 5 6 7 8 9
                    _ 5 6 7 8 9 1 2 3
                    7 8 9 1 2 3 4 5 6
                    2 3 4 5 6 7 8 9 1
                    5 6 7 8 9 1 2 3 4
                    8 9 1 2 3 4 5 6 7
                    3 4 5 6 7 8 9 1 2
                    6 7 8 9 1 2 3 4 5
                    9 1 2 3 4 5 6 7 8
                    """
                ),
                [new Assignment<Square, int>(new Square(0, 0), 1), new Assignment<Square, int>(new Square(0, 1), 1)],
                "Obstructing squares (0,0) and (0,1) have same number [1]."
            );
    }

    public sealed record GetVariablesTestCase(SudokuProblem Problem, HashSet<Square> ExpectedSquares);

    public sealed record GetDomainValuesTestCase(SudokuProblem Problem, Square Square, int[] ExpectedNumbers);

    public sealed record ValidSolutionHappyPathTestCase(SudokuProblem Problem, Assignment<Square, int>[] Assignments);

    public sealed record ValidSolutionSadPathTestCase(
        SudokuProblem Problem,
        Assignment<Square, int>[] Assignments,
        string ExpectedErrorMessage
    );
}
