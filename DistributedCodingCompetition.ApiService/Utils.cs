namespace DistributedCodingCompetition.ApiService;

public static class Utils
{
    public static string RandomOTC() =>
        Random.Shared.Next(100000, 999999).ToString();
}
