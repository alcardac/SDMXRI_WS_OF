// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthMutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The MutableRetrievalManagerFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Factory
{
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    ///     The MutableRetrievalManagerFactory interface.
    /// </summary>
    public interface IAuthMutableRetrievalManagerFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="IAuthSdmxMutableObjectRetrievalManager"/> created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager.
        /// </param>
        /// <returns>
        /// The <see cref="IAuthSdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        IAuthSdmxMutableObjectRetrievalManager GetRetrievalManager<T>(T settings, ISdmxMutableObjectRetrievalManager retrievalManager);

        #endregion
    }
}