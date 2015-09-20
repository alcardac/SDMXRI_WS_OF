// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Estat.Sri.MappingStoreRetrieval.Config;

    using log4net;

    /// <summary>
    ///     The database.
    /// </summary>
    public class Database
    {
        #region Static Fields

        /// <summary>
        ///     The database setting collection.
        /// </summary>
        private static readonly DatabaseSettingCollection _databaseSettingCollection = ConfigManager.Config.GeneralDatabaseSettings;

        /// <summary>
        ///     The regular expression that removes the <c>dbo.</c> from a string.
        /// </summary>
        private static readonly Regex _dboRemover = new Regex(@"\bdbo\.", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(Database));

        /// <summary>
        ///     The parameter markers per provider.
        /// </summary>
        /// <remarks>
        ///     <c>System.Data.SqlClient</c> is hardcoded because of a bug in <c>System.Data.SqlClient</c>
        /// </remarks>
        private static readonly ConcurrentDictionary<string, string> _parameterMarkersPerProvider = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        #endregion

        #region Fields

        /// <summary>
        ///     The mapping store connection string settings.
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        ///     The internal database helper class
        /// </summary>
        private readonly IDatabaseInternal _databaseInternal;

        /// <summary>
        ///     The factory to create connections.
        /// </summary>
        private readonly DbProviderFactory _factory;

        /// <summary>
        ///     The _provider name.
        /// </summary>
        private readonly string _providerName;

        /// <summary>
        ///     The method that normalizes the schema in query.
        /// </summary>
        private readonly Func<string, string> _schemaNormalizer;

        /// <summary>
        ///     The parameter marker format.
        /// </summary>
        private string _parameterMarkerFormat;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        public Database(Database database, DbTransaction transaction)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            // copy stuff from specified database
            this._providerName = database._providerName;
            this._connectionStringSettings = database._connectionStringSettings;
            this._factory = database._factory;
            this._schemaNormalizer = this.GetQueryNormalizer();
            this._parameterMarkerFormat = database._parameterMarkerFormat;

            this._databaseInternal = new DatabaseTransactional(transaction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The mapping store connection string settings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionStringSettings"/> is null
        /// </exception>
        public Database(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._connectionStringSettings = connectionStringSettings;

            this._providerName = connectionStringSettings.ProviderName;

            try
            {
                this._factory = DbProviderFactories.GetFactory(this._providerName);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error while trying to load provider : {0}", this._providerName);
                _log.Error(this._providerName, e);
                throw;
            }

            this._schemaNormalizer = this.GetQueryNormalizer();
            this._databaseInternal = new DatabaseNonTransactional(connectionStringSettings, this._factory);
        }

        #endregion

        #region Interfaces

        /// <summary>
        /// The DatabaseInternal interface.
        /// </summary>
        private interface IDatabaseInternal
        {
            #region Public Methods and Operators

            /// <summary>
            /// Executes the non query command
            /// </summary>
            /// <param name="command">The command.</param>
            /// <returns>The number of affected records.</returns>
            int ExecuteNonQuery(DbCommand command);

            /// <summary>
            /// Executes and return the <see cref="IDataReader"/> from the <paramref name="command"/>
            /// </summary>
            /// <param name="command">The command.</param>
            /// <returns>The executed reader.</returns>
            IDataReader ExecuteReader(IDbCommand command);

            /// <summary>
            /// Executes the scalar command.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <returns>The result of the <see cref="DbCommand.ExecuteScalar"/></returns>
            object ExecuteScalar(DbCommand command);

            /// <summary>
            /// Gets the connection.
            /// </summary>
            /// <returns>The connection instance</returns>
            DbConnection GetConnection();

            /// <summary>
            /// Opens the connection.
            /// </summary>
            /// <param name="connection">The connection.</param>
            void OpenConnection(IDbConnection connection);

            /// <summary>
            /// Setup  the command.
            /// </summary>
            /// <param name="command">The command.</param>
            void SetupCommand(IDbCommand command);

            #endregion
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the provider name.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return this._providerName;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Return an <see cref="IDataReader"/>. It will open the <paramref name="connection"/> if it is closed.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        public static IDataReader ExecuteReader(DbConnection connection, DbCommand command)
        {
            if ((connection.State & ConnectionState.Open) != ConnectionState.Open)
            {
                connection.Open();
            }

            command.Connection = connection;
            return command.ExecuteReader(CommandBehavior.Default);
        }

        /// <summary>
        /// Create and add an input <see cref="DbParameter"/> to the specified <paramref name="command"/>
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="name">
        /// The parameter name.
        /// </param>
        /// <param name="dbType">
        /// The parameter type.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            DbParameter dbParameter = this.CreateInParameter(name, dbType);
            if (dbParameter != null)
            {
                dbParameter.Value = value ?? DBNull.Value;
                command.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// Build the parameter name using the DB vendor specific format.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The parameter name using the DB vendor specific format.
        /// </returns>
        public string BuildParameterName(string name)
        {
            this.RetrieveMarkerFormat();

            return string.Format(CultureInfo.InvariantCulture, this._parameterMarkerFormat, name);
        }

        /// <summary>
        /// Returns the SQL in a string with the <paramref name="parameters"/> names applied to the
        ///     <paramref name="queryFormat"/>.
        /// </summary>
        /// <param name="queryFormat">
        /// The query format.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The SQL in a string with the <paramref name="parameters"/> names applied to the <paramref name="queryFormat"/>.
        /// </returns>
        public string BuildQuery(string queryFormat, params DbParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return queryFormat;
            }

            var names = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var dbParameter = parameters[i];
                names[i] = dbParameter.ParameterName;
            }

            var query = string.Format(CultureInfo.InvariantCulture, queryFormat, names);
            return query;
        }

        /// <summary>
        /// Create command of <paramref name="commandType"/> with the specified <paramref name="commandText"/>
        /// </summary>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand CreateCommand(CommandType commandType, string commandText)
        {
            DbCommand cmd = null;
            try
            {
                cmd = this._factory.CreateCommand();
                if (cmd != null)
                {
                    cmd.CommandType = commandType;
                    commandText = this.NormalizeQuerySchema(commandText);
                    cmd.CommandText = commandText;
                    this._databaseInternal.SetupCommand(cmd);
                    this.SetupProviderSpecific(cmd);
                }
            }
            catch
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                throw;
            }

            return cmd;
        }

        /// <summary>
        ///     Creates connection for the database
        /// </summary>
        /// <returns>
        ///     The <see cref="DbConnection" />.
        /// </returns>
        public DbConnection CreateConnection()
        {
            return this._databaseInternal.GetConnection();
        }

        /// <summary>
        /// Create an input <see cref="DbParameter"/> and return it
        /// </summary>
        /// <param name="name">
        /// The parameter name.
        /// </param>
        /// <param name="dbType">
        /// The parameter type.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        public DbParameter CreateInParameter(string name, DbType dbType)
        {
            DbParameter dbParameter = this._factory.CreateParameter();
            if (dbParameter != null)
            {
                dbParameter.DbType = dbType;
                dbParameter.Direction = ParameterDirection.Input;
                dbParameter.ParameterName = this.BuildParameterName(name);
            }

            return dbParameter;
        }

        /// <summary>
        /// Create a <see cref="DbParameter"/> and return it
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="dbType">
        /// The DB type of the parameter.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        public DbParameter CreateInParameter(string name, DbType dbType, object value)
        {
            DbParameter parameter = this.CreateInParameter(name, dbType);
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Executes the specified <paramref name="sqlStatement"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="sqlStatement">
        /// The SQL statement to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="sqlStatement"/>
        /// </returns>
        public int ExecuteNonQuery(string sqlStatement, IList<DbParameter> parameters)
        {
            try
            {
                using (var command = this.GetSqlStringCommand(sqlStatement, parameters))
                {
                    return this._databaseInternal.ExecuteNonQuery(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="sqlStatement"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="sqlStatement">
        /// The SQL statement to execute
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="sqlStatement"/>
        /// </returns>
        public int ExecuteNonQuery(string sqlStatement, DbTransaction transaction, IList<DbParameter> parameters)
        {
            try
            {
                IDatabaseInternal databaseInternal = new DatabaseTransactional(transaction);
                using (var command = this.GetSqlStringCommand(sqlStatement, parameters))
                {
                    databaseInternal.SetupCommand(command);
                    return databaseInternal.ExecuteNonQuery(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="sqlStatement"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="sqlStatement">
        /// The SQL statement to execute
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="sqlStatement"/>
        /// </returns>
        public int ExecuteNonQueryFormat(string sqlStatement, DbTransaction transaction, params DbParameter[] parameters)
        {
            try
            {
                IDatabaseInternal databaseInternal = new DatabaseTransactional(transaction);
                using (var command = this.GetSqlStringCommandFormat(sqlStatement, parameters))
                {
                    databaseInternal.SetupCommand(command);
                    return databaseInternal.ExecuteNonQuery(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="sqlStatement"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="sqlStatement">
        /// The SQL statement to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="sqlStatement"/>
        /// </returns>
        public int ExecuteNonQueryFormat(string sqlStatement, params DbParameter[] parameters)
        {
            try
            {
                using (var command = this.GetSqlStringCommandFormat(sqlStatement, parameters))
                {
                    this._databaseInternal.SetupCommand(command);
                    return this._databaseInternal.ExecuteNonQuery(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Return an <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReader"/>.
        /// </returns>
        public IDataReader ExecuteReader(DbCommand command)
        {
            DbConnection connection = null;
            try
            {
                connection = this.CreateConnection();
                this._databaseInternal.OpenConnection(connection);
                command.Connection = connection;
                this._databaseInternal.SetupCommand(command);
            }
            catch (Exception e)
            {
                if (connection != null)
                {
                    connection.Dispose();
                }

                _log.Error("While ExecuteReader", e);

                throw;
            }

            return this._databaseInternal.ExecuteReader(command);
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The SQL query to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="query"/>
        /// </returns>
        public object ExecuteScalar(string query, IList<DbParameter> parameters)
        {
            try
            {
                using (var command = this.GetSqlStringCommand(query, parameters))
                {
                    return this._databaseInternal.ExecuteScalar(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteScalar", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The SQL query to execute
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="query"/>
        /// </returns>
        public object ExecuteScalar(string query, DbTransaction transaction, IList<DbParameter> parameters)
        {
            try
            {
                IDatabaseInternal databaseInternal = new DatabaseTransactional(transaction);
                using (var command = this.GetSqlStringCommand(query, parameters))
                {
                    databaseInternal.SetupCommand(command);
                    return databaseInternal.ExecuteScalar(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The SQL query to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="query"/>
        /// </returns>
        public object ExecuteScalarFormat(string query, params DbParameter[] parameters)
        {
            try
            {
                using (var command = this.GetSqlStringCommandFormat(query, parameters))
                {
                    return this._databaseInternal.ExecuteScalar(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteScalar", e);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="query"/> with the specified <paramref name="parameters"/>
        /// </summary>
        /// <param name="query">
        /// The SQL query to execute
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="parameters">
        /// The parameters. Can be null.
        /// </param>
        /// <returns>
        /// The return value of <paramref name="query"/>
        /// </returns>
        public object ExecuteScalarFormat(string query, DbTransaction transaction, params DbParameter[] parameters)
        {
            try
            {
                IDatabaseInternal databaseInternal = new DatabaseTransactional(transaction);
                using (var command = this.GetSqlStringCommandFormat(query, parameters))
                {
                    databaseInternal.SetupCommand(command);
                    return databaseInternal.ExecuteScalar(command);
                }
            }
            catch (Exception e)
            {
                _log.Error("While ExecuteNonQuery", e);
                throw;
            }
        }

        /// <summary>
        /// Returns a <see cref="DbCommand"/> with <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand GetSqlStringCommand(string query)
        {
            return this.CreateCommand(CommandType.Text, query);
        }

        /// <summary>
        /// Returns a <see cref="DbCommand"/> with <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="parameters">
        /// The parameters
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand GetSqlStringCommand(string query, IList<DbParameter> parameters)
        {
            DbCommand cmd = null;
            try
            {
                cmd = this.CreateCommand(CommandType.Text, query);
                if (parameters != null)
                {
                    foreach (DbParameter dbParameter in parameters)
                    {
                        cmd.Parameters.Add(dbParameter);
                    }
                }
            }
            catch (Exception e)
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                _log.Error("While ExecuteReader", e);

                throw;
            }

            return cmd;
        }

        /// <summary>
        /// Returns a <see cref="DbCommand"/> with <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="queryFormat">
        /// The query format.
        /// </param>
        /// <param name="parameters">
        /// The parameters
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand GetSqlStringCommandFormat(string queryFormat, params DbParameter[] parameters)
        {
            var query = this.BuildQuery(queryFormat, parameters);

            return this.GetSqlStringCommand(query, parameters);
        }

        /// <summary>
        /// Returns a <see cref="DbCommand"/> with <see cref="CommandType.Text"/>
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="parameters">
        /// The parameters
        /// </param>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        public DbCommand GetSqlStringCommandParam(string query, params DbParameter[] parameters)
        {
            return this.GetSqlStringCommand(query, parameters);
        }

        /// <summary>
        /// Normalizes the schema of the specified <paramref name="query"/>.
        ///     Currently it removes the <c>dbo.</c> from the query string if the database is not a <c>SQL Server.</c>
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string NormalizeQuerySchema(string query)
        {
            return this._schemaNormalizer(query);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns the method for normalizing a SQL query.
        /// </summary>
        /// <returns>
        ///     The <see cref="Func{String, String}" /> method that normalizes a SQL Query .
        /// </returns>
        private Func<string, string> GetQueryNormalizer()
        {
            switch (this._providerName)
            {
                case MappingStoreDefaultConstants.SqlServerProvider:
                    return s => s;
                default:
                    return s => _dboRemover.Replace(s, string.Empty);
            }
        }

        /// <summary>
        ///     Retrieve the parameter marker format and set <see cref="_parameterMarkerFormat" />
        /// </summary>
        private void RetrieveMarkerFormat()
        {
            // put a value in this._parameterMarkerFormat
            // 1. see if we have it cached in this instance.
            if (this._parameterMarkerFormat == null)
            {
                string providerName = this._connectionStringSettings.ProviderName;

                // 2. if not, then see if we have it cached in the static provider <-> parameter mark format dictionary
                if (!_parameterMarkersPerProvider.TryGetValue(providerName, out this._parameterMarkerFormat))
                {
                    // 3. if not, then if we have it configured 
                    DatabaseSetting databaseSetting = _databaseSettingCollection[providerName];
                    if (databaseSetting != null && !string.IsNullOrWhiteSpace(databaseSetting.ParameterMarkerFormat))
                    {
                        this._parameterMarkerFormat = databaseSetting.ParameterMarkerFormat;

                        _log.DebugFormat("Got from configuration the parameter marker format '{0}' for provider '{1}'", this._parameterMarkerFormat, providerName);
                    }
                    else
                    {
                        // 4. if not, then try to get it from GetSchema(). SqlClient has a bug here.
                        using (DbConnection connection = this.CreateConnection())
                        {
                            connection.Open();
                            DataTable dataTable = connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
                            this._parameterMarkerFormat = dataTable.Rows[0][DbMetaDataColumnNames.ParameterMarkerFormat].ToString();
                            _log.DebugFormat("Got from GetSchema the parameter marker format '{0}' for provider '{1}'", this._parameterMarkerFormat, providerName);
                        }
                    }

                    _parameterMarkersPerProvider.TryAdd(providerName, this._parameterMarkerFormat);
                }
            }
        }

        /// <summary>
        /// Setup provider specific configuration.
        /// </summary>
        /// <param name="cmd">
        /// The <see cref="DbCommand"/> to setup.
        /// </param>
        private void SetupProviderSpecific(DbCommand cmd)
        {
            // Oracle ODP.NET driver by default expects parameters in a specific order
            switch (this._connectionStringSettings.ProviderName)
            {
                case MappingStoreDefaultConstants.OracleProviderOdp:

                    // NOTE this is cached. The slow reflection part occurs only once.
                    var dynamicCmd = (dynamic)cmd;
                    dynamicCmd.BindByName = true;

                    break;
            }
        }

        #endregion

        /// <summary>
        /// The database non transactional.
        /// </summary>
        private class DatabaseNonTransactional : IDatabaseInternal
        {
            #region Fields

            /// <summary>
            ///     The mapping store connection string settings.
            /// </summary>
            private readonly ConnectionStringSettings _connectionStringSettings;

            /// <summary>
            ///     The factory to create connections.
            /// </summary>
            private readonly DbProviderFactory _factory;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="DatabaseNonTransactional"/> class.
            /// </summary>
            /// <param name="connectionStringSettings">
            /// The connection string settings.
            /// </param>
            /// <param name="factory">
            /// The factory.
            /// </param>
            public DatabaseNonTransactional(ConnectionStringSettings connectionStringSettings, DbProviderFactory factory)
            {
                this._connectionStringSettings = connectionStringSettings;
                this._factory = factory;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The execute non query.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int ExecuteNonQuery(DbCommand command)
            {
                using (DbConnection connection = this.GetConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    return command.ExecuteNonQuery();
                }
            }

            /// <summary>
            /// The execute reader.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="IDataReader"/>.
            /// </returns>
            public IDataReader ExecuteReader(IDbCommand command)
            {
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }

            /// <summary>
            /// The execute scalar.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="object"/>.
            /// </returns>
            public object ExecuteScalar(DbCommand command)
            {
                using (DbConnection connection = this.GetConnection())
                {
                    connection.Open();
                    command.Connection = connection;
                    return command.ExecuteScalar();
                }
            }

            /// <summary>
            /// The get connection.
            /// </summary>
            /// <returns>
            /// The <see cref="DbConnection"/>.
            /// </returns>
            public DbConnection GetConnection()
            {
                DbConnection connection = null;
                try
                {
                    connection = this._factory.CreateConnection();
                    if (connection != null)
                    {
                        connection.ConnectionString = this._connectionStringSettings.ConnectionString;
                    }
                }
                catch (Exception e)
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }

                    _log.Error("While CreateConnection", e);

                    throw;
                }

                return connection;
            }

            /// <summary>
            /// The open connection.
            /// </summary>
            /// <param name="connection">
            /// The connection.
            /// </param>
            public void OpenConnection(IDbConnection connection)
            {
                connection.Open();
            }

            /// <summary>
            /// The setup command.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            public void SetupCommand(IDbCommand command)
            {
            }

            #endregion
        }

        /// <summary>
        /// The database transactional.
        /// </summary>
        private class DatabaseTransactional : IDatabaseInternal
        {
            #region Fields

            /// <summary>
            /// The _transaction.
            /// </summary>
            private readonly DbTransaction _transaction;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="DatabaseTransactional"/> class.
            /// </summary>
            /// <param name="transaction">
            /// The transaction.
            /// </param>
            public DatabaseTransactional(DbTransaction transaction)
            {
                this._transaction = transaction;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The execute non query.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int ExecuteNonQuery(DbCommand command)
            {
                this.SetupCommand(command);
                return command.ExecuteNonQuery();
            }

            /// <summary>
            /// The execute reader.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="IDataReader"/>.
            /// </returns>
            public IDataReader ExecuteReader(IDbCommand command)
            {
                return command.ExecuteReader(CommandBehavior.Default);
            }

            /// <summary>
            /// The execute scalar.
            /// </summary>
            /// <param name="command">
            /// The command.
            /// </param>
            /// <returns>
            /// The <see cref="object"/>.
            /// </returns>
            public object ExecuteScalar(DbCommand command)
            {
                this.SetupCommand(command);
                return command.ExecuteScalar();
            }

            /// <summary>
            /// The get connection.
            /// </summary>
            /// <returns>
            /// The <see cref="DbConnection"/>.
            /// </returns>
            public DbConnection GetConnection()
            {
                return this._transaction.Connection;
            }

            /// <summary>
            /// The open connection.
            /// </summary>
            /// <param name="connection">
            /// The connection.
            /// </param>
            public void OpenConnection(IDbConnection connection)
            {
            }

            /// <summary>
            /// Setup the command.
            /// </summary>
            /// <param name="command">The command.</param>
            public void SetupCommand(IDbCommand command)
            {
                command.Connection = this._transaction.Connection;
                command.Transaction = this._transaction;
            }

            #endregion
        }
    }
}