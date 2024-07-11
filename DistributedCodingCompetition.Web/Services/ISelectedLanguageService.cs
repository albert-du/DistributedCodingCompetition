namespace DistributedCodingCompetition.Web.Services;

public interface ISelectedLanguageService
{
    Task ReportLanguageSwitch(string language);
    Task<string?> GetSelectedLanguage();
}
