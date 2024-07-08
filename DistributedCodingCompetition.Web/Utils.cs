namespace DistributedCodingCompetition.Web;

/// <summary>
/// Utilities
/// </summary>
public static class Utils
{
    /// <summary>
    /// Generate 6 digit one time code
    /// </summary>
    /// <returns></returns>
    public static string RandomOTC() =>
        Random.Shared.Next(100000, 999999).ToString();

}
