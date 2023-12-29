namespace Ministry.Csv.Tests.TestSupport;

/// <summary>
/// Class for building CSV Definition test data.
/// </summary>
public static class CsvDefinitionProvider
{
    /// <summary>
    /// Gets the object for the provided data.
    /// </summary>
    public static CsvDefinition Get(string[] headers, params object[][] rows)
    {
        var definition = new CsvDefinition()
            .AddHeaders(headers);

        foreach (var row in rows)
            definition.AddRow(row);

        return definition;
    }

    /// <summary>
    /// Gets the definition with valid data.
    /// </summary>
    public static CsvDefinition GetValid()
        => Get(new[] {"Header 1", "Header 2", "Header 3"}, new object[] {1, 2, 3}, new object[] {"Things", 8, 9});
}