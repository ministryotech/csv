using Ministry.Csv.Tests.TestSupport;

namespace Ministry.Csv.Tests;

[Trait("Category", "Csv")]
public class CsvGeneratorTests
{
    [Theory]
    [InlineData("Header 1", "1")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,6")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,6|7,8,9|10,11,12")]
    [InlineData("Header 1,Header 2,Header 3", "Me,You,Cats Mother|Haddock,Fish,Rabbits")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,Rabbits")]
    [InlineData("Header 1,Header 2,Header 3", "Me,You,You're Great!|Haddock,Fish,Rabbits")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public void CanGenerateACsvDefinitionFromACompatibleString(string headers, string rows)
    {
            var definition = CsvDefinitionProvider.Get(headers.Split(','),
                rows.Split('|').Select(row => row.Split(',')).ToArray());
            var testInput = definition.GenerateString();

            var result = CsvGenerator.FromString(testInput);
            Assert.Equal(definition.Headers.Count, result.Headers.Count);
            Assert.Equal(definition.Rows.Count, result.Rows.Count);

            foreach (var header in definition.Headers)
                Assert.Contains(header, result.Headers);

            for (var i = 0; i < definition.Rows.Count; i++)
            {
                var definitionRow = definition.Rows[i];
                var resultRow = result.Rows[i];
                for (var ci = 0; ci < definitionRow.Count; ci++)
                {
                    Assert.Equal(definitionRow[ci].ToString(), resultRow[ci].ToString());
                }
            }
    }

    [Theory]
    [InlineData("Address 1", "\"9 Park Grove, Henleaze, Bristol. BS6 7XB\"")]
    [InlineData("Address 1,Address 2", "\"9 Park Grove, Henleaze, Bristol. BS6 7XB\",\"184B Henleaze Road, Henleaze, Bristol, BS9 4NE\"")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public void CanGenerateACsvDefinitionWhereCommasAreEscaped(string headers, string rows)
    {
        var definition = CsvDefinitionProvider.Get(headers.Split(','),
            rows.Split('|').Select(row => SplitWithEscape(row, ',', '"')).ToArray());
        var testInput = definition.GenerateString();

        var result = CsvGenerator.FromString(testInput);
        Assert.Equal(definition.Headers.Count, result.Headers.Count);
        Assert.Equal(definition.Rows.Count, result.Rows.Count);

        foreach (var header in definition.Headers)
            Assert.Contains(header, result.Headers);

        for (var i = 0; i < definition.Rows.Count; i++)
        {
            var definitionRow = definition.Rows[i];
            var resultRow = result.Rows[i];
            for (var ci = 0; ci < definitionRow.Count; ci++)
            {
                Assert.Equal(definitionRow[ci].ToString(), resultRow[ci].ToString());
            }
        }
    }

    [Theory]
    [InlineData("Header 1", "1")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,6")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,6|7,8,9|10,11,12")]
    [InlineData("Header 1,Header 2,Header 3", "Me,You,Cats Mother|Haddock,Fish,Rabbits")]
    [InlineData("Header 1,Header 2,Header 3", "1,2,3|4,5,Rabbits")]
    [InlineData("Header 1,Header 2,Header 3", "Me,You,You're Great!|Haddock,Fish,Rabbits")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public void CanGenerateAStringRepresentationOfACsvFile(string headers, string rows)
    {
        var definition = CsvDefinitionProvider.Get(headers.Split(','),
            rows.Split('|').Select(row => row.Split(',')).ToArray());
        var result = definition.GenerateString();

        Assert.Equal($"{headers}\n{rows.Replace("|","\n")}\n", result);
    }

    [Theory]
    [InlineData("Header 1")]
    [InlineData("Header 1,Header 2")]
    [InlineData("Header 1,Header 2,Header 3")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public void CanGenerateAStringRepresentationOfACsvFileWithNoRows(string headers)
    {
        var definition = CsvDefinitionProvider.Get(headers.Split(','));
        var result = definition.GenerateString();

        Assert.Equal($"{headers}\n", result);
    }

    [Fact]
    public void AttemptingToGenerateACsvFromAnInvalidDefinitionThrowsAnException()
    {
        var definition = CsvDefinitionProvider.GetValid();
        definition.Headers[1] = "WRONG";

        Assert.Throws<InvalidOperationException>(() => definition.GenerateString());
    }

    [Fact]
    public void WhenCallingGenerateStringDirectlyTheDefinitionParameterIsRequired()
        => Assert.Throws<ArgumentNullException>("definition", () => CsvGenerator.GenerateString(null!));

    [Fact]
    public void WhenCallingGenerateFileDirectlyTheDefinitionParameterIsRequired()
        => Assert.Throws<ArgumentNullException>("definition", () => CsvGenerator.GenerateFile(null!, "C:\\myfile.csv"));

    [Fact]
    public void WhenCallingGenerateBytesDirectlyTheDefinitionParameterIsRequired()
        => Assert.Throws<ArgumentNullException>("definition", () => CsvGenerator.GenerateBytes(null!));

    [Fact]
    public void WhenCallingGenerateFileThePathParameterIsRequired()
        => Assert.Throws<ArgumentNullException>("path", () => CsvDefinitionProvider.GetValid().GenerateFile(null!));

    [Fact]
    public void WhenCallingGenerateFileThePathParameterCannotBeEmpty()
        => Assert.Throws<ArgumentException>("path", () => CsvDefinitionProvider.GetValid().GenerateFile(string.Empty));

    #region | Supporting Methods |

    /// <summary>
    /// Splits the string taking into account escape characters.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="separator">The delimiter.</param>
    /// <param name="escapeCharacter">The escape character.</param>
    /// <returns>An array of strings.</returns>
    private static string[] SplitWithEscape(string source, char separator, char escapeCharacter)
    {
        var isEscaping = false;
        var retVal = new List<string>();
        var builder = new StringBuilder();

        foreach (var c in source)
        {
            if (c == escapeCharacter)
            {
                isEscaping = !isEscaping;
            }
            else if (c == separator && !isEscaping)
            {
                retVal.Add(builder.ToString());
                builder.Clear();
            }
            else
            {
                builder.Append(c);
            }
        }

        retVal.Add(builder.ToString());
        return retVal.ToArray();
    }

    #endregion
}