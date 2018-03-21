using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Ministry.Csv
{
    /// <summary>
    /// Defines the contents of a row in a CSV output.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class CsvCell
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvCell"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="value">The value.</param>
        [JsonConstructor]
        public CsvCell(string header, string value)
        {
            Header = header;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvCell"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="value">The value.</param>
        public CsvCell(string header, object value)
            : this(header, value.ToString())
        { }

        /// <summary>
        /// Initializes an empty instance of the <see cref="CsvCell"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        public CsvCell(string header)
        {
            Header = header;
        }

        #endregion

        /// <summary>
        /// The header that the value corresponds to.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether this cell is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this cell is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

        public override string ToString() => $"[{Header}] {Value}";
    }
}
