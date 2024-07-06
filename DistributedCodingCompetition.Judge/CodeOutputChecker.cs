namespace DistributedCodingCompetition.Judge;

public static class CodeOutputChecker
{
    public static bool CheckOutput(string expectedOutput, string actualOutput)
    {
        var expectedLines = expectedOutput.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        var actualLines = actualOutput.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        return expectedLines.SequenceEqual(actualLines);
    }
}
