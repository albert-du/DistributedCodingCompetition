﻿namespace DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Submission
/// </summary>
public class Submission
{
    /// <summary>
    /// Id of the submission
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Id of the submitter
    /// </summary>
    public Guid SubmitterId { get; set; }

    /// <summary>
    /// Navigation Property for the submitter
    /// </summary>
    public User? Submitter { get; set; }

    /// <summary>
    /// Id of the contest
    /// </summary>
    public Guid? ContestId { get; set; }

    /// <summary>
    /// Id of the problem
    /// </summary>
    public Guid ProblemId { get; set; }

    /// <summary>
    /// Problem
    /// </summary>
    public Problem? Problem { get; set; }

    /// <summary>
    /// Code submitted
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Language "lang=version"
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// UTC time of submission
    /// </summary>
    public DateTime SubmissionTime { get; set; } = DateTime.UtcNow;
    
    // Filled in by judge
    //   \/  \/  \/  \/  \/  \/  \/  \/ 

    /// <summary>
    /// Results of the submission
    /// </summary>
    public ICollection<TestCaseResult> Results { get; set; } = [];

    /// <summary>
    /// Score of the submission
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Max possible score
    /// </summary>
    public int MaxPossibleScore { get; set; }

    /// <summary>
    /// Points this submission is worth
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Time of evaluation
    /// </summary>
    public DateTime? EvaluationTime { get; set; }
}
