namespace DistributedCodingCompetition.Web.Services;

using Ganss.Xss;
using Markdig;

/// <summary>
/// Service to render markdown to HTML.
/// </summary>
/// <param name="htmlSanitizer"></param>
public class MarkdownRenderService(HtmlSanitizer htmlSanitizer) : IMarkdownRenderService
{
    /// <summary>
    /// Renders markdown to HTML.
    /// </summary>
    /// <param name="markdown"></param>
    /// <returns></returns>
    public string Render(string markdown)
    {
        var html = Markdown.ToHtml(markdown, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
        return htmlSanitizer.Sanitize(html);
    }
}