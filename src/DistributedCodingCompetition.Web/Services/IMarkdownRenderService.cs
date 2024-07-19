namespace DistributedCodingCompetition.Web.Services;

/// <summary>
/// Service to render markdown to HTML.
/// </summary>
public interface IMarkdownRenderService
{
    /// <summary>
    /// Renders markdown to HTML.
    /// </summary>
    string Render(string markdown);
}