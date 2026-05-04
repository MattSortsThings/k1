using System;
using System.Collections.Generic;
using System.Text;

namespace K1.Tests.Unit;

public sealed class PlaceholderTests
{
    [Test]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(10)]
    public async Task Always_passes(int power)
    {
        var result = Math.Pow(Math.PI, power);

        await Assert.That(result).IsPositive();
    }
}
