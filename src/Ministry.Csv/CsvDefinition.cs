using Ministry.Compositions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ministry.Csv
{
    /// <summary>
    /// Defines the structure of a CSV file.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CsvDefinition
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDefinition"/> class.
        /// </summary>
        public CsvDefinition()
        {
            Headers = new List<string>();
            Rows = new List<CsvRow>();
        }

        #endregion

        /// <summary>
        /// Gets the headers.
        /// </summary>
        public IList<string> Headers { get; }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        public IList<CsvRow> Rows { get; }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public CsvDefinition AddHeaders(params string[] headers)
        {
            headers.ThrowIf(h => !h.Any(), nameof(headers), "At least one item must be provided.");
            Headers.AddItems(headers);
            return this;
        }

        /// <summary>
        /// Adds a row
        /// </summary>
        /// <param name="rowValues">The value of items for the cells. These must be provided for all headers.</param>
        public CsvDefinition AddRow(params string[] rowValues)
        {
            rowValues.ThrowIf(rv => !rv.Any(), nameof(rowValues), "At least one item must be provided.");

            if (rowValues.Length != Headers.Count)
                throw new InvalidOperationException("The number of headers and rowValues provided must match.");
            
            Rows.Add(new CsvRow(rowValues.Select((val, headerIndex)
                => new CsvCell(Headers[headerIndex], val), 0)));
            return this;
        }

        /// <summary>
        /// Adds a row
        /// </summary>
        /// <param name="rowValues">The value of items for the cells. These must be provided for all headers.</param>
        public CsvDefinition AddRow(params object[] rowValues)
            => AddRow(rowValues.Select(rv => rv.ToString()).ToArray());

        /// <summary>
        /// Adds multiple rows at once if there are any in the provided source(s).
        /// </summary>
        /// <param name="rows">The rows.</param>
        public CsvDefinition AddRowsIfAny(params string[][] rows)
        {
            foreach (var row in rows)
                AddRow(row);

            return this;
        }

        /// <summary>
        /// Adds multiple rows at once if there are any in the provided source(s).
        /// </summary>
        /// <param name="rows">The rows.</param>
        public CsvDefinition AddRowsIfAny(params CsvRow[] rows)
        {
            foreach (var row in rows)
                AddRow(row.Select(c => c.Value).ToArray());

            return this;
        }

        /// <summary>
        /// Adds multiple rows at once.
        /// </summary>
        /// <param name="rows">The rows.</param>
        public CsvDefinition AddRows(params string[][] rows)
            => AddRowsIfAny(rows.ThrowIf(r => !r.Any(), nameof(rows), "At least one item must be provided."));

        /// <summary>
        /// Adds multiple rows at once from an existing timetable.
        /// </summary>
        /// <param name="rows">The rows.</param>
        public CsvDefinition AddRows(params CsvRow[] rows)
            => AddRowsIfAny(rows.ThrowIf(r => !r.Any(), nameof(rows), "At least one item must be provided."));

        /// <summary>
        /// Validates this definition.
        /// </summary>
        /// <returns>A flag indicating if the definition is valid.</returns>
        public bool Validate()
            => !Rows.Any(r => r.Any(c => !Headers.Contains(c.Header)));

        public override string ToString()
            => Headers.Aggregate((a, b) => $"{a}, {b}") + 
                " | " +
                Rows.Select(item => item.ToString()).Aggregate((a, b) => $"{a}, {b}");
    }
}
