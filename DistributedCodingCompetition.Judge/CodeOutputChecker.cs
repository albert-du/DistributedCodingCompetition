namespace DistributedCodingCompetition.Judge;

/// <summary>
/// Output checking methods
/// </summary>
public static class CodeOutputChecker
{
    /// <summary>
    /// Check if output matches expected output
    /// </summary>
    /// <param name="expectedOutput"></param>
    /// <param name="actualOutput"></param>
    /// <returns></returns>
    public static bool CheckOutput(string expectedOutput, string actualOutput)
    {
        var expectedLines = expectedOutput.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        var actualLines = actualOutput.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        return expectedLines.SequenceEqual(actualLines);
    }
}
