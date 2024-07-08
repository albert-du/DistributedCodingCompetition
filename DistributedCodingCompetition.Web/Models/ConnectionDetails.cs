namespace DistributedCodingCompetition.Web.Models;

/// <summary>
/// Connection details
/// </summary>
/// <param name="IpAddress">Ip of user</param>
/// <param name="UserAgent">Browser details</param>
public sealed record ConnectionDetails(string IpAddress, string UserAgent);