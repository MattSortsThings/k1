namespace K1.Problems.Elements;

public readonly record struct Rectangle(Square TopLeftSquare, Dimensions Dimensions) : IComparable<Rectangle>
{
    public int CompareTo(Rectangle other)
    {
        int topLeftSquareComparison = TopLeftSquare.CompareTo(other.TopLeftSquare);

        return topLeftSquareComparison != 0 ? topLeftSquareComparison : Dimensions.CompareTo(other.Dimensions);
    }

    public bool Equals(Rectangle other) =>
        TopLeftSquare.Equals(other.TopLeftSquare) && Dimensions.Equals(other.Dimensions);

    public override int GetHashCode() => HashCode.Combine(TopLeftSquare, Dimensions);

    public bool Contains(Square square)
    {
        (int col, int row) = square;
        (int leftCol, int topRow) = TopLeftSquare;
        (int rightCol, int bottomRow) = GetBottomRightSquare();

        return col >= leftCol && col <= rightCol && row >= topRow && row <= bottomRow;
    }

    private Square GetBottomRightSquare()
    {
        (int leftCol, int topRow) = TopLeftSquare;
        (int columns, int rows) = Dimensions;

        return new Square((leftCol + columns) - 1, (topRow + rows) - 1);
    }
}
