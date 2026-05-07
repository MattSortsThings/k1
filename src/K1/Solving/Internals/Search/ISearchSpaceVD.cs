using K1.Modelling;

namespace K1.Solving.Internals.Search;

internal interface ISearchSpace<V, D> : ISearchSpace
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    void Populate(IReadOnlyBinaryCsp<V, D> binaryCsp);

    void Reset();

    Assignment<V, D>[] GetMappedAssignments();
}
