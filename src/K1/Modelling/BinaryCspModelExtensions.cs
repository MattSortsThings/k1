namespace K1.Modelling;

public static class BinaryCspModelExtensions
{
    public static BinaryCsp<V, D> AsBinaryCsp<V, D>(this IBinaryCspModel<V, D> model)
        where V : IComparable<V>, IEquatable<V>
        where D : IComparable<D>, IEquatable<D>
    {
        BinaryCsp<V, D> binaryCsp = new();
        binaryCsp.Populate(model);

        return binaryCsp;
    }
}
