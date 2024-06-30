namespace DistributedCodingCompetition.Web.Services;

public interface IModalService
{
    void ShowError(string title, string message);
    void ShowInfo(string title, string message);
    event Action<ModalMessage> OnShow;
    record ModalMessage(ModalType Type, string Title, string Message);

    public enum ModalType
    {
        Error,
        Info,
    }
}
