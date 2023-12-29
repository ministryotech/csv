namespace Ministry.Csv;

/// <summary>
/// Defines the contents of a row in a CSV output.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
public class CsvRow : List<CsvCell>
{
    #region | Construction |

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvRow"/> class.
    /// </summary>
    public CsvRow()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvRow"/> class.
    /// </summary>
    /// <param name="cells">The cells.</param>
    public CsvRow(IEnumerable<CsvCell> cells)
        : base(cells)
    { }

    #endregion

    /// <summary>
    /// Gets a value indicating whether this row only consists of empty cells.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this row only consists of empty cells; otherwise, <c>false</c>.
    /// </value>
    public bool IsEmpty => this.All(item => item.IsEmpty);

    /// <summary>
    /// Gets a given cell by the specified header.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <returns>A cell.</returns>
    public CsvCell Cell(string header) => this.FirstOrDefault(item =>
        string.Equals(item.Header, header, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Converts the object to a string representation.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString()
        => this.Select(item => item.ToString()).Aggregate((a, b) => $"{a}, {b}");
}