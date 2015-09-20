// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryTabularWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A fast binary tabular writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.TabularWriters.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     A fast binary tabular writer.
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="BinaryTabularWriter" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingBinaryTabularWriter.cs" lang="cs" />
    /// </example>
    public class BinaryTabularWriter : ITabularWriter, IDisposable
    {
        #region Fields

        /// <summary>
        ///     The column list
        /// </summary>
        private readonly List<string> _columns = new List<string>();

        /// <summary>
        ///     The data file
        /// </summary>
        private readonly BinaryWriter _dataFile;

        /// <summary>
        ///     The index of the <see cref="_dataFile" /> . It holds the start position of each 1024 record page.
        /// </summary>
        private readonly IList<long> _index;

        /// <summary>
        ///     A value indicating whether it is closed
        /// </summary>
        private bool _closed;

        /// <summary>
        ///     A value indicating whether a <see cref="StartRecord" /> has been called
        /// </summary>
        private bool _dataStarted;

        /// <summary>
        ///     The number of records written
        /// </summary>
        private long _totalRecordsWritten;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTabularWriter"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file
        /// </param>
        /// <param name="index">
        /// The <paramref name="dataFile"/> index. It holds the start position of each 1024 record page.
        /// </param>
        public BinaryTabularWriter(BinaryWriter dataFile, IList<long> index)
        {
            if (dataFile == null)
            {
                throw new ArgumentNullException("dataFile");
            }

            if (index == null)
            {
                throw new ArgumentNullException("index");
            }

            this._dataFile = dataFile;
            this._index = index;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the Total number of rows written
        /// </summary>
        public long TotalRecordsWritten
        {
            get
            {
                return this._totalRecordsWritten;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Close this writer and commit the changes.
        /// </summary>
        public void Close()
        {
            if (!this._closed)
            {
                this._closed = true;
                this._dataFile.Flush();
                this._index.Add(this._dataFile.BaseStream.Position);
                this._dataFile.Close();
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Start of the column definition. This could be the CSV header or a database table.
        /// </summary>
        public void StartColumns()
        {
            if (this._closed)
            {
                throw new InvalidOperationException("Writer was closed");
            }

            if (this._dataStarted)
            {
                throw new InvalidOperationException("StartRecord was called");
            }
        }

        /// <summary>
        ///     Start of a record. This could be a new line in a CSV file or a new record in a database table.
        /// </summary>
        public void StartRecord()
        {
            if (this._closed)
            {
                throw new InvalidOperationException("Writer was closed");
            }

            if (this._totalRecordsWritten % 1024 == 0)
            {
                this._index.Add(this._dataFile.BaseStream.Position);
                this._dataStarted = true;
            }

            this._totalRecordsWritten++;
        }

        /// <summary>
        /// Write cell <paramref name="value"/> .
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteCellAttributeValue(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteCellAttributeValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write cell <paramref name="value"/> .
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteCellKeyValue(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteCellKeyValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write cell <paramref name="value"/> .
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteCellMeasureValue(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteCellMeasureValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write the specified <paramref name="attribute"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="attribute">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="attribute"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteColumnAttribute(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteColumnAttribute(string attribute)
        {
            this.WriteColumn(attribute);
        }

        /// <summary>
        /// Write the specified <paramref name="key"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="key">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="key"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteColumnKey(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteColumnKey(string key)
        {
            this.WriteColumn(key);
        }

        /// <summary>
        /// Write the specified <paramref name="measure"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="measure">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="measure"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteColumnMeasure(System.String)"/>
        ///     was called
        /// </remarks>
        public void WriteColumnMeasure(string measure)
        {
            this.WriteColumn(measure);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether to dispose managed resources
        /// </param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Write cell <paramref name="value"/> .
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteCellAttributeValue(System.String)"/>
        ///     was called
        /// </remarks>
        private void WriteCell(string value)
        {
            this._dataFile.Write(value);
        }

        /// <summary>
        /// Write the specified <paramref name="key"/> column. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="key">
        /// The column name. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="key"/> depends on the order
        ///     <see cref="M:Estat.Sri.TabularWriters.Engine.ITabularWriter.WriteColumnKey(System.String)"/>
        ///     was called
        /// </remarks>
        private void WriteColumn(string key)
        {
            this._columns.Add(key);
            this._dataFile.Write(key);
        }

        #endregion
    }
}