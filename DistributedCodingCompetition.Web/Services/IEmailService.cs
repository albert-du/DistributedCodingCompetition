namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Email sender
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send email with html message
    /// </summary>
    /// <param name="email"></param>
    /// <param name="subject"></param>
    /// <param name="message">html</param>
    /// <returns></returns>
    Task SendEmailAsync(string email, string subject, string message);
}
