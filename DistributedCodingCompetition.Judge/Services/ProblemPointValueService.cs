﻿namespace DistributedCodingCompetition.Judge.Services;

public class ProblemPointValueService(HttpClient httpClient) : IProblemPointValueService
{
    public async Task<int> GetPointMaxAsync(Guid contestId, Guid problemId)
    {
        var ppv = await httpClient.GetFromJsonAsync<ProblemPointValue> ($"api/contests/{contestId}/pointvalues/{problemId}");
        return ppv?.Points ?? 0;
    }
}
