// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceRetrievalManager.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     Gives the ability to retrieve the structure, or the service Uri for the application, allowing Objects to be built as a stub
    /// </summary>
    public interface IServiceRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a stub @object from the given maintainable artefact
        /// </summary>
        /// <param name="maintainableObject"> Maintainable Object
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        IMaintainableObject CreateStub(IMaintainableObject maintainableObject);

        /// <summary>
        /// Gets the Uri to obtain structures from, this will be either a service Uri
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainableObject.
        /// </param>
        /// <returns>
        /// The <see cref="ArtefactUrl"/> .
        /// </returns>
        ArtefactUrl GetStructureOrServiceUrl(IMaintainableObject maintainableObject);

        #endregion
    }
}