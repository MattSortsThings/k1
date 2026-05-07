namespace K1.Solving.Internals.Ordering;

internal interface IOrderingStrategy
{
    /// <summary>
    ///     Gets the strategy's unique identifier.
    /// </summary>
    OrderingStrategy Identifier { get; }
}
