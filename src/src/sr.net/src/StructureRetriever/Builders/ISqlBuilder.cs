// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISqlBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sql builder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using Estat.Nsi.StructureRetriever.Model;

    /// <summary>
    /// The sql builder interface.
    /// </summary>
    internal interface ISqlBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generate the SQL for executing on the DDB
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval information 
        /// </param>
        /// <returns>
        /// The generated sql. 
        /// </returns>
        string GenerateSql(StructureRetrievalInfo info);

        #endregion
    }
}