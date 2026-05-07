using K1.Modelling;
using K1.Problems.Elements;
using K1.Problems.NQueens;
using K1.Solving;

NQueensProblem problem = NQueensProblem.FromN(4);
BinaryCsp<int, Square> binaryCsp = problem.AsBinaryCsp();
SearchAlgorithm algorithm = new(TraversalStrategy.NaiveBacktracking, OrderingStrategy.NaturalOrder);
BinaryCspSolver<int, Square> solver = BinaryCspSolver<int, Square>.WithStepDelay(TimeSpan.FromSeconds(1));

SolvingResult<int, Square> result = await solver.SolveAsync(binaryCsp, algorithm, CancellationToken.None);
ValidationResult validationResult = problem.ValidSolution(result.Assignments);
Console.WriteLine(validationResult);
