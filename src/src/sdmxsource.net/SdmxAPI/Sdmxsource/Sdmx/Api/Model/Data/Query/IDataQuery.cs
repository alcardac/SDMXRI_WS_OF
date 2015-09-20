// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQuery.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data.Query
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    #endregion

    /// <summary>
    ///     A DataQuery is a Schema independent representation of a DataWhere/REST query
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IDataQuery" />
    ///     <code source="..\ReUsingExamples\DataQuery\ReUsingDataQueryParsingManager.cs" lang="cs" />
    /// </example>
    public interface IDataQuery : IBaseDataQuery
    {
        #region Public Properties

        /// <summary>
        ///     Gets last updated date is a filter on the data meaning 'only return data that was updated after this time'
        ///     <p />
        ///     If this attribute is used, the returned message should
        ///     only include the latest version of what has changed in the database since that point in time (updates and revisions).
        ///     <p />
        ///     This should include:
        ///     <ul>
        ///         <li>Observations that have been added since the last time the query was performed (INSERT)</li>
        ///         <li>Observations that have been revised since the last time the query was performed (UPDATE)</li>
        ///         <li>Observations that have been revised since the last time the query was performed (UPDATE)</li>
        ///         <li>Observations that have been deleted since the last time the query was performed (DELETE)</li>
        ///     </ul>
        ///     If no offset is specified, default to local
        ///     <p />
        ///     Gets null if unspecified
        /// </summary>
        /// <value> </value>
        ISdmxDate LastUpdatedDate { get; }

        /// <summary>
        ///     Gets a copy of all the selection groups
        /// </summary>
        /// <value> </value>
        IList<IDataQuerySelectionGroup> SelectionGroups { get; }

        #endregion
    }
}