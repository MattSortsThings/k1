namespace K1.Modelling;

public readonly record struct Assignment<V, D>(V Variable, D DomainValue)
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>;
