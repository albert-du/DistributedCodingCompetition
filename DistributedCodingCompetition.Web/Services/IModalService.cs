namespace DistributedCodingCompetition.Web.Services;

public interface IModalService
{
    public void ShowError(string title, string message);
    public void ShowInfo(string title, string message);
    public event Action<ModalMessage> OnShow;

    public record ModalMessage(ModalType Type, string Title, string Message);

    public enum ModalType
    {
        Error,
        Info,
    }
}
