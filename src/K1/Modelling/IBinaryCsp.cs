namespace K1.Modelling;

/// <summary>
///     A binary CSP that can be populated from a binary CSP model.
/// </summary>
/// <typeparam name="V">The binary CSP variable type.</typeparam>
/// <typeparam name="D">The binary CSP domain value type.</typeparam>
public interface IBinaryCsp<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    void Populate(IBinaryCspModel<V, D> model);

    void Clear();
}
