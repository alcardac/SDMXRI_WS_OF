// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqliteTabularWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   <c>Sqlite </c> data writer
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.TabularWriters.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    ///     <c>Sqlite </c> data writer
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="SqliteTabularWriter" /> (Depends on <c>System.Data.Sqlite</c>)
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingSqliteTabularWriter.cs" lang="cs" />
    /// </example>
    public class SqliteTabularWriter : ITabularWriter, IDisposable
    {
        #region Constants

        /// <summary>
        ///     The create table format template. 1st parameter is the table name and second the columns definition
        /// </summary>
        private const string CreateTableFormat2 = "CREATE TABLE {0} ( {1} )";

        /// <summary>
        ///     The data type used for all column definition
        /// </summary>
        private const string DefaultDataType = " TEXT";

        /// <summary>
        ///     The <see cref="DefaultDataType" /> followed by a comma
        /// </summary>
        private const string DefaultDataTypeComma = DefaultDataType + ",";

        /// <summary>
        ///     The insert into format template. 1st parameter is the table name, second the fields and 3rd the value parameters.
        /// </summary>
        private const string InsertIntoFormat3 = "INSERT INTO {0} ({1}) VALUES ({2})";

        #endregion

        #region Fields

        /// <summary>
        ///     A value indicating whether to close the connection.
        /// </summary>
        private readonly bool _closeConnection;

        /// <summary>
        ///     The _connection.
        /// </summary>
        private readonly IDbConnection _connection;

        /// <summary>
        ///     The _field list.
        /// </summary>
        private readonly List<string> _fieldList = new List<string>();

        /// <summary>
        ///     The _table.
        /// </summary>
        private readonly string _table;

        /// <summary>
        ///     A value indicating whether this writer was closed
        /// </summary>
        private bool _closed;

        /// <summary>
        ///     The _command.
        /// </summary>
        private IDbCommand _command;

        /// <summary>
        ///     The <see cref="_command" /> parameter index
        /// </summary>
        private int _parameterIndex;

        /// <summary>
        ///     A value indicating whether the header has started
        /// </summary>
        private bool _startHeader;

        /// <summary>
        ///     A value indicating whether the data has started
        /// </summary>
        private bool _startedData;

        /// <summary>
        ///     A value indicating whether a record has started
        /// </summary>
        private bool _startedRecord;

        /// <summary>
        ///     the Total number of rows written
        /// </summary>
        private long _totalRecordsWritten;

        /// <summary>
        ///     The _transaction.
        /// </summary>
        private IDbTransaction _transaction;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteTabularWriter"/> class.
        /// </summary>
        /// <param name="connection">
        /// The connection to the <c>Sqlite</c> database.
        /// </param>
        /// <param name="table">
        /// The name of the table that will be populated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection"/> is null
        ///     -or-
        ///     <paramref name="table"/> is null
        /// </exception>
        public SqliteTabularWriter(IDbConnection connection, string table)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            this._connection = connection;
            this._table = table;

            if ((this._connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                this._connection.Open();
                this._closeConnection = true;
            }

            this._transaction = connection.BeginTransaction();
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
            if (this._closed)
            {
                return;
            }

            this._closed = true;
            this.EndHeader();

            if (this._command != null)
            {
                if (this._startedData)
                {
                    this.EndRecord();
                    this.Commit();
                }

                this._command.Dispose();
                this._command = null;
            }

            if (this._closeConnection)
            {
                this._connection.Close();
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
        ///     Start of the header. This could be the CSV header or a database table.
        /// </summary>
        public void StartColumns()
        {
            this.CheckIfClosed();
            if (this._startHeader)
            {
                throw new InvalidOperationException("Header already started.");
            }

            if (this._startedData)
            {
                throw new InvalidOperationException("Data entry has started.");
            }

            this._startHeader = true;
        }

        /// <summary>
        ///     Start of a record. This could be a new line in a CSV file or a new record in a database table.
        /// </summary>
        public void StartRecord()
        {
            this.CheckIfClosed();
            this.EndHeader();
            this.EndRecord();
            this._totalRecordsWritten++;
            this._startedRecord = true;
            this._startedData = true;
            this._parameterIndex = 0;
        }

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order
        ///     <see cref="ITabularWriter.WriteCellAttributeValue"/>
        ///     was called
        /// </remarks>
        public void WriteCellAttributeValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="ITabularWriter.WriteCellKeyValue"/> was called
        /// </remarks>
        public void WriteCellKeyValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="ITabularWriter.WriteCellMeasureValue"/> was called
        /// </remarks>
        public void WriteCellMeasureValue(string value)
        {
            this.WriteCell(value);
        }

        /// <summary>
        /// Write the specified <paramref name="attribute"/> header value. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="attribute">
        /// The header value. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="attribute"/> depends on the order
        ///     <see cref="ITabularWriter.WriteColumnAttribute"/>
        ///     was called
        /// </remarks>
        public void WriteColumnAttribute(string attribute)
        {
            this.WriteHeader(attribute);
        }

        /// <summary>
        /// Write the specified <paramref name="key"/> header value. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="key">
        /// The header value. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="key"/> depends on the order <see cref="ITabularWriter.WriteColumnKey"/> was called
        /// </remarks>
        public void WriteColumnKey(string key)
        {
            this.WriteHeader(key);
        }

        /// <summary>
        /// Write the specified <paramref name="measure"/> header value. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="measure">
        /// The header value. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="measure"/> depends on the order <see cref="ITabularWriter.WriteColumnMeasure"/> was called
        /// </remarks>
        public void WriteColumnMeasure(string measure)
        {
            this.WriteHeader(measure);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether managed resources should be disposed
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }

        /// <summary>
        ///     Check if <see cref="SqliteTabularWriter" /> was closed and throw an exception if it is
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="SqliteTabularWriter" /> is closed
        /// </exception>
        private void CheckIfClosed()
        {
            if (this._closed)
            {
                throw new InvalidOperationException("SqliteTabularWriter is closed.");
            }
        }

        /// <summary>
        ///     Commit the <see cref="_transaction" /> if not null
        /// </summary>
        private void Commit()
        {
            if (this._transaction != null)
            {
                this._transaction.Commit();
                this._transaction = null;
            }
        }

        /// <summary>
        /// Create a <see cref="IDbCommand"/> using the specified <paramref name="sql"/> from <see cref="_connection"/> with
        ///     <see cref="_transaction"/>
        /// </summary>
        /// <param name="sql">
        /// The <c>SQL</c> statement.
        /// </param>
        /// <returns>
        /// A <see cref="IDbCommand"/> using the specified <paramref name="sql"/> from <see cref="_connection"/> with
        ///     <see cref="_transaction"/>
        /// </returns>
        private IDbCommand CreateCommand(string sql)
        {
            if (this._transaction == null)
            {
                this._transaction = this._connection.BeginTransaction();
            }

            IDbCommand command = this._connection.CreateCommand();
            command.Transaction = this._transaction;
            command.CommandText = sql;
            return command;
        }

        /// <summary>
        ///     Create the <see cref="_command" /> for inserting data
        /// </summary>
        private void CreateInsertCommand()
        {
            var paramList = new List<string>(this._fieldList.Count);

            foreach (string field in this._fieldList)
            {
                paramList.Add("@" + field);
            }

            string insert = string.Format(
                CultureInfo.InvariantCulture, 
                InsertIntoFormat3, 
                this._table, 
                string.Join(",", this._fieldList.ToArray()), 
                string.Join(",", paramList.ToArray()));

            this._command = this.CreateCommand(insert);
            foreach (string parameterName in paramList)
            {
                IDbDataParameter parameter = this._command.CreateParameter();
                parameter.ParameterName = parameterName;
                this._command.Parameters.Add(parameter);
            }

            this._command.Prepare();
        }

        /// <summary>
        ///     Create the <see cref="_table" /> with the columns from <see cref="_fieldList" />
        /// </summary>
        private void CreateTable()
        {
            if (this._fieldList.Count <= 0)
            {
                return;
            }

            string columnDefinition = string.Join(DefaultDataTypeComma, this._fieldList.ToArray()) + DefaultDataType;

            string createTable = string.Format(
                CultureInfo.InvariantCulture, CreateTableFormat2, this._table, columnDefinition);
            using (IDbCommand command = this.CreateCommand(createTable))
            {
                command.ExecuteNonQuery();
            }

            this.Commit();
        }

        /// <summary>
        ///     The end header.
        /// </summary>
        private void EndHeader()
        {
            if (!this._startedData)
            {
                this.CreateTable();
                this.CreateInsertCommand();
            }
        }

        /// <summary>
        ///     Execute the <see cref="_command" />
        /// </summary>
        private void EndRecord()
        {
            if (!this._startedRecord)
            {
                return;
            }

            if (this._parameterIndex != this._fieldList.Count)
            {
                throw new InvalidOperationException("The number of WriteCell doesn't match the number of WriteHeader");
            }

            this._command.ExecuteNonQuery();
            this._startedRecord = false;
        }

        /// <summary>
        /// Write cell <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The cell value
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="value"/> depends on the order <see cref="WriteCell"/> was called
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// StartRecord was not called
        ///     -or-
        ///     Cannot write more cells than header columns
        /// </exception>
        private void WriteCell(string value)
        {
            this.CheckIfClosed();
            if (!this._startedRecord)
            {
                throw new InvalidOperationException("StartRecord was not called");
            }

            if (this._parameterIndex >= this._fieldList.Count)
            {
                throw new InvalidOperationException("Cannot write more cells than header columns");
            }

            ((DbParameter)this._command.Parameters[this._parameterIndex]).Value = value;
            this._parameterIndex++;
        }

        /// <summary>
        /// Write the specified <paramref name="header"/> header value. This could be the CSV header value or a database field.
        /// </summary>
        /// <param name="header">
        /// The header value. i.e. the component name
        /// </param>
        /// <remarks>
        /// The ordinal of each <paramref name="header"/> depends on the order <see cref="WriteHeader"/> was called
        /// </remarks>
        private void WriteHeader(string header)
        {
            this.CheckIfClosed();
            if (!this._startHeader)
            {
                throw new InvalidOperationException("Header not started.");
            }

            if (this._startedData)
            {
                throw new InvalidOperationException("Data entry has started.");
            }

            this._fieldList.Add(header);
        }

        #endregion
    }
}