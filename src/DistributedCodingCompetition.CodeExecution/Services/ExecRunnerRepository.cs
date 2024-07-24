namespace DistributedCodingCompetition.CodeExecution.Services;

using MongoDB.Driver;
using DistributedCodingCompetition.CodeExecution.Models;

/// <inheritdoc/>
public class ExecRunnerRepository(IMongoClient mongoClient) : IExecRunnerRepository
{
    private IMongoCollection<ExecRunner> collection =
        mongoClient.GetDatabase("code-execution").GetCollection<ExecRunner>("exec-runners");

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ExecRunner>> GetExecRunnersAsync(bool enabled = false)
    {
        if (enabled)
            return await collection.Find(runner => runner.Enabled).ToListAsync();

        return await collection.AsQueryable().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<ExecRunner?> ReadExecRunnerAsync(Guid id) =>
        await collection.Find(runner => runner.Id == id).FirstOrDefaultAsync();

    /// <inheritdoc/>
    public async Task CreateExecRunnerAsync(ExecRunner execRunner)
    {
        await collection.InsertOneAsync(execRunner);
    }

    /// <inheritdoc/>
    public async Task UpdateExecRunnerAsync(ExecRunner execRunner)
    {
        await collection.ReplaceOneAsync(runner => runner.Id == execRunner.Id, execRunner);
    }

    /// <inheritdoc/>
    public async Task DeleteExecRunnerAsync(Guid id)
    {
        await collection.DeleteOneAsync(runner => runner.Id == id);
    }
}