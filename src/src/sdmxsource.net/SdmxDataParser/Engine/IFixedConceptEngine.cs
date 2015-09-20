// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFixedConceptEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    #endregion

    /// <summary>
    /// The fixed concept engine interface.
    /// </summary>
    /// <remarks>No documentation exists in Java master version.</remarks>
    public interface IFixedConceptEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the fixed concepts.
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        /// <param name="includeObs">
        /// The include observation
        /// </param>
        /// <param name="includeAttributes">
        /// The include attributes
        /// </param>
        /// <returns>The list of key values</returns>
        IList<IKeyValue> GetFixedConcepts(IDataReaderEngine dre, bool includeObs, bool includeAttributes);

        #endregion
    }
}
