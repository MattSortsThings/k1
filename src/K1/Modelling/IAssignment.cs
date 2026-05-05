namespace K1.Modelling;

/// <summary>
///     Represents an assignment of a domain value to a variable in a binary CSP.
/// </summary>
public interface IAssignment
{
    /// <summary>
    ///     Gets the zero-based index of the variable.
    /// </summary>
    int VariableIndex { get; }

    /// <summary>
    ///     Gets or sets the zero-based index of the value in the variable's domain.
    /// </summary>
    int DomainValueIndex { get; set; }

    void Deconstruct(out int variableIndex, out int domainValueIndex)
    {
        variableIndex = VariableIndex;
        domainValueIndex = DomainValueIndex;
    }
}
