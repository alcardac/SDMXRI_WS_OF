// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The constraint retrieval manager is used to retrieve constraints for the purpose of building a data query.
    /// </summary>
    public interface IConstraintRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a IContentConstraintObject defining the data that is allowed for the IConstrainableObject.
        ///     Will merge constraints attached to the child constrainableObject structures of the input constrainableObject.
        /// </summary>
        /// <param name="constrainableObject">Constrainable Object
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/> .
        /// </returns>
        IContentConstraintObject GetConstraintDefiningAllowedData(IConstrainableObject constrainableObject);

        /// <summary>
        /// Gets a IContentConstraintObject defining the data present for the IConstrainableObject.
        ///     Gets null if no constraint exists.
        /// </summary>
        /// <param name="constrainableObject">Constrainable Object
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/> .
        /// </returns>
        IContentConstraintObject GetContentConstraintDefiningDataPresent(IConstrainableObject constrainableObject);

        #endregion
    }
}