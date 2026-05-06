namespace K1.Problems.Elements;

public readonly record struct NumberedSquare(Square Square, int Number) : IComparable<NumberedSquare>
{
    public int CompareTo(NumberedSquare other)
    {
        int squareComparison = Square.CompareTo(other.Square);

        return squareComparison != 0 ? squareComparison : Number.CompareTo(other.Number);
    }

    public bool Equals(NumberedSquare other) => Square.Equals(other.Square) && Number == other.Number;

    public override int GetHashCode() => HashCode.Combine(Square, Number);

    public override string ToString() => $"{Square} [{Number}]";
}
