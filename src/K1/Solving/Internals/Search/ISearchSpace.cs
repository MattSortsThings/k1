namespace K1.Solving.Internals.Search;

internal interface ISearchSpace
{
    int LeafLevel { get; }

    IReadOnlyList<SearchStratum> Strata { get; }

    int PresentLevel { get; set; }

    bool DeadEnd { get; set; }
}
