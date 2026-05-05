using System.Diagnostics.CodeAnalysis;

namespace K1.Modelling;

/// <summary>
///     A problem that is modelled as a binary CSP.
/// </summary>
/// <typeparam name="V">The binary CSP variable type.</typeparam>
/// <typeparam name="D">The binary CSP domain value type.</typeparam>
public interface IBinaryCspModel<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    /// <summary>
    ///     Gets the set of binary CSP variables.
    /// </summary>
    /// <returns>A set of variables.</returns>
    IReadOnlySet<V> GetVariables();

    /// <summary>
    ///     Gets the sequence of all domain values for the specified binary CSP variable.
    /// </summary>
    /// <param name="v">The binary CSP variable.</param>
    /// <returns>A sequence of domain values.</returns>
    IEnumerable<D> GetDomainValues(V v);

    /// <summary>
    ///     Finds the binary predicate to be applied to the domain values for the specified binary CSP variable pair.
    /// </summary>
    /// <param name="v1">The first binary CSP variable.</param>
    /// <param name="v2">The second binary CSP variable.</param>
    /// <param name="binaryPredicate">
    ///     When this method returns, the binary predicate that evaluates the legality of any pair of domain values for
    ///     the ordered variable pair (<paramref name="v1" />, <paramref name="v2" />) if they <i>may</i> participate
    ///     in a constraint given the problem definition; otherwise, <see langword="null" />.
    ///     This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the ordered variable pair (<paramref name="v1" />, <paramref name="v2" />)
    ///     <i>may</i> participate in a constraint; otherwise, <see langword="false" />.
    /// </returns>
    bool TryGetBinaryPredicate(V v1, V v2, [NotNullWhen(true)] out Func<D, D, bool>? binaryPredicate);

    ValidationResult ValidSolution(IReadOnlyList<Assignment<V, D>> assignments);
}
