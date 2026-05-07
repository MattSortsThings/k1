using System.ComponentModel;

namespace K1.Solving.Internals.Traversal;

internal static class TraversalStrategyExtensions
{
    public static ITraversalStrategy CreateStrategy(this TraversalStrategy strategy) =>
        throw new InvalidEnumArgumentException(nameof(strategy), (int)strategy, typeof(TraversalStrategy));
}
