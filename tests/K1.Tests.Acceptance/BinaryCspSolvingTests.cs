using K1.Tests.Acceptance.Utils;

namespace K1.Tests.Acceptance;

public sealed class BinaryCspSolvingTests : AcceptanceTests
{
    [Test]
    [MethodDataSource(nameof(TestCases))]
    public async Task Always_passes(int p)
    {
        double result = Math.Pow(Math.PI, p);

        await Assert.That(result).IsPositive();
    }

    public static IEnumerable<int> TestCases() => Enumerable.Range(1, 10);
}
