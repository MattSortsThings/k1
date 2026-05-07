namespace K1.Solving.Internals.Traversal;

internal interface ITraversalStrategy
{
    /// <summary>
    ///     Gets the strategy's unique identifier.
    /// </summary>
    TraversalStrategy Identifier { get; }
}
