using K1.Modelling;
using K1.Tests.Unit.Utils;
using TUnit.Assertions.Enums;

namespace K1.Tests.Unit.Modelling;

public sealed partial class BinaryCspTests
{
    [Test]
    public async Task Populate_populates_variables_using_variable_type_comparison()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        AllDifferentProblem problem = new()
        {
            { "FI", ['b', 'w'] },
            { "CZ", ['b', 'r', 'w'] },
            { "LT", ['g', 'r', 'y'] },
        };

        // Act
        sut.Populate(problem);

        // Assert
        await Assert.That(sut.GetVariable(0)).IsEqualTo("CZ");
        await Assert.That(sut.GetVariable(1)).IsEqualTo("FI");
        await Assert.That(sut.GetVariable(2)).IsEqualTo("LT");
    }

    [Test]
    public async Task Populate_populates_domains_using_domain_value_type_comparison()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        AllDifferentProblem problem = new()
        {
            { "CZ", ['b', 'r', 'w'] },
            { "FI", ['b', 'w'] },
            { "LT", ['g', 'r', 'y'] },
        };

        // Act
        sut.Populate(problem);

        // Assert
        await Assert.That(sut.GetDomain(0)).IsEquivalentTo(['b', 'r', 'w'], CollectionOrdering.Matching);
        await Assert.That(sut.GetDomain(1)).IsEquivalentTo(['b', 'w'], CollectionOrdering.Matching);
        await Assert.That(sut.GetDomain(2)).IsEquivalentTo(['g', 'r', 'y'], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Populate_removes_duplicate_domain_values_using_domain_value_equality()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        AllDifferentProblem problem = new() { { "CZ", ['b', 'r', 'w', 'b', 'r', 'w'] } };

        // Act
        sut.Populate(problem);

        // Assert
        await Assert.That(sut.GetDomain(0)).IsEquivalentTo(['b', 'r', 'w'], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Populate_sets_proven_adjacent_variables_only_as_adjacent()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        AllDifferentProblem problem = new()
        {
            { "CZ", ['b', 'r', 'w'] },
            { "PL", ['r', 'w'] },
            { "SE", ['b', 'y'] },
        };

        // Act
        sut.Populate(problem);

        // Assert
        await Assert.That(sut.Adjacent(0, 0)).IsFalse().Because("No variable is adjacent to itself");
        await Assert.That(sut.Adjacent(0, 1)).IsTrue().Because("CZ and PL have w in common");
        await Assert.That(sut.Adjacent(0, 2)).IsTrue().Because("CZ and SE have b in common");

        await Assert.That(sut.Adjacent(1, 0)).IsTrue().Because("PL and CZ have w in common");
        await Assert.That(sut.Adjacent(1, 1)).IsFalse().Because("No variable is adjacent to itself");
        await Assert.That(sut.Adjacent(1, 2)).IsFalse().Because("PL and SE have no colours in common");

        await Assert.That(sut.Adjacent(2, 0)).IsTrue().Because("SE and CZ have b in common");
        await Assert.That(sut.Adjacent(2, 1)).IsFalse().Because("SE and PL have no colours in common");
        await Assert.That(sut.Adjacent(2, 2)).IsFalse().Because("No variable is adjacent to itself");
    }

    [Test]
    public async Task Populate_throws_when_model_yields_empty_variables_set()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        AllDifferentProblem problem = new();

        // Assert
        await Assert
            .That(() => sut.Populate(problem))
            .Throws<ArgumentException>()
            .WithMessage("Model yielded empty variables set.");
    }

    [Test]
    public async Task Populate_throws_when_model_arg_is_null()
    {
        // Arrange
        BinaryCsp<string, char> sut = new();

        IBinaryCspModel<string, char> model = null!;

        // Assert
        await Assert
            .That(() => sut.Populate(model))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'model')");
    }
}
