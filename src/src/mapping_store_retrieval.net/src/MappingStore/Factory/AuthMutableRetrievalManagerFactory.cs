// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthMutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The authorization mutable retrieval manager factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    ///     The authorization mutable retrieval manager factory.
    /// </summary>
    public class AuthMutableRetrievalManagerFactory : IAuthMutableRetrievalManagerFactory
    {
        #region Static Fields

        /// <summary>
        ///     The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, ISdmxMutableObjectRetrievalManager, IAuthSdmxMutableObjectRetrievalManager>> _factoryMethods =
            new Dictionary<Type, Func<object, ISdmxMutableObjectRetrievalManager, IAuthSdmxMutableObjectRetrievalManager>>
                {
                    {
                        typeof(Database),
                        (settings, retrieval) =>
                        new AuthMappingStoreRetrievalManager(settings as Database, retrieval)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        (settings, detail) =>
                        new AuthMappingStoreRetrievalManager(
                            settings as ConnectionStringSettings, detail)
                    }, 
                    {
                        typeof(IAuthSdmxMutableObjectRetrievalManager), 
                        (settings, detail) =>
                        new AuthCachedRetrievalManager(
                            null, settings as IAuthSdmxMutableObjectRetrievalManager)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The user provided factory method.
        /// </summary>
        private readonly Func<object, ISdmxMutableObjectRetrievalManager, IAuthSdmxMutableObjectRetrievalManager> _factoryMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        /// <param name="factoryMethod">
        /// The factory method.
        /// </param>
        public AuthMutableRetrievalManagerFactory(Func<object, ISdmxMutableObjectRetrievalManager, IAuthSdmxMutableObjectRetrievalManager> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        public AuthMutableRetrievalManagerFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="IAuthSdmxMutableObjectRetrievalManager"/> created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager.
        /// </param>
        /// <returns>
        /// The <see cref="IAuthSdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        public IAuthSdmxMutableObjectRetrievalManager GetRetrievalManager<T>(T settings, ISdmxMutableObjectRetrievalManager retrievalManager)
        {
            IAuthSdmxMutableObjectRetrievalManager manager = null;
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings, retrievalManager);
            }

            Func<object, ISdmxMutableObjectRetrievalManager, IAuthSdmxMutableObjectRetrievalManager> method;
            if (_factoryMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings, retrievalManager);
            }

            return manager;
        }

        #endregion
    }
}