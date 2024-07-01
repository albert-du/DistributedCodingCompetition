namespace DistributedCodingCompetition.Web.Services;

public interface IMarkdownRenderService
{
    Task<string> RenderAsync(string markdown);
}