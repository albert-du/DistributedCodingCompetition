namespace DistributedCodingCompetition.ApiService.Data;

/// <summary>
/// Utilities for the API
/// </summary>
public static class Utils
{
    /// <summary>
    /// Create a random string of a given length
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
