using System.ComponentModel;

namespace K1.Solving.Internals.Ordering;

internal static class OrderingStrategyExtensions
{
    public static IOrderingStrategy CreateStrategy(this OrderingStrategy strategy) =>
        throw new InvalidEnumArgumentException(nameof(strategy), (int)strategy, typeof(OrderingStrategy));
}
