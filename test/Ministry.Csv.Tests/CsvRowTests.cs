using Ministry.Csv.Tests.TestSupport;

namespace Ministry.Csv.Tests;

[Trait("Category", "Csv")]
public class CsvRowTests
{
    [Theory]
    [InlineData("Header 1", "1")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public void HeaderDeterminationIsCaseInsensitive(string headers, string rows)
    {
        var headersArray = headers.Split(',');
        var definition = CsvDefinitionProvider.Get(headersArray,
            rows.Split('|').Select(row => row.Split(',')).ToArray());
        var testInput = definition.GenerateString();

        var result = CsvGenerator.FromString(testInput);
        var testRow = result.Rows.First();

        foreach (var header in headersArray)
        {
            var cell = testRow.Cell(header);
            Assert.True(cell != null, "The header was not found at all.");
            cell = testRow.Cell(header.ToLower());
            Assert.True(cell != null, "The header does not match when lower cased.");
            cell = testRow.Cell(header.ToUpper());
            Assert.True(cell != null, "The header does not match when upper cased.");
        }
    }
}