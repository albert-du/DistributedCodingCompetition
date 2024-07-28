namespace DistributedCodingCompetition.CodeExecution.Services;

/// <inheritdoc/>
public class ExecRunnerRepository(IMongoClient mongoClient) : IExecRunnerRepository
{
    private readonly IMongoCollection<ExecRunner> collection =
        mongoClient.GetDatabase("code-execution").GetCollection<ExecRunner>("exec-runners");

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ExecRunner>> GetExecRunnersAsync(bool enabled = false) =>
        await (
            enabled
            ? collection.Find(runner => runner.Enabled).ToListAsync()
            : collection.AsQueryable().ToListAsync());

    /// <inheritdoc/>
    public async Task<ExecRunner?> ReadExecRunnerAsync(Guid id) =>
        await collection.Find(runner => runner.Id == id).FirstOrDefaultAsync();

    /// <inheritdoc/>
    public Task CreateExecRunnerAsync(ExecRunner execRunner) =>
        collection.InsertOneAsync(execRunner);

    /// <inheritdoc/>
    public Task UpdateExecRunnerAsync(ExecRunner execRunner) =>
         collection.ReplaceOneAsync(runner => runner.Id == execRunner.Id, execRunner);

    /// <inheritdoc/>
    public Task DeleteExecRunnerAsync(Guid id) =>
         collection.DeleteOneAsync(runner => runner.Id == id);
}