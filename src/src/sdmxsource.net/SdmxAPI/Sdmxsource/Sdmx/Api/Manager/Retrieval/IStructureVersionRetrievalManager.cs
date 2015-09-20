// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureVersionRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    /// Provides methods for retrieving beans where version information is the key criteria
    /// </summary>
    public interface IStructureVersionRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the latest version of the maintainable for the given maintainable input
        /// </summary>
        /// <param name="maintainableBean">
        /// The maintainable
        /// </param>
        /// <returns>
        /// The latest version of the maintainable
        /// </returns>
        IMaintainableObject GetLatest(IMaintainableObject maintainableBean);

        #endregion
    }
}
