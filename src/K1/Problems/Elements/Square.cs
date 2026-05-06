namespace K1.Problems.Elements;

public readonly record struct Square(int Column, int Row) : IComparable<Square>
{
    public int CompareTo(Square other)
    {
        int columnComparison = Column.CompareTo(other.Column);

        return columnComparison != 0 ? columnComparison : Row.CompareTo(other.Row);
    }

    public bool Equals(Square other) => Column == other.Column && Row == other.Row;

    public override int GetHashCode() => HashCode.Combine(Column, Row);

    public override string ToString() => $"({Column},{Row})";
}
