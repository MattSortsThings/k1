using K1.Modelling;
using K1.Tests.Unit.Utils;

namespace K1.Tests.Unit.Modelling;

public sealed partial class BinaryCspTests : UnitTests
{
    [Test]
    [Arguments(0, 0, 1, 1)]
    [Arguments(0, 1, 1, 0)]
    [Arguments(0, 1, 1, 1)]
    [Arguments(0, 2, 1, 0)]
    [Arguments(1, 0, 0, 1)]
    [Arguments(1, 0, 0, 2)]
    [Arguments(1, 1, 0, 0)]
    [Arguments(1, 1, 0, 1)]
    public async Task Consistent_returns_true_when_variables_are_adjacent_and_domain_value_pair_is_legal(
        int variableIndexA,
        int domainValueIndexA,
        int variableIndexB,
        int domainValueIndexB
    )
    {
        // Arrange
        AllDifferentProblem problem = new() { { "LT", ['b', 'r', 'y'] }, { "SE", ['b', 'y'] } };

        BinaryCsp<string, char> sut = problem.AsBinaryCsp();

        TestAssignment assignmentA = new() { VariableIndex = variableIndexA, DomainValueIndex = domainValueIndexA };
        TestAssignment assignmentB = new() { VariableIndex = variableIndexB, DomainValueIndex = domainValueIndexB };

        // Act
        bool result = sut.Consistent(assignmentA, assignmentB);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(0, 0, 1, 0)]
    [Arguments(0, 2, 1, 1)]
    [Arguments(1, 0, 0, 0)]
    [Arguments(1, 1, 0, 2)]
    public async Task Consistent_returns_false_when_variables_are_adjacent_and_domain_value_pair_is_illegal(
        int variableIndexA,
        int domainValueIndexA,
        int variableIndexB,
        int domainValueIndexB
    )
    {
        // Arrange
        AllDifferentProblem problem = new() { { "LT", ['b', 'r', 'y'] }, { "SE", ['b', 'y'] } };

        BinaryCsp<string, char> sut = problem.AsBinaryCsp();

        TestAssignment assignmentA = new() { VariableIndex = variableIndexA, DomainValueIndex = domainValueIndexA };
        TestAssignment assignmentB = new() { VariableIndex = variableIndexB, DomainValueIndex = domainValueIndexB };

        // Act
        bool result = sut.Consistent(assignmentA, assignmentB);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(0, 0, 1, 0)]
    [Arguments(0, 0, 1, 1)]
    [Arguments(0, 1, 1, 0)]
    [Arguments(0, 1, 1, 1)]
    [Arguments(1, 0, 0, 0)]
    [Arguments(1, 0, 0, 1)]
    [Arguments(1, 1, 0, 0)]
    [Arguments(1, 1, 0, 1)]
    public async Task Consistent_returns_true_when_variables_are_not_adjacent(
        int variableIndexA,
        int domainValueIndexA,
        int variableIndexB,
        int domainValueIndexB
    )
    {
        // Arrange
        AllDifferentProblem problem = new() { { "PL", ['r', 'w'] }, { "SE", ['b', 'y'] } };
        BinaryCsp<string, char> sut = problem.AsBinaryCsp();

        TestAssignment assignmentA = new() { VariableIndex = variableIndexA, DomainValueIndex = domainValueIndexA };
        TestAssignment assignmentB = new() { VariableIndex = variableIndexB, DomainValueIndex = domainValueIndexB };

        // Act
        bool result = sut.Consistent(assignmentA, assignmentB);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(0, 0, 0, 0)]
    [Arguments(0, 1, 0, 1)]
    public async Task Consistent_returns_true_when_variables_are_same_and_domain_values_are_same(
        int variableIndexA,
        int domainValueIndexA,
        int variableIndexB,
        int domainValueIndexB
    )
    {
        // Arrange
        AllDifferentProblem problem = new() { { "PL", ['r', 'w'] } };
        BinaryCsp<string, char> sut = problem.AsBinaryCsp();

        TestAssignment assignmentA = new() { VariableIndex = variableIndexA, DomainValueIndex = domainValueIndexA };
        TestAssignment assignmentB = new() { VariableIndex = variableIndexB, DomainValueIndex = domainValueIndexB };

        // Act
        bool result = sut.Consistent(assignmentA, assignmentB);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments(0, 0, 0, 1)]
    [Arguments(0, 1, 0, 0)]
    public async Task Consistent_returns_false_when_variables_are_same_and_domain_values_are_different(
        int variableIndexA,
        int domainValueIndexA,
        int variableIndexB,
        int domainValueIndexB
    )
    {
        // Arrange
        AllDifferentProblem problem = new() { { "PL", ['r', 'w'] } };
        BinaryCsp<string, char> sut = problem.AsBinaryCsp();

        TestAssignment assignmentA = new() { VariableIndex = variableIndexA, DomainValueIndex = domainValueIndexA };
        TestAssignment assignmentB = new() { VariableIndex = variableIndexB, DomainValueIndex = domainValueIndexB };

        // Act
        bool result = sut.Consistent(assignmentA, assignmentB);

        // Assert
        await Assert.That(result).IsFalse();
    }

    private sealed record TestAssignment : IAssignment
    {
        public int VariableIndex { get; init; }

        public int DomainValueIndex { get; set; }
    }
}
