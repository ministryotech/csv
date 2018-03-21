using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Ministry.Csv
{
    /// <summary>
    /// Provides functional utilities for generating CSV files.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class CsvGenerator
    {
        private const char DELIM = ',';

        /// <summary>
        /// Creates a CSV Definition object from a byte array.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A CSV Definition</returns>
        public static CsvDefinition FromBytes(byte[] source)
            => FromString(Encoding.UTF8.GetString(source));

        /// <summary>
        /// Creates a CSV Definition object from a string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A CSV Definition</returns>
        public static CsvDefinition FromString(string source)
        {
            var lines = source.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
            var rows = lines.Skip(1).ToArray();

            return new CsvDefinition()
                .AddHeaders(headers)
                .AddRowsIfAny(rows.Select(r => SplitWithEscape(r, ',', '"')).ToArray());
        }

        /// <summary>
        /// Converts the file definition to a string.
        /// </summary>
        /// <returns></returns>
        public static string GenerateString(this CsvDefinition definition)
        {
            if (!definition.ThrowIfNull(nameof(definition)).Validate())
                throw new InvalidOperationException("Unable to generate CSV output as the definition provided is currently invalid.");

            return new StringBuilder()
                .AppendHeaders(definition)
                .AppendRows(definition)
                .ToString();
        }

        /// <summary>
        /// Exports the file definition to a file.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="path">The path of the target file.</param>
        public static void GenerateFile(this CsvDefinition definition, string path)
            => System.IO.File.WriteAllText(path.ThrowIfNullOrEmpty(nameof(path)), GenerateString(definition));

        /// <summary>
        /// Exports the file definition to a byte array.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns>A byte array representing the file, ideal for downloading.</returns>
        public static byte[] GenerateBytes(this CsvDefinition definition)
            => Encoding.UTF8.GetBytes(GenerateString(definition));

        #region | Private Methods |

        /// <summary>
        /// Appends the headers to the builder.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="definition">The definition.</param>
        /// <returns>The mutated string builder.</returns>
        private static StringBuilder AppendHeaders(this StringBuilder sb, CsvDefinition definition)
        {
            foreach (var header in definition.Headers)
                sb.Append(header)
                    .Append(DELIM);

            return sb.AppendLineEnd();
        }

        /// <summary>
        /// Appends the line end.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <returns>The string builder with a line ender and any extraneous delimiters removed.</returns>
        private static StringBuilder AppendLineEnd(this StringBuilder sb)
            => sb.Remove(sb.ToString().LastIndexOf(DELIM), 1)
                .AppendLine();

        /// <summary>
        /// Appends the rows to the builder.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="definition">The definition.</param>
        /// <returns>The mutated string builder.</returns>
        private static StringBuilder AppendRows(this StringBuilder sb, CsvDefinition definition)
        {
            foreach (var row in definition.Rows)
                sb.AppendRow(definition, row);

            return sb;
        }

        /// <summary>
        /// Appends a row to the builder.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="row">The row.</param>
        /// <returns>The mutated string builder.</returns>
        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        private static StringBuilder AppendRow(this StringBuilder sb, CsvDefinition definition, CsvRow row)
        {
            foreach (var header in definition.Headers)
            {
                var cell = row.SingleOrDefault(c => c.Header == header)?.Value ?? string.Empty;
                sb.Append(Encode(cell)).Append(DELIM);
            }

            return sb.AppendLineEnd();
        }

        /// <summary>
        /// Encodes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "CanBeReplacedWithTryCastAndCheckForNull")]
        private static string Encode(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is DateTime datetime)
                return datetime.ToString("dd/MM/yyyy");

            var result = value.ToString();
            if (result.Contains(DELIM) || result.Contains("\""))
                result = '"' + result.Replace("\"", "\"\"") + '"';

            return result;
        }        
        
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
}
