using System.Diagnostics.CodeAnalysis;
using K1.Modelling;
using K1.Problems.Elements;

namespace K1.Problems.NQueens;

public sealed class NQueensProblem : IBinaryCspModel<int, Square>
{
    private NQueensProblem(int queens, Rectangle board)
    {
        Queens = queens;
        Board = board;
    }

    public int Queens { get; }

    public Rectangle Board { get; }

    public IReadOnlySet<int> GetVariables() => new HashSet<int>(Enumerable.Range(0, Queens));

    public IEnumerable<Square> GetDomainValues(int v)
    {
        for (int row = 0; row < Queens; row++)
        {
            yield return new Square(v, row);
        }
    }

    public bool TryGetBinaryPredicate(
        int v1,
        int v2,
        [NotNullWhen(true)] out Func<Square, Square, bool>? binaryPredicate
    )
    {
        binaryPredicate = DoNotCapture;

        return true;
    }

    public ValidationResult ValidSolution(IReadOnlyList<Assignment<int, Square>> assignments)
    {
        ValidationResult result = ValidationResult.Success;
        Square[] solution = ParseSolution(assignments);

        if (solution.Length != Queens)
        {
            result = ValidationResult.Failure($"Solution size is {solution.Length}, should be {Queens}.");
        }
        else if (CheckForSquareOutsideBoard(solution, out string? errorMessage1))
        {
            result = ValidationResult.Failure(errorMessage1);
        }
        else if (CheckForCapturingSquares(solution, out string? errorMessage2))
        {
            result = ValidationResult.Failure(errorMessage2);
        }

        return result;
    }

    public static NQueensProblem FromN(int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 2);

        Rectangle board = new(new Square(0, 0), new Dimensions(n, n));

        return new NQueensProblem(n, board);
    }

    private static bool DoNotCapture(Square s1, Square s2) => !s1.Captures(s2);

    private static Square[] ParseSolution(IReadOnlyList<Assignment<int, Square>> assignments) =>
        assignments.Select(assignment => assignment.DomainValue).ToArray();

    private bool CheckForSquareOutsideBoard(Square[] squares, [NotNullWhen(true)] out string? errorMessage)
    {
        foreach (Square square in squares)
        {
            if (Board.Contains(square))
            {
                continue;
            }

            errorMessage = $"Square {square} is outside board.";

            return true;
        }

        errorMessage = null;

        return false;
    }

    private static bool CheckForCapturingSquares(Square[] squares, [NotNullWhen(true)] out string? errorMessage)
    {
        for (int i = 0; i < squares.Length; i++)
        {
            Square squareAtI = squares[i];

            for (int j = i + 1; j < squares.Length; j++)
            {
                Square squareAtJ = squares[j];

                if (!squareAtI.Captures(squareAtJ))
                {
                    continue;
                }

                errorMessage = $"Squares {squareAtI} and {squareAtJ} capture each other.";

                return true;
            }
        }

        errorMessage = null;

        return false;
    }
}
