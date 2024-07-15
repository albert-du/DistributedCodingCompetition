namespace DistributedCodingCompetition.Models;

/// <summary>
/// Status of a user solving a problem
/// 
/// Not an entity
/// </summary>
/// <param name="Problem">Id of the Problem</param>
/// <param name="Points">Points it's worth in the contest</param>
/// <param name="Score">Score earned</param>
/// <param name="MaxScore">Max possible score</param>
public record ProblemUserSolveStatus(Guid Problem, int Points, int Score, int MaxScore, int CasesPassed, int CasesTotal)
{
    /// <summary>
    /// Maximum score achieved
    /// </summary>
    public bool Completed => Score == MaxScore;
}