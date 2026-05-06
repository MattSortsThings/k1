using System.Diagnostics.CodeAnalysis;
using K1.Modelling;
using K1.Problems.Elements;

namespace K1.Problems.Sudoku;

public sealed class SudokuProblem : IBinaryCspModel<Square, int>
{
    private SudokuProblem(Block grid, IReadOnlyList<NumberedSquare> hints)
    {
        Grid = grid;
        Hints = hints;
    }

    public Block Grid { get; }

    public IReadOnlyList<NumberedSquare> Hints { get; }

    public IReadOnlySet<Square> GetVariables()
    {
        IEnumerable<Square> allSquares = Enumerable
            .Range(0, 9)
            .SelectMany(column => Enumerable.Range(0, 9).Select(row => new Square(column, row)));

        IEnumerable<Square> filledSquares = Hints.Select(numberedSquare => numberedSquare.Square);

        return allSquares.Except(filledSquares).ToHashSet();
    }

    public IEnumerable<int> GetDomainValues(Square v)
    {
        IEnumerable<int> allNumbers = Enumerable.Range(1, 9);
        IEnumerable<int> obstructedNumbers = GetObstructedNumbers(v);

        return allNumbers.Except(obstructedNumbers);
    }

    public bool TryGetBinaryPredicate(
        Square v1,
        Square v2,
        [NotNullWhen(true)] out Func<int, int, bool>? binaryPredicate
    )
    {
        if (v1.Obstructs(v2))
        {
            binaryPredicate = Unequal;

            return true;
        }

        binaryPredicate = null;

        return false;
    }

    public ValidationResult ValidSolution(IReadOnlyList<Assignment<Square, int>> assignments)
    {
        NumberedSquare[] solution = ParseSolution(assignments);
        ValidationResult result = ValidationResult.Success;

        if (CheckForIncorrectSolutionSize(solution, out string? errorMessage1))
        {
            result = ValidationResult.Failure(errorMessage1);
        }
        else if (CheckForNumberOutOfRange(solution, out string? errorMessage2))
        {
            result = ValidationResult.Failure(errorMessage2);
        }
        else if (CheckForSquareOutsideGrid(solution, out string? errorMessage3))
        {
            result = ValidationResult.Failure(errorMessage3);
        }
        else if (CheckForSquareFilledTwice(solution, out string? errorMessage4))
        {
            result = ValidationResult.Failure(errorMessage4);
        }
        else if (CheckForObstructingSquaresWithSameNumber(solution, out string? errorMessage5))
        {
            result = ValidationResult.Failure(errorMessage5);
        }

        return result;
    }

    private IEnumerable<int> GetObstructedNumbers(Square square)
    {
        foreach ((Square hintSquare, int hintNumber) in Hints)
        {
            if (square.Obstructs(hintSquare))
            {
                yield return hintNumber;
            }
        }
    }

    private bool CheckForIncorrectSolutionSize(NumberedSquare[] solution, [NotNullWhen(true)] out string? errorMessage)
    {
        int expectedSize = 81 - Hints.Count;

        if (solution.Length != expectedSize)
        {
            errorMessage = $"Solution size is {solution.Length}, should be {expectedSize}.";

            return true;
        }

        errorMessage = null;

        return false;
    }

    private static bool CheckForNumberOutOfRange(
        NumberedSquare[] solution,
        [NotNullWhen(true)] out string? errorMessage
    )
    {
        foreach (NumberedSquare numberedSquare in solution)
        {
            if (numberedSquare.Number is >= 1 and <= 9)
            {
                continue;
            }

            errorMessage = $"Numbered square {numberedSquare} is out of range.";

            return true;
        }

        errorMessage = null;

        return false;
    }

    private bool CheckForSquareOutsideGrid(NumberedSquare[] solution, [NotNullWhen(true)] out string? errorMessage)
    {
        foreach (NumberedSquare numberedSquare in solution)
        {
            if (Grid.Contains(numberedSquare.Square))
            {
                continue;
            }

            errorMessage = $"Numbered square {numberedSquare} is outside grid.";

            return true;
        }

        errorMessage = null;

        return false;
    }

    private bool CheckForSquareFilledTwice(NumberedSquare[] solution, [NotNullWhen(true)] out string? errorMessage)
    {
        IEnumerable<string> duplicatedSquares = solution
            .Concat(Hints)
            .GroupBy(numberedSquare => numberedSquare.Square)
            .Where(group => group.Count() > 1)
            .OrderBy(group => group.Key)
            .Select(group => $"Square {group.Key} is filled twice.");

        errorMessage = duplicatedSquares.FirstOrDefault();

        return errorMessage is not null;
    }

    private bool CheckForObstructingSquaresWithSameNumber(
        NumberedSquare[] solution,
        [NotNullWhen(true)] out string? errorMessage
    )
    {
        NumberedSquare[] allSquares = solution.Concat(Hints).OrderBy(ns => ns).ToArray();

        for (int i = 0; i < allSquares.Length; i++)
        {
            (Square squareAtI, int numberAtI) = allSquares[i];

            for (int j = i + 1; j < allSquares.Length; j++)
            {
                (Square squareAtJ, int numberAtJ) = allSquares[j];

                if (squareAtI.Obstructs(squareAtJ) && numberAtI == numberAtJ)
                {
                    errorMessage = $"Obstructing squares {squareAtI} and {squareAtJ} have same number [{numberAtJ}].";

                    return true;
                }
            }
        }

        errorMessage = null;

        return false;
    }

    public static SudokuProblem Parse(string input)
    {
        List<NumberedSquare> hints = [];
        string[][] parsed = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(row => row.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (int.TryParse(parsed[row][col], out int number))
                {
                    hints.Add(new NumberedSquare(new Square(col, row), number));
                }
            }
        }

        return new SudokuProblem(new Block(new Square(0, 0), new Dimensions(9, 9)), hints);
    }

    private static bool Unequal(int n1, int n2) => n1 != n2;

    private static NumberedSquare[] ParseSolution(IReadOnlyList<Assignment<Square, int>> assignments)
    {
        return assignments
            .Select(assignment => new NumberedSquare(assignment.Variable, assignment.DomainValue))
            .ToArray();
    }
}
