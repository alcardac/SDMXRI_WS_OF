// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITabularWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The interface for tabular Data Writers, such as flat files like CSV or a database table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.TabularWriters.Engine
{
    /// <summary>
    ///     The interface for tabular Data Writers, such as flat files like CSV or a database table.
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="ITabularWriter" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingBinaryTabularWriter.cs" lang="cs" />
    /// </example>
    public interface ITabularWriter
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Total number of rows written
        /// </summary>
        long TotalRecordsWritten { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Close this writer and commit the changes.
        /// </summary>
        void Close();

        /// <summary>
        ///     Start of the column definition. This could be the CSV header or a database table.
        /// </summary>
        void StartColumns();

        /// <summary>
        ///     Start of a record. This could be a new line in a CSV file or a new record in a database table.
        /// </summary>
        void StartRecord();

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="WriteCellAttributeValue"/> was called
        /// </remarks>
        void WriteCellAttributeValue(string value);

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="WriteCellKeyValue"/> was called
        /// </remarks>
        void WriteCellKeyValue(string value);

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="WriteCellMeasureValue"/> was called
        /// </remarks>
        void WriteCellMeasureValue(string value);

        /// <summary>
        /// Write the specified <paramref name="attribute"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="attribute">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="attribute"/> depends on the order <see cref="WriteColumnAttribute"/> was called
        /// </remarks>
        void WriteColumnAttribute(string attribute);

        /// <summary>
        /// Write the specified <paramref name="key"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="key">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="key"/> depends on the order <see cref="WriteColumnKey"/> was called
        /// </remarks>
        void WriteColumnKey(string key);

        /// <summary>
        /// Write the specified <paramref name="measure"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="measure">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="measure"/> depends on the order <see cref="WriteColumnMeasure"/> was called
        /// </remarks>
        void WriteColumnMeasure(string measure);

        #endregion
    }
}