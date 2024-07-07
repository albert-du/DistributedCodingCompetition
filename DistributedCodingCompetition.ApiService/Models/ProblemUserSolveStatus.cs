namespace DistributedCodingCompetition.ApiService.Models;

public record ProblemUserSolveStatus(Guid Problem, int Points, int Score, int MaxScore)
{
    public bool Completed => Score == MaxScore;
}