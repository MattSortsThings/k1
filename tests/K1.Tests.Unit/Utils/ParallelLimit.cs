using TUnit.Core.Interfaces;

namespace K1.Tests.Unit.Utils;

public sealed class ParallelLimit : IParallelLimit
{
    public int Limit => Environment.ProcessorCount;
}
