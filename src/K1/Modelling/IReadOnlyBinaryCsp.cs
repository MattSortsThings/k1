namespace K1.Modelling;

/// <summary>
///     A read-only binary CSP that can be queried by variable and domain value indices.
/// </summary>
/// <typeparam name="V">The binary CSP variable type.</typeparam>
/// <typeparam name="D">The binary CSP domain value type.</typeparam>
public interface IReadOnlyBinaryCsp<V, D> : IBinaryCsp<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    int Variables { get; }

    int Pairs { get; }

    int Constraints { get; }

    double ConstraintDensity { get; }

    /// <summary>
    ///     Retrieves the variable at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the variable.</param>
    /// <returns>The queried variable.</returns>
    V GetVariable(int index);

    /// <summary>
    ///     Retrieves the ordered domain of the variable at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the variable.</param>
    /// <returns>The domain of the queried variable.</returns>
    IReadOnlyList<D> GetDomain(int index);

    IEnumerable<int> GetAdjacentVariableIndices(int index);

    int GetDegree(int index);

    double GetSumConstraintTightness(int index);

    double GetMeanConstraintTightness(int index);

    double GetMeanPairTightness(int index);

    Assignment<V, D> MapAssignment(IAssignment assignment);

    /// <summary>
    ///     Determines whether the variable pair at the specified indices are adjacent.
    /// </summary>
    /// <param name="indexA">The zero-based index of the first variable.</param>
    /// <param name="indexB">The zero-based index of the second variable.</param>
    /// <returns><see langword="true" /> if the queried variables are adjacent; otherwise, <see langword="false" />.</returns>
    bool Adjacent(int indexA, int indexB);

    bool Consistent(IAssignment assignmentA, IAssignment assignmentB);
}
