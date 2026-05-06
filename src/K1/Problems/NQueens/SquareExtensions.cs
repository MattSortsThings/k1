using K1.Problems.Elements;

namespace K1.Problems.NQueens;

internal static class SquareExtensions
{
    public static bool Captures(this Square square, Square other)
    {
        (int col1, int row1) = square;
        (int col2, int row2) = other;

        return col1 == col2 || row1 == row2 || Math.Abs(col1 - col2) == Math.Abs(row1 - row2);
    }
}
