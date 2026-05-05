namespace K1.Modelling;

public readonly record struct ValidationResult
{
    public ValidationResult() { }

    private ValidationResult(bool successful, string? errorMessage)
    {
        Successful = successful;
        ErrorMessage = errorMessage;
    }

    public bool Successful { get; } = true;

    public string? ErrorMessage { get; }

    public void Deconstruct(out bool successful, out string? errorMessage)
    {
        successful = Successful;
        errorMessage = ErrorMessage;
    }

    public static ValidationResult Success() => new();

    public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
}
