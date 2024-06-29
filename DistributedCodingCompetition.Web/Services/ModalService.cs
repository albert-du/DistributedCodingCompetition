namespace DistributedCodingCompetition.Web.Services;

public class ModalService : IModalService
{
    public event Action<IModalService.ModalMessage>? OnShow;

    public void ShowError(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Error, title, message));
    
    public void ShowInfo(string title, string message) =>
        OnShow?.Invoke(new(IModalService.ModalType.Info, title, message));
}
