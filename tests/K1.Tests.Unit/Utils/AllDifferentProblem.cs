using System.Diagnostics.CodeAnalysis;
using K1.Modelling;

namespace K1.Tests.Unit.Utils;

public sealed class AllDifferentProblem : Dictionary<string, char[]>, IBinaryCspModel<string, char>
{
    public IReadOnlySet<string> GetVariables() => Keys.ToHashSet();

    public IEnumerable<char> GetDomainValues(string v) => this[v];

    public bool TryGetBinaryPredicate(
        string v1,
        string v2,
        [NotNullWhen(true)] out Func<char, char, bool>? binaryPredicate
    )
    {
        binaryPredicate = Unequal;

        return true;
    }

    public ValidationResult ValidSolution(IReadOnlyList<Assignment<string, char>> assignments)
    {
        ValidationResult result = ValidationResult.Success;
        Dictionary<string, char> solution = MapToDictionary(assignments);

        if (solution.Count != Count)
        {
            result = ValidationResult.Failure("Incorrect solution size.");
        }
        else if (CheckForIllegalKey(solution) is { } illegalKeyMessage)
        {
            result = ValidationResult.Failure(illegalKeyMessage);
        }
        else if (CheckForIllegalKeyValuePair(solution) is { } illegalKeyValuePairMessage)
        {
            result = ValidationResult.Failure(illegalKeyValuePairMessage);
        }
        else if (CheckForDuplicateValues(solution) is { } duplicateValuesMessage)
        {
            result = ValidationResult.Failure(duplicateValuesMessage);
        }

        return result;
    }

    private string? CheckForIllegalKey(Dictionary<string, char> solution)
    {
        foreach ((string key, _) in solution)
        {
            if (!ContainsKey(key))
            {
                return $"Key ({key}) not present in problem.";
            }
        }

        return null;
    }

    private string? CheckForIllegalKeyValuePair(Dictionary<string, char> solution)
    {
        foreach ((string key, char value) in solution)
        {
            if (!this[key].Contains(value))
            {
                return $"Key ({key}) assigned value ({value}) outside problem domain.";
            }
        }

        return null;
    }

    private static string? CheckForDuplicateValues(Dictionary<string, char> solution) =>
        (
            from grouping in solution.GroupBy(kvp => kvp.Value)
            where grouping.Count() > 1
            select $"Multiple keys assigned same value ({grouping.Key})."
        ).FirstOrDefault();

    private static bool Unequal(char c1, char c2) => c1 != c2;

    private static Dictionary<string, char> MapToDictionary(IReadOnlyList<Assignment<string, char>> assignments) =>
        assignments.ToDictionary(assignment => assignment.Variable, assignment => assignment.DomainValue);
}
