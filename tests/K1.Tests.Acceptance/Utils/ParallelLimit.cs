using TUnit.Core.Interfaces;

namespace K1.Tests.Acceptance.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => Environment.ProcessorCount;
}
