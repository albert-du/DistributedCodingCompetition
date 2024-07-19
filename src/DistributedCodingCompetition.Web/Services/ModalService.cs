namespace DistributedCodingCompetition.Web.Services;

/// <inheritdoc/>
public sealed class ModalService : IModalService
{
    /// <inheritdoc/>
    public event Action<IModalService.ModalMessage>? OnShow;
    public event Action<IModalService.IntegerModalMessage>? OnShowInteger;

    public void AskInteger(string title, string message, int min, int max, Action<int?> onResult) =>
        OnShowInteger?.Invoke(new(title, message, min, max, onResult));

    /// <inheritdoc/>
    public void ShowError(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Error, title, message));

    /// <inheritdoc/>
    public void ShowInfo(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Info, title, message));
}
