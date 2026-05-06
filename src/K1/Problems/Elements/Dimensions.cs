namespace K1.Problems.Elements;

public readonly record struct Dimensions(int Columns, int Rows) : IComparable<Dimensions>
{
    public int CompareTo(Dimensions other)
    {
        int columnsComparison = Columns.CompareTo(other.Columns);

        return columnsComparison != 0 ? columnsComparison : Rows.CompareTo(other.Rows);
    }

    public bool Equals(Dimensions other) => Columns == other.Columns && Rows == other.Rows;

    public override int GetHashCode() => HashCode.Combine(Columns, Rows);

    public override string ToString() => $"{Columns}x{Rows}";
}
