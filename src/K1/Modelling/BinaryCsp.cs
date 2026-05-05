using System.Diagnostics.CodeAnalysis;

namespace K1.Modelling;

/// <summary>
///     A binary CSP that can be populated from a binary CSP model.
/// </summary>
/// <typeparam name="V">The binary CSP variable type.</typeparam>
/// <typeparam name="D">The binary CSP domain value type.</typeparam>
[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
public sealed class BinaryCsp<V, D> : IReadOnlyBinaryCsp<V, D>
    where V : IComparable<V>, IEquatable<V>
    where D : IComparable<D>, IEquatable<D>
{
    private readonly List<List<LegalityMatrix?>> _adjacencyMatrix = [];
    private readonly List<D[]> _domains = [];
    private readonly List<V> _variables = [];

    public void Populate(IBinaryCspModel<V, D> model)
    {
        ArgumentNullException.ThrowIfNull(model);
        PopulateVariablesAndDomains(model);
        ThrowIfZeroVariables();
        ResizeAdjacencyMatrix();
        PopulateAdjacencyMatrix(model);
    }

    public void Clear()
    {
        _domains.Clear();
        _variables.Clear();
        _adjacencyMatrix.Clear();
        Pairs = 0;
        Constraints = 0;
    }

    public int Variables => _variables.Count;

    public int Pairs { get; private set; }

    public int Constraints { get; private set; }

    public double ConstraintDensity =>
        Constraints == 0 ? 0
        : Constraints == Pairs ? 1
        : Constraints / (double)Pairs;

    public V GetVariable(int index) => _variables[index];

    public IReadOnlyList<D> GetDomain(int index) => _domains[index];

    public IEnumerable<int> GetAdjacentVariableIndices(int index)
    {
        List<LegalityMatrix?> adjacencyRow = _adjacencyMatrix[index];

        for (int i = 0; i < adjacencyRow.Count; i++)
        {
            if (adjacencyRow[i] is not null)
            {
                yield return i;
            }
        }
    }

    public int GetDegree(int index)
    {
        List<LegalityMatrix?> adjacencyRow = _adjacencyMatrix[index];
        int degree = 0;

        for (int i = 0; i < adjacencyRow.Count; i++)
        {
            if (adjacencyRow[i] is not null)
            {
                degree++;
            }
        }

        return degree;
    }

    public double GetSumConstraintTightness(int index)
    {
        List<LegalityMatrix?> adjacencyRow = _adjacencyMatrix[index];
        double sumConstraintTightness = 0;

        for (int i = 0; i < adjacencyRow.Count; i++)
        {
            if (adjacencyRow[i] is { } legalityMatrix)
            {
                sumConstraintTightness += legalityMatrix.Tightness;
            }
        }

        return sumConstraintTightness;
    }

    public double GetMeanConstraintTightness(int index)
    {
        List<LegalityMatrix?> adjacencyRow = _adjacencyMatrix[index];
        int constraints = 0;
        double sumConstraintTightness = 0;

        for (int i = 0; i < adjacencyRow.Count; i++)
        {
            if (adjacencyRow[i] is not { } legalityMatrix)
            {
                continue;
            }

            constraints++;
            sumConstraintTightness += legalityMatrix.Tightness;
        }

        return constraints == 0 ? 0 : sumConstraintTightness / constraints;
    }

    public double GetMeanPairTightness(int index)
    {
        List<LegalityMatrix?> adjacencyRow = _adjacencyMatrix[index];
        double sumConstraintTightness = 0;
        int pairs = adjacencyRow.Count - 1;

        for (int i = 0; i < adjacencyRow.Count; i++)
        {
            if (adjacencyRow[i] is not { } legalityMatrix)
            {
                continue;
            }

            sumConstraintTightness += legalityMatrix.Tightness;
        }

        return pairs == 0 ? 0 : sumConstraintTightness / pairs;
    }

    public Assignment<V, D> MapAssignment(IAssignment assignment)
    {
        (int variableIndex, int domainValueIndex) = assignment;

        return new Assignment<V, D>(_variables[variableIndex], _domains[variableIndex][domainValueIndex]);
    }

    public bool Adjacent(int indexA, int indexB) => _adjacencyMatrix[indexA][indexB] is not null;

    public bool Consistent(IAssignment assignmentA, IAssignment assignmentB)
    {
        (int variableIndexA, int domainValueIndexA) = assignmentA;
        (int variableIndexB, int domainValueIndexB) = assignmentB;

        return _adjacencyMatrix[variableIndexA][variableIndexB] is { } legalityMatrix
            ? legalityMatrix.Legal(domainValueIndexA, domainValueIndexB)
            : variableIndexA != variableIndexB || domainValueIndexA == domainValueIndexB;
    }

    private void PopulateAdjacencyMatrix(IBinaryCspModel<V, D> model)
    {
        for (int srcIndex = 0; srcIndex < _variables.Count; srcIndex++)
        {
            for (int dstIndex = srcIndex + 1; dstIndex < _variables.Count; dstIndex++)
            {
                PopulateIfAdjacent(srcIndex, dstIndex, model);
            }
        }
    }

    private void ResizeAdjacencyMatrix()
    {
        while (_adjacencyMatrix.Count < _variables.Count)
        {
            _adjacencyMatrix.Add(Enumerable.Repeat<LegalityMatrix?>(null, _variables.Count).ToList());
        }
    }

    private void ThrowIfZeroVariables()
    {
        if (_variables.Count == 0)
        {
            throw new ArgumentException("Model yielded empty variables set.");
        }
    }

    private void PopulateVariablesAndDomains(IBinaryCspModel<V, D> model)
    {
        foreach (V v in model.GetVariables().OrderBy(v => v))
        {
            _variables.Add(v);
            _domains.Add(model.GetDomainValues(v).Distinct().OrderBy(d => d).ToArray());
        }

        Pairs = Variables <= 1 ? 0 : (Variables * (Variables - 1)) / 2;
    }

    private void PopulateIfAdjacent(int srcIndex, int dstIndex, IBinaryCspModel<V, D> model)
    {
        (V srcVariable, V dstVariable) = (_variables[srcIndex], _variables[dstIndex]);

        if (!model.TryGetBinaryPredicate(srcVariable, dstVariable, out Func<D, D, bool>? binaryPredicate))
        {
            return;
        }

        (D[] srcDomain, D[] dstDomain) = (_domains[srcIndex], _domains[dstIndex]);
        (int srcDomainSize, int dstDomainSize) = (srcDomain.Length, dstDomain.Length);
        int cartesianProductSize = srcDomainSize * dstDomainSize;

        LegalityMask[] srcRows = Enumerable
            .Range(0, srcDomainSize)
            .Select(_ => LegalityMask.WithWordCount((int)Math.Ceiling(dstDomainSize / 64.0)))
            .ToArray();

        LegalityMask[] dstRows = Enumerable
            .Range(0, dstDomainSize)
            .Select(_ => LegalityMask.WithWordCount((int)Math.Ceiling(srcDomainSize / 64.0)))
            .ToArray();

        int illegalPairs = 0;

        for (int s = 0; s < srcDomainSize; s++)
        {
            D srcDomainValue = srcDomain[s];
            for (int d = 0; d < dstDomainSize; d++)
            {
                D dstDomainValue = dstDomain[d];

                if (binaryPredicate(srcDomainValue, dstDomainValue))
                {
                    srcRows[s].SetLegal(d);
                    dstRows[d].SetLegal(s);
                }
                else
                {
                    illegalPairs++;
                }
            }
        }

        if (illegalPairs == 0)
        {
            return;
        }

        double tightness = illegalPairs / (double)cartesianProductSize;
        _adjacencyMatrix[srcIndex][dstIndex] = new LegalityMatrix(srcRows, srcDomainSize, dstDomainSize, tightness);
        _adjacencyMatrix[dstIndex][srcIndex] = new LegalityMatrix(dstRows, dstDomainSize, srcDomainSize, tightness);
        Constraints++;
    }
}
