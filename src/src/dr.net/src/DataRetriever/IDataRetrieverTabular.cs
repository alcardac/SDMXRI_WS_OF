// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataRetrieverTabular.cs" company="Eurostat">
//   Date Created : 2012-01-24
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An interface for retrieving data and storing it in a tabular format using a <see cref="ITabularWriter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever
{
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Estat.Sri.TabularWriters.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    /// An interface for retrieving data and storing it in a tabular format using a <see cref="ITabularWriter"/>
    /// </summary>
    public interface IDataRetrieverTabular
    {
        #region Public Methods

        /// <summary>
        /// This method executes an SQL query on the dissemination database and writes the output to <paramref name="writer"/>
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="disseminationDbSql"/>
        ///   is null
        ///   -or-
        ///   <paramref name="writer"/>
        ///   is null
        ///   -or-
        ///   <paramref name="query"/>
        ///   is null
        /// </exception>
        /// <exception cref="DataRetrieverException">
        /// <see cref="ErrorTypes"/>
        /// </exception>
        /// <param name="query">
        /// The query bean for which data will be requested. 
        /// </param>
        /// <param name="disseminationDbSql">
        /// The SQL statement to be executed 
        /// </param>
        /// <param name="limit">
        /// Set to a positive integer to limit the number of observations returned, else set to 0 to return all observations 
        /// </param>
        /// <param name="writer">
        /// The <see cref="ITabularWriter"/> (e.g. CSV, SQLite e.t.c.) writer 
        /// </param>
        /// <example>
        /// An example using this method in C# 
        ///  <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverManySteps.cs" lang="cs" />
        /// </example>
        void ExecuteSqlQuery(IDataQuery query, string disseminationDbSql, int limit, ITabularWriter writer);

        void ExecuteSqlQuery(IComplexDataQuery query, string disseminationDbSql, int limit, ITabularWriter writer);

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="query"/>
        ///   is null
        /// </exception>
        /// <exception cref="DataRetrieverException">
        /// <see cref="ErrorTypes"/>
        /// </exception>
        /// <param name="query">
        /// The <see cref="Estat.Sdmx.Model.Query.QueryBean"/> modeling an SDMX-ML Query 
        /// </param>
        /// <returns>
        /// The generated sql query. 
        /// </returns>
        /// <example>
        /// An example using this method in C#
        /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverManySteps.cs" lang="cs" />
        /// </example>
        string GenerateSqlQuery(IDataQuery query);

        string GenerateSqlQuery(IComplexDataQuery query);

        /// <summary>
        /// Retrieve data from a DDB and write it to the specified <paramref name="writer"/> This is the main public method of the DataRetriever class. It is called with a populated QueryBean (containing essentially an SDMX-ML Query) and a database Connection to a "Mapping Store" database. This method is responsible for: 
        /// <list type="bullet">
        /// <item>
        /// Retrieving the <see cref="MappingSetEntity"/> (the class containing the performed mappings), 
        /// according to the provided Dataflow ID, from the "Mapping Store". Mapping Sets are defined on a Dataflow basis. 
        /// Thus, this method checks the input QueryBean for the Dataflow that data are requested and fetches the appropriate
        /// <see cref="MappingSetEntity"/>. If no <see cref="MappingSetEntity"/> exists, an exception (<see cref="DataRetrieverException"/>) is thrown.                                                                                                                                                                                                                                                                                                                           ) is thrown.
        /// </item>
        /// <item>
        /// Calling the method generating the appropriate SQL for the dissemination database.
        /// </item>
        /// <item>
        /// Calling the method that executes the generated SQL and uses the <paramref name="writer"/>
        /// to write the output.
        /// </item>
        /// </list>
        /// <note type="note">
        /// The "Data Retriever" expects exactly one Dataflow clause under the DataWhere clause, exactly one
        /// DataFlowBean within the DataWhereBean (which in turn resides inside the incoming QueryBean).
        /// </note>
        /// </summary>
        /// <exception cref="DataRetrieverException">
        /// See the
        ///   <see cref="ErrorTypes"/>
        ///   for more details
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="query"/>
        ///   is null
        ///   -or-
        ///   <paramref name="writer"/>
        ///   is null
        /// </exception>
        /// <param name="query">
        /// The query bean for which data will be requested 
        /// </param>
        /// <param name="writer">
        /// The <see cref="ITabularWriter"/> (e.g. CSV, SQLite e.t.c.) writer 
        /// </param>
        /// <param name="showOriginal">
        /// A value indicating whether to show original (DDB) values 
        /// </param>
        /// <example>
        /// An example using this method in C# (Without original columns) 
        /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverTabular.cs" lang="cs" />
        /// An example using this method in C# (With original columns) 
        /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverTabularWithOriginal.cs" lang="cs" />
        /// </example>
        void RetrieveData(IDataQuery query, ITabularWriter writer, bool showOriginal);

        #endregion
    }
}