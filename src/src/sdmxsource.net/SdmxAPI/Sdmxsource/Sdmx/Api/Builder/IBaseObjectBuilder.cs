// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseObjectBuilder.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    /// Base interface for specific base object builder classes, to be built from corresponding plain objects.
    /// </summary>
    /// <typeparam name="TOutput">
    /// structure base object (to)
    /// </typeparam>
    /// <typeparam name="TInput">
    /// corresponding object (from)
    /// </typeparam>
    public interface IBaseObjectBuilder<out TOutput, in TInput>
        where TOutput : IObjectBase where TInput : ISdmxObject
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a BaseObject from a SdmxObject and a corresponding SdmxObjectRetrievalManager (optional) to resolve any cross referenced artifacts
        /// </summary>
        /// <param name="buildFrom">
        /// - the SdmxObject to build from
        /// </param>
        /// <param name="objectRetrievalManager">
        /// - to resolve any cross referenced artifacts declared by V - the buildFrom argument
        /// </param>
        /// <param name="exitingObjects">
        /// - can be null if none already exit
        /// </param>
        /// <returns>
        /// K - the Sdmx base object representation of the SdmxObject argument
        /// </returns>
        TOutput Build(TInput buildFrom, ISdmxObjectRetrievalManager objectRetrievalManager, IObjectsBase exitingObjects);

        #endregion
    }
}