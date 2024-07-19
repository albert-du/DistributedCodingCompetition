namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Service to show modals
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Show an error modal
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    void ShowError(string title, string message);

    /// <summary>
    /// Show an info modal
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    void ShowInfo(string title, string message);

    void AskInteger(string title, string message, int min, int max, Action<int?> onResult);

    /// <summary>
    /// Event that is triggered when a modal is shown
    /// </summary>
    event Action<ModalMessage> OnShow;
    
    
    /// <summary>
    /// Event that is triggered when a modal is shown
    /// </summary>
    event Action<IntegerModalMessage> OnShowInteger;

    /// <summary>
    /// Model that represents a modal message
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="Title"></param>
    /// <param name="Message"></param>
    public sealed record ModalMessage(ModalType Type, string Title, string Message);


    /// <summary>
    /// Model that represents a modal message with an integer result
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Message"></param>
    /// <param name="OnResult"></param>
    public sealed record IntegerModalMessage(string Title, string Message, int Min, int Max, Action<int?> OnResult);

    /// <summary>
    /// Enum that represents the type of modal
    /// </summary>
    public enum ModalType
    {
        Error,
        Info,
    }
}
