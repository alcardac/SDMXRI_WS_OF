// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthAdvancedMutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-04-16
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

    /// <summary>
    /// The mutable retrieval manager factory.
    /// </summary>
    public class AuthAdvancedMutableRetrievalManagerFactory : IAuthAdvancedMutableRetrievalManagerFactory
    {
        #region Static Fields

        /// <summary>
        /// The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager>> _factoryMethods =
            new Dictionary<Type, Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager>>
                {
                    {
                        typeof(Database), 
                        settings => new AuthAdvancedStructureRetriever(settings as Database)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        settings =>
                        new AuthAdvancedStructureRetriever(settings as ConnectionStringSettings)
                    }, 
                    {
                        typeof(IAuthAdvancedSdmxMutableObjectRetrievalManager), 
                        settings =>
                        new AuthCachedAdvancedStructureRetriever(settings as IAuthAdvancedSdmxMutableObjectRetrievalManager)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        /// The user provided factory method.
        /// </summary>
        private readonly Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager> _factoryMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedMutableRetrievalManagerFactory"/> class.
        /// </summary>
        /// <param name="factoryMethod">
        /// The user provided factory method. 
        /// </param>
        public AuthAdvancedMutableRetrievalManagerFactory(Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthAdvancedMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        public AuthAdvancedMutableRetrievalManagerFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="IAuthAdvancedSdmxMutableObjectRetrievalManager"/> created using the specified <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// The <see cref="IAuthAdvancedSdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        public IAuthAdvancedSdmxMutableObjectRetrievalManager GetRetrievalManager<T>(T settings)
        {
            IAuthAdvancedSdmxMutableObjectRetrievalManager manager = null;
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings);
            }

            Func<object, IAuthAdvancedSdmxMutableObjectRetrievalManager> method;
            if (_factoryMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings);
            }

            return manager;
        }

        #endregion
    }
}