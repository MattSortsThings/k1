namespace K1.Solving;

public enum SearchStatus
{
    /// <summary>
    ///     No search operation.
    /// </summary>
    None,

    /// <summary>
    ///     The search has started at the root node of the search tree.
    /// </summary>
    Initializing,

    /// <summary>
    ///     The search is in progress and no dead end has been detected, i.e. a leaf node <i>might</i> be reached from the
    ///     current search tree node.
    /// </summary>
    Assigning,

    /// <summary>
    ///     The search is in progress and a dead end has been detected; i.e. no leaf node can be reached from the current
    ///     search tree node.
    /// </summary>
    Backtracking,

    /// <summary>
    ///     The search has finished at a leaf node of the search tree, i.e. a complete solution to the binary CSP.
    /// </summary>
    Solved,

    /// <summary>
    ///     The search has finished at the root node of the search tree, i.e. the binary CSP has no solution.
    /// </summary>
    NoSolution,
}
