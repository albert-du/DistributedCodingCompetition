namespace DistributedCodingCompetition.AuthService.Services;

public interface IModalService
{
    public Task ShowError(string title, string message);
}
