namespace DistributedCodingCompetition.Web.Services;

/// <inheritdoc/>
public sealed class ModalService : IModalService
{
    /// <inheritdoc/>
    public event Action<IModalService.ModalMessage>? OnShow;

    /// <inheritdoc/>
    public void ShowError(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Error, title, message));

    /// <inheritdoc/>
    public void ShowInfo(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Info, title, message));
}
