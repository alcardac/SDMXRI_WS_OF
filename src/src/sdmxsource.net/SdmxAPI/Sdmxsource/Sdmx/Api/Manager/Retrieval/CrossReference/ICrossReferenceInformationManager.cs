// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReferenceInformationManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.CrossReference
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// Provides information on which structures cross reference a given structure
    /// </summary>
    public interface ICrossReferenceInformationManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a tree structure representing the identifiable and all the structures that cross reference it, and the structures that reference them, and so on.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintanable object
        /// </param>
        /// <returns>
        /// The tree structure
        /// </returns>
        ICrossReferencingTree GetCrossReferenceTree(IMaintainableObject maintainableObject);

        #endregion
    }
}
