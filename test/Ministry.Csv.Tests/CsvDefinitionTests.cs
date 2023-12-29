namespace Ministry.Csv.Tests;

[Trait("Category", "Csv")]
public class CsvDefinitionTests
{
    [Fact]
    public void CanCreateACsvDefinition()
    {
            var objUt = new CsvDefinition();
            
            Assert.Empty(objUt.Headers);
            Assert.Empty(objUt.Rows);
        }

    [Fact]
    public void CanValidateACsvDefinition()
    {
            var objUt = new CsvDefinition();
            objUt.Headers.AddItems("Column 1", "Column 2", "TOTAL");
            objUt.Rows.AddItems(
                new CsvRow
                {
                    new("Column 1", 10),
                    new("Column 2", 20),
                    new("TOTAL", 30)
                },
                new CsvRow
                {
                    new("Column 1", 11),
                    new("Column 2", 21),
                    new("TOTAL", 31)
                });

            Assert.True(objUt.Validate());
        }

    [Fact]
    public void ACsvDefinitionWithACellForANonExistentHeaderIsInvalid()
    {
            var objUt = new CsvDefinition();
            objUt.Headers.AddItems("Column 1", "Column 2", "TOTAL");
            objUt.Rows.AddItems(
                new CsvRow
                {
                    new("Column 1", 10),
                    new("Column 2", 20),
                    new("TOTAL", 30)
                },
                new CsvRow
                {
                    new("Column 1", 11),
                    new("Column 2", 21),
                    new("Column 3", 31)
                });

            Assert.False(objUt.Validate());
        }

    [Fact]
    public void CanBuildACsvDefinitionByAddingHeadersDirectly()
    {
            var objUt = new CsvDefinition();
            objUt.AddHeaders("Column 1", "Column 2", "TOTAL");

            Assert.NotEmpty(objUt.Headers);
            Assert.Equal(3, objUt.Headers.Count);
        }

    [Fact]
    public void CanBuildACsvDefinitionByAddingARowDirectly()
    {
            var objUt = new CsvDefinition();
            objUt.AddHeaders("Column 1", "Column 2", "TOTAL");
            objUt.AddRow(10, 20, 30);
            objUt.AddRow(11, 21, 31);

            Assert.NotEmpty(objUt.Rows);
            Assert.Equal(2, objUt.Rows.Count);

            Assert.Equal(objUt.Headers[0], objUt.Rows[0][0].Header);
            Assert.Equal(objUt.Headers[1], objUt.Rows[0][1].Header);
            Assert.Equal(objUt.Headers[2], objUt.Rows[0][2].Header);
            Assert.Equal(objUt.Headers[0], objUt.Rows[1][0].Header);
            Assert.Equal(objUt.Headers[1], objUt.Rows[1][1].Header);
            Assert.Equal(objUt.Headers[2], objUt.Rows[1][2].Header);
        }

    [Fact]
    public void WhenAddingHeadersTheHeadersAreRequired()
    {
            var objUt = new CsvDefinition();
            Assert.Throws<ArgumentException>("headers", () => objUt.AddHeaders());
        }

    [Fact]
    public void AddingARowWhereTheTotalNumberOfRowsIsLessThanTheNumberOfHeadersThrowsAnInvalidOperationException()
    {
            var objUt = new CsvDefinition();
            objUt.AddHeaders("Column 1", "Column 2", "TOTAL");
            Assert.Throws<InvalidOperationException>(() => objUt.AddRow(10, 20));
        }

    [Fact]
    public void AddingARowWhereTheTotalNumberOfRowsIsGreaterThanTheNumberOfHeadersThrowsAnInvalidOperationException()
    {
            var objUt = new CsvDefinition();
            objUt.AddHeaders("Column 1", "Column 2", "TOTAL");
            Assert.Throws<InvalidOperationException>(() => objUt.AddRow(10, 20, 30, 40));
        }

    [Fact]
    public void AddHeadersAndAddRowReturnsTheCsvDefinitionToEnableChaining()
    {
            var objUt = new CsvDefinition()
                .AddHeaders("Column 1", "Column 2", "TOTAL")
                .AddRow(10, 20, 30)
                .AddRow(11, 21, 31);

            Assert.NotEmpty(objUt.Rows);
            Assert.Equal(2, objUt.Rows.Count);
        }
}