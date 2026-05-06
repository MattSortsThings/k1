using K1.Problems.Elements;

namespace K1.Problems.Sudoku;

internal static class SquareExtensions
{
    extension(Square square)
    {
        private int GetSector() => (square.Row / 3) + (3 * (square.Column / 3));

        public bool Obstructs(Square other) =>
            square.Row == other.Row || square.Column == other.Column || square.GetSector() == other.GetSector();
    }
}
