// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseObjectsBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    #endregion

    /// <summary>
    ///     Builds a BaseObjects container from the contents of the SdmxObjects container.
    /// </summary>
    public interface IBaseObjectsBuilder : IBuilder<IObjectsBase, ISdmxObjects>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds base objects from the input objects, obtains any additional required objects from the retrieval manager
        /// </summary>
        /// <param name="buildFrom">
        /// The source SDMX Object
        /// </param>
        /// <param name="exitingObjects">
        /// if any base objects exist then they should be passed in here in order to reuse - new objects will be added to this container
        /// </param>
        /// <param name="objectRetrievalManager">The Retrieval Manager
        /// </param>
        /// <returns>
        /// The <see cref="IObjectsBase"/> .
        /// </returns>
        /// <exception cref="SdmxException">
        /// Could not build object
        /// </exception>
        IObjectsBase Build(ISdmxObjects buildFrom, IObjectsBase exitingObjects, ISdmxObjectRetrievalManager objectRetrievalManager);

        #endregion
    }
}