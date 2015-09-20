// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISqlBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sql builder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Builders
{
    using Estat.Nsi.DataRetriever.Model;

    /// <summary>
    /// The sql builder interface.
    /// </summary>
    internal interface ISqlBuilder
    {
        #region Public Methods

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <param name="info">
        /// The current state of the data retrieval which containts the current query and mapping set 
        /// </param>
        void GenerateSql(DataRetrievalInfo info);

        #endregion
    }
}