// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthCrossMutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-06-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mutable retrieval manager factory.
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
    ///     The mutable retrieval manager factory.
    /// </summary>
    public class AuthCrossMutableRetrievalManagerFactory : IAuthCrossRetrievalManagerFactory
    {
        #region Static Fields

        /// <summary>
        ///     The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, IAuthSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager>> _factoryMethods =
            new Dictionary<Type, Func<object, IAuthSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager>>
                {
                    {
                        typeof(Database), 
                        (settings, retrievalManager) =>
                        new AuthCrossReferenceRetrievalManager(
                            retrievalManager, settings as Database)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        (settings, retrievalManager) =>
                        new AuthCrossReferenceRetrievalManager(
                            retrievalManager, settings as ConnectionStringSettings)
                    }, 
                };

        /// <summary>
        ///     The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager>> _factoryAdvancedMethods =
            new Dictionary<Type, Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager>>
                {
                    {
                        typeof(Database), 
                        (settings, retrievalManager) =>
                        new AuthCrossReferenceRetrievalManager(
                            retrievalManager, settings as Database)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        (settings, retrievalManager) =>
                        new AuthCrossReferenceRetrievalManager(
                            retrievalManager, settings as ConnectionStringSettings)
                    }, 
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The user provided factory method.
        /// </summary>
        private readonly Func<object, IAuthSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> _factoryMethod;

        /// <summary>
        ///     The user provided factory method.
        /// </summary>
        private readonly Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> _factoryMethodAdvanced;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossMutableRetrievalManagerFactory" /> class.
        /// </summary>
        /// <param name="factoryMethod">The user provided factory method.</param>
        /// <param name="factoryMethodAdvanced">The factory method advanced.</param>
        public AuthCrossMutableRetrievalManagerFactory(Func<object, IAuthSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> factoryMethod, Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> factoryMethodAdvanced)
        {
            this._factoryMethod = factoryMethod;
            this._factoryMethodAdvanced = factoryMethodAdvanced;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        public AuthCrossMutableRetrievalManagerFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="ISdmxMutableObjectRetrievalManager"/> created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        public IAuthCrossReferenceMutableRetrievalManager GetCrossRetrievalManager<T>(T settings, IAuthSdmxMutableObjectRetrievalManager retrievalManager)
        {
            IAuthCrossReferenceMutableRetrievalManager manager = null;
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings, retrievalManager);
            }

            Func<object, IAuthSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> method;
            if (_factoryMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings, retrievalManager);
            }

            return manager;
        }

        /// <summary>
        /// Returns an instance of <see cref="ICrossReferenceMutableRetrievalManager"/> created using the specified
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
        /// The <see cref="ICrossReferenceMutableRetrievalManager"/>.
        /// </returns>
        public IAuthCrossReferenceMutableRetrievalManager GetCrossRetrievalManager<T>(T settings, IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager)
        {
            IAuthCrossReferenceMutableRetrievalManager manager = null;
            if (this._factoryMethodAdvanced != null)
            {
                manager = this._factoryMethodAdvanced(settings, retrievalManager);
            }

            Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager, IAuthCrossReferenceMutableRetrievalManager> method;
            if (_factoryAdvancedMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings, retrievalManager);
            }

            return manager;
        }

        #endregion
    }
}